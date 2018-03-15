<%@ Page Title="Contact" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.vb" Inherits="BRBPortal.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server" >
    <%--<h2><%: Title %></h2>--%>
   <%-- <p>Your contact page.</p>--%>

    <br />
     <div class="form-horizontal" style="width: 809px; height: 500px; padding-left:20px" >
    <address style="padding-left:20px">
        2125 Milvia Street<br />
        Berkeley, CA 94704<br />
        <abbr title="Phone">P:</abbr>
        (510) 981-7368
    </address>

    <address style="padding-left:20px">
        <strong>Questions or Comments:&ensp;&ensp;</strong><a href="mailto:Rent@cityofberkeley.info">Rent@cityofberkeley.info</a><br />
        <%--<strong>Marketing:&ensp;&ensp;</strong><a href="mailto:Marketing@example.com">Marketing@example.com</a>--%>
    </address>
    </div>
</asp:Content>
