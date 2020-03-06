using System.Collections.Generic;
using System.Web;

namespace TMS.Web.Models
{
    public class SendMailViewModel
    {
        public string[] toEmail { set; get; }
        public string[] ccToEmail { set; get; }
        public string Subject { set; get; }
        public string Content { set; get; }
        public string[] attackFile { get; set; }
    }
}