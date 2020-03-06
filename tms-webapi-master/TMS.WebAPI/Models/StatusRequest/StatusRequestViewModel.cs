using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TMS.Web.Models.Request;

namespace TMS.Web.Models.StatusRequest
{
    public class StatusRequestViewModel
    {
        public int ID { set; get; }
        
        public string Name { set; get; }

        public IEnumerable<RequestViewModel> Requests { set; get; }
    }
}