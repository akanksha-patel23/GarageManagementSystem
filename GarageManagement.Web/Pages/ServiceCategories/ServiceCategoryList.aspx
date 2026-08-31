<%@ Page Title="Service Categories" Language="C#" MasterPageFile="~/Pages/Shared/Site.Master"
    AutoEventWireup="true" CodeBehind="ServiceCategoryList.aspx.cs" Inherits="GarageManagement.Web.Pages.ServiceCategories.ServiceCategoryList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="msc-page-title">Service Categories</h2>

    <div class="msc-search-bar">
        <asp:HyperLink ID="lnkAddNew" runat="server" NavigateUrl="~/Pages/ServiceCategories/ServiceCategoryDetails.aspx"
            CssClass="msc-btn msc-btn-primary">+ Add Category</asp:HyperLink>
    </div>

    <%-- Genuine TreeView usage: each node's NavigateUrl goes straight to the
         Details page for that category, built recursively in code-behind from
         the same flat Depth-annotated data the wizard's dropdown flattens. --%>
    <asp:TreeView ID="tvCategories" runat="server" CssClass="msc-treeview"
        ShowLines="true" NodeWrap="true" />

    <asp:Label ID="lblMessage" runat="server" CssClass="msc-message" Visible="false" />
</asp:Content>
