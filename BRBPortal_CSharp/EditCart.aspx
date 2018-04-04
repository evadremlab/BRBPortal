<%@ Page Title="Edit Payment Cart" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditCart.aspx.cs" Inherits="BRBPortal_CSharp.EditCart" %>
<%@ MasterType  virtualPath="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <hr />

    <asp:HiddenField ID="hdnFeeOption" runat="server" />

    <section id="cartForm">
        <div class="form-horizontal">
            <div class="form-group">
                <asp:GridView ID="gvCart" runat="server" AutoGenerateColumns="False"
                    ForeColor="#333333" GridLines="None" CellPadding="4" ShowFooter="True" 
                    OnRowDataBound="gvCart_RowDataBound"
                    OnRowCommand="gvCart_RowCommand">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField HeaderText="PropertyID" DataField="PropertyID" ReadOnly="True" Visible="false"></asp:BoundField>
                        <asp:BoundField HeaderText="Address" DataField="PropertyAddress" SortExpression="PropertyAddress">
                            <ItemStyle HorizontalAlign="Left" Width="200px" Wrap="False" />
                            </asp:BoundField>
                        <asp:BoundField HeaderText="Current Fee" DataField="CurrentFee" SortExpression="CurrentFee" DataFormatString="{0:c}" ReadOnly="True">
                            <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                            <ItemStyle HorizontalAlign="Right" Width="75px" Wrap="False" />
                            </asp:BoundField>
                        <asp:BoundField HeaderText="Prior Fee" DataField="PriorFee" SortExpression="PriorFee" DataFormatString="{0:c}" ReadOnly="True">
                            <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                            <ItemStyle HorizontalAlign="Right" Width="75px" Wrap="False" />
                            </asp:BoundField>
                        <asp:BoundField HeaderText="Current Penalty" DataField="CurrentPenalty" SortExpression="CurrentPenalty" DataFormatString="{0:c}" ReadOnly="True">
                            <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                            <ItemStyle HorizontalAlign="Right" Width="75px" Wrap="False" />
                            </asp:BoundField>
                        <asp:BoundField HeaderText="Prior Penalty" DataField="PriorPenalty" SortExpression="PriorPenalty" DataFormatString="{0:c}" ReadOnly="True">
                            <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                            <ItemStyle HorizontalAlign="Right" Width="75px" Wrap="False" />
                            </asp:BoundField>
                        <asp:BoundField HeaderText="Credits" DataField="Credits" SortExpression="Credit" DataFormatString="{0:c}" ReadOnly="True">
                            <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                            <ItemStyle HorizontalAlign="Right" Width="75px" Wrap="False" />
                            </asp:BoundField>
                        <asp:BoundField HeaderText="Balance" DataField="Balance" SortExpression="Balance" DataFormatString="{0:c}" ReadOnly="True">
                            <FooterStyle HorizontalAlign="Right" Width="80px" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                            <ItemStyle HorizontalAlign="Right" Width="80px" Wrap="False" />
                            </asp:BoundField>
                            <asp:ButtonField ButtonType="Button" CommandName="RemoveFromCart" Text="Remove">
                                <ControlStyle CssClass="btn btn-sm btn-danger" />
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
                <div class="radio radiobuttonlist" style="padding-top:0; padding-left:0;">
                    <asp:Radiobuttonlist ID="FeeOption" runat="server" RepeatDirection="Horizontal" CellPadding="5" 
                        ToolTip="Check Fees Only for all Current and Prior fees. Check All to include all Fees and Penalties." 
                        OnSelectedIndexChanged="FeeOption_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Enabled="true" Text="Fees Only" Value="Fees Only"></asp:ListItem>
                        <asp:ListItem Enabled="true" Text="All Fees and Penalties" Value="All Fees and Penalties"></asp:ListItem>
                    </asp:Radiobuttonlist>
                </div>
            </div>

            <div class="form-group">
                <asp:Button runat="server" id="btnBack" PostBackUrl="~/Cart.aspx" CausesValidation="false" Text="Cancel" CssClass="btn btn-sm btn-default" ToolTip="Cancel any changes to this cart." TabIndex="-1" />
                <asp:Button runat="server" id="btnUpdateCart" OnClick="UpdateCart_Click" Text="Save Changes" CssClass="btn btn-sm btn-primary" ToolTip="Save any changes to this cart." style="margin-left:1rem;" />
            </div>
        </div>
    </section>
</asp:Content>
