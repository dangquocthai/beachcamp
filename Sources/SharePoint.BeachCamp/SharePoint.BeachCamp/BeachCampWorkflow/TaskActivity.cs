using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Activities;
using System.Workflow.ComponentModel;
using SharePoint.BeachCamp.Util;
using System.ComponentModel;
using Microsoft.SharePoint.Workflow;
using SharePoint.BeachCamp.Util.Extensions;

namespace SharePoint.BeachCamp.BeachCampWorkflow
{
    public class TaskActivity : SequenceActivity
    {
        private Microsoft.SharePoint.WorkflowActions.CreateTaskWithContentType CreateTask;
        private WhileActivity WhileTaskNotComplete;
        private Microsoft.SharePoint.WorkflowActions.OnTaskChanged TaskChanged;
        public Guid CreateTask_TaskId = default(System.Guid);
        public Microsoft.SharePoint.Workflow.SPWorkflowTaskProperties TaskChanged_AfterProperties = new Microsoft.SharePoint.Workflow.SPWorkflowTaskProperties();
        private Microsoft.SharePoint.WorkflowActions.CompleteTask CompleteTask;
        private CodeActivity UpdateData;
        public Microsoft.SharePoint.Workflow.SPWorkflowTaskProperties TaskChanged_BeforeProperties = new Microsoft.SharePoint.Workflow.SPWorkflowTaskProperties();
        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity TaskActivityLog;
        public static DependencyProperty CreateTask_TaskPropertiesProperty = DependencyProperty.Register("CreateTask_TaskProperties", typeof(Microsoft.SharePoint.Workflow.SPWorkflowTaskProperties), typeof(SharePoint.BeachCamp.BeachCampWorkflow.TaskActivity));
        public string TaskActivityLog_HistoryDescription = default(System.String);
        private Microsoft.SharePoint.WorkflowActions.CompleteTask completeTask1;
        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity2;
        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity1;
        private SequenceActivity sequenceActivity1;
        public Microsoft.SharePoint.Workflow.SPWorkflowTaskProperties approvalTaskProperties = new Microsoft.SharePoint.Workflow.SPWorkflowTaskProperties();




        public string TaskTitle
        {
            get { return (string)GetValue(TaskTitleProperty); }
            set { SetValue(TaskTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TaskTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskTitleProperty =
            DependencyProperty.Register("TaskTitle", typeof(string), typeof(TaskActivity));



        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(TaskActivity));


        public string TaskOutcome
        {
            get { return (string)GetValue(TaskOutcomeProperty); }
            set { SetValue(TaskOutcomeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TaskOutcome.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskOutcomeProperty =
            DependencyProperty.Register("TaskOutcome", typeof(string), typeof(TaskActivity));



        public string TaskContentTypeId
        {
            get { return (string)GetValue(TaskContentTypeIdProperty); }
            set { SetValue(TaskContentTypeIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TaskContentTypeId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskContentTypeIdProperty =
            DependencyProperty.Register("TaskContentTypeId", typeof(string), typeof(TaskActivity));


        private void InitializeComponent()
        {
            this.CanModifyActivities = true;
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
            this.logToHistoryListActivity2 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.TaskChanged = new Microsoft.SharePoint.WorkflowActions.OnTaskChanged();
            this.logToHistoryListActivity1 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.sequenceActivity1 = new System.Workflow.Activities.SequenceActivity();
            this.TaskActivityLog = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.UpdateData = new System.Workflow.Activities.CodeActivity();
            this.completeTask1 = new Microsoft.SharePoint.WorkflowActions.CompleteTask();
            this.WhileTaskNotComplete = new System.Workflow.Activities.WhileActivity();
            this.CreateTask = new Microsoft.SharePoint.WorkflowActions.CreateTaskWithContentType();
            // 
            // logToHistoryListActivity2
            // 
            this.logToHistoryListActivity2.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity2.Enabled = false;
            this.logToHistoryListActivity2.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity2.HistoryDescription = "xyyyyyyyyyyyyy";
            this.logToHistoryListActivity2.HistoryOutcome = "yyyyyyyyy";
            this.logToHistoryListActivity2.Name = "logToHistoryListActivity2";
            this.logToHistoryListActivity2.OtherData = "";
            this.logToHistoryListActivity2.UserId = -1;
            // 
            // TaskChanged
            // 
            activitybind1.Name = "TaskActivity";
            activitybind1.Path = "TaskChanged_AfterProperties";
            activitybind2.Name = "TaskActivity";
            activitybind2.Path = "TaskChanged_BeforeProperties";
            correlationtoken1.Name = "TaskToken";
            correlationtoken1.OwnerActivityName = "TaskActivity";
            this.TaskChanged.CorrelationToken = correlationtoken1;
            this.TaskChanged.Executor = null;
            this.TaskChanged.Name = "TaskChanged";
            activitybind3.Name = "TaskActivity";
            activitybind3.Path = "CreateTask_TaskId";
            this.TaskChanged.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.TaskChanged_Invoked);
            this.TaskChanged.SetBinding(Microsoft.SharePoint.WorkflowActions.OnTaskChanged.AfterPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.TaskChanged.SetBinding(Microsoft.SharePoint.WorkflowActions.OnTaskChanged.BeforePropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.TaskChanged.SetBinding(Microsoft.SharePoint.WorkflowActions.OnTaskChanged.TaskIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // logToHistoryListActivity1
            // 
            this.logToHistoryListActivity1.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity1.Enabled = false;
            this.logToHistoryListActivity1.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity1.HistoryDescription = "xxxxxxxxxxxxxxxxxxxxx";
            this.logToHistoryListActivity1.HistoryOutcome = "x";
            this.logToHistoryListActivity1.Name = "logToHistoryListActivity1";
            this.logToHistoryListActivity1.OtherData = "";
            this.logToHistoryListActivity1.UserId = -1;
            // 
            // sequenceActivity1
            // 
            this.sequenceActivity1.Activities.Add(this.logToHistoryListActivity1);
            this.sequenceActivity1.Activities.Add(this.TaskChanged);
            this.sequenceActivity1.Activities.Add(this.logToHistoryListActivity2);
            this.sequenceActivity1.Name = "sequenceActivity1";
            // 
            // TaskActivityLog
            // 
            this.TaskActivityLog.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.TaskActivityLog.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            activitybind4.Name = "TaskActivity";
            activitybind4.Path = "TaskActivityLog_HistoryDescription";
            activitybind5.Name = "TaskActivity";
            activitybind5.Path = "TaskOutcome";
            this.TaskActivityLog.Name = "TaskActivityLog";
            this.TaskActivityLog.OtherData = "";
            this.TaskActivityLog.UserId = -1;
            this.TaskActivityLog.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.TaskActivityLog.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // UpdateData
            // 
            this.UpdateData.Name = "UpdateData";
            this.UpdateData.ExecuteCode += new System.EventHandler(this.UpdateData_ExecuteCode);
            // 
            // completeTask1
            // 
            this.completeTask1.CorrelationToken = correlationtoken1;
            this.completeTask1.Name = "completeTask1";
            activitybind6.Name = "TaskActivity";
            activitybind6.Path = "CreateTask_TaskId";
            activitybind7.Name = "TaskActivity";
            activitybind7.Path = "TaskOutcome";
            this.completeTask1.MethodInvoking += new System.EventHandler(this.CompleteTask_MethodInvoking);
            this.completeTask1.SetBinding(Microsoft.SharePoint.WorkflowActions.CompleteTask.TaskIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            this.completeTask1.SetBinding(Microsoft.SharePoint.WorkflowActions.CompleteTask.TaskOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            // 
            // WhileTaskNotComplete
            // 
            this.WhileTaskNotComplete.Activities.Add(this.sequenceActivity1);
            codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.IsTaskNotCompleted);
            this.WhileTaskNotComplete.Condition = codecondition1;
            this.WhileTaskNotComplete.Name = "WhileTaskNotComplete";
            // 
            // CreateTask
            // 
            activitybind8.Name = "TaskActivity";
            activitybind8.Path = "TaskContentTypeId";
            this.CreateTask.CorrelationToken = correlationtoken1;
            this.CreateTask.ListItemId = -1;
            this.CreateTask.Name = "CreateTask";
            this.CreateTask.SpecialPermissions = null;
            activitybind9.Name = "TaskActivity";
            activitybind9.Path = "CreateTask_TaskId";
            activitybind10.Name = "TaskActivity";
            activitybind10.Path = "approvalTaskProperties";
            this.CreateTask.MethodInvoking += new System.EventHandler(this.CreateTask_MethodInvoking);
            this.CreateTask.SetBinding(Microsoft.SharePoint.WorkflowActions.CreateTaskWithContentType.ContentTypeIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            this.CreateTask.SetBinding(Microsoft.SharePoint.WorkflowActions.CreateTaskWithContentType.TaskIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            this.CreateTask.SetBinding(Microsoft.SharePoint.WorkflowActions.CreateTaskWithContentType.TaskPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            // 
            // TaskActivity
            // 
            this.Activities.Add(this.CreateTask);
            this.Activities.Add(this.WhileTaskNotComplete);
            this.Activities.Add(this.completeTask1);
            this.Activities.Add(this.UpdateData);
            this.Activities.Add(this.TaskActivityLog);
            this.Name = "TaskActivity";
            this.CanModifyActivities = false;

        }
        public TaskActivity()
        {
            InitializeComponent();

        }


        public string AssignedTo
        {
            get { return (string)GetValue(AssignedToProperty); }
            set { SetValue(AssignedToProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AssignedTo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AssignedToProperty =
            DependencyProperty.Register("AssignedTo", typeof(string), typeof(TaskActivity));



        public string ApproveComments
        {
            get { return (string)GetValue(ApproveCommentsProperty); }
            set { SetValue(ApproveCommentsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ApproveComments.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ApproveCommentsProperty =
            DependencyProperty.Register("ApproveComments", typeof(string), typeof(TaskActivity));

        
        private void CreateTask_MethodInvoking(object sender, EventArgs e)
        {
            WorkflowProperties.TaskList.EnsureContentTypeInList(this.TaskContentTypeId);
            CreateTask_TaskId = Guid.NewGuid();
            approvalTaskProperties.AssignedTo = AssignedTo;
            //Add code to populate variable here.
            string emailMessage = string.Format(@"{0} <br />A new reservation has been scheduled with the following informations :<br />
                                                Name : {1} <br /> Date : {2} <br /> Section : {3} <br /><br /> Please approve/ reject the reservation.<br />"
                                                , Message, WorkflowProperties.Item["Title"].ToString(), Convert.ToDateTime(WorkflowProperties.Item["EventDate"].ToString()).ToString("dd/MM/yyyy")
                                                , WorkflowProperties.Item["Location"].ToString());
            approvalTaskProperties.EmailBody = emailMessage;
            approvalTaskProperties.Description = emailMessage;
            approvalTaskProperties.Title = TaskTitle + (string.IsNullOrEmpty(this.WorkflowProperties.Item.Title)? "" : this.WorkflowProperties.Item.Title);
        }

        public SPWorkflowActivationProperties WorkflowProperties
        {
            get { return (SPWorkflowActivationProperties)GetValue(WorkflowPropertiesProperty); }
            set { SetValue(WorkflowPropertiesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WorkflowProperties.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WorkflowPropertiesProperty =
            DependencyProperty.Register("WorkflowProperties", typeof(SPWorkflowActivationProperties), typeof(TaskActivity));



        private bool TaskCompleted = false;
        private void IsTaskNotCompleted(object sender, ConditionalEventArgs e)
        {
            e.Result = !TaskCompleted;
        }

        private void UpdateData_ExecuteCode(object sender, EventArgs e)
        {
            TaskActivityLog_HistoryDescription = "Task was " + TaskOutcome + " by " + approver;
        }

        private string approver = string.Empty;
        private void TaskChanged_Invoked(object sender, ExternalDataEventArgs e)
        {
            var approveStatus = TaskChanged_AfterProperties.ExtendedProperties[Constants.APPROVE_STATUS] as string;
            if (!string.IsNullOrEmpty(approveStatus))
            {
                if (approveStatus == TaskResult.Approved.ToString() || approveStatus == TaskResult.Rejected.ToString())
                {
                    TaskCompleted = true;
                    TaskOutcome = approveStatus;
                }
                ApproveComments = TaskChanged_AfterProperties.ExtendedProperties[Constants.APPROVE_MESSAGE] as string;
            }
            approver = e.Identity;
        }

        private void CompleteTask_MethodInvoking(object sender, EventArgs e)
        {

        }


    }
}
