<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyUnits.aspx.cs" Inherits="BRBPortal_CSharp.MyProperties.MyUnits" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hfDialogID" runat="server" />

    <h2>List of Units</h2>

    <div class="form-horizontal">
        <section id="propertiesForm">
            <div class="form-horizontal">
                <h4>at <asp:Literal ID="MainAddress" runat="server" ></asp:Literal></h4>
                <hr />
                <asp:PlaceHolder ID="AgentSection" runat="server">
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="MgrName" CssClass="control-label">Manager/Agent Name:</asp:Label>
                        <asp:Literal ID="MgrName" runat="server"></asp:Literal>
                        <%--<asp:Button runat="server" id="btnRemAgnt" OnClick="RemAgent_Click" Text="Remove Agent" CssClass="btn btn-default" ToolTip="Remove this agent." />--%>
                    </div>
                </asp:PlaceHolder>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="BillAddr" CssClass="control-label">Billing Address:</asp:Label>
                    <asp:Literal ID="BillAddr" runat="server" ></asp:Literal>
                </div>
            </div>
            <div class="form-group">
                <asp:GridView ID="gvUnits" runat="server" AutoGenerateColumns="False" CellPadding="4" 
                    ForeColor="#333333" GridLines="None" OnRowDataBound="OnRowDataBound" AllowPaging="True" 
                    OnRowCommand="gvUnits_RowCommand"
                    OnPageIndexChanging="gvUnits_PageIndexChanging" >
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="UnitID" HeaderText="UnitID (NV)" ReadOnly="True" Visible="true" />
                        <asp:BoundField HeaderText="Unit No" DataField="UnitNo" ReadOnly="True">
                        </asp:BoundField>
                        <asp:BoundField HeaderText="UnitStatID (NV)" DataField="UnitStatID" SortExpression="UnitStatID" Visible="False"/>
                        <asp:BoundField DataField="CPUnitStatCode" HeaderText="Unit Status (NV)" ReadOnly="True" SortExpression="UnitStatCode" Visible="False">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CPUnitStatDisp" HeaderText="Unit Status" ReadOnly="True" SortExpression="UnitStatCode">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
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
                            <HeaderStyle CssClass="text-right" Wrap="False" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            </asp:BoundField>
                        <asp:ButtonField ButtonType="Button" CommandName="UnitStatus" Text="Update Unit Status">
                            <ControlStyle CssClass="btn btn-sm btn-primary" />
                        </asp:ButtonField>
                        <asp:ButtonField ButtonType="Button" CommandName="Tenancy" Text="Update Tenancy">
                            <ControlStyle CssClass="btn btn-sm btn-primary" />
                        </asp:ButtonField>
                        <asp:ButtonField ButtonType="Button" CommandName="Both" Text="Update Both">
                            <ControlStyle CssClass="btn btn-sm btn-primary" />
                        </asp:ButtonField>
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
                <asp:Button runat="server" id="btnBack" UseSubmitBehavior="false" PostBackUrl="~/MyProperties/MyProperties" Text="Back" CssClass="btn btn-sm btn-default" ToolTip="Return to the list of Properties." TabIndex="-1" />
                <%--<asp:Button runat="server" OnClick="NextBtn_Click" Text="Next" CssClass="btn btn-primary" style="margin-left:1rem;" ToolTip="Proceed to Update Unit or Tenancy."/>--%>
            </div>
        </section>
    </div>
</asp:Content>
