<%@ Page Title="Search Accounts" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="SearchAccounts.aspx.vb" Inherits="BRBPortal.SearchAccounts" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%--<h2><%: Title %>.</h2>--%>

    <link rel="stylesheet" type="text/css" href="/Styles/StyleBRB.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/Baseline.Type.css" />

    <div class="form-horizontal" style="width: 809px; height: 520px; padding-left:30px" >
        <div class="form-group" style="height:50px; align-items:center">
            <h4>Portal Administratrion<br />UserManagement - Search</h4>
            <nav class="nav-right" style="float:right; width:100px">
                <a href="../Home">Home</a>
                &nbsp;&nbsp;
                <a href="../Logout">Logout</a>
            </nav>
        </div>

        <br />
        <asp:Label runat="server" >Search for user account:</asp:Label>
        <div class="form-group">
            <asp:Label runat="server" CssClass="col-md-1 control-label">User ID:</asp:Label>
            <asp:TextBox runat="server" ID="UserID" ToolTip="Enter a specific User Id."  />
        </div>

        <div class="form-group">
            <asp:Label runat="server" CssClass="col-md-1 control-label">Billing Code:</asp:Label>
            <asp:TextBox runat="server" ID="BillCode" ToolTip="Enter a specific Billing Code."  />
        </div>
        
        <div class="form-group">
            <asp:Label runat="server" CssClass="col-md-1 control-label">First Name:</asp:Label>
            <asp:TextBox runat="server" ID="FirstName" ToolTip="Enter a first name." />
            <asp:Label runat="server" CssClass="control-label">&nbsp;Last Name:&nbsp;&nbsp;</asp:Label>
            <asp:TextBox runat="server" ID="LastName" ToolTip="Enter a last name." />
        </div>

        <div class="form-group">
            <asp:Label runat="server" CssClass="col-md-1 control-label">Property Address:</asp:Label>
            <asp:TextBox runat="server" Width="300px" ID="PropAddress" ToolTip="Enter a property address."  />
        </div>

        <br />
        <div class="btn-group" style="padding-left:10px">
            <asp:Button runat="server" id="btnSearch" OnClick="Searchbtn_Click" Text="Search" CssClass="swd-button" 
                ToolTip="Search the accounts."/>
        </div>        

        <br /><br />
        <div class="form-group" style="margin-left:10px; height:180px">
            <table id="UnitList">
                <tr>
                    <th style="border:5px solid white; width: 30px; text-align:center"> </th>
                    <th style="background-color:aliceblue; border:5px solid white; width: 70px; text-align:center">User ID</th>
                    <th style="background-color:aliceblue; border:5px solid white; width: 100px; text-align:center">Name</th>  
                    <th style="background-color:aliceblue; border:5px solid white; width: 80px; text-align:center">Billing Code</th>    
                    <th style="background-color:aliceblue; border:5px solid white; width: 120px;text-align:center">Email Address</th>
                    <th style="background-color:aliceblue; border:5px solid white; width: 140px;text-align:center">Mailing Address</th>
                    <th style="background-color:aliceblue; border:5px solid white; width: 100px; text-align:center">Phone Number</th>
                </tr>
                <tr>
                    <td style="width: 30px"><input type="radio" id="Row1Sel" runat="server" style="margin-left:10px" /></td>
                    <td style="text-align:center; width: 70px" id="UserID1" runat="server">KSmith123</td>
                    <td style="text-align:center; width: 100px">Kelly Smith</td>
                    <td style="text-align:center; width: 80px">KSmith</td>
                    <td style="text-align:center; width: 120px">Ksmith@gmail.com</td>
                    <td style="text-align:left; width: 140px">1 Lane Street, Boston, MA 12345</td>
                    <td style="text-align:center; width: 100px">(245)234-0000</td>
                </tr>
                <tr>
                    <td style="width: 30px"><input type="radio" id="Row2Sel" runat="server" style="margin-left:10px" /></td>
                    <td style="text-align:center; width: 70px" id="UserID2" runat="server">SurferDude</td>
                    <td style="text-align:center; width: 100px">Mark E. Smith Jr.</td>
                    <td style="text-align:center; width: 80px">MSmith1</td>
                    <td style="text-align:center; width: 120px">SmithJr3@aol.com</td>
                    <td style="text-align:left; width: 140px">7 Wave Court, Santa Cruz, CA 94345</td>
                    <td style="text-align:center; width: 100px">(923)646-3222</td>
                </tr>
                <tr>
                    <td style="width: 30px"><input type="radio" id="Row3Sel" runat="server" style="margin-left:10px" /></td>
                    <td style="text-align:center; width: 70px" id="UserID3" runat="server">MBtywed</td>
                    <td style="text-align:center; width: 100px">Vic Smith</td>
                    <td style="text-align:center; width: 80px">VSmith</td>
                    <td style="text-align:center; width: 120px">VictorS@KBS.com</td>
                    <td style="text-align:left; width: 140px">123 Prince Street, Berkeley, CA 94704</td>
                    <td style="text-align:center; width: 100px">(510)981-4343</td>
                </tr>
            </table>
        </div>

<%--        <div class="form-group">
            <asp:RadioButtonList runat="server" ID="HomeOption" RepeatDirection="Vertical" ToolTip="Select your relationship." Height="66px" >
                    <asp:ListItem Enabled="true" Text="&nbsp;&nbsp;Manage Account Profile" Value="MngProfile"></asp:ListItem>
                    <asp:ListItem Enabled="true" Text="&nbsp;&nbsp;Manage Property Registration / Pay a Bill" Value="MngPay"></asp:ListItem>
            </asp:RadioButtonList>
        </div>--%>

        <br />
        <div class="btn-group" style="padding-left:10px">
            <asp:Button runat="server" id="btnUpdAcct" OnClick="UpdAcctbtn_Click" Text="Update" CssClass="swd-button" ToolTip="Proceed to Update Account."/>
            &nbsp;&nbsp;&nbsp;&nbsp; 
            <asp:Button runat="server" id="btnCreateAcct" OnClick="CreateAcct_Click" Text="Create New" CssClass="swd-button" 
                ToolTip="Proceed to create a new account." />
        </div>
    </div>
</asp:Content>
