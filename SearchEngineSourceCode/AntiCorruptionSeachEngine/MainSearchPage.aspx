<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="MainSearchPage.aspx.cs" Inherits="AntiCorruptionSeachEngine.MainSearchPage" %>
<%--Name: Johnathan Falbo
Date: 16/04/2015
Description: Main search page featuring the anti-corruption search engine and the didyouknow banner--%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <blockquote>
        <asp:Literal ID="welcomeMessage" runat="server"></asp:Literal>
    </blockquote>

    <ajax:AutoCompleteExtender ServicePath="MainSearchPage.aspx.cs"
        ServiceMethod="SearchAutoComplete"
        MinimumPrefixLength="2"
        EnableCaching="false"
        CompletionSetCount="10"
        TargetControlID="searchTextBox"
        FirstRowSelected="false"
        Enabled="True"
        ID="AutoCompleteSearch"
        runat="server" />


    <div id="myDiv" style="text-align: center" runat="server">
        <tr>
            <td>
                <h4><strong>Search Word:</strong></h4>
            </td>
            <td>
                <asp:TextBox ID="searchTextBox" Width="391px" runat="server" />
            </td>
        </tr>
        <tr>
            <td style="width: 157px">
                <h4><strong>Industry:</strong></h4>
            </td>
            <td style="text-align: left;" class="auto-style1">
                <asp:DropDownList ID="industryDropDown" runat="server" Width="210px" Style="margin-left: 0px">
                    <asp:ListItem Text="" />
                </asp:DropDownList>
            </td>
            <td style="text-align: left">&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 157px">
                <h4><strong>Country Of Head Office:</strong></h4>
            </td>
            <td style="text-align: left;" class="auto-style1">
                <asp:TextBox ID="headOfficeTextBox" runat="server" Width="210px" />

            </td>
            <td style="text-align: left">&nbsp;</td>
        </tr>
        <tr>
            <td style="height: 30px; width: 157px;">
                <h4><strong>Country Doing Business In:</strong></h4>
            </td>
            <td style="text-align: left;" class="auto-style2">
                <asp:TextBox ID="businessInTextBox" runat="server" Width="210px" />
            </td>
            <td style="height: 30px; text-align: left">&nbsp;</td>
        </tr>
        <br />
        <br />
        <asp:Button ID="searchButton" OnClick="SearchButtonClick" Text="Search" runat="server" />
        <br />
        <br />
    </div>
</asp:Content>
