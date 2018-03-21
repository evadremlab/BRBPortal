<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyUnits.aspx.cs" Inherits="BRBPortal_CSharp.MyProperties.MyUnits" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hfDialogID" runat="server" />

    <h2>My Properties</h2>

    <div class="form-horizontal">
        <section id="propertiesForm">
            <div class="form-horizontal">
                <hr />
                <div class="form-group">
                    <h4>
                        <asp:Literal ID="MainAddress" runat="server" ></asp:Literal>
                    </h4>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" CssClass="col-md-2 control-label">Manager/Agent Name:</asp:Label>
                    <div class="col-md-10">
                        <asp:Literal ID="MgrName" runat="server"></asp:Literal>
                        <asp:Button runat="server" id="btnRemAgnt" OnClick="RemAgent_Click" Text="Remove Agent" CssClass="btn btn-default" ToolTip="Remove this agent." />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" CssClass="col-md-2 control-label">Billing Address:</asp:Label>
                    <div class="col-md-10" style="padding-top:0.7rem;">
                        <asp:Literal ID="BillAddr" runat="server" ></asp:Literal>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <label>Select the update option for a unit:</label>
            </div>
            <div class="form-group">
                <asp:GridView ID="gvUnits" runat="server" AutoGenerateColumns="False" CellPadding="4" 
                    ForeColor="#333333" GridLines="None" OnRowDataBound="OnRowDataBound" AllowPaging="True" OnPageIndexChanging="gvUnits_PageIndexChanging" >
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField HeaderText="Update Unit Status">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbUnit" runat="server" OnCheckedChanged="cbUnit_CheckedChanged" AutoPostBack="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Update Tenancy">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbTenant" runat="server" OnCheckedChanged="cbTenant_CheckedChanged" AutoPostBack="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="UnitID" HeaderText="UnitID (NV)" ReadOnly="True" Visible="true" />
                        <asp:BoundField HeaderText="Unit No" DataField="UnitNo" ReadOnly="True">
                        <ItemStyle Width="90px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="UnitStatID (NV)" DataField="UnitStatID" SortExpression="UnitStatID" Visible="False">
                            </asp:BoundField>
                        <asp:BoundField DataField="CPUnitStatCode" HeaderText="Unit Status (NV)" ReadOnly="True" SortExpression="UnitStatCode" Visible="False">
                        <ItemStyle HorizontalAlign="Left" Width="100px" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CPUnitStatDisp" HeaderText="Unit Status" ReadOnly="True" SortExpression="UnitStatCode">
                        <ItemStyle HorizontalAlign="Left" Width="250px" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Rent Ceiling" DataField="RentCeiling" SortExpression="RentCeiling" DataFormatString="{0:c}" ReadOnly="True">
                            <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                            <ItemStyle HorizontalAlign="Right" Width="100px" Wrap="False" />
                            </asp:BoundField>
                        <asp:BoundField HeaderText="Tenancy Start Date" DataField="StartDt" SortExpression="StartDt" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="True">
                            <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                            <ItemStyle HorizontalAlign="Center" Width="80px" Wrap="False" />
                            </asp:BoundField>
                        <asp:BoundField HeaderText="Housing Services" DataField="HServices" SortExpression="HServices" ReadOnly="True">
                            <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                            <ItemStyle HorizontalAlign="Left" Width="250px" Wrap="False" />
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
                <asp:Button runat="server" id="btnBack" OnClick="ToProperty_Click" Text="Back" CssClass="btn tn-default" ToolTip="Return to the list of Properties." />
                <asp:Button runat="server" OnClick="NextBtn_Click" Text="Next" CssClass="btn btn-primary" style="margin-left:1rem;" ToolTip="Proceed to Update Unit or Tenancy."/>
            </div>
        </section>
    </div>
</asp:Content>
