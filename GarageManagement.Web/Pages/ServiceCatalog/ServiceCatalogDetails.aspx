<%@ Page Title="Service Details" Language="C#" MasterPageFile="~/Pages/Shared/Site.Master"
    AutoEventWireup="true" CodeBehind="ServiceCatalogDetails.aspx.cs" Inherits="GarageManagement.Web.Pages.ServiceCatalog.ServiceCatalogDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="msc-page-title">Service Details</h2>

    <div class="msc-form">
        <div class="msc-form-row">
            <asp:Label runat="server" AssociatedControlID="ddlCategory" Text="Category" CssClass="msc-label" />
            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="msc-input">
                <asp:ListItem Text="-- Select Category --" Value="" />
            </asp:DropDownList>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlCategory"
                InitialValue="" ErrorMessage="Please select a category."
                CssClass="msc-validation-error" Display="Dynamic" />
        </div>

        <div class="msc-form-row">
            <asp:Label runat="server" AssociatedControlID="txtServiceName" Text="Service Name" CssClass="msc-label" />
            <asp:TextBox ID="txtServiceName" runat="server" CssClass="msc-input" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtServiceName"
                ErrorMessage="Service name is required." CssClass="msc-validation-error" Display="Dynamic" />
        </div>

        <div class="msc-form-row">
            <asp:Label runat="server" AssociatedControlID="txtBasePrice" Text="Base Price" CssClass="msc-label" />
            <asp:TextBox ID="txtBasePrice" runat="server" CssClass="msc-input" TextMode="Number" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtBasePrice"
                ErrorMessage="Base price is required." CssClass="msc-validation-error" Display="Dynamic" />
            <asp:RangeValidator runat="server" ControlToValidate="txtBasePrice" Type="Currency"
                MinimumValue="0" MaximumValue="999999" ErrorMessage="Enter a valid price."
                CssClass="msc-validation-error" Display="Dynamic" />
        </div>

        <div class="msc-form-row">
            <asp:Label runat="server" AssociatedControlID="txtEstimatedHours" Text="Estimated Hours (optional)" CssClass="msc-label" />
            <asp:TextBox ID="txtEstimatedHours" runat="server" CssClass="msc-input" TextMode="Number" />
            <asp:RangeValidator runat="server" ControlToValidate="txtEstimatedHours" Type="Double"
                MinimumValue="0" MaximumValue="999" ErrorMessage="Enter a valid number of hours."
                CssClass="msc-validation-error" Display="Dynamic" />
        </div>

        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="msc-btn msc-btn-primary" OnClick="btnSave_Click" />
        <asp:HyperLink runat="server" NavigateUrl="~/Pages/ServiceCatalog/ServiceCatalogList.aspx"
            CssClass="msc-btn msc-btn-secondary">Cancel</asp:HyperLink>
    </div>

    <asp:ValidationSummary ID="valSummary" runat="server" CssClass="msc-validation-summary"
        HeaderText="Please fix the following:" />
    <asp:Label ID="lblMessage" runat="server" CssClass="msc-message" Visible="false" />
</asp:Content>
