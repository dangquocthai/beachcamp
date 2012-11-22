﻿<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BeachCampNewEvent.ascx.cs" Inherits="SharePoint.BeachCamp.ControlTemplates.SharePoint.BeachCamp.BeachCampNewEvent" %>

<%@ Register TagPrefix="wssuc" TagName="ToolBar" Src="~/_controltemplates/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBarButton" Src="~/_controltemplates/ToolBarButton.ascx" %>

<style type="text/css">
    .ms-dttimeinput
    {
        display:none;
    }
</style>

<span id='part1'>
    <SharePoint:InformationBar ID="InformationBar2" runat="server" />
    <div id="listFormToolBarTop">
        <wssuc:ToolBar CssClass="ms-formtoolbar" ID="toolBarTbltop" RightButtonSeparator="&amp;#160;"
            runat="server">
            <Template_RightButtons>
                <SharePoint:NextPageButton ID="NextPageButton1" runat="server" />
                <SharePoint:SaveButton ID="SaveButton1" runat="server" />
                <SharePoint:GoBackButton ID="GoBackButton1" runat="server" />
            </Template_RightButtons>
        </wssuc:ToolBar>
    </div>
    <SharePoint:FormToolBar ID="FormToolBar2" runat="server" />
    <SharePoint:ItemValidationFailedMessage ID="ItemValidationFailedMessage2" runat="server" />
    <table class="ms-formtable" style="margin-top: 8px;" border="0" cellpadding="0" cellspacing="0"
        width="100%">
        <%--<SharePoint:ChangeContentType ID="ChangeContentType1" runat="server"/>--%>
        <SharePoint:FolderFormFields ID="FolderFormFields1" runat="server" />
        <%--<SharePoint:ListFieldIterator ID="ListFieldIterator1" runat="server" />--%>
        <!-- myCustomForm -->
        <thead>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblError" Visible="false" runat="server" ForeColor="Red" Text=""></asp:Label>
                </td>
            </tr>
        </thead>
        <tbody id="tbodyMain" runat="server">
            <tr>
                <td width="190" class="ms-formlabel" nowrap="nowrap" valign="top">
                    <h3 class="ms-standardheader">
                        <nobr><SharePoint:FieldLabel runat="server" ID="FieldLabel1" FieldName="TypeOfBeachCamp" /></nobr>
                    </h3>
                </td>
                <td class="ms-formbody" valign="top">
                    <SharePoint:FormField FieldName="TypeOfBeachCamp" ID="ffTypeOfBeachCamp" runat="server"></SharePoint:FormField>
                </td>
            </tr>

            <tr>
                <td width="190" class="ms-formlabel" nowrap="nowrap" valign="top">
                    <h3 class="ms-standardheader">
                        <nobr><SharePoint:FieldLabel runat="server" ID="FieldLabel2" FieldName="Title" /></nobr>
                    </h3>
                </td>
                <td class="ms-formbody" valign="top">
                    <SharePoint:FormField FieldName="Title" ID="ffTitle" runat="server"></SharePoint:FormField>
                </td>
            </tr>

            <tr>
                <td width="190" class="ms-formlabel" nowrap="nowrap" valign="top">
                    <h3 class="ms-standardheader">
                        <nobr><SharePoint:FieldLabel runat="server" ID="FieldLabel3" FieldName="EmployeeCode" /></nobr>
                    </h3>
                </td>
                <td class="ms-formbody" valign="top">
                    <SharePoint:FormField FieldName="EmployeeCode" ID="ffEmployeeCode" runat="server"></SharePoint:FormField>
                </td>
            </tr>

            <tr>
                <td width="190" class="ms-formlabel" nowrap="nowrap" valign="top">
                    <h3 class="ms-standardheader">
                        <nobr><SharePoint:FieldLabel runat="server" ID="FieldLabel4" FieldName="Department" /></nobr>
                    </h3>
                </td>
                <td class="ms-formbody" valign="top">
                    <SharePoint:FormField FieldName="Department" ID="ffDepartment" runat="server"></SharePoint:FormField>
                </td>
            </tr>

            <tr>
                <td width="190" class="ms-formlabel" nowrap="nowrap" valign="top">
                    <h3 class="ms-standardheader">
                        <nobr><SharePoint:FieldLabel runat="server" ID="FieldLabel5" FieldName="OfficeTel" /></nobr>
                    </h3>
                </td>
                <td class="ms-formbody" valign="top">
                    <SharePoint:FormField FieldName="OfficeTel" ID="ffOfficeTel" runat="server"></SharePoint:FormField>
                </td>
            </tr>

            <tr>
                <td width="190" class="ms-formlabel" nowrap="nowrap" valign="top">
                    <h3 class="ms-standardheader">
                        <nobr><SharePoint:FieldLabel runat="server" ID="FieldLabel6" FieldName="Mobile" /></nobr>
                    </h3>
                </td>
                <td class="ms-formbody" valign="top">
                    <SharePoint:FormField FieldName="Mobile" ID="ffMobile" runat="server"></SharePoint:FormField>
                </td>
            </tr>

            <tr>
                <td width="190" class="ms-formlabel" nowrap="nowrap" valign="top">
                    <h3 class="ms-standardheader">
                        <nobr><SharePoint:FieldLabel runat="server" ID="FieldLabel7" FieldName="Reason" /></nobr>
                    </h3>
                </td>
                <td class="ms-formbody" valign="top">
                    <SharePoint:FormField FieldName="Reason" ID="ffReason" runat="server"></SharePoint:FormField>
                </td>
            </tr>

            <tr>
                <td width="190" class="ms-formlabel" nowrap="nowrap" valign="top">
                    <h3 class="ms-standardheader">
                        <nobr><SharePoint:FieldLabel runat="server" ID="FieldLabel8" FieldName="EventDate" /></nobr>
                    </h3>
                </td>
                <td class="ms-formbody" valign="top">
                    <SharePoint:FormField FieldName="EventDate" ID="ffEventDate" runat="server"></SharePoint:FormField>
                </td>
            </tr>

            <tr>
                <td width="190" class="ms-formlabel" nowrap="nowrap" valign="top">
                    <h3 class="ms-standardheader">
                        <nobr><SharePoint:FieldLabel runat="server" ID="FieldLabel9" FieldName="Section" /></nobr>
                    </h3>
                </td>
                <td class="ms-formbody" valign="top">
                    <SharePoint:FormField FieldName="Section" ID="ffSection" runat="server"></SharePoint:FormField>
                </td>
            </tr>

            <tr>
                <td width="190" class="ms-formlabel" nowrap="nowrap" valign="top">
                    <h3 class="ms-standardheader">
                        <nobr><SharePoint:FieldLabel runat="server" ID="FieldLabel10" FieldName="Period" /></nobr>
                    </h3>
                </td>
                <td class="ms-formbody" valign="top">
                    <SharePoint:FormField FieldName="Period" ID="ffPeriod" runat="server"></SharePoint:FormField>
                </td>
            </tr>

        </tbody>
        <!-- myCustomForm -->
        <SharePoint:ApprovalStatus ID="ApprovalStatus2" runat="server" />
        <SharePoint:FormComponent ID="FormComponent2" TemplateName="AttachmentRows" runat="server" />
    </table>
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td class="ms-formline">
                <img src="/_layouts/images/blank.gif" width='1' height='1' alt="" />
            </td>
        </tr>
    </table>
    <table cellpadding="0" cellspacing="0" width="100%" style="padding-top: 7px">
        <tr>
            <td width="100%">
                <SharePoint:ItemHiddenVersion ID="ItemHiddenVersion2" runat="server" />
                <SharePoint:ParentInformationField ID="ParentInformationField2" runat="server" />
                <SharePoint:InitContentType ID="InitContentType2" runat="server" />
                <wssuc:ToolBar CssClass="ms-formtoolbar" ID="toolBar1" RightButtonSeparator="&amp;#160;"
                    runat="server">
                    <Template_Buttons>
                        <SharePoint:CreatedModifiedInfo ID="CreatedModifiedInfo2" runat="server" />
                    </Template_Buttons>
                    <Template_RightButtons>
                        <%--<SharePoint:SaveButton ID="SaveButton2" runat="server" />--%>
                        <asp:Button ID="btnSave" CssClass="ms-ButtonHeightWidth" runat="server" Text="Save" />
                        <SharePoint:GoBackButton ID="GoBackButton2" runat="server" />
                    </Template_RightButtons>
                </wssuc:ToolBar>
            </td>
        </tr>
    </table>
</span>
<SharePoint:AttachmentUpload ID="AttachmentUpload1" runat="server" />