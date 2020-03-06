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
    [Table(CommonConstants.FingerMachineUsers)]
    public class FingerMachineUser
    {
        [Key]
        [MaxLength(50)]
        public string ID { set; get; }
        [MaxLength(128)]
        public string UserId { set; get; }
        [ForeignKey("UserId")]
        public virtual AppUser AppUser { set; get; }
    }
}
