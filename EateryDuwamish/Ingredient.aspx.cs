using BusinessFacade;
using Common.Data;
using Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Controls;

namespace EateryDuwamish
{
    public partial class Ingredient : System.Web.UI.Page
    {
        private int recipeID
        {
            get
            {
                if (ViewState["recipeID"] == null) return 0;
                return Convert.ToInt32(ViewState["recipeID"]);
            }
            set { ViewState["recipeID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["recipeID"] == null || Request.QueryString["recipeID"] == String.Empty)
                    Response.Redirect("~/Recipe.aspx");

                recipeID = Convert.ToInt32(Rijndael.Decrypt(Request.QueryString["recipeID"]));

                ShowNotificationIfExists();
                LoadIngredientTable();
            }
        }
        #region FORM MANAGEMENT
        private void FillForm(IngredientData ingredient)
        {
            hdfIngredientId.Value = ingredient.IngredientID.ToString();
            txtIngredientName.Text = ingredient.IngredientName;
            txtIngredientQuantity.Text = ingredient.IngredientQuantity.ToString();
            txtIngredientUnit.Text = ingredient.IngredientUnit;
        }
        private void ResetForm()
        {
            hdfIngredientId.Value = String.Empty;
            txtIngredientName.Text = String.Empty;
            txtIngredientQuantity.Text = String.Empty;
            txtIngredientUnit.Text = String.Empty;
        }
        private IngredientData GetFormData()
        {
            IngredientData ingredient = new IngredientData();
            ingredient.IngredientID = String.IsNullOrEmpty(hdfIngredientId.Value) ? 0 : Convert.ToInt32(hdfIngredientId.Value);
            ingredient.RecipeID = recipeID;
            ingredient.IngredientName = txtIngredientName.Text;
            ingredient.IngredientQuantity = Convert.ToInt32(txtIngredientQuantity.Text);
            ingredient.IngredientUnit = txtIngredientUnit.Text;
            return ingredient;
        }
        #endregion

        #region DATA TABLE MANAGEMENT
        private void LoadIngredientTable()
        {
            try
            {
                List<IngredientData> ListIngredient = new IngredientSystem().GetIngredientListByRecipeID(recipeID);
                rptIngredient.DataSource = ListIngredient;
                rptIngredient.DataBind();
            }
            catch (Exception ex)
            {
                notifIngredient.Show($"ERROR LOAD TABLE: {ex.Message}", NotificationType.Danger);
            }
        }
        protected void rptIngredient_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                IngredientData ingredient = (IngredientData)e.Item.DataItem;
                Literal litIngredientName = (Literal)e.Item.FindControl("litIngredientName");
                Literal litIngredientQuantity = (Literal)e.Item.FindControl("litIngredientQuantity");
                Literal litIngredientUnit = (Literal)e.Item.FindControl("litIngredientUnit");
                LinkButton lbIngredientEdit = (LinkButton)e.Item.FindControl("lbIngredientEdit");

                litIngredientName.Text = ingredient.IngredientName;
                litIngredientQuantity.Text = ingredient.IngredientQuantity.ToString();
                litIngredientUnit.Text = ingredient.IngredientUnit;

                lbIngredientEdit.CommandArgument = ingredient.IngredientID.ToString();

                CheckBox chkChoose = (CheckBox)e.Item.FindControl("chkChoose");
                chkChoose.Attributes.Add("data-value", ingredient.IngredientID.ToString());
            }
        }
        protected void rptIngredient_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "EDIT")
            {
                Literal litIngredientName = (Literal)e.Item.FindControl("litIngredientName");
                Literal litIngredientQuantity = (Literal)e.Item.FindControl("litIngredientQuantity");
                Literal litIngredientUnit = (Literal)e.Item.FindControl("litIngredientUnit");

                int ingredientID = Convert.ToInt32(e.CommandArgument.ToString());
                IngredientData ingredient = new IngredientSystem().GetIngredientByID(ingredientID);
                FillForm(new IngredientData
                {
                    IngredientID = ingredient.IngredientID,
                    IngredientName = ingredient.IngredientName,
                    IngredientQuantity = ingredient.IngredientQuantity,
                    IngredientUnit = ingredient.IngredientUnit
                });
                litFormType.Text = $"UBAH: {litIngredientName.Text}";
                pnlFormIngredient.Visible = true;
                txtIngredientName.Focus();
            }
        }
        #endregion

        #region BUTTON EVENT MANAGEMENT
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                IngredientData ingredient = GetFormData();
                int rowAffected = new IngredientSystem().InsertUpdateIngredient(ingredient);
                if (rowAffected <= 0)
                    throw new Exception("No Data Recorded");
                Session["save-success"] = 1;
                Response.Redirect("Ingredient.aspx?recipeID=" + HttpUtility.UrlEncode(Rijndael.Encrypt(recipeID.ToString())));
            }
            catch (Exception ex)
            {
                notifIngredient.Show($"ERROR SAVE DATA: {ex.Message}", NotificationType.Danger);
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ResetForm();
            litFormType.Text = "TAMBAH";
            pnlFormIngredient.Visible = true;
            txtIngredientName.Focus();
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string strDeletedIDs = hdfDeletedIngredients.Value;
                IEnumerable<int> deletedIDs = strDeletedIDs.Split(',').Select(Int32.Parse);
                int rowAffected = new IngredientSystem().DeleteIngredients(deletedIDs);
                if (rowAffected <= 0)
                    throw new Exception("No Data Deleted");
                Session["delete-success"] = 1;
                Response.Redirect("Ingredient.aspx?recipeID=" + HttpUtility.UrlEncode(Rijndael.Encrypt(recipeID.ToString())));
            }
            catch (Exception ex)
            {
                notifIngredient.Show($"ERROR DELETE DATA: {ex.Message}", NotificationType.Danger);
            }
        }
        #endregion

        #region NOTIFICATION MANAGEMENT
        private void ShowNotificationIfExists()
        {
            if (Session["save-success"] != null)
            {
                notifIngredient.Show("Data sukses disimpan", NotificationType.Success);
                Session.Remove("save-success");
            }
            if (Session["delete-success"] != null)
            {
                notifIngredient.Show("Data sukses dihapus", NotificationType.Success);
                Session.Remove("delete-success");
            }
        }
        #endregion
    }
}