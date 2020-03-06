using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMS.Web.Models.AbnormalCase
{
    public class ReasonTypeViewModel
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public IEnumerable<AbnormalCaseViewModel> AbnormalCase { set; get; }
        public ReasonTypeViewModel()
        {
            this.AbnormalCase = new HashSet<AbnormalCaseViewModel>();
        }
        public DateTime? CreatedDate { set; get; }
        
        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }
        
        public string UpdatedBy { set; get; }

        public bool Status { set; get; }
    }
}
