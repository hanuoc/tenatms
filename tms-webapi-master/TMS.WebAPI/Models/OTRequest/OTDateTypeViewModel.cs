using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TMS.Web.Models.OTRequest
{
    [Serializable]
    public class OTDateTypeViewModel
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public DateTime? CreatedDate { set; get; }
        public string CreatedBy { set; get; }
        public DateTime? UpdatedDate { set; get; }
        public string UpdatedBy { set; get; }
    }
}