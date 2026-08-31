<%@ Page Title="New Job Card" Language="C#" ClientIDMode="Static" MasterPageFile="~/Pages/Shared/Site.Master"
    AutoEventWireup="true" CodeBehind="JobCardWizard.aspx.cs" Inherits="GarageManagement.Web.Pages.JobCards.JobCardWizard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="msc-page-title">New Job Card</h2>

    <%-- Read by jobcard-wizard.js — never hardcode the API URL into the .js file itself,
         per 08-environment.md. --%>
    <input type="hidden" id="hdnApiBaseUrl" runat="server" />

    <%-- ClientIDMode="Static" is set at the Page level above, so every runat="server"
         control below renders with its literal ID — no ctl00$MainContent$... mangling —
         which is what jobcard-wizard.js's jQuery selectors depend on. --%>
    <input type="hidden" id="hdnSelectedVehicleId" runat="server" />
    <input type="hidden" id="hdnCheckedServiceIds" runat="server" />

    <asp:Label ID="lblWizardError" runat="server" CssClass="msc-message msc-message-error" Visible="false" Style="display:none;" />

    <%-- asp:Wizard's Next/Previous cause postbacks. Wrapping it in an UpdatePanel
         makes those PARTIAL postbacks instead of full page reloads, so Site.Master
         chrome and the jQuery document-ready bindings survive. What the UpdatePanel
         alone does NOT fix is client-only state inside the panel (the AJAX-fed
         Vehicle dropdown, checked Services) — jobcard-wizard.js restores that via
         the hidden fields above and PageRequestManager's begin/end-request hooks. --%>
    <asp:UpdatePanel ID="wizardUpdatePanel" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Wizard ID="jobCardWizard" runat="server" CssClass="msc-jobcard-wizard"
                DisplaySideBar="true" ActiveStepIndex="0">
                <WizardSteps>

                    <asp:WizardStep ID="stepVehicle" runat="server" Title="Vehicle" StepType="Step">
                        <div class="msc-form-row">
                            <asp:Label runat="server" AssociatedControlID="ddlCustomer" Text="Customer" CssClass="msc-label" />
                            <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="msc-input"
                                DataTextField="Name" DataValueField="CustomerID" AppendDataBoundItems="true">
                                <asp:ListItem Text="-- Select Customer --" Value="" />
                            </asp:DropDownList>
                        </div>
                        <div class="msc-form-row">
                            <asp:Label runat="server" AssociatedControlID="ddlVehicle" Text="Vehicle" CssClass="msc-label" />
                            <%-- Populated via jQuery AJAX when Customer changes — Flow C, no code-behind involved --%>
                            <select id="ddlVehicle" runat="server" class="msc-input">
                                <option value="">-- Select Customer First --</option>
                            </select>
                        </div>
                    </asp:WizardStep>

                    <asp:WizardStep ID="stepServices" runat="server" Title="Services" StepType="Step">
                        <div class="msc-form-row">
                            <asp:Label runat="server" AssociatedControlID="ddlCategory" Text="Service Category" CssClass="msc-label" />
                            <select id="ddlCategory" runat="server" class="msc-input"></select>
                        </div>
                        <div class="msc-form-row">
                            <label class="msc-label">Services</label>
                            <%-- Populated via jQuery AJAX when Category changes — Flow D, no code-behind involved --%>
                            <div id="chkServices" class="msc-checkbox-list"></div>
                        </div>
                        <div class="msc-form-row">
                            <label class="msc-label">Running Total</label>
                            <span id="lblLiveTotal" class="msc-live-total">$0.00</span>
                        </div>
                    </asp:WizardStep>

                    <asp:WizardStep ID="stepSchedule" runat="server" Title="Technician &amp; Schedule" StepType="Step">
                        <div class="msc-form-row">
                            <asp:Label runat="server" AssociatedControlID="ddlTechnician" Text="Technician" CssClass="msc-label" />
                            <select id="ddlTechnician" runat="server" class="msc-input">
                                <option value="">-- Unassigned --</option>
                            </select>
                        </div>
                        <div class="msc-form-row">
                            <label class="msc-label">Scheduled Date</label>
                            <asp:Calendar ID="calScheduledDate" runat="server" CssClass="msc-calendar"
                                OnSelectionChanged="calScheduledDate_SelectionChanged" />
                            <input type="text" id="txtScheduledDate" runat="server" readonly="readonly" class="msc-input"
                                placeholder="Pick a date on the calendar" />
                        </div>
                    </asp:WizardStep>

                    <asp:WizardStep ID="stepReview" runat="server" Title="Review" StepType="Step">
                        <p>Review your selections on the previous steps, then create the job card.</p>
                        <button type="button" id="btnCreateJobCard" class="msc-btn msc-btn-primary">Create Job Card</button>
                    </asp:WizardStep>

                </WizardSteps>
            </asp:Wizard>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script src="/Scripts/jobcard-wizard.js"></script>
</asp:Content>
