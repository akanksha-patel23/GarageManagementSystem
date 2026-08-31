<%@ Page Title="Vehicle Details" Language="C#" MasterPageFile="~/Pages/Shared/Site.Master" AutoEventWireup="true" CodeBehind="VehicleDetails.aspx.cs" Inherits="GarageManagement.Web.Pages.Vehicles.VehicleDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="msc-page-title">Vehicle Details</h2>

    <div class="msc-form">
        <div class="msc-form-row">
            <asp:Label runat="server" AssociatedControlID="ddlCustomer" Text="Customer" CssClass="msc-label" />
            <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="msc-input"
                DataTextField="Name" DataValueField="CustomerID" AppendDataBoundItems="true">
                <asp:ListItem Text="-- Select Customer --" Value="" />
            </asp:DropDownList>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlCustomer"
                InitialValue="" ErrorMessage="Please select a customer."
                CssClass="msc-validation-error" Display="Dynamic" />
        </div>

        <div class="msc-form-row">
            <asp:Label runat="server" AssociatedControlID="txtRegNumber" Text="Registration Number" CssClass="msc-label" />
            <asp:TextBox ID="txtRegNumber" runat="server" CssClass="msc-input" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtRegNumber"
                ErrorMessage="Registration number is required." CssClass="msc-validation-error" Display="Dynamic" />
            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtRegNumber"
                ValidationExpression="^[A-Za-z0-9\-\s]{4,20}$" ErrorMessage="Enter a valid registration number."
                CssClass="msc-validation-error" Display="Dynamic" />
        </div>

        <div class="msc-form-row">
            <asp:Label runat="server" AssociatedControlID="txtMake" Text="Make" CssClass="msc-label" />
            <asp:TextBox ID="txtMake" runat="server" CssClass="msc-input" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtMake"
                ErrorMessage="Make is required." CssClass="msc-validation-error" Display="Dynamic" />
        </div>

        <div class="msc-form-row">
            <asp:Label runat="server" AssociatedControlID="txtModel" Text="Model" CssClass="msc-label" />
            <asp:TextBox ID="txtModel" runat="server" CssClass="msc-input" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtModel"
                ErrorMessage="Model is required." CssClass="msc-validation-error" Display="Dynamic" />
        </div>

        <div class="msc-form-row">
            <asp:Label runat="server" AssociatedControlID="txtYear" Text="Year" CssClass="msc-label" />
            <asp:TextBox ID="txtYear" runat="server" CssClass="msc-input" TextMode="Number" />
            <asp:RangeValidator runat="server" ControlToValidate="txtYear" Type="Integer"
                MinimumValue="1980" MaximumValue="2100" ErrorMessage="Enter a valid year."
                CssClass="msc-validation-error" Display="Dynamic" />
        </div>

        <div class="msc-form-row">
            <asp:Label runat="server" AssociatedControlID="ddlVehicleType" Text="Vehicle Type" CssClass="msc-label" />
            <asp:DropDownList ID="ddlVehicleType" runat="server" CssClass="msc-input">
                <asp:ListItem Text="Sedan" Value="Sedan" />
                <asp:ListItem Text="SUV" Value="SUV" />
                <asp:ListItem Text="Hatchback" Value="Hatchback" />
                <asp:ListItem Text="Truck" Value="Truck" />
                <asp:ListItem Text="Motorcycle" Value="Motorcycle" />
                <asp:ListItem Text="Sports" Value="Sports" />
                <asp:ListItem Text="Other" Value="Other" />
            </asp:DropDownList>
        </div>

        <div class="msc-form-row">
            <asp:Label runat="server" AssociatedControlID="fuPhoto" Text="Vehicle Photo" CssClass="msc-label" />
            <asp:FileUpload ID="fuPhoto" runat="server" CssClass="msc-input" />
            <asp:CustomValidator ID="cvPhoto" runat="server" ControlToValidate="fuPhoto"
                OnServerValidate="cvPhoto_ServerValidate"
                ErrorMessage="Photo must be .jpg, .jpeg, or .png and no larger than 5 MB."
                CssClass="msc-validation-error" Display="Dynamic" />
            <asp:Label ID="lblExistingPhoto" runat="server" CssClass="msc-hint" Visible="false" />
        </div>

        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="msc-btn msc-btn-primary" OnClick="btnSave_Click" />
        <asp:HyperLink runat="server" NavigateUrl="~/Pages/Vehicles/VehicleList.aspx" CssClass="msc-btn msc-btn-secondary">Cancel</asp:HyperLink>
    </div>

    <asp:ValidationSummary ID="valSummary" runat="server" CssClass="msc-validation-summary"
        HeaderText="Please fix the following:" />
    <asp:Label ID="lblMessage" runat="server" CssClass="msc-message" Visible="false" />
</asp:Content>
