using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Common.Constants;
using TMS.Model.Abstract;

namespace TMS.Model.Models
{
    [Table(CommonConstants.OTRequestUsers)]
    public class OTRequestUser : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [Required]
        public int OTRequestID { set; get; }

        [Required]
        [MaxLength(256)]
        public string UserID { set; get; }

        [ForeignKey("OTRequestID")]
        public virtual OTRequest OTRequest { get; set; }

    }
   
}
