using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.ViewModels
{
    public class FilterDelegationAssignedModel
    {
        public string[] usernameAssigned { get; set; }
        public string[] StatusRequestType { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }
}
