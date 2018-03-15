<%@ Page Title="List of Units" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="MyUnits.aspx.vb" Inherits="BRBPortal.MyUnits" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%--<h2><%: Title %>.</h2>--%>

    <link rel="stylesheet" type="text/css" href="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/themes/start/jquery-ui.css" />
    <link rel="stylesheet" type="text/css" href="../Content/bootstrap.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/StyleBRB.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/Baseline.Type.css" />
    
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/ClientPortal.js") %>"> </script>
    <!-- Modal Dialog Box Ajax Code -->
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script src="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/jquery-ui.js" type="text/javascript"></script>

    <div class="form-horizontal" style="width: 820px; height: 670px; padding-left:30px" >
        <div class="form-group" style="height:40px; align-items:center">
            <h4>List of Units</h4>
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

        <div class="form-group" style="padding-left:10px">
            <h4 style="width:450px">
                <asp:Literal ID="MainAddress" runat="server" ></asp:Literal>
            </h4>
        </div>

        <br />
        <div class="form-group">
            <asp:Label runat="server" CssClass="col-md-1 control-label">Manager/Agent Name:</asp:Label>
            <asp:Literal ID="MgrName" runat="server" > </asp:Literal>
            <%--<asp:Label runat="server" CssClass="control-label" Width="400px">Joe Smith</asp:Label>--%>
            <a style="padding-left:150px">
                <asp:Button runat="server" id="btnRemAgnt" OnClick="RemAgent_Click" Text="Remove Agent" CssClass="btn-default active" 
                    ToolTip="Remove this agent." />
            </a>
        </div>

        <div class="form-group">
            <asp:Label runat="server" CssClass="col-md-1 control-label">Billing Address:</asp:Label>
            <asp:Literal ID="BillAddr" runat="server" > </asp:Literal>
        </div>
        
        <br />
        <div class="form-inline" >
            <label>Select the update option for a unit</label>
        </div>

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

        <br />
        <div class="btn-group" style="padding-left:10px">
            <asp:Button runat="server" OnClick="NextBtn_Click" Text="Next" CssClass="btn-default active" ToolTip="Proceed to Update Unit or Tenancy."/>
            &nbsp;&nbsp;&nbsp;&nbsp; 
            <asp:Button runat="server" id="btnBack" OnClick="ToProperty_Click" Text="Back" CssClass="btn-default active" 
                ToolTip="Return to the list of Properties." />
        </div>

        <asp:Button ID="btnDialogResponseYes" runat="server" Text="Hidden Button" OnClick="DialogResponseYes"
            Style="display: none" UseSubmitBehavior="false" />
        <asp:Button ID="btnDialogResponseNo" runat="server" Text="Hidden Button" OnClick="DialogResponseNo"
            Style="display: none" UseSubmitBehavior="false" />
        <div id="dialog2" style="display: none">
        </div>
        <asp:HiddenField ID="hfDialogID" runat="server" />
    </div>
</asp:Content>
