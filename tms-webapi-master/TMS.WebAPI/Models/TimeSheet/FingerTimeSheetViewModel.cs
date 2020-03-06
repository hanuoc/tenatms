using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TMS.Web.Models.TimeSheet
{
    public class FingerTimeSheetViewModel
    {
       
        public int ID { set; get; }
        public string UserNo { set; get; }
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
        public virtual FingerMachineUserViewModel FingerMachineUsers { set; get; }
    }
}