<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BeachCampTask.aspx.cs" Inherits="SharePoint.BeachCamp.Layouts.SharePoint.BeachCamp.BeachCampTask" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <SharePoint:CssRegistration ID="CssRegistration3" Name="/_layouts/1033/styles/Themable/layouts.css" runat="server" />
    <SharePoint:CssRegistration ID="CssRegistration2" Name="/_layouts/1033/styles/Themable/corev4.css" runat="server" />
    <SharePoint:CssRegistration ID="CssRegistration1" Name="/_layouts/1033/styles/Themable/forms.css" runat="server" />
    <table width="100%" class="ms-formtable" style="margin-top: 8px;" border="0" cellSpacing="0" cellPadding="0">
        <tr>
            <td colspan="4" style="padding: 0px 0px 2px 0px;"> 
                <asp:RadioButton GroupName="Approve" Text="Accepted and Reservation charges received." runat="server" ID="radApproved" /> <br />

                <asp:RadioButton GroupName="Approve" ID="radReject" Text="Not Accepted for the following reasons." runat="server" /> <br />
                <br />
                <asp:TextBox runat="server" TextMode="MultiLine" Columns="50" Rows="15" ID="txtMessage" />

            </td>
        </tr>
        
        <tr>
             <td nowrap="nowrap" align="right">
                <asp:Button runat="server" ID="btnUpdate" Text="Update" />
                 
                <asp:Button runat="server" ID="btnCancel" Text="Cancel" Enabled="true" OnClientClick="window.frameElement.commitPopup();return false;"/>
            </td>
        </tr>

    </table>
</asp:Content>



<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Application Page
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
My Application Page
</asp:Content>
