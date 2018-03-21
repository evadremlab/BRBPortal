<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UpdateUnit.aspx.cs" Inherits="BRBPortal_CSharp.MyProperties.UpdateUnit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hfDialogID" runat="server" />
    <asp:HiddenField ID="hfUnitID" runat="server" />

    <h2>Update Unit Status</h2>

    <div class="form-horizontal">
        <section id="updateUnitForm">
            <Div class="form-horizontal">
                <h4><asp:Literal ID="MainAddress" runat="server"></asp:Literal></h4>
                <hr />
                <div class="form-group">
                    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                        <p class="text-danger" >
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
                    </asp:PlaceHolder>
                </div>
                
                <h4>Current Unit Status</h4>
                
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="UnitNo" CssClass="control-label">Unit #: </asp:Label>
                    <asp:Literal ID="UnitNo" runat="server" ></asp:Literal>
                    <asp:Label runat="server" AssociatedControlID="UnitStatus" CssClass="control-label">Unit Status: </asp:Label>
                    <asp:Literal ID="UnitStatus" runat="server" ></asp:Literal>
                    <br />
                    <asp:Label runat="server" AssociatedControlID="ExemptReas" CssClass="control-label">Exemption Reason: </asp:Label>
                    <asp:Literal ID="ExemptReas" runat="server" ></asp:Literal>
                </div>

                <h4>Exemption Additional Information</h4>

                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="UnitStartDt" CssClass="control-label">Date Started: </asp:Label>
                    <asp:Literal ID="UnitStartDt" runat="server" ></asp:Literal>

                    <asp:Label runat="server" AssociatedControlID="UnitOccBy" CssClass="control-label">Occupied By: </asp:Label>
                    <asp:Literal ID="UnitOccBy" runat="server" ></asp:Literal>
                </div>

                <h4>New Unit Status</h4>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="NewUnit" CssClass="col-md-2 control-label">New Unit Status: </asp:Label>
                    <div class="col-md-10 radio radiobuttonlist">
                        <asp:RadioButtonList runat="server" ID="NewUnit" RepeatDirection="Horizontal" ToolTip="Select unit status." CellPadding="4" style="position:relative; top:-0.4rem; left:-0.4rem;">
                            <asp:ListItem Enabled="true" Text="Rented" Value="Rented"></asp:ListItem>
                            <asp:ListItem Enabled="true" Text="Exempt" Value="Exempt"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <div class="form-group" id="ExemptGroup" runat="server">
                    <asp:Label runat="server" AssociatedControlID="ExemptReason" CssClass="col-md-2 control-label">Exemption Reason: </asp:Label>
                    <div class="col-md-10 radio radiobuttonlist">
                        <asp:RadioButtonList runat="server" ID="ExemptReason" RepeatDirection="Vertical" ToolTip="Select exemption reason.">
                            <asp:ListItem Enabled="true" Text="Vacant and not available for rent" Value="NAR"></asp:ListItem>
                            <asp:ListItem Enabled="true" Text="Owner-Occupied" Value="OOCC"></asp:ListItem>
                            <asp:ListItem Enabled="true" Text="Section 8" Value="SEC8"></asp:ListItem>
                            <asp:ListItem Enabled="true" Text="Occupied Rent Free" Value="FREE"></asp:ListItem>
                            <asp:ListItem Enabled="true" Text="Other" Value="OTHER"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:dropdownlist runat="server" ID="OtherList" RepeatDirection="Vertical" ToolTip="Select a reason from the list (optional)." CssClass="form-control">
                            <asp:ListItem enabled="true" text="" value="-1"></asp:ListItem>
                            <asp:ListItem enabled="true" text="Commercial Use" value="COMM"></asp:ListItem>
                            <asp:ListItem enabled="true" text="Property Manager's Unit" value="MISC"></asp:ListItem>
                            <asp:ListItem enabled="true" text="Owner shares kitchen & bath with tenant" value="SHARED"></asp:ListItem>
                            <asp:ListItem enabled="true" text="Shelter Plus Care" value="SPLUS"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <h4>Exemption Additional Information</h4>

                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="UnitAsOfDt" CssClass="col-md-2 control-label">As of Date: </asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="UnitAsOfDt" TextMode="Date" CssClass="form-control" ToolTip="Enter the as of date for this change."  />
                    </div>
                </div>
                <%--<div class="form-group" runat="server" id="AsOfDtGrp"></div>--%>
                <div class="form-group" runat="server" id="DtStrtdGrp">
                    <asp:Label runat="server" AssociatedControlID="StartDt" CssClass="col-md-2 control-label">Date Started: </asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="StartDt" TextMode="Date" CssClass="form-control" ToolTip="Enter the start date." />
                    </div>
                </div>
                <div class="form-group" runat="server" id="OccByGrp">
                    <asp:Label runat="server" AssociatedControlID="OccupiedBy" CssClass="col-md-2 control-label">Occupied By: </asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="OccupiedBy" CssClass="form-control" ToolTip="Enter name of tenant." data-lpignore="true" />
                    </div>
               </div>
                <div class="form-group" runat="server" id="ContractGrp">
                    <asp:Label runat="server" AssociatedControlID="ContractNo" CssClass="col-md-2 control-label">Contract #: </asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="ContractNo" CssClass="form-control" ToolTip="Enter any contract number." />
                    </div>
                </div>
                <div class="form-group">
                    <div class="Prompt" style="align-content:stretch;" id="CommUseGrp" runat="server">
                        <asp:Label runat="server" AssociatedControlID="CommUseDesc" CssClass="control-label">Please describe the current commercial use of this unit: </asp:Label>
                        <br />
                        <asp:TextBox runat="server" ID="CommUseDesc" TextMode="MultiLine" CssClass="form-control" style="width:80rem; height:7.5rem;" ToolTip="Enter commercial use description." />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="RB1" CssClass="control-label">Is the property zoned for commercial use?</asp:Label>
                        <div class="radio radiobuttonlist" style="display:inline-block;">
                            <asp:RadioButtonList runat="server" ID="RB1" RepeatDirection="Horizontal" ToolTip="Select Yes or No." CellPadding="4" style="position:relative; top:-0.4rem; left:-0.4rem;">
                            <asp:ListItem Enabled="true" Text="Yes" Value="Yes"></asp:ListItem>
                            <asp:ListItem Enabled="true" Text="No" Value="No"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>

                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="CommResYN" CssClass="control-label">Is the unit used exclusively for commercial use?</asp:Label>
                    <div class="radio radiobuttonlist" style="display:inline-block;">
                        <asp:RadioButtonList runat="server" ID="CommResYN" RepeatDirection="Horizontal" ToolTip="Select Yes or No." CellPadding="4" style="position:relative; top:-0.4rem; left:-0.4rem;">
                            <asp:ListItem Enabled="true" Text="Yes" Value="Yes"></asp:ListItem>
                            <asp:ListItem Enabled="true" Text="No" Value="No"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="PropMgrName" CssClass="control-label">Name(s) of the Property Manager residing in the unit: </asp:Label>
                    <br />
                    <asp:TextBox runat="server" ID="PropMgrName" TextMode="MultiLine" CssClass="form-control" style="width:80rem;" ToolTip="Enter the property manager name(s)." />
                </div>

                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="PMEmailPhone" CssClass="control-label">Email address and/or phone number for Property Manager: </asp:Label>
                    <br />
                    <asp:TextBox runat="server" ID="PMEmailPhone" TextMode="MultiLine" CssClass="form-control" style="width:80rem;" ToolTip="Enter the property manager email and/or phone number." />
                </div>

                <%--<div class="form-group" runat="server" id="PMUnitGrp"></div>--%>

                <div runat="server" id="OwnerShrGrp">
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="PrincResYN" CssClass="control-label">Is this unit the owner's principal place of residence?</asp:label>
                        <div class="radio radiobuttonlist" style="display:inline-block;">
                            <asp:RadioButtonList runat="server" ID="PrincResYN" RepeatDirection="Horizontal" ToolTip="Select Yes or No." CellPadding="4" style="position:relative; top:-0.4rem; left:-0.4rem;">
                                <asp:ListItem Enabled="true" Text="Yes" Value="Yes"></asp:ListItem>
                                <asp:ListItem Enabled="true" Text="No" Value="No"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="MultiUnitYN" CssClass="control-label">Does the owner reside in any other unit on the property other than this unit?</asp:label>
                        <div class="radio radiobuttonlist" style="display:inline-block;">
                            <asp:RadioButtonList runat="server" ID="MultiUnitYN" RepeatDirection="Horizontal" ToolTip="Select Yes or No." CellPadding="4" style="position:relative; top:-0.4rem; left:-0.4rem;">
                                <asp:ListItem Enabled="true" Text="Yes" Value="Yes"></asp:ListItem>
                                <asp:ListItem Enabled="true" Text="No" Value="No"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="OtherUnits" CssClass="control-label">If Yes, please indicate which unit(s): </asp:Label>
                        <asp:TextBox runat="server" ID="OtherUnits" TextMode="SingleLine" CssClass="form-control" ToolTip="Enter the other units the owner occupies." />
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="TenantNames" CssClass="control-label">Name and contact information of the tenants residing in the unit?  Name(s): </asp:Label>
                        <br />
                        <asp:TextBox runat="server" ID="TenantNames" TextMode="MultiLine" CssClass="form-control" style="width:80rem; height:7.5rem;" ToolTip="Enter the names of the tenants." />
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="TenantContacts" CssClass="control-label">Contact info: </asp:Label>
                        <br />
                        <asp:TextBox runat="server" ID="TenantContacts" TextMode="MultiLine" CssClass="form-control" style="width:80rem; height:7.5rem;" ToolTip="Enter the contact information for the tenants." />
                    </div>
                </div>

                <div class="form-group">
                    <asp:CheckBox ID="chkDeclare" runat="server" Text="Declaration: I hereby declare under penalty of perjury that .." CssClass="checkbox bold" />
                </div>

                <div class="form-group">
                    <Label runat="server" AssociatedControlID="DeclareInits" CssClass="control-label">Declaration initials: </Label>
                    <asp:TextBox runat="server" ID="DeclareInits" Width="70px" CssClass="form-control" ToolTip="Enter your initials acknowledging the Declaration above." />
                </div>
                <div class="form-group">
                    <asp:Button runat="server" ID="btnConfirm" OnClick="UpdateUnit_Click" Text="Confirm" CssClass="btn btn-primary" ToolTip="Update this unit." />
                    <asp:Button runat="server" id="btnCancel" OnClick="CancelEdit_Click" Text="Cancel" CssClass="btn btn-default" ToolTip="Returns to list of units." />
                </div>
            </div>
        </section>
    </div>

    <script>
        $(document).ready(function () {
            $("#datepicker").datepicker();
        });
        // new unit clicked
        // exemption clicked
        //OtherList_Clicked
        //RB1_Clicked
        //RB1Y and RB1N
    </script>
</asp:Content>
