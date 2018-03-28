<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProfileConfirm.aspx.cs" EnableEventValidation="false" Inherits="BRBPortal_CSharp.Account.ProfileConfirm" %>
<%@ MasterType  virtualPath="~/Site.Master"%>

<%-- data-lpignore="true" tells LastPass not to show ellipsis on form fields --%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hfDialogID" runat="server" />
    <h2>Account Profile Confirmation</h2>
    <section id="profileConfirmForm">
        <div class="form-horizontal">
            <hr />
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
                        <asp:ListItem text="Select a Question" Value=""></asp:ListItem>
                        <asp:ListItem enabled="true" text="What was your favorite sport in high school?" value="What was your favorite sport in high school?"></asp:ListItem>
                        <asp:ListItem enabled="true" text="What was your favorite food as a child?" value="What was your favorite food as a child?"></asp:ListItem>
                        <asp:ListItem enabled="true" text="What is your favorite movie?" value="What is your favorite movie?"></asp:ListItem>
                        <asp:ListItem enabled="true" text="In what town was your first job?" value="In what town was your first job?"></asp:ListItem>
                        <asp:ListItem enabled="true" text="What was the make and model of your first car?" value="What was the make and model of your first car?"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Quest1" IntialValue="" CssClass="text-danger" Display="Dynamic" ErrorMessage="required" />
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="Answer1" CssClass="col-md-2 control-label">* Security Answer:</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="Answer1" CssClass="form-control" TextMode="SingleLine" style="width:40rem; max-width:40rem;" data-lpignore="true" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Answer1" CssClass="text-danger" Display="Dynamic" ErrorMessage="required" />
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="Quest2" CssClass="col-md-2 control-label">* Security Question:</asp:Label>
                <div class="col-md-10" style="max-width:43rem;">
                    <asp:dropdownlist runat="server" ID="Quest2" ToolTip="Select a question from the list." CssClass="form-control selectpicker">
                        <asp:ListItem text="Select a Question" Value=""></asp:ListItem>
                        <asp:ListItem enabled="true" text="What was your favorite sport in high school?" value="What was your favorite sport in high school?"></asp:ListItem>
                        <asp:ListItem enabled="true" text="What was your favorite food as a child?" value="What was your favorite food as a child?"></asp:ListItem>
                        <asp:ListItem enabled="true" text="What is your favorite movie?" value="What is your favorite movie?"></asp:ListItem>
                        <asp:ListItem enabled="true" text="In what town was your first job?" value="In what town was your first job?"></asp:ListItem>
                        <asp:ListItem enabled="true" text="What was the make and model of your first car?" value="What was the make and model of your first car?"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Quest2" IntialValue="" CssClass="text-danger" Display="Dynamic" ErrorMessage="required" />
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="Answer2" CssClass="col-md-2 control-label">* Security Answer:</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="Answer2" CssClass="form-control" TextMode="SingleLine" style="width:40rem; max-width:40rem;" data-lpignore="true" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Answer2" InitialValue="" CssClass="text-danger" Display="Dynamic" ErrorMessage="required" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:CheckBox ID="chkDeclare" runat="server" Text="Declaration: I hereby declare under penalty of perjury that .." CssClass="checkbox bold" />
                    <asp:CustomValidator runat="server" ID="CheckBoxRequired" ClientValidationFunction="CheckBoxRequired_ClientValidate" CssClass="text-danger hidden">Declaration must be checked.</asp:CustomValidator>
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="DeclareInits" CssClass="col-md-2 control-label">Declaration initials: </asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="DeclareInits" Width="70px" CssClass="form-control" ToolTip="Enter your initials acknowledging the Declaration above." />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Quest1" CssClass="text-danger" Display="Dynamic" ErrorMessage="required" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:Button runat="server" ID="btnCancel" OnClick="CancelProfile_Click" Text="Cancel & Logout" CssClass="btn btn-danger" ToolTip="Clicking this button will cause this validation screen to be displayed on your next login." UseSubmitBehavior="false" CausesValidation="false" />
                    <asp:Button runat="server" ID="btnSubmit" OnClick="SubmitProfile_Click" Text="Submit" CssClass="btn btn-primary" ToolTip="Click to confirm this information is correct." style="margin-left:1rem;" />
                </div>
            </div>
        </div>
    </section>

    <script>
        function CheckBoxRequired_ClientValidate(sender, e) {
            e.IsValid = $("#MainContent_chkDeclare").is(':checked');
        }
    </script>
</asp:Content>
