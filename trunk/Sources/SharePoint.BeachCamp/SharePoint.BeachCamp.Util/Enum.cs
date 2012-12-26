using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharePoint.BeachCamp.Util
{
    public enum TaskResult
    {
        Draft,
        Pending,
        Approved,
        Rejected
    }

    public enum MailType
    {
        Notify,
        Cancel
    }

    public enum CalendarOverlayColor
    {
        LightYellow = 1,
        LightGreen = 2,
        Orange = 3,
        LightTurquise = 4,
        Pink = 5,
        LightBlue = 6,
        IceBlue1 = 7,
        IceBlue2 = 8,
        White = 9
    }
}
