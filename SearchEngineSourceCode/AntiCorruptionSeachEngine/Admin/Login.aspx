<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AntiCorruptionSeachEngine.admin.Login" %>
<%--Name: Johnathan Falbo
Date: 16/04/2015
Description: Login page that allows the admin to login--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Admin Login</h2>
    <asp:Label runat="server" ID="errorLabel"></asp:Label>
    <table>
        <tr>
            <th scope="row">Username:</th>
            <td><asp:TextBox runat="server" ID="usernameTextBox" ></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="usernameTextBox" ErrorMessage="Username Required*"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th scope="row">Password:</th>
            <td><asp:TextBox runat="server" ID="passwordTextBox" TextMode="Password" ></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="passwordTextBox" ErrorMessage="Password Required*"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th rowspan="2"><asp:Button runat="server" Text="Submit" UseSubmitBehavior="true" OnClick="Authenticate_Login" /></th>
        </tr>
    </table>
    <asp:Label runat="server" ID="messageLabel1"></asp:Label>
</asp:Content>
