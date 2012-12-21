using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using SharePoint.BeachCamp.Util.Helpers;
using SharePoint.BeachCamp.Util.Utilities;
using SharePoint.BeachCamp.Util;
using Microsoft.SharePoint.Workflow;

namespace SharePoint.BeachCamp.ContentTypes
{
    public class BeachCampEventReciver : SPItemEventReceiver
    {
        /// <summary>
        /// An item is being added.
        /// </summary>
        public override void ItemAdding(SPItemEventProperties properties)
        {
            base.ItemAdding(properties);
        }

        /// <summary>
        /// An item was added.
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);

            BeachCampHelper.SendEmail(properties.Web, "anhtuan0030@gmail.com", "http://gooogle.com.vn");

            string status = properties.ListItem["GSApproval"].ToString();
            if (status == TaskResult.Pending.ToString())
                //BeachCampHelper.StartWorkflow(properties.ListItem, "Approve Beach Camp Reservation");
                StartWorkflow(properties);
            else
            {
                using (DisableItemEvent disableItemEvent = new DisableItemEvent())
                {
                    //Set permission for reservation
                    BeachCampHelper.ChangePermission(properties.Web, properties.ListId, properties.ListItemId, TaskResult.Draft.ToString());

                }
            }
        }

        /// <summary>
        /// An item was updating.
        /// </summary>
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            base.ItemUpdating(properties);
        }

        /// <summary>
        /// An item was updated.
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            base.ItemUpdated(properties);
            string status = properties.ListItem["GSApproval"].ToString();
            if (status == TaskResult.Pending.ToString()
                && !IsWorkflowRunning(properties.ListItem))
                StartWorkflow(properties);
            else
            {
                using (DisableItemEvent disableItemEvent = new DisableItemEvent())
                {
                    //Set permission for reservation
                    BeachCampHelper.ChangePermission(properties.Web, properties.ListId, properties.ListItemId, status);
                }
            }
        }

        #region Private Functions
        private static void StartWorkflow(SPItemEventProperties properties)
        {
            SPWorkflowManager spWorkflowManager = properties.ListItem.ParentList.ParentWeb.Site.WorkflowManager;
            SPWorkflowAssociationCollection spWorkflowAssociationCollection = properties.ListItem.ParentList.WorkflowAssociations;
            foreach (SPWorkflowAssociation item in spWorkflowAssociationCollection)
            {
                if (item.BaseId == new Guid("91418941-ddf2-4059-b67a-472d6c5fc48e"))
                {
                    spWorkflowManager.StartWorkflow(properties.ListItem, item, item.AssociationData, true);
                    break;
                }
            }
        }

        private bool WorkflowStatePresent(int wfState, int stateToCheckFor)
        {
            bool statePresent = false;
            if ((wfState & stateToCheckFor) == stateToCheckFor)
                statePresent = true;
            return statePresent;
        }


        private bool IsWorkflowRunning(SPListItem item)
        {
            foreach (SPWorkflow wf in item.Workflows)
            {
                if (WorkflowStatePresent((int)wf.InternalState, (int)SPWorkflowState.Running))
                {
                    return true;
                }
            }
            return false;
        }


        #endregion Private Functions
    }
}
