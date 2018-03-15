<%@ Page Title="EditCart" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditCart.aspx.vb" Inherits="BRBPortal.EditCart" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <link rel="stylesheet" type="text/css" href="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/themes/start/jquery-ui.css" />
    <link rel="stylesheet" type="text/css" href="../Content/bootstrap.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/StyleBRB.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/Baseline.Type.css" />
    
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/ClientPortal.js") %>"> </script>
    <!-- Modal Dialog Box Ajax Code -->
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script src="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/jquery-ui.js" type="text/javascript"></script>

    <div class="form-horizontal" style="width: 880px; height: 600px; padding-left:30px" >
        <div class="form-group" style="height:40px; align-items:center">
            <h4>Edit Payment Cart</h4>
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
        </div>

        <br />
        <asp:GridView ID="gvCart" runat="server" CssClass="Margin30" AutoGenerateColumns="False" CellPadding="4" 
            ForeColor="#333333" GridLines="None" AllowPaging="True" onpageindexchanging="gvCart_PageIndexChanging" 
            OnRowDataBound="gvCart_RowDataBound" ShowFooter="True" OnRowCommand="gvCart_RowCommand" >
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HiddenField runat="server" ID="hfPropID" Value='<%#Eval("PropertyID")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
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
                    <FooterStyle HorizontalAlign="Right" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                    <ItemStyle HorizontalAlign="Right" Width="80px" Wrap="False" />
                    </asp:BoundField>
                <asp:ButtonField ButtonType="Button" CommandName="Select" Text="Remove" />
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

        <asp:Radiobuttonlist ID="FeesAll" runat="server" RepeatDirection="Horizontal" CellPadding="5" 
            ToolTip="Check Fees Only for all Current and Prior fees. Check All to inluce all Fees and Penalties." 
            OnSelectedIndexChanged="FeesAll_SelectedIndexChanged" AutoPostBack="True" >
            <asp:ListItem Enabled="true" Text="&nbsp;&nbsp;Fees Only" Value="FeesOnly"></asp:ListItem>
            <asp:ListItem Enabled="true" Text="&nbsp;&nbsp;All" Value="AllFees"></asp:ListItem>
        </asp:Radiobuttonlist>
        
        <br />
        <div class="btn-group" style="padding-left:10px">
            <asp:Button runat="server" ID="btnSaveCart" OnClick="SaveCart_Click" Text="Save" CssClass="btn-default active" 
                ToolTip="Save changes to cart."/>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button runat="server" id="btnCancEditCart" OnClick="CancEditCart_Click" Text="Cancel" CssClass="btn-default active" 
                ToolTip="Cancel the cart edit." />
        </div>

        <asp:Button ID="btnDialogResponseYes" runat="server" Text="Hidden Button" OnClick="DialogResponseYes"
            Style="display: none" UseSubmitBehavior="false" />
        <asp:Button ID="btnDialogResponseNo" runat="server" Text="Hidden Button" OnClick="DialogResponseNo"
            Style="display: none" UseSubmitBehavior="false" />
        <div id="dialog2" style="display: none">
        </div>
        <asp:HiddenField ID="hfDialogID" runat="server" />
        <asp:HiddenField ID="OrigCart" runat="server" ValidateRequestMode="Disabled" />
    </div>
</asp:Content>
