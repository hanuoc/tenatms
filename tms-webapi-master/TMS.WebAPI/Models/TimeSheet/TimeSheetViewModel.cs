using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMS.Web.Models.TimeSheet
{
    public class TimeSheetViewModel
    {
        public int ID { set; get; }
        public string UserID { set; get; }
        public DateTime DayOfCheck { set; get; }
        public string CheckIn { set; get; }
        public string CheckOut { set; get; }
        public string Absent { set; get; }
        public virtual AppUserViewModel AppUser { set; get; }
    }
}