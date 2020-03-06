using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.ViewModels
{
    public class ExplanationRequestChartModel
    {
        public double TotalExRequest { get; set; }
        public string StatusExRequests { get; set; }
        public int CountStatusExRequest { get; set; }
        public double PercentStatusExRequest { get; set; }
    }
}
