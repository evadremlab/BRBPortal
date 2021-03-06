﻿<%@ Page Title="Request Online Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="BRBPortal_CSharp.Account.Register" %>
<%@ MasterType  virtualPath="~/Site.Master"%>

<%@ Import Namespace="Microsoft.AspNet.Identity" %>

<%-- data-lpignore="true" tells LastPass not to show ellipsis on form fields --%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .navbar-nav, .navbar-right { display: none; }
        input[type="text"] { text-transform:uppercase; }
    </style>

    <asp:PlaceHolder runat="server">        
         <%: Scripts.Render("~/bundles/inputmask") %>
    </asp:PlaceHolder>

    <h2><%: Title %></h2>
    <hr />

    <section id="registerForm">
        <div class="form-horizontal">
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="ReqUserID" CssClass="col-md-2 control-label">* User ID:</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox ID="ReqUserID" runat="server" CssClass="form-control" ToolTip="Enter a valid User ID." data-lpignore="true"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ReqUserID" CssClass="text-danger" ErrorMessage="required" />
                </div>    
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="BillCode" CssClass="col-md-2 control-label">* Billing Code: </asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="BillCode" CssClass="form-control" ToolTip="Enter a valid Billing Code." />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="BillCode" CssClass="text-danger" ErrorMessage="required" />
                </div>    
            </div>
            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <div class="radio radiobuttonlist" style="padding-top:0; padding-left:0;">
                        <asp:RadioButtonList runat="server" ID="PropRelate" RepeatDirection="Horizontal" ToolTip="Are you the Property Owner or Property Agent?">
                            <asp:ListItem Enabled="true" Text="Property Owner&nbsp;&nbsp;" Value="Owner" Selected="true"></asp:ListItem>
                            <asp:ListItem Enabled="true" Text="Property Agent" Value="Agent"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>    
            </div>

            <div id="OwnerGrp" runat="server">
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="LastName" CssClass="col-md-2 control-label">* Last Name: </asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="LastName" ToolTip="Enter your last name." CssClass="form-control" TextMode="SingleLine" style="width:40rem; max-width:40rem;" />
                        <asp:CustomValidator ID="CustomLastNameValidator" runat="server" ControlToValidate="LastName" ValidateEmptyText="True" ClientValidationFunction="validateOwnerLastName" CssClass=" text-danger" ErrorMessage="required" />
                    </div>
                </div>
            </div>

            <div id="AgencyGrp" runat="server" style="display:none;">
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="AgencyName" CssClass="col-md-2 control-label">* Agency Name: </asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="AgencyName" CssClass="form-control" ToolTip="Enter the agency name." style="width:40rem; max-width:40rem;" />
                        <asp:CustomValidator ID="CustomAgencyNameValidator" runat="server" ControlToValidate="AgencyName" ValidateEmptyText="True" ClientValidationFunction="validateAgencyName" CssClass=" text-danger" ErrorMessage="required" />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="PropOwnLastName" CssClass="col-md-2 control-label">* Property Owner's Last Name: </asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="PropOwnLastName" CssClass="form-control" ToolTip="Enter the owner's last name." style="width:40rem; max-width:40rem;" />
                        <asp:CustomValidator ID="PropertyOwnerLastNameValidator1" runat="server" ControlToValidate="PropOwnLastName" ValidateEmptyText="True" ClientValidationFunction="validatePropertyOwnerLastName" CssClass=" text-danger" ErrorMessage="required" />
                    </div>
                </div>
            </div>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="StNum" CssClass="col-md-2 control-label">* Street Number: </asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="StNum" ToolTip="Enter the street name of the mailing address." CssClass="form-control" TextMode="SingleLine" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="StNum"  CssClass="text-danger" Display="Dynamic" ErrorMessage="required" />
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="StName" CssClass="col-md-2 control-label">* Street Name: </asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="StName" CssClass="form-control" ToolTip="Enter the street name of the mailing address." style="width:40rem; max-width:40rem;" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="StName" CssClass="text-danger" Display="Dynamic" ErrorMessage="required" />
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="StCity" CssClass="col-md-2 control-label">* City: </asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="StCity" CssClass="form-control" ToolTip="Enter the city of the mailing address." />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="StCity" CssClass="text-danger" Display="Dynamic" ErrorMessage="required" />
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="StState" CssClass="col-md-2 control-label">* State: </asp:Label>
                <div class="radio radiobuttonlist">
                    <div class="col-md-10">
                        <span style="display:inline-block; max-width:12.5rem;">
                            <asp:dropdownlist runat="server" ID="StState" CssClass="form-control selectpicker" ToolTip="Select a state from the list.">
                                <asp:ListItem Enabled="false" Text="select one" Value=""></asp:ListItem>
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
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="EmailAddress" CssClass="col-md-2 control-label">* Email Address: </asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="EmailAddress" CssClass="form-control" ToolTip="Enter your email address." style="width:40rem; max-width:40rem;" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="EmailAddress" CssClass="text-danger" Display="Dynamic" ErrorMessage="required" />
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="PhoneNo" CssClass="col-md-2 control-label">* Phone Number: </asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="PhoneNo" CssClass="form-control" ToolTip="Enter your 10-digit phone number." />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="PhoneNo" CssClass="text-danger" Display="Dynamic" ErrorMessage="required" />
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="PropAddress" CssClass="col-md-2 control-label">* Property Address: </asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="PropAddress" CssClass="form-control" ToolTip="Enter the property address." style="width:40rem; max-width:40rem;" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="PropAddress" CssClass="text-danger" Display="Dynamic" ErrorMessage="required" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:Button runat="server" id="btnBack" UseSubmitBehavior="false" PostBackUrl="~/Default" CausesValidation="false" Text="Cancel" CssClass="btn btn-sm btn-default" ToolTip="Return to Home page." TabIndex="-1" />
                    <asp:Button runat="server" Text="Register" CssClass="btn btn-primary" style="margin-left:1rem;" />
                </div>
            </div>
        </div>
        <div style="height:3rem;">&nbsp;</div><!-- to move buttons off the bottom of the screen -->
    </section>
    
    <script>
        function validateOwnerLastName(source, args) {
            if ($('#MainContent_PropRelate_0').is(':checked')) { // Owner
                args.IsValid = args.Value != '';
            } else {
                args.IsValid = true;
            }
        }

        function validateAgencyName(source, args) {
            if ($('#MainContent_PropRelate_1').is(':checked')) { // Agency
                args.IsValid = args.Value != '';
            } else {
                args.IsValid = true;
            }
        }

        function validatePropertyOwnerLastName(source, args) {
            if ($('#MainContent_PropRelate_1').is(':checked')) { // Agency
                args.IsValid = args.Value != '';
            } else {
                args.IsValid = true;
            }
        }

        $(document).ready(function () {
            $(".selectpicker").selectpicker();

            //$('#MainContent_PhoneNo').inputmask({ mask: '000-000-0000' });

            $('#MainContent_StState').on('loaded.bs.select', function (e) {
                var stateCodeSelected = $('#MainContent_StState').val();

                if (!stateCodeSelected) {
                    $('#MainContent_StState').selectpicker('val', 'CA');
                }
            });

            $('input[type="radio"]').change(function () {
                if ($(this).val() === 'Owner') {
                    $('#MainContent_OwnerGrp').show();
                    $('#MainContent_AgencyGrp').hide();
                    $('#MainContent_AgencyName').val('');
                } else {
                    $('#MainContent_OwnerGrp').hide();
                    $('#MainContent_AgencyGrp').show();
                    $('#MainContent_FirstName, #MainContent_LastName').val('');
                }
            });
        });
    </script>
</asp:Content>
