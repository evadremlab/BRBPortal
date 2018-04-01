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

        <div class="form-horizontal">
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="CurrentPassword" CssClass="col-md-2 control-label">Current password:</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="CurrentPassword" TextMode="SingleLine" CssClass="form-control" data-lpignore="true" />
                </div>
            </div>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="NewPWD" CssClass="col-md-2 control-label">New password:</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="NewPWD" TextMode="SingleLine" CssClass="form-control" xxx-pattern="(?=.*[0-9])(?=.*[!@#$%^&_*])[a-zA-Z0-9!@#$%^&_*]{7,20}" title="Must contain at least one number, one letter, one symbol (!@#$%^&_*) and be 7-20 characters and not contain part of you user id" data-lpignore="true" />
                </div>
            </div>
        
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="ConfirmNewPassword" CssClass="col-md-2 control-label">Confirm new password:</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="ConfirmNewPassword" TextMode="SingleLine" CssClass="form-control" xxx-pattern="(?=.*[0-9])(?=.*[!@#$%^&_*])[a-zA-Z0-9!@#$%^&_*]{7,20}" title="Must contain at least one number, one letter, one symbol (!@#$%^&_*) and be 7-20 characters and not contain part of you user id" data-lpignore="true" />
                </div>
            </div>
        
            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:Button runat="server" id="btnBack" UseSubmitBehavior="false" PostBackUrl="~/Account/ProfileList" CausesValidation="false" Text="Cancel" CssClass="btn btn-sm btn-default" ToolTip="Return to Home page." TabIndex="-1" />
                    <asp:Button runat="server" ID="btnSubmit" Text="Submit" CssClass="btn btn-primary" style="margin-left:1rem;" />
                </div>
            </div>
        </div>
    </section>
    <script>
        function _enableSubmitButton() {
            var currentPassword = $('#<%:CurrentPassword.ClientID%>').val();
            var newPassword = $('#<%:NewPWD.ClientID%>').val();
            var confirmPassword = $('#<%:ConfirmNewPassword.ClientID%>').val();

            var hasCurrentPassword = currentPassword.length;
            var hasNewPassword = newPassword.length;
            var hasConfirmPassword = confirmPassword.length;

            if (newPassword.toUpperCase() === confirmPassword.toUpperCase()) {
                $('#<%:ConfirmNewPassword.ClientID%>').val('');
                showErrorModal("New and Confirm Passwords cannot be the same.", "Validation Error");
            } else if (newPassword.toUpperCase() === currentPassword.toUpperCase()) {
                $('#<%:NewPWD.ClientID%>').val('');
                showErrorModal("New and Current Passwords cannot be the same.", "Validation Error");
            } else {
                $('#<%:btnSubmit.ClientID%>').attr('disabled', (hasQuestion1 && hasAnswer1 && hasQuestion2 && hasAnswer2 && isChecked && hasInitials) ? false : true);
            }
        }

        $(document).ready(function () {
            $('#<%:btnSubmit.ClientID%>').attr('disabled', true); // initial state

            $('#<%:CurrentPassword.ClientID%>').change(_enableSubmitButton);
            $('#<%:NewPWD.ClientID%>').change(_enableSubmitButton);
            $('#<%:ConfirmNewPassword.ClientID%>').change(_enableSubmitButton);
        });
    </script>
</asp:Content>
