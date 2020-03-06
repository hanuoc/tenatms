using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.ViewModels
{
    public class ExplanationOfTimeSheetModel
    {
        public string Approver { set; get; }
        public string Status { set; get; }
        public bool IsExplained { set; get; }
    }
}
