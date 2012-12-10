using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharePoint.BeachCamp.Util.Helper.DocXGenerator
{
    public class BeachCampReport : IReportBase
    {
        [PlaceHolder("EmployeeName", PlaceHolderType.NonRecursive)]
        public string EmployeeName { get; set; }

        [PlaceHolder("EmployeeCode", PlaceHolderType.NonRecursive)]
        public string EmployeeCode { get; set; }

        [PlaceHolder("Department ", PlaceHolderType.NonRecursive)]
        public string Department { get; set; }

        [PlaceHolder("Section ", PlaceHolderType.NonRecursive)]
        public string Section { get; set; }

        [PlaceHolder("OfficeTel ", PlaceHolderType.NonRecursive)]
        public string OfficeTel { get; set; }

        [PlaceHolder("Mobile ", PlaceHolderType.NonRecursive)]
        public string Mobile { get; set; }


        [PlaceHolder("Reason ", PlaceHolderType.NonRecursive)]
        public string Reason { get; set; }

        [PlaceHolder("RequireDay ", PlaceHolderType.NonRecursive)]
        public string RequireDay { get; set; }

        [PlaceHolder("EventDate ", PlaceHolderType.NonRecursive)]
        public string EventDate { get; set; }


        [PlaceHolder("TotalPrice ", PlaceHolderType.NonRecursive)]
        public string TotalPrice { get; set; }

        [PlaceHolder("Location ", PlaceHolderType.NonRecursive)]
        public string Location { get; set; }

        [PlaceHolder("GSApproval ", PlaceHolderType.NonRecursive)]
        public string GSApproval { get; set; }

        [PlaceHolder("GSApprovalComment ", PlaceHolderType.NonRecursive)]
        public string GSApprovalComment { get; set; }

        [PlaceHolder("IsPaid ", PlaceHolderType.NonRecursive)]
        public string IsPaid { get; set; }

    }
}
