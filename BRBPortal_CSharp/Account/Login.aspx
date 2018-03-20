<%@ Page Title="Log in" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BRBPortal_CSharp.Account.Login" Async="true" %>

<%-- data-lpignore="true" tells LastPass not to show ellipsis on form fields --%>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %>.</h2>
    <section id="loginForm">
        <div class="form-horizontal">
            <h4>To your account</h4>
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
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="The password field is required." />
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:Button runat="server" OnClick="LogIn" Text="Log in" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
        <p>
            <asp:HyperLink runat="server" NavigateUrl="~/Account/ResetPassword" ID="ForgotPasswordHyperLink" ViewStateMode="Disabled">Forgot your password?</asp:HyperLink>
        </p>
    </section>
</asp:Content>
