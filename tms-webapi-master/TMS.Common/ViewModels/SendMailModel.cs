using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.ViewModels
{
    public class SendMailModel
    {
        public string[] toEmail { get; set; }
        public string[] ccMail { get; set; }
        public string Title { get; set; }
        public string CreateBy { get; set; }
        public string Action { get; set; }
        public string EmailType { get; set; }
        public string EmailSubject { get; set; }
        public string ReasonReject { get; set; }
        public string Category { get; set; }

        //Request
        public string GroupName { get; set; }
        public string RequestTypeName { get; set; }
        public string DetailReason { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        //OT Request
        public string OTDate { get; set; }
        public string OTDateType { get; set; }
        public string OTTimeType { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        //Abnormal
        public string ExplanationDate { get; set; }
        public string ExplanationReason { get; set; }
        public string Actual { get; set; }
        public string Description { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
    }
}
