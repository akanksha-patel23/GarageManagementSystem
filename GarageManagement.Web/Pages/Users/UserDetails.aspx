<%@ Page Title="Add User" Language="C#" MasterPageFile="~/Pages/Shared/Site.Master"
    AutoEventWireup="true" CodeBehind="UserDetails.aspx.cs" Inherits="GarageManagement.Web.Pages.Users.UserDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="msc-page-title">Add User</h2>

    <div class="msc-form">
        <div class="msc-form-row">
            <asp:Label runat="server" AssociatedControlID="txtUsername" Text="Username" CssClass="msc-label" />
            <asp:TextBox ID="txtUsername" runat="server" CssClass="msc-input" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUsername"
                ErrorMessage="Username is required." CssClass="msc-validation-error" Display="Dynamic" />
        </div>

        <div class="msc-form-row">
            <asp:Label runat="server" AssociatedControlID="txtFullName" Text="Full Name" CssClass="msc-label" />
            <asp:TextBox ID="txtFullName" runat="server" CssClass="msc-input" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFullName"
                ErrorMessage="Full name is required." CssClass="msc-validation-error" Display="Dynamic" />
        </div>

        <div class="msc-form-row">
            <asp:Label runat="server" AssociatedControlID="txtPassword" Text="Password" CssClass="msc-label" />
            <asp:TextBox ID="txtPassword" runat="server" CssClass="msc-input" TextMode="Password" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword"
                ErrorMessage="Password is required." CssClass="msc-validation-error" Display="Dynamic" />
        </div>

        <div class="msc-form-row">
            <label class="msc-label">Role</label>
            <asp:RadioButtonList ID="rblRole" runat="server" RepeatLayout="Flow">
                <asp:ListItem Text="Admin" Value="Admin" />
                <asp:ListItem Text="Customer" Value="Customer" Selected="True" />
            </asp:RadioButtonList>
        </div>

        <asp:Button ID="btnSave" runat="server" Text="Create User" CssClass="msc-btn msc-btn-primary" OnClick="btnSave_Click" />
        <asp:HyperLink runat="server" NavigateUrl="~/Pages/Users/UserList.aspx" CssClass="msc-btn msc-btn-secondary">Cancel</asp:HyperLink>
    </div>

    <asp:ValidationSummary ID="valSummary" runat="server" CssClass="msc-validation-summary"
        HeaderText="Please fix the following:" />
    <asp:Label ID="lblMessage" runat="server" CssClass="msc-message" Visible="false" />
</asp:Content>
