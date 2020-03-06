using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.ViewModels
{
    public class AbnormalChartPercentModel
    {
        public string ReasonType { get; set; }
        public double Percentage { get; set; }
        public double ApprovePercent { get; set; }
        public double RejectPercent { get; set; }
        public int ReasonID { get; set; }
        public int StatusRequestID { get; set; }

        public int ReasonTypeID { get; set; }
    }
}
