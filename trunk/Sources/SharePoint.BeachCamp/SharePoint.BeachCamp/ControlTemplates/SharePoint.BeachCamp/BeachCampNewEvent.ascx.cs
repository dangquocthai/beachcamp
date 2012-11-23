using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using Microsoft.SharePoint;
using SharePoint.BeachCamp.Util;
using SharePoint.BeachCamp.Util.Utilities;

namespace SharePoint.BeachCamp.ControlTemplates.SharePoint.BeachCamp
{
    public partial class BeachCampNewEvent : UserControl
    {

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            repeaterPrices.ItemDataBound += new RepeaterItemEventHandler(repeaterPrices_ItemDataBound);
            btnSave.Click += new EventHandler(btnSave_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Get price table
                SPList priceList = Utility.GetListFromURL("/Lists/BCPrices", SPContext.Current.Web);
                SPListItemCollection itemCollections = priceList.GetItems();
                repeaterPrices.DataSource = itemCollections.GetDataTable();
                repeaterPrices.DataBind();

                //Get user info

            }
        }

        #region Events
        void repeaterPrices_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DataRowView rowView = (DataRowView)e.Item.DataItem;
            if (rowView != null)
            {
                string period1 = GetPeriod(BeachCampColumnId.Period1);
                string period2 = GetPeriod(BeachCampColumnId.Period2);
                string fullDay = GetPeriod(BeachCampColumnId.FullDay);
                string ramadan = GetPeriod(BeachCampColumnId.Ramadan);

                Literal literalSection = (Literal)e.Item.FindControl("literalSection");
                literalSection.Text = rowView["Title"].ToString();

                //Literal literalPeriod1 = (Literal)e.Item.FindControl("literalPeriod1");
                //literalPeriod1.Text = rowView["Period1"].ToString();

                CheckBox chkPeriod1 = (CheckBox)e.Item.FindControl("chkPeriod1");
                chkPeriod1.Text = rowView["Period1"].ToString();
                chkPeriod1.ToolTip = rowView["Title"].ToString() + " - " + period1;

                //Literal literalPeriod2 = (Literal)e.Item.FindControl("literalPeriod2");
                //literalPeriod2.Text = rowView["Period2"].ToString();

                CheckBox chkPeriod2 = (CheckBox)e.Item.FindControl("chkPeriod2");
                chkPeriod2.Text = rowView["Period2"].ToString();
                chkPeriod2.ToolTip = rowView["Title"].ToString() + " - " + period2;

                //Literal literalFullDay = (Literal)e.Item.FindControl("literalFullDay");
                //literalFullDay.Text = rowView["FullDay"].ToString();

                CheckBox chkFullDay = (CheckBox)e.Item.FindControl("chkFullDay");
                chkFullDay.Text = rowView["FullDay"].ToString();
                chkFullDay.ToolTip = rowView["Title"].ToString() + " - " + fullDay;

                //Literal literalRamadan = (Literal)e.Item.FindControl("literalRamadan");
                //literalRamadan.Text = rowView["Ramadan"].ToString();

                CheckBox chkRamadan = (CheckBox)e.Item.FindControl("chkRamadan");
                chkRamadan.Text = rowView["Ramadan"].ToString();
                chkRamadan.ToolTip = rowView["Title"].ToString() + " - " + ramadan;
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.Page.IsValid)
                return;

            string output = AddBeachCampEvent();

            if (!string.IsNullOrEmpty(output))
            {
                ShowErrorMessages(output);
            }
            else
            {
                this.Page.Response.Clear();
                this.Page.Response.Write(
                string.Format(System.Globalization.CultureInfo.InvariantCulture, @"<script type='text/javascript'> window.frameElement.commonModalDialogClose(1, '{0}');</script>", ""));
                this.Page.Response.End();
            }
        }

        #endregion Events

        #region Functions

        private void ShowErrorMessages(string message)
        {

        }

        private string GetPeriod(Guid period)
        {
            SPList list = Utility.GetListFromURL("/Lists/BCPrices", SPContext.Current.Web);
            SPField field = list.Fields[period];
            if (field != null)
                return field.Title;
            return string.Empty;
        }

        private string AddBeachCampEvent()
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
                        sectionPeriod += chkPeriod1.ToolTip + " - " + chkPeriod1.Text + "|";
                        totalPrice += double.Parse(chkPeriod1.Text);
                    }

                    CheckBox chkPediod2 = (CheckBox)prices.FindControl("chkPeriod2");
                    if (chkPediod2 != null && chkPediod2.Checked)
                    {
                        sectionPeriod += chkPediod2.ToolTip + " - " + chkPeriod1.Text + "|";
                        totalPrice += double.Parse(chkPediod2.Text);
                    }

                    CheckBox chkFullDay = (CheckBox)prices.FindControl("chkFullDay");
                    if (chkFullDay != null && chkFullDay.Checked)
                    {
                        sectionPeriod += chkFullDay.ToolTip + " - " + chkPeriod1.Text + "|";
                        totalPrice += double.Parse(chkFullDay.Text);
                    }

                    CheckBox chkRamadan = (CheckBox)prices.FindControl("chkRamadan");
                    if (chkRamadan != null && chkRamadan.Checked)
                    {
                        sectionPeriod += chkRamadan.ToolTip + " - " + chkPeriod1.Text + "|";
                        totalPrice += double.Parse(chkRamadan.Text);
                    }
                }

                SPListItem item = SPContext.Current.List.AddItem();
                item[SPBuiltInFieldId.Title] = literalEmployeeName.Text;
                item["EmployeeCode"] = literalID.Text;
                item["Department"] = literalDepartment.Text;
                item["Section"] = literalSection.Text;
                item["OfficeTel"] = literalOfficeTel.Text;
                item["Mobile"] = literalMobile.Text;
                item[SPBuiltInFieldId.StartDate] = beachCampDate;
                item[SPBuiltInFieldId.EndDate] = beachCampDate.AddDays(int.Parse(ffRequireDay.Value.ToString()));
                item["Reason"] = ffReason.Value;
                item["RequireDay"] = ffRequireDay.Value;
                item["TotalPrice"] = totalPrice;
                item[SPBuiltInFieldId.Location] = sectionPeriod;
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
