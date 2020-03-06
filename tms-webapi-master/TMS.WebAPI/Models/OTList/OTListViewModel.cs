using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TMS.Model.Models;
using TMS.Web.Models.OTRequest;
using TMS.Web.Models.Request;

namespace TMS.Web.Models.OTList
{
    public class OTListViewModel
    {
        public string FullName { set; get; }
        public string Account { set; get; }
        public string Group { set; get; }
        public string OTDate { set; get; }
        public string OTDayType { get; set; }
        public string OTTimeType { get; set; }
        public string OTCheckIn { get; set; }
        public string OTCheckOut { get; set; }
        public string WorkingTime	 { get; set; }
        public string Approver { set; get; }
        public string Status { set; get; }
    }
}