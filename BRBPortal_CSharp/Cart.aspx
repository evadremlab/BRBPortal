<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="BRBPortal_CSharp.Cart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Payment Cart</h2>

    <section id="cartForm">
        <div class="form-horizontal">
            <hr />
            <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                <p class="text-danger">
                    <asp:Literal runat="server" ID="FailureText" />
                </p>
            </asp:PlaceHolder>

            <div class="form-group">
                <asp:GridView ID="gvCart" runat="server" AutoGenerateColumns="False" CellPadding="4" 
                    ForeColor="#333333" GridLines="None" AllowPaging="True" ShowFooter="True" 
                    OnRowDataBound="gvCart_RowDataBound" >
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField HeaderText="PropertyID" DataField="PropertyID" ReadOnly="True"></asp:BoundField>
                        <asp:BoundField HeaderText="Address" DataField="MainAddr" SortExpression="MainAddr">
                            <ItemStyle HorizontalAlign="Left" Width="200px" Wrap="False" />
                            </asp:BoundField>
                        <asp:BoundField HeaderText="Current Fee" DataField="CurrFees" SortExpression="CurrFees" DataFormatString="{0:c}" ReadOnly="True">
                            <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                            <ItemStyle HorizontalAlign="Right" Width="75px" Wrap="False" />
                            </asp:BoundField>
                        <asp:BoundField HeaderText="Prior Fee" DataField="PriorFees" SortExpression="PriorFees" DataFormatString="{0:c}" ReadOnly="True">
                            <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                            <ItemStyle HorizontalAlign="Right" Width="75px" Wrap="False" />
                            </asp:BoundField>
                        <asp:BoundField HeaderText="Current Penalty" DataField="CurrPenalty" SortExpression="CurrPenalty" DataFormatString="{0:c}" ReadOnly="True">
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
                <asp:Button runat="server" id="btnCancelCart" OnClick="CancelCart_Click" Text="Cancel" CssClass="btn btn-sm btn-default" ToolTip="Cancel any changes to this cart." TabIndex="-1" />
                <asp:Button runat="server" id="btnPayCart" OnClick="PayCart_Click" Text="Pay Now" CssClass="btn btn-primary" ToolTip="Pay for your cart balance." style="margin-left:1rem;" />
                <asp:Button runat="server" ID="btnEdCart" OnClick="EditCart_Click" Text="Edit Cart" CssClass="btn btn-success" ToolTip="Edit your cart." style="margin-left:1rem;" />
            </div>
        </div>
    </section>
</asp:Content>
