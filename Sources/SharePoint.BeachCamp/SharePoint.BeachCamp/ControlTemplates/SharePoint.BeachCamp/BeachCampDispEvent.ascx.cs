using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SharePoint.BeachCamp.Util.Utilities;
using SharePoint.BeachCamp.Util;
using Microsoft.SharePoint;
using System.Data;

namespace SharePoint.BeachCamp.ControlTemplates.SharePoint.BeachCamp
{
    public partial class BeachCampDispEvent : UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            repeaterPrices.ItemDataBound += new RepeaterItemEventHandler(repeaterPrices_ItemDataBound);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Load field value
                SPListItem item = SPContext.Current.ListItem;
                literalEmployeeName.Text = item[SPBuiltInFieldId.Title].ToString();
                literalEmployeeCode.Text = item[SPBuiltInFieldId.ID].ToString();
                literalDepartment.Text = item["Department"] == null ? string.Empty : item["Department"].ToString();
                literalSection.Text = item["Section"] == null ? string.Empty : item["Section"].ToString();
                literalOfficeTel.Text = item["OfficeTel"] == null ? string.Empty : item["OfficeTel"].ToString();
                literalMobile.Text = item["Mobile"] == null ? string.Empty : item["Mobile"].ToString();
                literalReason.Text = item["Reason"] == null ? string.Empty : item["Reason"].ToString();
                string output = string.Empty;
                //Get price table
                output = GetPrices();
                if (!string.IsNullOrEmpty(output))
                {
                    ShowErrorMessages(output);
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

        private void ShowErrorMessages(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
        }

        private string GetPeriod(Guid period)
        {
            SPList list = Utility.GetListFromURL("/Lists/BCPrices", SPContext.Current.Web);
            SPField field = list.Fields[period];
            if (field != null)
                return field.Title;
            return string.Empty;
        }

        #endregion Functions
    }
}
