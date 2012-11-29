﻿using System;
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
            publishItemActivity1___ListId = this.workflowProperties.List.ID.ToString();
            publishItemActivity1___ListItem = this.workflowProperties.ItemId;
        }

        public String publishItemActivity1___ListId = default(System.String);
        public Int32 publishItemActivity1___ListItem = default(System.Int32);

        private void UpdateItem_ExecuteCode(object sender, EventArgs e)
        {
            using (DisableItemEvent disableItemEvent = new DisableItemEvent())
            {
                var item = workflowProperties.Item;
                item["GSApproval"] = GeneralSupervisorApproval_TaskOutcome;
                item["GSApprovalComment"] = GeneralSupervisorApproval_ApproveComments;
                item.SystemUpdate();   
            }
        }

        public String GeneralSupervisorApproval_ApproveComments = default(System.String);
    }
}