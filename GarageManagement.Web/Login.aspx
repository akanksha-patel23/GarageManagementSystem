<%@ Page Title="Login" Language="C#" AutoEventWireup="true" Async="true" CodeBehind="Login.aspx.cs" Inherits="GarageManagement.Web.Login" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <title>Login - Garage Management System</title>
    <link rel="stylesheet" runat="server" href="~/Content/CSS/msc-base.css" />
    <link rel="stylesheet" runat="server" href="~/Content/CSS/msc-forms.css" />
</head>
<body>
    <form id="loginForm" runat="server">
        <main class="msc-main-content" style="max-width:420px;margin:4rem auto;">
            <h2 class="msc-page-title">Garage Management System</h2>

            <div class="msc-form">
                <div class="msc-form-row">
                    <asp:Label runat="server" AssociatedControlID="txtUsername" Text="Username" CssClass="msc-label" />
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="msc-input" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUsername"
                        ErrorMessage="Username is required." CssClass="msc-validation-error" Display="Dynamic" />
                </div>
                <div class="msc-form-row">
                    <asp:Label runat="server" AssociatedControlID="txtPassword" Text="Password" CssClass="msc-label" />
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="msc-input" TextMode="Password" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword"
                        ErrorMessage="Password is required." CssClass="msc-validation-error" Display="Dynamic" />
                </div>

                <asp:Button ID="btnLogin" runat="server" Text="Log In" CssClass="msc-btn msc-btn-primary" OnClick="btnLogin_Click" />
                <p class="msc-hint" style="margin-top:0.75rem;">
                    Don't have an account? <asp:HyperLink runat="server" NavigateUrl="~/Register.aspx">Register</asp:HyperLink>
                </p>
            </div>

            <asp:ValidationSummary runat="server" CssClass="msc-validation-summary" HeaderText="Please fix the following:" />
            <asp:Label ID="lblMessage" runat="server" CssClass="msc-message msc-message-error" Visible="false" />
        </main>
    </form>
</body>
</html>
