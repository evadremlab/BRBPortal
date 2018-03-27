<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProfileEdit.aspx.cs" enableEventValidation="false" Inherits="BRBPortal_CSharp.Account.ProfileEdit" %>
<%@ MasterType  virtualPath="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .form-control { width: auto; }
        input[type="text"] { text-transform:uppercase; }
    </style>

    <asp:HiddenField ID="hfDialogID" runat="server" />

    <h2>Edit Account Profile</h2>

    <section id="profileEditForm">
        <div class="form-horizontal">
            <hr />
            <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                <p class="text-danger">
                    <asp:Literal runat="server" ID="FailureText" />
                </p>
            </asp:PlaceHolder>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="UserIDCode2" CssClass="col-md-2 control-label">User ID: </asp:Label>
                <div class="col-md-10 literal">
                    <asp:Literal ID="UserIDCode2" runat="server"></asp:Literal>
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="BillCode2" CssClass="col-md-2 control-label">Billing Code: </asp:Label>
                <div class="col-md-10 literal">
                    <asp:Literal runat="server" ID="BillCode2"></asp:Literal>
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="Relationship" CssClass="col-md-2 control-label">Relationship: </asp:Label>
                <div class="col-md-10 literal">
                    <asp:Literal runat="server" ID="Relationship"></asp:Literal>
                </div>
            </div>

            <div id="OwnerGrp" runat="server">
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="FirstName" CssClass="col-md-2 control-label">* First Name: </asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="FirstName" ToolTip="Enter your first name." CssClass="form-control" TextMode="SingleLine" style="width:40rem; max-width:40rem;" autofocus="autofocus" />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="MidName" CssClass="col-md-2 control-label">&nbsp;&nbsp;&nbsp;Middle Name: </asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="MidName" ToolTip="Enter your middle name (optional)." CssClass="form-control" TextMode="SingleLine"  style="width:40rem; max-width:40rem;" />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="LastName" CssClass="col-md-2 control-label">* Last Name: </asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="LastName" ToolTip="Enter your last name." CssClass="form-control" TextMode="SingleLine" style="width:40rem; max-width:40rem;" />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="Suffix" CssClass="col-md-2 control-label">Suffix: </asp:Label>
                    <div class="col-md-10" style="max-width:15.5rem;">
                        <asp:dropdownlist runat="server" ID="Suffix" ToolTip="Select a suffix from the list (optional)." CssClass="form-control selectpicker">
                            <asp:ListItem enabled="true" text="Select suffix" value=""></asp:ListItem>
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
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="AgencyName" ToolTip="Enter the agency name." CssClass="form-control" TextMode="SingleLine"  />
                    </div>
                </div>
            </div>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="StNum" CssClass="col-md-2 control-label">* Street Number: </asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="StNum" ToolTip="Enter the street number of the mailing address." CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="StNum" CssClass="text-danger" Display="Dynamic" ErrorMessage="required" />
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="StName" CssClass="col-md-2 control-label">* Street Name: </asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="StName" ToolTip="Enter the street name of the mailing address." CssClass="form-control" TextMode="SingleLine" style="width:40rem; max-width:40rem;" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="StName" CssClass="text-danger" Display="Dynamic" ErrorMessage="required" />
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="StUnit" CssClass="col-md-2 control-label">Unit Number: </asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="StUnit" ToolTip="Enter the street number of the mailing address." CssClass="form-control" TextMode="SingleLine" />
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="StCity" CssClass="col-md-2 control-label">* City: </asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="StCity" ToolTip="Enter the city of the mailing address." CssClass="form-control" TextMode="SingleLine" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="StCity" CssClass="text-danger" Display="Dynamic" ErrorMessage="required" />
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="StState" CssClass="col-md-2 control-label">* State: </asp:Label>
                <div class="col-md-10">
                    <span style="display:inline-block; max-width:15.5rem;">
                        <asp:dropdownlist runat="server" ID="StState" ToolTip="Select a state from the list." CssClass="form-control selectpicker">
                            <asp:ListItem enabled="true" text="Select state" value=""></asp:ListItem>
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
                    </span>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="StState" CssClass="text-danger" Display="Dynamic" ErrorMessage="required" />
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="StZip" CssClass="col-md-2 control-label">* Zip Code: </asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="StZip" ToolTip="Enter the zip code of the mailing address." CssClass="form-control" TextMode="SingleLine" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="StZip" CssClass="text-danger" Display="Dynamic" ErrorMessage="required" />
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="StCountry" CssClass="col-md-2 control-label">* Country: </asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="StCountry" ToolTip="Enter the country of the mailing address." CssClass="form-control" TextMode="SingleLine" />
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="EmailAddress" CssClass="col-md-2 control-label">* Email Address: </asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="EmailAddress" ToolTip="Enter your email address." CssClass="form-control" TextMode="SingleLine" style="width:40rem; max-width:40rem;" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="EmailAddress" CssClass="text-danger" Display="Dynamic" ErrorMessage="required" />
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="PhoneNo" CssClass="col-md-2 control-label">* Phone Number: </asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="PhoneNo" ToolTip="Enter your phone number." CssClass="form-control" TextMode="SingleLine" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="PhoneNo" CssClass="text-danger" Display="Dynamic" ErrorMessage="required" />
                </div>
            </div>

            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:Button runat="server" id="btnCancel" OnClick="CancelEdit_Click" Text="Cancel" CssClass="btn btn-sm btn-default" ToolTip="Returns to profile view." TabIndex="-1" />
                    <asp:Button runat="server" ID="btnUpdate" OnClick="UpdateProfile_Click" Text="Update" CssClass="btn btn-primary" ToolTip="Update your profile." style="margin-left:1rem;" />
                </div>
            </div>
        </div>
        <div style="height:3rem;">&nbsp;</div><!-- to move buttons off the bottom of the screen -->
    </section>

    <script>
        $(document).ready(function () {
            $(".selectpicker").selectpicker();
        });
    </script>
</asp:Content>
