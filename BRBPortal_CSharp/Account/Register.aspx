<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="BRBPortal_CSharp.Account.Register" %>

<%@ Import Namespace="Microsoft.AspNet.Identity" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .navbar-nav, .navbar-right { display: none; }
    </style>

    <script src="/Scripts/jquery.mask.js" type="text/javascript"></script>

    <asp:HiddenField ID="hfDialogID" runat="server" />

    <h2>Request Online Account</h2>

    <section id="registerForm">
        <div class="form-horizontal">
            <asp:PlaceHolder runat="server" ID="ErrorMsg" Visible="false">
                <p class="text-danger">
                    <asp:Literal runat="server" ID="FailureText" />
                </p>
            </asp:PlaceHolder>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ReqUserID" CssClass="col-md-2 control-label">* User ID:</asp:Label>
            <div class="col-md-10">
                <asp:TextBox ID="ReqUserID" runat="server" ToolTip="Enter a valid User ID."></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ReqUserID" CssClass="text-danger" ErrorMessage="The User ID field is required." />
            </div>    
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="BillCode" CssClass="col-md-2 control-label">* Billing Code: </asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="BillCode" ToolTip="Enter a valid Billing Code." />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="BillCode" CssClass="text-danger" ErrorMessage="The Billing Code field is required." />
            </div>    
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="PropRelate" CssClass="col-md-2 control-label">* Property Relationship: </asp:Label>
            <div class="col-md-10">
                <asp:RadioButtonList runat="server" ID="PropRelate" RepeatDirection="Horizontal" ToolTip="Select your relationship." OnSelectedIndexChanged="PropRelate_SelectedIndexChanged" AutoPostBack="true" >
                        <asp:ListItem Enabled="true" Text="&nbsp;&nbsp;Owner&nbsp;&nbsp;" Value="Owner"></asp:ListItem>
                        <asp:ListItem Enabled="true" Text="&nbsp;&nbsp;Agent" Value="Agent"></asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="PropRelate" CssClass="text-danger" Display="Dynamic" ErrorMessage="The Relationship field is required." />
            </div>    
        </div>

        <div class="form-group" id="NameGrp" runat="server">
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="FirstName" CssClass="col-md-2 control-label">* First Name: </asp:Label>
            <div class="col-md-10">
            </div>    
                <asp:TextBox runat="server" ID="FirstName" ToolTip="Enter your first name."  />
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="MidName" CssClass="col-md-2 control-label">Middle Name: </asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="MidName" ToolTip="Enter your middle name (optional)." />
                </div>    
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="LastName" CssClass="col-md-2 control-label">* Last Name: </asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="LastName" ToolTip="Enter your last name." />
                </div>    
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="Suffix" CssClass="col-md-2 control-label">Suffix: </asp:Label>
                <div class="col-md-2">
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
        </div>

        <div class="form-group" id="AgencyGrp" runat="server">
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="AgencyName" CssClass="col-md-2 control-label">* Agency Name: </asp:Label>
                <div class="col-md-2">
                    <asp:TextBox runat="server" ID="AgencyName" ToolTip="Enter the agency name."  />
                </div>
            </div>
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="StNum" CssClass="col-md-2 control-label">* Street Number: </asp:Label>
            <div class="col-md-2">
            </div>
            <asp:TextBox runat="server" ID="StNum" ToolTip="Enter the street number of the mailing address." />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="StNum"  ValidationGroup="UpdCheck" CssClass="text-danger" Display="Dynamic" ErrorMessage="The street number field is required." />
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="StName" CssClass="col-md-2 control-label">* Street Name: </asp:Label>
            <div class="col-md-2">
                <asp:TextBox runat="server" ID="StName" ToolTip="Enter the street name of the mailing address." />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="StName"  ValidationGroup="UpdCheck" CssClass="text-danger" Display="Dynamic" ErrorMessage="The street name field is required." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="StUnit" CssClass="col-md-2 control-label">&nbsp;&nbsp;&nbsp;Unit Number: </asp:Label>
            <div class="col-md-2">
                <asp:TextBox runat="server" ID="StUnit" ToolTip="Enter the street number of the mailing address." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="StCity" CssClass="col-md-2 control-label">* City: </asp:Label>
            <div class="col-md-2">
                <asp:TextBox runat="server" ID="StCity" ToolTip="Enter the city of the mailing address." />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="StCity"  ValidationGroup="UpdCheck" CssClass="text-danger" Display="Dynamic" ErrorMessage="The city field is required." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="StState" CssClass="col-md-2 control-label">* State: </asp:Label>
            <div class="col-md-2">
                <asp:dropdownlist runat="server" ID="StState" ToolTip="Select a state from the list.">
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
                <asp:RequiredFieldValidator runat="server" ControlToValidate="StState"  ValidationGroup="UpdCheck" CssClass="text-danger" Display="Dynamic" ErrorMessage="The state field is required." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="StZip" CssClass="col-md-2 control-label">* Zip Code: </asp:Label>
            <div class="col-md-2">
                <asp:TextBox runat="server" ID="StZip" ToolTip="Enter the zip code of the mailing address." />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="StZip"  ValidationGroup="UpdCheck" CssClass="text-danger" Display="Dynamic" ErrorMessage="The zip code field is required." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="StCountry" CssClass="col-md-2 control-label">* Country: </asp:Label>
            <div class="col-md-2">
                <asp:TextBox runat="server" ID="StCountry" ToolTip="Enter the country of the mailing address." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="EmailAddress" CssClass="col-md-2 control-label">* Email Address: </asp:Label>
            <div class="col-md-2">
                <asp:TextBox runat="server" ID="EmailAddress" ToolTip="Enter your email address." />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="EmailAddress" CssClass="text-danger" Display="Dynamic" ErrorMessage="The Email Address field is required." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="PhoneNo" CssClass="col-md-2 control-label">* Phone Number: </asp:Label>
            <div class="col-md-2">
                <asp:TextBox runat="server" ID="PhoneNo" ToolTip="Enter your 10-digit phone number." />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="PhoneNo" CssClass="text-danger" Display="Dynamic" ErrorMessage="The Phone Number field is required." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="PropOwnLastName" CssClass="col-md-2 control-label">* Property Owner's Last Name: </asp:Label>
            <div class="col-md-2">
                <asp:TextBox runat="server" ID="PropOwnLastName" ToolTip="Enter the owner's last name." />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="PropOwnLastName" CssClass="text-danger" Display="Dynamic" ErrorMessage="The Property Owner's Last Name field is required." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="PropAddress" CssClass="col-md-2 control-label">* Property Address: </asp:Label>
            <div class="col-md-2">
                <asp:TextBox runat="server" ID="PropAddress" ToolTip="Enter the property address." />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="PropAddress" CssClass="text-danger" Display="Dynamic" ErrorMessage="The Property Address field is required." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="PurchaseYear" CssClass="col-md-2 control-label">* Property Purchase Year: </asp:Label>
            <div class="col-md-2">
                <asp:TextBox runat="server" ID="PurchaseYear" ToolTip="Enter the year property was purchased." />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="PurchaseYear" CssClass="text-danger" Display="Dynamic" ErrorMessage="The Property Purchase Year field is required." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Quest1" CssClass="col-md-2 control-label">* Security Question:</asp:Label>
            <div class="col-md-2">
                <asp:textbox runat="server" id="Quest1" ToolTip="Enter a Security Question."></asp:textbox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Quest1" CssClass="text-danger" Display="Dynamic" ErrorMessage="The Security Question field is required." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Answer1" CssClass="col-md-2 control-label">* Security Answer:</asp:Label>
            <div class="col-md-2">
                <asp:TextBox runat="server" ID="Answer1" TextMode="Password" ToolTip="Enter a Security Answer." />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Answer1" CssClass="text-danger" Display="Dynamic" ErrorMessage="The Security Answer field is required." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Quest2" CssClass="col-md-2 control-label">* Security Question:</asp:Label>
            <div class="col-md-2">
                <asp:textbox runat="server" id="Quest2" ToolTip="Enter a Security Question."></asp:textbox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Quest2" CssClass="text-danger" Display="Dynamic" ErrorMessage="The Security Question field is required." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Answer2" CssClass="col-md-2 control-label">* Security Answer:</asp:Label>
            <div class="col-md-2">
                <asp:TextBox runat="server" ID="Answer2" TextMode="Password" ToolTip="Enter a Security Answer." />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Answer2" CssClass="text-danger" Display="Dynamic" ErrorMessage="The Security Answer field is required." />
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button runat="server" OnClick="RegisterUser_Click" Text="Submit" CssClass="btn-default active" />
            </div>
        </div>
    </section>
    
    <script>
        $(document).ready(function () {
            $('input[name="PhoneNo"]').mask('000-000-0000');
        });
    </script>
</asp:Content>
