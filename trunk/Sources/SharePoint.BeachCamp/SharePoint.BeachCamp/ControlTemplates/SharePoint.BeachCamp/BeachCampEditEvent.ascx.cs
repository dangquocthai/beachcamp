using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using Microsoft.SharePoint;
using SharePoint.BeachCamp.Util.Utilities;
using SharePoint.BeachCamp.Util;

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

            repeaterPrices.ItemDataBound += new RepeaterItemEventHandler(repeaterPrices_ItemDataBound);
            btnSave.Click += new EventHandler(btnSave_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string output = string.Empty;
                //Get user info
                output = GetUserInfo();
                if (!string.IsNullOrEmpty(output))
                {
                    ShowErrorMessages(output, true);
                    return;
                }
                //Get price table
                output = GetPrices();
                if (!string.IsNullOrEmpty(output))
                {
                    ShowErrorMessages(output, true);
                    return;
                }
            }
        }

        #region Events
        void repeaterPrices_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DataRowView rowView = (DataRowView)e.Item.DataItem;
            if (rowView != null)
            {
                string period1 = GetPeriod(BeachCampFieldId.Period1);
                string period2 = GetPeriod(BeachCampFieldId.Period2);
                string fullDay = GetPeriod(BeachCampFieldId.FullDay);
                string ramadan = GetPeriod(BeachCampFieldId.Ramadan);

                string sectionPeriod = SPContext.Current.ListItem[SPBuiltInFieldId.Location].ToString();

                Literal literalSection = (Literal)e.Item.FindControl("literalSection");
                literalSection.Text = rowView["Title"].ToString();

                //Literal literalPeriod1 = (Literal)e.Item.FindControl("literalPeriod1");
                //literalPeriod1.Text = rowView["Period1"].ToString();

                CheckBox chkPeriod1 = (CheckBox)e.Item.FindControl("chkPeriod1");
                chkPeriod1.Text = rowView["Period1"].ToString();
                string toolTipPeriod1 = rowView["Title"].ToString() + " - " + period1;
                chkPeriod1.ToolTip = toolTipPeriod1;
                if (sectionPeriod.Contains(toolTipPeriod1))
                    chkPeriod1.Checked = true;

                //Literal literalPeriod2 = (Literal)e.Item.FindControl("literalPeriod2");
                //literalPeriod2.Text = rowView["Period2"].ToString();

                CheckBox chkPeriod2 = (CheckBox)e.Item.FindControl("chkPeriod2");
                chkPeriod2.Text = rowView["Period2"].ToString();
                string toolTipPeriod2 = rowView["Title"].ToString() + " - " + period2;
                chkPeriod2.ToolTip = toolTipPeriod2;
                if (sectionPeriod.Contains(toolTipPeriod2))
                    chkPeriod2.Checked = true;

                //Literal literalFullDay = (Literal)e.Item.FindControl("literalFullDay");
                //literalFullDay.Text = rowView["FullDay"].ToString();

                CheckBox chkFullDay = (CheckBox)e.Item.FindControl("chkFullDay");
                chkFullDay.Text = rowView["FullDay"].ToString();
                string to0lTipFullDay = rowView["Title"].ToString() + " - " + fullDay;
                chkFullDay.ToolTip = to0lTipFullDay;
                if (sectionPeriod.Contains(to0lTipFullDay))
                    chkFullDay.Checked = true;

                //Literal literalRamadan = (Literal)e.Item.FindControl("literalRamadan");
                //literalRamadan.Text = rowView["Ramadan"].ToString();

                CheckBox chkRamadan = (CheckBox)e.Item.FindControl("chkRamadan");
                chkRamadan.Text = rowView["Ramadan"].ToString();
                string toolTipRamadan = rowView["Title"].ToString() + " - " + ramadan;
                chkRamadan.ToolTip = toolTipRamadan;
                if (sectionPeriod.Contains(toolTipRamadan))
                    chkRamadan.Checked = true;
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.Page.IsValid)
                return;

            string output = UpdateBeachCampEvent();

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

        private string GetUserInfo()
        {
            string output = string.Empty;
            try
            {
                SPFieldUserValue createdBy = new SPFieldUserValue(SPContext.Current.Web, SPContext.Current.ListItem[SPBuiltInFieldId.Author].ToString());
                if (createdBy.User.ID != SPContext.Current.Web.CurrentUser.ID)
                    return "You can not modify this reservation !";

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
                                literalEmployeeName.Text = userItem["Title"].ToString();
                                literalEmployeeCode.Text = userItem["ID"].ToString();
                                literalDepartment.Text = userItem["Department"] == null ? "Null" : userItem["Department"].ToString();
                                literalSection.Text = "Section";
                                literalOfficeTel.Text = "Office Tel";
                                literalMobile.Text = userItem["MobilePhone"] == null ? "Null" : userItem["MobilePhone"].ToString();
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

        private string GetPrices()
        {
            string output = string.Empty;
            try
            {
                SPList priceList = Utility.GetListFromURL("/Lists/BCPrices", SPContext.Current.Web);
                SPListItemCollection itemCollections = priceList.GetItems();
                repeaterPrices.DataSource = itemCollections.GetDataTable();
                repeaterPrices.DataBind();
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
            if (hideSaveButton)
                btnSave.Visible = false;
        }

        private string GetPeriod(Guid period)
        {
            SPList list = Utility.GetListFromURL("/Lists/BCPrices", SPContext.Current.Web);
            SPField field = list.Fields[period];
            if (field != null)
                return field.Title;
            return string.Empty;
        }

        private string UpdateBeachCampEvent()
        {
            string output = string.Empty;
            try
            {
                string sectionPeriod = string.Empty;
                double totalPrice = 0;

                DateTime beachCampDate = DateTime.Parse(ffEventDate.Value.ToString());

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
                    return "Please choose a Section and Period !";

                string typeOfBeachCamp = rdbPersonal.Text;
                if (rdbBusiness.Checked)
                    typeOfBeachCamp = rdbBusiness.Text;

                totalPrice = totalPrice * int.Parse(ffRequireDay.Value.ToString());

                SPListItem item = SPContext.Current.ListItem;
                item[SPBuiltInFieldId.Title] = literalEmployeeName.Text;
                item["TypeOfBeachCamp"] = typeOfBeachCamp;
                item["EmployeeCode"] = literalEmployeeCode.Text;
                item["Department"] = literalDepartment.Text;
                item["Section"] = literalSection.Text;
                item["OfficeTel"] = literalOfficeTel.Text;
                item["Mobile"] = literalMobile.Text;
                item[SPBuiltInFieldId.StartDate] = beachCampDate;
                item[SPBuiltInFieldId.EndDate] = beachCampDate.AddDays(int.Parse(ffRequireDay.Value.ToString()));
                item["Reason"] = ffReason.Value;
                item["RequireDay"] = ffRequireDay.Value;
                item["TotalPrice"] = totalPrice;
                item[SPBuiltInFieldId.Location] = sectionPeriod.TrimEnd('|');
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
