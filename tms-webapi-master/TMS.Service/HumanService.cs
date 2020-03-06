using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TMS.Common;
using TMS.Common.Constants;
using TMS.Model.Models;

namespace TMS.Service
{
    public interface IHumanService
    {
        string getBodyMail(string template, string content);
        bool SendMail(string[] toEmail, string[] ccToMail, string Subject, string Content, string[] attackFile);
    }
    public class HumanService : IHumanService
    {
        public string getBodyMail(string template, string content)
        {
            string Body = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(template));
            Body = Body.Replace(MailConsstants.Content, content);
            return Body;
        }

        public bool SendMail(string[] toEmail,string[] ccToMail, string subject, string content, string[] attackFile)
        {
            bool sendMail = MailHelper.SendMail(toEmail,ccToMail, subject, content, attackFile);
            return sendMail;
        }
    }
}
