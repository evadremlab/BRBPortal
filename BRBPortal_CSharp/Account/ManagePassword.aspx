<%@ Page Title="Update Password" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManagePassword.aspx.cs" Inherits="BRBPortal_CSharp.Account.ManagePassword" %>
<%@ MasterType  virtualPath="~/Site.Master"%>

<%--data-lpignore="true" tells LastPass not to show ellipsis on form fields--%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .navbar-nav, .navbar-right { display: none; }
    </style>

    <h2><%: Title %></h2>
    <hr />

    <section id="managePasswordForm">
        <asp:HiddenField ID="hfNextPage" runat="server" ValidateRequestMode="Disabled" />
        <asp:HiddenField ID="hdnPassword" runat="server" ValidateRequestMode="Disabled" />

        <div class="form-horizontal">
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="NewPWD" CssClass="col-md-2 control-label">New password:</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="NewPWD" TextMode="Password" CssClass="form-control" data-lpignore="true" />
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="ConfirmNewPassword" CssClass="col-md-2 control-label">Confirm new password:</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="ConfirmNewPassword" TextMode="Password" CssClass="form-control" data-lpignore="true" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:Button runat="server" id="btnBack" UseSubmitBehavior="false" PostBackUrl="~/Account/ProfileList" CausesValidation="false" Text="Cancel" CssClass="btn btn-sm btn-default" ToolTip="Return to Home page." TabIndex="-1" />
                    <asp:Button runat="server" ID="btnSubmit" Text="Submit" OnClick="btnSubmit_Click" OnClientClick="return validate();" CssClass="btn btn-primary" style="margin-left:1rem;" />
                </div>
            </div>
        </div>
    </section>
    <script>
        var valErrors = [];

        function addValError(msg) {
            valErrors.push('<li>' + msg + '</li>');
        }

        function validate() {
            try {
                var currentPassword = $('#<%:hdnPassword.ClientID%>').val();
                var newPassword = $('#<%:NewPWD.ClientID%>').val();
                var confirmPassword = $('#<%:ConfirmNewPassword.ClientID%>').val();

                var hasCurrentPassword = currentPassword.length;
                var hasNewPassword = newPassword.length;
                var hasConfirmPassword = confirmPassword.length;

                valErrors = [];

                if (hasCurrentPassword && hasNewPassword && hasConfirmPassword) {
                    if (newPassword.toUpperCase() !== confirmPassword.toUpperCase()) {
                        addValError('New and Confirm Passwords must be the same.');
                    } else if (newPassword.toUpperCase() === currentPassword.toUpperCase()) {
                        addValError('New and Current Passwords cannot be the same.');
                    }
                } else {
                    if (!hasCurrentPassword) {
                        addValError('Current Password is required.');
                    }
                    if (!hasNewPassword) {
                        addValError('New Password is required.');
                    }
                    if (!hasConfirmPassword) {
                        addValError('Confirm Password is required.');
                    }
                }

                if (valErrors.length) {
                    showErrorModal(('<ul>' + valErrors.join('') + '</ul>'), "Validation Errors");
                    return false;
                } else {
                    return true;
                }
            } catch (ex) {
                showErrorModal(ex.message, "Validation Errors");
                return false;
            }
        }
    </script>
</asp:Content>
