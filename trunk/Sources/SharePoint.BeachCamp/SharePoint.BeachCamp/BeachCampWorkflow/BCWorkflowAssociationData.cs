using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharePoint.BeachCamp.BeachCampWorkflow
{
    [Serializable]
    public class BCWorkflowAssociationData
    {
        public string GeneralSupervisor { get; set; }

        public string TaskTitle { get; set; }

        public string Message { get; set; }
    }
}
