using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.ViewModels
{
    public class ConfigDelegationModel
    {
        public int ID { set; get; }
        public string FullName { set; get; }
        public string UserName { set; get; }
        public string UserId { set; get; }
        public string AssignTo { get; set; }
        public DateTime? StartDate { set; get; }
        public DateTime? EndDate { set; get; }
        public int? GroupID { set; get; }
        public string AssignName { set; get; }
    }
}
