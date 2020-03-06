using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.ViewModels
{
    public class FilterModelRequest
    {
        public string[] Creators { set; get; }
        public string[] StatusRequest { set; get; }
        public string[] RequestType { set; get; }
        public string[] RequestReasonType { set; get; }
        public string StartDate { set; get; }
        public string EndDate { set; get; }
    }
}
