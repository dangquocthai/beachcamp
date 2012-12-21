using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using Microsoft.SharePoint;
using SharePoint.BeachCamp.Util.Utilities;
using SharePoint.BeachCamp.Util;
using SharePoint.BeachCamp.Util.Helpers;

namespace SharePoint.BeachCamp.ControlTemplates.SharePoint.BeachCamp
{
    public partial class BeachCampEditEvent : UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Microsoft.SharePoint.WebControls.SPRibbon ribbon = Microsoft.SharePoint.WebControls.SPRibbon.GetCurrent(this.Page);
            if (ribbon != null)
            {
                ribbon.TrimById("Ribbon.ListForm.Edit.Commit");
            }
            //ffTitle.ControlMode = Microsoft.SharePoint.WebControls.SPControlMode.Display;
            //ffEmployeeCode.ControlMode = Microsoft.SharePoint.WebControls.SPControlMode.Display;
            repeaterPrices.ItemDataBound += new RepeaterItemEventHandler(repeaterPrices_ItemDataBound);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnSaveAndSubmit.Click += new EventHandler(btnSaveAndSubmit_Click);
        }

        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var item = SPContext.Current.ListItem;
                string personal = item["TypeOfBeachCamp"].ToString();
                if (personal == "Business")
                {
                    rdbBusiness.Checked = true;
                }
                txtEmployeeName.Text = item["Title"].ToString();
                txtEmployeeName.Enabled = false;
                txtEmployeeCode.Text = item["EmployeeCode"].ToString();
                txtEmployeeCode.Enabled = false;
                string output = string.Empty;
                //Get price table
                output = BeachCampHelper.GetPrices(repeaterPrices, SPContext.Current.Web);
                if (!string.IsNullOrEmpty(output))
                {
                    ShowErrorMessages(output, true);
                    return;
                }
            }
        }

        #region Events


        void btnSaveAndSubmit_Click(object sender, EventArgs e)
        {
            if (!this.Page.IsValid)
                return;

            string output = UpdateBeachCampEvent(TaskResult.Pending);

            if (!string.IsNullOrEmpty(output))
            {
                ShowErrorMessages(output, false);
                return;
            }

            this.Page.Response.Clear();
            this.Page.Response.Write(
            string.Format(System.Globalization.CultureInfo.InvariantCulture, @"<script type='text/javascript'> window.frameElement.commonModalDialogClose(1, '{0}');</script>", ""));
            this.Page.Response.End();
        }


        protected void SectionPeriod_OnCheckedChanged(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in repeaterPrices.Items)
            {
                CheckBox chkPeriod1 = (CheckBox)item.FindControl("chkPeriod1");
                chkPeriod1.Checked = false;

                CheckBox chkPeriod2 = (CheckBox)item.FindControl("chkPeriod2");
                chkPeriod2.Checked = false;

                CheckBox chkFullDay = (CheckBox)item.FindControl("chkFullDay");
                chkFullDay.Checked = false;

                CheckBox chkRamadan = (CheckBox)item.FindControl("chkRamadan");
                chkRamadan.Checked = false;
            }
            ((CheckBox)sender).Checked = true;

            /*
            int requiredDay = 0;
            //int.TryParse(ffRequireDay.Value == null ? "0" : ffRequireDay.Value.ToString(), out requiredDay);
            DateTime eventDate = DateTime.Now;
            DateTime.TryParse(ffEventDate.Value.ToString(), out eventDate);
            HideErrorMessages(true);
            bool isSectionPeriodReserved = BeachCampHelper.IsSectionPeriodReserved(SPContext.Current.Web, ((CheckBox)sender).ToolTip, eventDate, requiredDay, SPContext.Current.ItemId);
            if (isSectionPeriodReserved)
                ShowErrorMessages("This section and period is reserved. Please choose another section - period !", false);
             */
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
                string toolTipPeriod1 = rowView["Title"].ToString() + " - " + period1;
                chkPeriod1.ToolTip = toolTipPeriod1;
                if (sectionPeriod.Equals(toolTipPeriod1))
                    chkPeriod1.Checked = true;

                //Literal literalPeriod2 = (Literal)e.Item.FindControl("literalPeriod2");
                //literalPeriod2.Text = rowView["Period2"].ToString();

                CheckBox chkPeriod2 = (CheckBox)e.Item.FindControl("chkPeriod2");
                chkPeriod2.Text = rowView["Period2"].ToString();
                string toolTipPeriod2 = rowView["Title"].ToString() + " - " + period2;
                chkPeriod2.ToolTip = toolTipPeriod2;
                if (sectionPeriod.Equals(toolTipPeriod2))
                    chkPeriod2.Checked = true;

                //Literal literalFullDay = (Literal)e.Item.FindControl("literalFullDay");
                //literalFullDay.Text = rowView["FullDay"].ToString();

                CheckBox chkFullDay = (CheckBox)e.Item.FindControl("chkFullDay");
                chkFullDay.Text = rowView["FullDay"].ToString();
                string to0lTipFullDay = rowView["Title"].ToString() + " - " + fullDay;
                chkFullDay.ToolTip = to0lTipFullDay;
                if (sectionPeriod.Equals(to0lTipFullDay))
                    chkFullDay.Checked = true;

                //Literal literalRamadan = (Literal)e.Item.FindControl("literalRamadan");
                //literalRamadan.Text = rowView["Ramadan"].ToString();

                CheckBox chkRamadan = (CheckBox)e.Item.FindControl("chkRamadan");
                chkRamadan.Text = rowView["Ramadan"].ToString();
                string toolTipRamadan = rowView["Title"].ToString() + " - " + ramadan;
                chkRamadan.ToolTip = toolTipRamadan;
                if (sectionPeriod.Equals(toolTipRamadan))
                    chkRamadan.Checked = true;
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.Page.IsValid)
                return;

            string output = UpdateBeachCampEvent(TaskResult.Draft);

            if (!string.IsNullOrEmpty(output))
            {
                ShowErrorMessages(output, false);
                return;
            }

            this.Page.Response.Clear();
            this.Page.Response.Write(
            string.Format(System.Globalization.CultureInfo.InvariantCulture, @"<script type='text/javascript'> window.frameElement.commonModalDialogClose(1, '{0}');</script>", ""));
            this.Page.Response.End();
        }

        #endregion Events

        #region Functions

        private void ShowErrorMessages(string message, bool hideSaveButton)
        {
            lblError.Text = message;
            lblError.Visible = true;
            btnSave.Visible = !hideSaveButton;
        }

        private void HideErrorMessages(bool showSaveButton)
        {
            lblError.Text = string.Empty;
            lblError.Visible = false;
            btnSave.Visible = showSaveButton;
        }

        private string UpdateBeachCampEvent(TaskResult status)
        {
            string output = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(lblError.Text) && lblError.Visible)
                    return lblError.Text;

                string sectionPeriod = string.Empty;
                double totalPrice = 0;

                DateTime beachCampDate = DateTime.Parse(ffEventDate.Value.ToString());

                bool isReserved = BeachCampHelper.IsUserReserved(SPContext.Current.Web, SPContext.Current.ListItem["EmployeeCode"].ToString(), beachCampDate, SPContext.Current.ItemId);
                if (isReserved)
                    return Constants.ERROR_MESSAGE;//return "You can only reserve beach camp one a month. Please select another day!";

                foreach (RepeaterItem prices in repeaterPrices.Items)
                {
                    CheckBox chkPeriod1 = (CheckBox)prices.FindControl("chkPeriod1");
                    if (chkPeriod1 != null && chkPeriod1.Checked)
                    {
                        sectionPeriod += chkPeriod1.ToolTip + "|";
                        totalPrice += double.Parse(chkPeriod1.Text);
                    }

                    CheckBox chkPediod2 = (CheckBox)prices.FindControl("chkPeriod2");
                    if (chkPediod2 != null && chkPediod2.Checked)
                    {
                        sectionPeriod += chkPediod2.ToolTip + "|";
                        totalPrice += double.Parse(chkPediod2.Text);
                    }

                    CheckBox chkFullDay = (CheckBox)prices.FindControl("chkFullDay");
                    if (chkFullDay != null && chkFullDay.Checked)
                    {
                        sectionPeriod += chkFullDay.ToolTip + "|";
                        totalPrice += double.Parse(chkFullDay.Text);
                    }

                    CheckBox chkRamadan = (CheckBox)prices.FindControl("chkRamadan");
                    if (chkRamadan != null && chkRamadan.Checked)
                    {
                        sectionPeriod += chkRamadan.ToolTip + "|";
                        totalPrice += double.Parse(chkRamadan.Text);
                    }
                }

                if (string.IsNullOrEmpty(sectionPeriod))
                    return Constants.ERROR_MESSAGE;//return "Please choose a Section and Period !";

                sectionPeriod = sectionPeriod.TrimEnd('|');

                /*
                bool isSectionPeriodReserved = BeachCampHelper.IsSectionPeriodReserved(SPContext.Current.Web, sectionPeriod, beachCampDate, 1, SPContext.Current.ItemId);
                if (isSectionPeriodReserved)
                    return "This section and period is reserved. Please choose another section - period !";
                */

                string typeOfBeachCamp = rdbPersonal.Text;
                if (rdbBusiness.Checked)
                    typeOfBeachCamp = rdbBusiness.Text;

                //totalPrice = totalPrice * int.Parse(ffRequireDay.Value.ToString());

                SPListItem item = SPContext.Current.ListItem;
                //item[SPBuiltInFieldId.Title] = ffTitle.Value;
                item["TypeOfBeachCamp"] = typeOfBeachCamp;
                //item["EmployeeCode"] = ffEmployeeCode.Value;
                item["Department"] = ffDepartment.Value;
                item["Section"] = ffSection.Value;
                item["OfficeTel"] = ffOfficeTel.Value;
                item["Mobile"] = ffMobile.Value;
                item[SPBuiltInFieldId.StartDate] = beachCampDate;
                item[SPBuiltInFieldId.EndDate] = beachCampDate;//beachCampDate.AddDays(int.Parse(ffRequireDay.Value.ToString()));
                item["Reason"] = ffReason.Value;
                //item["RequireDay"] = ffRequireDay.Value;
                item["TotalPrice"] = totalPrice;
                item[SPBuiltInFieldId.Location] = sectionPeriod;

                item["GSApproval"] = status.ToString();

                item.Update();
            }
            catch (Exception ex)
            {
                Utility.LogError(ex.Message, BeachCampFeatures.BeachCamp);
                output = ex.Message;
            }
            return output;
        }
        #endregion Functions
    }
}
