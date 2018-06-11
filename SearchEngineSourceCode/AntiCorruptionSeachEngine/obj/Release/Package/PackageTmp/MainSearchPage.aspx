<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="MainSearchPage.aspx.cs" Inherits="AntiCorruptionSeachEngine.MainSearchPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <blockquote>
        <asp:Literal ID="welcomeMessage" runat="server"></asp:Literal>

    </blockquote>

    <blockquote>
        <asp:TextBox ID="errorText" border="hidden" color="red" Enabled="false" runat="server" />
    </blockquote>

    <asp:TextBox ID="searchTextBox" Width="391px" runat="server" />

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

    <asp:Button ID="searchButton" OnClick="SearchButtonClick" Text="Search" runat="server" />

    <asp:Button ID="btn" runat="server" Text="Advanced Search" OnClientClick="ToggleDiv('first');return false;" OnClick="AdvancedClick" />
    <div id="myDiv" runat="server">
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
                <h4><b><strong>Country Of Head Office:</strong>:</b></h4>
            </td>
            <td style="text-align: left;" class="auto-style1">
                <asp:textBox ID="headOfficeTextBox" runat="server" Width="210px" />

            </td>
            <td style="text-align: left">&nbsp;</td>
        </tr>
        <tr>
            <td style="height: 30px; width: 157px;">
                <h4><b>Country Doing Business In:</b></h4>
            </td>
            <td style="text-align: left;" class="auto-style2">
                <asp:TextBox ID="businessInTextBox" runat="server" Width="210px" />
            </td>
            <td style="height: 30px; text-align: left">&nbsp;</td>
        </tr>
        <tr>
    </div>
</asp:Content>
