using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using TMS.Common.Constants;
using TMS.Model.Models;
using TMS.Service;
using TMS.Web.Infrastructure.Core;
using TMS.Web.Models.Request;
using System.Web.Http;
using TMS.Common;
using TMS.Common.ViewModels;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Web.Controllers
{
    [Authorize]
    [RoutePrefix(RoutesConstant.System)]
    public class SystemController : ApiControllerBase
    {
        // GET: System
        private ISystemService _systemService;
        public SystemController(IErrorService errorService, ISystemService systemService) : base(errorService)
        {
            this._systemService = systemService;
        }
        /// <summary>
        /// Send Mail
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route(RoutesConstant.SendMail)]
        [HttpPost]
        public async Task<HttpResponseMessage> SendMail(HttpRequestMessage request, [FromBody]SendMailModel sendMailModel)
        {
            Func<HttpResponseMessage> func = () =>
            {
                string Body = "";
                string BodyInDelegate = "";
                string BodyCreateExplan = "";
                bool resultsendmailDelegate = true;
                string FullName = "";
                List<string> toEmail = new List<string>();
                var result = false;
                if (sendMailModel.Action.Equals(CommonConstants.StatusRejected.ToLower()))
                {
                    if (sendMailModel.OTDateType != null)
                    {
                        foreach (var item in sendMailModel.toEmail)
                        {
                            FullName = "";
                            toEmail = new List<string>();
                            var str = item.Split(',');
                            FullName = str[1];
                            toEmail.Add(str[0]);
                            Body = _systemService.getBodyMail(MailConsstants.TemplateRejectOTRequest, sendMailModel.Title, AppUserManager.FindByIdAsync(sendMailModel.CreateBy).Result.FullName, sendMailModel.Action, sendMailModel.EmailType, sendMailModel.ReasonReject, sendMailModel.Category, sendMailModel.GroupName, sendMailModel.RequestTypeName, sendMailModel.DetailReason, sendMailModel.StartDate, sendMailModel.EndDate, sendMailModel.OTDate, sendMailModel.OTDateType, sendMailModel.OTTimeType, sendMailModel.StartTime, sendMailModel.EndTime, sendMailModel.ExplanationDate, sendMailModel.ExplanationReason, sendMailModel.Actual, sendMailModel.Description, FullName);
                            resultsendmailDelegate = _systemService.SendMail(toEmail.ToArray(), sendMailModel.ccMail, sendMailModel.EmailSubject, Body);
                        }
                    }
                    else if (sendMailModel.RequestTypeName != null)
                    {
                        foreach (var item in sendMailModel.toEmail)
                        {
                            FullName = "";
                            toEmail = new List<string>();
                            var str = item.Split(',');
                            FullName = str[1];
                            toEmail.Add(str[0]);
                            Body = _systemService.getBodyMail(MailConsstants.TemplateRejectMail, sendMailModel.Title, AppUserManager.FindByIdAsync(sendMailModel.CreateBy).Result.FullName, sendMailModel.Action, sendMailModel.EmailType, sendMailModel.ReasonReject, sendMailModel.Category, sendMailModel.GroupName, sendMailModel.RequestTypeName, sendMailModel.DetailReason, sendMailModel.StartDate, sendMailModel.EndDate, sendMailModel.OTDate, sendMailModel.OTDateType, sendMailModel.OTTimeType, sendMailModel.StartTime, sendMailModel.EndTime, sendMailModel.ExplanationDate, sendMailModel.ExplanationReason, sendMailModel.Actual, sendMailModel.Description, FullName);
                            resultsendmailDelegate = _systemService.SendMail(toEmail.ToArray(), sendMailModel.ccMail, sendMailModel.EmailSubject, Body);
                        }

                    }
                    else if (sendMailModel.Description != null)
                    {
                        foreach (var item in sendMailModel.toEmail)
                        {
                            FullName = "";
                            toEmail = new List<string>();
                            var str = item.Split(',');
                            FullName = str[1];
                            toEmail.Add(str[0]);
                            if (sendMailModel.Actual != "")
                            {
                                Body = _systemService.getBodyMail(MailConsstants.TemplateRejectExplanation, sendMailModel.Title, AppUserManager.FindByIdAsync(sendMailModel.CreateBy).Result.FullName, sendMailModel.Action, sendMailModel.EmailType, sendMailModel.ReasonReject, sendMailModel.Category, sendMailModel.GroupName, sendMailModel.RequestTypeName, sendMailModel.DetailReason, sendMailModel.StartDate, sendMailModel.EndDate, sendMailModel.OTDate, sendMailModel.OTDateType, sendMailModel.OTTimeType, sendMailModel.StartTime, sendMailModel.EndTime, sendMailModel.ExplanationDate, sendMailModel.ExplanationReason, sendMailModel.Actual, sendMailModel.Description, FullName);
                            }
                            else
                            {
                                Body = _systemService.getBodyMail(MailConsstants.TemplateRejectExplanationNoActual, sendMailModel.Title, AppUserManager.FindByIdAsync(sendMailModel.CreateBy).Result.FullName, sendMailModel.Action, sendMailModel.EmailType, sendMailModel.ReasonReject, sendMailModel.Category, sendMailModel.GroupName, sendMailModel.RequestTypeName, sendMailModel.DetailReason, sendMailModel.StartDate, sendMailModel.EndDate, sendMailModel.OTDate, sendMailModel.OTDateType, sendMailModel.OTTimeType, sendMailModel.StartTime, sendMailModel.EndTime, sendMailModel.ExplanationDate, sendMailModel.ExplanationReason, sendMailModel.Actual, sendMailModel.Description, FullName);
                            }

                            resultsendmailDelegate = _systemService.SendMail(toEmail.ToArray(), sendMailModel.ccMail, sendMailModel.EmailSubject, Body);
                        }
                    }
                }
                else if (sendMailModel.Action.Equals(CommonConstants.StatusCancelled.ToLower()))
                {
                    Body = _systemService.getBodyMail(MailConsstants.TemplateSendMailAuto, sendMailModel.Title, AppUserManager.FindByIdAsync(sendMailModel.CreateBy).Result.FullName, sendMailModel.Action, sendMailModel.EmailType, null, sendMailModel.Category, null, null, null, null, null, sendMailModel.OTDate, sendMailModel.OTDateType, sendMailModel.OTTimeType, sendMailModel.StartTime, sendMailModel.EndTime, sendMailModel.ExplanationDate, sendMailModel.ExplanationReason, sendMailModel.Actual, sendMailModel.Description, FullName);
                }
                else if (sendMailModel.Action.Equals(CommonConstants.StatusDelegation.ToLower()))
                {

                    if (sendMailModel.ExplanationReason != null)
                    {
                        // Body = _systemService.getBodyMail(MailConsstants.TemplateDelegationToMember, sendMailModel.Title, AppUserManager.FindByIdAsync(sendMailModel.CreateBy).Result.FullName, sendMailModel.Action, sendMailModel.EmailType, null, sendMailModel.Category);
                        Body = _systemService.getBodyMailDelegate(MailConsstants.TemplateDelegationToMember, AppUserManager.FindByIdAsync(sendMailModel.Sender).Result.FullName, sendMailModel.CreateBy, sendMailModel.Action, sendMailModel.EmailType, null, sendMailModel.Category, sendMailModel.GroupName,
                         sendMailModel.EndDate, sendMailModel.ExplanationReason, sendMailModel.Description, AppUserManager.FindByIdAsync(sendMailModel.toEmail[0]).Result.FullName, AppUserManager.FindByIdAsync(sendMailModel.toEmail[0]).Result.FullName);
                        BodyCreateExplan = _systemService.getBodyMailDelegate(MailConsstants.TemplateDelegationToMember, AppUserManager.FindByIdAsync(sendMailModel.Sender).Result.FullName, sendMailModel.CreateBy, sendMailModel.Action, sendMailModel.EmailType, null, sendMailModel.Category, sendMailModel.GroupName,
                             sendMailModel.EndDate, sendMailModel.ExplanationReason, sendMailModel.Description, AppUserManager.FindByIdAsync(sendMailModel.Receiver).Result.FullName, AppUserManager.FindByIdAsync(sendMailModel.toEmail[0]).Result.FullName);
                        sendMailModel.toEmail = new string[] { AppUserManager.FindByIdAsync(sendMailModel.toEmail[0]).Result.Email };
                        //resultsendmailDelegate = _systemService.SendMail(sendMailModel.toEmail, sendMailModel.EmailSubject, Body);
                        string[] mail = new string[1]; // create
                        mail[0] = AppUserManager.FindByIdAsync(sendMailModel.Receiver).Result.Email;
                        resultsendmailDelegate = _systemService.SendMail(sendMailModel.toEmail, null, sendMailModel.EmailSubject, Body);

                        if (AppUserManager.FindByIdAsync(sendMailModel.Receiver).Result.Email != sendMailModel.toEmail[0].ToString())
                        {
                            var resultsendmailcreateexplanation = _systemService.SendMail(mail, null, sendMailModel.EmailSubject, BodyCreateExplan);
                        }
                    }
                    else if (sendMailModel.RequestTypeName != "")
                    {
                        // body là ng đuoc delegate
                        //  Body = _systemService.getBodyMailDelegateRequest(MailConsstants.TemplateDelegationRequest, AppUserManager.FindByIdAsync(sendMailModel.Sender).Result.FullName, sendMailModel.CreateBy, sendMailModel.Action, sendMailModel.EmailType, null, sendMailModel.Category, sendMailModel.GroupName,
                        //sendMailModel.EndDate, sendMailModel.ExplanationReason, sendMailModel.Description, AppUserManager.FindByIdAsync(sendMailModel.toEmail[0]).Result.FullName, AppUserManager.FindByIdAsync(sendMailModel.toEmail[0]).Result.FullName);
                        // template -- Grouplist --- Create Request -  Action -- mailtype -- Rệct  - - cagortỉy -- stảt -- end -- Detail -- request type  -- Nguòi duoc delegate
                        Body = _systemService.getBodyMailDelegateRequest(MailConsstants.TemplateDelegationRequest, AppUserManager.FindByIdAsync(sendMailModel.Sender).Result.FullName, AppUserManager.FindByIdAsync(sendMailModel.CreateBy).Result.FullName, sendMailModel.Action, sendMailModel.EmailType, null, sendMailModel.Category, sendMailModel.GroupName,
                         sendMailModel.StartDate, sendMailModel.EndDate, sendMailModel.DetailReason, sendMailModel.RequestTypeName, AppUserManager.FindByIdAsync(sendMailModel.toEmail[0]).Result.FullName, AppUserManager.FindByIdAsync(sendMailModel.toEmail[0]).Result.FullName);
                        BodyCreateExplan = _systemService.getBodyMailDelegateRequest(MailConsstants.TemplateDelegationRequest, AppUserManager.FindByIdAsync(sendMailModel.Sender).Result.FullName, AppUserManager.FindByIdAsync(sendMailModel.CreateBy).Result.FullName, sendMailModel.Action, sendMailModel.EmailType, null, sendMailModel.Category, sendMailModel.GroupName,
                         sendMailModel.StartDate, sendMailModel.EndDate, sendMailModel.DetailReason, sendMailModel.RequestTypeName, AppUserManager.FindByIdAsync(sendMailModel.toEmail[0]).Result.FullName, AppUserManager.FindByIdAsync(sendMailModel.CreateBy).Result.FullName);

                        string[] mail = new string[1]; // create
                        mail[0] = AppUserManager.FindByIdAsync(sendMailModel.CreateBy).Result.Email;
                        string[] mail1 = new string[1]; // create
                        mail1[0] = AppUserManager.FindByIdAsync(sendMailModel.toEmail[0]).Result.Email;

                        resultsendmailDelegate = _systemService.SendMail(mail1, null, sendMailModel.EmailSubject, Body);

                        if (sendMailModel.CreateBy != sendMailModel.toEmail[0].ToString())
                        {
                            var data = AppUserManager.FindByIdAsync(sendMailModel.CreateBy).Result.Email;
                            var data1 = sendMailModel.toEmail[0].ToString();
                            var resultsendmailcreateexplanation = _systemService.SendMail(mail, null, sendMailModel.EmailSubject, BodyCreateExplan);
                        }
                    }
                }
                else if (sendMailModel.Action.Equals(CommonConstants.Updated.ToLower()))
                {
                    foreach (var item in sendMailModel.toEmail)
                    {
                        FullName = "";
                        toEmail = new List<string>();
                        var str = item.Split(',');
                        FullName = str[1];
                        toEmail.Add(str[0]);
                        Body = _systemService.getBodyMail(MailConsstants.TemplateSendMailAfterUpdateUser, sendMailModel.Title, AppUserManager.FindByIdAsync(sendMailModel.CreateBy).Result.FullName, sendMailModel.Action, sendMailModel.EmailType, null, null, null, null, null, null, null, sendMailModel.OTDate, sendMailModel.OTDateType, sendMailModel.OTTimeType, sendMailModel.StartTime, sendMailModel.EndTime, sendMailModel.ExplanationDate, sendMailModel.ExplanationReason, sendMailModel.Actual, sendMailModel.Description, FullName);
                        resultsendmailDelegate = _systemService.SendMail(toEmail.ToArray(), sendMailModel.ccMail, sendMailModel.EmailSubject, Body);
                    }
                }
                else
                {
                    if (sendMailModel.OTDateType != null)
                    {
                        foreach (var tomail in sendMailModel.toEmail)
                        {
                            FullName = "";
                            toEmail = new List<string>();
                            var str = tomail.Split(',');
                            FullName = str[1];
                            toEmail.Add(str[0]);

                            Body = _systemService.getBodyMail(MailConsstants.TemplateSendMailOTRequest, sendMailModel.Title, AppUserManager.FindByIdAsync(sendMailModel.CreateBy).Result.FullName, sendMailModel.Action, sendMailModel.EmailType, null, sendMailModel.Category, sendMailModel.GroupName, sendMailModel.RequestTypeName, sendMailModel.DetailReason, sendMailModel.StartDate, sendMailModel.EndDate, sendMailModel.OTDate, sendMailModel.OTDateType, sendMailModel.OTTimeType, sendMailModel.StartTime, sendMailModel.EndTime, sendMailModel.ExplanationDate, sendMailModel.ExplanationReason, sendMailModel.Actual, sendMailModel.Description, FullName);
                            resultsendmailDelegate = _systemService.SendMail(toEmail.ToArray(), sendMailModel.ccMail, sendMailModel.EmailSubject, Body);
                            sendMailModel.ccMail = null;
                        }
                    }
                    else if (sendMailModel.RequestTypeName != null)
                    {
                        foreach (var item in sendMailModel.toEmail)
                        {
                            FullName = "";
                            toEmail = new List<string>();
                            var str = item.Split(',');
                            FullName = str[1];
                            toEmail.Add(str[0]);
                            Body = _systemService.getBodyMail(MailConsstants.TemplateSendMailAuto, sendMailModel.Title, AppUserManager.FindByIdAsync(sendMailModel.CreateBy).Result.FullName, sendMailModel.Action, sendMailModel.EmailType, null, sendMailModel.Category, sendMailModel.GroupName, sendMailModel.RequestTypeName, sendMailModel.DetailReason, sendMailModel.StartDate, sendMailModel.EndDate, sendMailModel.OTDate, sendMailModel.OTDateType, sendMailModel.OTTimeType, sendMailModel.StartTime, sendMailModel.EndTime, sendMailModel.ExplanationDate, sendMailModel.ExplanationReason, sendMailModel.Actual, sendMailModel.Description, FullName);
                            resultsendmailDelegate = _systemService.SendMail(toEmail.ToArray(), sendMailModel.ccMail, sendMailModel.EmailSubject, Body);
                            sendMailModel.ccMail = null;
                        }

                    }
                    else if (sendMailModel.Description != null)
                    {
                        foreach (var item in sendMailModel.toEmail)
                        {
                            FullName = "";
                            toEmail = new List<string>();
                            var str = item.Split(',');
                            FullName = str[1];
                            toEmail.Add(str[0]);
                            if (sendMailModel.Actual != "")
                            {
                                if(sendMailModel.Actual.Equals("ForgetToCheck"))
                                {
                                    sendMailModel.Actual = "Forget to check Fingerprints";
                                }
                                Body = _systemService.getBodyMail(MailConsstants.TemplateSendMailAbnormal, sendMailModel.Title, AppUserManager.FindByIdAsync(sendMailModel.CreateBy).Result.FullName, sendMailModel.Action, sendMailModel.EmailType, null, sendMailModel.Category, sendMailModel.GroupName, sendMailModel.RequestTypeName, sendMailModel.DetailReason, sendMailModel.StartDate, sendMailModel.EndDate, sendMailModel.OTDate, sendMailModel.OTDateType, sendMailModel.OTTimeType, sendMailModel.StartTime, sendMailModel.EndTime, sendMailModel.ExplanationDate, sendMailModel.ExplanationReason, sendMailModel.Actual, sendMailModel.Description, FullName);
                            }
                            else
                            {
                                Body = _systemService.getBodyMail(MailConsstants.TemplateSendMailAbnormalNoActual, sendMailModel.Title, AppUserManager.FindByIdAsync(sendMailModel.CreateBy).Result.FullName, sendMailModel.Action, sendMailModel.EmailType, null, sendMailModel.Category, sendMailModel.GroupName, sendMailModel.RequestTypeName, sendMailModel.DetailReason, sendMailModel.StartDate, sendMailModel.EndDate, sendMailModel.OTDate, sendMailModel.OTDateType, sendMailModel.OTTimeType, sendMailModel.StartTime, sendMailModel.EndTime, sendMailModel.ExplanationDate, sendMailModel.ExplanationReason, sendMailModel.Actual, sendMailModel.Description, FullName);
                            }
                            resultsendmailDelegate = _systemService.SendMail(toEmail.ToArray(), sendMailModel.ccMail, sendMailModel.EmailSubject, Body);
                        }
                    }
                }
                return request.CreateResponse(HttpStatusCode.OK, resultsendmailDelegate);
            };
            return await CreateHttpResponse(request, func);
        }

        /// <summary>
        /// Send Mail multi 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route(RoutesConstant.SendMailMulti)]
        [HttpPost]
        public async Task<HttpResponseMessage> SendMailMulti(HttpRequestMessage request, [FromBody]List<SendMailModel> sendMailModel)
        {
            Func<HttpResponseMessage> func = () =>
            {
                foreach (var items in sendMailModel)
                {
                    string Body = "";
                    string BodyInDelegate = "";
                    string BodyCreateExplan = "";
                    bool resultsendmailDelegate = true;
                    string FullName = "";
                    if (items.Action.Equals(CommonConstants.StatusRejected.ToLower()))
                    {
                        Body = _systemService.getBodyMail(MailConsstants.TemplateRejectMail, items.Title, AppUserManager.FindByIdAsync(items.CreateBy).Result.FullName, items.Action, items.EmailType, items.ReasonReject, items.Category, null, null, null, null, null, items.OTDate, items.OTDateType, items.OTTimeType, items.StartTime, items.EndTime, items.ExplanationDate, items.ExplanationReason, items.Actual, items.Description, FullName);
                    }
                    else if (items.Action.Equals(CommonConstants.StatusCancelled.ToLower()))
                    {
                        Body = _systemService.getBodyMail(MailConsstants.TemplateSendMailAuto, items.Title, AppUserManager.FindByIdAsync(items.CreateBy).Result.FullName, items.Action, items.EmailType, null, items.Category, null, null, null, null, null, items.OTDate, items.OTDateType, items.OTTimeType, items.StartTime, items.EndTime, items.ExplanationDate, items.ExplanationReason, items.Actual, items.Description, FullName);
                    }
                    else if (items.Action.Equals(CommonConstants.StatusDelegation.ToLower()))
                    {
                        Body = _systemService.getBodyMail(MailConsstants.TemplateDelegationToMember, items.Title, AppUserManager.FindByIdAsync(items.CreateBy).Result.FullName, items.Action, items.EmailType, null, items.Category, null, null, null, null, null, items.OTDate, items.OTDateType, items.OTTimeType, items.StartTime, items.EndTime, items.ExplanationDate, items.ExplanationReason, items.Actual, items.Description, FullName);
                        items.toEmail = new string[] { AppUserManager.FindByIdAsync(items.toEmail[0]).Result.Email };
                    }
                    else
                    {
                        Body = _systemService.getBodyMail(MailConsstants.TemplateSendMailAuto, items.Title, AppUserManager.FindByIdAsync(items.CreateBy).Result.FullName, items.Action, items.EmailType, null, items.Category, items.GroupName, items.RequestTypeName, items.DetailReason, items.StartDate, items.EndDate, items.OTDate, items.OTDateType, items.OTTimeType, items.StartTime, items.EndTime, items.ExplanationDate, items.ExplanationReason, items.Actual, items.Description, FullName);
                    }
                    var result = _systemService.SendMail(items.toEmail, items.ccMail, items.EmailSubject, Body);
                }
                return request.CreateResponse(HttpStatusCode.OK, true);
            };
            return await CreateHttpResponse(request, func);
        }

        /// <summary>
        /// Send Mail multi version new
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route(RoutesConstant.SendMailMultiFix)]
        [HttpPost]
        public async Task<HttpResponseMessage> SendMailMultiFix(HttpRequestMessage request, [FromBody]List<SendMailModel> sendMailModel)
        {
            string BodyInDelegate = "";
            string BodyCreateExplan = "";
            bool resultsendmailDelegate = true;
            Func<HttpResponseMessage> func = () =>
            {
                foreach (var items in sendMailModel)
                {
                    string Body = "";
                    string FullName = "";
                    List<string> toEmail = new List<string>();
                    var result = false;
                    if (items.Action.Equals(CommonConstants.StatusRejected.ToLower()))
                    {
                        if (items.OTDateType != null)
                        {
                            foreach (var item in items.toEmail)
                            {
                                FullName = "";
                                toEmail = new List<string>();
                                var str = item.Split(',');
                                FullName = str[1];
                                toEmail.Add(str[0]);
                                Body = _systemService.getBodyMail(MailConsstants.TemplateRejectOTRequest, items.Title, AppUserManager.FindByIdAsync(items.CreateBy).Result.FullName, items.Action, items.EmailType, items.ReasonReject, items.Category, items.GroupName, items.RequestTypeName, items.DetailReason, items.StartDate, items.EndDate, items.OTDate, items.OTDateType, items.OTTimeType, items.StartTime, items.EndTime, items.ExplanationDate, items.ExplanationReason, items.Actual, items.Description, FullName);
                                result = _systemService.SendMail(toEmail.ToArray(), items.ccMail, items.EmailSubject, Body);
                            }
                        }
                        else if (items.RequestTypeName != null)
                        {
                            foreach (var item in items.toEmail)
                            {
                                FullName = "";
                                toEmail = new List<string>();
                                var str = item.Split(',');
                                FullName = str[1];
                                toEmail.Add(str[0]);
                                Body = _systemService.getBodyMail(MailConsstants.TemplateRejectMail, items.Title, AppUserManager.FindByIdAsync(items.CreateBy).Result.FullName, items.Action, items.EmailType, items.ReasonReject, items.Category, items.GroupName, items.RequestTypeName, items.DetailReason, items.StartDate, items.EndDate, items.OTDate, items.OTDateType, items.OTTimeType, items.StartTime, items.EndTime, items.ExplanationDate, items.ExplanationReason, items.Actual, items.Description, FullName);
                                result = _systemService.SendMail(toEmail.ToArray(), items.ccMail, items.EmailSubject, Body);
                            }

                        }
                        else if (items.Description != null)
                        {
                            foreach (var item in items.toEmail)
                            {
                                FullName = "";
                                toEmail = new List<string>();
                                var str = item.Split(',');
                                FullName = str[1];
                                toEmail.Add(str[0]);
                                if (items.Actual != "")
                                {
                                    Body = _systemService.getBodyMail(MailConsstants.TemplateRejectExplanation, items.Title, AppUserManager.FindByIdAsync(items.CreateBy).Result.FullName, items.Action, items.EmailType, items.ReasonReject, items.Category, items.GroupName, items.RequestTypeName, items.DetailReason, items.StartDate, items.EndDate, items.OTDate, items.OTDateType, items.OTTimeType, items.StartTime, items.EndTime, items.ExplanationDate, items.ExplanationReason, items.Actual, items.Description, FullName);
                                }
                                else
                                {
                                    Body = _systemService.getBodyMail(MailConsstants.TemplateRejectExplanationNoActual, items.Title, AppUserManager.FindByIdAsync(items.CreateBy).Result.FullName, items.Action, items.EmailType, items.ReasonReject, items.Category, items.GroupName, items.RequestTypeName, items.DetailReason, items.StartDate, items.EndDate, items.OTDate, items.OTDateType, items.OTTimeType, items.StartTime, items.EndTime, items.ExplanationDate, items.ExplanationReason, items.Actual, items.Description, FullName);
                                }

                                result = _systemService.SendMail(toEmail.ToArray(), items.ccMail, items.EmailSubject, Body);
                            }
                        }
                    }
                    else if (items.Action.Equals(CommonConstants.StatusCancelled.ToLower()))
                    {
                        Body = _systemService.getBodyMail(MailConsstants.TemplateSendMailAuto, items.Title, AppUserManager.FindByIdAsync(items.CreateBy).Result.FullName, items.Action, items.EmailType, null, items.Category, null, null, null, null, null, items.OTDate, items.OTDateType, items.OTTimeType, items.StartTime, items.EndTime, items.ExplanationDate, items.ExplanationReason, items.Actual, items.Description, FullName);
                    }
                    else if (items.Action.Equals(CommonConstants.StatusDelegation.ToLower()))
                    {

                        if (items.ExplanationReason != null)
                        {
                            Body = _systemService.getBodyMailDelegate(MailConsstants.TemplateDelegationToMember, AppUserManager.FindByIdAsync(items.Sender).Result.FullName, AppUserManager.FindByIdAsync(items.Receiver).Result.FullName, items.Action, items.EmailType, null, items.Category, items.GroupName,
                        items.EndDate, items.ExplanationReason, items.Description, AppUserManager.FindByIdAsync(items.toEmail[0]).Result.FullName, AppUserManager.FindByIdAsync(items.toEmail[0]).Result.FullName);
                            BodyCreateExplan = _systemService.getBodyMailDelegate(MailConsstants.TemplateDelegationToMember, AppUserManager.FindByIdAsync(items.Sender).Result.FullName, AppUserManager.FindByIdAsync(items.Receiver).Result.FullName, items.Action, items.EmailType, null, items.Category, items.GroupName,
                                 items.EndDate, items.ExplanationReason, items.Description, AppUserManager.FindByIdAsync(items.Receiver).Result.FullName, AppUserManager.FindByIdAsync(items.toEmail[0]).Result.FullName);
                            items.toEmail = new string[] { AppUserManager.FindByIdAsync(items.toEmail[0]).Result.Email };
                            //Body = _systemService.getBodyMail(MailConsstants.TemplateDelegationToMember, items.Title, AppUserManager.FindByIdAsync(items.CreateBy).Result.FullName, items.Action, items.EmailType, null, items.Category);
                            //items.toEmail = new string[] { AppUserManager.FindByIdAsync(items.toEmail[0]).Result.Email };
                            resultsendmailDelegate = _systemService.SendMail(items.toEmail, null, items.EmailSubject, Body);
                            string[] mail = new string[1]; // create
                            mail[0] = AppUserManager.FindByIdAsync(items.Receiver).Result.Email;

                            if (AppUserManager.FindByIdAsync(items.Receiver).Result.Email != items.toEmail[0].ToString())
                            {
                                var resultsendmailcreateexplanation = _systemService.SendMail(mail, null, items.EmailSubject, BodyCreateExplan);
                            }
                        }
                        else if (items.RequestTypeName != "")
                        {
                            Body = _systemService.getBodyMailDelegateRequest(MailConsstants.TemplateDelegationRequest, AppUserManager.FindByIdAsync(items.Sender).Result.FullName, AppUserManager.FindByIdAsync(items.CreateBy).Result.FullName, items.Action, items.EmailType, null, items.Category, items.GroupName,
                         items.StartDate, items.EndDate, items.DetailReason, items.RequestTypeName, AppUserManager.FindByIdAsync(items.toEmail[0]).Result.FullName, AppUserManager.FindByIdAsync(items.toEmail[0]).Result.FullName);
                            BodyCreateExplan = _systemService.getBodyMailDelegateRequest(MailConsstants.TemplateDelegationRequest, AppUserManager.FindByIdAsync(items.Sender).Result.FullName, AppUserManager.FindByIdAsync(items.CreateBy).Result.FullName, items.Action, items.EmailType, null, items.Category, items.GroupName,
                             items.StartDate, items.EndDate, items.DetailReason, items.RequestTypeName, AppUserManager.FindByIdAsync(items.toEmail[0]).Result.FullName, AppUserManager.FindByIdAsync(items.CreateBy).Result.FullName);

                            string[] mail = new string[1]; // create
                            mail[0] = AppUserManager.FindByIdAsync(items.CreateBy).Result.Email;
                            string[] mail1 = new string[1]; // create
                            mail1[0] = AppUserManager.FindByIdAsync(items.toEmail[0]).Result.Email;

                            resultsendmailDelegate = _systemService.SendMail(mail1, null, items.EmailSubject, Body);

                            if (AppUserManager.FindByIdAsync(items.CreateBy).Result.Email != items.toEmail[0].ToString())
                            {
                                var resultsendmailcreateexplanation = _systemService.SendMail(mail, null, items.EmailSubject, BodyCreateExplan);
                            }
                        }
                    }
                    else
                    {
                        if (items.OTDateType != null)
                        {
                            foreach (var item in items.toEmail)
                            {
                                FullName = "";
                                toEmail = new List<string>();
                                var str = item.Split(',');
                                FullName = str[1];
                                toEmail.Add(str[0]);
                                Body = _systemService.getBodyMail(MailConsstants.TemplateSendMailOTRequest, items.Title, AppUserManager.FindByIdAsync(items.CreateBy).Result.FullName, items.Action, items.EmailType, null, items.Category, items.GroupName, items.RequestTypeName, items.DetailReason, items.StartDate, items.EndDate, items.OTDate, items.OTDateType, items.OTTimeType, items.StartTime, items.EndTime, items.ExplanationDate, items.ExplanationReason, items.Actual, items.Description, FullName);
                                result = _systemService.SendMail(toEmail.ToArray(), items.ccMail, items.EmailSubject, Body);
                            }

                        }
                        else if (items.RequestTypeName != null)
                        {
                            foreach (var item in items.toEmail)
                            {
                                FullName = "";
                                toEmail = new List<string>();
                                var str = item.Split(',');
                                FullName = str[1];
                                toEmail.Add(str[0]);
                                Body = _systemService.getBodyMail(MailConsstants.TemplateSendMailAuto, items.Title, AppUserManager.FindByIdAsync(items.CreateBy).Result.FullName, items.Action, items.EmailType, null, items.Category, items.GroupName, items.RequestTypeName, items.DetailReason, items.StartDate, items.EndDate, items.OTDate, items.OTDateType, items.OTTimeType, items.StartTime, items.EndTime, items.ExplanationDate, items.ExplanationReason, items.Actual, items.Description, FullName);
                                result = _systemService.SendMail(toEmail.ToArray(), items.ccMail, items.EmailSubject, Body);
                            }

                        }
                        else if (items.Description != null)
                        {
                            foreach (var item in items.toEmail)
                            {
                                FullName = "";
                                toEmail = new List<string>();
                                var str = item.Split(',');
                                FullName = str[1];
                                toEmail.Add(str[0]);
                                if (items.Actual != "")
                                {
                                    Body = _systemService.getBodyMail(MailConsstants.TemplateSendMailAbnormal, items.Title, AppUserManager.FindByIdAsync(items.CreateBy).Result.FullName, items.Action, items.EmailType, null, items.Category, items.GroupName, items.RequestTypeName, items.DetailReason, items.StartDate, items.EndDate, items.OTDate, items.OTDateType, items.OTTimeType, items.StartTime, items.EndTime, items.ExplanationDate, items.ExplanationReason, items.Actual, items.Description, FullName);
                                }
                                else
                                {
                                    Body = _systemService.getBodyMail(MailConsstants.TemplateSendMailAbnormalNoActual, items.Title, AppUserManager.FindByIdAsync(items.CreateBy).Result.FullName, items.Action, items.EmailType, null, items.Category, items.GroupName, items.RequestTypeName, items.DetailReason, items.StartDate, items.EndDate, items.OTDate, items.OTDateType, items.OTTimeType, items.StartTime, items.EndTime, items.ExplanationDate, items.ExplanationReason, items.Actual, items.Description, FullName);
                                }

                                result = _systemService.SendMail(toEmail.ToArray(), items.ccMail, items.EmailSubject, Body);
                            }
                        }
                    }
                }
                return request.CreateResponse(HttpStatusCode.OK, true);
            };
            return await CreateHttpResponse(request, func);
        }
    }
}
