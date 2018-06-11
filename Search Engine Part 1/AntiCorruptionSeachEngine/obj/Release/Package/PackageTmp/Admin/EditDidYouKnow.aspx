<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="EditDidYouKnow.aspx.cs" Inherits="AntiCorruptionSeachEngine.admin.EditDidYouKnow" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <asp:GridView ID="gridView1" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowUpdating="GridView1_RowUpdating" AutoGenerateColumns="false" AllowPaging="true" runat="server">
        <Columns>
            <asp:CommandField ShowEditButton="True" HeaderText="Edit" />
            <asp:CommandField ShowDeleteButton="True" HeaderText="Delete" />

            <asp:TemplateField HeaderText="ImageUrl">
                <ItemTemplate>
                    <asp:Label ID="lblImageUrl" runat="server" Text='<%# Eval("ImageUrl") %>' />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="editImageUrl" AutoPostBack="true" runat="server"></asp:DropDownList>
                    <%--                   <asp:TextBox ID="editImageUrl" Text='<%# Eval("ImageUrl") %>' runat="server"></asp:TextBox>--%>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="NavigateUrl">
                <ItemTemplate>
                    <asp:Label ID="lblNavigateUrl" runat="server" Text='<%# Eval("NavigateUrl") %>' />
                </ItemTemplate>

                <EditItemTemplate>
                    <asp:TextBox ID="editNavigateUrl" Text='<%# Eval("NavigateUrl") %>' runat="server"></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="AlternateText">
                <ItemTemplate>
                    <asp:Label ID="lblAlternateText" runat="server" Text='<%# Eval("AlternateText") %>' />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="editAlternateText" Text='<%# Eval("AlternateText") %>' runat="server"></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Impressions">
                <ItemTemplate>
                    <asp:Label ID="lblImpressions" runat="server" Text='<%# Eval("Impressions") %>' />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="editImpressions" Text='<%# Eval("Impressions") %>' runat="server"></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Keyword">
                <ItemTemplate>
                    <asp:Label ID="lblKeyword" runat="server" Text='<%# Eval("Keyword") %>' />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="editKeyword" Text='<%# Eval("Keyword") %>' runat="server"></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>

        </Columns>
    </asp:GridView>
    <br />
    <br />
    <%--/Insert add text boxes --%>
    <ul class="form">
        <li class="labelLeft">
            <asp:Label AssociatedControlID="dropDownListImageUrl" Text="ImageUrl: " runat="server"></asp:Label>
        </li>
        <li class="textboxRight">
            <asp:DropDownList ID="dropDownListImageUrl" AutoPostBack="true" runat="server"></asp:DropDownList>
        </li>
        <li class="labelLeft">
            <asp:Label AssociatedControlID="textboxNavigateUrl" Text="NavigateUrl: " runat="server"></asp:Label>
        </li>
        <li class="textboxRight">
            <asp:TextBox ID="textboxNavigateUrl" runat="server"></asp:TextBox>
        </li>
        <li class="labelLeft">
            <asp:Label AssociatedControlID="textboxAlternateText" Text="AlternateText: " runat="server"></asp:Label>
        </li>
        <li class="textboxRight">
            <asp:TextBox ID="textboxAlternateText" runat="server"></asp:TextBox>
        </li>
        <li class="labelLeft">
            <asp:Label AssociatedControlID="textboxImpressions" Text="Impressions: " runat="server"></asp:Label>
        </li>
        <li class="textboxRight">
            <asp:TextBox ID="textboxImpressions" runat="server"></asp:TextBox>
        </li>
        <li class="labelLeft">
            <asp:Label AssociatedControlID="textboxKeyword" Text="Keyword: " runat="server"></asp:Label>
        </li>
        <li class="textboxRight">
            <asp:TextBox ID="textboxKeyword" runat="server"></asp:TextBox>
        </li>
        <asp:Button ID="buttonAddRow" Text="Add Row" OnClick="buttonAddRow_Click" runat="server" />
    </ul>

    <%--Insert fileupload  --%>
    <br />
    <br />
    <br />
    <%--If uploading doesn't work check the Web.config--%>
    <ajax:AjaxFileUpload ID="AjaxFileUpload1" runat="server" ForeColor="#3e3e3e" Font-Bold="true" BackColor="#e6e6e6" AllowedFileTypes="jpg,jpeg,png,gif"
        MaximumNumberOfFiles="10" OnUploadComplete="AjaxFileUpload1_UploadComplete"
        Width="700px" />    

</asp:Content>
