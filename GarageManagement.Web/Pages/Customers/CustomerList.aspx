<%@ Page Title="Customers" Language="C#" MasterPageFile="~/Pages/Shared/Site.Master"
    AutoEventWireup="true" CodeBehind="CustomerList.aspx.cs" Inherits="GarageManagement.Web.Pages.Customers.CustomerList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="msc-page-title">Customers</h2>

    <div class="msc-search-bar">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="msc-input" placeholder="Search by name or phone" />
        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="msc-btn" OnClick="btnSearch_Click" />
        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="msc-btn msc-btn-secondary" OnClick="btnClear_Click" />
        <asp:HyperLink ID="lnkAddNew" runat="server" NavigateUrl="~/Pages/Customers/CustomerDetails.aspx"
            CssClass="msc-btn msc-btn-primary">+ Add Customer</asp:HyperLink>
    </div>

    <asp:GridView ID="gvCustomers" runat="server" CssClass="msc-grid" AutoGenerateColumns="false"
        DataKeyNames="CustomerID" AllowPaging="true" PageSize="10"
        OnPageIndexChanging="gvCustomers_PageIndexChanging"
        OnRowCommand="gvCustomers_RowCommand"
        OnRowDeleting="gvCustomers_RowDeleting">
        <Columns>
            <asp:BoundField DataField="CustomerID" HeaderText="ID" />
            <asp:BoundField DataField="Name" HeaderText="Name" />
            <asp:BoundField DataField="Phone" HeaderText="Phone" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
            <asp:BoundField DataField="RegisteredDate" HeaderText="Registered" DataFormatString="{0:dd-MMM-yyyy}" />
            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <asp:HyperLink runat="server"
                        NavigateUrl='<%# "~/Pages/Customers/CustomerDetails.aspx?CustomerID=" + Eval("CustomerID") %>'
                        CssClass="msc-link">Edit</asp:HyperLink>
                    <asp:LinkButton runat="server" CommandName="Delete" CommandArgument='<%# Eval("CustomerID") %>'
                        CssClass="msc-link msc-link-danger"
                        OnClientClick="return confirm('Delete this customer?');">Delete</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <asp:Label ID="lblMessage" runat="server" CssClass="msc-message" Visible="false" />
</asp:Content>
