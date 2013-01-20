using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;
using SharePoint.BeachCamp.Util.Helpers;
using System.Data;
using SharePoint.BeachCamp.Util.Utilities;
using SharePoint.BeachCamp.Util;
using Microsoft.SharePoint.Utilities;
using System.Linq;

namespace SharePoint.BeachCamp.Layouts.SharePoint.BeachCamp
{
    public partial class BeachCampPayment : LayoutsPageBase
    {
        protected override void OnInit(EventArgs e)
        {
            btnUpdate.Click += new EventHandler(btnUpdate_Click);
            repeaterPrices.ItemDataBound += new RepeaterItemEventHandler(repeaterPrices_ItemDataBound);
            
            base.OnInit(e);
        }

        
        void btnUpdate_Click(object sender, EventArgs e)
        {
            using (DisableItemEvent disableItemEvent = new DisableItemEvent())
            {
                var currentItem = SPContext.Current.ListItem;
                if (radPaid.Checked)
                    currentItem["Paid"] = true;                 
                else
                    currentItem["Paid"] = false;
                currentItem.SystemUpdate();
            }
            
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
                //Update
                SPGroup reservationAdminGroup = Web.Groups[Constants.BEACH_CAMP_ADMIN_GROUP];
                if (!IsValidUser(SPContext.Current.Web.CurrentUser, reservationAdminGroup))
                {
                    lblError.Text = "You do not have GS Approval permission.";
                    lblError.Visible = true;
                    radPaid.Enabled = false;
                    radUnpaid.Enabled = false;
                    btnUpdate.Enabled = false;
                }
            }
        }

        #region Functions

        private bool IsValidUser(SPUser spUser, SPGroup spGroup)
        {
            return spUser.Groups.Cast<SPGroup>()
              .Any(g => g.ID == spGroup.ID);
        }

        private void GetBeachCampReservation()
        {
            try
            {
                SPListItem item = SPContext.Current.ListItem;

                string personal = item["TypeOfBeachCamp"].ToString();
                if (personal == "Business")
                {
                    rdbBusiness.Checked = true;
                }
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

                //Load GSApproval
                if (item["GSApproval"] != null && item["GSApproval"].ToString() == TaskResult.Approved.ToString())
                {
                    radApproved.Checked = true;
                }
                else if(item["GSApproval"] != null && item["GSApproval"].ToString() == TaskResult.Rejected.ToString())
                {
                    radReject.Checked = true;
                    txtMessage.Text = item["GSApprovalComment"] == null ? string.Empty : item["GSApprovalComment"].ToString();
                    txtMessage.Enabled = true;
                }
                else
                {
                    lblApprovalError.Text = "This reservation status is : " + item["GSApproval"].ToString();
                    lblApprovalError.Visible = true;
                }

                //Check reservation is paid or unpaid
                radPaid.Enabled = false;
                radUnpaid.Enabled = false;
                btnUpdate.Enabled = false;
                if (item["GSApproval"].ToString() == TaskResult.Approved.ToString())
                {
                    radPaid.Enabled = true;
                    radUnpaid.Enabled = true;
                    btnUpdate.Enabled = true;
                    if (bool.Parse(item["Paid"].ToString()))
                    {
                        radPaid.Checked = true;
                    }
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

                string sectionPeriod = SPContext.Current.ListItem[SPBuiltInFieldId.Location].ToString();

                Literal literalSection = (Literal)e.Item.FindControl("literalSection");
                literalSection.Text = rowView["Title"].ToString();

                //Literal literalPeriod1 = (Literal)e.Item.FindControl("literalPeriod1");
                //literalPeriod1.Text = rowView["Period1"].ToString();

                CheckBox chkPeriod1 = (CheckBox)e.Item.FindControl("chkPeriod1");
                chkPeriod1.Text = rowView["Period1"].ToString() + " SR";
                chkPeriod1.Enabled = false;
                string toolTipPeriod1 = rowView["Title"].ToString() + " - " + period1;
                chkPeriod1.ToolTip = toolTipPeriod1;
                if (sectionPeriod.Contains(toolTipPeriod1))
                    chkPeriod1.Checked = true;

                //Literal literalPeriod2 = (Literal)e.Item.FindControl("literalPeriod2");
                //literalPeriod2.Text = rowView["Period2"].ToString();

                CheckBox chkPeriod2 = (CheckBox)e.Item.FindControl("chkPeriod2");
                chkPeriod2.Text = rowView["Period2"].ToString() + " SR";
                chkPeriod2.Enabled = false;
                string toolTipPeriod2 = rowView["Title"].ToString() + " - " + period2;
                chkPeriod2.ToolTip = toolTipPeriod2;
                if (sectionPeriod.Contains(toolTipPeriod2))
                    chkPeriod2.Checked = true;

                //Literal literalFullDay = (Literal)e.Item.FindControl("literalFullDay");
                //literalFullDay.Text = rowView["FullDay"].ToString();

                CheckBox chkFullDay = (CheckBox)e.Item.FindControl("chkFullDay");
                chkFullDay.Text = rowView["FullDay"].ToString() + " SR";
                chkFullDay.Enabled = false;
                string to0lTipFullDay = rowView["Title"].ToString() + " - " + fullDay;
                chkFullDay.ToolTip = to0lTipFullDay;
                if (sectionPeriod.Contains(to0lTipFullDay))
                    chkFullDay.Checked = true;

                //Literal literalRamadan = (Literal)e.Item.FindControl("literalRamadan");
                //literalRamadan.Text = rowView["Ramadan"].ToString();

                CheckBox chkRamadan = (CheckBox)e.Item.FindControl("chkRamadan");
                chkRamadan.Text = rowView["Ramadan"].ToString() + " SR";
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
