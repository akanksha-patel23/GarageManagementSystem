<%@ Page Title="Generate Invoice" Language="C#" MasterPageFile="~/Pages/Shared/Site.Master"
    AutoEventWireup="true" CodeBehind="InvoiceGenerate.aspx.cs" Inherits="GarageManagement.Web.Pages.Invoices.InvoiceGenerate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="msc-page-title">Generate Invoice</h2>

    <div class="msc-form">
        <div class="msc-form-row">
            <asp:Label runat="server" AssociatedControlID="ddlJobCardId" Text="Job Card ID" CssClass="msc-label" />
            <asp:DropDownList ID="ddlJobCardId" runat="server" CssClass="msc-input" DataTextField="JobCardID" DataValueField="JobCardID" AutoPostBack="false">
                <asp:ListItem Text="Select a job card" Value="" />
            </asp:DropDownList>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlJobCardId"
                ErrorMessage="Job Card ID is required." CssClass="msc-validation-error" Display="Dynamic" InitialValue="" />
        </div>

        <div class="msc-form-row">
            <asp:Label runat="server" AssociatedControlID="txtTotalAmount" Text="Total Amount" CssClass="msc-label" />
            <asp:TextBox ID="txtTotalAmount" runat="server" CssClass="msc-input" TextMode="Number" ReadOnly="true" />
            <asp:HiddenField ID="hfTotalAmount" runat="server" />
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
    <asp:HiddenField ID="hfJobCardData" runat="server" />

    <script type="text/javascript">
        function bindTotalAmount(jobCardId) {
            var jobCardData = JSON.parse(document.getElementById('<%= hfJobCardData.ClientID %>').value);
            var totalAmountInput = document.getElementById('<%= txtTotalAmount.ClientID %>');
            var totalAmountHidden = document.getElementById('<%= hfTotalAmount.ClientID %>');
            
            for (var i = 0; i < jobCardData.length; i++) {
                if (jobCardData[i].JobCardID == jobCardId) {
                    var amount = jobCardData[i].TotalAmount.toFixed(2);
                    totalAmountInput.value = amount;
                    totalAmountHidden.value = amount;
                    break;
                }
            }
        }

        function onJobCardChange() {
            var dropdown = document.getElementById('<%= ddlJobCardId.ClientID %>');
            var jobCardId = dropdown.value;
            if (jobCardId) {
                bindTotalAmount(jobCardId);
            }
        }

        function initializeJobCardSelection() {
            var dropdown = document.getElementById('<%= ddlJobCardId.ClientID %>');
            if (dropdown) {
                dropdown.addEventListener('change', function() {
                    onJobCardChange();
                });
            }
        }

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function() {
            initializeJobCardSelection();
        });
    </script>
</asp:Content>
