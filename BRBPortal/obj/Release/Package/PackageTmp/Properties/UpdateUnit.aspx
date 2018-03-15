<%@ Page Title="UpdateUnit" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="UpdateUnit.aspx.vb" Inherits="BRBPortal.UpdateUnit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%--<h2><%: Title %>.</h2>--%>

    <script src="/Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.dynDateTime.min.js" type="text/javascript"></script>
    <script src="/Scripts/calendar-en.min.js" type="text/javascript"></script>

    <script>
        $( function() {
        $( "#datepicker" ).datepicker();
        } );
    </script>

    <link rel="stylesheet" type="text/css" href="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/themes/start/jquery-ui.css" />
    <link rel="stylesheet" type="text/css" href="../Content/bootstrap.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/StyleBRB.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/Baseline.Type.css" />
    
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/ClientPortal.js") %>"> </script>
    <!-- Modal Dialog Box Ajax Code -->
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script src="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/jquery-ui.js" type="text/javascript"></script>

    <div class="form-horizontal" style="width: 880px; height: 1200px; padding-left:30px" >
        <div class="form-group" style="height:40px; align-items:center">
            <h4>Update Unit Status</h4>
            <nav class="nav-right" style="float:right">
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
                <p class="text-danger" >
                    <asp:Literal runat="server" ID="FailureText" />
                </p>
            </asp:PlaceHolder>
        </div>

        <div class="form-group" style="padding-left:10px; height:219px"  >
            <h4>
                <asp:Literal ID="MainAddress" runat="server" ></asp:Literal>
            </h4>
            <h4>Current Unit Status</h4>
            <table style="margin-left:15px">
                <tr>
                    <td>Unit #:</td>
                    <td><asp:Literal ID="UnitNo" runat="server" ></asp:Literal></td>
                </tr>
                <tr>
                    <td>Unit Status:</td>
                    <td><asp:Literal ID="UnitStatus" runat="server" ></asp:Literal></td>
                </tr>
                <tr>
                    <td>Exemption Reason:&nbsp;&nbsp;</td>
                    <td><asp:Literal ID="ExemptReas" runat="server" ></asp:Literal></td>
                </tr>
            </table>
            <label style="margin-left:15px; font:normal">Exemption Additional Information:</label>
            <table style="margin-left:15px">
                <tr>
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Date Started:&nbsp;&nbsp;</td>
                    <td><asp:Literal ID="UnitStartDt" runat="server" ></asp:Literal></td>
                </tr>
                <tr>
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Occupied By:&nbsp;&nbsp;</td>
                    <td><asp:Literal ID="UnitOccBy" runat="server" ></asp:Literal></td>
                </tr>
            </table>
        </div>
        
        <div class="form-group" style="padding-left:10px; height:70px" >
            <h4>New Unit Status</h4>
            <asp:Label runat="server" AssociatedControlID="NewUnit" Font-Size="Medium" Width="150px" CssClass="col-md-1 control-label">New Unit Status: </asp:Label>
            <asp:RadioButtonList runat="server" ID="NewUnit" RepeatDirection="Horizontal" OnSelectedIndexChanged="NewUnit_Clicked"
                 ToolTip="Select unit status." AutoPostBack="True" CellPadding="4" >
                    <asp:ListItem Enabled="true" Text="Rented" Value="Rented"></asp:ListItem>
                    <asp:ListItem Enabled="true" Text="Exempt" Value="Exempt"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
        
        <div class="form-group" style="height:155px; padding-left:10px" id="ExemptGroup" runat="server">
            <asp:Label runat="server" AssociatedControlID="ExemptReason" Width="160px" CssClass="col-md-1 control-label">Exemption Reason: </asp:Label>
            <asp:RadioButtonList runat="server" ID="ExemptReason" RepeatDirection="Vertical" Width="243px" 
                OnSelectedIndexChanged="Exemption_Clicked" ToolTip="Select exemption reason." AutoPostBack="true" >
                <asp:ListItem Enabled="true" Text="Vacant and not available for rent" Value="NAR"></asp:ListItem>
                <asp:ListItem Enabled="true" Text="Owner-Occupied" Value="OOCC"></asp:ListItem>
                <asp:ListItem Enabled="true" Text="Section 8" Value="SEC8"></asp:ListItem>
                <asp:ListItem Enabled="true" Text="Occupied Rent Free" Value="FREE"></asp:ListItem>
                <asp:ListItem Enabled="true" Text="Other" Value="OTHER"></asp:ListItem>
            </asp:RadioButtonList>
            <asp:dropdownlist runat="server" ID="OtherList" ToolTip="Select a reason from the list (optional)."
                onSelectedIndexChanged="OtherList_Clicked" AutoPostBack="true" >
                <asp:ListItem enabled="true" text="" value="-1"></asp:ListItem>
                <asp:ListItem enabled="true" text="Commercial Use" value="COMM"></asp:ListItem>
                <%--<asp:ListItem enabled="true" text="Owner-Occupied Exempt Duplex" value="2"></asp:ListItem>--%>
                <asp:ListItem enabled="true" text="Property Manager's Unit" value="MISC"></asp:ListItem>
                <asp:ListItem enabled="true" text="Owner shares kitchen & bath with tenant" value="SHARED"></asp:ListItem>
                <asp:ListItem enabled="true" text="Shelter Plus Care" value="SPLUS"></asp:ListItem>
            </asp:DropDownList>
        </div>

        <br />
        <label style="font:normal">Exemption Additional Information:</label>
        
        <div class="form-group" style="padding-left:10px" runat="server" id="AsOfDtGrp">
            <asp:Label runat="server" AssociatedControlID="UnitAsOfDt" Width="120px" CssClass="col-md-1 control-label">As of Date: </asp:Label>
            <asp:TextBox runat="server" ID="UnitAsOfDt" TextMode="Date" ToolTip="Enter the as of date for this change."  />
        </div>

        <div class="form-group" style="padding-left:10px" runat="server" id="DtStrtdGrp">
            <asp:Label runat="server" AssociatedControlID="StartDt" Width="120px" CssClass="col-md-1 control-label">Date Started: </asp:Label>
            <asp:TextBox runat="server" ID="StartDt" TextMode="Date" ToolTip="Enter the start date." />
        </div>
        
        <div class="form-group" style="padding-left:10px" runat="server" id="OccByGrp">
            <asp:Label runat="server" AssociatedControlID="OccupiedBy" Width="120px" CssClass="col-md-1 control-label">Occupied By: </asp:Label>
            <asp:TextBox runat="server" ID="OccupiedBy" ToolTip="Enter name of tenant."  />
       </div>


        <div class="form-group" style="padding-left:10px" runat="server" id="ContractGrp">
            <asp:Label runat="server" AssociatedControlID="ContractNo" Width="120px" CssClass="col-md-1 control-label">Contract #: </asp:Label>
            <asp:TextBox runat="server" ID="ContractNo" ToolTip="Enter any contract number." />
        </div>

        <div class="Prompt" style="align-content:stretch; padding-left:10px" id="CommUseGrp" runat="server">
            <asp:Label runat="server" AssociatedControlID="CommUseDesc">Please describe the current commercial use of this unit&nbsp;&nbsp;</asp:Label>
            <asp:TextBox runat="server" ID="CommUseDesc" Width="240px" ToolTip="Enter commercial use description." />

            <label for="RB1">Is the property zoned for commercial use?&nbsp;</label>
            <input type="radio" runat="server" name="RB1" value="Yes" id="RB1Y" onclick="RB1_Clicked" />
            <label>Yes&nbsp;</label>
            <input type="radio" runat="server" name="RB1" value="No" id="RB1N" onclick="RB1_Clicked" />
            <label>No</label>
            <label>&nbsp;If yes, please describe the approved use</label>
            <input type="text" id="CommZoneUse"  runat="server" ToolTip="Enter commercial zoning description." />

            <asp:Label runat="server" AssociatedControlID="CommResYN" width=360px>Is the unit used exclusively for commercial use?</asp:Label>
            <asp:RadioButtonList runat="server" ID="CommResYN" RepeatDirection="Horizontal" ToolTip="Select Yes or No." >
                    <asp:ListItem Enabled="true" Text="Yes" Value="Yes"></asp:ListItem>
                    <asp:ListItem Enabled="true" Text="No" Value="No"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
                
        <div class="form-group" style="padding-left:25px; height:50px" runat="server" id="PMUnitGrp" >
            <label class="control-label" style="width:350px">Name(s) of the Property Manager residing in the unit</label>
            <asp:TextBox runat="server" ID="PropMgrName" ToolTip="Enter the property manager name(s)." />
            <br />
            <label class="control-label" style="width:360px">Email address and/or phone number Property Manager</label>
            <asp:TextBox runat="server" ID="PMEmailPhone" ToolTip="Enter the property manager email and/or phone number." />
        </div>
                        
        <div class="form-group" style="padding-left:10px; height:170px" runat="server" id="OwnerShrGrp">
            <asp:Label runat="server" AssociatedControlID="PrincResYN" Width="370px"
                CssClass="col-md-1 control-label">Is this unit the owner's principal place of residence?</asp:label>
            <asp:RadioButtonList runat="server" ID="PrincResYN" RepeatDirection="Horizontal" CellPadding="3" ToolTip="Select Yes or No." >
                <asp:ListItem Enabled="true" Text="Yes" Value="Yes"></asp:ListItem>
                <asp:ListItem Enabled="true" Text="No" Value="No"></asp:ListItem>
            </asp:RadioButtonList>

            <asp:Label runat="server" AssociatedControlID="MultiUnitYN" Width="520px"
                CssClass="col-md-1 control-label">Does the owner reside in any other unit on the property other than this unit?</asp:label>
            <asp:RadioButtonList runat="server" ID="MultiUnitYN" RepeatDirection="Horizontal" CellPadding="3" ToolTip="Select Yes or No." >
                <asp:ListItem Enabled="true" Text="Yes" Value="Yes"></asp:ListItem>
                <asp:ListItem Enabled="true" Text="No" Value="No"></asp:ListItem>
            </asp:RadioButtonList>

            <div class="form-group" style="padding-left:150px; width:613px">
                <label class="control-label" style="width:250px">If Yes, please indicate which unit(s)</label>
                <asp:TextBox runat="server" ID="OtherUnits" ToolTip="Enter the other units the owner occupies." />
            </div>

            <div class="form-group" style="padding-left:25px">
                <label class="control-label" style="width:500px">Name and contact information of the tenants residing in the unit?  Name(s)</label>
                <asp:TextBox runat="server" ID="TenantNames" ToolTip="Enter the names of the tenants." />
            </div>
                                           
            <div class="form-group" style="padding-left:350px; width:600px">
                <label class="control-label" style="width:120px">Contact info</label>
                <asp:TextBox runat="server" ID="TenantContacts" ToolTip="Enter the contact information for the tenants." />
            </div>
        </div>
        
        <br />
        <div class="form-group" style="padding-left:25px">
            <asp:CheckBox ID="chkDeclare" runat="server" Text="&nbsp;&nbsp;&nbsp;Declaration:  I hereby declare under penalty of perjury that .." />
        </div>
        
        <div class="form-group" style="padding-left:36px">
            <Label runat="server" AssociatedControlID="DeclareInits" CssClass="col-md-1 control-label">&nbsp;&nbsp;&nbsp;Declaration initials: </Label>
            <asp:TextBox runat="server" ID="DeclareInits" Width="60px" ToolTip="Enter your initials acknowledging the Declaration above." />
        </div>

        <br />
        <div class="btn-group" style="padding-left:25px">
            <asp:Button runat="server" ID="btnConfirm" OnClick="UpdateUnit_Click" Text="Confirm" CssClass="btn-default active" 
                ToolTip="Update this unit." />
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button runat="server" id="btnCancel" OnClick="CancelEdit_Click" Text="Cancel" CssClass="btn-default active" 
                ToolTip="Returns to list of units." />
        </div>

        <asp:Button ID="btnDialogResponseYes" runat="server" Text="Hidden Button" OnClick="DialogResponseYes"
            Style="display: none" UseSubmitBehavior="false" />
        <asp:Button ID="btnDialogResponseNo" runat="server" Text="Hidden Button" OnClick="DialogResponseNo"
            Style="display: none" UseSubmitBehavior="false" />
        <div id="dialog2" style="display: none">
        </div>
        <asp:HiddenField ID="hfDialogID" runat="server" />
        <asp:HiddenField ID="hfUnitID" runat="server" />

    </div>
</asp:Content>