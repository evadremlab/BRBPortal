<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="BRBPortal_CSharp.Contact" %>
<%@ MasterType  virtualPath="~/Site.Master"%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <h4>Berkeley Rent Stabilization Board</h4>
    <hr />
    <address>
        2125 Milvia Street<br />
        Berkeley, CA 94704<br />
        (510) 981-7368
    </address>
    <address>
        <strong>Questions or Comments:</strong>   <a href="mailto:Rent@cityofberkeley.info">Rent@cityofberkeley.info</a>
    </address>
</asp:Content>
