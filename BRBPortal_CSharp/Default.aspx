<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BRBPortal_CSharp._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .navbar-nav, .navbar-right { display: none; }
        .jumbotron { border-radius: 10px; }
    </style>
    <div class="jumbotron">
        <h1>Berkeley Rent Stabilization Board</h1>
        <p class="lead">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam dignissim purus eu neque porta finibus. Suspendisse consectetur luctus leo in mattis. Maecenas quis commodo mauris. In eu malesuada elit. Ut magna magna, fringilla in vestibulum id, malesuada ut leo. Phasellus fringilla eget lorem pellentesque pulvinar. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Sed hendrerit risus ante, vel malesuada dui finibus nec. Duis euismod facilisis lectus, at bibendum nunc sodales ut. In pulvinar ipsum in urna fringilla, eget scelerisque mi mattis. Nunc tempor facilisis dictum. Proin eget nisi vel nisi interdum aliquam in vel enim.</p>
    </div>
    <div class="row">
        <div class="col-md-6">
            <h2>If you have already registered</h2>
            <p>Phasellus pretium ligula sit amet ullamcorper malesuada. Donec auctor augue eget posuere blandit. Aliquam blandit finibus nisi, id venenatis est. Phasellus id elit sit amet sapien vestibulum iaculis ut a tellus. Aliquam eget nisi enim. Donec ultricies dignissim orci quis volutpat. Phasellus elementum, quam id suscipit ullamcorper, sapien dui imperdiet dolor, tristique tempus tellus elit quis est. Aliquam nec dolor efficitur, tristique odio eu, blandit eros. Nunc facilisis consectetur justo, ac scelerisque leo rutrum at. Ut sollicitudin blandit odio id faucibus. Aliquam orci purus, scelerisque vitae interdum at, pharetra eu risus. Aliquam erat volutpat. In hac habitasse platea dictumst.</p>
            <p><a class="btn btn-lg btn-primary" href="/Account/Login.aspx">Login</a></p>
        </div>
        <div class="col-md-6">
            <h2>Create a new account</h2>
            <p>Maecenas porta nulla viverra feugiat blandit. Sed viverra non ligula ut congue. Nulla vel efficitur nunc, ultrices commodo ipsum. Nullam ac consectetur nisi. Integer quis erat magna. Etiam sed elit ornare, rhoncus risus vel, pretium diam. Nunc hendrerit odio ac turpis posuere, vitae sollicitudin ligula accumsan. Nullam et molestie tortor. Ut in sapien fermentum, dignissim massa quis, laoreet augue. Pellentesque malesuada, ante ut lobortis accumsan, urna ex maximus augue, vitae laoreet leo mi quis libero. Proin facilisis consectetur lectus non ornare. Etiam pulvinar leo quis metus hendrerit, at maximus velit imperdiet.</p>
            <p><a class="btn btn-lg btn-success" href="/Account/Register.aspx">Register</a></p>
        </div>
    </div>
</asp:Content>
