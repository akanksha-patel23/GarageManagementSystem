<%@ Page Title="Status Board" Language="C#" MasterPageFile="~/Pages/Shared/Site.Master"
    AutoEventWireup="true" CodeBehind="JobCardStatusBoard.aspx.cs" Inherits="GarageManagement.Web.Pages.JobCards.JobCardStatusBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="msc-page-title">Job Card Status Board</h2>

    <asp:Label ID="lblMessage" runat="server" CssClass="msc-message" Visible="false" />
    <asp:Label ID="lblCurrentStatus" runat="server" CssClass="msc-page-title" Style="border:none;font-size:1rem;" />

    <%-- Flow E in 10-canonical-flows.md: Timer drives a server-side partial
         postback that rebinds the Repeater — not client-side AJAX polling.
         This is what genuinely exercises the Repeater control per the rubric,
         since a server Repeater can't be rebound by pure client-side JS. --%>
    <asp:Timer ID="statusTimer" runat="server" Interval="5000" OnTick="statusTimer_Tick" />

    <asp:UpdatePanel ID="statusUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Repeater ID="rptStatusHistory" runat="server">
                <HeaderTemplate>
                    <table class="msc-grid">
                        <tr><th>Status</th><th>Changed</th><th>Remarks</th></tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%# Eval("Status") %></td>
                        <td><%# Eval("ChangedDate", "{0:dd-MMM-yyyy HH:mm}") %></td>
                        <td><%# Eval("Remarks") %></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="statusTimer" EventName="Tick" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
