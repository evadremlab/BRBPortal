<%@ Page Title="Payment Return" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="PmtPosting.aspx.vb" Inherits="BRBPortal.PmtPosting" Async="true" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="form-horizontal" style="width: 809px; height: 680px; padding-left:30px" >
        <h4>Return from Payment Process</h4>

        <div style="padding-left:10px">
            <asp:Label runat="server">Charge Amount:  </asp:Label>
            <asp:Literal runat="server" ID="Literal1"></asp:Literal>
        </div>

        <div style="padding-left:10px">
            <asp:Label runat="server">Confirmation No:  </asp:Label>
            <asp:Literal runat="server" ID="ConfNo"></asp:Literal>
        </div>

        <br />

        <div style="padding-left:10px">
            <asp:Label runat="server">Messages:  </asp:Label>
            <asp:Literal runat="server" ID="MiscMsgs"></asp:Literal>
        </div>

        <br />

        <div class="btn-group" style="padding-left:10px">
            <asp:Button runat="server" ID="btnRtnHome" OnClick="RtnHome_Click" Text="Home" CssClass="btn-default active" 
                ToolTip="Return to Home page." />
        </div>
    </div>

</asp:content>
