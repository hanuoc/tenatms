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
    [Table(CommonConstants.CheckInOut)]
    public class CHECKINOUT
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public int USERID { set; get; }
        [Key, Column(Order = 1)]
        public DateTime CHECKTIME { set; get; }
        [MaxLength(1)]
        public string CHECKTYPE { set; get; }
        public int VERIFYCODE { set; get; }
        [MaxLength(5)]
        public string SENSORID { set; get; }
        [MaxLength(30)]
        public string Memoinfo { set; get; }
        [MaxLength(10)]
        public string WorkCode { set; get; }
        [MaxLength(20)]
        public string sn { set; get; }
        public short? UserExtFmt { set; get; }
    }
}
