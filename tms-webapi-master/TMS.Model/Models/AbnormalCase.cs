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
    [Table(CommonConstants.AbnormalCases)]
    public class AbnormalCase : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }
        public int TimeSheetID { set; get; }
        public int ReasonId { set; get; }
        [ForeignKey("ReasonId")]
        public virtual AbnormalReason AbnormalReason { set; get; }
        [ForeignKey("TimeSheetID")]
        public virtual FingerTimeSheet FingerTimeSheet { set; get; }
    }
}
