<%@ Page Title="Invoices" Language="C#" MasterPageFile="~/Pages/Shared/Site.Master"
    AutoEventWireup="true" CodeBehind="InvoiceList.aspx.cs" Inherits="GarageManagement.Web.Pages.Invoices.InvoiceList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="msc-page-title">Invoices</h2>

    <div class="msc-search-bar">
        <asp:HyperLink ID="lnkGenerate" runat="server" NavigateUrl="~/Pages/Invoices/InvoiceGenerate.aspx"
            CssClass="msc-btn msc-btn-primary">+ Generate Invoice</asp:HyperLink>
    </div>

    <asp:ListView ID="lvInvoices" runat="server" OnItemCommand="lvInvoices_ItemCommand">
        <LayoutTemplate>
            <table class="msc-grid">
                <tr>
                    <th>ID</th><th>Job Card</th><th>Generated</th><th>Total</th>
                    <th>Payment Mode</th><th>Status</th><th>Actions</th>
                </tr>
                <tr id="itemPlaceholder" runat="server" />
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr class="msc-invoice-row">
                <td><%# Eval("InvoiceID") %></td>
                <td><%# Eval("JobCardID") %></td>
                <td><%# Eval("GeneratedDate", "{0:dd-MMM-yyyy}") %></td>
                <td><%# Eval("TotalAmount", "{0:C}") %></td>
                <td><%# Eval("PaymentMode") %></td>
                <td><%# Eval("PaymentStatus") %></td>
                <td>
                    <asp:LinkButton runat="server" CommandName="MarkPaid" CommandArgument='<%# Eval("InvoiceID") %>'
                        CssClass="msc-link" Visible='<%# Eval("PaymentStatus").ToString() == "Pending" %>'>
                        Mark Paid
                    </asp:LinkButton>
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="msc-invoice-row msc-invoice-row-alt">
                <td><%# Eval("InvoiceID") %></td>
                <td><%# Eval("JobCardID") %></td>
                <td><%# Eval("GeneratedDate", "{0:dd-MMM-yyyy}") %></td>
                <td><%# Eval("TotalAmount", "{0:C}") %></td>
                <td><%# Eval("PaymentMode") %></td>
                <td><%# Eval("PaymentStatus") %></td>
                <td>
                    <asp:LinkButton runat="server" CommandName="MarkPaid" CommandArgument='<%# Eval("InvoiceID") %>'
                        CssClass="msc-link" Visible='<%# Eval("PaymentStatus").ToString() == "Pending" %>'>
                        Mark Paid
                    </asp:LinkButton>
                </td>
            </tr>
        </AlternatingItemTemplate>
        <EmptyDataTemplate>
            <p class="msc-hint">No invoices yet.</p>
        </EmptyDataTemplate>
    </asp:ListView>

    <asp:Label ID="lblMessage" runat="server" CssClass="msc-message" Visible="false" />
</asp:Content>
