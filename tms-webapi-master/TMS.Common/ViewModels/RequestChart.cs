using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.ViewModels
{
    public class RequestChart
    {
        public double TotalRequest { get; set; }
        public string StatusRequests { get; set; }
        public int CountStatusRequest { get; set; }
        public double PercentStatusRequest { get; set; }
    }
}
