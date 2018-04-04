<%@ Page Title="Account Profile Confirmation" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProfileConfirm.aspx.cs" Inherits="BRBPortal_CSharp.Account.ProfileConfirm" %>
<%@ MasterType  virtualPath="~/Site.Master"%>

<%-- data-lpignore="true" tells LastPass not to show ellipsis on form fields --%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <hr />
    <section id="profileConfirmForm">
        <div class="form-horizontal">
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="UserIDCode0" CssClass="col-md-2 control-label">User ID: </asp:Label>
                <div class="col-md-10 literal">
                    <asp:Literal ID="UserIDCode0" runat="server" > </asp:Literal>
                </div>
            </div>        
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="BillCode0" CssClass="col-md-2 control-label">Billing Code: </asp:Label>
                <div class="col-md-10 literal">
                    <asp:Literal ID="BillCode0" runat="server"></asp:Literal>
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="FullName0" CssClass="col-md-2 control-label">Full Name: </asp:Label>
                <div class="col-md-10 literal">
                    <asp:Literal ID="FullName0" runat="server"></asp:Literal>
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="MailAddress0" CssClass="col-md-2 control-label">Mailing Address: </asp:Label>
                <div class="col-md-10 literal">
                    <asp:Literal ID="MailAddress0" runat="server"></asp:Literal>
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="EmailAddress0" CssClass="col-md-2 control-label">Email Address: </asp:Label>
                <div class="col-md-10 literal">
                    <asp:Literal ID="EmailAddress0" runat="server"></asp:Literal>
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="PhoneNo0" CssClass="col-md-2 control-label">Phone Number: </asp:Label>
                <div class="col-md-10 literal">
                    <asp:Literal ID="PhoneNo0" runat="server"></asp:Literal>
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="Quest1" CssClass="col-md-2 control-label">* Security Question:</asp:Label>
                <div class="col-md-10" style="max-width:43rem;">
                    <asp:dropdownlist runat="server" ID="Quest1" ToolTip="Select a question from the list." CssClass="form-control selectpicker">
                        <asp:ListItem Enabled="false" Text="select one" Value=""></asp:ListItem>
                        <asp:ListItem enabled="true" text="What was your favorite sport in high school?" value="What was your favorite sport in high school?"></asp:ListItem>
                        <asp:ListItem enabled="true" text="What was your favorite food as a child?" value="What was your favorite food as a child?"></asp:ListItem>
                        <asp:ListItem enabled="true" text="What is your favorite movie?" value="What is your favorite movie?"></asp:ListItem>
                        <asp:ListItem enabled="true" text="In what town was your first job?" value="In what town was your first job?"></asp:ListItem>
                        <asp:ListItem enabled="true" text="What was the make and model of your first car?" value="What was the make and model of your first car?"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="Answer1" CssClass="col-md-2 control-label">* Security Answer:</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="Answer1" CssClass="form-control" TextMode="SingleLine" style="width:40rem; max-width:40rem;" data-lpignore="true" />
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="Quest2" CssClass="col-md-2 control-label">* Security Question:</asp:Label>
                <div class="col-md-10" style="max-width:43rem;">
                    <asp:dropdownlist runat="server" ID="Quest2" ToolTip="Select a question from the list." CssClass="form-control selectpicker">
                        <asp:ListItem Enabled="false" Text="select one" Value=""></asp:ListItem>
                        <asp:ListItem enabled="true" text="What was your favorite sport in high school?" value="What was your favorite sport in high school?"></asp:ListItem>
                        <asp:ListItem enabled="true" text="What was your favorite food as a child?" value="What was your favorite food as a child?"></asp:ListItem>
                        <asp:ListItem enabled="true" text="What is your favorite movie?" value="What is your favorite movie?"></asp:ListItem>
                        <asp:ListItem enabled="true" text="In what town was your first job?" value="In what town was your first job?"></asp:ListItem>
                        <asp:ListItem enabled="true" text="What was the make and model of your first car?" value="What was the make and model of your first car?"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="Answer2" CssClass="col-md-2 control-label">* Security Answer:</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="Answer2" CssClass="form-control" TextMode="SingleLine" style="width:40rem; max-width:40rem;" data-lpignore="true" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:CheckBox ID="chkDeclare" runat="server" Text="Declaration: I hereby declare under penalty of perjury that .." CssClass="checkbox bold" />
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="DeclareInits" CssClass="col-md-2 control-label">Declaration initials: </asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="DeclareInits" Width="70px" CssClass="form-control" ToolTip="Enter your initials acknowledging the Declaration above." />
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:Button runat="server" ID="btnCancel" OnClick="CancelProfile_Click" Text="Cancel & Logout" CssClass="btn btn-danger" ToolTip="Clicking this button will cause this validation screen to be displayed on your next login." UseSubmitBehavior="false" CausesValidation="false" TabIndex="-1" />
                    <asp:Button runat="server" ID="btnSubmit" OnClick="SubmitProfile_Click" Text="Submit" CssClass="btn btn-primary" ToolTip="Click to confirm this information is correct." style="margin-left:1rem;" />
                </div>
            </div>
        </div>
    </section>

    <script>
        function _enableSubmitButton() {
            var question1 = $('#<%:Quest1.ClientID%>').val();
            var question2 = $('#<%:Quest2.ClientID%>').val();

            var hasQuestion1 = question1.length;
            var hasAnswer1 = $('#<%:Answer1.ClientID%>').val().length;
            var hasQuestion2 = question2.length;
            var hasAnswer2 = $('#<%:Answer2.ClientID%>').val().length;
            var isChecked = $('#<%:chkDeclare.ClientID%>').is(':checked');
            var hasInitials = $('#<%:DeclareInits.ClientID%>').val().length;

            if (question1 === question2) {
                $('#<%:Quest2.ClientID%>').val('');
                showErrorModal("Security Questions cannot be the same.", "Validation Error");
            } else {
                $('#<%:btnSubmit.ClientID%>').attr('disabled', (hasQuestion1 && hasAnswer1 && hasQuestion2 && hasAnswer2 && isChecked && hasInitials) ? false : true);
            }
        }

        $(document).ready(function () {
            $('#<%:Quest1.ClientID%>').change(_enableSubmitButton);
            $('#<%:Answer1.ClientID%>').change(_enableSubmitButton);
            $('#<%:Quest2.ClientID%>').change(_enableSubmitButton);
            $('#<%:Answer2.ClientID%>').change(_enableSubmitButton);
            $('#<%:chkDeclare.ClientID%>').change(_enableSubmitButton);
            $('#<%:DeclareInits.ClientID%>').change(_enableSubmitButton);
        });
    </script>
</asp:Content>
