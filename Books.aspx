<%@ Page Title="Books" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Books.aspx.cs"
    Inherits="DigitalLibrary.Books" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container" style="padding:30px;">

        <h1>Digital Library - Book Management</h1>
        <hr />

        <div class="form-group">
            <label>Book Name</label>
            <asp:TextBox ID="txtBookName" runat="server"
                CssClass="form-control" />
        </div>

        <br />

        <div class="form-group">
            <label>Author Name</label>
            <asp:TextBox ID="txtAuthor" runat="server"
                CssClass="form-control" />
        </div>

        <br />

        <div class="form-group">
            <label>Category</label>
            <asp:TextBox ID="txtCategory" runat="server"
                CssClass="form-control" />
        </div>

        <br />

        <asp:Button ID="btnAddBook" runat="server"
            Text="Add Book"
            CssClass="btn btn-primary"
            OnClick="btnAddBook_Click" />

        <br /><br />

        <asp:Label ID="lblMessage" runat="server" />

        <hr />

        <h2>Available Books</h2>

        <asp:GridView ID="gvBooks" runat="server"
            AutoGenerateColumns="true"
            CssClass="table table-bordered table-striped"
            EmptyDataText="No books found.">
        </asp:GridView>

        <br />

        <asp:HyperLink ID="lnkHome" runat="server"
            NavigateUrl="~/Default.aspx">
            Back to Home
        </asp:HyperLink>

    </div>

</asp:Content>