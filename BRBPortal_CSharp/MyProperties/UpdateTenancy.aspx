<%@ Page Title="Update Tenancy" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UpdateTenancy.aspx.cs" Inherits="BRBPortal_CSharp.MyProperties.UpdateTenancy" %>
<%@ MasterType  virtualPath="~/Site.Master"%>

<%--data-lpignore="true" tells LastPass not to show ellipsis on form fields--%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        html, body { height: 101%; } /* always show scrollbar so the controls don't shife when hiding/showing fields */
        #MainContent_HServs { min-width: 35rem; margin-top: 0.7rem; }
        #MainContent_HServs label { padding-left: 0.5rem; }
    </style>

    <asp:HiddenField ID="hdnRemovedTenantIDs" runat="server" />
    <asp:HiddenField ID="hdnDelimitedTenants" runat="server" />

    <h2><%: Title %></h2>
    <h4>at <asp:Literal ID="MainAddress" runat="server"></asp:Literal>, Unit # <asp:Literal ID="UnitNo" runat="server" ></asp:Literal></h4>
    <hr />

    <section id="updateTenancyForm">
        <div class="form-horizontal">
            <!-- HEADER -->
            <div class="row">
                <asp:Label runat="server" AssociatedControlID="OwnerName" CssClass="col-md-2 control-label">Owner Name:&nbsp;</asp:Label>
                <div class="col-md-10 literal">
                    <asp:Literal ID="OwnerName" runat="server"></asp:Literal>
                </div>
            </div>
            <div id="AgencyNameSection" runat="server" class="row">
                <asp:Label runat="server" AssociatedControlID="AgentName" CssClass="col-md-2 control-label">Agent Name:&nbsp;</asp:Label>
                <div class="col-md-10 literal">
                    <asp:Literal ID="AgentName" runat="server" ></asp:Literal>
                </div>
            </div>
            <div class="row">
                <asp:Label runat="server" AssociatedControlID="BalAmt" CssClass="col-md-2 control-label">Total Balance:&nbsp;</asp:Label>
                <div class="col-md-10 literal">
                    <asp:Literal ID="BalAmt" runat="server" ></asp:Literal>
                </div>                
            </div>
            <div class="row">
                <asp:Label runat="server" AssociatedControlID="UnitStatus" CssClass="col-md-2 control-label">Unit Status:&nbsp;</asp:Label>
                <div class="col-md-10 literal">
                    <asp:Literal ID="UnitStatus" runat="server"></asp:Literal>
                </div>
            </div>
            <!-- FIELDS -->
            <div class="form-group" style="margin-top:1rem;">
                <asp:Label runat="server" AssociatedControlID="InitRent" CssClass="col-md-2 control-label">Initial Rent: $</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="InitRent" CssClass="form-control" TextMode="SingleLine" data-lpignore="true"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="PTenDt" CssClass="col-md-2 control-label">Prior Tenancy End Date:&nbsp;</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="PTenDt" TextMode="Date" ToolTip="Enter the Prior Tenancy Date." CssClass="form-control" style="width:16rem;" />
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="TermReas" CssClass="col-md-2 control-label">Reason for Termination:&nbsp;</asp:Label>
                <div class="col-md-10" style="max-width:24.5rem;">
                    <asp:DropDownList runat="server" ID="TermReas" ToolTip="Select a termination reason." CssClass="form-control selectpicker">
                        <asp:ListItem Enabled="false" Text="select one" Value=""></asp:ListItem>
                        <asp:ListItem enabled="true" text="Voluntary Vacancy" value="1"></asp:ListItem>
                        <asp:ListItem enabled="true" text="Landlord move in" value="2"></asp:ListItem>
                        <asp:ListItem enabled="true" text="Non-payment of rent" value="3"></asp:ListItem>
                        <asp:ListItem enabled="true" text="Other" value="4"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div id="ExplainOtherTermination" runat="server" class="form-group">
                <asp:Label runat="server" AssociatedControlID="TermDescr" CssClass="col-md-2 control-label">Explain Involuntary termination:</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="TermDescr" TextMode="MultiLine" MaxLength="200" CssClass="form-control" style="width:80rem; height:5.5rem;"  ToolTip="Enter the termination explaination." />
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="TenStDt" CssClass="col-md-2 control-label">Tenancy Start Date:</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="TenStDt" CssClass="form-control" TextMode="Date" style="width:16rem;"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="HServs" CssClass="col-md-2 control-label">Housing Services: </asp:Label>
                <div class="col-md-10" style="max-width:15.5rem;">
                    <asp:CheckBoxList runat="server" ID="HServs" RepeatColumns="3" RepeatDirection="Horizontal" CellSpacing="0">
                        <asp:ListItem Text="Storage"></asp:ListItem>
                        <asp:ListItem Text="Gas"></asp:ListItem>
                        <asp:ListItem Text="Electricity"></asp:ListItem>
                        <asp:ListItem Text="Water"></asp:ListItem>
                        <asp:ListItem Text="Garbage"></asp:ListItem>
                        <asp:ListItem Text="Parking"></asp:ListItem>
                        <asp:ListItem Text="Laundry Access"></asp:ListItem>
                        <asp:ListItem Text="Heat"></asp:ListItem>
                        <asp:ListItem Text="Appliances"></asp:ListItem>
                        <asp:ListItem Text="Other"></asp:ListItem>
                    </asp:CheckBoxList>
                </div>
            </div><!-- do not save "Other" as HServs, take HServOthrBox-->
            <div id="OtherHousingServices" runat="server" class="form-group">
                <div class="col-md-2"></div>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="HServOthrBox" CssClass="form-control" placeholder="Other Housing Services" data-lpignore="true" style="width:40rem; min-width:40rem;"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="RB1" CssClass="col-md-2 control-label">Does Lease Prohibit Smoking?</asp:Label>
                <div class="col-md-10">
                    <div class="radio radiobuttonlist" style="display:inline-block; padding-left:0.25rem;">
                        <asp:RadioButtonList runat="server" ID="RB1" RepeatDirection="Horizontal" ToolTip="Select Yes or No." CellPadding="4" style="position:relative; top:-0.4rem; left:-0.4rem;">
                            <asp:ListItem Enabled="true" Text="Yes" Value="Yes"></asp:ListItem>
                            <asp:ListItem Enabled="true" Text="No" Value="No"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
            </div>
            <div id="SmokingDateSection" runat="server" class="form-group hidden">
                <asp:Label runat="server" AssociatedControlID="SmokeDt" CssClass="col-md-2 control-label">Effective date of prohibition on smoking:</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="SmokeDt" TextMode="Date" CssClass="form-control" style="width:16rem; margin-top:1rem;"></asp:TextBox>
                </div>
            </div>
            
            <div class="form-group">
                <div class="col-md-2"></div>
                <div class="col-md-10">
                    <h4>Tenants</h4>
                    <table id="tenantsTable" cellspacing="0" cellpadding="4" style="color:#333333;border-collapse:collapse;min-width:85rem;">
		                <tbody>
                            <tr style="color:White;background-color:#507CD1;font-weight:bold;">
			                    <th scope="col">First Name</th>
                                <th scope="col">Last Name</th>
                                <th scope="col">Phone Number</th>
                                <th scope="col">Email Address</th>
                                <th scope="col"></th>
		                    </tr>
                        <% foreach (var tenant in Tenants) { %>
                            <tr data-tenant-id="<%: tenant.TenantID %>" class="active" style="background-color:#EFF3FB;">
			                    <td><%: tenant.FirstName %></td>
                                <td><%: tenant.LastName %></td>
                                <td><%: tenant.PhoneNumber %></td>
                                <td><%: tenant.Email %></td>
                                <td style="text-align:right;"><button type="button" class="btn btn-sm btn-danger btnRemoveTenant">Remove</button></td>
		                    </tr>
                        <% } %>
                            <tr id="noTenants" style="background-color:#EFF3FB;<%: (Tenants.Count == 0 ? "" : "display:none;") %>">
			                    <td>no tenants</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
		                    </tr>
                            <tr id="addTenant" style="display:none; background-color:#EFF3FB;">
                                <td><input type="text" name="firstname" style="text-transform:uppercase" data-lpignore="true" /></td>
                                <td><input type="text" name="lastname" style="text-transform:uppercase" data-lpignore="true" /></td>
                                <td><input type="tel" name="phone" data-lpignore="true" /></td>
                                <td><input type="text" name="email" data-lpignore="true" /></td>
                                <td style="text-align:right;">
                                    <button id="btnAddNewTenant" type="button" class="btn btn-sm btn-primary">Add</button>
                                    <button id="btnCancelAddTenant" type="button" class="btn btn-sm btn-default" style="margin-left:0.5rem;" tabindex="-1">Cancel</button>
                                </td>
                            </tr>
    	                </tbody>
                    </table>
                    <button id="btnNewTenant" type="button" class="btn btn-sm btn-primary" style="margin-top:0.5rem;">Add Tenant</button>
                </div>
            </div>

            <div class="col-md-offset-2 col-md-10" style="margin-bottom:5rem;">
                <div class="form-group">
                    <asp:CheckBox ID="chkDeclare" runat="server" Text="&nbsp;Declaration: I hereby declare under penalty of perjury that the Vacancy Registration Form is true and correct to the best of my knowledge and belief." />
                </div>

                <div class="form-group">
                    <Label runat="server" AssociatedControlID="DeclareInits" CssClass="control-label">Declaration initials:&nbsp;</Label>
                    <asp:TextBox runat="server" ID="DeclareInits" Width="70px" CssClass="form-control" ToolTip="Enter your initials acknowledging the Declaration above." />
                </div>

                <div class="form-group">
                    <asp:Button runat="server" id="btnCancel" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false" CssClass="btn btn-sm btn-default" ToolTip="Returns to list of units." TabIndex="-1" />
                    <asp:Button runat="server" ID="btnSubmit" Text="Confirm" OnClick="UpdateTenancy_Click" OnClientClick="return validate();" CssClass="btn btn-primary" ToolTip="Update the tenants." style="margin-left:1rem;" />
                </div>
            </div>
        </div>
    </section>

    <script>
        var valErrors = [];
        var removedTenantIDs = [];

        function addValError(msg) {
            valErrors.push('<li>' + msg + '</li>');
        }

        function validate() {
            var tenants = [];
            var housingServices = 0;

            valErrors = [];

            try {
                $('#tenantsTable tbody tr.active').each(function () {
                    var fields = [];
                    var tenantID = $(this).data('tenantId');

                    fields.push(tenantID || -1);
                    fields.push($(this).find('td').eq(0).text().toUpperCase());
                    fields.push($(this).find('td').eq(1).text().toUpperCase());
                    fields.push($(this).find('td').eq(2).text());
                    fields.push($(this).find('td').eq(3).text());

                    tenants.push(fields.join('^'));
                });

                $('#<%:hdnDelimitedTenants.ClientID%>').val(tenants.join('|'));
                $('#<%:hdnRemovedTenantIDs.ClientID%>').val(removedTenantIDs.join(','));

                if (tenants.length === 0) {
                    addValError('At least one tenant is required.');
                }

                for (var i = 0; i < 10; i++) {
                    if ($('#MainContent_HServs_' + i).is(':checked')) {
                        housingServices++;
                    }
                }

                if (housingServices === 0) {
                    addValError('Some Housing Services must be selected.');
                }

                if ($('#<%:InitRent.ClientID%>').val() === '') {
                    addValError('Initial Rent is required.');
                }

                if ($('#<%:TenStDt.ClientID%>').val() === '') {
                    addValError('Tenancy Start Date is required');
                }

                if ($('#MainContent_RB1_0').is(':checked') && $('#<%:SmokeDt.ClientID%>').val() === '') {
                    addValError('Effective date of prohibition on smoking must must be entered when Lease Prohibits Smoking.');
                }

                if ($('#<%:PTenDt.ClientID%>').val() === '') {
                    addValError('Prior Tenancy Date is required.');
                }

                if ($('#<%:TermReas.ClientID%>').val() === '') {
                    addValError('Reason for Termination is required.');
                }

                if (valErrors.length) {
                    showErrorModal(('<ul>' + valErrors.join('') + '</ul>'), "Validation Errors");
                    return false;
                } else {
                    return true;
                }
            }
            catch (ex) {
                showErrorModal(ex.message, "Validation Errors");
                return false;
            }
        }

        function _enableSubmitButton() {
            var isChecked = $('#<%:chkDeclare.ClientID%>').is(':checked');
            var hasInitials = $('#<%:DeclareInits.ClientID%>').val().length;
            $('#<%:btnSubmit.ClientID%>').attr('disabled', (isChecked && hasInitials) ? false : true);
        }

        function _setTerminationReason() {
            if ($(this).val() === '4') { // Other
                $('#<%:ExplainOtherTermination.ClientID%>').show();
                setTimeout(function () {
                    $('#<%:TermDescr.ClientID%>').focus();
                }, 10);
            } else {
                $('#<%:TermDescr.ClientID%>').val('');
                $('#<%:ExplainOtherTermination.ClientID%>').hide();
            }
        }

        function _setHousingServicesOther() {
            if ($(this).is(':checked')) {
                $('#<%:OtherHousingServices.ClientID%>').show();
            } else {
                $('#<%:OtherHousingServices.ClientID%>').hide();
            }
        }

        $(document).ready(function () {
            if ($('#MainContent_SmokeDt').val()) {
                $('#MainContent_RB1_0').prop('checked', true);
                $('#MainContent_SmokingDateSection').removeClass('hidden');
            }

            $('#<%:chkDeclare.ClientID%>').change(_enableSubmitButton);
            $('#<%:DeclareInits.ClientID%>').change(_enableSubmitButton);
            $('#<%:TermReas.ClientID%>').change(_setTerminationReason);
            $('#MainContent_HServs_9').change(_setHousingServicesOther);
            $('#MainContent_RB1_0').click(function () { // Smoking Probibition = Yes
                $('#MainContent_SmokingDateSection').removeClass('hidden');
            });
            $('#MainContent_RB1_1').click(function () { // Smoking Probibition = No
                $('#MainContent_SmokingDateSection').addClass('hidden');
                $('#MainContent_SmokeDt').val('');
            });

            $('#btnNewTenant').click(function () {
                $('#noTenants').hide();
                $('#addTenant').show();

                setTimeout(function () {
                    $('#addTenant').find('input[name="firstname"]').focus();
                }, 10);
            });

            $('#btnAddNewTenant').click(function () {
                var valErrors = [];
                var $form = $('#addTenant');
                var firstName = $form.find('input[name="firstname"]').val().toUpperCase();
                var lastName = $form.find('input[name="lastname"]').val().toUpperCase();
                var phone = $form.find('input[name="phone"]').val().toUpperCase();
                var email = $form.find('input[name="email"]').val();

                var _addValError = function (msg) {
                    valErrors.push('<li>' + msg + '</li>');
                };

                if (!firstName) { _addValError('First Name is required.'); }
                if (!lastName) { _addValError('Last Name is required.'); }
                if (!phone) { _addValError('Phone Number is required.'); }
                if (!email) { _addValError('Email Address is required.'); }

                if (valErrors.length) {
                    showErrorModal(('<ul>' + valErrors.join('') + '</ul>'), "Add Tenant - Validation Errors");
                } else {
                    var fields = ['<tr data-tenant="" class="active">'];
                    $('#noTenants').hide();
                    fields.push('<td>' + firstName + '</td>');
                    fields.push('<td>' + lastName + '</td>');
                    fields.push('<td>' + phone + '</td>');
                    fields.push('<td>' + email + '</td>');
                    fields.push('<td style="text-align:right;"><button type="button" class="btn btn-sm btn-danger btnRemoveTenant">Remove</button></td>');
                    fields.push('</tr>');
                    $(fields.join('')).appendTo('#tenantsTable tbody');
                    $('#addTenant').hide().find('input').val('');
                }

                $form = null;
            });

            $('#btnCancelAddTenant').click(function () {
                $('#addTenant').hide().find('input').val('');

                if ($('#tenantsTable tbody tr.active').length === 0) {
                    $('#noTenants').show();
                }
            });

            $('#tenantsTable').on('click', '.btnRemoveTenant', function () {
                var $thisRow = $(this).closest('tr');

                var tenantID = $thisRow.data('tenantId');

                if (tenantID) {
                    removedTenantIDs.push(tenantID);
                }

                $thisRow.remove();

                $thisRow = null;

                setTimeout(function () {
                    var remainingRows = $('#tenantsTable tbody tr.active').length;

                    if (remainingRows === 0) {
                        $('#noTenants').show();
                    }
                }, 10);
            });
        });
    </script>
</asp:Content>
