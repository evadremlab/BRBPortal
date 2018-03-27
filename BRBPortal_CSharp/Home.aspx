<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="BRBPortal_CSharp.Home" %>
<%@ MasterType  virtualPath="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .radio tr { padding-bottom: 1rem; }
    </style>
    <h2>Home</h2>
    <section id="homeForm">
        <h4>Select an option:</h4>
        <hr />
        <div class="form-horizontal offset-col-md-2 col-md-10">
            <div class="form-group">
                <asp:LinkButton runat="server" PostBackUrl="~/Account/ProfileList.aspx" CssClass="btn btn-lg btn-primary">
                    <span class="glyphicon glyphicon-cog"></span>&nbsp;&nbsp;Manage Account Profile
                </asp:LinkButton>
            </div>
            <div class="form-group">
                <asp:LinkButton runat="server" PostBackUrl="~/MyProperties/MyProperties.aspx" CssClass="btn btn-lg btn-primary">
                    <span class="glyphicon glyphicon-usd"></span>&nbsp;&nbsp;Manage Property Registration / Pay a Bill
                </asp:LinkButton>
            </div>
        </div>
    </section>
</asp:Content>