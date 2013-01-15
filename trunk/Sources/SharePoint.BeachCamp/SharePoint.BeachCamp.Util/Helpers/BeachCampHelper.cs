using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using SharePoint.BeachCamp.Util.Utilities;
using SharePoint.BeachCamp.Util.Extensions;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.Workflow;
using System.Xml;
using System.Globalization;
using Microsoft.SharePoint.Utilities;

namespace SharePoint.BeachCamp.Util.Helpers
{
    public class BeachCampHelper
    {
        public static void SendEmail(SPWeb web, string email, SPListItem item, string url, MailType type)
        {
            try
            {
                /*
                string body = string.Format("Hi there, <br /> Your Beach Camp Reservation is approved. Please paid and enjoy it ...<br /> {0}", url);

                System.Collections.Specialized.StringDictionary headers = new System.Collections.Specialized.StringDictionary();
                headers.Add("to", email);
                //headers.Add("cc","anhtuan0030@gmail.com");
                //headers.Add("bcc","anhtuan0030@gmail.com");
                //headers.Add("from",anhtuan0030@gmail.com);
                headers.Add("subject", "[Beach Camp Reservation] - Payment Notify");
                headers.Add("content-type", "text/html");
                string bodyText = body;
                SPUtility.SendEmail(web, headers, bodyText);
                */
                string subject = "[Beach Camp Reservation] - Payment Notify";
                string htmlBody = string.Format(@"A reservation has been approved with the following informations : <br />
                                                Name : {0} <br />
                                                Date : {1} <br />
                                                Section : {2} <br />
                                                Please paid the reservation. <br />
                                                {3}", item.Title, Convert.ToDateTime(item["EventDate"].ToString()).ToString("dd/MM/yyyy"), item["Location"].ToString(), url);

                if (type == MailType.Cancel)
                {
                    subject = "[Beach Camp Reservation] - Reservation Cancel";
                    htmlBody = string.Format(@"A reservation has been canceled with the following informations : <br />
                                                Name : {0} <br />
                                                Date : {1} <br />
                                                Section : {2} <br />
                                                Please reverse another one. <br />
                                                {3}", item.Title, Convert.ToDateTime(item["EventDate"].ToString()).ToString("dd/MM/yyyy"), item["Location"].ToString(), url);
                }


                SPUtility.SendEmail(web, true, true, email, subject, htmlBody);
            }
            catch (Exception ex)
            {
                Utility.LogError(ex.Message, BeachCampFeatures.BeachCamp);
            }
        }

        public static void StartWorkflow(SPListItem spListItem, string workflowName)
        {
            //var beachCampCalendar = Utility.GetListFromURL("/Lists/BCCalendar", spWeb);
            //var spListItem = beachCampCalendar.GetItemById(listItemId);
            SPWorkflowAssociation wfAssoc = spListItem.ParentList.WorkflowAssociations.GetAssociationByName(workflowName, System.Globalization.CultureInfo.CurrentCulture);
            if (wfAssoc != null)
            {
                spListItem.Web.Site.WorkflowManager.StartWorkflow(spListItem, wfAssoc, wfAssoc.AssociationData, true);
            }
        }

        public static void ChangePermission(SPWeb spWeb, Guid listId, int listItemId, string status)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(spWeb.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb(spWeb.ID))
                        {
                            SPList list = web.Lists[listId];
                            SPListItem item = list.GetItemById(listItemId);

                            item.RemoveAllPermissions();
                            SPUser creator = ((SPFieldUserValue)(item.Fields["Created By"]).GetFieldValue(item["Created By"].ToString())).User;
                            SPUser authenticatedUsers = web.EnsureUser("NT Authority\\Authenticated Users");
                            SPGroup reservationAdminGroup = web.Groups["Beach Camp General Supervisor"];

                            item.SetPermissions(authenticatedUsers, SPRoleType.Reader);
                            item.SetPermissions(reservationAdminGroup, SPRoleType.Contributor);

                            if (status == TaskResult.Draft.ToString() ||
                                status == TaskResult.Rejected.ToString())
                            {
                                item.SetPermissions(creator, SPRoleType.Contributor);
                            }
                            else if (status == TaskResult.Pending.ToString()
                                || status == TaskResult.Approved.ToString())
                            {
                                item.SetPermissions(creator, SPRoleType.Reader);
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Utility.LogError(ex.Message, BeachCampFeatures.BeachCamp);
            }
        }

        public static string GetPrices(Repeater repeaterPrices, SPWeb web)
        {
            string output = string.Empty;
            try
            {
                SPList priceList = Utility.GetListFromURL("/Lists/BCPrices", web);
                SPListItemCollection itemCollections = priceList.GetItems();
                repeaterPrices.DataSource = itemCollections.GetDataTable();
                repeaterPrices.DataBind();
            }
            catch (Exception ex)
            {
                output = ex.Message;
            }

            return output;
        }

        public static string GetPeriod(Guid period, SPWeb web)
        {
            try
            {
                SPList list = Utility.GetListFromURL("/Lists/BCPrices", web);
                SPField field = list.Fields[period];
                if (field != null)
                    return field.Title;
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string GetReservationByDate(SPWeb web, DateTime date)
        {
            string sections = string.Empty;
            try
            {
                SPList beachCampList = Utility.GetListFromURL(Constants.BEACH_CAMP_CALENDAR_LIST_URL, web);
                SPQuery query = new SPQuery();
                string caml = string.Format(@"<Where>
                                                <And>
                                                    <Geq>
                                                        <FieldRef Name='EventDate' />
                                                        <Value Type='DateTime'>{0}</Value>
                                                    </Geq>
                                                    <Leq>
                                                        <FieldRef Name='EventDate' />
                                                        <Value Type='DateTime'>{0}</Value>
                                                    </Leq>
                                                </And>
                                            </Where>", date.ToString("yyyy-MM-dd"));
                query.Query = caml;
                SPListItemCollection listItemCollections = beachCampList.GetItems(query);
                if (listItemCollections != null && listItemCollections.Count > 0)
                {
                    foreach (SPListItem item in listItemCollections)
                    {
                        sections += item["Location"] + "#";
                    }
                }
                return sections.TrimEnd('#');
            }
            catch (Exception ex)
            {
                Utility.LogError(ex.Message, BeachCampFeatures.BeachCamp);
                return string.Empty;
            }
        }

        public static string GetReservationByDate(SPWeb web, DateTime date, int id)
        {
            string sections = string.Empty;
            try
            {
                SPList beachCampList = Utility.GetListFromURL(Constants.BEACH_CAMP_CALENDAR_LIST_URL, web);
                SPQuery query = new SPQuery();
                string caml = string.Format(@"<Where>
                                                <And>
                                                    <Neq>
                                                        <FieldRef Name='ID' />
                                                        <Value Type='Counter'>{0}</Value>
                                                    </Neq>
                                                    <And>
                                                        <Geq>
                                                            <FieldRef Name='EventDate' />
                                                            <Value Type='DateTime'>{1}</Value>
                                                        </Geq>
                                                        <Leq>
                                                            <FieldRef Name='EventDate' />
                                                            <Value Type='DateTime'>{1}</Value>
                                                        </Leq>
                                                    </And>
                                                </And>
                                            </Where>", id, date.ToString("yyyy-MM-dd"));
                query.Query = caml;
                SPListItemCollection listItemCollections = beachCampList.GetItems(query);
                if (listItemCollections != null && listItemCollections.Count > 0)
                {
                    foreach (SPListItem item in listItemCollections)
                    {
                        sections += item["Location"] + "#";
                    }
                }
                return sections.TrimEnd('#');
            }
            catch (Exception ex)
            {
                Utility.LogError(ex.Message, BeachCampFeatures.BeachCamp);
                return string.Empty;
            }
        }

        public static bool IsUserReserved(SPWeb web, string employeeCode, DateTime date)
        {
            try
            {
                if (date < DateTime.Now.AddDays(-1))
                    return true;

                string caml = @"<Where>
                            <And>
                                <Eq>
                                    <FieldRef Name='Author' LookupId='True' />
                                    <Value Type='User'>{0}</Value>
                                </Eq>
                                <And>
                                    <Geq>
                                        <FieldRef Name='EventDate' />
                                        <Value Type='DateTime'>{1}</Value>
                                    </Geq>
                                    <Leq>
                                        <FieldRef Name='EventDate' />
                                        <Value Type='DateTime'>{2}</Value>
                                    </Leq>
                                </And>
                            </And>
                        </Where>";
                bool isReserved = GetItemByCaml(web, string.Format(caml, employeeCode, date.AddDays(-60).ToString("yyyy-MM-dd"), date.AddDays(-1).ToString("yyyy-MM-dd")));
                if (isReserved)
                    return true;

                isReserved = GetItemByCaml(web, string.Format(caml, employeeCode, date.ToString("yyyy-MM-dd"), date.AddDays(60).ToString("yyyy-MM-dd")));
                if (isReserved)
                    return true;

            }
            catch (Exception ex)
            {
                Utility.LogError(ex.Message, BeachCampFeatures.BeachCamp);
            }
            return false;
        }

        public static bool IsUserReserved(SPWeb web, string employeeCode, DateTime date, int id)
        {
            try
            {
                string caml = @"<Where>
                                    <And>
                                        <Neq>
                                            <FieldRef Name='ID' />
                                            <Value Type='Counter'>{0}</Value>
                                        </Neq>
                                        <And>
                                            <Eq>
                                                <FieldRef Name='Author' LookupId='True' />
                                                <Value Type='User'>{1}</Value>
                                            </Eq>
                                            <And>
                                                <Geq>
                                                    <FieldRef Name='EventDate' />
                                                    <Value Type='DateTime'>{2}</Value>
                                                </Geq>
                                                <Leq>
                                                    <FieldRef Name='EventDate' />
                                                    <Value Type='DateTime'>{3}</Value>
                                                </Leq>
                                            </And>
                                        </And>
                                    </And>
                                </Where>";
                bool isReserved = GetItemByCaml(web, string.Format(caml, id, employeeCode, date.AddDays(-60).ToString("yyyy-MM-dd"), date.AddDays(-1).ToString("yyyy-MM-dd")));
                if (isReserved)
                    return true;

                isReserved = GetItemByCaml(web, string.Format(caml, id, employeeCode, date.ToString("yyyy-MM-dd"), date.AddDays(60).ToString("yyyy-MM-dd")));
                if (isReserved)
                    return true;
            }
            catch (Exception ex)
            {
                Utility.LogError(ex.Message, BeachCampFeatures.BeachCamp);
            }
            return false;
        }


        public static bool IsSectionPeriodReserved(SPWeb web, string sectionPeriod, DateTime date, int requiredDay)
        {
            string caml = string.Format(@"<Where>
                                            <Or>
                                                <And>
                                                    <Eq>
                                                        <FieldRef Name='Location' />
                                                        <Value Type='Text'>{0}</Value>
                                                    </Eq>
                                                    <And>
                                                        <Gt>
                                                            <FieldRef Name='EndDate' />
                                                            <Value Type='DateTime'>{1}</Value>
                                                        </Gt>
                                                        <Leq>
                                                            <FieldRef Name='EventDate' />
                                                            <Value Type='DateTime'>{1}</Value>
                                                        </Leq>
                                                    </And>
                                                </And>
                                                <And>
                                                    <Eq>
                                                        <FieldRef Name='Location' />
                                                        <Value Type='Text'>{0}</Value>
                                                    </Eq>
                                                    <And>
                                                        <Gt>
                                                            <FieldRef Name='EndDate' />
                                                            <Value Type='DateTime'>{2}</Value>
                                                        </Gt>
                                                        <Leq>
                                                            <FieldRef Name='EventDate' />
                                                            <Value Type='DateTime'>{2}</Value>
                                                        </Leq>
                                                    </And>
                                                </And>
                                            </Or>
                                        </Where>", sectionPeriod, date.ToString("yyyy-MM-dd"), date.AddDays(requiredDay - 1).ToString("yyyy-MM-dd")); ;

            return GetItemByCaml(web, caml);
        }

        public static bool IsSectionPeriodReserved(SPWeb web, string sectionPeriod, DateTime date)
        {
            string caml = string.Format(@"<Where>
                                            <And>
                                                <Eq>
                                                    <FieldRef Name='Location' />
                                                    <Value Type='Text'>{0}</Value>
                                                </Eq>
                                                <And>
                                                    <Gt>
                                                        <FieldRef Name='EndDate' />
                                                        <Value Type='DateTime'>{1}</Value>
                                                    </Gt>
                                                    <Leq>
                                                        <FieldRef Name='EventDate' />
                                                        <Value Type='DateTime'>{1}</Value>
                                                    </Leq>
                                                </And>
                                            </And>
                                        </Where>
                                    <OrderBy>
                                        <FieldRef Name='EndDate' Ascending='False' />
                                    </OrderBy>", sectionPeriod, date.AddDays(2).ToString("yyyy-MM-dd"));

            string abc = string.Format(@"<Where>
                                            <Or>
                                                <And>
                                                    <Eq>
                                                        <FieldRef Name='Location' />
                                                        <Value Type='Text'>{0}</Value>
                                                    </Eq>
                                                    <And>
                                                        <Gt>
                                                            <FieldRef Name='EndDate' />
                                                            <Value Type='DateTime'>{1}</Value>
                                                        </Gt>
                                                        <Leq>
                                                            <FieldRef Name='EventDate' />
                                                            <Value Type='DateTime'>{1}</Value>
                                                        </Leq>
                                                    </And>
                                                </And>
                                                <And>
                                                    <Eq>
                                                        <FieldRef Name='Location' />
                                                        <Value Type='Text'>{0}</Value>
                                                    </Eq>
                                                    <And>
                                                        <Gt>
                                                            <FieldRef Name='EndDate' />
                                                            <Value Type='DateTime'>{2}</Value>
                                                        </Gt>
                                                        <Leq>
                                                            <FieldRef Name='EventDate' />
                                                            <Value Type='DateTime'>{2}</Value>
                                                        </Leq>
                                                    </And>
                                                </And>
                                            </Or>
                                        </Where>", sectionPeriod, date.ToString("yyyy-MM-dd"), date.AddDays(2).ToString("yyyy-MM-dd")); ;

            //            string caml = string.Format(@"<Where>
            //                                                <Eq>
            //                                                    <FieldRef Name='Location' />
            //                                                    <Value Type='Text'>{0}</Value>
            //                                                </Eq>
            //                                        </Where>
            //                                    <OrderBy>
            //                                        <FieldRef Name='EndDate' Ascending='False' />
            //                                    </OrderBy>", sectionPeriod, date.ToString("yyyy-MM-dd"));

            return GetItemByCaml(web, abc);

        }

        public static bool IsSectionPeriodReserved(SPWeb web, string sectionPeriod, DateTime date, int requiredDay, int id)
        {
            string caml = string.Format(@"<Where>
                                            <Or>
                                                <And>
                                                    <Neq>
                                                        <FieldRef Name='ID' />
                                                        <Value Type='Counter'>{3}</Value>
                                                    </Neq>
                                                    <And>
                                                        <Eq>
                                                            <FieldRef Name='Location' />
                                                            <Value Type='Text'>{0}</Value>
                                                        </Eq>
                                                        <And>
                                                            <Gt>
                                                                <FieldRef Name='EndDate' />
                                                                <Value Type='DateTime'>{1}</Value>
                                                            </Gt>
                                                            <Leq>
                                                                <FieldRef Name='EventDate' />
                                                                <Value Type='DateTime'>{1}</Value>
                                                            </Leq>
                                                        </And>
                                                    </And>
                                                </And>
                                                <And>
                                                    <Neq>
                                                        <FieldRef Name='ID' />
                                                        <Value Type='Counter'>{3}</Value>
                                                    </Neq>
                                                    <And>
                                                        <Eq>
                                                            <FieldRef Name='Location' />
                                                            <Value Type='Text'>{0}</Value>
                                                        </Eq>
                                                        <And>
                                                            <Gt>
                                                                <FieldRef Name='EndDate' />
                                                                <Value Type='DateTime'>{2}</Value>
                                                            </Gt>
                                                            <Leq>
                                                                <FieldRef Name='EventDate' />
                                                                <Value Type='DateTime'>{2}</Value>
                                                            </Leq>
                                                        </And>
                                                    </And>
                                                </And>
                                            </Or>
                                        </Where>", sectionPeriod, date.ToString("yyyy-MM-dd"), date.AddDays(requiredDay - 1).ToString("yyyy-MM-dd"), id);

            //            string caml = string.Format(@"<<Where>
            //                                            <And>
            //                                                <Neq>
            //                                                    <FieldRef Name='ID' />
            //                                                    <Value Type='Counter'>{0}</Value>
            //                                                </Neq>
            //                                                <And>
            //                                                    <Eq>
            //                                                        <FieldRef Name='Location' />
            //                                                        <Value Type='Text'>{0}</Value>
            //                                                    </Eq>
            //                                                    <And>
            //                                                        <Gt>
            //                                                            <FieldRef Name='EndDate' />
            //                                                            <Value Type='DateTime'>{2}</Value>
            //                                                        </Gt>
            //                                                        <Leq>
            //                                                            <FieldRef Name='EndDate' />
            //                                                            <Value Type='DateTime'>{3}</Value>
            //                                                        </Leq>
            //                                                    </And>
            //                                                </And>
            //                                            </And>
            //                                        </Where>", id, sectionPeriod, date.ToString("yyyy-MM-dd"), date.AddDays(requiredDay).ToString("yyyy-MM-dd"));

            return GetItemByCaml(web, caml);
        }

        private static bool GetItemByCaml(SPWeb web, string caml)
        {
            bool output = false;
            try
            {
                SPList beachCampCalendar = Utility.GetListFromURL("/Lists/BCCalendar", web);
                SPQuery spQuery = new SPQuery();
                spQuery.Query = caml;
                SPListItemCollection itemCollections = beachCampCalendar.GetItems(spQuery);
                if (itemCollections != null && itemCollections.Count > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return output;
        }

        #region Overlay Calendar

        public static void AddCalendarOverlay(SPList targetList, string viewName, string owaUrl, string exchangeUrl, string overlayName, string overlayDescription, CalendarOverlayColor color, bool alwaysShow, bool clearExisting)
        {
            AddCalendarOverlay(targetList, viewName, owaUrl, exchangeUrl, null, overlayName, overlayDescription, color, alwaysShow, clearExisting);
        }

        public static void AddCalendarOverlay(SPList targetList, string viewName, SPList overlayList, string overlayName, string overlayDescription, CalendarOverlayColor color, bool alwaysShow, bool clearExisting)
        {
            AddCalendarOverlay(targetList, viewName, null, null, overlayList, overlayName, overlayDescription, color, alwaysShow, clearExisting);
        }

        private static void AddCalendarOverlay(SPList targetList, string viewName, string owaUrl, string exchangeUrl, SPList overlayList, string overlayName, string overlayDescription, CalendarOverlayColor color, bool alwaysShow, bool clearExisting)
        {
            bool sharePoint = overlayList != null;
            string linkUrl = owaUrl;
            if (sharePoint)
                linkUrl = overlayList.DefaultViewUrl;

            SPView targetView = targetList.DefaultView;
            if (!string.IsNullOrEmpty(viewName))
                targetView = targetList.Views[viewName];

            XmlDocument xml = new XmlDocument();
            XmlElement aggregationElement = null;
            string existAggregationElements = string.Empty;
            int count = 0;
            if (string.IsNullOrEmpty(targetView.CalendarSettings) || clearExisting)
            {
                xml.AppendChild(xml.CreateElement("AggregationCalendars"));
                aggregationElement = xml.CreateElement("AggregationCalendar");
                xml.DocumentElement.AppendChild(aggregationElement);
            }
            else
            {
                xml.LoadXml(targetView.CalendarSettings);
                XmlNodeList calendars = xml.SelectNodes("/AggregationCalendars/AggregationCalendar");
                existAggregationElements = xml.SelectSingleNode("/AggregationCalendars").InnerXml;
                if (calendars != null)
                    count = calendars.Count;
                aggregationElement = xml.SelectSingleNode(string.Format("/AggregationCalendars/AggregationCalendar[@CalendarUrl='{0}']", linkUrl)) as XmlElement;
                if (aggregationElement == null)
                {
                    if (count >= 10)
                        throw new SPException(string.Format("10 calendar ovarlays already exist for the calendar {0}.", targetList.RootFolder.ServerRelativeUrl));
                    aggregationElement = xml.CreateElement("AggregationCalendar");
                    xml.DocumentElement.AppendChild(aggregationElement);
                }
            }
            if (!aggregationElement.HasAttribute("Id"))
                aggregationElement.SetAttribute("Id", Guid.NewGuid().ToString("B", CultureInfo.InvariantCulture));

            aggregationElement.SetAttribute("Type", sharePoint ? "SharePoint" : "Exchange");
            aggregationElement.SetAttribute("Name", !string.IsNullOrEmpty(overlayName) ? overlayName : (overlayList == null ? "" : overlayList.Title));
            aggregationElement.SetAttribute("Description", !string.IsNullOrEmpty(overlayDescription) ? overlayDescription : (overlayList == null ? "" : overlayList.Description));
            aggregationElement.SetAttribute("Color", ((int)color).ToString());
            aggregationElement.SetAttribute("AlwaysShow", alwaysShow.ToString());
            aggregationElement.SetAttribute("CalendarUrl", linkUrl);

            XmlElement settingsElement = aggregationElement.SelectSingleNode("./Settings") as XmlElement;
            if (settingsElement == null)
            {
                settingsElement = xml.CreateElement("Settings");
                aggregationElement.AppendChild(settingsElement);
            }
            if (sharePoint)
            {
                settingsElement.SetAttribute("WebUrl", overlayList.ParentWeb.Site.MakeFullUrl(overlayList.ParentWebUrl));
                settingsElement.SetAttribute("ListId", overlayList.ID.ToString("B", CultureInfo.InvariantCulture));
                settingsElement.SetAttribute("ViewId", overlayList.Views[overlayName].ID.ToString("B", CultureInfo.InvariantCulture));
                settingsElement.SetAttribute("ListFormUrl", overlayList.Forms[PAGETYPE.PAGE_DISPLAYFORM].ServerRelativeUrl);
            }
            else
            {
                settingsElement.SetAttribute("ServiceUrl", exchangeUrl);
            }

            if (count > 0)
            {
                XmlNode xmlNode = xml.SelectSingleNode("//AggregationCalendars");
                xmlNode.InnerXml = string.Format("{0}{1}", aggregationElement.OuterXml, existAggregationElements);
                //xml.InnerXml = string.Format("{0}{1}", aggregationElement.OuterXml, existAggregationElements);
            }

            targetView.CalendarSettings = xml.OuterXml;

            targetView.Update();
            /*
            <AggregationCalendars>
                <AggregationCalendar 
                    Id="{cfc22c0b-688e-4555-b1d0-784081a91464}" 
                    Type="SharePoint" 
                    Name="My Overlay Calendar"
                    Description="" 
                    Color="1" 
                    AlwaysShow="True" 
                    CalendarUrl="/Lists/MyOverlayCalendar/calendar.aspx">
                    <Settings 
                        WebUrl="http://demo" 
                        ListId="{4a15e596-674f-4af7-a548-0b01470e8d75}" 
                        ViewId="{594c2916-14e7-4b08-ba36-1126b825bf45}" 
                        ListFormUrl="/Lists/MyOverlayCalendar/DispForm.aspx" />
                </AggregationCalendar>
                <AggregationCalendar 
                    Id="{cfc22c0b-688e-4555-b1d0-784081a91465}" 
                    Type="Exchange" 
                    Name="My Overlay Calendar"
                    Description="" 
                    Color="1" 
                    AlwaysShow="True" 
                    CalendarUrl="<url>">
                    <Settings ServiceUrl="<url>" />
                </AggregationCalendar>
            </AggregationCalendars>
            */
        }
        #endregion Overlay Calendar



    }

}
