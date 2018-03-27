<%@ Page Title="Manage Password" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManagePassword.aspx.cs" Inherits="BRBPortal_CSharp.Account.ManagePassword" %>
<%@ MasterType  virtualPath="~/Site.Master"%>

<%--data-lpignore="true" tells LastPass not to show ellipsis on form fields--%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <section id="managePasswordForm">

        <asp:HiddenField ID="hfDialogID" runat="server" />
        <asp:HiddenField ID="hfNextPage" runat="server" ValidateRequestMode="Disabled" />

        <div class="form-horizontal">
            <hr />
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="CurrentPassword" CssClass="col-md-2 control-label">Current password:</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="CurrentPassword" TextMode="Password" CssClass="form-control" data-lpignore="true" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="CurrentPassword" CssClass="text-danger" ErrorMessage="required" />
                </div>
            </div>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="NewPWD" CssClass="col-md-2 control-label">New password:</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="NewPWD" TextMode="Password" CssClass="form-control" xxx-pattern="(?=.*[0-9])(?=.*[!@#$%^&_*])[a-zA-Z0-9!@#$%^&_*]{7,20}" title="Must contain at least one number, one letter, one symbol (!@#$%^&_*) and be 7-20 characters and not contain part of you user id" data-lpignore="true" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="NewPWD" CssClass="text-danger" ErrorMessage="required" />
                </div>
            </div>
        
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="ConfirmNewPassword" CssClass="col-md-2 control-label">Confirm new password:</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="ConfirmNewPassword" TextMode="Password" CssClass="form-control" xxx-pattern="(?=.*[0-9])(?=.*[!@#$%^&_*])[a-zA-Z0-9!@#$%^&_*]{7,20}" title="Must contain at least one number, one letter, one symbol (!@#$%^&_*) and be 7-20 characters and not contain part of you user id" data-lpignore="true" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmNewPassword" CssClass="text-danger" Display="Dynamic" ErrorMessage="required" />
                    <asp:CompareValidator runat="server" ControlToCompare="NewPWD" ControlToValidate="ConfirmNewPassword" CssClass="text-danger" Display="Dynamic" ErrorMessage="New and Confirmation Password do not match." />
                </div>
            </div>
        
            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:Button runat="server" id="btnBack" UseSubmitBehavior="false" PostBackUrl="~/Home" CausesValidation="false" Text="Cancel" CssClass="btn btn-sm btn-default" ToolTip="Return to Home page." TabIndex="-1" />
                    <asp:Button runat="server" Text="Submit" CssClass="btn btn-primary" style="margin-left:1rem;" />
                </div>
            </div>
        </div>
    </section>
</asp:Content>
