<%@ Page Title="Digital Library" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DigitalLibrary._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container" style="padding:30px;">
        <h1 class="text-center">Digital Library</h1>
        <p class="text-center">Welcome to Digital Library Management System</p>
        <hr />

        <div class="row">
            <div class="col-md-4">
                <h3>
                  <a href="Books.aspx">Book Management</a>
                </h3>
                <p>Add and manage library books.</p>
            </div>

            <div class="col-md-4">
                <h3><a href="Students.aspx">Student Management</a></h3>
                <p>Manage student details.</p>
            </div>

            <div class="col-md-4">
                <h3><a href="IssueReturn.aspx">Issue / Return Books</a></h3>
                <p>Manage book issue and return details.</p>
            </div>
        </div>
    </div>

</asp:Content>