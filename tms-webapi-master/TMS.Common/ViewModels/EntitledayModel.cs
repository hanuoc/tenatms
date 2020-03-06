using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.ViewModels
{
    public class EntitledayModel
    {
        public int IDEntitleday { get; set; }
        public string IDUser { set; get; }
        public int EntitleDayAppUserId { set; get; }
        public string FullName { set; get; }
        public string UserName { set; get; }
        public string HolidayType { get; set; }
        public string UnitType { set; get; }
        public float MaxEntitleDay { set; get; }
        public float NumberDayOff { set; get; }
        public float RemainDayOff { set; get; }
        public float RemainDayOfBeforeYear { set; get; }
        public float? DayBreak { set; get; }
        public string Description { set; get; }
        public int AuthorizedLeaveBonus { set; get; }
        public float TemporaryMaxEntitleDay { set; get; }
        public string Note { set; get; }
        public bool? Gender { set; get; }
    }
}
