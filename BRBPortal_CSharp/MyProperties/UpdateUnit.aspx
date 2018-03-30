<%@ Page Title="Unit Status" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="UpdateUnit.aspx.cs" Inherits="BRBPortal_CSharp.MyProperties.UpdateUnit" %>
<%@ MasterType  virtualPath="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hfDialogID" runat="server" />
    <asp:HiddenField ID="hfUnitID" runat="server" />

    <style>
        html, body { overflow-y: scroll; } /* always show scrollbar so the controls don't jump when hiding/showing */
        .table { margin-bottom: 0; border: none; }
        .table th, .table td {  border-top: none !important; padding: 4px !important; }
        .table .radio { padding-left: 0; }
    </style>

    <h2><%: Title %></h2>
    <h4>at <asp:Literal ID="MainAddress" runat="server"></asp:Literal>, Unit # <asp:Literal ID="UnitNo" runat="server" ></asp:Literal></h4>
    <hr />

    <div class="form-horizontal">
        <section id="updateUnitForm">
            <div class="form-horizontal">
                <!-- HEADER -->
                <div class="row">
                    <div class="col-md-offset-1 col-md-11">
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="UnitStatus" CssClass="control-label">Unit Status:&nbsp;</asp:Label>
                            <asp:Literal ID="UnitStatus" runat="server"></asp:Literal>
                            <br />
                            <asp:PlaceHolder ID="CurrentRental" runat="server">
                                <asp:Label runat="server" AssociatedControlID="UnitStartDt" CssClass="control-label">Date Started:&nbsp;</asp:Label>
                                <asp:Literal ID="UnitStartDt" runat="server"></asp:Literal>
                                <br />
                                <asp:Label runat="server" AssociatedControlID="UnitOccBy" CssClass="control-label">Occupied By:&nbsp;</asp:Label>
                                <asp:Literal ID="UnitOccBy" runat="server"></asp:Literal>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="CurrentExemption" runat="server">
                                <asp:Label runat="server" AssociatedControlID="ExemptReas" CssClass="control-label">Exemption Reason:&nbsp;</asp:Label>
                                <asp:Literal ID="ExemptReas" runat="server"></asp:Literal>
                                <br />
                            </asp:PlaceHolder>
                        </div>
                    </div>
                </div>
                <!-- BUTTONS -->
                <div class="row">
                    <div class="col-md-offset-1 col-md-11">
                        <div id="InitalEditButtons" runat="server" class="form-group">
                            <asp:Button runat="server" id="btnBack" PostBackUrl="~/MyProperties/MyUnits" Text="Back" CssClass="btn btn-sm btn-default" ToolTip="Returns to list of units." TabIndex="-1" />
                            <button id="btnEdit" type="button" class="btn btn-primary" style="margin-left:1rem;">Edit Unit Status</button>
                        </div>
                    </div>
                </div>
                <!-- PANELS -->
                <div class="row" id="EditUnitStatusPanel" runat="server" style="display:none; margin-bottom:1rem;">
                    <div class="col-md-offset-1 col-md-11">
                        <div class="pull-left" style="width:30rem;">
                            <div class="form-group">
                                <h4>New Unit Status</h4>
                                <div class="radio radiobuttonlist">
                                    <asp:RadioButtonList runat="server" ID="NewUnit" RepeatDirection="Horizontal" ToolTip="Select unit status." CellPadding="4" style="position:relative; top:-0.4rem; left:-0.4rem;">
                                        <asp:ListItem Enabled="true" Text="Rented" Value="Rented"></asp:ListItem>
                                        <asp:ListItem Enabled="true" Text="Exempt" Value="Exempt"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                        </div>
                        <div class="pull-left">
                            <div class="form-group" id="ExemptGroup" runat="server">
                                <asp:Label runat="server" AssociatedControlID="ExemptReason" CssClass="control-label">Exemption Reason: </asp:Label>
                                <div class="radio radiobuttonlist">
                                    <asp:RadioButtonList runat="server" ID="ExemptReason" RepeatDirection="Vertical" ToolTip="Select exemption reason.">
                                        <asp:ListItem Enabled="true" Text="Vacant and not available for rent" Value="NAR"></asp:ListItem>
                                        <asp:ListItem Enabled="true" Text="Owner-Occupied" Value="OOCC"></asp:ListItem>
                                        <asp:ListItem Enabled="true" Text="Section 8" Value="SEC8"></asp:ListItem>
                                        <asp:ListItem Enabled="true" Text="Occupied Rent Free" Value="FREE"></asp:ListItem>
                                        <asp:ListItem Enabled="true" Text="Other" Value="OTHER"></asp:ListItem>
                                    </asp:RadioButtonList>
                                    <div runat="server" id="OtherListContainer">
                                        <asp:dropdownlist runat="server" ID="OtherList" RepeatDirection="Vertical" ToolTip="Select a reason from the list (optional)." CssClass="form-control selectpicker" style="width:auto;">
                                            <%--<asp:ListItem enabled="true" text="" value="-1"></asp:ListItem>--%>
                                            <asp:ListItem enabled="true" text="Commercial Use" value="COMM"></asp:ListItem>
                                            <asp:ListItem enabled="true" text="Property Manager's Unit" value="MISC"></asp:ListItem>
                                            <asp:ListItem enabled="true" text="Owner shares kitchen & bath with tenant" value="SHARED"></asp:ListItem>
                                            <asp:ListItem enabled="true" text="Shelter Plus Care" value="SPLUS"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- FIELDS-->
                <div class="row" runat="server" id="AsOfDtGrp">
                    <div class="col-md-offset-1 col-md-11">
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="UnitAsOfDt" CssClass="control-label" style="min-width:10rem; text-align:left;">As of Date: </asp:Label>
                            <asp:TextBox runat="server" ID="UnitAsOfDt" TextMode="Date" CssClass="form-control" ToolTip="Enter the as of date for this change." />
                        </div>
                    </div>
                </div>

                <div class="row" runat="server" id="DtStrtdGrp">
                    <div class="col-md-offset-1 col-md-11">
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="StartDt" CssClass="control-label" style="min-width:10rem; text-align:left;">Date Started: </asp:Label>
                            <asp:TextBox runat="server" ID="StartDt" TextMode="Date" CssClass="form-control" ToolTip="Enter the start date." />
                        </div>
                    </div>
                </div>

                <div class="row" runat="server" id="OccByGrp">
                    <div class="col-md-offset-1 col-md-11">
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="OccupiedBy" CssClass="control-label" style="min-width:10rem; text-align:left;">Occupied By: </asp:Label>
                            <asp:TextBox runat="server" ID="OccupiedBy" CssClass="form-control" ToolTip="Enter name of tenant." data-lpignore="true" />
                        </div>
                    </div>
                </div>

                <div class="row" runat="server" id="ContractGrp">
                    <div class="col-md-offset-1 col-md-11">
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="ContractNo" CssClass="control-label" style="min-width:10rem; text-align:left;">Contract #: </asp:Label>
                            <asp:TextBox runat="server" ID="ContractNo" CssClass="form-control" ToolTip="Enter any contract number." />
                        </div>
                    </div>
                </div>
                
                <div class="row" runat="server" id="CommUseGrp">
                    <div class="col-md-offset-1 col-md-11">
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="CommUseDesc" CssClass="control-label">Please describe the current commercial use of this unit: </asp:Label>
                            <br />
                            <asp:TextBox runat="server" ID="CommUseDesc" TextMode="MultiLine" CssClass="form-control" style="width:80rem; height:7.5rem;" ToolTip="Enter commercial use description." />
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="RB1" CssClass="control-label">Is the property zoned for commercial use?</asp:Label>
                                <div class="radio radiobuttonlist" style="display:inline-block;">
                                    <asp:RadioButtonList runat="server" ID="RB1" RepeatDirection="Horizontal" ToolTip="Select Yes or No." CellPadding="4" style="position:relative; top:-0.4rem; left:-0.4rem;">
                                    <asp:ListItem Enabled="true" Text="Yes" Value="Yes"></asp:ListItem>
                                    <asp:ListItem Enabled="true" Text="No" Value="No"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="CommZoneUse" CssClass="control-label">Enter commercial zoning description: </asp:Label>
                            <br />
                            <asp:TextBox runat="server" ID="CommZoneUse" TextMode="MultiLine" CssClass="form-control" style="width:80rem; height:7.5rem;" ToolTip="Enter commercial zoning description." />
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="CommResYN" CssClass="control-label">Is the unit used exclusively for commercial use?</asp:Label>
                            <div class="radio radiobuttonlist" style="display:inline-block;">
                                <asp:RadioButtonList runat="server" ID="CommResYN" RepeatDirection="Horizontal" ToolTip="Select Yes or No." CellPadding="4" style="position:relative; top:-0.4rem; left:-0.4rem;">
                                    <asp:ListItem Enabled="true" Text="Yes" Value="Yes"></asp:ListItem>
                                    <asp:ListItem Enabled="true" Text="No" Value="No"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row" id="PMUnitGrp" runat="server">
                    <div class="col-md-offset-1 col-md-11">
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="PropMgrName" CssClass="control-label">Name(s) of the Property Manager residing in the unit: </asp:Label>
                            <br />
                            <asp:TextBox runat="server" ID="PropMgrName" TextMode="MultiLine" CssClass="form-control" style="width:80rem;" ToolTip="Enter the property manager name(s)." />
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="PMEmailPhone" CssClass="control-label">Email address and/or phone number for Property Manager: </asp:Label>
                            <br />
                            <asp:TextBox runat="server" ID="PMEmailPhone" TextMode="MultiLine" CssClass="form-control" style="width:80rem;" ToolTip="Enter the property manager email and/or phone number." />
                        </div>
                    </div>
                </div>                            

                <div class="row" id="OwnerShrGrp" runat="server">
                    <div class="col-md-offset-1 col-md-11">
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="PrincResYN" CssClass="control-label">Is this unit the owner's principal place of residence?</asp:label>
                            <div class="radio radiobuttonlist" style="display:inline-block;">
                                <asp:RadioButtonList runat="server" ID="PrincResYN" RepeatDirection="Horizontal" ToolTip="Select Yes or No." CellPadding="4" style="position:relative; top:-0.4rem; left:-0.4rem;">
                                    <asp:ListItem Enabled="true" Text="Yes" Value="Yes"></asp:ListItem>
                                    <asp:ListItem Enabled="true" Text="No" Value="No"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="MultiUnitYN" CssClass="control-label">Does the owner reside in any other unit on the property other than this unit?</asp:label>
                            <div class="radio radiobuttonlist" style="display:inline-block;">
                                <asp:RadioButtonList runat="server" ID="MultiUnitYN" RepeatDirection="Horizontal" ToolTip="Select Yes or No." CellPadding="4" style="position:relative; top:-0.4rem; left:-0.4rem;">
                                    <asp:ListItem Enabled="true" Text="Yes" Value="Yes"></asp:ListItem>
                                    <asp:ListItem Enabled="true" Text="No" Value="No"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="OtherUnits" CssClass="control-label">If Yes, please indicate which unit(s): </asp:Label>
                            <asp:TextBox runat="server" ID="OtherUnits" TextMode="SingleLine" CssClass="form-control" ToolTip="Enter the other units the owner occupies." style="width:38rem; max-width:38rem;" />
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="TenantNames" CssClass="control-label">Name and contact information of the tenants residing in the unit?  Name(s): </asp:Label>
                            <br />
                            <asp:TextBox runat="server" ID="TenantNames" TextMode="MultiLine" CssClass="form-control" style="width:80rem; height:7.5rem;" ToolTip="Enter the names of the tenants." />
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="TenantContacts" CssClass="control-label">Contact info: </asp:Label>
                            <br />
                            <asp:TextBox runat="server" ID="TenantContacts" TextMode="MultiLine" CssClass="form-control" style="width:80rem; height:7.5rem;" ToolTip="Enter the contact information for the tenants." />
                        </div>
                    </div>
                </div>

                <div class="row hidden" id="DeclareAndSubmit" runat="server">
                    <div class="col-md-offset-1 col-md-11">
                        <div class="form-group">
                            <asp:CheckBox ID="chkDeclare" runat="server" Text="&nbsp;Declaration: I hereby declare under penalty of perjury that .."  />
                        </div>

                        <div class="form-group">
                            <Label runat="server" AssociatedControlID="DeclareInits" CssClass="control-label">Declaration initials:&nbsp;</Label>
                            <asp:TextBox runat="server" ID="DeclareInits" Width="70px" CssClass="form-control" ToolTip="Enter your initials acknowledging the Declaration above." />
                        </div>

                        <div class="form-group">
                            <asp:Button runat="server" id="btnCancel" Text="Cancel" OnClick="btnCancel_Click" CssClass="btn btn-sm btn-default" ToolTip="Returns to list of units." TabIndex="-1" />
                            <asp:Button runat="server" ID="btnConfirm" Text="Confirm" OnClick="btnConfirm_Click" OnClientClick="return validate();" CssClass="btn btn-primary" ToolTip="Update this unit." style="margin-left:1rem;"/>
                        </div>
                    </div>
                </div>
            </div>
        </section>
        <div style="height:3rem;">&nbsp;</div><!-- to move buttons off the bottom of the screen -->
    </div>

    <script>
        var valErrors = [];

        function addValError(msg) {
            valErrors.push('<li>' + msg + '</li>');
        }

        function validate()
        {
            valErrors = [];
            var el$ = {
                UnitAsOfDt: $('#<%=UnitAsOfDt.ClientID %>'),
                OtherList: $('#<%=OtherList.ClientID %>'),
                StartDt: $('#<%=StartDt.ClientID %>'),
                OccupiedBy: $('#<%=OccupiedBy.ClientID %>'),
                ContractNo: $('#<%=ContractNo.ClientID %>'),
                CommUseDesc: $('#<%=CommUseDesc.ClientID %>'),
                CommZoneUse: $('#<%=CommZoneUse.ClientID %>'),
                PropMgrName: $('#<%=PropMgrName.ClientID %>'),
                PMEmailPhone: $('#<%=PMEmailPhone.ClientID %>'),
                OtherUnits: $('#<%=OtherUnits.ClientID %>'),
                TenantNames: $('#<%=TenantNames.ClientID %>'),
                TenantContacts: $('#<%=TenantContacts.ClientID %>')
            };

            try {
                if ($('#MainContent_NewUnit_1').is(':checked')) { // Exempt
                    validateExemptions(el$);
                } else { // Rented
                    if (el$.UnitAsOfDt.val() === '') {
                        addValError('As of Date must be entered.');
                    }
                }
                if (valErrors.length) {
                    showErrorDialog(('<ul>' + valErrors.join('') + '</ul>'), "Validation Errors");
                    return false;
                }
            } catch (ex) {
                showErrorDialog(ex.message, "Validation Errors");
                return false;
            }

            return true;
        }

        function validateExemptions(el$) {
            if ($('#MainContent_ExemptReason_0').is(':checked')) { // Vacant and not available for rent
                if (el$.UnitAsOfDt.val() === '') {
                    addValError('As of Date must be entered.');
                }
            } else if ($('#MainContent_ExemptReason_1').is(':checked')) { // Owner-Occupied
                if (el$.StartDt.val() === '') {
                    addValError('Date Started must be entered.');
                }
                if (el$.OccupiedBy.val() === '') {
                    addValError('Occupied By must be entered.');
                }
            } else if ($('#MainContent_ExemptReason_2').is(':checked')) { // Section 8
                if (el$.StartDt.val() === '') {
                    addValError('Date Started must be entered.');
                }
                if (el$.ContractNo.val() === '') {
                    addValError('Contract # must be entered.');
                }
            } else if ($('#MainContent_ExemptReason_3').is(':checked')) { // Occupied Rent Free
                if (el$.StartDt.val() === '') {
                    addValError('Date Started must be entered.');
                }
                if (el$.OccupiedBy.val() === '') {
                    addValError('Occupied By must be entered.');
                }
            } else if ($('#MainContent_ExemptReason_4').is(':checked')) { // Other
                switch (el$.OtherList.val()) {
                    case 'COMM': // Commercial Use
                        if (el$.StartDt.val() === '') {
                            addValError('Date Started must be entered.');
                        }
                        if (el$.CommUseDesc.val() === '') {
                            addValError('Description of the current commercial use must be entered.');
                        }
                        if ($('#MainContent_RB1_0').is(':checked') && el$.CommZoneUse.val() === '') {
                            addValError('When zoned for commercial use is Yes the description must also be entered.');
                        }
                        break;
                    case 'MISC': // Property Managers Unit
                        if (el$.StartDt.val() === '') {
                            addValError('Date Started must be entered.');
                        }
                        if (el$.PropMgrName.val() === '') {
                            addValError('Property manager name must be entered.');
                        }
                        if (el$.PMEmailPhone.val() === '') {
                            addValError('Property manager email/phone must be entered.');
                        }
                        break;
                    case 'SHARED': // Owner share kitchen & bath with tenant
                        if (el$.StartDt.val() === '') {
                            addValError('Date Started must be entered.');
                        }
                        if ($('#MainContent_MultiUnitYN_0').is(':checked') && el$.OtherUnits.val() === '') {
                            addValError('If the owner resides in more than one unit you must also state which units they reside in.');
                        }
                        if (el$.TenantNames.val() === '' || el$.TenantContacts.val() === '') {
                            addValError('Tenant name(s) and contact information must be entered.');
                        }
                        break;
                    case 'SPLUS': // Shelter plus care
                        if (el$.StartDt.val() === '') {
                            addValError('Date Started must be entered.');
                        }
                        if (el$.ContractNo.val() === '') {
                            addValError('Contract # must be entered.');
                        }
                        break;
                    default:
                        addValError('An Other Exemption Reason must be selected.');
                }
            } else {
                addValError('An Exemption Reason must be selected.');
            }

            el$ = null; // release references
        }

        $(document).ready(function () {
            function _setExemptReasonFields() {
                switch ($('#MainContent_ExemptReason input:checked').val()) {
                    case "NAR": // Vacant and not available for rent
                        $('#MainContent_ExemptGroup').removeClass('hidden');
                        $('#MainContent_CommUseGrp').addClass('hidden');
                        $('#MainContent_PMUnitGrp').addClass('hidden');
                        $('#MainContent_OwnerShrGrp').addClass('hidden');
                        $('#MainContent_AsOfDtGrp').removeClass('hidden');
                        $('#MainContent_DtStrtdGrp').addClass('hidden');
                        $('#MainContent_OccByGrp').addClass('hidden');
                        $('#MainContent_ContractGrp').addClass('hidden');
                        $('#MainContent_OtherListContainer').addClass('hidden');
                        $('#MainContent_OtherList').val('');
                        break;
                    case "OOCC": // Owner-Occupied
                        $('#MainContent_ExemptGroup').removeClass('hidden');
                        $('#MainContent_CommUseGrp').addClass('hidden');
                        $('#MainContent_PMUnitGrp').addClass('hidden');
                        $('#MainContent_OwnerShrGrp').addClass('hidden');
                        $('#MainContent_AsOfDtGrp').addClass('hidden');
                        $('#MainContent_DtStrtdGrp').removeClass('hidden');
                        $('#MainContent_OccByGrp').removeClass('hidden');
                        $('#MainContent_ContractGrp').addClass('hidden');
                        $('#MainContent_OtherListContainer').addClass('hidden');
                        $('#MainContent_OtherList').val('');
                        break;
                    case "SEC8": // Section 8
                        $('#MainContent_ExemptGroup').removeClass('hidden');
                        $('#MainContent_CommUseGrp').addClass('hidden');
                        $('#MainContent_PMUnitGrp').addClass('hidden');
                        $('#MainContent_OwnerShrGrp').addClass('hidden');
                        $('#MainContent_AsOfDtGrp').addClass('hidden');
                        $('#MainContent_DtStrtdGrp').removeClass('hidden');
                        $('#MainContent_OccByGrp').addClass('hidden');
                        $('#MainContent_ContractGrp').removeClass('hidden');
                        $('#MainContent_OtherListContainer').addClass('hidden');
                        $('#MainContent_OtherList').val('');
                        break;
                    case "FREE": // Occupied Rent Free
                        $('#MainContent_ExemptGroup').removeClass('hidden');
                        $('#MainContent_CommUseGrp').addClass('hidden');
                        $('#MainContent_PMUnitGrp').addClass('hidden');
                        $('#MainContent_OwnerShrGrp').addClass('hidden');
                        $('#MainContent_AsOfDtGrp').addClass('hidden');
                        $('#MainContent_DtStrtdGrp').removeClass('hidden');
                        $('#MainContent_OccByGrp').removeClass('hidden');
                        $('#MainContent_ContractGrp').addClass('hidden');
                        $('#MainContent_OtherListContainer').addClass('hidden');
                        $('#MainContent_OtherList').val('');
                        break;
                    case "OTHER":
                        $('#MainContent_ExemptGroup').removeClass('hidden');
                        $('#MainContent_CommUseGrp').addClass('hidden');
                        $('#MainContent_PMUnitGrp').addClass('hidden');
                        $('#MainContent_OwnerShrGrp').addClass('hidden');
                        $('#MainContent_AsOfDtGrp').addClass('hidden');
                        $('#MainContent_DtStrtdGrp').addClass('hidden');
                        $('#MainContent_OccByGrp').addClass('hidden');
                        $('#MainContent_ContractGrp').addClass('hidden');
                        $('#MainContent_OtherListContainer').removeClass('hidden');
                        $('#MainContent_OtherList').val('');
                        break;
                }
            }

            function _setNewUnitfields() {
                if ($('#MainContent_NewUnit input:checked').val() === 'Rented') {
                    $('#MainContent_ExemptGroup').addClass('hidden');
                    $('#MainContent_ExemptReason').addClass('hidden');
                    $('#MainContent_OtherListContainer').addClass('hidden');
                    $('#MainContent_ExemptReason').val('');
                    $('#MainContent_OtherList').val('');
                    $('#MainContent_AsOfDtGrp').removeClass('hidden');
                } else {
                    $('#MainContent_ExemptGroup').removeClass('hidden');
                    $('#MainContent_ExemptReason').removeClass('hidden');
                    $('#MainContent_OtherListContainer').removeClass('hidden');
                    $('#MainContent_ExemptReason').val('');
                    $('#MainContent_OtherList').val('');
                    $('#MainContent_AsOfDtGrp').addClass('hidden');
                }

                _setExemptReasonFields();
            }

            function _otherListChanged() {
                switch ($(this).val()) {
                    case "COMM": // Commercial Use
                        $('#MainContent_CommUseGrp').removeClass('hidden'); // Visible = True
                        $('#MainContent_DtStrtdGrp').removeClass('hidden'); // Visible = True
                        $('#MainContent_ContractGrp').addClass('hidden');   // Visible = False
                        $('#MainContent_PMUnitGrp').addClass('hidden');     // Visible = False
                        $('#MainContent_OwnerShrGrp').addClass('hidden');   // Visible = False
                        break;
                    case "MISC": // Property Managers Unit
                        $('#MainContent_CommUseGrp').addClass('hidden');    // Visible = False
                        $('#MainContent_DtStrtdGrp').removeClass('hidden'); // Visible = True
                        $('#MainContent_ContractGrp').addClass('hidden');   // Visible = False
                        $('#MainContent_PMUnitGrp').removeClass('hidden');  // Visible = True
                        $('#MainContent_OwnerShrGrp').addClass('hidden');   // Visible = False
                        break;
                    case "SHARED": // Owner share kitchen & bath with tenant
                        $('#MainContent_CommUseGrp').addClass('hidden');    // Visible = False
                        $('#MainContent_DtStrtdGrp').removeClass('hidden'); // Visible = True
                        $('#MainContent_ContractGrp').addClass('hidden');   // Visible = False
                        $('#MainContent_PMUnitGrp').addClass('hidden');     // Visible = False
                        $('#MainContent_OwnerShrGrp').removeClass('hidden');// Visible = True
                        break;
                    case "SPLUS": // Shelter plus care
                        $('#MainContent_CommUseGrp').addClass('hidden');    // Visible = False
                        $('#MainContent_DtStrtdGrp').removeClass('hidden'); // Visible = True
                        $('#MainContent_ContractGrp').removeClass('hidden');// Visible = True
                        $('#MainContent_PMUnitGrp').addClass('hidden');     // Visible = False
                        $('#MainContent_OwnerShrGrp').addClass('hidden');   // Visible = False
                        break;
                }
            }

            function _rb1Changed() {
                if ($(this).val() == 'No') {
                    $('#MainContent_CommZoneUse').val('');
                }
            }

            function _enableConfirmButton() {
                var isChecked = $('#MainContent_chkDeclare').is(':checked');
                var hasInitials = $('#MainContent_DeclareInits').val().length;
                $('#MainContent_btnConfirm').attr('disabled', (isChecked && hasInitials) ? false : true);
            }

            $(".selectpicker").selectpicker();
            $('#MainContent_btnConfirm').attr('disabled', true); // initial state

            $('#btnEdit').click(function () {
                $('#MainContent_InitalEditButtons').hide();
                $('#MainContent_EditUnitStatusPanel').show();
                $('#MainContent_AsOfDtGrp').removeClass('hidden');
                $('#MainContent_DeclareAndSubmit').removeClass('hidden');
            });

            $('#MainContent_NewUnit').change(_setNewUnitfields);
            $('#MainContent_ExemptReason').change(_setExemptReasonFields);
            $('#MainContent_OtherList').change(_otherListChanged);
            $('#MainContent_RB1').change(_rb1Changed);
            $('#MainContent_chkDeclare').change(_enableConfirmButton);
            $('#MainContent_DeclareInits').change(_enableConfirmButton);
        });
    </script>
</asp:Content>