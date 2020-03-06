using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMS.Web.Models.EntitleDay
{
    public class EntitleDay_AppUserModel
    {
        public int EntitleDayAppUserId { set; get; }
        public string FullName { set; get; }
        public string UserName { set; get; }
        public string HolidayType { get; set; }
        public string UnitType { set; get; }
        public float MaxEntitleDay { set; get; }
        public float NumberDayOff { set; get; }
        public float RemainDayOff { set; get; }
        public string Note { set; get; }
    }
}