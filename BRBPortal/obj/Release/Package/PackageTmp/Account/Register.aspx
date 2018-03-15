<%@ Page Title="Register" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Register.aspx.vb" Inherits="BRBPortal.Register" Async="true" %>

<%@ Import Namespace="BRBPortal" %>
<%@ Import Namespace="Microsoft.AspNet.Identity" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <script src="/Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>

    <script src="/Scripts/jquery.mask.js" type="text/javascript"></script>
    
    <script>
        $(document).ready(function ($) {
            $('input[name="PhoneNo"]').mask('000-000-0000');
            //$('#PhoneNo').mask('999-999-9999');
        } );
    </script>

    <link rel="stylesheet" type="text/css" href="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/themes/start/jquery-ui.css" />
    <link rel="stylesheet" type="text/css" href="/Content/bootstrap.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/StyleBRB.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/Baseline.Type.css" />
    
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/ClientPortal.js") %>"> </script>
    <!-- Modal Dialog Box Ajax Code -->
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script src="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/jquery-ui.js" type="text/javascript"></script>
    
    <div class="form-horizontal" style="width: 809px; height: 800px; padding-left:20px">
        <h4>Request Online Account</h4>
        <asp:PlaceHolder runat="server" ID="ErrorMsg" Visible="false">
            <p class="text-danger">
                <asp:Literal runat="server" ID="FailureText" />
            </p>
        </asp:PlaceHolder>
        
        <br />
        <div class="form-group">
             <asp:Label runat="server" AssociatedControlID="ReqUserID" Width="140px" CssClass="col-md-1 control-label">* User ID:</asp:Label>
             <asp:TextBox ID="ReqUserID" runat="server"  Width="180px" ToolTip="Enter a valid User ID."></asp:TextBox>
             <asp:RequiredFieldValidator runat="server" ControlToValidate="ReqUserID" CssClass="text-danger"
                  ErrorMessage="The User ID field is required." />
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="BillCode" Width="140px" CssClass="col-md-1 control-label">* Billing Code: </asp:Label>
            <asp:TextBox runat="server" ID="BillCode"  Width="180px" ToolTip="Enter a valid Billing Code." />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="BillCode" 
                CssClass="text-danger" ErrorMessage="The Billing Code field is required." />
        </div>
        <br />
        
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="PropRelate" Width="190px" CssClass="col-md-1 control-label">* Property Relationship: </asp:Label>
            <asp:RadioButtonList runat="server" ID="PropRelate" RepeatDirection="Horizontal" ToolTip="Select your relationship." 
                OnSelectedIndexChanged="PropRelate_SelectedIndexChanged" AutoPostBack="true" >
                    <asp:ListItem Enabled="true" Text="&nbsp;&nbsp;Owner&nbsp;&nbsp;" Value="Owner"></asp:ListItem>
                    <asp:ListItem Enabled="true" Text="&nbsp;&nbsp;Agent" Value="Agent"></asp:ListItem>
            </asp:RadioButtonList>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="PropRelate"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The Relationship field is required." />
        </div>

        <div class="form-group" id="NameGrp" runat="server" style="height:115px; padding-left:14px">
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="FirstName" Width="140px" CssClass="col-md-1 control-label">* First Name: </asp:Label>
                <asp:TextBox runat="server" ID="FirstName" ToolTip="Enter your first name."  />
                <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="FirstName"
                        CssClass="text-danger" Display="Dynamic" ErrorMessage="The First Name field is required." />--%>
            </div>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="MidName" Width="140px" CssClass="col-md-1 control-label">&nbsp;&nbsp;&nbsp;Middle Name: </asp:Label>
                <asp:TextBox runat="server" ID="MidName" ToolTip="Enter your middle name (optional)." />
            </div>
        
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="LastName" Width="140px" CssClass="col-md-1 control-label">* Last Name: </asp:Label>
                <asp:TextBox runat="server" ID="LastName" ToolTip="Enter your last name."  />
                <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="LastName"
                        CssClass="text-danger" Display="Dynamic" ErrorMessage="The Last Name field is required." />--%>
            </div>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="Suffix"  Width="140px" CssClass="col-md-1 control-label">&nbsp;&nbsp;&nbsp;Suffix: </asp:Label>
                <asp:dropdownlist runat="server" ID="Suffix" Height="24px" ToolTip="Select a suffix from the list (optional)." >
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
                <asp:TextBox runat="server" ID="AgencyName" ToolTip="Enter the agency name."  />
                <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="AgencyName" ValidationGroup="CheckReq"
                        CssClass="text-danger" Display="Dynamic" ErrorMessage="The Agency Name field is required." />--%>
            </div>
        </div>

        <div class="form-group" style="height:174px">
            <asp:Label runat="server" AssociatedControlID="StNum" Width="160px" CssClass="col-md-1 control-label">* Street Number: </asp:Label>
            <asp:TextBox runat="server" ID="StNum" Width="80px" ToolTip="Enter the street number of the mailing address." />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="StNum"  ValidationGroup="UpdCheck"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The street number field is required." />
            <br />
            <asp:Label runat="server" AssociatedControlID="StName" Width="160px" CssClass="col-md-1 control-label">* Street Name: </asp:Label>
            <asp:TextBox runat="server" ID="StName" Width="200px" ToolTip="Enter the street name of the mailing address." />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="StName"  ValidationGroup="UpdCheck"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The street name field is required." />
            <br />
            <asp:Label runat="server" AssociatedControlID="StUnit" Width="160px" CssClass="col-md-1 control-label">&nbsp;&nbsp;&nbsp;Unit Number: </asp:Label>
            <asp:TextBox runat="server" ID="StUnit" Width="80px" ToolTip="Enter the street number of the mailing address." />
            <br />
            <asp:Label runat="server" AssociatedControlID="StCity" Width="160px" CssClass="col-md-1 control-label">* City: </asp:Label>
            <asp:TextBox runat="server" ID="StCity" Width="100px" ToolTip="Enter the city of the mailing address." />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="StCity"  ValidationGroup="UpdCheck"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The city field is required." />
            <br />
            <asp:Label runat="server" AssociatedControlID="StState" Width="160px" CssClass="col-md-1 control-label">* State: </asp:Label>
            <asp:dropdownlist runat="server" ID="StState" Height="24px" ToolTip="Select a state from the list." >
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
            <asp:Label runat="server" AssociatedControlID="StZip" Width="160px" CssClass="col-md-1 control-label">* Zip Code: </asp:Label>
            <asp:TextBox runat="server" ID="StZip" Width="80px" ToolTip="Enter the zip code of the mailing address." />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="StZip"  ValidationGroup="UpdCheck"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The zip code field is required." />
            <br />
            <asp:Label runat="server" AssociatedControlID="StCountry" Width="160px" CssClass="col-md-1 control-label">* Country: </asp:Label>
            <asp:TextBox runat="server" ID="StCountry" Width="80px" ToolTip="Enter the country of the mailing address." />
<%--            <asp:RequiredFieldValidator runat="server" ControlToValidate="StCountry" 
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The country field is required." />--%>
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="EmailAddress" Width="160px" CssClass="col-md-1 control-label">* Email Address: </asp:Label>
            <asp:TextBox runat="server" ID="EmailAddress" Width="240px" ToolTip="Enter your email address." />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="EmailAddress"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The Email Address field is required." />
        </div>
        
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="PhoneNo" Width="160px" CssClass="col-md-1 control-label">* Phone Number: </asp:Label>
            <asp:TextBox runat="server" ID="PhoneNo" Width="175px" ToolTip="Enter your 10-digit phone number." />
            
            <%--<input type="text" runat="server" name="PhoneNo" id="PhoneNo" />--%>
           <%-- <input type="text" id="PhoneNo" name="PhoneNo" data-masked-input="999-999-9999" maxlength="12" />--%>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="PhoneNo"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The Phone Number field is required." />
              <%--<label style="padding-left:15px">* Phone Number
                <input type="text" name="PhoneNo" id="PhoneNo" placeholder="999-999-9999" style="margin-left:32px" />
              </label>--%>
        </div>
        
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="PropOwnLastName" width="240px" CssClass="col-md-1 control-label">* Property Owner's Last Name: </asp:Label>
            <asp:TextBox runat="server" ID="PropOwnLastName" Width="175px" ToolTip="Enter the owner's last name." />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="PropOwnLastName"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The Property Owner's Last Name field is required." />
        </div>
        
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="PropAddress" CssClass="col-md-1 control-label">* Property Address: </asp:Label>
            <asp:TextBox runat="server" ID="PropAddress" Width="240px" ToolTip="Enter the property address." />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="PropAddress"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The Property Address field is required." />
        </div>
        
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="PurchaseYear" Width="200px" CssClass="col-md-1 control-label">* Property Purchase Year: </asp:Label>
            <asp:TextBox runat="server" ID="PurchaseYear" Width="58px" ToolTip="Enter the year property was purchased." />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="PurchaseYear"
                CssClass="text-danger" Display="Dynamic" ErrorMessage="The Property Purchase Year field is required." />
        </div>

        <div class="form-group" style="height:110px">
            <asp:Label runat="server" AssociatedControlID="Quest1" Width="170px" CssClass="col-md-1 control-label">* Security Question:</asp:Label>
            <asp:textbox runat="server" Width="500px" id="Quest1" ToolTip="Enter a Security Question."></asp:textbox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="Quest1"
                CssClass="text-danger" Display="Dynamic" ErrorMessage="The Security Question field is required." />
            <br />
            <asp:Label runat="server" AssociatedControlID="Answer1" Width="170px" CssClass="col-md-1 control-label">* Security Answer:</asp:Label>
            <asp:TextBox runat="server" ID="Answer1" TextMode="Password" width="150px" ToolTip="Enter a Security Answer." />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="Answer1"
                CssClass="text-danger" Display="Dynamic" ErrorMessage="The Security Answer field is required." />
            <br />
            <asp:Label runat="server" AssociatedControlID="Quest2" Width="170px" CssClass="col-md-1 control-label">* Security Question:</asp:Label>
            <asp:textbox runat="server" Width="500px" id="Quest2" ToolTip="Enter a Security Question."></asp:textbox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="Quest2"
                CssClass="text-danger" Display="Dynamic" ErrorMessage="The Security Question field is required." />
            <br />
            <asp:Label runat="server" AssociatedControlID="Answer2" Width="170px" CssClass="col-md-1 control-label">* Security Answer:</asp:Label>
            <asp:TextBox runat="server" ID="Answer2" TextMode="Password" width="150px" ToolTip="Enter a Security Answer." />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="Answer2"
                CssClass="text-danger" Display="Dynamic" ErrorMessage="The Security Answer field is required." />
        </div>

        <br />
        <div class="btn-group" style="padding-left:10px">
            <asp:Button runat="server" OnClick="RegisterUser_Click" Text="Submit" CssClass="btn-default active" />
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
