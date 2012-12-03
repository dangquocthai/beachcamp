using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;
using SharePoint.BeachCamp.Util;
using System.Data;
using SharePoint.BeachCamp.Util.Utilities;
using SharePoint.BeachCamp.Util.Helpers;

namespace SharePoint.BeachCamp.Layouts.SharePoint.BeachCamp
{
    public partial class BeachCampExport : LayoutsPageBase
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            repeaterPrices.ItemDataBound+=new RepeaterItemEventHandler(repeaterPrices_ItemDataBound);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //int id = int.Parse(Request.QueryString["BeachCampId"]);
            GetBeachCampReservation();
        }


        #region Functions

        private void GetBeachCampReservation()
        {
            try
            {
                var beachCampList = Utility.GetListFromURL("/Lists/BCCalendar", SPContext.Current.Web);

                SPListItem item = SPContext.Current.ListItem;//beachCampList.GetItemById(beachCampId);

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
                literalEventDate.Text = DateTime.Parse(item["EventDate"].ToString()).ToString("dd/MM/yyyy");
                //Check reservation is approved or rejected
                if (item["GSApproval"] != null
                    && !string.IsNullOrEmpty(item["GSApproval"].ToString()))
                {
                    radReject.Checked = true;
                    if (item["GSApproval"].ToString() == TaskResult.Approved.ToString())
                        radApproved.Checked = true;
                    literalApproveComments.Text = item["GSApprovalComment"].ToString();
                    radReject.Enabled = false;
                    radApproved.Enabled = false;
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
