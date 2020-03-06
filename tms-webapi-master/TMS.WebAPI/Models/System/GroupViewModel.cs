using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TMS.Common.Constants;

namespace TMS.Web.Models
{
    public class GroupViewModel
    {
        public int? ID { set; get; }
        [MaxLength(25)]
        public string Name { get; set; }
        [MaxLength(128)]
        public string Description { get; set; }
        [MaxLength(128)]
        public string GroupLead { get; set; }
        [MaxLength(128)]
        public string GroupLeadID { get; set; }
        public string GroupLeadAccount { get; set; }
        public string DelegateId { set; get; }
        public DateTime? StartDate { set; get; }

        public DateTime? EndDate { set; get; }
    }
}