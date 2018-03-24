<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BRBPortal_CSharp.Account.Login" Async="true" %>

<%-- data-lpignore="true" tells LastPass not to show ellipsis on form fields --%>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <style>
        .navbar-nav, .navbar-right { display: none; }
    </style>
    <h2><%: Title %></h2>
    <section id="loginForm">
        <div class="form-horizontal">
            <hr />
            <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                <div class="alert alert-danger" role="alert">
                    <asp:Literal runat="server" ID="FailureText" />
                </div>
            </asp:PlaceHolder>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="UserIDCode" CssClass="col-md-2 control-label">User ID:</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="UserIDCode" CssClass="form-control" TextMode="SingleLine" autofocus="autofocus" />
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="BillCode" CssClass="col-md-2 control-label" data-lpignore="true">Billing Code:</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="BillCode" CssClass="form-control" TextMode="SingleLine" />
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-2 control-label">Password</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="password is required." />
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:Button runat="server" id="btnBack" UseSubmitBehavior="false" PostBackUrl="~/Default" Text="Cancel" CssClass="btn btn-sm btn-default" ToolTip="Return to Home page." TabIndex="-1" />
                    <asp:Button runat="server" OnClick="LogIn" Text="Login" CssClass="btn btn-primary" style="margin-left:1rem;" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:HyperLink runat="server" ID="ForgotPasswordHyperLink" NavigateUrl="~/Account/ResetPassword" ViewStateMode="Disabled">Forgot your password?</asp:HyperLink>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
