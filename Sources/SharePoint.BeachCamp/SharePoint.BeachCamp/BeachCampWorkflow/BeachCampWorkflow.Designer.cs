using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace SharePoint.BeachCamp.BeachCampWorkflow
{
    public sealed partial class BeachCampWorkflow
    {
        #region Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        private void InitializeComponent()
        {
            this.CanModifyActivities = true;
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            this.GeneralSupervisorApproval = new SharePoint.BeachCamp.BeachCampWorkflow.TaskActivity();
            this.CreateInitialParams = new System.Workflow.Activities.CodeActivity();
            this.onWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
            // 
            // GeneralSupervisorApproval
            // 
            activitybind1.Name = "BeachCampWorkflow";
            activitybind1.Path = "GeneralSupervisorApproval_AssignedTo";
            this.GeneralSupervisorApproval.Name = "GeneralSupervisorApproval";
            this.GeneralSupervisorApproval.TaskContentTypeId = "0x01080100E6FA232BCA3B4B25B9DF4B2E3791D3CC";
            activitybind2.Name = "BeachCampWorkflow";
            activitybind2.Path = "GeneralSupervisorApproval_TaskOutcome";
            activitybind3.Name = "BeachCampWorkflow";
            activitybind3.Path = "workflowProperties";
            this.GeneralSupervisorApproval.SetBinding(SharePoint.BeachCamp.BeachCampWorkflow.TaskActivity.TaskOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.GeneralSupervisorApproval.SetBinding(SharePoint.BeachCamp.BeachCampWorkflow.TaskActivity.AssignedToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.GeneralSupervisorApproval.SetBinding(SharePoint.BeachCamp.BeachCampWorkflow.TaskActivity.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // CreateInitialParams
            // 
            this.CreateInitialParams.Name = "CreateInitialParams";
            this.CreateInitialParams.ExecuteCode += new System.EventHandler(this.CreateInitialParams_ExecuteCode);
            activitybind5.Name = "BeachCampWorkflow";
            activitybind5.Path = "workflowId";
            // 
            // onWorkflowActivated1
            // 
            correlationtoken1.Name = "workflowToken";
            correlationtoken1.OwnerActivityName = "BeachCampWorkflow";
            this.onWorkflowActivated1.CorrelationToken = correlationtoken1;
            this.onWorkflowActivated1.EventName = "OnWorkflowActivated";
            this.onWorkflowActivated1.Name = "onWorkflowActivated1";
            activitybind4.Name = "BeachCampWorkflow";
            activitybind4.Path = "workflowProperties";
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // BeachCampWorkflow
            // 
            this.Activities.Add(this.onWorkflowActivated1);
            this.Activities.Add(this.CreateInitialParams);
            this.Activities.Add(this.GeneralSupervisorApproval);
            this.Name = "BeachCampWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private CodeActivity CreateInitialParams;

        private TaskActivity GeneralSupervisorApproval;

        private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated1;






    }
}
