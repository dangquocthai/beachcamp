using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using SharePoint.BeachCamp.Util;
using System.Collections;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.Utilities;
using SharePoint.BeachCamp.Util.Utilities;

namespace SharePoint.BeachCamp.Layouts.SharePoint.BeachCamp
{
    public partial class BeachCampTask : LayoutsPageBase
    {
        protected SPList CurrentTaskList
        {
            get
            {
                if (SPContext.Current.List != null)
                    return SPContext.Current.List;
                return null;
            }
        }

        protected SPListItem CurrentTaskItem
        {
            get
            {
                if (SPContext.Current.ListItem != null)
                    return SPContext.Current.ListItem;
                return null;
            }
        }

        protected SPListItem CurrentWorkflowItem
        {
            get
            {
                try
                {
                    if (CurrentTaskItem == null) return null;
                    string fileUrl = (string)CurrentTaskItem[SPBuiltInFieldId.WorkflowLink];
                    fileUrl = fileUrl.Split(',')[0];
                    return Utility.GetItemByDocumentUrl(fileUrl);
                }
                catch { return null; }
            }
        }

        protected Hashtable CurrentTaskExtendedProperties
        {
            get
            {
                if (CurrentTaskItem != null)
                    return SPWorkflowTask.GetExtendedPropertiesAsHashtable(CurrentTaskItem);
                return null;
            }
        }

        protected SPContentType CurrentTaskContentType
        {
            get
            {
                try
                {
                    if (SPContext.Current.ListItem != null)
                        return SPContext.Current.ListItem.ContentType;
                    else if (!string.IsNullOrEmpty(Request.QueryString["ContentTypeId"]))
                        return SPContext.Current.List.ContentTypes[new SPContentTypeId(Request.QueryString["ContentTypeId"])];

                    return SPContext.Current.List.ContentTypes[0];
                }
                catch { return null; }
            }
        }


        protected override void OnInit(EventArgs e)
        {
            btnUpdate.Click += new EventHandler(btnUpdate_Click);
            
            btnCancel.Click += new EventHandler(btnCancel_Click);
            base.OnInit(e);
        }

        void btnUpdate_Click(object sender, EventArgs e)
        {
            Hashtable properties = CurrentTaskExtendedProperties;
            properties[Constants.APPROVE_STATUS] = radApproved.Checked ? TaskResult.Approved.ToString() : TaskResult.Rejected.ToString();
            if(!string.IsNullOrEmpty(txtMessage.Text))
            properties[Constants.APPROVE_MESSAGE] = txtMessage.Text.Trim();

            CurrentTaskItem[SPBuiltInFieldId.WorkflowVersion] = 1;
            SPWorkflowTask.AlterTask(CurrentTaskItem, properties, true);
            CurrentTaskItem.SystemUpdate();
            Back();

        }

        protected void Back()
        {
            if (IsDialog)
                ClosePopup();
            else
                if (string.IsNullOrEmpty(SourceUrl))
                    SPUtility.Redirect(Request.RawUrl, SPRedirectFlags.Default, this.Context);
                else
                    SPUtility.Redirect(SPEncode.UrlDecodeAsUrl(SourceUrl), SPRedirectFlags.Default, this.Context);
        }
        private void ClosePopup()
        {
            Context.Response.Clear();
            Context.Response.Write("<script type='text/javascript'>window.frameElement.commitPopup();</script>");
            Context.Response.Flush();
            Context.Response.End();
        }
        protected bool IsDialog
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString["IsDlg"]))
                    return false;
                try
                {
                    return Convert.ToBoolean(Convert.ToByte(Request.QueryString["IsDlg"].Split(',')[0]));
                }
                catch { return false; }
            }
        }

        protected string SourceUrl
        {
            get
            {
                return base.Request.QueryString["Source"];
            }
        }
        void btnCancel_Click(object sender, EventArgs e)
        {
            
        }

        
        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}
