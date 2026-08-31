<%@ Page Language="C#" MasterPageFile="~/Pages/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="GarageManagement.Web.Pages.Shared.Error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="msc-error-page">
        <h2 class="msc-page-title">Something went wrong</h2>
        <p>An unexpected error occurred and has been logged. Please try again, or contact
           support if the problem continues.</p>
        <asp:HyperLink runat="server" NavigateUrl="~/Pages/Dashboard/Dashboard.aspx"
            CssClass="msc-btn msc-btn-primary">Return to Dashboard</asp:HyperLink>
    </div>
</asp:Content>

