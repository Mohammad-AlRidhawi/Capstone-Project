<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="WebCrawl.aspx.cs" Inherits="AntiCorruptionSeachEngine.admin.WebCrawl" %>
<%--Name: Johnathan Falbo
Date: 16/04/2015
Description: Web Crawl page that allows the admin to crawl--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Web Crawl</h2>
    <table>
        <tr>
            <th scope="row">Web Crawl:</th>
            <td><asp:TextBox ID="crawlTextBox" runat="server"></asp:TextBox></td>
            <td><asp:Button runat="server" OnClick="CrawlClick" Text="Start" /></td>
            <asp:Label ID="errorText" runat="server"></asp:Label>
        </tr>
    </table>
</asp:Content>
