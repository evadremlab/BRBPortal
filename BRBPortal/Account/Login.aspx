<%@ Page Title="Log in" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Login.aspx.vb" Inherits="BRBPortal.Login" Async="true" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <%--  <br />--%>
    
    <link rel="stylesheet" type="text/css" href="/Content/bootstrap.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/StyleBRB.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/Baseline.Type.css" />


    <div class="form-horizontal" style="width: 825px; height: 500px; padding-left:15px">
        <h4>Login</h4>
        <%--<hr />--%>
        <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
            <p class="text-danger">
                <asp:Literal runat="server" ID="FailureText" />
            </p>
        </asp:PlaceHolder>
        
        <div class="form-group" style="padding-left:10px; width:600px" >
            <asp:Label runat="server" AssociatedControlID="UserIDCode" CssClass="col-md-1 control-label" Width="120px" >User ID:</asp:Label>
            <asp:TextBox runat="server" ID="UserIDCode" TextMode="SingleLine" Width="150px" />
            <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="BillCode" CssClass="text-danger" ErrorMessage="The User ID/Billing Code field is required." />--%>
        </div>

        <div class="form-group" style="padding-left:10px; width:600px">
            <asp:Label runat="server" AssociatedControlID="BillCode" CssClass="col-md-1 control-label" Width="120px" >Billing Code:</asp:Label>
            <asp:TextBox runat="server" ID="BillCode" TextMode="SingleLine" Width="150px" />
            <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="BillCode" CssClass="text-danger" ErrorMessage="The User ID/Billing Code field is required." />--%>
        </div>

        <div class="form-group row" style="padding-left:10px; width:600px">
            <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-1 control-label" Width="120px" >Password:</asp:Label>
            <asp:TextBox runat="server" ID="Password" TextMode="Password" Width="150px" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="The password field is required." />
        </div>
        
        <br />
        <p>
            <%-- Enable this once you have account confirmation enabled for password reset functionality --%>
            <asp:HyperLink runat="server" NavigateUrl="/Account/ResetPassword" ID="ForgotPasswordHyperLink" style="padding-left:10px"
                ViewStateMode="Disabled">Forgot your password?</asp:HyperLink>
        </p>

        <br />
        <div class="btn-group" style="padding-left:10px">
            <asp:Button runat="server" OnClick="LogIn" Text="Submit" CssClass="btn-default active" ID="btnLogin" />
        </div>
    </div>

</asp:Content>

