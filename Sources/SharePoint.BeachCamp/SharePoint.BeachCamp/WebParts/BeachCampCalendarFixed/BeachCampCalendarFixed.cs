using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace SharePoint.BeachCamp.WebParts.BeachCampCalendarFixed
{
    [ToolboxItemAttribute(false)]
    public class BeachCampCalendarFixed : WebPart
    {
        protected override void CreateChildControls()
        {
            this.Controls.Add(new Literal() { Text = "<script src='/_layouts/1033/jquery-1.8.2.min.js' type='text/javascript'></script>" });
            this.Controls.Add(new Literal() { Text = "<script src='/_layouts/1033/fixed-calendar.js' type='text/javascript'></script>" });
            this.Controls.Add(new Literal() { Text = "<script src='/_layouts/1033/colour-calendar.js' type='text/javascript'></script>" });
            DateTime nextMonth = DateTime.Now.AddMonths(1);
            this.Controls.Add(
                    new Literal()
                    {
                        Text = string.Format(@"<script language='javascript' type='text/javascript'>
                                                        $(window).load(function() {0}
                                                            $('#WPQ1_nav_prev_a').parent().hide();
                                                            $('#WPQ1_nav_next_a').parent().hide();
                                                            $('#WPQ2_nav_prev_a').parent().hide();
                                                            $('#WPQ2_nav_next_a').parent().hide();
                                                            MoveToDate('{1}','WPQ2');
                                                            $('td').removeAttr('evtid');
                                                            $('th').removeAttr('evtid');
                                                        {2});
                                                        $(document).ready(function() {0}
                                                            setTimeout(function(){0}
                                                                $('td').removeAttr('evtid');
                                                                $('th').removeAttr('evtid');
                                                            {2},800);
                                                            
                                                        {2});
                                                    </script>", "{", nextMonth.ToString("yyyy-MM-dd"), "}")
                    }
                    );
        }
    }
}
