using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.ViewModels
{
     public class FingerTimeSheetExcel
    {
        public string FullName { set; get; }
        public string Date { set; get; }
        public string CheckIn { set; get; }
        public string CheckOut { set; get; }
        public string OTCheckIn { set; get; }
        public string OTCheckOut { set; get; }
        public string LateTime { set; get; }
        public string LeaveEarly { set; get; }
        public string Absent { set; get; }
        public double WorkingDay { set; get; }
        public string Allowance { set; get; }
        public string Status { set; get; }
    }
}
