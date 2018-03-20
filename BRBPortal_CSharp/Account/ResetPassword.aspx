<%@ Page Title="Reset Password" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="BRBPortal_CSharp.Account.ResetPassword" Async="true" %>

<%-- data-lpignore="true" tells LastPass not to show ellipsis on form fields --%>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %>.</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>

    <div class="form-horizontal">
        <asp:HiddenField ID="hfDialogID" runat="server" />
        <h4>Login Password Reset</h4>
        <hr />
            <asp:PlaceHolder runat="server" ID="ErrMessage" Visible="false">
                <div class="alert alert-danger" role="alert">
                    <asp:Literal runat="server" ID="FailureText" />
                </div>
            </asp:PlaceHolder>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="UserIDCode" CssClass="col-md-2 control-label">User ID:</asp:Label>
            <asp:TextBox runat="server" ID="UserIDCode" TextMode="SingleLine" OnTextChanged="UserIDCode_TextChanged" AutoPostBack="true" />
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="BillingCode" CssClass="col-md-2 control-label">Billing Code:</asp:Label>
            <asp:TextBox runat="server" ID="BillingCode" TextMode="SingleLine" OnTextChanged="BillingCode_TextChanged" AutoPostBack="true" data-lpignore="true" />
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Quest1" CssClass="col-md-2 control-label">Security Question:</asp:Label>
            <asp:literal runat="server" id="Quest1"></asp:literal>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Answer1" CssClass="col-md-2 control-label">Security Answer:</asp:Label>
            <asp:TextBox runat="server" ID="Answer1" TextMode="SingleLine" />
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Quest2" CssClass="col-md-2 control-label">Security Question:</asp:Label>
            <asp:literal runat="server" id="Quest2"></asp:literal>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Answer2" CssClass="col-md-2 control-label">Security Answer:</asp:Label>
            <asp:TextBox runat="server" ID="Answer2" TextMode="SingleLine" />
        </div>
        <div class="form-group">
            <asp:Button runat="server" ID="btnResetPWD" OnClick="Reset_Click" Text="Reset Password" CssClass="btn btn-primary" />
        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button runat="server" OnClick="Reset_Click" Text="Reset" CssClass="btn btn-default" />
            </div>
        </div>
    </div>
</asp:Content>
