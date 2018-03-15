<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="BRBPortal_CSharp.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%--<link rel="stylesheet" type="text/css" href="/Styles/StyleBRB.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/Baseline.Type.css" />--%>

    <h2>Home</h2>

    <div class="form-horizontal">
        <hr />
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="HomeOption" CssClass="control-label">Select an Option:</asp:Label>
        </div>

        <div class="form-group">
            <asp:RadioButtonList runat="server" ID="HomeOption" RepeatDirection="Vertical" ToolTip="Select your relationship.">
                <asp:ListItem Enabled="true" Text="&nbsp;&nbsp;Manage Account Profile" Value="MngProfile"></asp:ListItem>
                <asp:ListItem Enabled="true" Text="&nbsp;&nbsp;Manage Property Registration / Pay a Bill" Value="MngPay"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
        
        <div class="form-group">
            <asp:Button runat="server" OnClick="MngSel_Click" Text="Submit" CssClass="btn btn-primary" />
        </div>
    </div>
</asp:Content>