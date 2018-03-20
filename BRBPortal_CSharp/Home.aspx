<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="BRBPortal_CSharp.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Home</h2>
    <div class="form-horizontal">
        <hr />
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="HomeOption" CssClass="control-label">Select an Option:</asp:Label>
        </div>
        <div class="form-group">
            <asp:RadioButtonList runat="server" ID="HomeOption" RepeatDirection="Vertical">
                <asp:ListItem Enabled="true" Text="&nbsp;&nbsp;Manage Account Profile" Value="Profile" Selected="True"></asp:ListItem>
                <asp:ListItem Enabled="true" Text="&nbsp;&nbsp;Manage Property Registration / Pay a Bill" Value="Properties"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <div class="form-group">
            <asp:Button runat="server" OnClick="MngSel_Click" Text="Submit" CssClass="btn btn-primary" autofocus="autofocus" />
        </div>
    </div>
</asp:Content>