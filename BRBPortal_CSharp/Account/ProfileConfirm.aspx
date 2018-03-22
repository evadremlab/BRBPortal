﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProfileConfirm.aspx.cs" Inherits="BRBPortal_CSharp.Account.ProfileConfirm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hfDialogID" runat="server" />
    <h2>Account Profile Confirmation</h2>
    <section id="profileConfirmForm">
        <div class="form-horizontal">
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
                <asp:CheckBox ID="chkDeclare" runat="server" Text="Declaration: I hereby declare under penalty of perjury that .." CssClass="checkbox bold" />
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="DeclareInits" CssClass="control-label">Declaration initials: </asp:Label>
                <asp:TextBox runat="server" ID="DeclareInits" Width="70px" CssClass="form-control" ToolTip="Enter your initials acknowledging the Declaration above." />
            </div>
            <div class="form-group">
                <asp:Button runat="server" ID="btnCancel" OnClick="CancelProfile_Click" Text="Cancel & Logout" CssClass="btn btn-danger" ToolTip="Clicking this button will cause this validation screen to be displayed on your next login." UseSubmitBehavior="false" />
                <asp:Button runat="server" ID="btnSubmit" OnClick="SubmitProfile_Click" Text="Submit" CssClass="btn btn-primary" ToolTip="Click to confirm this information is correct." style="margin-left:1rem;" />
            </div>
        </div>
    </section>

    <script>
        $(document).ready(function () {
            $('#MainContent_chkDeclare, #MainContent_DeclareInits').change(function () {
                var declareInits = $('#MainContent_DeclareInits').val().trim();
                var declareChecked = $('#MainContent_chkDeclare').prop('checked');
                var enableSubmit = declareInits && declareChecked;
                $('#MainContent_btnSubmit').prop('disabled', !enableSubmit);
                setTimeout(function () { // wait until submit is enabled
                    if (enableSubmit) {
                        $('#MainContent_btnSubmit').focus();
                    }
                }, 10);
            });
        });
    </script>
</asp:Content>
