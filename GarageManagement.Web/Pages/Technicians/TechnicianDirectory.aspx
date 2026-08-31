<%@ Page Title="Technician Directory" Language="C#" MasterPageFile="~/Pages/Shared/Site.Master" AutoEventWireup="true" CodeBehind="TechnicianDirectory.aspx.cs" Inherits="GarageManagement.Web.Pages.Technicians.TechnicianDirectory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="msc-page-title">Technicians</h2>

    <div class="msc-search-bar">
        <asp:HyperLink ID="lnkAddNew" runat="server" NavigateUrl="~/Pages/Technicians/TechnicianDetails.aspx"
            CssClass="msc-btn msc-btn-primary">+ Add Technician</asp:HyperLink>
    </div>

    <asp:DataList ID="dlTechnicians" runat="server" CssClass="msc-datalist" RepeatColumns="3"
        RepeatDirection="Horizontal" OnItemCommand="dlTechnicians_ItemCommand">
        <ItemTemplate>
            <div class="msc-technician-card">
                <h3><%# Eval("Name") %></h3>
                <p class="msc-hint"><%# Eval("Specialization") %></p>
                <p><%# Eval("ContactNumber") %></p>
                <asp:HyperLink runat="server"
                    NavigateUrl='<%# "~/Pages/Technicians/TechnicianDetails.aspx?TechnicianID=" + Eval("TechnicianID") %>'
                    CssClass="msc-link">Edit</asp:HyperLink>
                <asp:LinkButton runat="server" CommandName="Delete" CommandArgument='<%# Eval("TechnicianID") %>'
                    CssClass="msc-link msc-link-danger"
                    OnClientClick="return confirm('Delete this technician?');">Delete</asp:LinkButton>
            </div>
        </ItemTemplate>
    </asp:DataList>

    <asp:Label ID="lblMessage" runat="server" CssClass="msc-message" Visible="false" />
</asp:Content>
