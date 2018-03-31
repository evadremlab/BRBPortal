<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BRBPortal_CSharp.Account.Login" Async="true" %>
<%@ MasterType  virtualPath="~/Site.Master"%>

<%-- data-lpignore="true" tells LastPass not to show ellipsis on form fields --%>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <style>
        .navbar-nav, .navbar-right { display: none; }
    </style>
    <h2><%: Title %></h2>
    <hr />
    <section id="loginForm">
        <div class="form-horizontal">
            <div id="TemporaryPasswordMsg" runat="server" class="alert alert-info alert-dismissible" role="alert" style="width:62rem;">
                <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                A temporary new password has been sent to your email address registered with this account. Please access your account with your UserId and new temporary password.
            </div>
            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <div class="radio radiobuttonlist" style="padding-top:0; padding-left:0;">
                        <asp:RadioButtonList runat="server" ID="UserIDOrBillCode" RepeatDirection="Horizontal" ToolTip="Do you have a User ID or Billing Code?">
                            <asp:ListItem Enabled="true" Text="User ID&nbsp;&nbsp;" Value="UserID" Selected="True"></asp:ListItem>
                            <asp:ListItem Enabled="true" Text="Billing Code" Value="BillingCode"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>    
            </div>

            <div id="UserIDGrp" runat="server">
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="UserIDCode" CssClass="col-md-2 control-label">User ID:</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="UserIDCode" CssClass="form-control" TextMode="SingleLine" autofocus="autofocus" />
                    </div>
                </div>
            </div>

            <div id="BillCodeGrp" runat="server" style="display:none;">
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="BillCode" CssClass="col-md-2 control-label" data-lpignore="true">Billing Code:</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="BillCode" CssClass="form-control" TextMode="SingleLine" />
                    </div>
                </div>
            </div>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-2 control-label">Password</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="required." />
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
    <script>
        $(document).ready(function () {
            $('input[type="radio"]').change(function () {
                if ($(this).val() === 'UserID') {
                    $('#MainContent_UserIDGrp').show();
                    $('#MainContent_BillCodeGrp').hide();
                    $('#MainContent_BillCode').val('');
                    $('#MainContent_UserCode').focus();
                } else {
                    $('#MainContent_UserIDGrp').hide();
                    $('#MainContent_BillCodeGrp').show();
                    $('#MainContent_UserCode').val('');
                    $('#MainContent_BillCode').focus();
                }
            });
        });
    </script>
</asp:Content>
