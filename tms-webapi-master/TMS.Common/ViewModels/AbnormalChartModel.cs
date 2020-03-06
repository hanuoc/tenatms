using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.ViewModels
{
    public class AbnormalChartModel
    {
        public DateTime AbnormalDate { get; set; }
        public int LateComing { get; set; }
        public int EarlyLeaving { get; set; }
        public int UnauthorizedLeave { get; set; }

        public int UnusedAuthorizedEarlyLeaving { get; set; }
        public int UnusedAuthorizedLateComing { get; set; }
        public int UnusedAuthorizedLeave { get; set; }
        public int OTWithoutCheckIn { get; set; }
        public int OTWithoutCheckOut { get; set; }
        public int OTWithoutCheckInOut { get; set; }

        public DateTime FingerTimeSheet { get; set; }
        public int ReasonId { get; set; }

        public int? GroupId { get; set; }
    } 
}
