<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditCart.aspx.cs" Inherits="BRBPortal_CSharp.EditCart" %>
<%@ MasterType  virtualPath="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Edit Payment Cart</h2>

    <section id="cartForm">
        <div class="form-horizontal">
            <hr />
            <div class="form-group">
                <asp:GridView ID="gvCart" runat="server" AutoGenerateColumns="False" CellPadding="4" 
                    ForeColor="#333333" GridLines="None" AllowPaging="True" ShowFooter="True" 
                    onRowCommand="gvCart_RowCommand" OnRowDataBound="gvCart_RowDataBound">
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
                <asp:Literal ID="ShowFeesAll" runat="server" ></asp:Literal>
            </div>

            <div class="form-group">
                <asp:Button runat="server" id="btnBack" PostBackUrl="~/Cart.aspx" CausesValidation="false" Text="Back" CssClass="btn btn-sm btn-default" ToolTip="Cancel any changes to this cart." TabIndex="-1" />
            </div>
        </div>
    </section>
</asp:Content>
