using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.ViewModels
{
    public class ReportModel
    {
        public int ID { set; get; }
        public string UserID { set; get; }
        //public string UserNo { set; get; }
        public string FullName { set; get; }
        public float TotalEntitleYear { set; get;}
        public float RemainEntitleDayAtBeginningOfPeriod { set; get; }
        public float TotalAuthorizedLeavesInPeriod { set; get; }
        public double WorkingDaysFromFingerPrint { set; get; }
        public double WorkingDaysToCalculateSalary { set; get; }
        public float Remain { set; get; }
    }
}
