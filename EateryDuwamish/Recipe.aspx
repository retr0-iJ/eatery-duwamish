﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Recipe.aspx.cs" Inherits="EateryDuwamish.Recipe" %>
<%@ Register Src="~/UserControl/NotificationControl.ascx" TagName="NotificationControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <%--Datatable Configuration--%>
    <script type="text/javascript">
        function ConfigureDatatable() {
            var table = null;
            if ($.fn.dataTable.isDataTable('#htblRecipe')) {
                table = $('#htblRecipe').DataTable();
            }
            else {
                table = $('#htblRecipe').DataTable({
                    stateSave: false,
                    order: [[1, "asc"]],
                    columnDefs: [{ orderable: false, targets: [0] }]
                });
            }
            return table;
        }
    </script>
    <%--Checkbox Event Configuration--%>
    <script type="text/javascript">
        function ConfigureCheckboxEvent() {
            $('.checkDelete input').change(function () {
                var parent = $(this).parent();
                var value = $(parent).attr('data-value');
                var deletedList = [];

                if ($('#<%=hdfDeletedRecipes.ClientID%>').val())
                    deletedList = $('#<%=hdfDeletedRecipes.ClientID%>').val().split(',');

                if ($(this).is(':checked')) {
                    deletedList.push(value);
                    $('#<%=hdfDeletedRecipes.ClientID%>').val(deletedList.join(','));
                }
                else {
                    var index = deletedList.indexOf(value);
                    if (index >= 0)
                        deletedList.splice(index, 1);
                    $('#<%=hdfDeletedRecipes.ClientID%>').val(deletedList.join(','));
                }
            });
        }
    </script>
    <%--Main Configuration--%>
    <script type="text/javascript">
        function ConfigureElements() {
            ConfigureDatatable();
            ConfigureCheckboxEvent();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <script type="text/javascript">
                $(document).ready(function () {
                    ConfigureElements();
                });
                <%--On Partial Postback Callback Function--%>
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_endRequest(function () {
                    ConfigureElements();
                });
            </script>
            <uc1:NotificationControl ID="notifRecipe" runat="server" />
            <div class="page-title">Recipes</div><hr style="margin:0"/>
            <%--FORM DISH RECIPE--%>
            <asp:Panel runat="server" ID="pnlFormRecipe" Visible="false">
                <div class="form-slip">
                    <div class="form-slip-header">
                        <div class="form-slip-title">
                            FORM RECIPE - 
                            <asp:Literal runat="server" ID="litFormType"></asp:Literal>
                        </div>
                        <hr style="margin:0"/>
                    </div>
                    <div class="form-slip-main">
                        <asp:HiddenField ID="hdfRecipeId" runat="server" Value="0"/>
                        <div>
                            <%--Recipe Name Field--%>
                            <div class="col-lg-6 form-group">
                                <div class="col-lg-4 control-label">
                                    Recipe Name*
                                </div>
                                <div class="col-lg-6">
                                    <asp:TextBox ID="txtRecipeName" CssClass="form-control" runat="server"></asp:TextBox>
                                    <%--Validator--%>
                                    <asp:RequiredFieldValidator ID="rfvRecipeName" runat="server" ErrorMessage="Please fill this field"
                                        ControlToValidate="txtRecipeName" ForeColor="Red" 
                                        ValidationGroup="InsertRecipe" Display="Dynamic">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revRecipeName" runat="server" ErrorMessage="This field has a maximum of 200 characters"
                                        ControlToValidate="txtRecipeName" ValidationExpression="^[\s\S]{0,200}$" ForeColor="Red"
                                        ValidationGroup="InsertRecipe" Display="Dynamic">
                                    </asp:RegularExpressionValidator>
                                    <%--End of Validator--%>
                                </div>
                            </div>
                            <%--End of Recipe Name Field--%>
                        </div>
                        <div class="col-lg-12">
                            <div class="col-lg-2">
                            </div>
                            <div class="col-lg-2">
                                <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Width="100px"
                                    Text="SAVE" OnClick="btnSave_Click" ValidationGroup="InsertRecipe">
                                </asp:Button>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <%--END OF FORM DISH--%>

            <div class="row">
                <div class="table-header">
                    <div class="table-header-title">
                        <asp:Literal ID="litDishName" runat="server"></asp:Literal> 
                    </div>
                    <div class="table-header-button">
                        <asp:Button ID="btnAdd" runat="server" Text="ADD" CssClass="btn btn-primary" Width="100px"
                            OnClick="btnAdd_Click" />
                        <asp:Button ID="btnDelete" runat="server" Text="DELETE" CssClass="btn btn-danger" Width="100px"
                            OnClick="btnDelete_Click" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="table-main col-sm-12">
                    <asp:HiddenField ID="hdfDeletedRecipes" runat="server" />
                    <asp:Repeater ID="rptRecipe" runat="server" OnItemDataBound="rptRecipe_ItemDataBound" OnItemCommand="rptRecipe_ItemCommand">
                        <HeaderTemplate>
                            <table id="htblRecipe" class="table">
                                <thead>
                                    <tr role="row">
                                        <th aria-sort="ascending" style="" colspan="1" rowspan="1"
                                            tabindex="0" class="sorting_asc center">
                                        </th>
                                        <th aria-sort="ascending" style="" colspan="1" rowspan="1" tabindex="0"
                                            class="sorting_asc text-center">
                                            Recipe Name
                                        </th>
                                        <th style="" colspan="1" rowspan="1" tabindex="0"
                                            class="text-center">
                                            Toggle
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="odd text-center" role="row" runat="server" onclick="">
                                <td>
                                    <div">
                                        <asp:CheckBox ID="chkChoose" CssClass="checkDelete" runat="server">
                                        </asp:CheckBox>
                                    </div>
                                </td>
                                <td>
                                    <asp:Literal ID="litRecipeName" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lbRecipeDetail" runat="server" CommandName="DETAIL">Detail</asp:LinkButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </tbody> 
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
