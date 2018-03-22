<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="BRBPortal_CSharp.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .radio tr { padding-bottom: 1rem; }
    </style>
    <h2>Home</h2>
    <section id="homeForm">
        <hr />
        <div class="form-horizontal offset-col-md-2 col-md-10">
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="HomeOption" CssClass="control-label">Select an Option:</asp:Label>
            </div>
            <div class="form-group">
                <div class="radio radiobuttonlist" style="display:inline-block;">
                    <asp:RadioButtonList runat="server" ID="HomeOption" RepeatDirection="Vertical">
                        <asp:ListItem Enabled="true" Text="Manage Account Profile" Value="Profile" Selected="True"></asp:ListItem>
                        <asp:ListItem Enabled="true" Text="Manage Property Registration / Pay a Bill" Value="Properties"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
            <div class="form-group" style="margin-top:2rem;">
                <asp:Button runat="server" OnClick="MngSel_Click" Text="Submit" CssClass="btn btn-primary" autofocus="autofocus" />
            </div>
        </div>
    </section>
</asp:Content>