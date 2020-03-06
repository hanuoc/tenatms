using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TMS.Common.ViewModels;
using TMS.Web.Models.Request;
using TMS.Web.Models.TimeSheet;

namespace TMS.Web.Models.AbnormalCase
{
    public class AbnormalCaseViewModel
    {
        public int ID { set; get; }
        public string UserID { set; get; }
        public string UserName { set; get; }
        public string FullName { set; get; }
        public string GroupName { set; get; }
        public int TimeSheetId { get; set; }
        public DateTime AbnormalDate { set; get; }
        //public string AbnormalReasonDay { set; get; }
        public string Absent { set; get; }
        public string CheckIn { set; get; }
        public string CheckOut { set; get; }
        public string Workingtime { set; get; }
        public string Approver { set; get; }
        public string StatusRequest { set; get; }

        public string ReasonList { get; set; }

        public int StatusrequestId { set; get; }
        public List<AbnormalReasonModel> AbnormalReason { get; set; }


    }
}