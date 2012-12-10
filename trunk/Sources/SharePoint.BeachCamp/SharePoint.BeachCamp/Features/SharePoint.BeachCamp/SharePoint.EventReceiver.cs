using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using SharePoint.BeachCamp.Util.Extensions;
using SharePoint.BeachCamp.Util.Helpers;
using SharePoint.BeachCamp.Util.Models;
using SharePoint.BeachCamp.Util.Utilities;
using Microsoft.SharePoint.Navigation;
using SharePoint.BeachCamp.Util;
using System.Xml;
using System.Globalization;

namespace SharePoint.BeachCamp.Features.SharePoint.BeachCamp
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("637ced0c-5b29-4ebd-be9a-55bc6fbc6525")]
    public class SharePointEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb web = (SPWeb)properties.Feature.Parent;
            try
            {
                ProvisionWebParts(web);
                AddNavigation(web);
                CreateOverlapCalenday(web);
                EnsureSupervisorGroup(web);
                SetListPermission(web);
            }
            catch (Exception ex)
            {
                Utility.LogError(ex.Message, Util.BeachCampFeatures.BeachCamp);
            }
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWeb web = (SPWeb)properties.Feature.Parent;
            try
            {
                RemoveNavigation(web);
            }
            catch (Exception ex)
            {
                Utility.LogError(ex.Message, Util.BeachCampFeatures.BeachCamp);
            }
        }


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}


        #region Functions

        private bool IsGroupAlreadyExist(SPWeb web, string groupName)
        {
            bool isExist = false;

            try
            {
                SPGroup group = web.SiteGroups[groupName];
                isExist = true;
            }
            catch (SPException)
            {
                isExist = false;
            }
            catch (Exception)
            {
                isExist = false;
            }
            return isExist;
        }

        private void CreateNewGroup(SPWeb web, string groupName, string groupDescription)
        {
            if (string.IsNullOrEmpty(groupName)) return;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    SPUserCollection users = web.AllUsers;
                    SPUser owner = users[web.Author.LoginName];
                    SPMember member = users[web.Author.LoginName];

                    try
                    {
                        //Add the group to the SPWeb web
                        SPGroupCollection groups = web.SiteGroups;
                        groups.Add(groupName, member, owner, groupDescription);

                        //Associate the group with SPWeb
                        web.AssociatedGroups.Add(web.SiteGroups[groupName]);
                        web.Update();
                    }
                    catch { }

                    //Assignment of the roles to the group.
                    SPRoleAssignment assignment = new SPRoleAssignment(web.SiteGroups[groupName]);
                    SPRoleDefinition _role = web.RoleDefinitions.GetByType(SPRoleType.Reader);
                    assignment.RoleDefinitionBindings.Add(_role);
                    web.RoleAssignments.Add(assignment);
                });
            }
            catch
            {
                // Not catch exception because check group exists
            }
        }

        private void CreateCalendarView(SPList list, string viewName)
        {
            System.Collections.Specialized.StringCollection viewFields = list.Views["Calendar"].ViewFields.ToStringCollection();
            string query = string.Format(@"<Where>
                                                <And>
                                                    <DateRangesOverlap>
                                                        <FieldRef Name='EventDate'/>
                                                        <FieldRef Name='EndDate'/>
                                                        <FieldRef Name='RecurrenceID'/>
                                                        <Value Type='DateTime'>
                                                        <Month/>
                                                        </Value>
                                                    </DateRangesOverlap>
                                                    <Eq>
                                                        <FieldRef Name='GSApproval'/>
                                                        <Value Type='Text'>{0}</Value>
                                                    </Eq>
                                                </And>
                                            </Where>", viewName);

            SPView newView = list.Views.Add(viewName, viewFields, query, 0, true, false, SPViewCollection.SPViewType.Calendar, false);

            newView.ViewData = @"<FieldRef Name='Author' Type='CalendarMonthTitle' />
                                <FieldRef Name='AssignedTo' Type='CalendarWeekTitle' /> 
                                <FieldRef Name='AssignedTo' Type='CalendarDayTitle' />
                                <FieldRef Name='Title' Type='CalendarDayLocation' />";
            //newView.Hidden = true;
            newView.Update();
        }

        private void CreateOverlapCalenday(SPWeb web)
        {
            var beachCampCalendar = Utility.GetListFromURL(Constants.BEACH_CAMP_CALENDAR_LIST_URL, web);
            if (beachCampCalendar != null)
            {
                CreateCalendarView(beachCampCalendar, TaskResult.Draft.ToString());
                CreateCalendarView(beachCampCalendar, TaskResult.Pending.ToString());
                CreateCalendarView(beachCampCalendar, TaskResult.Approved.ToString());
                CreateCalendarView(beachCampCalendar, TaskResult.Rejected.ToString());

                SPView calendar = beachCampCalendar.Views["Calendar"];
                SPView draft = beachCampCalendar.Views[TaskResult.Draft.ToString()];
                SPView pending = beachCampCalendar.Views[TaskResult.Pending.ToString()];
                SPView approved = beachCampCalendar.Views[TaskResult.Approved.ToString()];
                SPView rejected = beachCampCalendar.Views[TaskResult.Rejected.ToString()];

                //XmlDocument xmlDocument = new XmlDocument();
                if (string.IsNullOrEmpty(calendar.CalendarSettings))
                {
                    string xmlOverlay = string.Format(@"<AggregationCalendars>
                                                          <AggregationCalendar Id='{0}' Type='SharePoint' Name='Draft' Description='Draft' Color='3' AlwaysShow='True' CalendarUrl='{8}'>
                                                            <Settings WebUrl='{12}' ListId='{13}' ViewId='{4}' ListFormUrl='{14}' />
                                                          </AggregationCalendar>
                                                          <AggregationCalendar Id='{1}' Type='SharePoint' Name='Pending' Description='Pending' Color='2' AlwaysShow='True' CalendarUrl='{9}'>
                                                            <Settings WebUrl='{12}' ListId='{13}' ViewId='{5}' ListFormUrl='{14}' />
                                                          </AggregationCalendar>
                                                          <AggregationCalendar Id='{2}' Type='SharePoint' Name='Approved' Description='Approved' Color='5' AlwaysShow='True' CalendarUrl='{10}'>
                                                            <Settings WebUrl='{12}' ListId='{13}' ViewId='{6}' ListFormUrl='{14}' />
                                                          </AggregationCalendar>
                                                          <AggregationCalendar Id='{3}' Type='SharePoint' Name='Rejected' Description='Rejected' Color='4' AlwaysShow='True' CalendarUrl='{11}'>
                                                            <Settings WebUrl='{12}' ListId='{13}' ViewId='{7}' ListFormUrl='{14}' />
                                                          </AggregationCalendar>
                                                        </AggregationCalendars>", Guid.NewGuid().ToString("B", CultureInfo.InvariantCulture)// Draft ID
                                                                                , Guid.NewGuid().ToString("B", CultureInfo.InvariantCulture)// Pending ID
                                                                                , Guid.NewGuid().ToString("B", CultureInfo.InvariantCulture)// Approved ID
                                                                                , Guid.NewGuid().ToString("B", CultureInfo.InvariantCulture)// Rejceted ID
                                                                                , draft.ID.ToString("B", CultureInfo.InstalledUICulture)// Draft View ID
                                                                                , pending.ID.ToString("B", CultureInfo.InstalledUICulture)// Pending View ID
                                                                                , approved.ID.ToString("B", CultureInfo.InstalledUICulture)// Approved View ID
                                                                                , rejected.ID.ToString("B", CultureInfo.InstalledUICulture)// Rejected View ID
                                                                                , web.ServerRelativeUrl.TrimEnd('/') + "/" + calendar.Url // Draft CalendarUrl
                                                                                , web.ServerRelativeUrl.TrimEnd('/') + "/" + calendar.Url // Pending CalendarUrl
                                                                                , web.ServerRelativeUrl.TrimEnd('/') + "/" + calendar.Url // Approved CalendarUrl
                                                                                , web.ServerRelativeUrl.TrimEnd('/') + "/" + calendar.Url // Rejected CalendarUrl
                                                                                , beachCampCalendar.ParentWeb.Site.MakeFullUrl(beachCampCalendar.ParentWebUrl) //WebUrl
                                                                                , beachCampCalendar.ID.ToString("B", CultureInfo.InvariantCulture) // List ID
                                                                                , beachCampCalendar.Forms[PAGETYPE.PAGE_DISPLAYFORM].ServerRelativeUrl // ListFormUrl
                                                                                );
                    calendar.CalendarSettings = xmlOverlay;
                    calendar.Update();
                }

                //BeachCampHelper.AddCalendarOverlay(beachCampCalendar, "Calendar", beachCampCalendar, TaskResult.Draft.ToString(), TaskResult.Draft.ToString(), CalendarOverlayColor.Pink, true, false);
                //BeachCampHelper.AddCalendarOverlay(beachCampCalendar, "Calendar", beachCampCalendar, TaskResult.Pending.ToString(), TaskResult.Pending.ToString(), CalendarOverlayColor.LightYellow, true, false);
                //BeachCampHelper.AddCalendarOverlay(beachCampCalendar, "Calendar", beachCampCalendar, TaskResult.Approved.ToString(), TaskResult.Approved.ToString(), CalendarOverlayColor.Orange, true, false);
                //BeachCampHelper.AddCalendarOverlay(beachCampCalendar, "Calendar", beachCampCalendar, TaskResult.Rejected.ToString(), TaskResult.Rejected.ToString(), CalendarOverlayColor.LightGreen, true, false);
            }
        }


        private void SettingOverlayCalendar(SPView targetView)
        {
            XmlDocument xml = new XmlDocument();
            XmlElement aggregationElement = null;
            if (string.IsNullOrEmpty(targetView.CalendarSettings))
            {
                xml.AppendChild(xml.CreateElement("AggregationCalendars"));
                aggregationElement = xml.CreateElement("AggregationCalendar");
                xml.DocumentElement.AppendChild(aggregationElement);
            }
            else
            {
                xml.LoadXml(targetView.CalendarSettings);
            }
        }


        private void SetListPermission(SPWeb web)
        {
            var authenticatedUser = web.EnsureUser("NT Authority\\Authenticated Users");
            SPGroup reservationAdminGroup = web.Groups["Beach Camp General Supervisor"];

            var beachCampCalendar = Utility.GetListFromURL(Constants.BEACH_CAMP_CALENDAR_LIST_URL, web);
            if (beachCampCalendar != null && !beachCampCalendar.HasUniqueRoleAssignments)
            {
                beachCampCalendar.BreakRoleInheritance(false);
                beachCampCalendar.SetPermissions(authenticatedUser, SPRoleType.Contributor);
                beachCampCalendar.SetPermissions(reservationAdminGroup, SPRoleType.Contributor);
            }

            var beachCampPrice = Utility.GetListFromURL(Constants.BEACH_CAMP_PRICE_LIST_URL, web);
            if (beachCampPrice != null && !beachCampPrice.HasUniqueRoleAssignments)
            {
                beachCampPrice.BreakRoleInheritance(false);
                beachCampPrice.SetPermissions(authenticatedUser, SPRoleType.Reader);
                beachCampPrice.SetPermissions(reservationAdminGroup, SPRoleType.Contributor);
            }

            var beachCampTask = Utility.GetListFromURL(Constants.BEACH_CAMP_TASK_LIST_URL, web);
            if (beachCampTask != null && !beachCampTask.HasUniqueRoleAssignments)
            {
                beachCampTask.BreakRoleInheritance(false);
                beachCampTask.SetPermissions(reservationAdminGroup, SPRoleType.Contributor);
                beachCampTask.SetPermissions(authenticatedUser, SPRoleType.Reader);
            }
        }

        private void EnsureSupervisorGroup(SPWeb web)
        {
            string reservationAdminGroup = "Beach Camp General Supervisor";
            if (!IsGroupAlreadyExist(web, reservationAdminGroup))
            {
                CreateNewGroup(web, reservationAdminGroup, reservationAdminGroup);
            }
        }

        private void ProvisionWebParts(SPWeb web)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string xml = assembly.GetResourceTextFile("SharePoint.BeachCamp.Webparts.xml");

            var webpartPages = SerializationHelper.DeserializeFromXml<WebpartPageDefinitionCollection>(xml);
            WebPartHelper.ProvisionWebpart(web, webpartPages);
        }

        private void AddNavigation(SPWeb web)
        {
            web.AllowUnsafeUpdates = true;
            if (!web.Navigation.UseShared)
            {
                SPNavigationNodeCollection topNavigationNodes = web.Navigation.TopNavigationBar;

                //You can also edit the Quick Launch the same way  
                //SPNavigationNodeCollection topNavigationNodes = web.Navigation.QuickLaunch;  

                SPNavigationNode objItem = new SPNavigationNode("Beach Camp Reservation", web.ServerRelativeUrl.TrimEnd('/') + "/SitePages/BeachCampReservation.aspx", false);
                topNavigationNodes.AddAsLast(objItem);
                SPNavigationNode objItemChild = new SPNavigationNode("Management Reservation", web.ServerRelativeUrl.TrimEnd('/') + "/Lists/BCCalendar/AllItems.aspx", false);
                objItem.Children.AddAsFirst(objItemChild);
                
                
            }
            web.Update();
            web.AllowUnsafeUpdates = false;   
        }


        private void RemoveNavigation(SPWeb web)
        {
            try
            {
                web.AllowUnsafeUpdates = true;
                if (!web.Navigation.UseShared)
                {
                    SPNavigationNodeCollection topNavigationNodes = web.Navigation.TopNavigationBar;

                    //You can also edit the Quick Launch the same way  
                    //SPNavigationNodeCollection topNavigationNodes = web.Navigation.QuickLaunch;  

                    SPNavigationNode objItem = topNavigationNodes.Navigation.GetNodeByUrl("/sites/beachcamp/SitePages/BeachCampReservation.aspx");
                    //topNavigationNodes.AddAsFirst(objItem);
                    topNavigationNodes.Delete(objItem);
                }
                web.Update();
                web.AllowUnsafeUpdates = false;
            }
            catch
            {
            }
        }

        #endregion Functions
    }
}
