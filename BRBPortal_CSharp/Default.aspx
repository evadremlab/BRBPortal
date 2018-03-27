<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BRBPortal_CSharp._Default" %>
<%@ MasterType  virtualPath="~/Site.Master"%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .navbar-nav, .navbar-right { display: none; }
        .jumbotron { border-radius: 10px; }
    </style>
    <div class="jumbotron">
        <h1>Berkeley Rent Stabilization Board</h1>
        <p class="lead">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam dignissim purus eu neque porta finibus. Suspendisse consectetur luctus leo in mattis. Maecenas quis commodo mauris. In eu malesuada elit. Ut magna magna, fringilla in vestibulum id, malesuada ut leo..</p>
    </div>
    <div class="row">
        <div class="col-md-6">
            <h3>If you have already registered</h3>
            <p>Phasellus pretium ligula sit amet ullamcorper malesuada. Donec auctor augue eget posuere blandit. Aliquam blandit finibus nisi, id venenatis est. Phasellus id elit sit amet sapien vestibulum iaculis ut a tellus. Aliquam eget nisi enim. Donec ultricies dignissim orci quis volutpat.</p>
            <p><a class="btn btn-lg btn-primary" href="/Account/Login.aspx">Login</a></p>
        </div>
        <div class="col-md-6">
            <h3>Create a new account</h3>
            <p>Maecenas porta nulla viverra feugiat blandit. Sed viverra non ligula ut congue. Nulla vel efficitur nunc, ultrices commodo ipsum. Nullam ac consectetur nisi. Integer quis erat magna. Etiam sed elit ornare, rhoncus risus vel, pretium diam. Nunc hendrerit odio ac turpis posuere, vitae sollicitudin ligula accumsan.</p>
            <p><a class="btn btn-lg btn-success" href="/Account/Register.aspx">Register</a></p>
        </div>
    </div>
</asp:Content>
