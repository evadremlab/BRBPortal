<%@ Page Title="Confirm Payment" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConfirmPayment.aspx.cs" Inherits="BRBPortal_CSharp.ConfirmPayment" %>
<%@ MasterType  virtualPath="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <h4>needs something about payment processing...</h4>
    <hr />
    
    <input type="hidden" name="lockAmount" value="true" />
    <input type="hidden" name="productId" value="24241003506464897434114212714156124" />
    <input type="hidden" name="returnUrl" value="http://rentportaldev.cityofberkeley.info/PaymentProcessed" />
    <input type="hidden" name="errorUrl" value="http://rentportaldev.cityofberkeley.info/PaymentError" />
    <input type="hidden" name="cancelUrl" value="http://rentportaldev.cityofberkeley.info/PaymentCancelled" />
    <input type="hidden" name="postbackUrl" value="http://clipper.transsight.com/api/Values" />
    <input type="hidden" name="cde-Cart-17" value="<%: CartID %>" />
    <input type="hidden" name="cde-BillingCode-1" value="<%: BillingCode %>" />
    <input type="hidden" name="paymentAmount" value="<%: PaymentAmount %>" />

    <section id="cartForm">
        <div class="form-horizontal">
            <div class="form-group">
                <asp:GridView ID="gvCart" runat="server" AutoGenerateColumns="False"
                    ForeColor="#333333" GridLines="None" CellPadding="4" ShowFooter="True" 
                    OnRowDataBound="gvCart_RowDataBound">
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
                        <asp:BoundField HeaderText="Credits" DataField="Credits" SortExpression="Credits" DataFormatString="{0:c}" ReadOnly="True">
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
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" CssClass="grid-pager" />
                    <RowStyle BackColor="#EFF3FB" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                </asp:GridView>
            </div>
            
            <div class="form-group">
                <asp:Literal ID="FeeOption" runat="server" ></asp:Literal>
            </div>

            <div class="form-group hidden">
                Payment Amount: <asp:Literal ID="litPaymentAmount" runat="server" ></asp:Literal>
            </div>

            <div class="form-group">
                <asp:Button runat="server" id="btnCancelCart" OnClick="CancelCart_Click" Text="Back" CssClass="btn btn-sm btn-default" ToolTip="Back to your list of properties." TabIndex="-1" />
                <asp:Button runat="server" ID="btnSubmit" Text="Submit" OnClick="PayCart_Click" CssClass="btn btn-primary" style="margin-left:1rem;" />
            </div>
        </div>
    </section>
</asp:Content>
