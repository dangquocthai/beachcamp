using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.WebPartPages;
using SharePoint.BeachCamp.Util.Utilities;

namespace SharePoint.BeachCamp.WebParts.BeachCampViewer
{
    [ToolboxItemAttribute(false)]
    public class BeachCampViewer : System.Web.UI.WebControls.WebParts.WebPart
    {
        protected override void CreateChildControls()
        {
            var beachCampCalendar = Utility.GetListFromURL("/Lists/BCCalendar", SPContext.Current.Web);
            ListViewWebPart wp = null;
            if (beachCampCalendar != null)
            {
                wp = new ListViewWebPart()
                {
                    ListId = beachCampCalendar.ID,
                    ViewId = 0,
                    DetailLink = beachCampCalendar.RootFolder.Url,
                    ViewType = ViewType.Calendar,
                    ViewGuid = beachCampCalendar.Views["Calendar"].ID.ToString(),
                };

                UpdatePanel updatePanel1 = new UpdatePanel();
                updatePanel1.ID = "updatePanel1";
                updatePanel1.ContentTemplateContainer.Controls.Add(wp);
                this.Controls.Add(updatePanel1);

                this.Controls.Add(new Literal() { Text = "<br />" });

                wp = new ListViewWebPart()
                {
                    ListId = beachCampCalendar.ID,
                    ViewId = 1,
                    DetailLink = beachCampCalendar.RootFolder.Url,
                    ViewType = ViewType.Calendar,
                    ViewGuid = beachCampCalendar.Views["Calendar"].ID.ToString(),
                };

                UpdatePanel updatePanel2 = new UpdatePanel();
                updatePanel2.ID = "updatePanel2";
                updatePanel2.ContentTemplateContainer.Controls.Add(wp);
                this.Controls.Add(updatePanel2);
            }
        }
    }
}
