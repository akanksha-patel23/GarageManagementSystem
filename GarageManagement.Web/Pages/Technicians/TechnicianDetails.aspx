<%@ Page Title="Technician Details" Language="C#" AutoEventWireup="true" MasterPageFile="~/Pages/Shared/Site.Master" CodeBehind="TechnicianDetails.aspx.cs" Inherits="GarageManagement.Web.Pages.Technicians.TechnicianDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="msc-page-title">Technician Details</h2>

    <div class="msc-form">
        <div class="msc-form-row">
            <asp:Label runat="server" AssociatedControlID="txtName" Text="Name" CssClass="msc-label" />
            <asp:TextBox ID="txtName" runat="server" CssClass="msc-input" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName"
                ErrorMessage="Name is required." CssClass="msc-validation-error" Display="Dynamic" />
        </div>

        <div class="msc-form-row">
            <asp:Label runat="server" AssociatedControlID="txtSpecialization" Text="Specialization" CssClass="msc-label" />
            <asp:TextBox ID="txtSpecialization" runat="server" CssClass="msc-input" />
        </div>

        <div class="msc-form-row">
            <asp:Label runat="server" AssociatedControlID="txtContactNumber" Text="Contact Number" CssClass="msc-label" />
            <asp:TextBox ID="txtContactNumber" runat="server" CssClass="msc-input" />
            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtContactNumber"
                ValidationExpression="^[0-9+\-\s]{7,20}$" ErrorMessage="Enter a valid contact number."
                CssClass="msc-validation-error" Display="Dynamic" />
        </div>

        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="msc-btn msc-btn-primary" OnClick="btnSave_Click" />
        <asp:HyperLink runat="server" NavigateUrl="~/Pages/Technicians/TechnicianDirectory.aspx"
            CssClass="msc-btn msc-btn-secondary">Cancel</asp:HyperLink>
    </div>

    <asp:ValidationSummary ID="valSummary" runat="server" CssClass="msc-validation-summary"
        HeaderText="Please fix the following:" />
    <asp:Label ID="lblMessage" runat="server" CssClass="msc-message" Visible="false" />
</asp:Content>