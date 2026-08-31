<%@ Page Title="Customer Details" Language="C#" MasterPageFile="~/Pages/Shared/Site.Master"
    AutoEventWireup="true" CodeBehind="CustomerDetails.aspx.cs" Inherits="GarageManagement.Web.Pages.Customers.CustomerDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="msc-page-title">Customer Details</h2>

    <asp:DetailsView ID="dvCustomer" runat="server" CssClass="msc-detailsview" AutoGenerateRows="false"
        OnItemInserting="dvCustomer_ItemInserting" OnItemUpdating="dvCustomer_ItemUpdating" OnModeChanging="dvCustomer_ModeChanging">
        <Fields>
            <asp:TemplateField HeaderText="Name">
                <ItemTemplate><%# Eval("Name") %></ItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox ID="txtNameInsert" runat="server" Text='<%# Bind("Name") %>' CssClass="msc-input" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNameInsert"
                        ErrorMessage="Name is required." CssClass="msc-validation-error" Display="Dynamic" />
                </InsertItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtNameEdit" runat="server" Text='<%# Bind("Name") %>' CssClass="msc-input" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNameEdit"
                        ErrorMessage="Name is required." CssClass="msc-validation-error" Display="Dynamic" />
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Phone">
                <ItemTemplate><%# Eval("Phone") %></ItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox ID="txtPhoneInsert" runat="server" Text='<%# Bind("Phone") %>' CssClass="msc-input" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPhoneInsert"
                        ErrorMessage="Phone is required." CssClass="msc-validation-error" Display="Dynamic" />
                    <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPhoneInsert"
                        ValidationExpression="^[0-9+\-\s]{7,20}$" ErrorMessage="Enter a valid phone number."
                        CssClass="msc-validation-error" Display="Dynamic" />
                </InsertItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtPhoneEdit" runat="server" Text='<%# Bind("Phone") %>' CssClass="msc-input" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPhoneEdit"
                        ErrorMessage="Phone is required." CssClass="msc-validation-error" Display="Dynamic" />
                    <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPhoneEdit"
                        ValidationExpression="^[0-9+\-\s]{7,20}$" ErrorMessage="Enter a valid phone number."
                        CssClass="msc-validation-error" Display="Dynamic" />
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Email">
                <ItemTemplate><%# Eval("Email") %></ItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox ID="txtEmailInsert" runat="server" Text='<%# Bind("Email") %>' CssClass="msc-input" />
                    <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmailInsert"
                        ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorMessage="Enter a valid email."
                        CssClass="msc-validation-error" Display="Dynamic" />
                </InsertItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtEmailEdit" runat="server" Text='<%# Bind("Email") %>' CssClass="msc-input" />
                    <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmailEdit"
                        ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorMessage="Enter a valid email."
                        CssClass="msc-validation-error" Display="Dynamic" />
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Address">
                <ItemTemplate><%# Eval("Address") %></ItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox ID="txtAddressInsert" runat="server" Text='<%# Bind("Address") %>'
                        CssClass="msc-input" TextMode="MultiLine" />
                </InsertItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtAddressEdit" runat="server" Text='<%# Bind("Address") %>'
                        CssClass="msc-input" TextMode="MultiLine" />
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:CommandField ShowEditButton="true" ShowInsertButton="true" ButtonType="Button" />
        </Fields>
    </asp:DetailsView>

    <asp:ValidationSummary ID="valSummary" runat="server" CssClass="msc-validation-summary"
        HeaderText="Please fix the following:" />
    <asp:Label ID="lblMessage" runat="server" CssClass="msc-message" Visible="false" />
</asp:Content>
