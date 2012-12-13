<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BeachCampTask.aspx.cs"
    Inherits="SharePoint.BeachCamp.Layouts.SharePoint.BeachCamp.BeachCampTask" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <SharePoint:CssRegistration ID="CssRegistration3" Name="/_layouts/1033/styles/Themable/layouts.css"
        runat="server" />
    <SharePoint:CssRegistration ID="CssRegistration2" Name="/_layouts/1033/styles/Themable/corev4.css"
        runat="server" />
    <SharePoint:CssRegistration ID="CssRegistration1" Name="/_layouts/1033/styles/Themable/forms.css"
        runat="server" />

    <%--<script src='/_layouts/1033/jquery-1.8.2.min.js' type='text/javascript'></script>--%>
    <script src="../1033/jquery-1.8.2.min.js" type="text/javascript"></script>

    <style type="text/css">
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
            width: 70px;
            border-right: #f5f5f5 1px solid;
            border-top: #b3c5e1 1px solid;
            border-left: #b3c5e1 1px solid;
            border-bottom: #f5f5f5 1px solid;
            border-collapse: collapse border=1;
        }
        
        .ms-dttimeinput
        {
            display: none;
        }
        
        .tbl-beachcamp-reservation
        {
            display:none;
        }
        
        .tbl-main
        {
            width: 595px;
            border-collapse: collapse;
            border: 1px solid black;
            font: normal 12px Arial, Tahoma, Verdana, Helvetica, sans-serif !important;
            font-size: 12px !important;
            color: #000000 !important;
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

    <script type="text/javascript">
        $(document).ready(function () {
            $("#btnToggleBCR").click(function () {
                $("table.tbl-beachcamp-reservation").slideToggle();
            });
        });
    </script>

</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">

    <input id="btnToggleBCR" style="float:right; margin:8px 10px 8px 0px;" type="button" value="Show/Hide Beach Camp Reservation" />

    <div style="clear:both;" ></div>

    <table class="tbl-main tbl-beachcamp-reservation" id="tblMain" runat="server">
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
                        <td style="width: 20%; font-weight: bold;">
                            Name :
                        </td>
                        <td style="width: 25%;" align="left">
                            <asp:Literal ID="literalEmployeeName" Text="Tran Anh Tuan" runat="server"></asp:Literal>
                            <%--<asp:TextBox ID="txtEmployeeName" runat="server"></asp:TextBox>--%>
                        </td>
                        <td style="width: 20%; font-weight: bold;">
                            ID :
                        </td>
                        <td style="width: 35%;" align="left">
                            <asp:Literal ID="literalEmployeeCode" Text="250692114" runat="server"></asp:Literal>
                            <%--<asp:TextBox ID="txtEmployeeCode" runat="server"></asp:TextBox>--%>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold;">
                            Department :
                        </td>
                        <td>
                            <asp:Literal ID="literalDepartment" Text="Giai Phap" runat="server"></asp:Literal>
                            <%--<asp:TextBox ID="txtDepartment" runat="server"></asp:TextBox>--%>
                        </td>
                        <td style="font-weight: bold;">
                            Section :
                        </td>
                        <td>
                            <asp:Literal ID="literalSection" Text="Section ABCD" runat="server"></asp:Literal>
                            <%--<asp:TextBox ID="txtSection" runat="server"></asp:TextBox>--%>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold;">
                            Office Tel :
                        </td>
                        <td>
                            <asp:Literal ID="literalOfficeTel" Text="(08)-393 284 000" runat="server"></asp:Literal>
                            <%--<asp:TextBox ID="txtOfficeTel" runat="server"></asp:TextBox>--%>
                        </td>
                        <td style="font-weight: bold;">
                            Mobile :
                        </td>
                        <td>
                            <asp:Literal ID="literalMobile" Text="0906 760 486" runat="server"></asp:Literal>
                            <%--<asp:TextBox ID="txtMobile" runat="server"></asp:TextBox>--%>
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
                                for the following reason/s:</b> &nbsp;
                            <asp:Literal ID="literalReason" runat="server"></asp:Literal>
                            <%--<SharePoint:FormField FieldName="Reason" ID="ffReason" runat="server">
                                </SharePoint:FormField>--%>
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
                                    <td style="font-weight: bold;">
                                        Require day:&nbsp;
                                    </td>
                                    <td>
                                        <asp:Literal ID="literalRequireDay" runat="server"></asp:Literal>
                                    </td>
                                    <td style="font-weight: bold;">
                                        &nbsp;On:&nbsp;
                                    </td>
                                    <td>
                                        <asp:Literal ID="literalEventDate" runat="server"></asp:Literal>
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
                                                00:70-02:00 hrs
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
                                                <asp:CheckBox ID="chkPeriod1" runat="server" />
                                            </td>
                                            <td valign="middle" align="center">
                                                <asp:Literal ID="literalPeriod2" runat="server"></asp:Literal>
                                                <asp:CheckBox ID="chkPeriod2" runat="server" />
                                            </td>
                                            <td valign="middle" align="center">
                                                <asp:Literal ID="literalFullDay" runat="server"></asp:Literal>
                                                <asp:CheckBox ID="chkFullDay" runat="server" />
                                            </td>
                                            <td valign="middle" align="center">
                                                <asp:Literal ID="literalRamadan" runat="server"></asp:Literal>
                                                <asp:CheckBox ID="chkRamadan" runat="server" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                            <br />
                            <hr />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            I understand that:<br />
                            <br />
                            1. I will be responsible for cleaning the Beach Camp before leaving.<br />
                            2. I will be responsible for the conduct and behavior of my guests and consequently
                            the general moral of those who might jeopardize the reputation of the company.<br />
                            3. I will be responsible for any damages due to negligence or misuse and the cost
                            of the repair or replacing missing items will be determined by the company and to
                            be deducted from my salary.<br />
                            4. I must submit the camp fees to GS maximum 10 days before the required date.
                            <br />
                            <br />
                            Requestor Signature: ______________________ Date: _________________
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td class="ms-formline">
                <img src="/_layouts/images/blank.gif" width='1' height='1' alt="" />
            </td>
        </tr>
    </table>

    <table class="tbl-main" width="100%" cellspacing="0" cellpadding="0">
        <tr class="tr-main">
            <td class="td-main" >
                <asp:RadioButton GroupName="BeachCampApprove" Checked="true" Font-Bold="true" Text="Accepted and Reservation charges received."
                    runat="server" ID="radApproved" />
                <br />
                <asp:RadioButton GroupName="BeachCampApprove" ID="radReject" Font-Bold="true" Text="Not Accepted for the following reasons."
                    runat="server" />
                <br />
                <br />
                <asp:TextBox runat="server" Enabled="false" TextMode="MultiLine" Width="100%" Rows="15" ID="txtMessage" />
            </td>
        </tr>
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
            <td width="100%" align="right">
                <asp:Button runat="server" ID="btnUpdate" CssClass="ms-ButtonHeightWidth" Text="Update" />
                <asp:Button runat="server" ID="btnCancel" CssClass="ms-ButtonHeightWidth" Text="Cancel" Enabled="true" OnClientClick="window.frameElement.commitPopup();return false;" />
            </td>
        </tr>
    </table>
    
</asp:Content>
<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Approve Beach Camp Reservation
</asp:Content>
<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea"
    runat="server">
    Approve Beach Camp Reservation
</asp:Content>
