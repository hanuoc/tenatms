using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Web.Models.Common
{
    public class FilterOTRequestModel
    {
        public string[] StatusRequestType { get; set; }
        public string[] OTTimeType { get; set; }
        public string[] OTDateType { get; set; }
        public string[] FullName { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }
}