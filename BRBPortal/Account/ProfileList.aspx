<%@ Page Title="My Profile" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="ProfileList.aspx.vb" Inherits="BRBPortal.Profilelist" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%--<h2><%: Title %>.</h2>--%>

    <link rel="stylesheet" type="text/css" href="../Content/bootstrap.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/StyleBRB.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/Baseline.Type.css" />

    <div class="form-horizontal" style="width: 809px; height: 500px; padding-left:30px" >
        <div class="form-group" style="height:40px; align-items:center">
            <h4>My Profile</h4>
            <nav class="nav-right" style="float:right">
                <a href="../Home">Home</a>
                &nbsp;&nbsp;
                <a href="ProfileList">My Profile</a>
                &nbsp;&nbsp;
                <a href="../Cart">Cart</a>
                &nbsp;&nbsp;
                <a href="../Contact">Contact Us</a>
                &nbsp;&nbsp;
                <a href="../Logout">Logout</a>
            </nav>
        </div>

        <br />
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="UserIDCode1" Width="130px" CssClass="col-md-1 control-label">User ID: </asp:Label>
            <asp:Literal ID="UserIDCode1" runat="server" > </asp:Literal>
            <%--<asp:TextBox runat="server" ID="UserIDCode" ReadOnly="true" BorderStyle="None" CausesValidation="false" text="userid" />--%>
        </div>
        
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="BillCode1" Width="130px" CssClass="col-md-1 control-label">Billing Code: </asp:Label>
            <asp:Literal ID="BillCode1" runat="server" > </asp:Literal>
            <%--<asp:TextBox runat="server" ID="BillCode" ReadOnly="true" BorderStyle="None" CausesValidation="false" text="billcode"/>--%>
        </div>
        
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="FullName1" Width="280px" CssClass="col-md-1 control-label">Name (First, Middle, Last, and suffix): </asp:Label>
            <asp:Literal ID="FullName1" runat="server" > </asp:Literal>
            <%--<asp:TextBox runat="server" ID="FullName" ReadOnly="true" BorderStyle="None" CausesValidation="false" text="fullname" Width="301px"/>--%>
        </div>
        
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="MailAddress1" Width="150px" CssClass="col-md-1 control-label">Mailing Address: </asp:Label>
            <asp:Literal ID="MailAddress1" runat="server" > </asp:Literal>
            <%--<asp:TextBox runat="server" ID="MailAddress" ReadOnly="true" BorderStyle="None" CausesValidation="false" text="mailaddress" Width="300px"/>--%>
        </div>
        
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="EmailAddress1" Width="150px" CssClass="col-md-1 control-label">Email Address: </asp:Label>
            <asp:Literal ID="EmailAddress1" runat="server" > </asp:Literal>
            <%--<asp:TextBox runat="server" ID="EmailAddress" ReadOnly="true" BorderStyle="None" CausesValidation="false" text="emailaddress" Width="300px"/>--%>
        </div>
        
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="PhoneNo1" Width="140px" CssClass="col-md-1 control-label">Phone Number: </asp:Label>
            <asp:Literal ID="PhoneNo1" runat="server" > </asp:Literal>
            <%--<asp:TextBox runat="server" ID="PhoneNo" ReadOnly="true" BorderStyle="None" CausesValidation="false" text="phoneno" Width="150px"/>--%>
        </div>

        <div class="form-group" >
            <asp:Label runat="server" AssociatedControlID="Quest1" Width="160px" CssClass="col-md-1 control-label">Security Question:</asp:Label>
            <asp:literal runat="server" id="Quest1"></asp:literal>
           <%-- <br />--%>
<%--            <asp:Label runat="server" AssociatedControlID="Answer1"  CssClass="col-md-1 control-label">Security Answer:</asp:Label>
            <asp:TextBox runat="server" ID="Answer1" TextMode="SingleLine" width="200px" />--%>
        </div>

        <div class="form-group" >
            <asp:Label runat="server" AssociatedControlID="Quest2" Width="160px" CssClass="col-md-1 control-label">Security Question:</asp:Label>
            <asp:literal runat="server" id="Quest2"></asp:literal>
            <%--<br />--%>
<%--            <asp:Label runat="server" AssociatedControlID="Answer2" CssClass="col-md-1 control-label">Security Answer:</asp:Label>
            <asp:TextBox runat="server" ID="Answer2" TextMode="Password" width="200px" />--%>
        </div>

        <asp:HyperLink runat="server" NavigateUrl="/Account/ManagePassword" ID="UpdatePasswordHyperLink" 
                ViewStateMode="Disabled">Update Password</asp:HyperLink>
        
        <br /><br />
        <div class="btn-group">
            <asp:Button runat="server" ID="btnEdit" OnClick="EditProfile_Click" Text="Edit" CssClass="btn-default active" 
                ToolTip="Edit your profile." />
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button runat="server" id="btnCancel" OnClick="CancelList_Click" Text="Cancel" CssClass="btn-default active" 
                ToolTip="Returns to Home page." />
        </div>

    </div>
</asp:Content>