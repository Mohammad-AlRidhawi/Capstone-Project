<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AntiCorruptionSeachEngine.admin._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Admin Home</h2>
    <asp:Label runat="server" ID="welcomLabel"></asp:Label>
    <br />
    <a href="EditDidYouKnow.aspx">Did You Know Editor</a>
    <br />
    <a href="WebCrawl.aspx">Web Crawl</a>
</asp:Content>
