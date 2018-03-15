<%@ Page Title="Reset Password" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.vb" Inherits="BRBPortal.ResetPassword" Async="true" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <%--<h2><%: Title %>.</h2>--%>

    <link rel="stylesheet" type="text/css" href="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/themes/start/jquery-ui.css" />
    <link rel="stylesheet" type="text/css" href="../Content/bootstrap.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/StyleBRB.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/Baseline.Type.css" />
    
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/ClientPortal.js") %>"> </script>
    <!-- Modal Dialog Box Ajax Code -->
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script src="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/jquery-ui.js" type="text/javascript"></script>

    <div class="form-horizontal" style="width: 809px; height: 500px; padding-left:10px">
        <%--<div style="height: 15px"></div>--%>
        <h4>Login Password Reset</h4>
        <asp:PlaceHolder runat="server" ID="ErrMessage" Visible="false">
            <p class="text-danger">
                <asp:Literal runat="server" ID="FailureText" />
            </p>
        </asp:PlaceHolder>
        
        <div class="form-group" style="padding-left:10px">
            <asp:Label runat="server" AssociatedControlID="UserIDCode" Width="140px" CssClass="col-md-1 control-label">User ID:</asp:Label>
            <asp:TextBox runat="server" ID="UserIDCode" TextMode="SingleLine" Width="150px" OnTextChanged="UserIDCode_TextChanged" AutoPostBack="true" />
            <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                    CssClass="text-danger" ErrorMessage="The User ID/Billing Code field is required." />--%>
        </div>

        <div class="form-group" style="padding-left:10px">
            <asp:Label runat="server" AssociatedControlID="BillingCode" Width="140px" CssClass="col-md-1 control-label">Billing Code:</asp:Label>
            <asp:TextBox runat="server" ID="BillingCode" TextMode="SingleLine" Width="150px" OnTextChanged="BillingCode_TextChanged" AutoPostBack="true" />
            <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                    CssClass="text-danger" ErrorMessage="The User ID/Billing Code field is required." />--%>
        </div>

        <br />
        <div class="form-group" style="padding-left:10px; height:110px">
            <asp:Label runat="server" AssociatedControlID="Quest1" Width="150px" CssClass="col-md-1 control-label">Security Question:</asp:Label>
            <asp:literal runat="server" id="Quest1"></asp:literal>
            <br />
            <asp:Label runat="server" AssociatedControlID="Answer1" Width="150px" CssClass="col-md-1 control-label">Security Answer:</asp:Label>
            <asp:TextBox runat="server" ID="Answer1" TextMode="Password" width="200px" />
            <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="SecurityAnswer"
                    CssClass="text-danger" ErrorMessage="The Security Answer field is required." />--%>
            <br /><br />
            <asp:Label runat="server" AssociatedControlID="Quest2" Width="150px" CssClass="col-md-1 control-label">Security Question:</asp:Label>
            <asp:literal runat="server" id="Quest2"></asp:literal>
            <br />
            <asp:Label runat="server" AssociatedControlID="Answer2" Width="150px" CssClass="col-md-1 control-label">Security Answer:</asp:Label>
            <asp:TextBox runat="server" ID="Answer2" TextMode="Password" width="200px" />
        </div>

        <br />
        <div class="btn-group" style="padding-left:10px">
            <asp:Button runat="server" ID="btnResetPWD" OnClick="Reset_Click" Text="Reset Password" CssClass="btn-default active" />
        </div>

        <asp:Button ID="btnDialogResponseYes" runat="server" Text="Hidden Button" OnClick="DialogResponseYes"
            Style="display: none" UseSubmitBehavior="false" />
        <asp:Button ID="btnDialogResponseNo" runat="server" Text="Hidden Button" OnClick="DialogResponseNo"
            Style="display: none" UseSubmitBehavior="false" />
        <div id="dialog2" style="display: none">
        </div>
        <asp:HiddenField ID="hfDialogID" runat="server" />
    </div>
</asp:Content>
