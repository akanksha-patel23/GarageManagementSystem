<%@ Page Title="Category Details" Language="C#" MasterPageFile="~/Pages/Shared/Site.Master"
    AutoEventWireup="true" CodeBehind="ServiceCategoryDetails.aspx.cs" Inherits="GarageManagement.Web.Pages.ServiceCategories.ServiceCategoryDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="msc-page-title">Category Details</h2>

    <div class="msc-form">
        <div class="msc-form-row">
            <asp:Label runat="server" AssociatedControlID="ddlParentCategory" Text="Parent Category" CssClass="msc-label" />
            <asp:DropDownList ID="ddlParentCategory" runat="server" CssClass="msc-input">
                <asp:ListItem Text="-- None (Root Category) --" Value="" />
            </asp:DropDownList>
        </div>

        <div class="msc-form-row">
            <asp:Label runat="server" AssociatedControlID="txtCategoryName" Text="Category Name" CssClass="msc-label" />
            <asp:TextBox ID="txtCategoryName" runat="server" CssClass="msc-input" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCategoryName"
                ErrorMessage="Category name is required." CssClass="msc-validation-error" Display="Dynamic" />
        </div>

        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="msc-btn msc-btn-primary" OnClick="btnSave_Click" />
        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="msc-btn msc-btn-secondary" Visible="false"
            OnClick="btnDelete_Click" OnClientClick="return confirm('Delete this category? Child categories and any services under it may be affected.');" />
        <asp:HyperLink runat="server" NavigateUrl="~/Pages/ServiceCategories/ServiceCategoryList.aspx"
            CssClass="msc-btn msc-btn-secondary">Cancel</asp:HyperLink>
    </div>

    <asp:ValidationSummary ID="valSummary" runat="server" CssClass="msc-validation-summary"
        HeaderText="Please fix the following:" />
    <asp:Label ID="lblMessage" runat="server" CssClass="msc-message" Visible="false" />
</asp:Content>
