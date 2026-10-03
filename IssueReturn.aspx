<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IssueReturn.aspx.cs" Inherits="DigitalLibrary.IssueReturn" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Issue / Return Books</title>
    <style>
        body { font-family: Arial; margin: 40px; }
        .form-control {
            display: block;
            width: 300px;
            padding: 8px;
            margin: 8px 0 18px;
        }
        .btn {
            padding: 10px 18px;
            background: #087dcc;
            color: white;
            border: none;
            cursor: pointer;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Issue / Return Books</h1>
        <hr />

        <asp:Label runat="server" Text="Student Name" />
        <asp:TextBox ID="txtStudentName" runat="server"
            CssClass="form-control" />

        <asp:Label runat="server" Text="Book Name" />
        <asp:TextBox ID="txtBookName" runat="server"
            CssClass="form-control" />

        <asp:Label runat="server" Text="Action" />
        <asp:DropDownList ID="ddlAction" runat="server"
            CssClass="form-control">
            <asp:ListItem>Issue</asp:ListItem>
            <asp:ListItem>Return</asp:ListItem>
        </asp:DropDownList>

        <asp:Button ID="btnSubmit" runat="server"
            Text="Submit" CssClass="btn"
            OnClick="btnSubmit_Click" />

        <br /><br />
        <asp:Label ID="lblMessage" runat="server" />

        <br /><br />
        <a href="Default.aspx">Back to Home</a>
    </form>
</body>
</html>