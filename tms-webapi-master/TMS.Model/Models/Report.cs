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
    [Table(CommonConstants.Report)]
    public class Report : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }
        public int TimeSheetID { set; get; }
        public float LeaveMounth { set; get; }
        public DateTime DateCheckRequest { set; get; }
        [ForeignKey("TimeSheetID")]
        public virtual FingerTimeSheet FingerTimeSheet { set; get; }
    }
}
