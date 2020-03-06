using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Common.Constants;

namespace TMS.Model.Models
{
    [Table(CommonConstants.EntitleDay_AppUser)]
    public class Entitleday_AppUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }
        public int EntitleDayId { set; get; }
        [MaxLength(128)]
        public string UserId { set; get; }
        public float NumberDayOff { set; get; }
        [DefaultValue(0)]
        public float RemainDayOfBeforeYear { set; get; }
        [DefaultValue(0)]
        public float DayBreak { set; get; }
        public int AuthorizedLeaveBonus{ set; get; }
        public float TemporaryMaxEntitleDay { set; get; }
        public float MaxEntitleDayAppUser { set; get; }
        public string Note { set; get; }
        [ForeignKey("EntitleDayId")]
        public virtual EntitleDay EntitleDay { set; get; }
        [ForeignKey("UserId")]
        public virtual AppUser AppUser { get; set; }


    }
}
