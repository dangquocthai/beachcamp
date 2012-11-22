<%@ Control Language="C#" AutoEventWireup="false" %>
<%@ Assembly Name="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"
    Namespace="Microsoft.SharePoint.WebControls" %>
<%@ Register TagPrefix="ApplicationPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"
    Namespace="Microsoft.SharePoint.ApplicationPages.WebControls" %>
<%@ Register TagPrefix="SPHttpUtility" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"
    Namespace="Microsoft.SharePoint.Utilities" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBar" Src="~/_controltemplates/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBarButton" Src="~/_controltemplates/ToolBarButton.ascx" %>

<%@ Register TagPrefix="uc" TagName="BeachCampNewEventControl" Src="~/_controltemplates/SharePoint.BeachCamp/BeachCampNewEvent.ascx" %>
<%@ Register TagPrefix="uc" TagName="BeachCampDispEventControl" Src="~/_controltemplates/SharePoint.BeachCamp/BeachCampDispEvent.ascx" %>


<SharePoint:RenderingTemplate ID="BeachCampDispEventTemplate" runat="server">
  <Template>
        <uc:BeachCampDispEventControl ID="BeachCampDispEventTemplate1" runat="server" />
  </Template>
</SharePoint:RenderingTemplate>

<SharePoint:RenderingTemplate ID="BeachCampNewEventTemplate" runat="server">
  <Template>
        <uc:BeachCampNewEventControl ID="BeachCampNewEventTemplate1" runat="server" />
  </Template>
</SharePoint:RenderingTemplate>
