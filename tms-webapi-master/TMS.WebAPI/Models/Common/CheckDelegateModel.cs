using System.Collections.Generic;

namespace TMS.Web.Models
{
    public class CheckDelegateModel
    {
        public bool CheckGroupDelegateDefault { set; get; }
        public bool CheckConfigDelegateDefault { set; get; }
        public string AssignGroupDelegate { set; get; }
        public string AssignConfigDelegate { set; get; }
    }
}