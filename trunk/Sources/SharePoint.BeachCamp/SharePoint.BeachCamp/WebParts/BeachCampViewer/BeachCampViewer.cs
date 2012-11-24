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
            this.Controls.Add(new Literal() { Text = "<script src='/_layouts/1033/jquery-1.8.2.min.js' type='text/javascript'></script>" });
            
            var beachCampCalendar = Utility.GetListFromURL("/Lists/BCCalendar", SPContext.Current.Web);
            ListViewWebPart wp = null;
            if (beachCampCalendar != null)
            {
                wp = new ListViewWebPart()
                {
                    ID = "currentMonthBeachCamp",
                    ListId = beachCampCalendar.ID,
                    ViewId = 0,
                    TitleUrl = beachCampCalendar.RootFolder.Url,
                    ViewType = ViewType.Calendar,
                    ViewGuid = beachCampCalendar.Views["Calendar"].ID.ToString(),
                };
                
                //UpdatePanel updatePanel1 = new UpdatePanel();
                //updatePanel1.ID = "updatePanel1";
                //updatePanel1.ContentTemplateContainer.Controls.Add(wp);
                //this.Controls.Add(updatePanel1);

                this.Controls.Add(wp);
                this.Controls.Add(new Literal() { Text = "<br />" });

                wp = new ListViewWebPart()
                {
                    ID = "nextMonthBeachCamp",
                    ListId = beachCampCalendar.ID,
                    ViewId = 1,
                    TitleUrl = beachCampCalendar.RootFolder.Url,
                    ViewType = ViewType.Calendar,                    
                    ViewGuid = beachCampCalendar.Views["Calendar"].ID.ToString(),
                };

                string xml = wp.ListViewXml;

                this.Controls.Add(wp);

                DateTime nextMonth = DateTime.Now.AddMonths(1);

                this.Controls.Add(
                    new Literal() 
                        { 
                            Text = string.Format(@"<script language='javascript' type='text/javascript'>
                                                        $(window).load(function() {0}
                                                            $('#WPQ1_nav_prev_a').parent().hide();
                                                            $('#WPQ1_nav_next_a').parent().hide();
                                                            $('#WPQ2_nav_prev_a').parent().hide();
                                                            $('#WPQ2_nav_next_a').parent().hide();
                                                            MoveToDate('{1}','WPQ2');
                                                        {2});
                                                    </script>", "{", nextMonth.ToString("yyyy-MM-dd"), "}")
                        }
                    );

                //UpdatePanel updatePanel2 = new UpdatePanel();
                //updatePanel2.ID = "updatePanel2";
                //updatePanel2.ContentTemplateContainer.Controls.Add(wp);
                //this.Controls.Add(updatePanel2);
            }
        }
    }
}
