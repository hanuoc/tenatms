using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.ViewModels
{
    public class ListOTModel
    {
        public string UserName { set; get; }
        public string FullName { set; get; }
        public string GroupName { set; get; }
        public DateTime? OTDate { set; get; }
        public string NameOTDateType { get; set; }
        public string NameOTDateTime { get; set; }
        public string OTCheckIn { get; set; }
        public string OTCheckOut { get; set; }
        public double WorkingTime { get; set; }
        public string StatusRequest { set; get; }
        public string UpdatedByName { set; get; }
        public int StatusID { get; set; }
    }
}
