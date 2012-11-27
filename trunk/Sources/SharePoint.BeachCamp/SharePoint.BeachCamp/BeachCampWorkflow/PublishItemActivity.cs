using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.ComponentModel;

using Microsoft.SharePoint;
using SharePoint.BeachCamp.Util.Extensions;
using System.Workflow.ComponentModel.Compiler;
using Microsoft.SharePoint.Workflow;


namespace SharePoint.BeachCamp.BeachCampWorkflow
{
    public class PublishItemActivity : CCICoreActivity
    {


        public string CommentText
        {
            get { return (string)GetValue(CommentTextProperty); }
            set { SetValue(CommentTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommentText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommentTextProperty =
            DependencyProperty.Register("CommentText", typeof(string), typeof(PublishItemActivity));

        
        public SPModerationStatusType Status
        {
            get { return (SPModerationStatusType)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Status.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", typeof(SPModerationStatusType), typeof(PublishItemActivity));

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPListItem sourceListItem = __ActivationProperties.GetListItem(__ListId, __ListItem);
                
                if (sourceListItem == null) return;

                EnsureVersioningControl(sourceListItem.ParentList);
                try
                {
                    if (sourceListItem.ModerationInformation != null)
                    {
                        //update moderation status
                        sourceListItem.ModerationInformation.Comment = CommentText;
                        sourceListItem.ModerationInformation.Status = Status;
                        sourceListItem.SystemUpdate();
                        this.__ActivationProperties.LogToWorkflowHistory(SPWorkflowHistoryEventType.WorkflowComment, "Set content approval on current item", Status.ToString());
                    }
                }
                catch (Exception e)
                {
                    
                }
                
            });

            return ActivityExecutionStatus.Closed;
            
        }

        private void EnsureVersioningControl(SPList list)
        {
            var temp = list.ParentWeb.AllowUnsafeUpdates;

            try
            {
                list.ParentWeb.AllowUnsafeUpdates = true;

                list.EnableVersioning = true;
                list.EnableModeration = true;

                //enable minor versions for documents
                list.EnableMinorVersions = true;

                //set maximum limits for major/minor versions
                list.MajorVersionLimit = 5;
                list.MajorWithMinorVersionsLimit = 5;

                list.Update();
            }
            catch { }
            finally
            {
                list.ParentWeb.AllowUnsafeUpdates = temp;
            }


        }
    }
}
