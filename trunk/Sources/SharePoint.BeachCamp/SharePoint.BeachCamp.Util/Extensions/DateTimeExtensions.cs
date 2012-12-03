using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharePoint.BeachCamp.Util.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime FirstDayOfMonthFromDateTime(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public static DateTime LastDayOfMonthFromDateTime(this DateTime dateTime)
        {
            DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        }
    }
}
