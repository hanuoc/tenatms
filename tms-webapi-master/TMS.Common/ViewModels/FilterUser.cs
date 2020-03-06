using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.ViewModels
{
    public class FilterUser
    {
        public string[] UserID { set; get; }
        public string[] GroupID { set; get; }
        public bool[] Active { set; get; }
    }
}
