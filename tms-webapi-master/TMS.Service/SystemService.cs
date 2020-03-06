using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TMS.Common;
using TMS.Common.Constants;

namespace TMS.Service
{
    public interface ISystemService
    {
        string getBodyMail(string template, string title, string createBy, string action, string EmailType,string reasonReject,string category);
        string getBodyMailDelegate(string template, string GroupList, string createBy, string action, string EmailType, string reasonReject, string category, 
            string Group , string date, string ExplanationReason, string Description, string sender,string DearName);
        string getBodyMailDelegateRequest(string template, string Sender, string CreateBy, string Action, string EmailType, string ReasonReject, string Category,
           string GroupName, string StartDate, string EndDate, string ExplanationReason, string Description, string toEmail,string namedelegate);
        bool SendMail(string[] toEmail,string[] cctoMail, string Subject, string Content);
        string getBodyMail(string template, string title, string createBy, string action, string EmailType,string reasonReject,string category,string groupName, string requestTypeName, string detailReason, string startDate, string endDate,string otDate, string otDateType, string otTimeType,string startTime,string endTime,string explanationDate,string explanationReason,string actual,string description, string fullname);
        string getBodyMailDelegateDefault(string template, string GroupList, string createBy, string action, string EmailType, string reasonReject, string category,
            string Group, string date, string ExplanationReason, string Description, string sender, string DearName);
        string getBodyMailNotificationAbnormal(string template, string abnormalName, string groupName, string abnormalReason, string abnormalDate, string EmailType, string category, string datecheck);
        string getBodyMailNotificationJoblog(string template, string JobName, string JobLink);
    }
    public class SystemService : ISystemService
    {
public string getBodyMail(string template, string title, string createBy, string action, string EmailType, string reasonReject, string category, string groupName, string requestTypeName, string detailReason, string startDate, string endDate, string otDate, string otDateType, string otTimeType, string startTime, string endTime, string explanationDate, string explanationReason, string actual, string description, string fullname)
        {
            string Body = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(template));
            Body = Body.Replace(MailConsstants.Title, title);
            Body = Body.Replace(MailConsstants.CreateBy,createBy);
            Body = Body.Replace(MailConsstants.EmailType, EmailType);
            Body = Body.Replace(MailConsstants.Category, category);
            Body = Body.Replace(MailConsstants.GroupName, groupName);
            Body = Body.Replace(MailConsstants.RequestTypeName, requestTypeName);
            Body = Body.Replace(MailConsstants.DetailReason, detailReason);
            Body = Body.Replace(MailConsstants.StartDate, startDate);
            Body = Body.Replace(MailConsstants.EndDate, endDate);

            Body = Body.Replace(MailConsstants.OTDate, otDate);
            Body = Body.Replace(MailConsstants.OTDateType, otDateType);
            Body = Body.Replace(MailConsstants.OTTimeType, otTimeType);
            Body = Body.Replace(MailConsstants.StartTime, startTime);
            Body = Body.Replace(MailConsstants.EndTime, endTime);

            Body = Body.Replace(MailConsstants.ExplanationDate, explanationDate);
            Body = Body.Replace(MailConsstants.ExplanationReason, explanationReason);
            Body = Body.Replace(MailConsstants.Actual, actual);
            Body = Body.Replace(MailConsstants.Description, description);

            Body = Body.Replace(MailConsstants.FullName, fullname);
            if (!string.IsNullOrEmpty(reasonReject))
            {
                Body = Body = Body.Replace(MailConsstants.ReasonReject, reasonReject);
            }
            if (action == CommonConstants.StatusCancelled.ToLower())
            {
                Body = Body.Replace(MailConsstants.Action, CommonConstants.StatusCancelled.ToLower());
            }
            if (action == CommonConstants.StatusCreated.ToLower())
            {
                Body = Body.Replace(MailConsstants.Action, CommonConstants.StatusCreated.ToLower());
            }
            if (action == CommonConstants.StatusApproved.ToLower())
            {
                Body = Body.Replace(MailConsstants.Action, CommonConstants.StatusApproved.ToLower());
            }
            if (action == CommonConstants.StatusRejected.ToLower())
            {
                Body = Body.Replace(MailConsstants.Action, CommonConstants.StatusRejected.ToLower());
            }
            if (action == CommonConstants.StatusDelegation.ToLower())
            {
                Body = Body.Replace(MailConsstants.Action, CommonConstants.StatusDelegation.ToLower());
            }
            if (action == CommonConstants.Updated.ToLower())
            {
                Body = Body.Replace(MailConsstants.Action, CommonConstants.Updated.ToLower());
            }
            return Body;
        }
		
		
	    public string getBodyMail(string template, string title, string createBy, string action,string EmailType, string reasonReject, string category)
        {
            string Body = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(template));
            Body = Body.Replace(MailConsstants.Title, title);
            Body = Body.Replace(MailConsstants.CreateBy,createBy);
            Body = Body.Replace(MailConsstants.EmailType, EmailType);
            Body = Body.Replace(MailConsstants.Category, category);
            if (!string.IsNullOrEmpty(reasonReject))
            {
                Body = Body = Body.Replace(MailConsstants.ReasonReject, reasonReject);
            }
            if (action == CommonConstants.StatusCancelled.ToLower())
            {
                Body = Body.Replace(MailConsstants.Action, CommonConstants.StatusCancelled.ToLower());
            }
            if (action == CommonConstants.StatusCreated.ToLower())
            {
                Body = Body.Replace(MailConsstants.Action, CommonConstants.StatusCreated.ToLower());
            }
            if (action == CommonConstants.StatusApproved.ToLower())
            {
                Body = Body.Replace(MailConsstants.Action, CommonConstants.StatusApproved.ToLower());
            }
            if (action == CommonConstants.StatusRejected.ToLower())
            {
                Body = Body.Replace(MailConsstants.Action, CommonConstants.StatusRejected.ToLower());
            }
            if (action == CommonConstants.StatusDelegation.ToLower())
            {
                Body = Body.Replace(MailConsstants.Action, CommonConstants.StatusDelegation.ToLower());
            }
            if (action == CommonConstants.Updated.ToLower())
            {
                Body = Body.Replace(MailConsstants.Action, CommonConstants.Updated.ToLower());
            }
            return Body;
        }

        public string getBodyMailDelegate(string template, string GroupList, string createBy, string action, string EmailType, string reasonReject, string category, string Group,  string date,  string ExplanationReason, string Description, string sender, string DearName)
        {
            string Body = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(template));
            Body = Body.Replace("#GroupList", GroupList);
            Body = Body.Replace("#CreateBy#", createBy);
            Body = Body.Replace("#Receiver", sender);
            Body = Body.Replace(MailConsstants.EmailType, EmailType);
            Body = Body.Replace(MailConsstants.Category, category); //#recipient
            Body = Body.Replace("#GroupName#", Group); //#recipient
            

            DateTime datetime = DateTime.Parse(date);
            Body = Body.Replace("#EndDate#", datetime.ToString("dd/MM/yyyy")); //#recipient
            Body = Body.Replace("#ExplanationReason#", ExplanationReason); //#recipient

            Body = Body.Replace("#Description#", Description); //#recipient
            if(sender == DearName)
            {
                Body = Body.Replace("#your#", "a"); //#recipient
                Body = Body.Replace("#DearName#", "you"); //#recipient

            }
            else
            {
                Body = Body.Replace("#your#", "your"); //#recipient
                Body = Body.Replace("#DearName#", DearName); //#recipient

            }
            return Body;
        }

        public string getBodyMailDelegateRequest(string template, string Sender, string CreateBy, string Action, string EmailType, string ReasonReject, string Category, string GroupName, string StartDate, string EndDate, string DetailReason, string RequestTypeName, string toEmail, string namedelegate)
        {
            // template-- Grouplist-- - Create Request - Action-- mailtype-- Rệct - -cagortỉy-- stảt-- end-- Detail-- request type  --Nguòi duoc delegate
            // throw new NotImplementedException();
            string Body = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(template));
            Body = Body.Replace("#GroupList", Sender);
            Body = Body.Replace("#CreateBy#", CreateBy);
            // Body = Body.Replace("#Receiver", sender);
            Body = Body.Replace(MailConsstants.EmailType, EmailType);
            Body = Body.Replace(MailConsstants.Category, Category); //#recipient
            Body = Body.Replace("#ReasonReject#", ReasonReject); //#recipient

            Body = Body.Replace("#GroupName#", GroupName); //#recipient

            DateTime DateStart = DateTime.Parse(StartDate);
            DateTime DateEnd = DateTime.Parse(EndDate);
            Body = Body.Replace("#StartDate#", DateStart.ToString("dd/MM/yyyy")); //#recipient
            Body = Body.Replace("#EndDate#", DateEnd.ToString("dd/MM/yyyy")); //#recipient

            Body = Body.Replace("#DetailReason#", DetailReason); //#recipient
            Body = Body.Replace("#RequestTypeName#", RequestTypeName); //#recipient
            Body = Body.Replace("#Receiver", namedelegate); //#recipient
            if(namedelegate == toEmail)
            {
                Body = Body.Replace("#your#", "a");
                Body = Body.Replace("#NameDelegate#", "you");
            }
            else
            {
                Body = Body.Replace("#your#", "your");
                Body = Body.Replace("#NameDelegate#", toEmail); //#recipient
            }
            return Body;
        }

        public string getBodyMailDelegateDefault(string template, string GroupList, string createBy, string action, string EmailType, string reasonReject, string category, string Group, string date, string ExplanationReason, string Description, string sender, string DearName)
        {
            string Body = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(template));
            Body = Body.Replace("#GroupList", GroupList);
            Body = Body.Replace("#CreateBy#", createBy);
            Body = Body.Replace("#Receiver", sender);
            Body = Body.Replace(MailConsstants.EmailType, EmailType);
            Body = Body.Replace(MailConsstants.Category, category); //#recipient
            Body = Body.Replace("#GroupName#", Group); //#recipient

            Body = Body.Replace("#EndDate#", date); //#recipient
            Body = Body.Replace("#ExplanationReason#", ExplanationReason); //#recipient

            Body = Body.Replace("#Description#", Description); //#recipient
            Body = Body.Replace("#DearName#", DearName); //#recipient

            return Body;
        }
        public string getBodyMailNotificationAbnormal (string template , string abnormalName, string groupName, string abnormalReason, string abnormalDate, string EmailType, string category, string datecheck)
        {
            string filepath = Path.Combine(HttpRuntime.AppDomainAppPath, "Templates/EmailNotificationAbnormal.htm");
            string Body = System.IO.File.ReadAllText(filepath);

            Body = Body.Replace("#GroupName#", groupName);
            Body = Body.Replace("#AbnormalReason#", abnormalReason);
            DateTime datetime = DateTime.Parse(abnormalDate);
            Body = Body.Replace("#AbnormalDate#", datetime.ToString("dd/MM/yyyy"));

            Body = Body.Replace(MailConsstants.EmailType, EmailType);
            Body = Body.Replace(MailConsstants.Category, category);

            Body = Body.Replace("#FullName#", abnormalName);
            DateTime date = DateTime.Parse(datecheck);
            Body = Body.Replace("#Date#", date.ToString("dd/MM/yyyy"));

            return Body;
        }
        public string getBodyMailNotificationJoblog(string template, string JobName, string JobLink)
        {
            string filepath = Path.Combine(HttpContext.Current.Server.MapPath(template));
            string Body = System.IO.File.ReadAllText(filepath);
            Body = Body.Replace("#JobName#", JobName);
            Body = Body.Replace("#JobLink#", JobLink);
            return Body;
        }

        public bool SendMail(string[] toEmail, string[] cctoMail,string Subject, string Content)
        {
            bool sendMail = MailHelper.SendMail(toEmail,cctoMail,Subject, Content, null);
            return sendMail;
        }


    }
}
