using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.ViewModels
{
    public class AbnormalCaseModel
    {
        public int ID { set; get; }
        public int TimeSheetID { get; set; }
        public string UserID { set; get; }
        public string UserName { set; get; }
        public string FullName { set; get; }
        public string GroupName { set; get; }
        public DateTime? AbnormalDate { set; get; }
        //public string AbnormalReasonDay { set; get; }
        public string Absent { set; get; }
        public string CheckIn { set; get; }
        public string CheckOut { set; get; }
        public double Workingtime { set; get; }
        public string ReasonList { set; get; }
        public string Approver { set; get; }
        public string StatusRequest { set; get; }
        public string Email { set; get; }
        public int StatusrequestId { set; get; }
        public List<AbnormalReasonModel> AbnormalReason { get; set; }

    }
}
