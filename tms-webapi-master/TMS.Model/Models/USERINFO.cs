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
    [Table(CommonConstants.USERINFO)]
    public class USERINFO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int USERID { set; get; }
        [Required]
        [MaxLength(24)]
        public string Badgenumber { set; get; }
        [MaxLength(20)]
        public string SSN { set; get; }
        [MaxLength(40)]
        public string Name { set; get; }
        [MaxLength(8)]
        public string Gender { set; get; }
        [MaxLength(20)]
        public string TITLE { set; get; }
        [MaxLength(20)]
        public string PAGER { set; get; }
        public DateTime? BIRTHDAY { set; get; }
        public DateTime? HIREDDAY { set; get; }
        [MaxLength(80)]
        public string street { set; get; }
        [MaxLength(2)]
        public string CITY { set; get; }
        [MaxLength(2)]
        public string STATE { set; get; }
        [MaxLength(12)]
        public string ZIP { set; get; }
        [MaxLength(20)]
        public string OPHONE { set; get; }
        [MaxLength(20)]
        public string FPHONE { set; get; }
        public int? VERIFICATIONMETHOD { set; get; }
        public int? DEFAULTDEPTID { set; get; }
        public int? SECURITYFLAGS { set; get; }
        public int? ATT { set; get; }
        public int? INLATE { set; get; }
        public int? OUTEARLY { set; get; }
        public int? OVERTIME { set; get; }
        public int? SEP { set; get; }
        public int? HOLIDAY { set; get; }
        [MaxLength(8)]
        public string MINZU { set; get; }
        [MaxLength(50)]
        public string PASSWORD { set; get; }
        public int? LUNCHDURATION { set; get; }
        public byte[] PHOTO { set; get; }
        [MaxLength(10)]
        public string mverifypass { set; get; }
        public byte[] Notes { set; get; }
        public int? privilege { set; get; }
        public int? InheritDeptSch { set; get; }
        public int? InheritDeptSchClass { set; get; }
        public int? AutoSchPlan { set; get; }
        public int? MinAutoSchInterval { set; get; }
        public int? RegisterOT { set; get; }
        public int? InheritDeptRule { set; get; }
        public int? EMPRIVILEGE { set; get; }
        [MaxLength(20)]
        public string CardNo { set; get; }
    }
}
