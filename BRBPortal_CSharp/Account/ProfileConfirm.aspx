<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProfileConfirm.aspx.cs" Inherits="BRBPortal_CSharp.Account.ProfileConfirm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%--<link rel="stylesheet" type="text/css" href="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/themes/start/jquery-ui.css" />--%>
    <link rel="stylesheet" type="text/css" href="../Content/bootstrap.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/StyleBRB.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/Baseline.Type.css" />
    <style>
        .form-control { 
            display: inline-block; 
            width: 15rem; 
        }
    </style>
        
    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundle/jqueryui") %>
    </asp:PlaceHolder>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/ClientPortal.js") %>"> </script>
    <!-- Modal Dialog Box Ajax Code -->
    <%--<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>--%>
    <%--<script src="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/jquery-ui.js" type="text/javascript"></script>--%>

    <asp:HiddenField ID="hfDialogID" runat="server" />

    <div class="row" style="margin-top:1rem;">
        <div class="col-md-8">
            <div class="form-horizontal">
                <h4>Account Profile Confirmation</h4>
                <hr />

                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="UserIDCode0" CssClass="control-label">User ID: </asp:Label>
                    <asp:Literal ID="UserIDCode0" runat="server" > </asp:Literal>
                </div>
        
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="BillCode0" CssClass="control-label">Billing Code: </asp:Label>
                    <asp:Literal ID="BillCode0" runat="server" > </asp:Literal>
                </div>
        
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="FullName0" CssClass="control-label">Name (First, Middle, Last, and suffix): </asp:Label>
                    <asp:Literal ID="FullName0" runat="server" > </asp:Literal>
                </div>
        
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="MailAddress0" CssClass="control-label">Mailing Address: </asp:Label>
                    <asp:Literal ID="MailAddress0" runat="server" > </asp:Literal>
                </div>
        
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="EmailAddress0" CssClass="control-label">Email Address: </asp:Label>
                    <asp:Literal ID="EmailAddress0" runat="server" > </asp:Literal>
                </div>
        
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="PhoneNo0" CssClass="control-label">Phone Number: </asp:Label>
                    <asp:Literal ID="PhoneNo0" runat="server" > </asp:Literal>
                </div>

                <div class="form-group">
                    <asp:CheckBox ID="chkDeclare" runat="server" Text="&nbsp;Declaration:  I hereby declare under penalty of perjury that .." AutoPostBack="True" />
                </div>
        
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="DeclareInits" CssClass="control-label">Declaration initials: </asp:Label>
                    <asp:TextBox runat="server" ID="DeclareInits" Width="70px" ToolTip="Enter your initials acknowledging the Declaration above." AutoPostBack="True" />
                </div>

                <div class="form-group">
                    <asp:Button runat="server" ID="btnSubmit" OnClick="SubmitProfile_Click" Text="Submit" CssClass="btn btn-primary" ToolTip="Click to confirm this information is correct." />
                    <asp:Button runat="server" ID="btnCancel" OnClick="CancelProfile_Click" Text="Cancel & Logout" CssClass="btn btn-default" ToolTip="Clicking this button will cause this validation screen to be displayed on your next login." UseSubmitBehavior="false" />
                </div>
                
                <asp:Button ID="btnDialogResponseYes" runat="server" Text="Hidden Button" OnClick="DialogResponseYes"
                    Style="display: none" UseSubmitBehavior="false" />
                <asp:Button ID="btnDialogResponseNo" runat="server" Text="Hidden Button" OnClick="DialogResponseNo"
                    Style="display: none" UseSubmitBehavior="false" />
                <div id="dialog2" style="display: none;"></div>
            </div>
        </div>
    </div>
</asp:Content>
