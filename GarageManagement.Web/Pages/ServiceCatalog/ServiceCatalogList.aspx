<%@ Page Title="Service Catalog" Language="C#" MasterPageFile="~/Pages/Shared/Site.Master"
    AutoEventWireup="true" CodeBehind="ServiceCatalogList.aspx.cs" Inherits="GarageManagement.Web.Pages.ServiceCatalog.ServiceCatalogList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="msc-page-title">Service Catalog</h2>

    <div class="msc-search-bar">
        <asp:HyperLink ID="lnkAddNew" runat="server" NavigateUrl="~/Pages/ServiceCatalog/ServiceCatalogDetails.aspx"
            CssClass="msc-btn msc-btn-primary">+ Add Service</asp:HyperLink>
    </div>

    <asp:GridView ID="gvServiceCatalog" runat="server" CssClass="msc-grid" AutoGenerateColumns="false"
        DataKeyNames="ServiceID" AllowPaging="true" PageSize="10"
        OnPageIndexChanging="gvServiceCatalog_PageIndexChanging"
        OnRowCommand="gvServiceCatalog_RowCommand"
        OnRowDeleting="gvServiceCatalog_RowDeleting">
        <Columns>
            <asp:BoundField DataField="ServiceID" HeaderText="ID" />
            <asp:BoundField DataField="CategoryName" HeaderText="Category" />
            <asp:BoundField DataField="ServiceName" HeaderText="Service Name" />
            <asp:BoundField DataField="BasePrice" HeaderText="Base Price" DataFormatString="{0:C}" />
            <asp:BoundField DataField="EstimatedHours" HeaderText="Est. Hours" />
            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <asp:HyperLink runat="server"
                        NavigateUrl='<%# "~/Pages/ServiceCatalog/ServiceCatalogDetails.aspx?ServiceID=" + Eval("ServiceID") %>'
                        CssClass="msc-link">Edit</asp:HyperLink>
                    <asp:LinkButton runat="server" CommandName="Delete" CommandArgument='<%# Eval("ServiceID") %>'
                        CssClass="msc-link msc-link-danger"
                        OnClientClick="return confirm('Delete this service?');">Delete</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <asp:Label ID="lblMessage" runat="server" CssClass="msc-message" Visible="false" />
</asp:Content>
