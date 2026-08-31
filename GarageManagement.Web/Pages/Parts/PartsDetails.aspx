<%@ Page Title="Part Details" Language="C#" AutoEventWireup="true" MasterPageFile="~/Pages/Shared/Site.Master" CodeBehind="PartsDetails.aspx.cs" Inherits="GarageManagement.Web.Pages.Parts.PartsDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="msc-page-title">Part Details</h2>

    <div class="msc-form">
        <div class="msc-form-row">
            <asp:Label runat="server" AssociatedControlID="txtPartName" Text="Part Name" CssClass="msc-label" />
            <asp:TextBox ID="txtPartName" runat="server" CssClass="msc-input" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPartName"
                ErrorMessage="Part name is required." CssClass="msc-validation-error" Display="Dynamic" />
        </div>

        <div class="msc-form-row">
            <asp:Label runat="server" AssociatedControlID="txtStockQty" Text="Initial Stock Quantity" CssClass="msc-label" />
            
            <asp:TextBox ID="txtStockQty" runat="server" CssClass="msc-input" TextMode="Number" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtStockQty"
                ErrorMessage="Stock quantity is required." CssClass="msc-validation-error" Display="Dynamic" />
            <asp:RangeValidator runat="server" ControlToValidate="txtStockQty" Type="Integer"
                MinimumValue="0" MaximumValue="999999" ErrorMessage="Enter a valid quantity."
                CssClass="msc-validation-error" Display="Dynamic" />
            <asp:Label ID="lblStockHint" runat="server" CssClass="msc-hint" Visible="false"
                Text="Stock quantity is fixed here — use the +1/-1 actions on the Parts list to adjust it." />
        </div>

        <div class="msc-form-row">
            <asp:Label runat="server" AssociatedControlID="txtUnitPrice" Text="Unit Price" CssClass="msc-label" />
            
             <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="msc-input" TextMode="Number" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUnitPrice"
                ErrorMessage="Unit price is required." CssClass="msc-validation-error" Display="Dynamic" />
            <asp:RangeValidator runat="server" ControlToValidate="txtUnitPrice" Type="Currency"
                MinimumValue="0" MaximumValue="999999" ErrorMessage="Enter a valid price."
                CssClass="msc-validation-error" Display="Dynamic" />
        </div>

        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="msc-btn msc-btn-primary" OnClick="btnSave_Click" />
        <asp:HyperLink runat="server" NavigateUrl="~/Pages/Parts/PartsList.aspx"
            CssClass="msc-btn msc-btn-secondary">Cancel</asp:HyperLink>
    </div>

    <asp:ValidationSummary ID="valSummary" runat="server" CssClass="msc-validation-summary"
        HeaderText="Please fix the following:" />
    <asp:Label ID="lblMessage" runat="server" CssClass="msc-message" Visible="false" />
</asp:Content>
