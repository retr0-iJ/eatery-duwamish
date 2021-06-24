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
    public partial class Recipe : System.Web.UI.Page
    {
        private int dishID
        {
            get
            {
                if (ViewState["dishID"] == null) return 0;
                return Convert.ToInt32(ViewState["dishID"]);
            }
            set { ViewState["dishID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["dishID"] == null || Request.QueryString["dishID"] == String.Empty)
                    Response.Redirect("~/Dish.aspx");

                dishID = Convert.ToInt32(Rijndael.Decrypt(Request.QueryString["dishID"]));

                ShowNotificationIfExists();
                LoadRecipeTable();
            }
        }
        #region FORM MANAGEMENT
        private void ResetForm()
        {
            hdfRecipeId.Value = String.Empty;
            txtRecipeName.Text = String.Empty;
        }
        private RecipeData GetFormData()
        {
            RecipeData recipe = new RecipeData();
            recipe.RecipeID = String.IsNullOrEmpty(hdfRecipeId.Value) ? 0 : Convert.ToInt32(hdfRecipeId.Value);
            recipe.DishID = dishID;
            recipe.RecipeName = txtRecipeName.Text;
            recipe.RecipeDescription = String.Empty;
            return recipe;
        }
        #endregion

        #region DATA TABLE MANAGEMENT
        private void LoadRecipeTable()
        {
            try
            {
                List<RecipeData> ListRecipe = new RecipeSystem().GetRecipeListByDishID(dishID);
                rptRecipe.DataSource = ListRecipe;
                rptRecipe.DataBind();
            }
            catch (Exception ex)
            {
                notifRecipe.Show($"ERROR LOAD TABLE: {ex.Message}", NotificationType.Danger);
            }
        }
        protected void rptRecipe_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                RecipeData recipe = (RecipeData)e.Item.DataItem;
                Literal litRecipeName = (Literal)e.Item.FindControl("litRecipeName");
                LinkButton lbRecipeDetail = (LinkButton)e.Item.FindControl("lbRecipeDetail");

                litRecipeName.Text = recipe.RecipeName;
                lbRecipeDetail.CommandArgument = recipe.RecipeID.ToString();

                CheckBox chkChoose = (CheckBox)e.Item.FindControl("chkChoose");
                chkChoose.Attributes.Add("data-value", recipe.RecipeID.ToString());
            }
        }
        protected void rptRecipe_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DETAIL")
            {
                Response.Redirect("~/Ingredient.aspx?recipeID=" + HttpUtility.UrlEncode(Rijndael.Encrypt(e.CommandArgument.ToString())));
            }
        }
        #endregion

        #region BUTTON EVENT MANAGEMENT
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                RecipeData recipe = GetFormData();
                int rowAffected = new RecipeSystem().InsertUpdateRecipe(recipe);
                if (rowAffected <= 0)
                    throw new Exception("No Data Recorded");
                Session["save-success"] = 1;
                Response.Redirect("Recipe.aspx?dishID=" + HttpUtility.UrlEncode(Rijndael.Encrypt(dishID.ToString())));
            }
            catch (Exception ex)
            {
                notifRecipe.Show($"ERROR SAVE DATA: {ex.Message}", NotificationType.Danger);
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ResetForm();
            litFormType.Text = "TAMBAH";
            pnlFormRecipe.Visible = true;
            txtRecipeName.Focus();
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string strDeletedIDs = hdfDeletedRecipes.Value;
                IEnumerable<int> deletedIDs = strDeletedIDs.Split(',').Select(Int32.Parse);
                int rowAffected = new RecipeSystem().DeleteRecipes(deletedIDs);
                if (rowAffected <= 0)
                    throw new Exception("No Data Deleted");
                Session["delete-success"] = 1;
                Response.Redirect("Recipe.aspx?dishID=" + HttpUtility.UrlEncode(Rijndael.Encrypt(dishID.ToString())));
            }
            catch (Exception ex)
            {
                notifRecipe.Show($"ERROR DELETE DATA: {ex.Message}", NotificationType.Danger);
            }
        }
        #endregion

        #region NOTIFICATION MANAGEMENT
        private void ShowNotificationIfExists()
        {
            if (Session["save-success"] != null)
            {
                notifRecipe.Show("Data sukses disimpan", NotificationType.Success);
                Session.Remove("save-success");
            }
            if (Session["delete-success"] != null)
            {
                notifRecipe.Show("Data sukses dihapus", NotificationType.Success);
                Session.Remove("delete-success");
            }
        }
        #endregion
    }
}