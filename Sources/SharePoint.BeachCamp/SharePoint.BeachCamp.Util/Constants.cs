using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharePoint.BeachCamp.Util
{
    public class Constants
    {
        public const string BEACH_CAMP_CALENDAR_LIST_URL = "/Lists/BCCalendar";
        public const string BEACH_CAMP_PRICE_LIST_URL = "/Lists/BCPrices";
        public const string BEACH_CAMP_TASK_LIST_URL = "/Lists/BeachCampTask";
        public const string BEACH_CAMP_EMAIL_TEMPLATE_LIST_URL = "/Lists/EmailTemplates";
        
        public const string APPROVE_STATUS = "TaskStatus";
        public const string APPROVE_MESSAGE = "ApproveMessage";

        public const string TITLE_COLOUR_FORMAT = "|||{0}|||{1}";

        public const string PERIOD_1ST = "07:00-16:30";
        public const string PERIOD_2ST = "17:30-02:00";
        public const string PERIOD_FULLDAY = "07:00-02:00";
        public const string PERIOD_RAMADAN = "15:00-04:00";

        public const string ERROR_MESSAGE = "* indicates a required field";
        public const string ERROR_MESSAGE1 = "You can only reserve beach camp one a 60 days. Please select another day!";
        public const string ERROR_MESSAGE2 = "This section is not available. Please choose another one!";
        public const string ERROR_MESSAGE3 = "Please choose a Section and Period!";

        public const string BEACH_CAMP_ADMIN_GROUP = "Beach Camp General Supervisor";
    }
}
