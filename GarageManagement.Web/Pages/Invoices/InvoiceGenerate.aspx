<%@ Page Title="Generate Invoice" Language="C#" MasterPageFile="~/Pages/Shared/Site.Master"
    AutoEventWireup="true" CodeBehind="InvoiceGenerate.aspx.cs" Inherits="GarageManagement.Web.Pages.Invoices.InvoiceGenerate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="msc-page-title">Generate Invoice</h2>

    <div class="msc-form">
        <div class="msc-form-row">
            <asp:Label runat="server" AssociatedControlID="txtJobCardId" Text="Job Card ID" CssClass="msc-label" />
            <asp:TextBox ID="txtJobCardId" runat="server" CssClass="msc-input" TextMode="Number" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtJobCardId"
                ErrorMessage="Job Card ID is required." CssClass="msc-validation-error" Display="Dynamic" />
        </div>

        <div class="msc-form-row">
            <asp:Label runat="server" AssociatedControlID="txtTotalAmount" Text="Total Amount" CssClass="msc-label" />
            <asp:TextBox ID="txtTotalAmount" runat="server" CssClass="msc-input" TextMode="Number" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTotalAmount"
                ErrorMessage="Total amount is required." CssClass="msc-validation-error" Display="Dynamic" />
            <asp:RangeValidator runat="server" ControlToValidate="txtTotalAmount" Type="Currency"
                MinimumValue="0" MaximumValue="999999" ErrorMessage="Enter a valid amount."
                CssClass="msc-validation-error" Display="Dynamic" />
        </div>

        <div class="msc-form-row">
            <label class="msc-label">Payment Mode</label>
            <asp:RadioButtonList ID="rblPaymentMode" runat="server" CssClass="msc-input" RepeatLayout="Flow">
                <asp:ListItem Text="Cash" Value="Cash" Selected="True" />
                <asp:ListItem Text="Card" Value="Card" />
                <asp:ListItem Text="UPI" Value="UPI" />
                <asp:ListItem Text="Bank Transfer" Value="BankTransfer" />
            </asp:RadioButtonList>
        </div>

        <asp:Button ID="btnGenerate" runat="server" Text="Generate Invoice" CssClass="msc-btn msc-btn-primary" OnClick="btnGenerate_Click" />
        <asp:HyperLink runat="server" NavigateUrl="~/Pages/Invoices/InvoiceList.aspx" CssClass="msc-btn msc-btn-secondary">Cancel</asp:HyperLink>
    </div>

    <asp:ValidationSummary ID="valSummary" runat="server" CssClass="msc-validation-summary"
        HeaderText="Please fix the following:" />
    <asp:Label ID="lblMessage" runat="server" CssClass="msc-message" Visible="false" />
</asp:Content>
