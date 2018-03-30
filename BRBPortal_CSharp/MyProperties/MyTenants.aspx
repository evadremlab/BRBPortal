<%@ Page Title="List of Tenants" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyTenants.aspx.cs" Inherits="BRBPortal_CSharp.MyProperties.MyTenants" %>
<%@ MasterType  virtualPath="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <h4>at <asp:Literal ID="MainAddress" runat="server"></asp:Literal>, Unit # <asp:Literal ID="UnitNo" runat="server" ></asp:Literal></h4>
    <hr />

    <asp:HiddenField ID="hfDialogID" runat="server" />

    <div class="form-horizontal">
        <section id="updatePropertiesForm">
            <div class="form-horizontal">
                <hr />
                <div class="row hidden">
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="OwnerName" CssClass="col-md-2 control-label">Owner Name:</asp:Label>
                        <div class="col-md-10 literal">
                            <asp:Literal ID="OwnerName" runat="server" ></asp:Literal>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="row hidden">
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="AgentName" CssClass="col-md-4 control-label">Agent Name:</asp:Label>
                                <div class="col-md-8 literal">
                                    <asp:Literal ID="AgentName" runat="server" ></asp:Literal>
                                </div>
                            </div>
                        </div>
                        <div class="row hidden">
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="BalAmt" CssClass="col-md-4 control-label">Total Balance:</asp:Label>
                                <div class="col-md-8 literal">
                                    <asp:Literal ID="BalAmt" runat="server" ></asp:Literal>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="UnitStat" CssClass="col-md-4 control-label">Unit Status: </asp:Label>
                                <div class="col-md-8 literal">
                                    <asp:Literal ID="UnitStat" runat="server"></asp:Literal>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="NumTenants" CssClass="col-md-4 control-label" style="padding-left:2rem;"># of Tenants: </asp:Label>
                                <div class="col-md-8 literal">
                                    <asp:Literal ID="NumTenants" runat="server"></asp:Literal>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TenStDt" CssClass="col-md-4 control-label">Tenancy Start Date: </asp:Label>
                                <div class="col-md-8 literal">
                                    <asp:Literal ID="TenStDt" runat="server"></asp:Literal>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="SmokYN" CssClass="col-md-4 control-label">Smoking prohibition in lease:</asp:Label>
                                <div class="col-md-8">
                                    <asp:Literal ID="SmokYN" runat="server"></asp:Literal>
                                    <asp:Label runat="server" AssociatedControlID="SmokDt" CssClass="control-label" style="padding-left:2rem;">Effective date:</asp:Label>
                                    <% if (!(string.IsNullOrEmpty(SmokYN.Text) || SmokYN.Text.Equals("N/A"))) { %>
                                    <asp:Literal ID="SmokDt" runat="server"></asp:Literal>
                                    <% } %>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="row">
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="InitRent" CssClass="col-md-4 control-label">Initial Rent: </asp:Label>
                                <div class="col-md-8 literal">
                                    <asp:Literal ID="InitRent" runat="server"></asp:Literal>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="HouseServs" CssClass="col-md-4 control-label">Housing Services: </asp:Label>
                                <div class="col-md-8 literal">
                                    <asp:Literal ID="HouseServs" runat="server"></asp:Literal>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="PriorEndDt" CssClass="col-md-4 control-label">Prior Tenancy end date:</asp:Label>
                                <div class="col-md-8 literal">
                                    <asp:Literal ID="PriorEndDt" runat="server"></asp:Literal>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TermReason" CssClass="col-md-4 control-label">Reason for termination:</asp:Label>
                                <div class="col-md-8 literal">
                                    <asp:Literal ID="TermReason" runat="server"></asp:Literal>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <asp:GridView ID="gvTenants" runat="server" AutoGenerateColumns="False" CellPadding="4" Width="100%"
                                ForeColor="#333333" GridLines="None" AllowPaging="True" PageSize="10" OnPageIndexChanging="gvTenants_PageIndexChanging">
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:BoundField HeaderText="Tenant ID" DataField="TenantID" Visible="False" />
                                    <asp:BoundField HeaderText="First Name" DataField="FirstName" Visible="False" />
                                    <asp:BoundField HeaderText="Last Name" DataField="LastName" Visible="False" />
                                    <asp:BoundField HeaderText="Tenant Name" DataField="DisplayName" SortExpression="DisplayName">
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Phone Number" DataField="PhoneNumber" SortExpression="PhoneNumber">
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Email Address" DataField="Email" SortExpression="Email">
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                </Columns>
                                <EditRowStyle BackColor="#2461BF" />
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#EFF3FB" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                            </asp:GridView>
                        </div>    
                        <div class="form-group">
                            <asp:Button runat="server" id="btnBack" UseSubmitBehavior="false" PostBackUrl="~/MyProperties/MyUnits" Text="Back" CssClass="btn btn-sm btn-default" ToolTip="Return to the list of Units." TabIndex="-1" />
                            <asp:Button runat="server" Text="Update Tenancy" CssClass="btn btn-primary" style="margin-left:1rem;" ToolTip="Proceed to Update Tenants." />
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
    
    <script>
        $(document).ready(function () {
            $('#aspForm').submit(function (evt) {
                var form = this;
                evt.preventDefault();
                $('#YesNoModal .modal-title').text('Update Tenancy');
                $('#YesNoModal .modal-body').text('Have all original tenants moved out?');
                $('#YesNoModal').one('hidden.bs.modal', function () {
                    if ($(document.activeElement).is('.btn-primary')) { // clicked on Yes button
                        form.submit();
                    } else {
                        setTimeout(function() {
                            $('#OkModal .modal-title').text('Update Tenancy');
                            $('#OkModal .modal-body').text('New tenancy cannot be updated until all original tenants have moved out.');
                            $('#OkModal').modal('show');
                        }, 10);
                    }
                });
                $('#YesNoModal').modal('show');
            });
        });
    </script>
</asp:Content>
