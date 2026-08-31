<%@ Page Title="Parts List" Language="C#" MasterPageFile="~/Pages/Shared/Site.Master" AutoEventWireup="true" CodeBehind="PartsList.aspx.cs" Inherits="GarageManagement.Web.Pages.Parts.PartsList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="msc-page-title">Parts Inventory</h2>

    <div class="msc-search-bar">
        <asp:HyperLink ID="lnkAddNew" runat="server" NavigateUrl="~/Pages/Parts/PartsDetails.aspx"
            CssClass="msc-btn msc-btn-primary">+ Add Part</asp:HyperLink>
    </div>

    <asp:GridView ID="gvParts" runat="server" CssClass="msc-grid" AutoGenerateColumns="false"
        DataKeyNames="PartID" AllowPaging="true" PageSize="10"
        OnPageIndexChanging="gvParts_PageIndexChanging"
        OnRowCommand="gvParts_RowCommand"
        OnRowDeleting="gvParts_RowDeleting">
        <Columns>
            <asp:BoundField DataField="PartID" HeaderText="ID" />
            <asp:BoundField DataField="PartName" HeaderText="Part Name" />
            <asp:BoundField DataField="StockQty" HeaderText="Stock Qty" />
            <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price" DataFormatString="{0:C2}" />

            <asp:TemplateField HeaderText="Adjust Stock">
                <ItemTemplate>
                    <asp:LinkButton runat="server" CommandName="StockUp" CommandArgument='<%# Eval("PartID") %>'
                        CssClass="msc-link">+1</asp:LinkButton>
                    <asp:LinkButton runat="server" CommandName="StockDown" CommandArgument='<%# Eval("PartID") %>'
                        CssClass="msc-link">-1</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <asp:HyperLink runat="server"
                        NavigateUrl='<%# "~/Pages/Parts/PartsDetails.aspx?PartID=" + Eval("PartID") %>'
                        CssClass="msc-link">Edit</asp:HyperLink>
                    <asp:LinkButton runat="server" CommandName="Delete" CommandArgument='<%# Eval("PartID") %>'
                        CssClass="msc-link msc-link-danger"
                        OnClientClick="return confirm('Delete this part?');">Delete</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <asp:Label ID="lblMessage" runat="server" CssClass="msc-message" Visible="false" />
</asp:Content>
