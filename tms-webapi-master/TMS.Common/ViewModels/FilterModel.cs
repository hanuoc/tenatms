using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TMS.Common.ViewModels
{
    public class FilterModel
    {
        public string[] StatusExplanation { set; get; }
        public string[] AbnormalTimeSheetType { set; get; }
        public string FromDate { set; get; }
        public string ToDate { set; get; }

        public string[] FullName { set; get; }
    }
}
