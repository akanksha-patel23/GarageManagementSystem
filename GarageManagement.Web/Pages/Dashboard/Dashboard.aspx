<%@ Page Title="Dashboard" Language="C#" Async="true" MasterPageFile="~/Pages/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="GarageManagement.Web.Pages.Dashboard.Dashboard" %>

<%@ Register Assembly="System.Web.DataVisualization" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="msc-page-title">Dashboard</h2>

    <asp:Label ID="lblMessage" runat="server" CssClass="msc-message" Visible="false" />

    <div class="msc-dashboard-grid">
        <div class="msc-dashboard-panel">
            <h3>Monthly Revenue (Last 12 Months)</h3>
            <asp:Chart ID="chartRevenue" runat="server" Width="500px" Height="300px">
                <Series>
                    <asp:Series Name="Revenue" ChartType="Column" />
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="RevenueArea" />
                </ChartAreas>
            </asp:Chart>
        </div>

        <div class="msc-dashboard-panel">
            <h3>Job Status Distribution</h3>
            <asp:Chart ID="chartStatus" runat="server" Width="400px" Height="300px">
                <Series>
                    <asp:Series Name="Status" ChartType="Pie" />
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="StatusArea" />
                </ChartAreas>
            </asp:Chart>
        </div>
    </div>
</asp:Content>
