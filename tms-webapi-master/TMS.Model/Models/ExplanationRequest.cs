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
    [Table(CommonConstants.ExplanationRequests)]
    public class ExplanationRequest : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [Required]
        [MaxLength(256)]
        public string Title { get; set; }

        [Required]
        public int StatusRequestId { get; set; }

        [MaxLength(2000)]
        public string ReasonDetail { get; set; }
        [MaxLength(15)]
        public string Actual { get; set; }
        public int TimeSheetId { get; set; }
        public string ReceiverId { get; set; }
        public string DelegateId { get; set; }

        [ForeignKey("StatusRequestId")]
        public virtual StatusRequest StatusRequest { set; get; }

        [ForeignKey("DelegateId")]
        public virtual AppUser Delegate { set; get; }

        [ForeignKey("ReceiverId")]
        public virtual AppUser Receiver { set; get; }

        [ForeignKey("TimeSheetId")]
        public virtual FingerTimeSheet FingerTimeSheet { set; get; }

        [ForeignKey("CreatedBy")]
        public virtual AppUser AppUserCreatedBy { get; set; }

        [ForeignKey("UpdatedBy")]
        public virtual AppUser AppUserUpdatedBy { get; set; }

    }
}
