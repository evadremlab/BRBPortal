<%@ Page Title="List of Tenants" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="MyTenants.aspx.vb" Inherits="BRBPortal.MyTenants" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%--<h2><%: Title %>.</h2>--%>

    <link rel="stylesheet" type="text/css" href="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/themes/start/jquery-ui.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/StyleBRB.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/Baseline.Type.css" />
    
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/ClientPortal.js") %>"> </script>
    <!-- Modal Dialog Box Ajax Code -->
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script src="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/jquery-ui.js" type="text/javascript"></script>

    <div class="form-horizontal" style="width: 809px; height: 800px; padding-left:30px" >
        <div class="form-group" style="height:40px; align-items:center">
            <h4>View Tenancy</h4>
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

        <div class="form-group" style="padding-left:10px">
            <h4 style="width:450px">
                <asp:Literal ID="MainAddress" runat="server" ></asp:Literal>
            </h4>
        </div>

        <br />
        <div class="form-group">
            <asp:Label runat="server" CssClass="control-label">&nbsp;&nbsp;Owner Name:&nbsp;&nbsp;</asp:Label><asp:Literal ID="OwnerName" runat="server" ></asp:Literal>
            <asp:Label runat="server" CssClass="control-label">&nbsp;&nbsp;Agent Name:&nbsp;&nbsp;</asp:Label><asp:Literal ID="AgentName" runat="server" ></asp:Literal>
            <asp:Label runat="server" CssClass="control-label">&nbsp;&nbsp;Total Balance:&nbsp;&nbsp;</asp:Label><asp:Literal ID="BalAmt" runat="server" ></asp:Literal>
        </div>

        <table>
            <tr>
                <td>Unit #:</td>
                <td><asp:Literal ID="UnitNo" runat="server" ></asp:Literal></td>
            </tr>
            <tr>
                <td>Unit Status:</td>
                <td><asp:Literal ID="UnitStat" runat="server" ></asp:Literal></td>
            </tr>
            <tr>
                <td>Initial Rent:</td>
                <td><asp:Literal ID="InitRent" runat="server" ></asp:Literal></td>
            </tr>
            <tr>
                <td>Tenancy Start Date:&nbsp;&nbsp;</td>
                <td><asp:Literal ID="TenStDt" runat="server" ></asp:Literal></td>
            </tr>
            <tr>
                <td>Housing Services:</td>
                <td><asp:Literal ID="HouseServs" runat="server" ></asp:Literal></td>
            </tr>
            <tr>
                <td># of tenants:</td>
                <td><asp:Literal ID="NumTenants" runat="server" ></asp:Literal></td>
            </tr>
        </table>

        <div class="form-group" style="height:40px">
            <asp:Label runat="server" CssClass="col-md-1 control-label" Width="250px">Smoking prohibition in lease status:</asp:Label>
            <asp:Literal ID="SmokYN" runat="server" ></asp:Literal>
            <asp:Label runat="server" CssClass="control-label">&nbsp;&nbsp;Smoking prohibition effective date:&nbsp;&nbsp;</asp:Label>
            <asp:Literal ID="SmokDt" runat="server" ></asp:Literal>
            <br />
            <asp:Label runat="server" CssClass="col-md-1 control-label">Prior Tenancy end date:</asp:Label>
            <asp:Literal ID="PriorEndDt" runat="server" ></asp:Literal>
            <asp:Label runat="server" CssClass="control-label">&nbsp;&nbsp;Reason for termination:&nbsp;&nbsp;</asp:Label>
            <asp:Literal ID="TermReason" runat="server" ></asp:Literal>
        </div>

        <br />
        <asp:GridView ID="gvTenants" runat="server" AutoGenerateColumns="False"
            onpageindexchanging="gvTenants_PageIndexChanging" CellPadding="4" ForeColor="#333333" GridLines="None" AllowPaging="True" >
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="TenantID" HeaderText="Tenant ID" Visible="False" />
                <asp:BoundField DataField="FirstName" HeaderText="First Name" Visible="False" />
                <asp:BoundField DataField="LastName" HeaderText="Last Name" Visible="False" />
                <asp:BoundField HeaderText="Tenant Name" DataField="DispName" SortExpression="DispName">
                <ItemStyle HorizontalAlign="Left" Width="250px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Phone Number" DataField="PhoneNo" SortExpression="PhoneNo">
                <ItemStyle HorizontalAlign="Left" Width="100px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Email Address" DataField="EmailAddr" SortExpression="EmailAddr">
                <ItemStyle HorizontalAlign="Left" Width="220px" />
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
            <asp:Button runat="server" OnClick="btnUpdTen_Click" Text="Update Tenancy" CssClass="btn-default active" 
                ToolTip="Proceed to Update Tenants." />
            &nbsp;&nbsp;&nbsp;&nbsp; 
            <asp:Button runat="server" id="btnBack" OnClick="ToUnits_Click" Text="Back" CssClass="btn-default active" 
                ToolTip="Return to the list of Units." />
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
