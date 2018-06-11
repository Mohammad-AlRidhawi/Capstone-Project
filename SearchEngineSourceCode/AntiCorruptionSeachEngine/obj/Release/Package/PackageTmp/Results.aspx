<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Results.aspx.cs" Inherits="AntiCorruptionSeachEngine.Results" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <br />
      <blockquote>
        <table>
            <tr>
                <td>Searched For: </td>
                <td><asp:Label ID="searchforLabel" runat="server" /></td>
            </tr>
            <tr>
                <td>Industry:</td> 
                <td><asp:Label ID="industryLabel" Text="" runat="server"/></td>
            </tr>
            <tr>
                <td>Head Office:</td>
                <td><asp:Label ID="officeLabel" Text="" runat="server"/></td>
            </tr>
            <tr>
                <td>Business In:</td> 
                <td><asp:Label ID="businessInLabel" Text="" runat="server"/></td>
            </tr>
        </table>
          <asp:Label ID="notFoundLabel" runat="server" />
      </blockquote>
    <blockquote>
        <asp:Repeater id="results" runat="server">
            <ItemTemplate>
                <asp:HyperLink ID="resultsLink" Font-Size="Large" Text='<%#Eval("title")%>' NavigateUrl='<%#Eval("anchor")%>' runat="server" />
                <br />
                <br />
                <%#Eval("info")%>;
                <br />
                <br />
                <hr />
                <br />
            </ItemTemplate>
        </asp:Repeater>
        </blockquote>
        <div align="center">
            <asp:Button ID="prevButton" OnClick="PrevClick" Text="<< Prev" Visible="false" runat="server" />
            &nbsp&nbsp&nbsp&nbsp
            <asp:Button ID="nextButton" OnClick="NextClick" Text="Next >>" Visible="false" runat="server" />
        </div>
  <%--  <asp:Label ID="Label1" runat="server" />
    <asp:HyperLink ID="resultsLink" runat="server" />
    <br />
    <br />
    <asp:Label ID="descriptionLabel" runat="server" />--%>
</asp:Content>
