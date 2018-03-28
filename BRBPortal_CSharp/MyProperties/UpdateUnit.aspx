<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="UpdateUnit.aspx.cs" Inherits="BRBPortal_CSharp.MyProperties.UpdateUnit" %>
<%@ MasterType  virtualPath="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hfDialogID" runat="server" />
    <asp:HiddenField ID="hfUnitID" runat="server" />

    <style>
        html, body { min-height: 101%; } /* always show scrollbar so the controls don't jump when hiding/showing */
        .table { margin-bottom: 0; border: none; }
        .table th, .table td {  border-top: none !important; padding: 4px !important; }
        .table .radio { padding-left: 0; }
    </style>

    <h2>Unit Status</h2>

    <div class="form-horizontal">
        <section id="updateUnitForm">
            <div class="form-horizontal">
                <h4>at <asp:Literal ID="MainAddress" runat="server"></asp:Literal>, Unit # <asp:Literal ID="UnitNo" runat="server" ></asp:Literal></h4>
                <hr />

                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="UnitStatus" CssClass="control-label">Unit Status: </asp:Label>
                    <asp:Literal ID="UnitStatus" runat="server" ></asp:Literal>
                    <br />
                    <asp:Label runat="server" AssociatedControlID="ExemptReas" CssClass="control-label">Exemption Reason: </asp:Label>
                    <asp:Literal ID="ExemptReas" runat="server" ></asp:Literal>
                    <br />
                    <asp:Label runat="server" AssociatedControlID="UnitStartDt" CssClass="control-label">Date Started: </asp:Label>
                    <asp:Literal ID="UnitStartDt" runat="server"></asp:Literal>
                    <br />
                    <asp:Label runat="server" AssociatedControlID="UnitOccBy" CssClass="control-label">Occupied By: </asp:Label>
                    <asp:Literal ID="UnitOccBy" runat="server"></asp:Literal>
                </div>

                <div id="InitalEditButtons" runat="server" class="form-group">
                    <asp:Button runat="server" id="btnBack" PostBackUrl="~/MyProperties/MyUnits" Text="Back" CssClass="btn btn-sm btn-default" ToolTip="Returns to list of units." TabIndex="-1" />
                    <button id="btnEdit" type="button" class="btn btn-primary" style="margin-left:1rem;">Edit Unit Status</button>
                </div>

                <div id="EditUnitStatusPanel" runat="server" style="display:none;">
                    <div class="table table-responsive">
                        <div style="display:table-cell; min-width:20rem;">
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="NewUnit" CssClass="control-label">New Unit Status: </asp:Label>
                                <div class="radio radiobuttonlist">
                                    <asp:RadioButtonList runat="server" ID="NewUnit" RepeatDirection="Horizontal" ToolTip="Select unit status." CellPadding="4" style="position:relative; top:-0.4rem; left:-0.4rem;">
                                        <asp:ListItem Enabled="true" Text="Rented" Value="Rented"></asp:ListItem>
                                        <asp:ListItem Enabled="true" Text="Exempt" Value="Exempt"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                        </div>
                        <div style="display:table-cell;">
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

                    <div class="row" runat="server" id="AsOfDtGrp">
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="UnitAsOfDt" CssClass="control-label" style="min-width:10rem; text-align:left;">As of Date: </asp:Label>
                            <asp:TextBox runat="server" ID="UnitAsOfDt" TextMode="Date" CssClass="form-control" ToolTip="Enter the as of date for this change."  />
                        </div>
                    </div>

                    <div class="row" runat="server" id="DtStrtdGrp">
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="StartDt" CssClass="control-label" style="min-width:10rem; text-align:left;">Date Started: </asp:Label>
                            <asp:TextBox runat="server" ID="StartDt" TextMode="Date" CssClass="form-control" ToolTip="Enter the start date." />
                        </div>
                    </div>

                    <div class="row" runat="server" id="OccByGrp">
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="OccupiedBy" CssClass="control-label" style="min-width:10rem; text-align:left;">Occupied By: </asp:Label>
                            <asp:TextBox runat="server" ID="OccupiedBy" CssClass="form-control" ToolTip="Enter name of tenant." data-lpignore="true" />
                        </div>
                    </div>

                    <div class="row" runat="server" id="ContractGrp">
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="ContractNo" CssClass="control-label" style="min-width:10rem; text-align:left;">Contract #: </asp:Label>
                            <asp:TextBox runat="server" ID="ContractNo" CssClass="form-control" ToolTip="Enter any contract number." />
                        </div>
                    </div>
                
                    <div class="row" runat="server" id="CommUseGrp">
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="CommUseDesc" CssClass="control-label">Please describe the current commercial use of this unit: </asp:Label>
                            <br />
                            <asp:TextBox runat="server" ID="CommUseDesc" TextMode="MultiLine" CssClass="form-control" style="width:80rem; height:7.5rem;" ToolTip="Enter commercial use description." />
                        </div>
                        <asp:Label runat="server" AssociatedControlID="RB1" CssClass="control-label">Is the property zoned for commercial use?</asp:Label>
                            <div class="radio radiobuttonlist" style="display:inline-block;">
                                <asp:RadioButtonList runat="server" ID="RB1" RepeatDirection="Horizontal" ToolTip="Select Yes or No." CellPadding="4" style="position:relative; top:-0.4rem; left:-0.4rem;">
                                <asp:ListItem Enabled="true" Text="Yes" Value="Yes"></asp:ListItem>
                                <asp:ListItem Enabled="true" Text="No" Value="No"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
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

                    <div runat="server" id="PMUnitGrp">
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

                    <div id="OwnerShrGrp" runat="server">
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

                    <div class="table table-responsive" style="margin-top:2rem;">
                        <div class="table-cell">
                            <div class="form-group">
                                <asp:CheckBox ID="chkDeclare" runat="server" Text="&nbsp;Declaration: I hereby declare under penalty of perjury that .."  />
                            </div>

                            <div class="form-group">
                                <Label runat="server" AssociatedControlID="DeclareInits" CssClass="control-label">Declaration initials: </Label>
                                <asp:TextBox runat="server" ID="DeclareInits" Width="70px" CssClass="form-control" ToolTip="Enter your initials acknowledging the Declaration above." />
                            </div>

                            <div class="form-group">
                                <asp:Button runat="server" id="btnCancel" OnClick="btnCancel_Click" Text="Cancel" CssClass="btn btn-sm btn-default" ToolTip="Returns to list of units." TabIndex="-1" />
                                <asp:Button runat="server" ID="btnConfirm" OnClick="btnConfirm_Click" Text="Confirm" CssClass="btn btn-primary" ToolTip="Update this unit." style="margin-left:1rem;" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
        <div style="height:3rem;">&nbsp;</div><!-- to move buttons off the bottom of the screen -->
    </div>

    <script>
        function AsOfDate_ClientValidate(sender, e) {
            var foo = $('#MainContent_NewUnit_1').is(':checked');
            //MainContent_OtherList
            alert('is checked: ' + foo);
            e.IsValid = false;
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

            $('#btnEdit').click(function () {
                $('#MainContent_InitalEditButtons').hide();
                $('#MainContent_EditUnitStatusPanel').fadeIn();
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