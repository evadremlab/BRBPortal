<%@ Page Title="My Profile" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="ProfileEdit.aspx.vb" Inherits="BRBPortal.ProfileEdit" Async="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%--<h2><%: Title %>.</h2>--%>

    <link rel="stylesheet" type="text/css" href="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/themes/start/jquery-ui.css" />
    <link rel="stylesheet" type="text/css" href="/Content/bootstrap.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/StyleBRB.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/Baseline.Type.css" />
    
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/ClientPortal.js") %>"> </script>
    <!-- Modal Dialog Box Ajax Code -->
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script src="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/jquery-ui.js" type="text/javascript"></script>

    <div class="form-horizontal" style="width: 809px; height: 680px; padding-left:30px" >
        <div class="form-group" style="height:40px; align-items:center">
            <h4>My Profile</h4>
            <nav class="nav-right" style="float:right">
                <a href="../Home">Home</a>
                &nbsp;&nbsp;
                <a href="../Account/ProfileList">My Profile</a>
                &nbsp;&nbsp;
                <a href="../Cart">Cart</a>
                &nbsp;&nbsp;
                <a href="../Contact">Contact Us</a>
                &nbsp;&nbsp;
                <a href="../Logout">Logout</a>
            </nav>
            <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                <p class="text-danger">
                    <asp:Literal runat="server" ID="FailureText" />
                </p>
            </asp:PlaceHolder>
        </div>

        <br />
        <div class="form-group" style="align-items:center">
            <asp:Label runat="server" AssociatedControlID="UserIDCode2" Width="140px" CssClass="col-md-1 control-label" Height="17px">User ID: </asp:Label>
            <asp:Literal ID="UserIDCode2" runat="server" > </asp:Literal>
            <%--<asp:TextBox runat="server" ID="UserIDCode" ReadOnly="true" BorderStyle="None" CausesValidation="false" text="userid" />--%>
        </div>
        
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="BillCode2" Width="140px" CssClass="col-md-1 control-label">Billing Code: </asp:Label>
            <asp:Literal ID="BillCode2" runat="server" > </asp:Literal>
            <%--<asp:TextBox runat="server" ID="BillCode" ReadOnly="true" BorderStyle="None" CausesValidation="false" text="billcode"/>--%>
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Relationship" Width="140px" CssClass="col-md-1 control-label">Relationship: </asp:Label>
            <asp:Literal ID="Relationship" runat="server"></asp:Literal>
        </div>
        
        <div id="OwnerGrp" runat="server" style="height:115px">
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="FirstName" Width="140px" CssClass="col-md-1 control-label">* First Name: </asp:Label>
                <asp:TextBox runat="server" ID="FirstName" ToolTip="Enter your first name."  />
                <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="FirstName" ValidationGroup="UpdCheck"
                        CssClass="text-danger" Display="Dynamic" ErrorMessage="The First Name field is required." />--%>
            </div>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="MidName" Width="140px" CssClass="col-md-1 control-label">&nbsp;&nbsp;&nbsp;Middle Name: </asp:Label>
                <asp:TextBox runat="server" ID="MidName" ToolTip="Enter your middle name (optional)." />
            </div>
        
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="LastName" Width="140px" CssClass="col-md-1 control-label">* Last Name: </asp:Label>
                <asp:TextBox runat="server" ID="LastName" ToolTip="Enter your last name."  />
                <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="LastName" ValidationGroup="UpdCheck"
                        CssClass="text-danger" Display="Dynamic" ErrorMessage="The Last Name field is required." />--%>
            </div>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="Suffix" Width="140px" CssClass="col-md-1 control-label">&nbsp;&nbsp;&nbsp;Suffix: </asp:Label>
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
        </div>
        
        <div class="form-group" id="AgencyGrp" style="height:20px; padding-left:14px" runat="server">
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="AgencyName" Width="140px" CssClass="col-md-1 control-label">* Agency Name: </asp:Label>
                <asp:TextBox runat="server" ID="AgencyName" Width="180px" ToolTip="Enter the agency name."  />
                <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="AgencyName" ValidationGroup="CheckReq"
                        CssClass="text-danger" Display="Dynamic" ErrorMessage="The Agency Name field is required." />--%>
            </div>
        </div>
        <br />

        <div class="form-group" style="height:170px">
            <asp:Label runat="server" AssociatedControlID="StNum" Width="150px" CssClass="col-md-1 control-label">* Street Number: </asp:Label>
            <asp:TextBox runat="server" ID="StNum" Width="80px" ToolTip="Enter the street number of the mailing address." />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="StNum"  ValidationGroup="UpdCheck"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The street number field is required." />
            <br />
            <asp:Label runat="server" AssociatedControlID="StName" Width="150px" CssClass="col-md-1 control-label">* Street Name: </asp:Label>
            <asp:TextBox runat="server" ID="StName" Width="200px" ToolTip="Enter the street name of the mailing address." />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="StName"  ValidationGroup="UpdCheck"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The street name field is required." />
            <br />
            <asp:Label runat="server" AssociatedControlID="StUnit" Width="150px" CssClass="col-md-1 control-label">&nbsp;&nbsp;&nbsp;Unit Number: </asp:Label>
            <asp:TextBox runat="server" ID="StUnit" Width="80px" ToolTip="Enter the street number of the mailing address." />
            <br />
            <asp:Label runat="server" AssociatedControlID="StCity" Width="150px" CssClass="col-md-1 control-label">* City: </asp:Label>
            <asp:TextBox runat="server" ID="StCity" Width="100px" ToolTip="Enter the city of the mailing address." />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="StCity"  ValidationGroup="UpdCheck"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The city field is required." />
            <br />
            <asp:Label runat="server" AssociatedControlID="StState" Width="150px" CssClass="col-md-1 control-label">* State: </asp:Label>
            <asp:dropdownlist runat="server" ID="StState" Height="20px" ToolTip="Select a state from the list." >
                <asp:ListItem enabled="true" text="Select state" value="-1"></asp:ListItem>
                <asp:ListItem enabled="true" text="AK" value="AK"></asp:ListItem>
                <asp:ListItem enabled="true" text="AL" value="AL"></asp:ListItem>
                <asp:ListItem enabled="true" text="AR" value="AR"></asp:ListItem>
                <asp:ListItem enabled="true" text="AZ" value="AZ"></asp:ListItem>
                <asp:ListItem enabled="true" text="CA" value="CA"></asp:ListItem>
                <asp:ListItem enabled="true" text="CT" value="CT"></asp:ListItem>
                <asp:ListItem enabled="true" text="DE" value="DE"></asp:ListItem>
                <asp:ListItem enabled="true" text="FL" value="FL"></asp:ListItem>
                <asp:ListItem enabled="true" text="GA" value="GA"></asp:ListItem>
                <asp:ListItem enabled="true" text="HI" value="HI"></asp:ListItem>
                <asp:ListItem enabled="true" text="IA" value="IA"></asp:ListItem>
                <asp:ListItem enabled="true" text="ID" value="ID"></asp:ListItem>
                <asp:ListItem enabled="true" text="IL" value="IL"></asp:ListItem>
                <asp:ListItem enabled="true" text="IN" value="IN"></asp:ListItem>
                <asp:ListItem enabled="true" text="KS" value="KS"></asp:ListItem>
                <asp:ListItem enabled="true" text="KY" value="KY"></asp:ListItem>
                <asp:ListItem enabled="true" text="LA" value="LA"></asp:ListItem>
                <asp:ListItem enabled="true" text="MA" value="MA"></asp:ListItem>
                <asp:ListItem enabled="true" text="MD" value="MD"></asp:ListItem>
                <asp:ListItem enabled="true" text="ME" value="ME"></asp:ListItem>
                <asp:ListItem enabled="true" text="MI" value="MI"></asp:ListItem>
                <asp:ListItem enabled="true" text="MN" value="MN"></asp:ListItem>
                <asp:ListItem enabled="true" text="MO" value="MO"></asp:ListItem>
                <asp:ListItem enabled="true" text="MS" value="MS"></asp:ListItem>
                <asp:ListItem enabled="true" text="MT" value="MT"></asp:ListItem>
                <asp:ListItem enabled="true" text="NC" value="NC"></asp:ListItem>
                <asp:ListItem enabled="true" text="ND" value="ND"></asp:ListItem>
                <asp:ListItem enabled="true" text="NE" value="NE"></asp:ListItem>
                <asp:ListItem enabled="true" text="NH" value="NH"></asp:ListItem>
                <asp:ListItem enabled="true" text="NJ" value="NJ"></asp:ListItem>
                <asp:ListItem enabled="true" text="NM" value="NM"></asp:ListItem>
                <asp:ListItem enabled="true" text="NV" value="NV"></asp:ListItem>
                <asp:ListItem enabled="true" text="NY" value="NY"></asp:ListItem>
                <asp:ListItem enabled="true" text="OH" value="OH"></asp:ListItem>
                <asp:ListItem enabled="true" text="OK" value="OK"></asp:ListItem>
                <asp:ListItem enabled="true" text="OR" value="OR"></asp:ListItem>
                <asp:ListItem enabled="true" text="PA" value="PA"></asp:ListItem>
                <asp:ListItem enabled="true" text="RI" value="RI"></asp:ListItem>
                <asp:ListItem enabled="true" text="SC" value="SC"></asp:ListItem>
                <asp:ListItem enabled="true" text="SD" value="SD"></asp:ListItem>
                <asp:ListItem enabled="true" text="TN" value="TN"></asp:ListItem>
                <asp:ListItem enabled="true" text="TX" value="TX"></asp:ListItem>
                <asp:ListItem enabled="true" text="UT" value="UT"></asp:ListItem>
                <asp:ListItem enabled="true" text="VA" value="VA"></asp:ListItem>
                <asp:ListItem enabled="true" text="VT" value="VT"></asp:ListItem>
                <asp:ListItem enabled="true" text="WA" value="WA"></asp:ListItem>
                <asp:ListItem enabled="true" text="WI" value="WI"></asp:ListItem>
                <asp:ListItem enabled="true" text="WV" value="WV"></asp:ListItem>
                <asp:ListItem enabled="true" text="WY" value="WY"></asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="StState"  ValidationGroup="UpdCheck"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The state field is required." />
            <br />
            <asp:Label runat="server" AssociatedControlID="StZip" Width="150px" CssClass="col-md-1 control-label">* Zip Code: </asp:Label>
            <asp:TextBox runat="server" ID="StZip" Width="80px" ToolTip="Enter the zip code of the mailing address." />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="StZip"  ValidationGroup="UpdCheck"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The zip code field is required." />
            <br />
            <asp:Label runat="server" AssociatedControlID="StCountry" Width="150px" CssClass="col-md-1 control-label">* Country: </asp:Label>
            <asp:TextBox runat="server" ID="StCountry" Width="80px" ToolTip="Enter the country of the mailing address." />
<%--            <asp:RequiredFieldValidator runat="server" ControlToValidate="StCountry" 
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The country field is required." />--%>
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="EmailAddress" Width="150px" CssClass="col-md-1 control-label">* Email Address: </asp:Label>
            <asp:TextBox runat="server" ID="EmailAddress" Width="240px" ToolTip="Enter your email address." />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="EmailAddress" ValidationGroup="UpdCheck"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The Email Address field is required." />
        </div>
        
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="PhoneNo" Width="150px" CssClass="col-md-1 control-label">* Phone Number: </asp:Label>
            <asp:TextBox runat="server" ID="PhoneNo" Width="175px" ToolTip="Enter your phone number." />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="PhoneNo" ValidationGroup="UpdCheck"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The Phone Number field is required." />
        </div>
        
        <div class="form-group" style="height:110px">
            <asp:Label runat="server" Width="175px" AssociatedControlID="Quest1" CssClass="col-md-1 control-label">* Security Question:</asp:Label>
            <asp:textbox runat="server" Width="500px" id="Quest1"></asp:textbox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="Quest1" ValidationGroup="UpdCheck"
                CssClass="text-danger" Display="Dynamic" ErrorMessage="The Security Question field is required." />
            <br />
            <asp:Label runat="server" AssociatedControlID="Answer1" CssClass="col-md-1 control-label">* Security Answer:</asp:Label>
            <asp:TextBox runat="server" ID="Answer1" TextMode="Password" width="200px" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="Answer1" ValidationGroup="UpdCheck"
                CssClass="text-danger" Display="Dynamic" ErrorMessage="The Security Answer field is required." />
            <br />
            <asp:Label runat="server" Width="175px" AssociatedControlID="Quest2" CssClass="col-md-1 control-label">* Security Question:</asp:Label>
            <asp:textbox runat="server" Width="500px" id="Quest2"></asp:textbox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="Quest2" ValidationGroup="UpdCheck"
                CssClass="text-danger" Display="Dynamic" ErrorMessage="The Security Question field is required." />
            <br />
            <asp:Label runat="server" AssociatedControlID="Answer2" CssClass="col-md-1 control-label">* Security Answer:</asp:Label>
            <asp:TextBox runat="server" ID="Answer2" TextMode="Password" width="200px" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="Answer2" ValidationGroup="UpdCheck"
                CssClass="text-danger" Display="Dynamic" ErrorMessage="The Security Answer field is required." />
        </div>

        <br />
        <div class="btn-group" style="padding-left:10px">
            <asp:Button runat="server" ID="btnUpdate" OnClick="UpdateProfile_Click" Text="Update" CssClass="btn-default active" 
                ToolTip="Update your profile." ValidationGroup="UpdCheck" />
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button runat="server" id="btnCancel" OnClick="CancelEdit_Click" Text="Cancel" CssClass="btn-default active" 
                ToolTip="Returns to profile view." />
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