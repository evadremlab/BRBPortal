<%@ Page Title="Home" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="Home.aspx.vb" Inherits="BRBPortal.Home" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <link rel="stylesheet" type="text/css" href="/Styles/StyleBRB.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/Baseline.Type.css" />

    <div class="form-horizontal" style="width: 809px; height: 500px; padding-left:20px" >
        <div class="form-group" style="height:40px; align-items:center">
            <h4 style="padding-left:10px">Home</h4>
            <nav class="nav-right" style="float:right" >
                <a href="Home">Home</a>
                &nbsp;&nbsp;
                <a href="../Account/ProfileList">My Profile</a>
                &nbsp;&nbsp;
                <a href="../Cart">Cart</a>
                &nbsp;&nbsp;
                <a href="../Contact">Contact Us</a>
                &nbsp;&nbsp;
                <a href="../Logout">Logout</a>
            </nav>
        </div>

        <br />
        <asp:Label runat="server" AssociatedControlID="HomeOption" Width="190px" CssClass="control-label">Select an Option: </asp:Label>
        
        <br />
        <div class="form-group" style="padding-left:20px; height:60px">
            <asp:RadioButtonList runat="server" ID="HomeOption" RepeatDirection="Vertical" ToolTip="Select your relationship." Height="66px" >
                    <asp:ListItem Enabled="true" Text="&nbsp;&nbsp;Manage Account Profile" Value="MngProfile"></asp:ListItem>
                    <asp:ListItem Enabled="true" Text="&nbsp;&nbsp;Manage Property Registration / Pay a Bill" Value="MngPay"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
        
        <br />
        <div class="form-group" style="padding-left:20px">
            <asp:Button runat="server" OnClick="MngSel_Click" Text="Submit" CssClass="btn-default active" />
        </div>
    </div>
</asp:Content>
