using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.ViewModels
{
    public class ReportExModel
    {
        public string UserID { set; get; }
        public string EmployeeID { set; get; }
        public string FullName { set; get; }
        public string TotalEntitleDay { set; get; }
        public float RemainEntitleDayAtBeginningOfPeriod { set; get; }
        public float RemainEntitleDay { set; get; }
        //public int? GroupID { set; get; }
        //public string GroupName { set; get; }
    }
}
