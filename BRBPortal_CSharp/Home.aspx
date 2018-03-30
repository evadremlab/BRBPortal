<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="BRBPortal_CSharp.Home" %>
<%@ MasterType  virtualPath="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .thumbnail {
            background-color: #428bca;
            border-color: #357ebd;
            min-height: 15rem;
            margin-bottom: 1rem;
        }
        .thumbnail:hover {
            cursor: pointer;
            background-color: #3276b1;
            border-color: #285e8e;
        }
        .thumbnail .caption {
            color: #fff;
        }
        .thumbnail .caption h3 {
            text-align: center;
        }
    </style>

    <h2><%: Title %></h2>
    <h4>Select an option:</h4>
    <hr />

    <section id="homeForm">
        <div class="form-horizontal offset-col-md-2 col-md-10">
            <div class="row">
                <div class="col-md-3">
                    <div class="thumbnail btnProfile">
                        <div class="caption">
                            <h3>Manage<br />Account Profile</h3>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="thumbnail btnProperty">
                        <div class="caption">
                            <h3>Manage<br />Property Registration<br />and Pay a Bill</h3>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <script>
        $(document).ready(function () {
            $('.btnProfile').click(function () {
                window.location.href = "/Account/ProfileList.aspx";
            });
            $('.btnProperty').click(function () {
                window.location.href = "/MyProperties/MyProperties.aspx";
            });
        });
    </script>
</asp:Content>