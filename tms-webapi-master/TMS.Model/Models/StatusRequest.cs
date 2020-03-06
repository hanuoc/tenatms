using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Common.Constants;

namespace TMS.Model.Models
{
    [Table(CommonConstants.StatusRequests)]
    public class StatusRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; } 

        [MaxLength(256)]
        public string Name { set; get; }

        public IEnumerable<Request> Requests { set; get; }
        public IEnumerable<ExplanationRequest> ExplanationRequests { set; get; }
    }
}
