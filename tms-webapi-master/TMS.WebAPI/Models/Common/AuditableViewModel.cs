using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMS.Web.Models.Common
{
    public class AuditableViewModel
    {
        public DateTime? CreatedDate { set; get; }
        
        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }
        
        public string UpdatedBy { set; get; }

        public bool Status { set; get; }
    }
}