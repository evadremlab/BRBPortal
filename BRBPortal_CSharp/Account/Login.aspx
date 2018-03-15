<%@ Page Title="Log in" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BRBPortal_CSharp.Account.Login" Async="true" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<%--data-lpignore="false" tells LastPass not to show ellipsis on form fields--%>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <style>
        .form-control { 
            display: inline-block; 
            width: 15rem; 
        }
    </style>
    <h2><%: Title %>.</h2>
    <div class="row" style="margin-top:1rem;">
        <div class="col-md-8">
            <section id="loginForm">
                <div class="form-horizontal">
                    <h4>To your account</h4>
                    <hr />
                    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
                    </asp:PlaceHolder>
                    <div class="form-group">
                        <%--<asp:Label runat="server" AssociatedControlID="Email" CssClass="col-md-3 control-label">Email</asp:Label>--%>
                        <%--<div class="col-md-9">
                            <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Email" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                                CssClass="text-danger" ErrorMessage="The email field is required." />
                        </div>--%>
                        <asp:Label runat="server" AssociatedControlID="UserIDCode" CssClass="col-md-3 control-label">User ID:</asp:Label>
                        <div class="col-md-9">
                            <asp:TextBox runat="server" ID="UserIDCode" CssClass="form-control" TextMode="SingleLine" data-lpignore="true" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="BillCode" CssClass="col-md-3 control-label">Billing Code:</asp:Label>
                        <div class="col-md-9">
                            <asp:TextBox runat="server" ID="BillCode" CssClass="form-control" TextMode="SingleLine" data-lpignore="true" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-3 control-label">Password</asp:Label>
                        <div class="col-md-9">
                            <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" data-lpignore="true" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="The password field is required." />
                        </div>
                    </div>
                    <div class="form-group hidden">
                        <div class="col-md-offset-3 col-md-9">
                            <div class="checkbox">
                                <asp:CheckBox runat="server" ID="RememberMe" />
                                <asp:Label runat="server" AssociatedControlID="RememberMe">Remember me?</asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-3 col-md-9">
                            <asp:Button runat="server" OnClick="LogIn" Text="Log in" CssClass="btn btn-default" />
                        </div>
                    </div>
                </div>
                <p class="hidden">
                    <asp:hyperlink runat="server" id="registerhyperlink" viewstatemode="disabled">register as a new user</asp:hyperlink>
                </p>
                <p>
                    <%-- enable this once you have account confirmation enabled for password reset functionality--%>
                    <asp:HyperLink runat="server" NavigateUrl="~/Account/ResetPassword" ID="ForgotPasswordHyperLink" ViewStateMode="Disabled">Forgot your password?</asp:HyperLink>
                </p>
            </section>
        </div>

       <%-- <div class="col-md-4">
            <section id="socialLoginForm">
                <uc:OpenAuthProviders runat="server" ID="OpenAuthLogin" />
            </section>
        </div>--%>
    </div>
</asp:Content>
