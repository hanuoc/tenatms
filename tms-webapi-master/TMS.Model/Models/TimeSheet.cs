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
    [Table(CommonConstants.TimeSheets)]
    public class TimeSheet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }
        [Required]
        [MaxLength(128)]
        public string UserID { set; get; }
        [Required]
        public DateTime DayOfCheck { set; get; }
        [MaxLength(8)]
        public string CheckIn { set; get; }
        [MaxLength(8)]
        public string CheckOut { set; get; }
        public bool ComeLate { set; get; }
        public bool ComeBackSoon { set; get; }
        [MaxLength(8)]
        public string Absent { set; get; }
        public double NumOfWorkingDay { set; get; }
        [MaxLength(8)]
        public string MinusAllowance { set; get; }
        [ForeignKey("UserID")]
        public virtual AppUser AppUser { set; get; }
        public virtual IEnumerable<AbnormalCase> AbnormalCase { set; get; }
    }
}
