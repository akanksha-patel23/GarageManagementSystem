<%@ Page Title="Job Cards" Language="C#" MasterPageFile="~/Pages/Shared/Site.Master"
    AutoEventWireup="true" CodeBehind="JobCardList.aspx.cs" Inherits="GarageManagement.Web.Pages.JobCards.JobCardList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="msc-page-title">Job Cards</h2>

    <div class="msc-search-bar">
        <asp:HyperLink ID="lnkNewJobCard" runat="server" NavigateUrl="~/Pages/JobCards/JobCardWizard.aspx"
            CssClass="msc-btn msc-btn-primary">+ New Job Card</asp:HyperLink>
    </div>

    <asp:GridView ID="gvJobCards" runat="server" CssClass="msc-grid" AutoGenerateColumns="false"
        DataKeyNames="JobCardID" AllowPaging="true" PageSize="10"
        OnPageIndexChanging="gvJobCards_PageIndexChanging"
        OnRowCommand="gvJobCards_RowCommand">
        <Columns>
            <asp:BoundField DataField="JobCardID" HeaderText="ID" />
            <asp:BoundField DataField="VehicleID" HeaderText="Vehicle ID" />
            <asp:BoundField DataField="ScheduledDate" HeaderText="Scheduled" DataFormatString="{0:dd-MMM-yyyy}" />
            <asp:BoundField DataField="Status" HeaderText="Status" />
            <asp:BoundField DataField="TotalAmount" HeaderText="Total" DataFormatString="{0:C}" />
            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <asp:HyperLink runat="server"
                        NavigateUrl='<%# "~/Pages/JobCards/JobCardStatusBoard.aspx?JobCardID=" + Eval("JobCardID") %>'
                        CssClass="msc-link">Status Board</asp:HyperLink>

                    <asp:LinkButton runat="server" CommandName="Advance" CommandArgument='<%# Eval("JobCardID") %>'
                        CssClass="msc-link" Visible='<%# Eval("Status").ToString() != "Completed" && Eval("Status").ToString() != "Cancelled" %>'>
                        Advance Status
                    </asp:LinkButton>

                    <asp:LinkButton runat="server" CommandName="Cancel" CommandArgument='<%# Eval("JobCardID") %>'
                        CssClass="msc-link msc-link-danger"
                        Visible='<%# Eval("Status").ToString() != "Completed" && Eval("Status").ToString() != "Cancelled" %>'
                        OnClientClick="return confirm('Cancel this job card?');">
                        Cancel
                    </asp:LinkButton>

                    <%-- ModalPopupExtender per row: placing the trigger LinkButton, the
                         Panel, and the extender all inside the same ItemTemplate gives
                         each GridView row its own independent popup instance, bound to
                         that row's own data via Eval() — no code-behind needed to
                         populate a shared panel. --%>
                    <asp:LinkButton ID="lnkPreview" runat="server" CssClass="msc-link">Preview</asp:LinkButton>
                    <ajaxToolkit:ModalPopupExtender ID="mpePreview" runat="server"
                        TargetControlID="lnkPreview"
                        PopupControlID="pnlPreview"
                        CancelControlID="btnClosePreview"
                        BackgroundCssClass="msc-modal-backdrop" />
                    <asp:Panel ID="pnlPreview" runat="server" CssClass="msc-modal" Style="display:none;">
                        <h3>Job Card #<%# Eval("JobCardID") %></h3>
                        <p><strong>Vehicle ID:</strong> <%# Eval("VehicleID") %></p>
                        <p><strong>Scheduled:</strong> <%# Eval("ScheduledDate", "{0:dd-MMM-yyyy}") %></p>
                        <p><strong>Status:</strong> <%# Eval("Status") %></p>
                        <p><strong>Total:</strong> <%# Eval("TotalAmount", "{0:C}") %></p>
                        <asp:Button ID="btnClosePreview" runat="server" Text="Close" CssClass="msc-btn msc-btn-secondary" />
                    </asp:Panel>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <asp:Label ID="lblMessage" runat="server" CssClass="msc-message" Visible="false" />
</asp:Content>
