using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using SharePoint.BeachCamp.Util;
using System.Collections;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.Utilities;
using SharePoint.BeachCamp.Util.Utilities;
using System.Web.UI.WebControls;
using System.Data;
using SharePoint.BeachCamp.Util.Helpers;

namespace SharePoint.BeachCamp.Layouts.SharePoint.BeachCamp
{
    public partial class BeachCampTask : LayoutsPageBase
    {
        #region Properties
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
        #endregion Properties

        protected override void OnInit(EventArgs e)
        {
            btnUpdate.Click += new EventHandler(btnUpdate_Click);
            repeaterPrices.ItemDataBound += new RepeaterItemEventHandler(repeaterPrices_ItemDataBound);
            radApproved.CheckedChanged += new EventHandler(radApproved_CheckedChanged);
            radApproved.AutoPostBack = true;
            radReject.CheckedChanged += new EventHandler(radApproved_CheckedChanged);
            radReject.AutoPostBack = true;
            base.OnInit(e);
        }

        void radApproved_CheckedChanged(object sender, EventArgs e)
        {
            txtMessage.Enabled = false;
            if (!radApproved.Checked)
            {
                txtMessage.Enabled = true;
            }
        }

        void btnUpdate_Click(object sender, EventArgs e)
        {
            Hashtable properties = CurrentTaskExtendedProperties;
            properties[Constants.APPROVE_STATUS] = radApproved.Checked ? TaskResult.Approved.ToString() : TaskResult.Rejected.ToString();
            if(!string.IsNullOrEmpty(txtMessage.Text))
            properties[Constants.APPROVE_MESSAGE] = txtMessage.Text.Trim();

            CurrentTaskItem[SPBuiltInFieldId.WorkflowVersion] = 1;
            SPWorkflowTask.AlterTask(CurrentTaskItem, properties,true);
            //CurrentTaskItem.SystemUpdate();
            CurrentTaskItem.Update();
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
            //Context.Response.Clear();
            //Context.Response.Write("<script type='text/javascript'>window.frameElement.commitPopup();</script>");
            //Context.Response.Flush();
            //Context.Response.End();
            this.Page.Response.Clear();
            this.Page.Response.Write(string.Format(System.Globalization.CultureInfo.InvariantCulture, @"<script type='text/javascript'> window.frameElement.commonModalDialogClose(1, '{0}');</script>", ""));
            this.Page.Response.End();
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Get Beach Camp Reservation
                GetBeachCampReservation();
            }
        }

        #region Functions

        private void GetBeachCampReservation()
        {
            try
            {
                SPListItem item = CurrentWorkflowItem;

                string personal = item["TypeOfBeachCamp"].ToString();
                rdbBusiness.Checked = true;
                if (personal == "Personal")
                    rdbPersonal.Checked = true;
                rdbBusiness.Enabled = false;
                rdbPersonal.Enabled = false;
                literalEmployeeName.Text = item[SPBuiltInFieldId.Title].ToString();
                literalEmployeeCode.Text = item["EmployeeCode"].ToString();
                literalDepartment.Text = item["Department"] == null ? string.Empty : item["Department"].ToString();
                literalSection.Text = item["Section"] == null ? string.Empty : item["Section"].ToString();
                literalOfficeTel.Text = item["OfficeTel"] == null ? string.Empty : item["OfficeTel"].ToString();
                literalMobile.Text = item["Mobile"] == null ? string.Empty : item["Mobile"].ToString();
                literalReason.Text = item["Reason"] == null ? string.Empty : item["Reason"].ToString();
                literalRequireDay.Text = item["RequireDay"].ToString();
                literalEventDate.Text = item["EventDate"].ToString();
                //Check reservation is approved or rejected
                if (item["GSApproval"] != null  
                    && (item["GSApproval"].ToString() == TaskResult.Rejected.ToString() || item["GSApproval"].ToString() == TaskResult.Approved.ToString()))
                {
                    radApproved.Checked = true;
                    if(item["GSApproval"].ToString() == TaskResult.Rejected.ToString())
                        radApproved.Checked = true;
                    txtMessage.Text = item["GSApprovalComment"] == null ? string.Empty : item["GSApprovalComment"].ToString();
                    radReject.Enabled = false;
                    radApproved.Enabled = false;
                    txtMessage.Enabled = false;
                    btnUpdate.Visible = false;
                }
                BeachCampHelper.GetPrices(repeaterPrices, SPContext.Current.Web);
            }
            catch (Exception ex)
            {
                Utility.LogError(ex.Message, BeachCampFeatures.Workflow);
            }
        }

        void repeaterPrices_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DataRowView rowView = (DataRowView)e.Item.DataItem;
            if (rowView != null)
            {
                var currentWeb = SPContext.Current.Web;
                string period1 = BeachCampHelper.GetPeriod(BeachCampFieldId.Period1, currentWeb);
                string period2 = BeachCampHelper.GetPeriod(BeachCampFieldId.Period2, currentWeb);
                string fullDay = BeachCampHelper.GetPeriod(BeachCampFieldId.FullDay, currentWeb);
                string ramadan = BeachCampHelper.GetPeriod(BeachCampFieldId.Ramadan, currentWeb);

                string sectionPeriod = CurrentWorkflowItem[SPBuiltInFieldId.Location].ToString();

                Literal literalSection = (Literal)e.Item.FindControl("literalSection");
                literalSection.Text = rowView["Title"].ToString();

                //Literal literalPeriod1 = (Literal)e.Item.FindControl("literalPeriod1");
                //literalPeriod1.Text = rowView["Period1"].ToString();

                CheckBox chkPeriod1 = (CheckBox)e.Item.FindControl("chkPeriod1");
                chkPeriod1.Text = rowView["Period1"].ToString();
                chkPeriod1.Enabled = false;
                string toolTipPeriod1 = rowView["Title"].ToString() + " - " + period1;
                chkPeriod1.ToolTip = toolTipPeriod1;
                if (sectionPeriod.Contains(toolTipPeriod1))
                    chkPeriod1.Checked = true;

                //Literal literalPeriod2 = (Literal)e.Item.FindControl("literalPeriod2");
                //literalPeriod2.Text = rowView["Period2"].ToString();

                CheckBox chkPeriod2 = (CheckBox)e.Item.FindControl("chkPeriod2");
                chkPeriod2.Text = rowView["Period2"].ToString();
                chkPeriod2.Enabled = false;
                string toolTipPeriod2 = rowView["Title"].ToString() + " - " + period2;
                chkPeriod2.ToolTip = toolTipPeriod2;
                if (sectionPeriod.Contains(toolTipPeriod2))
                    chkPeriod2.Checked = true;

                //Literal literalFullDay = (Literal)e.Item.FindControl("literalFullDay");
                //literalFullDay.Text = rowView["FullDay"].ToString();

                CheckBox chkFullDay = (CheckBox)e.Item.FindControl("chkFullDay");
                chkFullDay.Text = rowView["FullDay"].ToString();
                chkFullDay.Enabled = false;
                string to0lTipFullDay = rowView["Title"].ToString() + " - " + fullDay;
                chkFullDay.ToolTip = to0lTipFullDay;
                if (sectionPeriod.Contains(to0lTipFullDay))
                    chkFullDay.Checked = true;

                //Literal literalRamadan = (Literal)e.Item.FindControl("literalRamadan");
                //literalRamadan.Text = rowView["Ramadan"].ToString();

                CheckBox chkRamadan = (CheckBox)e.Item.FindControl("chkRamadan");
                chkRamadan.Text = rowView["Ramadan"].ToString();
                chkRamadan.Enabled = false;
                string toolTipRamadan = rowView["Title"].ToString() + " - " + ramadan;
                chkRamadan.ToolTip = toolTipRamadan;
                if (sectionPeriod.Contains(toolTipRamadan))
                    chkRamadan.Checked = true;
            }
        }

        #endregion Functions
    }
}
