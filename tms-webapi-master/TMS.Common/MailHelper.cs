using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;

namespace TMS.Common
{
    public class MailHelper
    {
        public static bool SendMail(string[] toEmail,string[] ccToMail, string subject, string content, string[] attackFile)
        {
            try
            {
                var host = ConfigHelper.GetByKey("SMTPHost");
                var port = int.Parse(ConfigHelper.GetByKey("SMTPPort"));
                var fromEmail = ConfigHelper.GetByKey("FromEmailAddress");
                var password = ConfigHelper.GetByKey("FromEmailPassword");
                var fromName = ConfigHelper.GetByKey("FromName");

                var smtpClient = new SmtpClient(host, port)
                {
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(fromEmail, password),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = true,
                    Timeout = 100000,
                };

                var mail = new MailMessage
                {
                    Body = content,
                    Subject = subject,
                    From = new MailAddress(fromEmail, fromName)
                };

                string url = "/UploadedFiles/SendMail/";

                if (attackFile != null)
                {
                    foreach(var item in attackFile.Distinct())
                    {
                        string path = url + item.ToString();
                        FileStream Body = System.IO.File.OpenRead(HttpContext.Current.Server.MapPath(path));
                        mail.Attachments.Add(new Attachment(Body.Name));
                    }
                }

                foreach (var item in toEmail.Distinct())
                {      
                    mail.To.Add(new MailAddress(item));
                }
                if (ccToMail != null)
                {
                    foreach (var cctomail in ccToMail.Distinct())
                    {
                        mail.CC.Add(new MailAddress(cctomail));
                    }
                }
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception smex)
            {
                return false;
            }
        }
    }
}
