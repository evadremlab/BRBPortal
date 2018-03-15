<%@ Page Title="Create Account" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CreateAccount.aspx.vb" Inherits="BRBPortal.CreateAccount" %>

<%@ Import Namespace="BRBPortal" %>
<%@ Import Namespace="Microsoft.AspNet.Identity" %>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <%--<h2><%: Title %>.</h2>--%>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>

    <link rel="stylesheet" type="text/css" href="/Styles/StyleBRB.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/Baseline.Type.css" />

    <div class="form-horizontal" style="width: 809px; height: 622px; padding-left:30px">
        <div class="form-group" style="height:50px; align-items:center">
            <h4>Portal Administration<br />User Management - New Account</h4>
            <nav class="nav" style="float:right; width:100px">
                <a href="../Home">Home</a>
                &nbsp;&nbsp;
                <a href="../Logout">Logout</a>
            </nav>
        </div>

        <br />
        <div class="form-group">
             <asp:Label runat="server" AssociatedControlID="ReqUserID" CssClass="col-md-1 control-label">* User ID:</asp:Label>
             <asp:TextBox ID="ReqUserID" runat="server" Width="160px" ToolTip="Enter a valid User ID."></asp:TextBox>
             <asp:RequiredFieldValidator runat="server" ControlToValidate="ReqUserID" CssClass="text-danger" ErrorMessage="The User ID field is required." />
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="BillCode" CssClass="col-md-1 control-label">* Billing Code: </asp:Label>
            <asp:TextBox runat="server" ID="BillCode"  Width="160px" ToolTip="Enter a valid Billing Code." />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="BillCode" 
                CssClass="text-danger" ErrorMessage="The Billing Code field is required." />
        </div>
        <br />

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="FirstName" CssClass="col-md-1 control-label">* First Name: </asp:Label>
            <asp:TextBox runat="server" ID="FirstName" Width="160px" ToolTip="Enter your first name."  />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="FirstName"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The First Name field is required." />
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="MidName" CssClass="col-md-1 control-label">&nbsp;&nbsp;&nbsp;Middle Name: </asp:Label>
            <asp:TextBox runat="server" ID="MidName" Width="160px" ToolTip="Enter your middle name (optional)." />
        </div>
        
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="LastName" CssClass="col-md-1 control-label">* Last Name: </asp:Label>
            <asp:TextBox runat="server" ID="LastName" Width="160px" ToolTip="Enter your last name."  />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="LastName"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The Last Name field is required." />
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Suffix" CssClass="col-md-1 control-label">&nbsp;&nbsp;&nbsp;Suffix: </asp:Label>
            <asp:dropdownlist runat="server" ID="Suffix" Height="20px" ToolTip="Select a suffix from the list (optional)." >
                <asp:ListItem enabled="true" text="Select suffix" value="-1"></asp:ListItem>
                <asp:ListItem enabled="true" text="Jr." value="1"></asp:ListItem>
                <asp:ListItem enabled="true" text="Sr." value="2"></asp:ListItem>
                <asp:ListItem enabled="true" text="I." value="3"></asp:ListItem>
                <asp:ListItem enabled="true" text="II" value="4"></asp:ListItem>
                <asp:ListItem enabled="true" text="III" value="5"></asp:ListItem>
                <asp:ListItem enabled="true" text="IV" value="6"></asp:ListItem>
                <asp:ListItem enabled="true" text="V" value="7"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <br />

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="MailAddress" CssClass="col-md-1 control-label">* Mailing Address: </asp:Label>
            <asp:TextBox runat="server" ID="MailAddress" Width="250px" ToolTip="Enter your mailing address." />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="MailAddress" 
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The Mailing Address field is required." />
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="EmailAddress" CssClass="col-md-1 control-label">* Email Address: </asp:Label>
            <asp:TextBox runat="server" ID="EmailAddress" Width="250px" ToolTip="Enter your email address." />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="EmailAddress"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The Email Address field is required." />
        </div>
        
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="PhoneNo" CssClass="col-md-1 control-label">* Phone Number: </asp:Label>
            <asp:TextBox runat="server" ID="PhoneNo" Width="250px" ToolTip="Enter your phone number." />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="PhoneNo"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The Phone Number field is required." />
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="SecAnswer" CssClass="col-md-1 control-label">&nbsp;&nbsp;Security Answer: </asp:Label>
            <asp:TextBox runat="server" ID="SecAnswer" Width="250px" ToolTip="Enter your phone number." />
        </div>

        <br />
        <div class="btn-group" style="padding-left:10px">
            <asp:Button runat="server" OnClick="CreateUser_Click" Text="Create" CssClass="swd-button" />
            &nbsp;&nbsp;
            <asp:Button runat="server" OnClick="Cancel_Click" Text="Cancel" CssClass="swd-button" />
        </div>

    </div>
</asp:Content>
