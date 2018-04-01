<%@ Page Title="List of Units" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyUnits.aspx.cs" Inherits="BRBPortal_CSharp.MyProperties.MyUnits" %>
<%@ MasterType  virtualPath="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <h4>at <asp:Literal ID="PropertyAddress" runat="server" ></asp:Literal></h4>
    <hr />

    <div class="form-horizontal">
        <section id="propertiesForm">
            <div class="form-horizontal">
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="CurrFee" CssClass="control-label">Current Fee:</asp:Label>
                    <asp:Literal ID="CurrFee" runat="server" ></asp:Literal>
                    <asp:Label runat="server" AssociatedControlID="Balance" CssClass="control-label" style="margin-left:2rem;">Balance:</asp:Label>
                    <asp:Literal ID="Balance" runat="server" ></asp:Literal>
                    <asp:Button runat="server" id="btnAddCart" OnClick="AddCart_Click"  Text="Add to Cart" CssClass="btn btn-primary" ToolTip="Add to cart." TabIndex="-1" style="margin-left:1rem;" />
                </div>
                <div class="form-group">
                    <asp:Literal ID="UnitStatusDescription" runat="server"></asp:Literal>
                </div>
                <asp:PlaceHolder ID="AgentSection" runat="server">
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="MgrName" CssClass="control-label">Manager/Agent Name:</asp:Label>
                        <asp:Literal ID="MgrName" runat="server"></asp:Literal>
                    </div>
                </asp:PlaceHolder>
                <div class="form-group">
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="BillingAddress" CssClass="control-label">Billing Address:</asp:Label>
                    <asp:Literal ID="BillingAddress" runat="server" ></asp:Literal>
                </div>
            </div>
            <div class="form-group">
                <asp:GridView ID="gvUnits" runat="server" AutoGenerateColumns="False"
                    ForeColor="#333333" GridLines="None" CellPadding="4"
                    AllowPaging="true" PageSize="10" OnPageIndexChanging="gvUnits_PageIndexChanging"
                    OnRowDataBound="gvUnits_OnRowDataBound"
                    OnRowCommand="gvUnits_RowCommand">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="UnitID" HeaderText="UnitID (NV)" ReadOnly="True" Visible="false" />
                        <asp:BoundField HeaderText="UnitStatID (NV)" DataField="UnitStatID" ReadOnly="true" SortExpression="UnitStatID" Visible="false" />
                        <asp:BoundField DataField="CPUnitStatCode" HeaderText="Unit Status (NV)" ReadOnly="True" SortExpression="UnitStatCode" Visible="false" />
                        <asp:BoundField DataField="StreetAddress" HeaderText="Address" ReadOnly="True" SortExpression="StreetAddress">
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Unit No" DataField="UnitNo" ReadOnly="True" />
                        <asp:BoundField DataField="ClientPortalUnitStatusCode" HeaderText="Unit Status" ReadOnly="True" SortExpression="ClientPortalUnitStatusCode">
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Rent Ceiling" DataField="RentCeiling" SortExpression="RentCeiling" DataFormatString="{0:c}" ReadOnly="True">
                            <HeaderStyle CssClass="text-right" Wrap="False" />
                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </asp:BoundField>
                        <asp:BoundField HeaderText="Tenancy Start Date" DataField="StartDt" SortExpression="StartDt" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="True">
                            <HeaderStyle CssClass="text-right" Wrap="False" />
                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </asp:BoundField>
                        <asp:BoundField HeaderText="Housing Services" DataField="HServices" SortExpression="HServices" ReadOnly="True">
                            <HeaderStyle Wrap="False" />
                            <ItemStyle Wrap="False" />
                            </asp:BoundField>
                        <asp:ButtonField ButtonType="Button" CommandName="UnitStatus" Text="Update Unit Status">
                            <ControlStyle CssClass="btn btn-sm btn-success" />
                        </asp:ButtonField>
                        <asp:ButtonField ButtonType="Button" CommandName="Tenancy" Text="Update Tenancy">
                            <ControlStyle CssClass="btn btn-sm btn-success" />
                        </asp:ButtonField>
                    </Columns>
                    <EditRowStyle BackColor="#2461BF" />
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Left" CssClass="grid-pager" />
                    <RowStyle BackColor="#EFF3FB" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                </asp:GridView>
            </div>
            <div class="form-group">
                <asp:Button runat="server" id="btnBack" UseSubmitBehavior="false" PostBackUrl="~/MyProperties/MyProperties" CausesValidation="false" Text="Back" CssClass="btn btn-sm btn-default" ToolTip="Return to the list of Properties." TabIndex="-1" />
                <%--<asp:Button runat="server" OnClick="NextBtn_Click" Text="Next" CssClass="btn btn-primary" style="margin-left:1rem;" ToolTip="Proceed to Update Unit or Tenancy."/>--%>
            </div>
        </section>
    </div>
</asp:Content>
