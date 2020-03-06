using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.Constants
{
    public class MailConsstants
    {
        //Template
        public const string TemplateSendMailAuto = "../../Templates/EmailTemplate.htm";
        public const string TemplateRejectMail = "../../Templates/EmailRejectTemplate.htm";
        public const string TemplateDelegationToMember = "../../Templates/EmailDelegation.htm";
        public const string TemplateDelegationRequest = "../../Templates/EmailDelegationRequest.htm";
        public const string TemplateSendMailToMember = "../../Templates/SendMailMemberTemplate.htm";
        public const string TemplateSendMailAll = "../../Templates/EmailTemplateAll.htm";
        public const string TemplateSendMailAfterUpdateUser = "../../Templates/EmailUpdateUserTemplate.htm";
        public const string TemplateSendMailOTRequest = "../../Templates/EmailOTRequestTemplate.htm";
        public const string TemplateSendMailAbnormal = "../../Templates/EmailAbnormalTemplate.htm";
        public const string TemplateRejectOTRequest= "../../Templates/EmailRejectOTRequestTemplate.htm";
        public const string TemplateRejectExplanation = "../../Templates/EmailRejectExplanationTemplate.htm";
        public const string TemplateSendMailAbnormalNoActual = "../../Templates/EmailAbnormalNoActualTemplate.htm";
        public const string TemplateRejectExplanationNoActual = "../../Templates/EmailRejectExplanationNoActualTemplate.htm";
        public const string TemplateNotificationAbnormal = "../../Templates/EmailNotificationAbnormal.htm";
        public const string TemplateNotificationJob = "../../Templates/EmailNotificationJob.htm";
        //String Replace in Template
        public const string GroupLeadName = "#GroupLeadName#";
        public const string Title = "#Title#";
        public const string CreateBy = "#CreateBy#";
        public const string Action = "#Action#";
        public const string EmailType = "#EmailType#";
        public const string ReasonReject = "#ReasonReject#";
        public const string Content = "#Content#";
        public const string Category = "#Category#"; 
        public const string GroupList = "#GroupList"; 
        public const string Receiver = "#Receiver";
        //String Replace Request
        public const string GroupName = "#GroupName#";
        public const string RequestTypeName = "#RequestTypeName#";
        public const string DetailReason = "#DetailReason#";
        public const string StartDate = "#StartDate#";
        public const string EndDate = "#EndDate#";
        //String Replace OT Request
        public const string OTDate = "#OTDate#";
        public const string OTDateType = "#OTDateType#";
        public const string OTTimeType = "#OTTimeType#";
        public const string StartTime = "#StartTime#";
        public const string EndTime = "#EndTime#";
        //String Replace Abnormalcase
        public const string ExplanationDate = "#ExplanationDate#";
        public const string ExplanationReason = "#ExplanationReason#";
        public const string Actual = "#Actual#";
        public const string Description = "#Description#";

        public const string FullName = "#FullName#";
        // String Replace Notification Abnormal 
        public const string AbnormalDate = "#AbnormalDate#";
        public const string AbnormalReason = "#AbnormalReason#";
        public const string DateCheck = "#Date#";

        //Subject Mail Constant
        public const string TitleCreateOTRequest = "TitleCreateOTRequest";
        public const string TitleApproved = "Request had been approved";
        public const string TitleRejected = "Request had been rejected";
        public const string TitleCancelled = "Request had been cancelled";
        public const string SubjectMailJob = "[Notification - TMS]";
    }
}
