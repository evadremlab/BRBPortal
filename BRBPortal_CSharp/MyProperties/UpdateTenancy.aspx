<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UpdateTenancy.aspx.cs" Inherits="BRBPortal_CSharp.MyProperties.UpdateTenancy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hfDialogID" runat="server" />
    <asp:HiddenField ID="hfUnitID" runat="server" />
    <asp:HiddenField ID="hfOrigTenStDt" runat="server" />
    <asp:HiddenField ID="hfOwnerEmail" runat="server" />

    <div class="form-horizontal" style="width: 809px; height: 1200px; padding-left:30px" >
        <div class="form-group" style="height:40px; align-items:center">
            <h4>Update Tenancy</h4>
        </div>

        <div class="form-group" style="padding-left:10px; height:142px"  >
            <h4>
                <asp:Literal ID="MainAddress" runat="server" ></asp:Literal>
            </h4>
            
            <div class="form-group" style="padding-left:18px" >
                <asp:Label runat="server" CssClass="control-label">&nbsp;&nbsp;Owner Name:&nbsp;&nbsp;</asp:Label><asp:Literal ID="OwnerName" runat="server" ></asp:Literal>
                <asp:Label runat="server" CssClass="control-label">&nbsp;&nbsp;Agent Name:&nbsp;&nbsp;</asp:Label><asp:Literal ID="AgentName" runat="server" ></asp:Literal>
                <asp:Label runat="server" CssClass="control-label">&nbsp;&nbsp;Total Balance:&nbsp;&nbsp;</asp:Label><asp:Literal ID="BalAmt" runat="server" ></asp:Literal>
            </div>

            <table style="margin-left:10px">
                <tr>
                    <td>Unit #:&nbsp;&nbsp;</td>
                    <td><asp:Literal ID="UnitNo" runat="server" ></asp:Literal></td>
                </tr>
                <tr>
                    <td>Unit Status:&nbsp;&nbsp;</td>
                    <td><asp:Literal ID="UnitStatus" runat="server" ></asp:Literal></td>
                </tr>
            </table>
        </div>
        
        <div class="form-group" style="padding-left:10px">
            <asp:Label runat="server" AssociatedControlID="InitRent" Width="140px" CssClass="col-md-1 control-label">Initial Rent:&nbsp;&nbsp;&nbsp;$</asp:Label>
            <asp:TextBox runat="server" ID="InitRent"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="InitRent" ValidationGroup="CheckFields"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="Initial Rent is required." />
        </div>

        <div class="form-group" style="padding-left:10px">
            <asp:Label runat="server" AssociatedControlID="TenStDt" Width="160px" CssClass="col-md-1 control-label">Tenancy Start Date:</asp:Label>
            <asp:TextBox runat="server" ID="TenStDt" TextMode="Date"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="TenStDt" ValidationGroup="CheckFields"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="Tenancy Start Date is required." />
        </div>

        <br />
        <div class="form-group" style="padding-left:10px; height:120px">
            <asp:Label runat="server" AssociatedControlID="HServs" Width="140px" CssClass="col-md-1 control-label">Housing Services: </asp:Label>
            <asp:CheckBoxList runat="server" ID="HServs" RepeatColumns="3" RepeatDirection="Horizontal" Width="400px" CellSpacing="0">
                <asp:ListItem Text="Storage"></asp:ListItem>
                <asp:ListItem Text="Gas"></asp:ListItem>
                <asp:ListItem Text="Electricity"></asp:ListItem>
                <asp:ListItem Text="Water"></asp:ListItem>
                <asp:ListItem Text="Garbage"></asp:ListItem>
                <asp:ListItem Text="Parking"></asp:ListItem>
                <asp:ListItem Text="Laundry Access"></asp:ListItem>
                <asp:ListItem Text="Heat"></asp:ListItem>
                <asp:ListItem Text="Appliances"></asp:ListItem>
            </asp:CheckBoxList>
            <div class="form-group" style="padding-left:155px" >
                <asp:checkbox runat="server" ID="HServsOthr"  Width="80px" Text="Other"></asp:checkbox>
                <asp:TextBox runat="server" ID="HServOthrBox" MaxLength="150" ></asp:TextBox>
            </div>
        </div>

        <br />
        <div class="form-group" style="padding-left:10px">
            <label class="control-label" style="width:100px; padding-bottom:4px"># of tenants:</label>
            <asp:TextBox runat="server" ID="NumTenants" Width="70px" ></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="NumTenants" ValidationGroup="CheckFields"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="Number of Tenants is required." />
        </div>

        <div class="Prompt" style="align-content:stretch">
            <label for="RB1">Does Lease Prohibit Smoking?&nbsp;&nbsp;</label>
            <input type="radio" runat="server" name="RB1" value="Yes" id="RB1Y" />
            <label>Yes</label>
            <input type="radio" runat="server" name="RB1" value="No" id="RB1N" />
            <label>No</label>
            <asp:Label runat="server" AssociatedControlID="SmokeDt" Width="280px" style="padding-left:10px"
                CssClass="control-label">Effective date of prohibition on smoking:</asp:Label>
            <asp:TextBox runat="server" ID="SmokeDt" TextMode="Date"></asp:TextBox>
        </div>
        
        <br />
        <div class="form-group" style="padding-left:10px">
            <label class="control-label" style="width:160px; padding-bottom:4px">Prior Tenancy end date:</label>
            <asp:TextBox runat="server" ID="PTenDt" TextMode="Date" ToolTip="Enter the prior tenency date." />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="PTenDt" ValidationGroup="CheckFields"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="Prior Tenancy end date is required." />
        </div>
        
        <div class="form-group" style="padding-left:10px">
            <label class="control-label"  style="padding-bottom:4px; width:160px" >Reason for termination:</label>
            <asp:dropdownlist runat="server" ID="TermReas" Height="20px" ToolTip="Select a termination reason." >
                <asp:ListItem enabled="true" text="" value="-1"></asp:ListItem>
                <asp:ListItem enabled="true" text="Voluntary Vacancy" value="1"></asp:ListItem>
                <asp:ListItem enabled="true" text="Landlord move in" value="2"></asp:ListItem>
                <asp:ListItem enabled="true" text="Non-payment of rent" value="3"></asp:ListItem>
                <asp:ListItem enabled="true" text="Other" value="4"></asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="TermReas" ValidationGroup="CheckFields"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="Reason for termination is required." />
        </div>

        <%--<br />--%>
        <div class="form-group" style="padding-left:10px">
            <label class="control-label" style="width:220px; padding-bottom:4px" >Explain Involuntary termination:</label>
            <asp:TextBox runat="server" ID="TermDescr" Width="500px" ToolTip="Enter the termination explaination." MaxLength="200" />
        </div>

        <br />
        <asp:GridView ID="gvTenants" runat="server" CssClass="Margin30" AutoGenerateColumns="False"
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
        <asp:Button runat="server" ID="btnAddTenant" OnClick="AddTenant_Click" Text="+" Font-Bold="true" CssClass="btn btn-sm btn-default" 
                ToolTip="Add tenant." />

        <div class="form-group" style="padding-left:20px; height:70px" id="AddTenant"  runat="server">
            <asp:Label runat="server" AssociatedControlID="NewFirst" Width="100px" CssClass="control-label">First Name</asp:Label>
            <asp:Label runat="server" AssociatedControlID="NewLast" Width="100px" CssClass="control-label">Last Name</asp:Label>
            <asp:Label runat="server" AssociatedControlID="NewPhon" Width="100px" CssClass="control-label">Phone No</asp:Label>
            <asp:Label runat="server" AssociatedControlID="NewEmail" Width="100px" CssClass="control-label">Email</asp:Label>
            <br />
            <asp:TextBox runat="server" Width="100px" ID="NewFirst" TextMode="SingleLine"></asp:TextBox>
            <asp:TextBox runat="server" Width="100px" ID="NewLast" TextMode="SingleLine"></asp:TextBox>
            <asp:TextBox runat="server" Width="100px" ID="NewPhon" TextMode="SingleLine"></asp:TextBox>
            <asp:TextBox runat="server" Width="100px" ID="NewEmail" TextMode="SingleLine"></asp:TextBox>
            <br />
            <asp:Button runat="server" ID="SaveNewTen" OnClick="SaveNewTenant_Click" Text="Save Tenant" CssClass="btn btn-sm btn-primary" ToolTip="Save the tenant." />
            <asp:Button runat="server" id="CancelNewTen" OnClick="CancelNewTenant_Click" Text="Cancel Tenant" CssClass="btn btn-sm btn-danger" ToolTip="Discard this tenant." />
        </div>

        <br /> <br />
        <div class="form-group" style="padding-left:10px; height:40px; width:700px">
            <asp:CheckBox ID="chkDeclare" runat="server" 
                Text="&nbsp;&nbsp;&nbsp; Declaration:  I hereby declare under penalty of perjury that all the information in this "
                CausesValidation="True" AutoPostBack="True" />
            <label style="padding-left:34px">Vacancy Registration Form is true and correct to the best of my knowledge and belief. </label>
        </div>
        
        <div class="form-group" style="padding-left:43px">
            <asp:Label runat="server" AssociatedControlID="DeclareInits" Width="140px" style="padding-bottom:4px"
                CssClass="control-label">Declaration initials: </asp:Label>
            <asp:TextBox runat="server" Width="70px" ID="DeclareInits"  ToolTip="Enter your initials acknowledging the Declaration above." />
        </div>


        <div class="form-group">
            <asp:Button runat="server" id="btnCancel" UseSubmitBehavior="false" PostBackUrl="~/MyProperties/MyTenants.aspx" Text="Cancel" CssClass="btn btn-sm btn-default" ToolTip="Return to the list of Tenants." TabIndex="-1" />
            <asp:Button runat="server" ID="btnUpdTen" OnClick="UpdateTenancy_Click" Text="Submit" CssClass="btn btn-primary" ToolTip="Update the tenants." ValidationGroup="CheckFields" style="margin-left:1rem;" />
        </div>
    </div>
<!--
    HServsOthr => HServsOthr_CheckedChanged
-->
</asp:Content>
