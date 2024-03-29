﻿<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BeachCampNewEvent.ascx.cs"
    Inherits="SharePoint.BeachCamp.ControlTemplates.SharePoint.BeachCamp.BeachCampNewEvent" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBar" Src="~/_controltemplates/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBarButton" Src="~/_controltemplates/ToolBarButton.ascx" %>
<script src="../../_layouts/1033/jquery-1.8.2.min.js" type="text/javascript"></script>
<link href="../../_layouts/1033/public/css/zebra_datepicker.css" rel="stylesheet"
    type="text/css" />
<link href="../../_layouts/1033/public/css/zebra_datepicker_metallic.css" rel="stylesheet"
    type="text/css" />
<script src="../../_layouts/1033/public/javascript/zebra_datepicker.js" type="text/javascript"></script>
<style type="text/css">
    .ms-dttimeinput
    {
        display: none;
    }
    
    .ms-long
    {
        width: 100%;
    }
    
    .ms-input
    {
        color: #000000;
        background: #ffffff;
        font: normal 12px Arial,Tahoma, Verdana, Helvetica, sans-serif;
        height: 15px;
        width: 85px;
        border-right: #f5f5f5 1px solid;
        border-top: #b3c5e1 1px solid;
        border-left: #b3c5e1 1px solid;
        border-bottom: #f5f5f5 1px solid;
        border-collapse: collapse border=1;
    }
    
    .tbl-main
    {
        width: 100%;
        border-collapse: collapse;
        border: 1px solid black;
        font: normal 12px Arial, Tahoma, Verdana, Helvetica, sans-serif !important;
        font-size: 12px !important;
        color: #000000 !important;
        width: 595px !important;
    }
    
    .tr-main
    {
        border: 1px solid black;
        padding: 10px 5px 10px 5px;
    }
    
    .td-main
    {
        padding: 10px 5px 10px 5px;
    }
    
    .tbl-info
    {
        width: 100%;
    }
    
    .tbl-price
    {
        width: 100%;
        border: 1px solid black;
    }
    
    .row_titlelist
    {
        background: #EEEEEE;
        font-weight: bold;
        padding: 2px 0px 2px 0px;
    }
    
    
    
</style>

<link href="../../_layouts/images/SharePoint.BeachCamp/css/style.css" rel="stylesheet" type="text/css" />
<span id='part1'>
    <%--<SharePoint:InformationBar ID="InformationBar2" runat="server" />--%>
    <%--<div id="listFormToolBarTop">
        <wssuc:ToolBar CssClass="ms-formtoolbar" ID="toolBarTbltop" RightButtonSeparator="&amp;#160;"
            runat="server">
            <Template_RightButtons>
                <SharePoint:NextPageButton ID="NextPageButton1" runat="server" />
                <SharePoint:SaveButton ID="SaveButton1" runat="server" />
                <SharePoint:GoBackButton ID="GoBackButton1" runat="server" />
            </Template_RightButtons>
        </wssuc:ToolBar>
    </div>--%>
    <SharePoint:FormToolBar ID="FormToolBar2" runat="server" />
    <SharePoint:ItemValidationFailedMessage ID="ItemValidationFailedMessage2" runat="server" />
    <%--class="ms-formtable"--%>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <%--<SharePoint:ChangeContentType ID="ChangeContentType1" runat="server"/>--%>
        <SharePoint:FolderFormFields ID="FolderFormFields1" runat="server" />
        <%--<SharePoint:ListFieldIterator ID="ListFieldIterator1" runat="server" />--%>
        <!-- myCustomForm -->
        <asp:Label ID="lblError" Font-Bold="true" ForeColor="Red" Visible="false" runat="server"
            Text=""></asp:Label>
        &nbsp;<br />
        <br />
        
        <table class="tbl-main" id="tblMain" runat="server">
            <tr class="tr-main">
                <td class="td-main">
                    <table width="100%" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 50%;" align="right" valign="middle">
                                <asp:RadioButton ID="rdbPersonal" Checked="true" Font-Bold="true" Font-Size="Larger"
                                    Text="Personal" GroupName="BeachCamp" runat="server" />
                                &nbsp;&nbsp;&nbsp;
                            </td>
                            <td style="width: 50%;" align="left" valign="middle">
                                &nbsp;&nbsp;&nbsp;
                                <asp:RadioButton ID="rdbBusiness" Font-Bold="true" Font-Size="Larger" Text="Business"
                                    GroupName="BeachCamp" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

            <tr class="tr-main">
                <td class="td-main">
                    <table class="tbl-info">
                        <tr>
                            <td style="width: 15%; font-weight: bold;">
                                Name <span title="This is a required field." class="ms-formvalidation">(*)</span>
                                :
                            </td>
                            <td style="width: 35%;" align="left">
                                <%--<asp:Literal ID="literalEmployeeName" Text="Tran Anh Tuan" runat="server"></asp:Literal>--%>
                                <%--<asp:TextBox ID="txtEmployeeName" CssClass="ms-long ms-spellcheck-true" runat="server"></asp:TextBox>--%>
                                <SharePoint:FormField FieldName="Title" ID="ffTitle" runat="server">
                                </SharePoint:FormField>
                            </td>
                            <td style="width: 15%; padding:0px 0px 0px 15px; font-weight: bold;">
                                ID <span title="This is a required field." class="ms-formvalidation">(*)</span>
                                :
                            </td>
                            <td style="width: 35%;" align="left">
                                <%--<asp:Literal ID="literalEmployeeCode" Text="250692114" runat="server"></asp:Literal>--%>
                                <%--<asp:TextBox ID="txtEmployeeCode" CssClass="ms-long ms-spellcheck-true" runat="server"></asp:TextBox>--%>
                                <SharePoint:FormField FieldName="EmployeeCode" ID="ffEmployeeCode" runat="server">
                                </SharePoint:FormField>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold;">
                                Department :
                            </td>
                            <td>
                                <%--<asp:Literal ID="literalDepartment" Text="Giai Phap" runat="server"></asp:Literal>--%>
                                <%--<asp:TextBox ID="txtDepartment" runat="server"></asp:TextBox>--%>
                                <SharePoint:FormField FieldName="Department" ID="ffDepartment" runat="server">
                                </SharePoint:FormField>
                            </td>
                            <td style="padding:0px 0px 0px 15px; font-weight: bold;">
                                Section :
                            </td>
                            <td>
                                <%--<asp:Literal ID="literalSection" Text="Section ABCD" runat="server"></asp:Literal>--%>
                                <%--<asp:TextBox ID="txtSection" runat="server"></asp:TextBox>--%>
                                <SharePoint:FormField FieldName="Section" ID="ffSection" runat="server">
                                </SharePoint:FormField>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold;">
                                Office Tel :
                            </td>
                            <td>
                                <%--<asp:Literal ID="literalOfficeTel" Text="(08)-393 284 000" runat="server"></asp:Literal>--%>
                                <%--<asp:TextBox ID="txtOfficeTel" runat="server"></asp:TextBox>--%>
                                <SharePoint:FormField FieldName="OfficeTel" ID="ffOfficeTel" runat="server">
                                </SharePoint:FormField>
                            </td>
                            <td style="padding:0px 0px 0px 15px; font-weight: bold;">
                                Mobile <span title="This is a required field." class="ms-formvalidation">(*)</span>
                                :
                            </td>
                            <td>
                                <%--<asp:Literal ID="literalMobile" Text="0906 760 486" runat="server"></asp:Literal>--%>
                                <%--<asp:TextBox ID="txtMobile" runat="server"></asp:TextBox>--%>
                                <SharePoint:FormField FieldName="Mobile" ID="ffMobile" runat="server">
                                </SharePoint:FormField>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

            <tr class="tr-main">
                <td class="td-main">
                    <table>
                        <tr>
                            <td>
                                <b>I would like to request the G.S. department to reserve for me the company beach camp
                                    for the following reason/s:</b> &nbsp;<br />
                                <SharePoint:FormField FieldName="Reason" ID="ffReason" runat="server">
                                </SharePoint:FormField>
                                &nbsp;
                                <br />
                                <hr />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <%--<td style="font-weight: bold;">
                                            Require day:&nbsp;
                                        </td>
                                        <td>
                                            <SharePoint:FormField FieldName="RequireDay" ID="ffRequireDay" runat="server">
                                            </SharePoint:FormField>
                                        </td>--%>
                                        <td style="font-weight: bold;">
                                            &nbsp;On<span title="This is a required field." class="ms-formvalidation"> (*)</span>:&nbsp;
                                        </td>
                                        <td>
                                            <%--<SharePoint:FormField FieldName="EventDate" ID="ffEventDate" runat="server">
                                            </SharePoint:FormField>--%>
                                            <asp:TextBox ID="txtEventDate" CssClass="eventDatePicker ms-input" runat="server"></asp:TextBox>
                                            <script type="text/javascript">
                                                $(".eventDatePicker").attr('readonly', true);
                                                $('.eventDatePicker').Zebra_DatePicker({
                                                    format: 'd/m/Y'
                                                    , direction: [1, 60]
                                                    //,disabled_dates: ['* * * *, *']
                                                    // all days, all monts, all years as long                 
                                                    // as the weekday is 0 or 6 (Sunday or Saturday) 
                                                }); 
                                            </script>
                                        </td>
                                    </tr>
                                </table>
                                <table class="tbl-prices" border="1" style="border-collapse: collapse" width="100%"
                                    cellspacing="0" cellpadding="0">
                                    <asp:Repeater ID="repeaterPrices" runat="server">
                                        <HeaderTemplate>
                                            <tr class="row_titlelist">
                                                <td style="padding-top: 3px; padding-bottom: 3px;" width="20%" valign="middle" align="center">
                                                    Section
                                                </td>
                                                <td style="padding-top: 3px; padding-bottom: 3px;" width="20%" valign="middle" align="center">
                                                    1<sup>st</sup> Period
                                                    <br />
                                                    07:00-16:30 hrs
                                                </td>
                                                <td style="padding-top: 3px; padding-bottom: 3px;" width="20%" valign="middle" align="center">
                                                    2<sup>nd</sup> Period
                                                    <br />
                                                    17:30-02:00 hrs
                                                </td>
                                                <td style="padding-top: 3px; padding-bottom: 3px;" width="20%" valign="middle" align="center">
                                                    Full day
                                                    <br />
                                                    07:00-02:00 hrs
                                                </td>
                                                <td style="padding-top: 3px; padding-bottom: 3px;" width="20%" valign="middle" align="center">
                                                    Ramadan
                                                    <br />
                                                    15:00-04:00 hrs
                                                </td>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr class="row1">
                                                <td valign="middle" align="center" class="titlelinks">
                                                    <asp:Literal ID="literalSection" runat="server"></asp:Literal>
                                                </td>
                                                <td valign="middle" align="center" class="textlist">
                                                    <asp:Literal ID="literalPeriod1" runat="server"></asp:Literal>
                                                    <asp:CheckBox ID="chkPeriod1" OnCheckedChanged="SectionPeriod_OnCheckedChanged" AutoPostBack="true"
                                                        runat="server" />
                                                    <%--<asp:RadioButton ID="radPeriod1" OnCheckedChanged="SectionPeriod_OnCheckedChanged" AutoPostBack="true" runat="server" />--%>
                                                </td>
                                                <td valign="middle" align="center">
                                                    <asp:Literal ID="literalPeriod2" runat="server"></asp:Literal>
                                                    <asp:CheckBox ID="chkPeriod2" OnCheckedChanged="SectionPeriod_OnCheckedChanged" AutoPostBack="true"
                                                        runat="server" />
                                                    <%--<asp:RadioButton ID="radPeriod2" OnCheckedChanged="SectionPeriod_OnCheckedChanged" AutoPostBack="true" runat="server" />--%>
                                                </td>
                                                <td valign="middle" align="center">
                                                    <asp:Literal ID="literalFullDay" runat="server"></asp:Literal>
                                                    <asp:CheckBox ID="chkFullDay" OnCheckedChanged="SectionPeriod_OnCheckedChanged" AutoPostBack="true"
                                                        runat="server" />
                                                    <%--<asp:RadioButton ID="radFullDay" OnCheckedChanged="SectionPeriod_OnCheckedChanged" AutoPostBack="true" runat="server" />--%>
                                                </td>
                                                <td valign="middle" align="center">
                                                    <asp:Literal ID="literalRamadan" runat="server"></asp:Literal>
                                                    <asp:CheckBox ID="chkRamadan" OnCheckedChanged="SectionPeriod_OnCheckedChanged" AutoPostBack="true"
                                                        runat="server" />
                                                    <%--<asp:RadioButton ID="radRamadan" OnCheckedChanged="SectionPeriod_OnCheckedChanged" AutoPostBack="true" runat="server" />--%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                                <br />
                                <br />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

            <tr class="tr-main">
                <td class="td-main">
                    
                    <table  width="100%" cellspacing="0" cellpadding="0">
                        <tr>
                            <td colspan="2">
                                <b>I understand that:</b>
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 50%;">
                                <div style="float:left;">
                                    1. I will be responsible for cleaning the Beach Camp before leaving.<br />
                                    2. I will be responsible for the conduct and behavior of my guests and consequently
                                    the general moral of those who might jeopardize the reputation of the company.<br />
                                    3. I will be responsible for any damages due to negligence or misuse and the cost
                                    of the repair or replacing missing items will be determined by the company and to
                                    be deducted from my salary.<br />
                                    4. I must submit the camp fees to GS maximum 10 days before the required date.
                                </div>
                            </td>
                            <td style="width: 50%;">
                                <div style="float:left; font: Arabic Transparent; font-size: 14px !important; direction: rtl; unicode-bidi: bidi-override;">
                                    1. يمنع منعاً باتاً إقامة المحاظرات والندوات الدينة أو حفلات الزفاف.
                                    <br />2. في حالة الشركة للمخيم فإن لها الحق في إلغاء حجز الموظف ومنحه حجز آخر في وقت لاحق بدون أجر.
                                    <br />3. في حالة سؤ استخدام أو أي تلف يلحق بالمخيم أو نواقص تحدث لمعدات المخيم يكون الموظف مسؤلاً مسؤلية تامة عن ذلك وسوف يتم تقييم التلفيات أو النواقص وإستقطاع المبالغ المترتبة على ذلك من الموظف.
                                    <br />4. سوف أكون مسؤلاً مسؤلية تامة عن نظافة المخيم قبل مغادرته.
                                    <br />5. يمنع منعاً باتاً التخين والشيشة في داخل مباني المخيم.
                                </div>
                            </td>
                        </tr>
                    </table>

                    <div style="clear: both;"></div>
                    <br />
                    <br />
                    <asp:CheckBox ID="chkUnderstand" AutoPostBack="true" Font-Bold="true" Text="Read and understood the terms and undertake to comply with it’s" runat="server" />

                </td>
            </tr>

        </table>

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
                        <asp:Button ID="btnSave" Enabled="false" CssClass="ms-ButtonHeightWidth" runat="server" Text="Save" />
                        <asp:Button ID="btnSaveAndSubmit" Visible="false" CssClass="ms-ButtonHeightWidth"
                            runat="server" Text="Save & Submit to GS" />
                        <SharePoint:GoBackButton ID="GoBackButton2" runat="server" />
                    </Template_RightButtons>
                </wssuc:ToolBar>
            </td>
        </tr>
    </table>
</span>
<SharePoint:AttachmentUpload ID="AttachmentUpload1" runat="server" />
