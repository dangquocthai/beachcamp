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
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference1 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            this.SetApprovalData = new System.Workflow.Activities.CodeActivity();
            this.ifElseBranchActivity2 = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifElseBranchActivity1 = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifElseActivity1 = new System.Workflow.Activities.IfElseActivity();
            this.UpdateItem = new System.Workflow.Activities.CodeActivity();
            this.GeneralSupervisorApproval = new SharePoint.BeachCamp.BeachCampWorkflow.TaskActivity();
            this.CreateInitialParams = new System.Workflow.Activities.CodeActivity();
            this.onWorkflowActivated = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
            // 
            // SetApprovalData
            // 
            this.SetApprovalData.Name = "SetApprovalData";
            this.SetApprovalData.ExecuteCode += new System.EventHandler(this.SetApprovalData_ExecuteCode);
            // 
            // ifElseBranchActivity2
            // 
            this.ifElseBranchActivity2.Name = "ifElseBranchActivity2";
            // 
            // ifElseBranchActivity1
            // 
            this.ifElseBranchActivity1.Activities.Add(this.SetApprovalData);
            ruleconditionreference1.ConditionName = "Condition1";
            this.ifElseBranchActivity1.Condition = ruleconditionreference1;
            this.ifElseBranchActivity1.Name = "ifElseBranchActivity1";
            // 
            // ifElseActivity1
            // 
            this.ifElseActivity1.Activities.Add(this.ifElseBranchActivity1);
            this.ifElseActivity1.Activities.Add(this.ifElseBranchActivity2);
            this.ifElseActivity1.Name = "ifElseActivity1";
            // 
            // UpdateItem
            // 
            this.UpdateItem.Name = "UpdateItem";
            this.UpdateItem.ExecuteCode += new System.EventHandler(this.UpdateItem_ExecuteCode);
            // 
            // GeneralSupervisorApproval
            // 
            activitybind1.Name = "BeachCampWorkflow";
            activitybind1.Path = "GeneralSupervisorApproval_ApproveComments";
            activitybind2.Name = "BeachCampWorkflow";
            activitybind2.Path = "associationData.GeneralSupervisor";
            activitybind3.Name = "BeachCampWorkflow";
            activitybind3.Path = "associationData.Message";
            this.GeneralSupervisorApproval.Name = "GeneralSupervisorApproval";
            this.GeneralSupervisorApproval.TaskContentTypeId = "0x01080100E6FA232BCA3B4B25B9DF4B2E3791D3CC";
            activitybind4.Name = "BeachCampWorkflow";
            activitybind4.Path = "GeneralSupervisorApproval_TaskOutcome";
            activitybind5.Name = "BeachCampWorkflow";
            activitybind5.Path = "associationData.TaskTitle";
            activitybind6.Name = "BeachCampWorkflow";
            activitybind6.Path = "workflowProperties";
            this.GeneralSupervisorApproval.SetBinding(SharePoint.BeachCamp.BeachCampWorkflow.TaskActivity.TaskOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.GeneralSupervisorApproval.SetBinding(SharePoint.BeachCamp.BeachCampWorkflow.TaskActivity.AssignedToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.GeneralSupervisorApproval.SetBinding(SharePoint.BeachCamp.BeachCampWorkflow.TaskActivity.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            this.GeneralSupervisorApproval.SetBinding(SharePoint.BeachCamp.BeachCampWorkflow.TaskActivity.MessageProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            this.GeneralSupervisorApproval.SetBinding(SharePoint.BeachCamp.BeachCampWorkflow.TaskActivity.TaskTitleProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.GeneralSupervisorApproval.SetBinding(SharePoint.BeachCamp.BeachCampWorkflow.TaskActivity.ApproveCommentsProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // CreateInitialParams
            // 
            this.CreateInitialParams.Name = "CreateInitialParams";
            this.CreateInitialParams.ExecuteCode += new System.EventHandler(this.CreateInitialParams_ExecuteCode);
            activitybind8.Name = "BeachCampWorkflow";
            activitybind8.Path = "workflowId";
            // 
            // onWorkflowActivated
            // 
            correlationtoken1.Name = "workflowToken";
            correlationtoken1.OwnerActivityName = "BeachCampWorkflow";
            this.onWorkflowActivated.CorrelationToken = correlationtoken1;
            this.onWorkflowActivated.EventName = "OnWorkflowActivated";
            this.onWorkflowActivated.Name = "onWorkflowActivated";
            activitybind7.Name = "BeachCampWorkflow";
            activitybind7.Path = "workflowProperties";
            this.onWorkflowActivated.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.onWorkflowActivated_Invoked);
            this.onWorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            this.onWorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            // 
            // BeachCampWorkflow
            // 
            this.Activities.Add(this.onWorkflowActivated);
            this.Activities.Add(this.CreateInitialParams);
            this.Activities.Add(this.GeneralSupervisorApproval);
            this.Activities.Add(this.UpdateItem);
            this.Activities.Add(this.ifElseActivity1);
            this.Name = "BeachCampWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private CodeActivity UpdateItem;

        private CodeActivity SetApprovalData;

        private IfElseBranchActivity ifElseBranchActivity2;

        private IfElseBranchActivity ifElseBranchActivity1;

        private IfElseActivity ifElseActivity1;

        private CodeActivity CreateInitialParams;

        private TaskActivity GeneralSupervisorApproval;

        private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated;























    }
}
