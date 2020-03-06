using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.ViewModels
{
    public class TotalUser
    {
        public int TotalUsers { get; set; }
        public int TotalMale { get; set; }
        public int TotalFemale { get; set; }
        public int TotalActive { get; set; }
        public int TotalInactive { get; set; }
        public int TotalOnsite { get; set;}
    }
}
