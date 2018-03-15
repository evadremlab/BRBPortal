<%@ Page Title="Update Account" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="UpdateAccount.aspx.vb" Inherits="BRBPortal.UpdateAccount" %>

<%@ Import Namespace="BRBPortal" %>
<%@ Import Namespace="Microsoft.AspNet.Identity" %>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <%--<h2><%: Title %>.</h2>--%>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>

    <link rel="stylesheet" type="text/css" href="/Styles/StyleBRB.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/Baseline.Type.css" />

    <div class="form-horizontal" style="width: 809px; height: 622px; padding-left:20px">
        <div class="form-group"  style="height:50px; align-content:center; padding-left:10px">
            <h4>Portal Administration<br />User Management - Update Account</h4>
            <nav class="nav-right" style="float:right; width:100px">
                <a href="../Home">Home</a>
                &nbsp;&nbsp;
                <a href="../Logout">Logout</a>
            </nav>
        </div>

        <br />
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="UserIDCode" CssClass="col-md-1 control-label">&nbsp;&nbsp;&nbsp;User ID:</asp:Label>
            <asp:Literal ID="UserIDCode" runat="server" ></asp:Literal>
        </div>

        <div class="form-group" style="align-items:center">
            <asp:Label runat="server" AssociatedControlID="BillCode" CssClass="col-md-1 control-label">* Billing Code: </asp:Label>
            <asp:TextBox runat="server" ID="BillCode" height="18px" Width="160px" ToolTip="Enter a valid Billing Code." />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="BillCode" 
                CssClass="text-danger" ErrorMessage="The Billing Code field is required." />
        </div>

        <br />
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="FirstName" CssClass="col-md-1 control-label">* First Name: </asp:Label>
            <asp:TextBox runat="server" ID="FirstName"  Width="160px" ToolTip="Enter your first name."  />
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
            <asp:TextBox runat="server" ID="MailAddress" Width="300px" ToolTip="Enter your mailing address." />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="MailAddress" 
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The Mailing Address field is required." />
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="EmailAddress" CssClass="col-md-1 control-label">* Email Address: </asp:Label>
            <asp:TextBox runat="server" ID="EmailAddress" Width="300px" ToolTip="Enter your email address." />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="EmailAddress"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The Email Address field is required." />
        </div>
        
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="PhoneNo" CssClass="col-md-1 control-label">* Phone Number: </asp:Label>
            <asp:TextBox runat="server" ID="PhoneNo" Width="300px" ToolTip="Enter your phone number." />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="PhoneNo"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The Phone Number field is required." />
        </div>
                
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="SecAnswer" CssClass="col-md-1 control-label">&nbsp;&nbsp;Security Answer: </asp:Label>
            <asp:TextBox runat="server" ID="SecAnswer" Width="300px" ToolTip="Enter your phone number." />
        </div>

        <br />
        <div class="form-group" style="padding-left:20px">
            <asp:CheckBox ID="chkDeActivate" runat="server" Text="&nbsp;&nbsp;&nbsp;De-activate account" />
        </div>

        <br />
        <div class="btn-group" style="padding-left:10px">
            <asp:Button runat="server" OnClick="UpdateUser_Click" Text="Update" CssClass="swd-button" />
            &nbsp;&nbsp;
            <asp:Button runat="server" OnClick="ResetPaswd_Click" Text="Reset Password" CssClass="swd-button" />
            &nbsp;&nbsp;
            <asp:Button runat="server" OnClick="Cancel_Click" Text="Cancel" CssClass="swd-button" />
        </div>

    </div>
</asp:Content>
