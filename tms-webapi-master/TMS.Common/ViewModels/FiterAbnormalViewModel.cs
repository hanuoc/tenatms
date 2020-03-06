using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.ViewModels
{
    public class FiterAbnormalViewModel
    {
        public string[] AbnormalReason { set; get; }
        public string[] StatusRequestsss { set; get; }
        public string[] AbnormalReasonTypeFilter { set; get; }
        public string[] FullName { set; get; }
        public string StartDate { set; get; }
        public string EndDate { set; get; }
    }
}
