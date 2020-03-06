using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TMS.Model.Abstract;
using TMS.Model.Models;
using TMS.Web.Models.AbnormalCase;
using TMS.Web.Models.Common;
using TMS.Web.Models.Request;

namespace TMS.Web.Models.Explanation
{
    [Serializable]
    public class ExplanationRequestViewModel
    {

        public DateTime? CreatedDate { set; get; }
        public DateTime ExplanationDate { set; get; }
        public string CreatedBy { set; get; }
        public virtual AppUserViewModel User { set; get; }
        public virtual AppUserViewModel Receiver { set; get; }

        public DateTime? UpdatedDate { set; get; }

        public string UpdatedBy { set; get; }
        public string FullName { set; get; }

        public bool Status { set; get; }

        public int ID { set; get; }
        
        public string Title { get; set; }
        
        public string ReasonDetail { get; set; }
        public string Actual { get; set; }
        public string ReasonList { get; set; }
        public int StatusRequestId { get; set; }
        public int TimeSheetId { get; set; }
        public string ApproverId { get; set; }
        public string ReceiverId { get; set; }
        public string DelegateId { get; set; }
        public string GroupName { get; set; }
        public string GroupId { get; set; }
        public string DateExplanation { get; set; }
        public string FullNameDelegate { get; set; }
		public bool IsExpiredDate { get; set; }
        public virtual AppUserViewModel Delegate { set; get; }

        public virtual StatusRequestViewModel StatusRequest { set; get; }
        
        public virtual AppUserViewModel Approver { set; get; }
        

        public virtual AbnormalCaseViewModel AbnormalCase { set; get; }

        public virtual List<AbnormalCaseReasonViewModel> AbnormalReason { set; get; }
        
    }
}