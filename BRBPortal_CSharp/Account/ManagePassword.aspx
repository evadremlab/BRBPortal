<%@ Page Title="Manage Password" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManagePassword.aspx.cs" Inherits="BRBPortal_CSharp.Account.ManagePassword" %>

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
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="CurrentPassword" CssClass="text-danger" ErrorMessage="The current password field is required." ValidationGroup="ChangePassword" />
                </div>
            </div>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="NewPWD" CssClass="col-md-2 control-label">New password:</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="NewPWD" TextMode="Password" CssClass="form-control" xxx-pattern="(?=.*[0-9])(?=.*[!@#$%^&_*])[a-zA-Z0-9!@#$%^&_*]{7,20}" title="Must contain at least one number, one letter, one symbol (!@#$%^&_*) and be 7-20 characters and not contain part of you user id" data-lpignore="true" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="NewPWD" CssClass="text-danger" ErrorMessage="The new password is required." ValidationGroup="ChangePassword" />
                </div>
            </div>
        
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="ConfirmNewPassword" CssClass="col-md-2 control-label">Confirm new password:</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="ConfirmNewPassword" TextMode="Password" CssClass="form-control" xxx-pattern="(?=.*[0-9])(?=.*[!@#$%^&_*])[a-zA-Z0-9!@#$%^&_*]{7,20}" title="Must contain at least one number, one letter, one symbol (!@#$%^&_*) and be 7-20 characters and not contain part of you user id" data-lpignore="true" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmNewPassword" CssClass="text-danger" Display="Dynamic" ErrorMessage="Confirm new password is required." ValidationGroup="ChangePassword" />
                    <asp:CompareValidator runat="server" ControlToCompare="NewPWD" ControlToValidate="ConfirmNewPassword" CssClass="text-danger" Display="Dynamic" ErrorMessage="The new password and confirmation password do not match." ValidationGroup="ChangePassword" />
                </div>
            </div>
        
            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:Button runat="server" Text="Submit" ValidationGroup="ChangePassword" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
    </section>
</asp:Content>
