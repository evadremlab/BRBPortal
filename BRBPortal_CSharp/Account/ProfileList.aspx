<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProfileList.aspx.cs" Inherits="BRBPortal_CSharp.Account.ProfileList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>My Profile</h2>
    <section id="profileListForm">
        <div class="form-horizontal">
            <hr />
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="UserIDCode1" CssClass="control-label">User ID: </asp:Label>
                <asp:Literal ID="UserIDCode1" runat="server"></asp:Literal>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="BillCode1" CssClass="control-label">Billing Code: </asp:Label>
                <asp:Literal ID="BillCode1" runat="server"></asp:Literal>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="FullName1" CssClass="control-label">Name (First, Middle, Last, and suffix): </asp:Label>
                <asp:Literal ID="FullName1" runat="server"></asp:Literal>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="MailAddress1" CssClass="control-label">Mailing Address: </asp:Label>
                <asp:Literal ID="MailAddress1" runat="server"></asp:Literal>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="EmailAddress1" CssClass="control-label">Email Address: </asp:Label>
                <asp:Literal ID="EmailAddress1" runat="server"></asp:Literal>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="PhoneNo1" CssClass="control-label">Phone Number: </asp:Label>
                <asp:Literal ID="PhoneNo1" runat="server"></asp:Literal>
            </div>
            <div class="form-group" >
                <asp:Label runat="server" AssociatedControlID="Quest1" CssClass="control-label">Security Question:</asp:Label>
                <asp:literal runat="server" id="Quest1"></asp:literal>
            </div>
            <div class="form-group" >
                <asp:Label runat="server" AssociatedControlID="Quest2" CssClass="control-label">Security Question:</asp:Label>
                <asp:literal runat="server" id="Quest2"></asp:literal>
            </div>
            <div class="form-group">
                <asp:HyperLink runat="server" NavigateUrl="/Account/ManagePassword" ID="UpdatePasswordHyperLink" ViewStateMode="Disabled">Update Password</asp:HyperLink>
            </div>
            <div class="form-group">
                <asp:Button runat="server" id="btnCancel" OnClick="CancelList_Click" Text="Cancel" CssClass="btn btn-default" ToolTip="Returns to Home page." UseSubmitBehavior="false" />
                <asp:Button runat="server" ID="btnEdit" OnClick="EditProfile_Click" Text="Edit" CssClass="btn btn-primary" ToolTip="Edit your profile." style="margin-left:1rem;" />
            </div>
        </div>
    </section>
</asp:Content>
