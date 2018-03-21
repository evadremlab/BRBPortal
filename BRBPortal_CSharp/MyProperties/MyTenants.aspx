<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyTenants.aspx.cs" Inherits="BRBPortal_CSharp.MyProperties.MyTenants" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>View Tenancy</h2>
    <asp:HiddenField ID="hfDialogID" runat="server" />
    <div class="form-horizontal">
        <section id="propertiesForm">
            <div class="form-horizontal">
                <hr />
                <div class="col-md-12">
                    <div class="form-group">
                        <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                            <p class="text-danger">
                                <asp:Literal runat="server" ID="FailureText" />
                            </p>
                        </asp:PlaceHolder>
                    </div>
                    <div class="form-group">
                        <h4>
                            <asp:Literal ID="MainAddress" runat="server" ></asp:Literal>
                        </h4>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="OwnerName" CssClass="control-label">Owner Name:</asp:Label>
                        <asp:Literal ID="OwnerName" runat="server" ></asp:Literal>
                    </div>
                </div>

                <div class="col-md-6">
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="AgentName" CssClass="control-label">Agent Name:</asp:Label>
                        <asp:Literal ID="AgentName" runat="server" ></asp:Literal>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="BalAmt" CssClass="control-label">Total Balance:</asp:Label>
                        <asp:Literal ID="BalAmt" runat="server" ></asp:Literal>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="UnitNo" CssClass="control-label">Unit #: </asp:Label>
                        <asp:Literal ID="UnitNo" runat="server" > </asp:Literal>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="UnitStat" CssClass="control-label">Unit Status: </asp:Label>
                        <asp:Literal ID="UnitStat" runat="server" > </asp:Literal>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="SmokYN" CssClass="control-label">Smoking prohibition in lease status:</asp:Label>
                        <asp:Literal ID="SmokYN" runat="server" ></asp:Literal>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="SmokDt" CssClass="control-label">Smoking prohibition effective date:</asp:Label>
                        <asp:Literal ID="SmokDt" runat="server" ></asp:Literal>
                    </div>
                </div>

                <div class="col-md-6">
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="InitRent" CssClass="control-label">Initial Rent: </asp:Label>
                        <asp:Literal ID="InitRent" runat="server" > </asp:Literal>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="TenStDt" CssClass="control-label">Tenancy Start Date: </asp:Label>
                        <asp:Literal ID="TenStDt" runat="server" > </asp:Literal>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="HouseServs" CssClass="control-label">Housing Services: </asp:Label>
                        <asp:Literal ID="HouseServs" runat="server" > </asp:Literal>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="NumTenants" CssClass="control-label"># of Tenants: </asp:Label>
                        <asp:Literal ID="NumTenants" runat="server" > </asp:Literal>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="PriorEndDt" CssClass="control-label">Prior Tenancy end date:</asp:Label>
                        <asp:Literal ID="PriorEndDt" runat="server" ></asp:Literal>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="TermReason" CssClass="control-label">Reason for termination:</asp:Label>
                        <asp:Literal ID="TermReason" runat="server"></asp:Literal>
                    </div>
                </div>

                <div class="col-md-12">
                    <div class="form-group">
                        <asp:GridView ID="gvTenants" runat="server" AutoGenerateColumns="False" CellPadding="4"
                            ForeColor="#333333" GridLines="None" AllowPaging="True" PageSize="10" OnPageIndexChanging="gvTenants_PageIndexChanging">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="TenantID" HeaderText="Tenant ID" Visible="False" />
                                <asp:BoundField DataField="FirstName" HeaderText="First Name" Visible="False" />
                                <asp:BoundField DataField="LastName" HeaderText="Last Name" Visible="False" />
                                <asp:BoundField HeaderText="Tenant Name" DataField="DispName" SortExpression="DispName">
                                    <ItemStyle HorizontalAlign="Left" Width="250px" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Phone Number" DataField="PhoneNo" SortExpression="PhoneNo">
                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Email Address" DataField="EmailAddr" SortExpression="EmailAddr">
                                    <ItemStyle HorizontalAlign="Left" Width="220px" />
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
                        <asp:Button runat="server" id="btnBack" OnClick="ToUnits_Click" Text="Back" CssClass="btn btn-default" ToolTip="Return to the list of Units." />
                        <asp:Button runat="server" OnClick="btnUpdTen_Click" Text="Update Tenancy" CssClass="btn btn-primary" style="margin-left:1rem;" ToolTip="Proceed to Update Tenants." />
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>
