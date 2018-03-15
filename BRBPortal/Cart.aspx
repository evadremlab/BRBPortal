<%@ Page Title="Cart" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.vb" Inherits="BRBPortal.Cart" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <link rel="stylesheet" type="text/css" href="../Content/bootstrap.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/StyleBRB.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/Baseline.Type.css" />

    <div class="form-horizontal" style="width: 809px; height: 500px; padding-left:30px" >
        <div class="form-group" style="height:40px; align-items:center">
            <h4>Payment Cart</h4>
            <nav class="nav" style="float:right">
                <a href="../Home">Home</a>
                &nbsp;&nbsp;
                <a href="../Account/ProfileList">My Profile</a>
                &nbsp;&nbsp;
                <a href="../Cart">Cart</a>
                &nbsp;&nbsp;
                <a href="../Contact">Contact Us</a>
                &nbsp;&nbsp;
                <a href="../Logout">Logout</a>
            </nav>
            <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                <p class="text-danger">
                    <asp:Literal runat="server" ID="FailureText" />
                </p>
            </asp:PlaceHolder>
        </div>

        <br />
        <asp:GridView ID="gvCart" runat="server" AutoGenerateColumns="False" CellPadding="4" 
            ForeColor="#333333" GridLines="None" AllowPaging="True" onpageindexchanging="gvCart_PageIndexChanging" ShowFooter="True" 
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

        <%--<asp:Radiobuttonlist ID="FeesAll" runat="server" RepeatDirection="Horizontal" CellPadding="5" 
            ToolTip="Check Fees Only for all Current and Prior fees. Check All to inluce all Fees and Penalties." 
            OnSelectedIndexChanged="FeesAll_SelectedIndexChanged" AutoPostBack="True" >
            <asp:ListItem Enabled="true" Text="&nbsp;&nbsp;Fees Only" Value="FeesOnly"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="&nbsp;&nbsp;All" Value="AllFees"></asp:ListItem>
        </asp:Radiobuttonlist>--%>

        <div style="padding-left:10px">
            <asp:Literal ID="ShowFeesAll" runat="server" ></asp:Literal>
        </div>

        <br />
        <div class="btn-group" style="padding-left:10px">
            <asp:Button runat="server" ID="btnEdCart" OnClick="EditCart_Click" Text="Edit Cart" CssClass="btn-default active" 
                ToolTip="Edit your cart."/>
            &nbsp;&nbsp;&nbsp;&nbsp; 
            <asp:Button runat="server" id="btnPayCart" OnClick="PayCart_Click" Text="Pay Now" CssClass="btn-default active" 
                ToolTip="Pay for your cart balance." />
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button runat="server" id="btnCancelCart" OnClick="CancelCart_Click" Text="Cancel" CssClass="btn-default active" 
                ToolTip="Cancel any changes to this cart." />
        </div>
    </div>
</asp:Content>
