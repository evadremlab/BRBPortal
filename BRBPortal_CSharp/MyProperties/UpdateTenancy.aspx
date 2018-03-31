<%@ Page Title="Update Tenancy" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UpdateTenancy.aspx.cs" Inherits="BRBPortal_CSharp.MyProperties.UpdateTenancy" %>
<%@ MasterType  virtualPath="~/Site.Master"%>

<%--data-lpignore="true" tells LastPass not to show ellipsis on form fields--%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        html, body { height: 101%; } /* always show scrollbar so the controls don't jump when hiding/showing fields */
        #MainContent_HServs { min-width: 35rem; margin-top: 0.7rem; }
        #MainContent_HServs label { padding-left: 0.5rem; }
    </style>

    <h2><%: Title %></h2>
    <h4>at <asp:Literal ID="Literal1" runat="server"></asp:Literal>, Unit # <asp:Literal ID="Literal2" runat="server" ></asp:Literal></h4>
    <hr />

    <section id="updateTenancyForm">
        <div class="form-horizontal">
            <!-- HEADER -->
            <div class="row">
                <asp:Label runat="server" AssociatedControlID="OwnerName" CssClass="col-md-2 control-label">Owner Name:&nbsp;</asp:Label>
                <div class="col-md-10 literal">
                    <asp:Literal ID="OwnerName" runat="server"></asp:Literal>
                </div>
            </div>
            <div class="row">
                <asp:Label runat="server" AssociatedControlID="AgentName" CssClass="col-md-2 control-label">Agent Name:&nbsp;</asp:Label>
                <div class="col-md-10 literal">
                    <asp:Literal ID="AgentName" runat="server" ></asp:Literal>
                </div>
            </div>
            <div class="row">
                <asp:Label runat="server" AssociatedControlID="BalAmt" CssClass="col-md-2 control-label">Total Balance:&nbsp;</asp:Label>
                <div class="col-md-10 literal">
                    <asp:Literal ID="BalAmt" runat="server" ></asp:Literal>
                </div>                
            </div>
            <div class="row">
                <asp:Label runat="server" AssociatedControlID="UnitStatus" CssClass="col-md-2 control-label">Unit Status:&nbsp;</asp:Label>
                <div class="col-md-10 literal">
                    <asp:Literal ID="UnitStatus" runat="server"></asp:Literal>
                </div>
            </div>
            <!-- FIELDS -->
            <div class="form-group" style="margin-top:1rem;">
                <asp:Label runat="server" AssociatedControlID="InitRent" CssClass="col-md-2 control-label">Initial Rent: $</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="InitRent" CssClass="form-control" TextMode="Number"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="InitRent" CssClass="text-danger" Display="Dynamic" ErrorMessage="required" />
                </div>
            </div>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="TenStDt" CssClass="col-md-2 control-label">Tenancy Start Date:</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="TenStDt" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TenStDt" CssClass="text-danger" Display="Dynamic" ErrorMessage="required" />
                </div>
            </div>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="HServs" CssClass="col-md-2 control-label">Housing Services: </asp:Label>
                <div class="col-md-10" style="max-width:15.5rem;">
                    <asp:CheckBoxList runat="server" ID="HServs" RepeatColumns="3" RepeatDirection="Horizontal" CellSpacing="0">
                        <asp:ListItem Text="Storage"></asp:ListItem>
                        <asp:ListItem Text="Gas"></asp:ListItem>
                        <asp:ListItem Text="Electricity"></asp:ListItem>
                        <asp:ListItem Text="Water"></asp:ListItem>
                        <asp:ListItem Text="Garbage"></asp:ListItem>
                        <asp:ListItem Text="Parking"></asp:ListItem>
                        <asp:ListItem Text="Laundry Access"></asp:ListItem>
                        <asp:ListItem Text="Heat"></asp:ListItem>
                        <asp:ListItem Text="Appliances"></asp:ListItem>
                        <asp:ListItem Text="Other"></asp:ListItem>
                    </asp:CheckBoxList>
                </div>
            </div><!-- do not save "Other" as HServs, take HServOthrBox-->

            <div class="form-group">
                <div class="col-md-2"></div>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="HServOthrBox" CssClass="form-control" data-lpignore="true" style="width:40rem; min-width:40rem;"></asp:TextBox>
                </div>
            </div>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="NumTenants" CssClass="col-md-2 control-label"># of Tenants: </asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="NumTenants" CssClass="form-control" TextMode="Number"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="NumTenants" CssClass="text-danger" Display="Dynamic" ErrorMessage="required" />
                </div>
            </div>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="RB1" CssClass="col-md-2 control-label">Does Lease Prohibit Smoking?</asp:Label>
                <div class="col-md-10">
                    <div class="radio radiobuttonlist" style="display:inline-block; padding-left:0.25rem;">
                        <asp:RadioButtonList runat="server" ID="RB1" RepeatDirection="Horizontal" ToolTip="Select Yes or No." CellPadding="4" style="position:relative; top:-0.4rem; left:-0.4rem;">
                        <asp:ListItem Enabled="true" Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Enabled="true" Text="No" Value="No"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="SmokeDt" CssClass="col-md-2 control-label">Effective date of prohibition on smoking:</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="SmokeDt" TextMode="Date" CssClass="form-control" style="margin-top:1rem;"></asp:TextBox>
                </div>
            </div>
        
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="PTenDt" CssClass="col-md-2 control-label">Prior Tenancy end date:&nbsp;</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="PTenDt" TextMode="Date" ToolTip="Enter the prior tenency date." CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="PTenDt" CssClass="text-danger" Display="Dynamic" ErrorMessage="required" />
                </div>
            </div>
        
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="TermReas" CssClass="col-md-2 control-label">Reason for termination:&nbsp;</asp:Label>
                <div class="col-md-10" style="max-width:24.5rem;">
                    <asp:DropDownList runat="server" ID="TermReas" ToolTip="Select a termination reason." CssClass="form-control selectpicker">
                        <%--<asp:ListItem enabled="true" text="" value=""></asp:ListItem>--%>
                        <asp:ListItem enabled="true" text="Voluntary Vacancy" value="1"></asp:ListItem>
                        <asp:ListItem enabled="true" text="Landlord move in" value="2"></asp:ListItem>
                        <asp:ListItem enabled="true" text="Non-payment of rent" value="3"></asp:ListItem>
                        <asp:ListItem enabled="true" text="Other" value="4"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TermReas" IntialValue="" CssClass="text-danger" Display="Dynamic" ErrorMessage="required" />
                </div>
            </div>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="TermDescr" CssClass="col-md-2 control-label">Explain Involuntary termination:</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="TermDescr" TextMode="MultiLine" MaxLength="200" CssClass="form-control" style="width:80rem; height:5.5rem;"  ToolTip="Enter the termination explaination." />
                </div>
            </div>

<%--            <asp:GridView ID="gvTenants" runat="server" CssClass="Margin30" AutoGenerateColumns="False"
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
                    ToolTip="Add tenant." />--%>

<%--            <div class="form-group" style="padding-left:20px; height:70px" id="AddTenant"  runat="server">
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
            </div>--%>

            <div class="col-md-offset-2 col-md-10">
                <div class="form-group">
                    <asp:CheckBox ID="chkDeclare" runat="server" Text="&nbsp;Declaration: I hereby declare under penalty of perjury that the Vacancy Registration Form is true and correct to the best of my knowledge and belief." />
                </div>

                <div class="form-group">
                    <Label runat="server" AssociatedControlID="DeclareInits" CssClass="control-label">Declaration initials:&nbsp;</Label>
                    <asp:TextBox runat="server" ID="DeclareInits" Width="70px" CssClass="form-control" ToolTip="Enter your initials acknowledging the Declaration above." />
                </div>

                <div class="form-group">
                    <asp:Button runat="server" id="btnCancel" UseSubmitBehavior="false" PostBackUrl="~/MyProperties/MyTenants.aspx" Text="Cancel" CssClass="btn btn-sm btn-default" ToolTip="Return to the list of Tenants." TabIndex="-1" />
                    <asp:Button runat="server" ID="btnUpdTen" Text="Confirm" OnClick="UpdateTenancy_Click" OnClientClick="return validate();" CssClass="btn btn-primary" ToolTip="Update the tenants." style="margin-left:1rem;" />
                </div>
            </div>
        </div>
        <div style="height:3rem;">&nbsp;</div><!-- to move buttons off the bottom of the screen -->
    </section>
    <script>
        var valErrors = [];

        function addValError(msg) {
            valErrors.push('<li>' + msg + '</li>');
        }

        function validate() {
            valErrors = [];

            return true;
        }
    </script>
</asp:Content>
