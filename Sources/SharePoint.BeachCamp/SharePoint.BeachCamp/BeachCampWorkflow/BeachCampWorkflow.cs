using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;
using SharePoint.BeachCamp.Util.Helpers;
using SharePoint.BeachCamp.Util;
using SharePoint.BeachCamp.Util.Utilities;
using System.Globalization;

namespace SharePoint.BeachCamp.BeachCampWorkflow
{
    public sealed partial class BeachCampWorkflow : SequentialWorkflowActivity
    {
        public BeachCampWorkflow()
        {
            InitializeComponent();
        }

        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
        public String GeneralSupervisorApproval_TaskOutcome = default(System.String);
        public String GeneralSupervisorApproval_AssignedTo = default(System.String);
        public BCWorkflowAssociationData associationData;
        private void CreateInitialParams_ExecuteCode(object sender, EventArgs e)
        {


            associationData = SerializationHelper.DeserializeFromXml<BCWorkflowAssociationData>(workflowProperties.AssociationData);

        }

        public static DependencyProperty publishItemActivity1_CommentTextProperty = DependencyProperty.Register("publishItemActivity1_CommentText", typeof(System.String), typeof(SharePoint.BeachCamp.BeachCampWorkflow.BeachCampWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public String publishItemActivity1_CommentText
        {
            get
            {
                return ((string)(base.GetValue(SharePoint.BeachCamp.BeachCampWorkflow.BeachCampWorkflow.publishItemActivity1_CommentTextProperty)));
            }
            set
            {
                base.SetValue(SharePoint.BeachCamp.BeachCampWorkflow.BeachCampWorkflow.publishItemActivity1_CommentTextProperty, value);
            }
        }

        private void SetApprovalData_ExecuteCode(object sender, EventArgs e)
        {
            //publishItemActivity1___ListId = this.workflowProperties.List.ID.ToString();
            //publishItemActivity1___ListItem = this.workflowProperties.ItemId;
        }

        public String publishItemActivity1___ListId = default(System.String);
        public Int32 publishItemActivity1___ListItem = default(System.Int32);

        private void UpdateItem_ExecuteCode(object sender, EventArgs e)
        {
            //var item = workflowProperties.Item;
            //item["GSApproval"] = GeneralSupervisorApproval_TaskOutcome == null ? TaskResult.Pending.ToString() : GeneralSupervisorApproval_TaskOutcome;
            //item["GSApprovalComment"] = GeneralSupervisorApproval_ApproveComments;
            ////item.SystemUpdate();
            //item.Update();
        }

        public String GeneralSupervisorApproval_ApproveComments = default(System.String);

        private void onWorkflowActivated_Invoked(object sender, ExternalDataEventArgs e)
        {
            var item = workflowProperties.Item;
            item["GSApproval"] = TaskResult.Pending.ToString();
            item.SystemUpdate();
            //item.Update();
        }

        private void OnItemDeleted_Invoked(object sender, ExternalDataEventArgs e)
        {
            //delete uncompleted tasks when 
            //an item is deleted
            SPWorkflow workflowInstance =
                workflowProperties.Workflow;
            SPWorkflowTaskCollection taskCollection =
                GetWorkflowTasks(workflowInstance);
            for (int i = taskCollection.Count; i > 0; i--)
            {
                SPWorkflowTask task =
                    taskCollection[i - 1];
                using (SPWeb web =
                    workflowProperties.Web)
                {
                    //if (task[SPBuiltInFieldId.TaskStatus]
                    //    .ToString() != SPResource.GetString
                    //    (new CultureInfo((int)web.Language, false),
                    //    "WorkflowTaskStatusComplete", new object[0]))
                    {
                        task.Delete();
                    }
                }
            }
        }



        /// <summary>
        /// Reads the workflow tasks. This method 
        /// is implemented because the Tasks property
        /// of the SPWorkflow instance takes a 
        /// while to be populated.
        /// </summary>
        public static SPWorkflowTaskCollection
            GetWorkflowTasks(SPWorkflow workflowInstance)
        {
            SPWorkflowTaskCollection taskCollection = null;
            bool tasksPopulated = false;
            while (!tasksPopulated)
            {
                try
                {
                    taskCollection = workflowInstance.Tasks;
                    tasksPopulated = true;
                }
                catch { }
            }

            return taskCollection;
        }
        private bool endProcess = false;
        private void EndOfLogicProcess(object sender, ConditionalEventArgs e)
        {
            e.Result = endProcess;
        }

        private void FinishProcess_ExecuteCode(object sender, EventArgs e)
        {
            endProcess = true;
        }
    }

}
