<%@ Page Title="My Properties" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="MyProperties.aspx.vb" Inherits="BRBPortal.MyProperties" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%--<h2><%: Title %>.</h2>--%>

    <link rel="stylesheet" type="text/css" href="/Styles/StyleBRB.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/Baseline.Type.css" />

    <script type = "text/javascript">
    function checkAll(objRef)
    {
        var GridView = objRef.parentNode.parentNode.parentNode;
        var inputList = GridView.getElementsByTagName("input");
        for (var i=0;i<inputList.length;i++)
        {
            //Get the Cell To find out ColumnIndex
            var row = inputList[i].parentNode.parentNode;
            if(inputList[i].type == "checkbox"  && objRef != inputList[i])
            {
                if (objRef.checked)
                {
                    //If the header checkbox is checked
                    //check all checkboxes
                    //and highlight all rows
                    //row.style.backgroundColor = "aqua";
                    inputList[i].checked=true;
                }
                else
                {
                    //If the header checkbox is checked
                    //uncheck all checkboxes
                    inputList[i].checked=false;
                }
            }
        }
    }
    </script>     
    
    <div class="form-horizontal" style="width: 809px; height: 650px; padding-left:30px" >
        <div class="form-group" style="height:40px; align-items:center">
            <h4>My Properties</h4>
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
                  
        <p style="font-size: small; padding-left:15px; width:350px">Please contact the Rent Stabilization Board if you wish to </p>
        <ul style="font-size: small; padding-left:35px">
            <li>Add or delete a unit or property</li>
            <li>Change ownership information</li>
            <li>Change the Manager or agent</li>
        </ul>

        <br />
        <div class="form-inline" style="padding-left:8px">
            Select a property
            <%--<br />
            <Label runat="server" style="margin-left:15px; width:300px; padding-left:80px; background-color:aliceblue" >Property Address</Label>--%>
        </div>

        <br />
 
        <asp:GridView ID="gvProperties" runat="server" CssClass="Margin30" AutoGenerateColumns="False" CellPadding="4" 
            ForeColor="#333333" GridLines="None" onRowCommand="UpdatePropClicked" PageSize="10" AllowPaging="true"
            onpageindexchanging="gvProperties_PageIndexChanging" >
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                      <asp:CheckBox ID="checkAll" runat="server" onclick = "checkAll(this);" />
                    </HeaderTemplate>
                   <ItemTemplate>
                     <asp:CheckBox ID="chkProp" runat="server" />
                     <asp:HiddenField runat="server" ID="hfPropID" Value='<%#Eval("PropertyID")%>' />
                   </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:CheckBoxField DataField="chkProp" />--%>
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
                    <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                    <ItemStyle HorizontalAlign="Right" Width="80px" Wrap="False" />
                    </asp:BoundField>
                <asp:ButtonField ButtonType="Button" CommandName="Select" Text="Update" />
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
            <%--<asp:Button runat="server" OnClick="NextBtn_Click" ID="MyPropNext" Text="Next" CssClass="swd-button" 
                ToolTip="Proceed to list of Units."/>
                        &nbsp;&nbsp;&nbsp;&nbsp;--%>
            <asp:Button runat="server" id="btnAddToCart" OnClick="AddCart_Click" Text="Add to Cart" CssClass="btn-default active" 
                ToolTip="Add this property balance to your cart." />
        </div>
    </div>
</asp:Content>
