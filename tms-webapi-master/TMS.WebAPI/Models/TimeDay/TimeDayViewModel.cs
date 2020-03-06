using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMS.Web.Models.TimeDay
{
    public class TimeDayViewModel
    {
        public int ID { set; get; }
        public string Workingday { set; get; }
        public string CheckIn { set; get; }
        public string CheckOut { set; get; }
    }
}