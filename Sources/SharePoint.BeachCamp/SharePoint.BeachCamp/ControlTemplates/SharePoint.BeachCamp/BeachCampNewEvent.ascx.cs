﻿using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using Microsoft.SharePoint;
using SharePoint.BeachCamp.Util;
using SharePoint.BeachCamp.Util.Utilities;
using SharePoint.BeachCamp.Util.Helpers;
using Microsoft.SharePoint.Workflow;

namespace SharePoint.BeachCamp.ControlTemplates.SharePoint.BeachCamp
{
    public partial class BeachCampNewEvent : UserControl
    {

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Microsoft.SharePoint.WebControls.SPRibbon ribbon = Microsoft.SharePoint.WebControls.SPRibbon.GetCurrent(this.Page);
            if (ribbon != null)
            {
                ribbon.TrimById("Ribbon.ListForm.Edit.Commit");
            }

            repeaterPrices.ItemDataBound += new RepeaterItemEventHandler(repeaterPrices_ItemDataBound);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnSaveAndSubmit.Click += new EventHandler(btnSaveAndSubmit_Click);
            chkUnderstand.CheckedChanged += new EventHandler(chkUnderstand_CheckedChanged);
            //txtEventDate.AutoPostBack = true;
            //txtEventDate.TextChanged += new EventHandler(txtEventDate_TextChanged);

            string output = string.Empty;
            //Get user info
            output = GetUserInfo();
            if (!string.IsNullOrEmpty(output))
            {
                ShowErrorMessages(output, true);
                return;
            }
        }

        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Get price table
                string output = BeachCampHelper.GetPrices(repeaterPrices, SPContext.Current.Web);
                if (!string.IsNullOrEmpty(output))
                {
                    ShowErrorMessages(output, true);
                    return;
                }
            }
        }

        #region Events

        void chkUnderstand_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUnderstand.Checked)
                btnSave.Enabled = true;
            else
                btnSave.Enabled = false;
        }

        void txtEventDate_TextChanged(object sender, EventArgs e)
        {
            string[] eventDateArray = txtEventDate.Text.Split('/');
            string sectionPeriod = BeachCampHelper.GetReservationByDate(SPContext.Current.Web, new DateTime(int.Parse(eventDateArray[2]), int.Parse(eventDateArray[1]), int.Parse(eventDateArray[0])));
        }

        void btnSaveAndSubmit_Click(object sender, EventArgs e)
        {
            if (!this.Page.IsValid)
                return;

            string output = AddBeachCampEvent(TaskResult.Pending);

            if (!string.IsNullOrEmpty(output))
            {
                ShowErrorMessages(output, false);
                return;
            }

            //StartWorkflow(itemId, "Approve Beach Camp Reservation");

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

            string errorMessage = CheckReverseSection(((CheckBox)sender).ToolTip);
            if (!string.IsNullOrEmpty(errorMessage))
                ShowErrorMessages(errorMessage, false);

            /*
             HideErrorMessages(true);
            if (!string.IsNullOrEmpty(txtEventDate.Text))
            {
                string[] eventDateArray = txtEventDate.Text.Split('/');
                string sectionPeriod = BeachCampHelper.GetReservationByDate(SPContext.Current.Web, new DateTime(int.Parse(eventDateArray[2]), int.Parse(eventDateArray[1]), int.Parse(eventDateArray[0])));
                if (!string.IsNullOrEmpty(sectionPeriod))
                {
                    string selectedSectionPeriod = ((CheckBox)sender).ToolTip;
                    string[] sectionPeriodArray = sectionPeriod.Split('#');
                    for (int i = 0; i < sectionPeriodArray.Length; i++)
                    {
                        if (selectedSectionPeriod.Split('-')[0].TrimEnd(' ').Contains(sectionPeriodArray[i].Split('-')[0].TrimEnd(' ')))
                        {
                            ShowErrorMessages(Constants.ERROR_MESSAGE2, false);
                            break;
                        }
                    }
                }               
            }
            int requiredDay = 1;
            //int.TryParse(ffRequireDay.Value == null ? "0" : ffRequireDay.Value.ToString(), out requiredDay);
            DateTime eventDate = DateTime.Now;
            DateTime.TryParse(ffEventDate.Value.ToString(), out eventDate);
            HideErrorMessages(true);
            bool isSectionPeriodReserved = BeachCampHelper.IsSectionPeriodReserved(SPContext.Current.Web, ((CheckBox)sender).ToolTip, eventDate, requiredDay);
            if(isSectionPeriodReserved)
                ShowErrorMessages(false);//ShowErrorMessages("This section and period is reserved. Please choose another section - period !", false);
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

                Literal literalSection = (Literal)e.Item.FindControl("literalSection");
                literalSection.Text = rowView["Title"].ToString();

                //Literal literalPeriod1 = (Literal)e.Item.FindControl("literalPeriod1");
                //literalPeriod1.Text = rowView["Period1"].ToString();

                CheckBox chkPeriod1 = (CheckBox)e.Item.FindControl("chkPeriod1");
                chkPeriod1.Text = rowView["Period1"].ToString() + " SR";
                chkPeriod1.ToolTip = rowView["Title"].ToString() + " - " + period1;

                //RadioButton chkPeriod1 = (RadioButton)e.Item.FindControl("radPeriod1");
                //chkPeriod1.Text = rowView["Period1"].ToString();
                //chkPeriod1.ToolTip = rowView["Title"].ToString() + " - " + period1;

                //Literal literalPeriod2 = (Literal)e.Item.FindControl("literalPeriod2");
                //literalPeriod2.Text = rowView["Period2"].ToString();

                CheckBox chkPeriod2 = (CheckBox)e.Item.FindControl("chkPeriod2");
                chkPeriod2.Text = rowView["Period2"].ToString() + " SR";
                chkPeriod2.ToolTip = rowView["Title"].ToString() + " - " + period2;

                //RadioButton chkPeriod2 = (RadioButton)e.Item.FindControl("radPeriod2");
                //chkPeriod2.Text = rowView["Period1"].ToString();
                //chkPeriod2.ToolTip = rowView["Title"].ToString() + " - " + period1;

                //Literal literalFullDay = (Literal)e.Item.FindControl("literalFullDay");
                //literalFullDay.Text = rowView["FullDay"].ToString();

                CheckBox chkFullDay = (CheckBox)e.Item.FindControl("chkFullDay");
                chkFullDay.Text = rowView["FullDay"].ToString() + " SR";
                chkFullDay.ToolTip = rowView["Title"].ToString() + " - " + fullDay;

                //RadioButton chkFullDay = (RadioButton)e.Item.FindControl("radFullDay");
                //chkFullDay.Text = rowView["Period1"].ToString();
                //chkFullDay.ToolTip = rowView["Title"].ToString() + " - " + period1;

                //Literal literalRamadan = (Literal)e.Item.FindControl("literalRamadan");
                //literalRamadan.Text = rowView["Ramadan"].ToString();

                CheckBox chkRamadan = (CheckBox)e.Item.FindControl("chkRamadan");
                chkRamadan.Text = rowView["Ramadan"].ToString() + " SR";
                chkRamadan.ToolTip = rowView["Title"].ToString() + " - " + ramadan;

                //RadioButton chkRamadan = (RadioButton)e.Item.FindControl("radRamadan");
                //chkRamadan.Text = rowView["Period1"].ToString();
                //chkRamadan.ToolTip = rowView["Title"].ToString() + " - " + period1;
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.Page.IsValid)
                return;
            string output = AddBeachCampEvent(TaskResult.Draft);

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

        private string CheckReverseSection(string selectedSectionPeriod)
        {
            try
            {
                HideErrorMessages(true);
                if (!string.IsNullOrEmpty(txtEventDate.Text))
                {
                    string[] eventDateArray = txtEventDate.Text.Split('/');
                    string sectionPeriod = BeachCampHelper.GetReservationByDate(SPContext.Current.Web, new DateTime(int.Parse(eventDateArray[2]), int.Parse(eventDateArray[1]), int.Parse(eventDateArray[0])));
                    if (!string.IsNullOrEmpty(sectionPeriod))
                    {
                        string[] sectionPeriodArray = sectionPeriod.Split('#');
                        for (int i = 0; i < sectionPeriodArray.Length; i++)
                        {
                            string[] selectedSectionPeriodDetailArray = selectedSectionPeriod.Split('-');
                            string[] sectionPeriodDetailArray = sectionPeriodArray[i].Split('-');
                            if (selectedSectionPeriodDetailArray[1].TrimEnd(' ').Equals(sectionPeriodDetailArray[1].TrimEnd(' ')))
                            {
                                if (selectedSectionPeriodDetailArray[0].TrimEnd(' ').Contains(sectionPeriodDetailArray[0].TrimEnd(' '))
                                    || sectionPeriodDetailArray[0].TrimEnd(' ').Contains(selectedSectionPeriodDetailArray[0].TrimEnd(' ')))
                                {
                                    return Constants.ERROR_MESSAGE2;
                                }
                            }
                        }

                        //for (int i = 0; i < sectionPeriodArray.Length; i++)
                        //{
                        //    if (selectedSectionPeriod.TrimEnd(' ').Equals(sectionPeriodArray[i].TrimEnd(' ')))
                        //    {
                        //        return Constants.ERROR_MESSAGE2;
                        //    }
                        //}

                        //for (int i = 0; i < sectionPeriodArray.Length; i++)
                        //{
                        //    if (selectedSectionPeriod.Split('-')[0].TrimEnd(' ').Contains(sectionPeriodArray[i].Split('-')[0].TrimEnd(' '))
                        //        || sectionPeriodArray[i].Split('-')[0].TrimEnd(' ').Contains(selectedSectionPeriod.Split('-')[0].TrimEnd(' ')))
                        //    {
                        //        return Constants.ERROR_MESSAGE2;
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.LogError(ex.Message, BeachCampFeatures.BeachCamp);
                return ex.Message;
            }
            return string.Empty;
        }

        private string GetUserInfo()
        {
            string output = string.Empty;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPListItemCollection userItems = web.Lists.TryGetList(web.SiteUserInfoList.Title).GetItems();

                            SPListItem userItem = web.Lists.TryGetList(web.SiteUserInfoList.Title).GetItemById(SPContext.Current.Web.CurrentUser.ID);
                            if (userItem != null)
                            {
                                ffTitle.Value = userItem["Title"].ToString();
                                //txtEmployeeName.Enabled = false;
                                ffEmployeeCode.Value = userItem["ID"].ToString();
                                //txtEmployeeCode.Enabled = false;
                                ffDepartment.Value = userItem["Department"] == null ? string.Empty : userItem["Department"].ToString();
                                //ffSection.Value = "";
                                //txtOfficeTel.Text = "";
                                ffMobile.Value = userItem["MobilePhone"] == null ? string.Empty : userItem["MobilePhone"].ToString();
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                output = ex.Message;
            }

            return output;
        }

        private void ShowErrorMessages(string message, bool hideSaveButton)
        {
            lblError.Text = message;
            lblError.Visible = true;
            btnSave.Visible = !hideSaveButton;
        }

        private void ShowErrorMessages(bool hideSaveButton)
        {
            lblError.Visible = true;
            btnSave.Visible = !hideSaveButton;
        }

        private void HideErrorMessages(bool showSaveButton)
        {
            lblError.Text = string.Empty;
            lblError.Visible = false;
            btnSave.Visible = showSaveButton;
        }

        private string AddBeachCampEvent(TaskResult status)
        {
            string output = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(lblError.Text) && lblError.Visible)
                    return lblError.Text;

                string sectionPeriod = string.Empty;
                double totalPrice = 0;

                if (string.IsNullOrEmpty(txtEventDate.Text))
                    return Constants.ERROR_MESSAGE;

                string[] eventDateArray = txtEventDate.Text.Split('/');

                DateTime beachCampDate = new DateTime(int.Parse(eventDateArray[2]), int.Parse(eventDateArray[1]),int.Parse(eventDateArray[0]));//DateTime.Parse(ffEventDate.Value.ToString());
                //int requireDay = int.Parse(ffRequireDay.Value.ToString());
                DateTime beachCampEndDate = beachCampDate;//beachCampDate.AddDays(requireDay);

                bool isReserved = BeachCampHelper.IsUserReserved(SPContext.Current.Web, SPContext.Current.Web.CurrentUser.ID.ToString(), beachCampDate);
                if (isReserved)
                    return Constants.ERROR_MESSAGE1; //return "You can only reserve beach camp one a month. Please select another day!";

                foreach (RepeaterItem prices in repeaterPrices.Items)
                {
                    #region CheckBox
                    CheckBox chkPeriod1 = (CheckBox)prices.FindControl("chkPeriod1");
                    if (chkPeriod1 != null && chkPeriod1.Checked)
                    {
                        sectionPeriod += chkPeriod1.ToolTip + "|";
                        totalPrice += double.Parse(chkPeriod1.Text.Split(' ')[0]);
                        break;//Choose one
                    }

                    CheckBox chkPediod2 = (CheckBox)prices.FindControl("chkPeriod2");
                    if (chkPediod2 != null && chkPediod2.Checked)
                    {
                        sectionPeriod += chkPediod2.ToolTip + "|";
                        totalPrice += double.Parse(chkPediod2.Text.Split(' ')[0]);
                        break;//Choose one
                    }

                    CheckBox chkFullDay = (CheckBox)prices.FindControl("chkFullDay");
                    if (chkFullDay != null && chkFullDay.Checked)
                    {
                        sectionPeriod += chkFullDay.ToolTip + "|";
                        totalPrice += double.Parse(chkFullDay.Text.Split(' ')[0]);
                        break;//Choose one
                    }

                    CheckBox chkRamadan = (CheckBox)prices.FindControl("chkRamadan");
                    if (chkRamadan != null && chkRamadan.Checked)
                    {
                        sectionPeriod += chkRamadan.ToolTip + "|";
                        totalPrice += double.Parse(chkRamadan.Text.Split(' ')[0]);
                        break;//Choose one
                    }
                    #endregion CheckBox

                    #region RadioButton
                    //RadioButton chkPeriod1 = (RadioButton)prices.FindControl("radPeriod1");
                    //if (chkPeriod1 != null && chkPeriod1.Checked)
                    //{
                    //    sectionPeriod += chkPeriod1.ToolTip + "|";
                    //    totalPrice += double.Parse(chkPeriod1.Text);
                    //    break;
                    //}

                    //RadioButton chkPediod2 = (RadioButton)prices.FindControl("radPeriod2");
                    //if (chkPediod2 != null && chkPediod2.Checked)
                    //{
                    //    sectionPeriod += chkPediod2.ToolTip + "|";
                    //    totalPrice += double.Parse(chkPediod2.Text);
                    //    break;
                    //}

                    //RadioButton chkFullDay = (RadioButton)prices.FindControl("radFullDay");
                    //if (chkFullDay != null && chkFullDay.Checked)
                    //{
                    //    sectionPeriod += chkFullDay.ToolTip + "|";
                    //    totalPrice += double.Parse(chkFullDay.Text);
                    //    break;
                    //}

                    //RadioButton chkRamadan = (RadioButton)prices.FindControl("radRamadan");
                    //if (chkRamadan != null && chkRamadan.Checked)
                    //{
                    //    sectionPeriod += chkRamadan.ToolTip + "|";
                    //    totalPrice += double.Parse(chkRamadan.Text);
                    //    break;
                    //}
                    #endregion RadioButton
                }

                if (string.IsNullOrEmpty(sectionPeriod))
                    return Constants.ERROR_MESSAGE3;//return "Please choose a Section and Period !";

                sectionPeriod = sectionPeriod.TrimEnd('|');

                
                string sectionPeriodReserved = CheckReverseSection(sectionPeriod);
                if (!string.IsNullOrEmpty(sectionPeriodReserved))
                    return sectionPeriodReserved;//"This section and period is reserved. Please choose another section - period !";

                string typeOfBeachCamp = rdbPersonal.Text;
                if (rdbBusiness.Checked)
                    typeOfBeachCamp = rdbBusiness.Text;

                //totalPrice = totalPrice * int.Parse(ffRequireDay.Value.ToString());

                SPListItem item = SPContext.Current.List.AddItem();
                item[SPBuiltInFieldId.Title] = ffTitle.Value;
                item["TypeOfBeachCamp"] = typeOfBeachCamp;
                item["EmployeeCode"] = ffEmployeeCode.Value;
                item["Department"] = ffDepartment.Value;
                item["Section"] = ffSection.Value;
                item["OfficeTel"] = ffOfficeTel.Value;
                item["Mobile"] = ffMobile.Value;

                beachCampDate = new DateTime(beachCampDate.Year, beachCampDate.Month, beachCampDate.Day, 0, 0, 1);
                beachCampEndDate = new DateTime(beachCampEndDate.Year, beachCampEndDate.Month, beachCampEndDate.Day, 23, 59, 59);

                item[SPBuiltInFieldId.StartDate] = beachCampDate;
                item[SPBuiltInFieldId.EndDate] = beachCampDate;//beachCampDate.AddDays(requireDay);

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
