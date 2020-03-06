using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TMS.Model.Models;
using TMS.Web.Models.Common;

namespace TMS.Web.Models.Explanation
{
    public class ExplanationStatusViewModel
    {
        public int ID { set; get; }

        public string Name { set; get; }

        public IEnumerable<ExplanationRequestViewModel> ExplanationRequests { set; get; }
    }
}
