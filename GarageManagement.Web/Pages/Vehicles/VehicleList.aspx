<%@ Page Title="Vehicle List" Language="C#" MasterPageFile="~/Pages/Shared/Site.Master" AutoEventWireup="true" CodeBehind="VehicleList.aspx.cs" Inherits="GarageManagement.Web.Pages.Vehicles.VehicleList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="msc-page-title">Vehicles</h2>

    <div class="msc-search-bar">
        <asp:HyperLink ID="lnkAddNew" runat="server" NavigateUrl="~/Pages/Vehicles/VehicleDetails.aspx"
            CssClass="msc-btn msc-btn-primary">+ Add Vehicle</asp:HyperLink>
    </div>

    <asp:GridView ID="gvVehicles" runat="server" CssClass="msc-grid" AutoGenerateColumns="false"
        DataKeyNames="VehicleID" AllowPaging="true" PageSize="10"
        OnPageIndexChanging="gvVehicles_PageIndexChanging"
        OnRowCommand="gvVehicles_RowCommand"
        OnRowDeleting="gvVehicles_RowDeleting">
        <Columns>
            <asp:BoundField DataField="VehicleID" HeaderText="ID" />
            <asp:BoundField DataField="RegNumber" HeaderText="Reg. Number" />
            <asp:BoundField DataField="MakeName" HeaderText="Make" />
            <asp:BoundField DataField="ModelName" HeaderText="Model" />
            <asp:BoundField DataField="Year" HeaderText="Year" />
            <asp:BoundField DataField="VehicleTypeName" HeaderText="Type" />
            <asp:TemplateField HeaderText="Photo">
                <ItemTemplate>
                    <asp:HyperLink runat="server"
                        NavigateUrl='<%# Eval("PhotoPath") == null || string.IsNullOrEmpty(Eval("PhotoPath").ToString()) ? "#" : "~/" + Eval("PhotoPath").ToString() %>'
                        Target="_blank"
                        CssClass="msc-link"
                        Enabled='<%# Eval("PhotoPath") != null && !string.IsNullOrEmpty(Eval("PhotoPath").ToString()) %>'
                        Text='<%# Eval("PhotoPath") == null || string.IsNullOrEmpty(Eval("PhotoPath").ToString()) ? "No Photo" : "View Photo" %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <asp:HyperLink runat="server"
                        NavigateUrl='<%# "~/Pages/Vehicles/VehicleDetails.aspx?VehicleID=" + Eval("VehicleID") %>'
                        CssClass="msc-link">Edit</asp:HyperLink>
                    <asp:LinkButton runat="server" CommandName="Delete" CommandArgument='<%# Eval("VehicleID") %>'
                        CssClass="msc-link msc-link-danger"
                        OnClientClick="return confirm('Delete this vehicle?');">Delete</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <asp:Label ID="lblMessage" runat="server" CssClass="msc-message" Visible="false" />
</asp:Content>
