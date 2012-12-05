using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using SharePoint.BeachCamp.Util.Extensions;
using SharePoint.BeachCamp.Util.Helpers;
using SharePoint.BeachCamp.Util.Models;
using SharePoint.BeachCamp.Util.Utilities;
using Microsoft.SharePoint.Navigation;

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
        private static void ProvisionWebParts(SPWeb web)
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
