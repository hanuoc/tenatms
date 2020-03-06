using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMS.Web.Models.TimeSheet
{
    public class FingerTimeSheetTmpViewModel
    {
        public int ID { set; get; }
        public string UserNo { set; get; }
        public DateTime Date { set; get; }
        public int NumberFinger { set; get; }
        public string UserName { set; get; }
    }
}