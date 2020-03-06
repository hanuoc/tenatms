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
    [Table(CommonConstants.OTRequests)]
    public class OTRequest : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [Required]
        [MaxLength(256)]
        public string Title { set; get; }

        [Required]
        public DateTime? OTDate { set; get; }
        [MaxLength(8)]
        public string StartTime { set; get; }
        [MaxLength(8)]
        public string EndTime { set; get; }

        [Required]
        public int OTDateTypeID { set; get; }

        [Required]
        public int OTTimeTypeID { set; get; }

        [Required]
        public int StatusRequestID { set; get; }

        [MaxLength(128)]
        public string UserAssignedID { set; get; }

        [ForeignKey("OTDateTypeID")]
        public virtual OTDateType OTDateType { set; get; }

        [ForeignKey("OTTimeTypeID")]
        public virtual OTTimeType OTTimeType { set; get; }

        [ForeignKey("StatusRequestID")]
        public virtual StatusRequest StatusRequest { set; get; }

        [ForeignKey("UserAssignedID")]
        public virtual AppUser AppUserAssigned { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual AppUser AppUserCreatedBy { get; set; }

        [ForeignKey("UpdatedBy")]
        public virtual AppUser AppUserUpdatedBy { get; set; }
        public ICollection<OTRequestUser> OTRequestUser { get; set; }
    }
}
