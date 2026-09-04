<%@ Page Title="Users" Language="C#" MasterPageFile="~/Pages/Shared/Site.Master"
    AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="GarageManagement.Web.Pages.Users.UserList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="msc-page-title">Users</h2>

    <div class="msc-search-bar">
        <asp:HyperLink ID="lnkAddNew" runat="server" NavigateUrl="~/Pages/Users/UserDetails.aspx"
            CssClass="msc-btn msc-btn-primary">+ Add User</asp:HyperLink>
    </div>

    <asp:GridView ID="gvUsers" runat="server" CssClass="msc-grid" AutoGenerateColumns="false"
        DataKeyNames="UserID" AllowPaging="true" PageSize="10"
        OnPageIndexChanging="gvUsers_PageIndexChanging"
        OnRowCommand="gvUsers_RowCommand">
        <Columns>
            <asp:BoundField DataField="UserID" HeaderText="ID" />
            <asp:BoundField DataField="Username" HeaderText="Username" />
            <asp:BoundField DataField="FullName" HeaderText="Full Name" />
            <asp:BoundField DataField="Role" HeaderText="Role" />
            <asp:TemplateField HeaderText="Status">
                <ItemTemplate>
                    <%# (bool)Eval("IsActive") ? "Active" : "Disabled" %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <asp:LinkButton runat="server" CommandName="ToggleActive" CommandArgument='<%# Eval("UserID") %>'
                        CssClass="msc-link">
                        <%# (bool)Eval("IsActive") ? "Disable" : "Enable" %>
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <asp:Label ID="lblMessage" runat="server" CssClass="msc-message" Visible="false" />
</asp:Content>
