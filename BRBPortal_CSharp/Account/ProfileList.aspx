<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProfileList.aspx.cs" Inherits="BRBPortal_CSharp.Account.ProfileList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>My Profile</h2>
    <section id="profileListForm">
        <div class="form-horizontal">
            <hr />
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="UserIDCode1" CssClass="col-md-2 control-label">User ID: </asp:Label>
                <div class="col-md-10 literal">
                    <asp:Literal ID="UserIDCode1" runat="server"></asp:Literal>
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="BillCode1" CssClass="col-md-2 control-label">Billing Code: </asp:Label>
                <div class="col-md-10 literal">
                    <asp:Literal ID="BillCode1" runat="server"></asp:Literal>
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="Relationship" CssClass="col-md-2 control-label">Relationship: </asp:Label>
                <div class="col-md-10 literal">
                    <asp:Literal runat="server" ID="Relationship"></asp:Literal>
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="FullName1" CssClass="col-md-2 control-label">Full Name: </asp:Label>
                <div class="col-md-10 literal">
                    <asp:Literal ID="FullName1" runat="server"></asp:Literal>
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="MailAddress1" CssClass="col-md-2 control-label">Mailing Address: </asp:Label>
                <div class="col-md-10 literal">
                    <asp:Literal ID="MailAddress1" runat="server"></asp:Literal>
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="EmailAddress1" CssClass="col-md-2 control-label">Email Address: </asp:Label>
                <div class="col-md-10 literal">
                    <asp:Literal ID="EmailAddress1" runat="server"></asp:Literal>
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="PhoneNo1" CssClass="col-md-2 control-label">Phone Number: </asp:Label>
                <div class="col-md-10 literal">
                    <asp:Literal ID="PhoneNo1" runat="server"></asp:Literal>
                </div>
            </div>
            <div class="form-group" >
                <asp:Label runat="server" AssociatedControlID="Quest1" CssClass="col-md-2 control-label">Security Question:</asp:Label>
                <div class="col-md-10 literal">
                    <asp:literal runat="server" id="Quest1"></asp:literal>
                </div>
            </div>
            <div class="form-group" >
                <asp:Label runat="server" AssociatedControlID="Quest2" CssClass="col-md-2 control-label">Security Question:</asp:Label>
                <div class="col-md-10 literal">
                    <asp:literal runat="server" id="Quest2"></asp:literal>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:Button runat="server" id="btnCancel" OnClick="CancelList_Click" Text="Cancel" CssClass="btn btn-default" ToolTip="Returns to Home page." UseSubmitBehavior="false" TabIndex="-1" />
                    <asp:Button runat="server" ID="btnEdit" OnClick="EditProfile_Click" Text="Edit" CssClass="btn btn-primary" ToolTip="Edit your profile." style="margin-left:1rem;" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:HyperLink runat="server" NavigateUrl="/Account/ManagePassword" ID="UpdatePasswordHyperLink" ViewStateMode="Disabled">Update Password</asp:HyperLink>
                    &nbsp;|&nbsp; 
                    <asp:HyperLink runat="server" NavigateUrl="/Account/ManageSecurityQuestions" ID="UpdateSecurityQuestionsHyperLink1" ViewStateMode="Disabled">Update Security Questions</asp:HyperLink>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
