<%@ Page Title="Profile Confirmation" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="ProfileConfirm.aspx.vb" Inherits="BRBPortal.ProfileConfirm" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%--<h2><%: Title %>.</h2>--%>

    <link rel="stylesheet" type="text/css" href="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/themes/start/jquery-ui.css" />
    <link rel="stylesheet" type="text/css" href="../Content/bootstrap.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/StyleBRB.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/Baseline.Type.css" />
    
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/ClientPortal.js") %>"> </script>
    <!-- Modal Dialog Box Ajax Code -->
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script src="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/jquery-ui.js" type="text/javascript"></script>

    <div class="form-horizontal" style="width: 809px; height: 500px; padding-left:30px" >
        <div class="form-group" style="height:40px; align-items:center">
            <h4>Account Profile Confirmation</h4>
            <nav class="nav-right" style="width:130px; float:right" >
                <a href="../Contact">Contact Us</a>
                &nbsp;&nbsp;
                <a href="../Logout">Logout</a>
            </nav>
        </div>

        <br />
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="UserIDCode0" CssClass="col-md-1 control-label">User ID: </asp:Label>
            <asp:Literal ID="UserIDCode0" runat="server" > </asp:Literal>
            <%--<asp:TextBox runat="server" ID="UserIDCode" ReadOnly="true" BorderStyle="None" CausesValidation="false" text="userid" />--%>
        </div>
        
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="BillCode0" CssClass="col-md-1 control-label">Billing Code: </asp:Label>
            <asp:Literal ID="BillCode0" runat="server" > </asp:Literal>
            <%--<asp:TextBox runat="server" ID="BillCode" ReadOnly="true" BorderStyle="None" CausesValidation="false" text="billcode"/>--%>
        </div>
        
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="FullName0" Width="280px" CssClass="col-md-1 control-label">Name (First, Middle, Last, and suffix): </asp:Label>
            <asp:Literal ID="FullName0" runat="server" > </asp:Literal>
            <%--<asp:TextBox runat="server" ID="FullName" ReadOnly="true" BorderStyle="None" CausesValidation="false" text="fullname" Width="301px"/>--%>
        </div>
        
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="MailAddress0" CssClass="col-md-1 control-label">Mailing Address: </asp:Label>
            <asp:Literal ID="MailAddress0" runat="server" > </asp:Literal>
            <%--<asp:TextBox runat="server" ID="MailAddress" ReadOnly="true" BorderStyle="None" CausesValidation="false" text="mailaddress" Width="300px"/>--%>
        </div>
        
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="EmailAddress0" CssClass="col-md-1 control-label">Email Address: </asp:Label>
            <asp:Literal ID="EmailAddress0" runat="server" > </asp:Literal>
            <%--<asp:TextBox runat="server" ID="EmailAddress" ReadOnly="true" BorderStyle="None" CausesValidation="false" text="emailaddress" Width="300px"/>--%>
        </div>
        
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="PhoneNo0" CssClass="col-md-1 control-label">Phone Number: </asp:Label>
            <asp:Literal ID="PhoneNo0" runat="server" > </asp:Literal>
            <%--<asp:TextBox runat="server" ID="PhoneNo" ReadOnly="true" BorderStyle="None" CausesValidation="false" text="phoneno" Width="150px"/>--%>
        </div>

        <br />
        <div class="form-group" style="padding-left:10px">
            <asp:CheckBox ID="chkDeclare" runat="server" Text="&nbsp;&nbsp;&nbsp;Declaration:  I hereby declare under penalty of perjury that .." AutoPostBack="True" />
        </div>
        
        <div class="form-group" style="padding-left:32px">
            <asp:Label runat="server" AssociatedControlID="DeclareInits" Width="140px" CssClass="control-label">
                Declaration initials: </asp:Label>
            <asp:TextBox runat="server" ID="DeclareInits" Width="70px" ToolTip="Enter your initials acknowledging the Declaration above." AutoPostBack="True" />
        </div>
        
        <br />
        <div class="btn-group">
            <asp:Button runat="server" ID="btnSubmit" OnClick="SubmitProfile_Click" Text="Submit" CssClass="btn-default active" 
                ToolTip="Click to confirm this information is correct." />
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button runat="server" id="btnCancel" OnClick="CancelProfile_Click" Text="Cancel & Logout" CssClass="btn-default active" 
                ToolTip="Clicking this button will cause this validation screen to be displayed on your next login." />
        </div>
        
        <asp:Button ID="btnDialogResponseYes" runat="server" Text="Hidden Button" OnClick="DialogResponseYes"
            Style="display: none" UseSubmitBehavior="false" />
        <asp:Button ID="btnDialogResponseNo" runat="server" Text="Hidden Button" OnClick="DialogResponseNo"
            Style="display: none" UseSubmitBehavior="false" />
        <div id="dialog2" style="display: none">
        </div>
        <asp:HiddenField ID="hfDialogID" runat="server" />
    </div>
</asp:Content>