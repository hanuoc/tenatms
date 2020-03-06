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
    [Table(CommonConstants.JobLog)]
    public class JobLog
    {
        [Key]
        public DateTime Date { set; get; }
        public bool ImportTimeSheet { set; get; }
        public bool ChangeStatus { set; get; }
        public bool UpdateEntitleDay { set; get; }
        public bool ResetEntitleDay { set; get; } 
    }
}
