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
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference1 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind12 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind11 = new System.Workflow.ComponentModel.ActivityBind();
            this.publishItemActivity1 = new SharePoint.BeachCamp.BeachCampWorkflow.PublishItemActivity();
            this.SetApprovalData = new System.Workflow.Activities.CodeActivity();
            this.ifElseBranchActivity2 = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifElseBranchActivity1 = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifElseActivity1 = new System.Workflow.Activities.IfElseActivity();
            this.UpdateItem = new System.Workflow.Activities.CodeActivity();
            this.GeneralSupervisorApproval = new SharePoint.BeachCamp.BeachCampWorkflow.TaskActivity();
            this.CreateInitialParams = new System.Workflow.Activities.CodeActivity();
            this.onWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
            // 
            // publishItemActivity1
            // 
            activitybind1.Name = "BeachCampWorkflow";
            activitybind1.Path = "workflowProperties";
            activitybind2.Name = "BeachCampWorkflow";
            activitybind2.Path = "publishItemActivity1___ListId";
            activitybind3.Name = "BeachCampWorkflow";
            activitybind3.Path = "publishItemActivity1___ListItem";
            activitybind4.Name = "BeachCampWorkflow";
            activitybind4.Path = "publishItemActivity1_CommentText";
            this.publishItemActivity1.Name = "publishItemActivity1";
            this.publishItemActivity1.Status = Microsoft.SharePoint.SPModerationStatusType.Approved;
            this.publishItemActivity1.SetBinding(SharePoint.BeachCamp.BeachCampWorkflow.CCICoreActivity.@__ActivationPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.publishItemActivity1.SetBinding(SharePoint.BeachCamp.BeachCampWorkflow.PublishItemActivity.CommentTextProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.publishItemActivity1.SetBinding(SharePoint.BeachCamp.BeachCampWorkflow.CCICoreActivity.@__ListIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.publishItemActivity1.SetBinding(SharePoint.BeachCamp.BeachCampWorkflow.CCICoreActivity.@__ListItemProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
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
            this.ifElseBranchActivity1.Activities.Add(this.publishItemActivity1);
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
            activitybind5.Name = "BeachCampWorkflow";
            activitybind5.Path = "GeneralSupervisorApproval_ApproveComments";
            activitybind6.Name = "BeachCampWorkflow";
            activitybind6.Path = "associationData.GeneralSupervisor";
            activitybind7.Name = "BeachCampWorkflow";
            activitybind7.Path = "associationData.Message";
            this.GeneralSupervisorApproval.Name = "GeneralSupervisorApproval";
            this.GeneralSupervisorApproval.TaskContentTypeId = "0x01080100E6FA232BCA3B4B25B9DF4B2E3791D3CC";
            activitybind8.Name = "BeachCampWorkflow";
            activitybind8.Path = "GeneralSupervisorApproval_TaskOutcome";
            activitybind9.Name = "BeachCampWorkflow";
            activitybind9.Path = "associationData.TaskTitle";
            activitybind10.Name = "BeachCampWorkflow";
            activitybind10.Path = "workflowProperties";
            this.GeneralSupervisorApproval.SetBinding(SharePoint.BeachCamp.BeachCampWorkflow.TaskActivity.TaskOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            this.GeneralSupervisorApproval.SetBinding(SharePoint.BeachCamp.BeachCampWorkflow.TaskActivity.AssignedToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            this.GeneralSupervisorApproval.SetBinding(SharePoint.BeachCamp.BeachCampWorkflow.TaskActivity.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            this.GeneralSupervisorApproval.SetBinding(SharePoint.BeachCamp.BeachCampWorkflow.TaskActivity.MessageProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            this.GeneralSupervisorApproval.SetBinding(SharePoint.BeachCamp.BeachCampWorkflow.TaskActivity.TaskTitleProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            this.GeneralSupervisorApproval.SetBinding(SharePoint.BeachCamp.BeachCampWorkflow.TaskActivity.ApproveCommentsProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            // 
            // CreateInitialParams
            // 
            this.CreateInitialParams.Name = "CreateInitialParams";
            this.CreateInitialParams.ExecuteCode += new System.EventHandler(this.CreateInitialParams_ExecuteCode);
            activitybind12.Name = "BeachCampWorkflow";
            activitybind12.Path = "workflowId";
            // 
            // onWorkflowActivated1
            // 
            correlationtoken1.Name = "workflowToken";
            correlationtoken1.OwnerActivityName = "BeachCampWorkflow";
            this.onWorkflowActivated1.CorrelationToken = correlationtoken1;
            this.onWorkflowActivated1.EventName = "OnWorkflowActivated";
            this.onWorkflowActivated1.Name = "onWorkflowActivated1";
            activitybind11.Name = "BeachCampWorkflow";
            activitybind11.Path = "workflowProperties";
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind12)));
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
            // 
            // BeachCampWorkflow
            // 
            this.Activities.Add(this.onWorkflowActivated1);
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

        private PublishItemActivity publishItemActivity1;

        private CodeActivity CreateInitialParams;

        private TaskActivity GeneralSupervisorApproval;

        private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated1;


















    }
}
