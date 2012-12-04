using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using SharePoint.BeachCamp.Util.Utilities;
using SharePoint.BeachCamp.Util.Extensions;
using System.Web.UI.WebControls;

namespace SharePoint.BeachCamp.Util.Helpers
{
    public class BeachCampHelper
    {
        public static string GetPrices(Repeater repeaterPrices, SPWeb web)
        {
            string output = string.Empty;
            try
            {
                SPList priceList = Utility.GetListFromURL("/Lists/BCPrices", web);
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

        public static string GetPeriod(Guid period, SPWeb web)
        {
            try
            {
                SPList list = Utility.GetListFromURL("/Lists/BCPrices", web);
                SPField field = list.Fields[period];
                if (field != null)
                    return field.Title;
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static bool IsUserReserved(SPWeb web, string employeeCode, DateTime date)
        {
           string caml = string.Format(@"<Where>
                                            <And>
                                                <Eq>
                                                    <FieldRef Name='EmployeeCode' />
                                                    <Value Type='Text'>{0}</Value>
                                                </Eq>
                                                <And>
                                                    <Geq>
                                                        <FieldRef Name='EventDate' />
                                                        <Value Type='DateTime'>{1}</Value>
                                                    </Geq>
                                                    <Leq>
                                                        <FieldRef Name='EventDate' />
                                                        <Value Type='DateTime'>{2}</Value>
                                                    </Leq>
                                                </And>
                                            </And>
                                        </Where>
                                        <OrderBy>
                                            <FieldRef Name='EventDate' Ascending='False' />
                                        </OrderBy>", employeeCode, date.FirstDayOfMonthFromDateTime().ToString("yyyy-MM-dd"), date.LastDayOfMonthFromDateTime().ToString("yyyy-MM-dd"));

            return IsUserReserved(web, caml);
        }

        public static bool IsUserReserved(SPWeb web, string employeeCode, DateTime date, int id)
        {
            string caml = string.Format(@"<<Where>
                                            <And>
                                                <Eq>
                                                    <FieldRef Name='ID' />
                                                    <Value Type='Counter'>{0}</Value>
                                                </Eq>
                                                <And>
                                                    <Eq>
                                                        <FieldRef Name='EmployeeCode' />
                                                        <Value Type='Text'>{1}</Value>
                                                    </Eq>
                                                    <And>
                                                        <Geq>
                                                            <FieldRef Name='EventDate' />
                                                            <Value Type='DateTime'>{2}</Value>
                                                        </Geq>
                                                        <Leq>
                                                            <FieldRef Name='EventDate' />
                                                            <Value Type='DateTime'>{3}</Value>
                                                        </Leq>
                                                    </And>
                                                </And>
                                            </And>
                                        </Where>
                                        <OrderBy>
                                            <FieldRef Name='EventDate' Ascending='False' />
                                        </OrderBy>", id, employeeCode, date.FirstDayOfMonthFromDateTime().ToString("yyyy-MM-dd"), date.LastDayOfMonthFromDateTime().ToString("yyyy-MM-dd"));

            return IsUserReserved(web, caml);
        }

        private static bool IsUserReserved(SPWeb web, string caml)
        {
            bool output = false;
            try
            {
                SPList beachCampCalendar = Utility.GetListFromURL("/Lists/BCCalendar", web);
                SPQuery spQuery = new SPQuery();
                spQuery.Query = caml;
                SPListItemCollection itemCollections = beachCampCalendar.GetItems(spQuery);
                if (itemCollections != null && itemCollections.Count > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return output;
        }

    }

}
