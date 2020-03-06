using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMS.Web.Models.EntitleDayManagement
{
    public class EntitleDayManagementViewModel
    {
        public int ID { set; get; }
        public string HolidayType { set; get; }
        public string UnitType { set; get; }
        public int MaxEntitleDay { set; get; }
        public string Description { set; get; }
        public bool Status { set; get; }
    }
}