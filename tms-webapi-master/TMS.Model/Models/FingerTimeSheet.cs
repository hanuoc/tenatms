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
    [Table(CommonConstants.FingerTimeSheets)]
    public class FingerTimeSheet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }
        [Required]
        [MaxLength(50)]
        public string UserNo { set; get; }
        //[Required]
        //[MaxLength(50)]
        //public string AccName { set; get; }
        [Required]
        public DateTime DayOfCheck { set; get; }
        [MaxLength(8)]
        public string CheckIn { set; get; }
        [MaxLength(8)]
        public string CheckOut { set; get; }
        [MaxLength(8)]
        public string Late{ set; get; }
        [MaxLength(8)]
        public string LeaveEarly { set; get; }
        [MaxLength(8)]
        public string OTCheckIn { set; get; }
        [MaxLength(8)]
        public string OTCheckOut { set; get; }
        [MaxLength(8)]
        public string Absent { set; get; }
        public double NumOfWorkingDay { set; get; }
        [MaxLength(8)]
        public string MinusAllowance { set; get; }
        [ForeignKey("UserNo")]
        public virtual FingerMachineUser FingerMachineUsers { set; get; }
        public virtual IEnumerable<AbnormalCase> AbnormalCase { set; get; }
    }
}
