using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.ViewModels
{
    public class FilterReport
    {
        public string[] ListUserID { set; get; }
        public string StartDate { set; get; }
        public string EndDate { set; get; }
    }
}
