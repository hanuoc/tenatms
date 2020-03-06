using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TMS.Common.Constants;

namespace TMS.Web.Models.Group
{
    public class GroupCreateUpdateModel
    {
        public int ID { set; get; }
        [Required(ErrorMessage = MessageSystem.RequireGroupName)]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(128)]
        public string Description { get; set; }
        [MaxLength(128)]
        public string GroupLead { get; set; }
        [MaxLength(128)]
        [Required(ErrorMessage = MessageSystem.RequireGroupLead)]
        public string GroupLeadID { get; set; }
        public string DelegateId { set; get; }
        public DateTime? StartDate { set; get; }

        public DateTime? EndDate { set; get; }
    }
}