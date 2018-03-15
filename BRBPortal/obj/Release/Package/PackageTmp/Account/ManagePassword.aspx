<%@ Page Title="Manage Password" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="ManagePassword.aspx.vb" Inherits="BRBPortal.ManagePassword" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%--<h2><%: Title %>.</h2>--%>

    <link rel="stylesheet" type="text/css" href="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/themes/start/jquery-ui.css" />
    <link rel="stylesheet" type="text/css" href="../Content/bootstrap.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/StyleBRB.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/Baseline.Type.css" />
    
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/ClientPortal.js") %>"> </script>
    <!-- Modal Dialog Box Ajax Code -->
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script src="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/jquery-ui.js" type="text/javascript"></script>

<%--    <script type="text/javascript">
        $(document).ready(
        function CheckPassword(inputtxt) {
            var paswd = /^(?=.*[0-9])(?=.*[!@#$%^&_*])[a-zA-Z0-9!@#$%^&_*]{7,20}$/;
            if (inputtxt.value.match(paswd)) {
                //alert('Correct, try another...')
                return true;
            }
            else {
                alert('Passwords must be 7-20 characters long, contain at least one digit (0-9) and at least one symbol (!@#$%^&_*) and not contain part of you user id.')
                return false;
            }
        })
    </script>--%>
    <div class="form-horizontal" style="width: 809px; height: 500px; padding-left:10px" >
        <h4>Portal Update Password</h4>
        <%--<asp:ValidationSummary runat="server" ShowModelStateErrors="true" CssClass="text-danger" />--%>
            
        <div class="form-group" style="padding-left:10px">
            <asp:Label runat="server" Width="180px" AssociatedControlID="CurrentPassword" CssClass="col-md-1 control-label">Current password</asp:Label>
            <asp:TextBox runat="server" ID="CurrentPassword" TextMode="Password" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="CurrentPassword"
                     CssClass="text-danger" ErrorMessage="The current password field is required." ValidationGroup="ChangePassword" />
        </div>

        <div class="form-group" style="padding-left:10px">
            <asp:Label runat="server" Width="180px" AssociatedControlID="NewPWD" CssClass="col-md-1 control-label">New password</asp:Label>
            <input type="password" id="NewPWD" maxlength="20" runat="server" required="required" pattern="(?=.*[0-9])(?=.*[!@#$%^&_*])[a-zA-Z0-9!@#$%^&_*]{7,20}" 
                title="Must contain at least one number, one letter, one symbol (!@#$%^&_*) and be 7-20 characters and not contain part of you user id" 
                onfocusout="CheckPassword(NewPWD.value)" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="NewPWD"
                     CssClass="text-danger" ErrorMessage="The new password is required." ValidationGroup="ChangePassword" />
        </div>
        
        <div class="form-group" style="padding-left:10px">
            <asp:Label runat="server" Width="180px" AssociatedControlID="ConfirmNewPassword" CssClass="col-md-1 control-label">Confirm new password</asp:Label>
            <asp:TextBox runat="server" ID="ConfirmNewPassword" TextMode="Password" pattern="(?=.*[0-9])(?=.*[!@#$%^&_*])[a-zA-Z0-9!@#$%^&_*]{7,20}" 
                title="Must contain at least one number, one letter, one symbol (!@#$%^&_*) and be 7-20 characters and not contain part of you user id" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmNewPassword" CssClass="text-danger" 
                    Display="Dynamic" ErrorMessage="Confirm new password is required." ValidationGroup="ChangePassword" />

            <asp:CompareValidator runat="server" ControlToCompare="NewPWD" ControlToValidate="ConfirmNewPassword" CssClass="text-danger" 
                    Display="Dynamic" ErrorMessage="The new password and confirmation password do not match." ValidationGroup="ChangePassword" />
        </div>
        
        <br />
        <div class="form-group" style="padding:15px">
            <asp:Button runat="server" Text="Submit" ValidationGroup="ChangePassword" OnClick="ChangePassword_Click" CssClass="btn-default active" />
        </div>
        <%--<br />--%>

        <asp:Button ID="btnDialogResponseYes" runat="server" Text="Hidden Button" OnClick="DialogResponseYes"
            Style="display: none" UseSubmitBehavior="false" />
        <asp:Button ID="btnDialogResponseNo" runat="server" Text="Hidden Button" OnClick="DialogResponseNo"
            Style="display: none" UseSubmitBehavior="false" />
        <div id="dialog2" style="display: none">
        </div>
        <asp:HiddenField ID="hfDialogID" runat="server" />
        <asp:HiddenField ID="hfNextPage" runat="server" ValidateRequestMode="Disabled" />
    </div>
</asp:Content>
