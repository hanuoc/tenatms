using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.ViewModels
{
    public class FingerTimeSheetModel
    {
        public int ID { set; get; }
        public string UserNo { set; get; }
        public string UserName { set; get; }
        public string UserId { set; get; }
        public DateTime DayOfCheck { set; get; }
        public string CheckIn { set; get; }
        public string CheckOut { set; get; }
        public string Late { set; get; }
        public string LeaveEarly { set; get; }
        public string OTCheckIn { set; get; }
        public string OTCheckOut { set; get; }
        public string Absent { set; get; }
        public double NumOfWorkingDay { set; get; }
        public string MinusAllowance { set; get; }
        public bool Explanation { set; get; }
        public string ApproverName { set; get; }
        public string StatusExplanation { set; get; }
    }
}
