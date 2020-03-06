using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.ViewModels
{
    public class AbnormalExcel
    {
        public string FullName { set; get; }
        public string Group { set; get; }
        public string AbnormalDate { set; get; }
        public string ReasonType { get; set; }
        public string AbsentType { set; get; }
        public string Status { set; get; }
    }
}
