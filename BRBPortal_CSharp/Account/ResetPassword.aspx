<%@ Page Title="Reset Password" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="BRBPortal_CSharp.Account.ResetPassword" Async="true" %>

<%-- data-lpignore="true" tells LastPass not to show ellipsis on form fields --%>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:HiddenField ID="hfDialogID" runat="server" />

    <h2><%: Title %></h2>

    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>

    <div class="form-horizontal">
        <hr />
        <asp:PlaceHolder runat="server" ID="ErrMessage" Visible="false">
            <div class="alert alert-danger" role="alert">
                <asp:Literal runat="server" ID="FailureText" />
            </div>
        </asp:PlaceHolder>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="UserIDCode" CssClass="col-md-2 control-label">User ID:</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="UserIDCode" CssClass="form-control" TextMode="SingleLine" OnTextChanged="UserIDCode_Or_BillingCode_TextChanged" AutoPostBack="true" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="BillingCode" CssClass="col-md-2 control-label">Billing Code:</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="BillingCode" CssClass="form-control" TextMode="SingleLine" OnTextChanged="UserIDCode_Or_BillingCode_TextChanged" AutoPostBack="true" data-lpignore="true" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Quest1" CssClass="col-md-2 control-label">Security Question:</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Quest1" CssClass="form-control" TextMode="SingleLine" ReadOnly="true" TabIndex="-1" style="width:40rem; max-width:50rem;" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Answer1" CssClass="col-md-2 control-label">Security Answer:</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Answer1" CssClass="form-control" TextMode="SingleLine" style="width:40rem; max-width:50rem;" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Quest2" CssClass="col-md-2 control-label">Security Question:</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Quest2" CssClass="form-control" TextMode="SingleLine" ReadOnly="true" TabIndex="-1" style="width:40rem; max-width:50rem;" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Answer2" CssClass="col-md-2 control-label">Security Answer:</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Answer2" CssClass="form-control" TextMode="SingleLine" style="width:40rem; max-width:50rem;" />
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button runat="server" id="btnBack" UseSubmitBehavior="false" PostBackUrl="~/Default" Text="Cancel" CssClass="btn btn-sm btn-default" ToolTip="Return to Home page." TabIndex="-1" />
                <asp:Button runat="server" ID="btnResetPWD" OnClick="Reset_Click" Text="Reset Password" CssClass="btn btn-primary" style="margin-left:1rem;" />
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            function toggleSubmitButton() {
                var userCode = $('#MainContent_UserIDCode').val().trim();
                var billingCode = $('#MainContent_BillingCode').val().trim();
                var answer1 = $('#MainContent_Answer1').val().trim();
                var answer2 = $('#MainContent_Answer2').val().trim();
                var enableSubmit = (userCode || billingCode) && answer1 && answer2;
                $('#MainContent_btnResetPWD').prop('disabled', !enableSubmit);
                setTimeout(function () { // wait until button is enabled
                    if (enableSubmit) {
                        $('#MainContent_btnResetPWD').focus();
                    }
                }, 10);
            }

            toggleSubmitButton();

            $('#MainContent_UserIDCode, #MainContent_BillingCode, #MainContent_Answer1, #MainContent_Answer2').change(toggleSubmitButton);
        });
    </script>
</asp:Content>
