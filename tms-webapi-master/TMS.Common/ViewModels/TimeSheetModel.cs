using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.ViewModels
{
    public class TimeSheetModel
    {
        public int ID { set; get; }
        public string UserID { set; get; }
        public string UserName { set; get; }
        public DateTime DayOfCheck { set; get; }
        public string CheckIn { set; get; }
        public string CheckOut { set; get; }
        public bool ComeLate { set; get; }
        public bool ComeBackSoon { set; get; }
        public string Absent { set; get; }
        public double NumOfWorkingDay { set; get; }
        public string MinusAllowance { set; get; }
        public bool Explanation { set; get; }
        public string ApproverName { set; get; }
        public string StatusExplanation { set; get; }
    }
}
