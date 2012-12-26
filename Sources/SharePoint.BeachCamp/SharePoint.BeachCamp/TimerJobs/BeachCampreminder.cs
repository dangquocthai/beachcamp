using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using SharePoint.BeachCamp.Util.Utilities;
using SharePoint.BeachCamp.Util;
using SharePoint.BeachCamp.Util.Helpers;


namespace SharePoint.BeachCamp.TimerJobs
{
    public class BeachCampReminder : SPJobDefinition
    {
        public static string BEACH_CAMP_JOB_NAME = "[BeachCamp] - Beach Camp Reminder";

        public BeachCampReminder() : base() { }

        public BeachCampReminder(string jobName, SPService service)
            : base(jobName, service, null, SPJobLockType.None)
        {
            this.Title = jobName;
        }

        public BeachCampReminder(SPWebApplication webApp) :
            base(BeachCampReminder.BEACH_CAMP_JOB_NAME, webApp, null, SPJobLockType.Job) { }

        protected override bool HasAdditionalUpdateAccess()
        {
            return true;
        }

        public override void Execute(Guid targetInstanceId)
        {
            try
            {
                SPWebApplication webApplication = this.Parent as SPWebApplication;
                for (int i = 0; i < webApplication.Sites.Count; i++)
                {
                    foreach (SPWeb web in webApplication.Sites[i].AllWebs)
                    {
                        SPList beachCamp = Utility.GetListFromURL(Constants.BEACH_CAMP_CALENDAR_LIST_URL, web);
                        if (beachCamp != null)
                        {
                            string calm = string.Format(@"<Where>
                                                        <And>
                                                            <Eq>
                                                                <FieldRef Name='GSApproval' />
                                                                <Value Type='Text'>Approved</Value>
                                                            </Eq>
                                                            <And>
                                                                <Eq>
                                                                    <FieldRef Name='IsPaid' />
                                                                    <Value Type='Boolean'>0</Value>
                                                                </Eq>
                                                                <And>
                                                                    <Geq>
                                                                        <FieldRef Name='EventDate' />
                                                                        <Value Type='DateTime'>{0}</Value>
                                                                    </Geq>
                                                                    <Leq>
                                                                        <FieldRef Name='EventDate' />
                                                                        <Value Type='DateTime'>{1}</Value>
                                                                    </Leq>
                                                                </And>
                                                            </And>
                                                        </And>
                                                    </Where>", DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.AddDays(15).ToString("yyyy-MM-dd"));

                            SPQuery spQuery = new SPQuery();
                            spQuery.Query = calm;

                            SPListItemCollection itemCollections = beachCamp.GetItems(spQuery);
                            string url = web.Site.MakeFullUrl(beachCamp.DefaultViewUrl);
                            foreach (SPListItem item in itemCollections)
                            {
                                SPUser creator = ((SPFieldUserValue)(item.Fields["Created By"]).GetFieldValue(item["Created By"].ToString())).User;
                                DateTime eventDate = Convert.ToDateTime(item["EventDate"].ToString());
                                if (eventDate <= DateTime.Now.AddDays(10))
                                {
                                    BeachCampHelper.SendEmail(web, creator.Email, item, url, MailType.Cancel);
                                    item["GSApproval"] = TaskResult.Rejected;
                                    item.SystemUpdate();
                                }
                                else
                                {
                                    BeachCampHelper.SendEmail(web, creator.Email, item, url, MailType.Notify);
                                }
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.LogError(ex.Message, BeachCampFeatures.BeachCamp);
            }
        }

        /*
        private void SyncTimesheets(SPWeb web, SPList list)
        {
            try
            {
                SPListCollection timesheets = web.Lists;
                foreach (SPList timesheet in timesheets)
                {
                    if (timesheet.BaseTemplate == SPListTemplateType.Events)
                    {
                        if (timesheet.Title != list.Title)
                            Reminder(timesheet, list);

                    }
                }
            }
            catch (Exception ex)
            {
                Utility.LogError(ex.Message, "BeachCampReminder");
            }
        }

        private void Reminder(SPList timesheet, SPList list)
        {
            try
            {
                DateTime queryDate = DateTime.Now;
                string caml = Camlex.Query()
                              .Where(x => (DateTime)x["EventDate"] > queryDate.AddDays(-6).IncludeTimeValue()
                              && (DateTime)x["EventDate"] < queryDate.IncludeTimeValue()
                              && x["IsSync"] == (DataTypes.Text)"False"
                              && x["_ModerationStatus"] == (DataTypes.ModStat)"Approved")
                              .OrderBy(x => new[] { x["EventDate"] as Camlex.Asc })
                              .ToString();
                var query = new CAMLListQuery<TimesheetItem>(timesheet);
                List<TimesheetItem> tiemsheetItems = query.ExecuteListQuery(caml);
                foreach (var timesheetItem in tiemsheetItems)
                {
                    //Update status
                    TimesheetService.UpdateTimesheet(timesheet, timesheetItem.ID);
                    //Add to timesheet list
                    SPListItem item = list.AddItem();
                    item[SPBuiltInFieldId.Title] = timesheetItem.Title;
                    item[IOfficeColumnId.Timesheet.TimesheetTask] = timesheetItem.TimesheetTask;
                    item[IOfficeColumnId.Timesheet.TypeOfWork] = timesheetItem.TypeOfWork;
                    item[SPBuiltInFieldId.StartDate] = timesheetItem.EventDate;
                    item[SPBuiltInFieldId.EndDate] = timesheetItem.EndDate;
                    item[SPBuiltInFieldId.Comments] = timesheetItem.Comments;
                    item[IOfficeColumnId.Timesheet.WorkTime] = timesheetItem.WorkTime;
                    item[IOfficeColumnId.Timesheet.Employee] = timesheetItem.EmployeeId;
                    item[IOfficeColumnId.Timesheet.DepartmentSiteColumn] = timesheetItem.DepartmentId;
                    item[IOfficeColumnId.Timesheet.Year] = timesheetItem.Year;
                    item[IOfficeColumnId.Timesheet.Month] = timesheetItem.Month;
                    item[IOfficeColumnId.Timesheet.Week] = timesheetItem.Week;
                    item.Update();
                }
            }
            catch (Exception ex)
            {
                Utility.LogError(ex.Message, "TimesheetJob");
            }
        }

         */

        private bool IsFeatureActivated(SPWeb parentWeb, string featureID)
        {
            bool isActivated = false;

            try
            {
                foreach (SPFeature feature in parentWeb.Site.Features)
                {
                    if (feature.DefinitionId.Equals(new Guid(featureID)))
                    {
                        isActivated = true;
                        break;
                    }
                    else
                    {
                        isActivated = false;
                    }
                }
            }
            catch (Exception ex)
            {
                isActivated = false;
                Utility.LogError(ex.Message, "BeachCampReminder");
            }
            return isActivated;
        }

    }
}
