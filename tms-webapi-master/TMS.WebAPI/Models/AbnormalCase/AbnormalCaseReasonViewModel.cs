using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMS.Web.Models.AbnormalCase
{
    public class AbnormalCaseReasonViewModel
    {
        public int AbnormalId { get; set; }
        public int ReasonId { get; set; }

        public string ReasonName { get; set; }
        public virtual AbnormalCaseViewModel AbnormalCase { set; get; }
        public virtual ReasonTypeViewModel ReasonType { set; get; }

        public DateTime? CreatedDate { set; get; }
        
        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }
        
        public string UpdatedBy { set; get; }

        public bool Status { set; get; }
    }
}