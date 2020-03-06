using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.ViewModels
{
    public class OTRequestChartModel
    {
        public double TotalOTRequest { get; set; }
        public string StatusOTRequests { get; set; }
        public int CountStatusOTRequest { get; set; }
        public double PercentStatusOTRequest { get; set; }
    }
}
