<%@ Page Title="Register" Language="C#" AutoEventWireup="true"
    CodeBehind="Register.aspx.cs" Inherits="GarageManagement.Web.Register" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <title>Register - Garage Management System</title>
    <link rel="stylesheet" runat="server" href="~/Content/CSS/msc-base.css" />
    <link rel="stylesheet" runat="server" href="~/Content/CSS/msc-forms.css" />
</head>
<body>
    <form id="registerForm" runat="server">
        <main class="msc-main-content" style="max-width:420px;margin:4rem auto;">
            <h2 class="msc-page-title">Create an Account</h2>

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
                    <asp:Label runat="server" AssociatedControlID="txtConfirmPassword" Text="Confirm Password" CssClass="msc-label" />
                    <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="msc-input" TextMode="Password" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtConfirmPassword"
                        ErrorMessage="Please confirm your password." CssClass="msc-validation-error" Display="Dynamic" />
                    <asp:CompareValidator runat="server" ControlToValidate="txtConfirmPassword"
                        ControlToCompare="txtPassword" ErrorMessage="Passwords do not match."
                        CssClass="msc-validation-error" Display="Dynamic" />
                </div>

                <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="msc-btn msc-btn-primary" OnClick="btnRegister_Click" />
                <p class="msc-hint" style="margin-top:0.75rem;">
                    Already have an account? <asp:HyperLink runat="server" NavigateUrl="~/Login.aspx">Log in</asp:HyperLink>
                </p>
            </div>

            <asp:ValidationSummary runat="server" CssClass="msc-validation-summary" HeaderText="Please fix the following:" />
            <asp:Label ID="lblMessage" runat="server" CssClass="msc-message msc-message-error" Visible="false" />
        </main>
    </form>
</body>
</html>
