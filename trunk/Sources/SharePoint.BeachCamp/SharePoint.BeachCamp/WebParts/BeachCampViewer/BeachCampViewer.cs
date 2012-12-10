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
using System.Reflection;
using SharePoint.BeachCamp.Util.Extensions;
using System.Collections.Generic;
using System.Globalization;

namespace SharePoint.BeachCamp.WebParts.BeachCampViewer
{
    [ToolboxItemAttribute(false)]
    public class BeachCampViewer : System.Web.UI.WebControls.WebParts.WebPart
    {

        protected XsltListViewWebPart FindListViewWebPart(Control control)
        {
            XsltListViewWebPart listview = null;
            if (control is XsltListViewWebPart)
            {
                listview = control as XsltListViewWebPart;
            }
            else
            {
                foreach (Control child in control.Controls)
                {
                    listview = FindListViewWebPart(child);
                    if (listview != null)
                    {
                        break;
                    }
                }
            }
            return listview;
        }

        protected ListViewWebPart FindListViewWebPartXXX(Control control)
        {
            ListViewWebPart listview = null;
            if (control is ListViewWebPart)
            {
                listview = control as ListViewWebPart;
            }
            else
            {
                foreach (Control child in control.Controls)
                {
                    listview = FindListViewWebPartXXX(child);
                    if (listview != null)
                    {
                        break;
                    }
                }
            }
            return listview;
        }

        protected List<XsltListViewWebPart> FindListViewWebParts(Control control)
        {
            List<XsltListViewWebPart> listViews = new List<XsltListViewWebPart>();
            XsltListViewWebPart listView = null;
            if (control is XsltListViewWebPart)
            {
                listView = control as XsltListViewWebPart;
                listViews.Add(listView);
            }
            else
            {
                foreach (Control child in control.Controls)
                {
                    listView = FindListViewWebPart(child);
                    if (listView != null)
                    {
                        listViews.Add(listView);
                    }
                }
            }
            return listViews;
        }

        private void ConvertXLSTListViewWebPartToListViewWebPart(SPWeb web)
        {
            //Guid ListWebPartTypeId = new Guid("baf5274e-a800-8dc3-96d0-0003d9405663");
            //Guid XslListWebPartTypeId = new Guid("874f5460-71f9-fecc-e894-e7e858d9713e");
            //using (SPLimitedWebPartManager mgr = web.GetLimitedWebPartManager(relativUrl, System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared))
            //{

            //    foreach (WebPart webPart in mgr.WebParts)
            //    {


            //        if (webPart.GetType() == typeof(ListViewWebPart))
            //        {
            //            PropertyInfo viewPropertyInfo = webPart.GetType().GetProperty("View",
            //                                                                          BindingFlags.NonPublic |
            //                                                                          BindingFlags.Instance |
            //                                                                          BindingFlags.GetProperty);

            //            var uncustomizedViewByBaseViewId = (SPView)viewPropertyInfo.GetValue(webPart, null);

            //            if (((uncustomizedViewByBaseViewId != null) && (uncustomizedViewByBaseViewId.Type == "GRID")))
            //            {
            //                //Skipping webpart, view type is GRID
            //                continue;
            //            }

            //            Guid webPartTypeId;
            //            if ((uncustomizedViewByBaseViewId != null) && (uncustomizedViewByBaseViewId.Type != "HTML"))
            //            {
            //                webPartTypeId = ListWebPartTypeId;
            //            }
            //            else if (uncustomizedViewByBaseViewId == null)
            //            {
            //                webPartTypeId = XslListWebPartTypeId;
            //            }
            //            else
            //            {
            //                PropertyInfo listInfo = webPart.GetType().GetProperty("List",
            //                                                                      BindingFlags.NonPublic |
            //                                                                      BindingFlags.Instance);

            //                var list = (SPList)listInfo.GetValue(webPart, null);

            //                MethodInfo method = typeof(SPWebPartManager).GetMethod("UseDataView",
            //                                                                        BindingFlags.NonPublic |
            //                                                                        BindingFlags.Static |
            //                                                                        BindingFlags.InvokeMethod, null,
            //                                                                        new[]
            //                                                                {
            //                                                                    typeof (SPList),
            //                                                                    typeof (SPView)
            //                                                                }, null);

            //                var b = (bool)method.Invoke(null, new object[] { list, uncustomizedViewByBaseViewId });
            //                webPartTypeId = b ? XslListWebPartTypeId : ListWebPartTypeId;
            //            }

            //            if (webPartTypeId == ListWebPartTypeId)
            //            {
            //                //Skipping web part, will not be updated
            //                continue;
            //            }
            //            PropertyInfo propertyInfo = webPart.GetType().GetProperty("NewWebPartTypeId",
            //                                                                      BindingFlags.NonPublic |
            //                                                                      BindingFlags.Instance |
            //                                                                      BindingFlags.SetProperty);
            //            propertyInfo.SetValue(webPart, webPartTypeId, null);
            //            mgr.SaveChanges(webPart);

            //        }
            //        if (webPart is XsltListViewWebPart || webPart is ListViewWebPart)
            //        {


            //            Guid gList = webPart is XsltListViewWebPart
            //                             ? ((XsltListViewWebPart)webPart).ListId
            //                             : ((ListViewWebPart)webPart).ListId;

            //            var gView =
            //                new Guid(webPart is XsltListViewWebPart
            //                             ? ((XsltListViewWebPart)webPart).ViewGuid
            //                             : ((ListViewWebPart)webPart).ViewGuid);

            //            SPList list = web.Lists[gList];
            //            SPView view = list.Views[gView];

            //            view.GetType().InvokeMember("EnsureFullBlownXmlDocument",
            //                                        BindingFlags.NonPublic | BindingFlags.Instance |
            //                                        BindingFlags.InvokeMethod, null, view, null,
            //                                        CultureInfo.CurrentCulture);



            //        }
            //    }
            //}
        }

        protected override void CreateChildControls()
        {
            this.Controls.Add(new Literal() { Text = "<script src='/_layouts/1033/jquery-1.8.2.min.js' type='text/javascript'></script>" });

            var beachCampCalendar = Utility.GetListFromURL("/Lists/BCCalendar", SPContext.Current.Web);

            #region Load calendar settings
            //var calendar = Utility.GetListFromURL("/Lists/BCCalendar", SPContext.Current.Web);
            //System.Xml.XmlDocument xmlDocument = null;
            //SPView calendarView = calendar.Views["Calendar"];
            //SPView beachCampCalendarView = beachCampCalendar.Views["Calendar"];
            //if (!string.IsNullOrEmpty(calendarView.CalendarSettings) && !string.IsNullOrEmpty(beachCampCalendarView.CalendarSettings))
            //{
            //    xmlDocument = new System.Xml.XmlDocument();
            //    xmlDocument.LoadXml(calendarView.CalendarSettings);

            //    xmlDocument = new System.Xml.XmlDocument();
            //    xmlDocument.LoadXml(beachCampCalendarView.CalendarSettings);
            //}
            #endregion Load calendar settings

            ListViewWebPart wp = null;
            if (beachCampCalendar != null)
            {

                #region Add ListViewWebPart
                //wp = new ListViewWebPart()
                //{
                //    ID = "currentMonthBeachCamp",
                //    ListId = beachCampCalendar.ID,
                //    ViewId = 0,
                //    TitleUrl = beachCampCalendar.RootFolder.Url,
                //    ViewType = ViewType.Calendar,
                //    ViewGuid = beachCampCalendar.Views["Calendar"].ID.ToString(),
                //};

                ////UpdatePanel updatePanel1 = new UpdatePanel();
                ////updatePanel1.ID = "updatePanel1";
                ////updatePanel1.ContentTemplateContainer.Controls.Add(wp);
                ////this.Controls.Add(updatePanel1);

                ////wp.GetDesignTimeHtml();
                //this.Controls.Add(wp);
                //this.Controls.Add(new Literal() { Text = "<br />" });

                //wp = new ListViewWebPart()
                //{
                //    ID = "nextMonthBeachCamp",
                //    ListId = beachCampCalendar.ID,
                //    ViewId = 1,
                //    TitleUrl = beachCampCalendar.RootFolder.Url,
                //    ViewType = ViewType.Calendar,
                //    ViewGuid = beachCampCalendar.Views["Calendar"].ID.ToString(),
                //};

                ////wp.GetDesignTimeHtml();
                //this.Controls.Add(wp);

                #endregion Add ListViewWebPart

                #region Change view of XsltListViewWebPart

                //ListViewWebPart listViewWebPart = FindListViewWebPartXXX(this.Page);
                //if (listViewWebPart != null)
                //{
                //    //listViewWebPart.ViewGuid = beachCampCalendar.Views["Calendar"].ID.ToString("B", System.Globalization.CultureInfo.InvariantCulture);		 
                //    //listViewWebPart.XmlDefinition = beachCampCalendar.Views["Calendar"].GetViewXml();
                //    string xmlDefinition = listViewWebPart.ListViewXml;
                //}

                //List<XsltListViewWebPart> listViewWebParts = FindListViewWebParts(this.Page);
                //if (listViewWebParts != null)
                //{
                //    for (int i = 0; i < listViewWebParts.Count; i++)
                //    {
                //        string xmlDefinition = listViewWebParts[i].XmlDefinition;
                //    }
                //}

                #endregion Change view of XsltListViewWebPart

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
                                                            $('td').removeAttr('evtid');
                                                            $('th').removeAttr('evtid');
                                                        {2});
                                                        $(document).ready(function() {0}
                                                            setTimeout(function(){0}
                                                                $('td').removeAttr('evtid');
                                                                $('th').removeAttr('evtid');
                                                            {2},800);
                                                            
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
