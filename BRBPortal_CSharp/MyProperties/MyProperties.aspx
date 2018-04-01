<%@ Page Title="List of Properties" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyProperties.aspx.cs" Inherits="BRBPortal_CSharp.MyProperties.MyProperties" %>
<%@ MasterType  virtualPath="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <hr />
    <div class="form-horizontal">
        <section id="propertiesForm">
            <div class="form-horizontal">
                <div class="form-group">
                    <p><b>You can can select a property to:</b></p>
                    <p>
                        <span class="glyphicon glyphicon-home" style="color:#507CD1;" aria-hidden="true"></span> Register a unit or property
                        <span class="glyphicon glyphicon-user" style="color:#507CD1;" aria-hidden="true"></span> Register a new tenancy (i.e., file a VR)
                        <span class="glyphicon glyphicon-file" style="color:#507CD1;" aria-hidden="true"></span> Claim an exemption
                    </p>
                    <p>You will be able to pay the registration fees after reviewing your units.</p>
                </div>
                <div class="form-group">
                    <asp:GridView ID="gvProperties" runat="server" AutoGenerateColumns="False" 
                        ForeColor="#333333" GridLines="None" CellPadding="4"
                        AllowPaging="true" PageSize="10" OnPageIndexChanging="gvProperties_PageIndexChanging"
                        onRowCommand="gvProperties_RowCommand">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField>
                               <ItemTemplate>
                                 <asp:HiddenField runat="server" ID="hfPropID" Value='<%#Eval("PropertyID")%>' />
                               </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="PropertyID" DataField="PropertyID" ReadOnly="True" Visible="false"></asp:BoundField>
                            <asp:BoundField HeaderText="Address" DataField="PropertyAddress" SortExpression="PropertyAddress">
                                <HeaderStyle Wrap="False" />
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                </asp:BoundField>
                            <asp:BoundField HeaderText="Current Fee" DataField="CurrentFee" SortExpression="CurrentFee" DataFormatString="{0:c}" ReadOnly="True">
                                <HeaderStyle CssClass="text-right" Wrap="False" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                </asp:BoundField>
                            <asp:BoundField HeaderText="Prior Fee" DataField="PriorFee" SortExpression="PriorFee" DataFormatString="{0:c}" ReadOnly="True">
                                <HeaderStyle CssClass="text-right" Wrap="False" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                </asp:BoundField>
                            <asp:BoundField HeaderText="Current Penalty" DataField="CurrentPenalty" SortExpression="CurrentPenalty" DataFormatString="{0:c}" ReadOnly="True">
                                <HeaderStyle CssClass="text-right" Wrap="False" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                </asp:BoundField>
                            <asp:BoundField HeaderText="Prior Penalty" DataField="PriorPenalty" SortExpression="PriorPenalty" DataFormatString="{0:c}" ReadOnly="True">
                                <HeaderStyle CssClass="text-right" Wrap="False" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                </asp:BoundField>
                            <asp:BoundField HeaderText="Credits" DataField="Credits" SortExpression="Credits" DataFormatString="{0:c}" ReadOnly="True">
                                <HeaderStyle CssClass="text-right" Wrap="False" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                </asp:BoundField>
                            <asp:BoundField HeaderText="Balance" DataField="Balance" SortExpression="Balance" DataFormatString="{0:c}" ReadOnly="True">
                                <HeaderStyle CssClass="text-right" Wrap="False" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                </asp:BoundField>
                            <asp:ButtonField ButtonType="Button" CommandName="Select" Text="Review and Update">
                                <ControlStyle CssClass="btn btn-sm btn-success" />
                            </asp:ButtonField>
                        </Columns>
                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Left" CssClass="grid-pager" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                    </asp:GridView>
                </div>
                <div class="form-group">
                    <asp:Button runat="server" id="btnBack" UseSubmitBehavior="false" PostBackUrl="~/Home" Text="Back" CssClass="btn btn-sm btn-default" ToolTip="Return to the list of Units." TabIndex="-1" />
                    <%--<asp:Button runat="server" id="btnAddToCart" OnClick="AddCart_Click" Text="Add to Cart" CssClass="btn btn-primary" ToolTip="Add this property balance to your cart." />--%>
                </div>
                <div class="well" style="display:inline-block; width:auto;">
                    <p style="font-size: small;">Please contact the Rent Stabilization Board if you wish to: </p>
                    <ul style="font-size: small;">
                        <li>Add or delete a unit or property</li>
                        <li>Change ownership information</li>
                        <li>Change the Manager or agent</li>
                    </ul>
                </div>
            </div>
        </section>
    </div>
    
    <script>
        function checkAll(objRef)
        {
            var gridView = objRef.parentNode.parentNode.parentNode;
            var inputList = gridView.getElementsByTagName("input");

            for (var i = 0; i < inputList.length; i++)
            {
                if (inputList[i].type === "checkbox"  && objRef != inputList[i])
                {
                    inputList[i].checked = objRef.checked;
                }
            }
        }
    </script>     
</asp:Content>
