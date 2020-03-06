using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMS.Web.Models.EntitleDay
{
    public class EntitleDayViewModel
    {
        public string FullName { set; get; }
        public string Account { set; get; }
        public string DayOffType { get; set; }
        public string Unit { set; get; }
        public float MaximumAllowed { set; get; }
        public float Approved { set; get; }
        public float Remain { set; get; }
    }
}