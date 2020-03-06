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
    [Table(CommonConstants.Requests)]
    public class Request : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [Required]
        [MaxLength(256)]
        public string Title { set; get; }

        [Required]
        [MaxLength((128))]
        public string UserId { set; get; }

        [MaxLength((128))]
        public string DelegateId { set; get; }

        public int? EntitleDayId { set; get; }
        public int RequestTypeId { set; get; }

        public int RequestStatusId { set; get; }

        [MaxLength(500)]
        public string DetailReason { set; get; }
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }

        [MaxLength((128))]
        public string ChangeStatusById { set; get; }

        [MaxLength((128))]
        public string AssignToId { set; get; }

        [ForeignKey("EntitleDayId")]
        public virtual EntitleDay EntitleDay { set; get; }

        [ForeignKey("RequestTypeId")]
        public virtual RequestType RequestType { set; get; }

        [ForeignKey("UserId")]
        public virtual AppUser AppUser { set; get; }
        [ForeignKey("DelegateId")]
        public virtual AppUser AppUserDelegate { set; get; }

        [ForeignKey("ChangeStatusById")]
        public virtual AppUser AppUserChangeStatus { set; get; }

        [ForeignKey("AssignToId")]
        public virtual AppUser AppUserAssign { set; get; }
        [ForeignKey("RequestStatusId")]
        public virtual StatusRequest StatusRequest { set; get; }


    }
}
