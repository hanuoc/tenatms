using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Common;
using TMS.Common.Constants;
using TMS.Common.Enums;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Model.Models;

namespace TMS.Service
{
    public interface IJobLogService
    {
        string AddLogAndExcuteJob(JobLogEnum key, bool status, DateTime? dateLog);
    }
    public class JobLogService : IJobLogService
    {
        private IJobLogRepository _jobLogRepository;
        private IScheduleService _scheduleService;
        private ISystemService _systemService;
        private ICommonService _commonService;
        private IUnitOfWork _unitOfWork;
        public JobLogService(IJobLogRepository jobLogRepository, IScheduleService scheduleService, ISystemService systemService, ICommonService commonService, IUnitOfWork unitOfWork)
        {
            _jobLogRepository = jobLogRepository;
            _scheduleService = scheduleService;
            _systemService = systemService;
            _commonService = commonService;
            _unitOfWork = unitOfWork;
        }
        public string AddLogAndExcuteJob(JobLogEnum key, bool status, DateTime? dateLog)
        {
            string message = "";
            if (!dateLog.HasValue)
            {
                dateLog = DateTime.Now.Date;
            }
            bool sendMailFlag = false;
            JobLog jobLog = _jobLogRepository.GetSingleByCondition(x => x.Date == dateLog);
            if (jobLog != null)
            {
                // Update
                switch (key)
                {
                    case JobLogEnum.ImportTimeSheet:
                        if (jobLog.ImportTimeSheet)
                            return "Job Import TimeSheet Had Done(" + dateLog.Value.ToShortDateString() + "). Dont Need Excute Again!";
                        jobLog.ImportTimeSheet = _scheduleService.ImportTimeSheet(out message, dateLog.Value);
                        sendMailFlag = !jobLog.ImportTimeSheet;
                        _jobLogRepository.Update(jobLog);
                        break;
                    case JobLogEnum.ChangeStatus:
                        if (jobLog.ChangeStatus)
                            return "Job Change Status Had Done(" + dateLog.Value.ToShortDateString() + "). Dont Need Excute Again!";
                        jobLog.ChangeStatus = _scheduleService.JobChangeStatus(out message);
                        sendMailFlag = !jobLog.ChangeStatus;
                        _jobLogRepository.Update(jobLog);
                        break;
                    case JobLogEnum.UpdateEntitleDay:
                        if (jobLog.UpdateEntitleDay)
                            return "Job Update Entitle Day Had Done(" + dateLog.Value.ToShortDateString() + "). Dont Need Excute Again!";
                        jobLog.UpdateEntitleDay = _scheduleService.JobUpdateEntitleDayByRequest(out message, dateLog.Value);
                        sendMailFlag = !jobLog.UpdateEntitleDay;
                        _jobLogRepository.Update(jobLog);
                        break;

                    case JobLogEnum.ResetEntitleDay:
                        if (jobLog.ResetEntitleDay)
                            return "Job Reset Entitle Day Had Done(" + dateLog.Value.ToShortDateString() + "). Dont Need Excute Again!";
                        jobLog.ResetEntitleDay = _scheduleService.JobEntitleDay(out message, dateLog.Value);
                        sendMailFlag = !jobLog.ResetEntitleDay;
                        _jobLogRepository.Update(jobLog);
                        break;
                }
            }
            else
            {
                // Insert
                switch (key)
                {
                    case JobLogEnum.ImportTimeSheet:
                        _jobLogRepository.Add(InitJobLogAndExcuteJob(JobLogEnum.ImportTimeSheet, status, dateLog.Value, ref message, ref sendMailFlag));
                        break;
                    case JobLogEnum.ChangeStatus:
                        _jobLogRepository.Add(InitJobLogAndExcuteJob(JobLogEnum.ChangeStatus, status, dateLog.Value, ref message, ref sendMailFlag));
                        break;
                    case JobLogEnum.UpdateEntitleDay:
                        _jobLogRepository.Add(InitJobLogAndExcuteJob(JobLogEnum.UpdateEntitleDay, status, dateLog.Value, ref message, ref sendMailFlag));
                        break;
                    case JobLogEnum.ResetEntitleDay:
                        _jobLogRepository.Add(InitJobLogAndExcuteJob(JobLogEnum.ResetEntitleDay, status, dateLog.Value, ref message, ref sendMailFlag));
                        break;
                }
            }
            _unitOfWork.Commit();
            if (sendMailFlag)
            {
                SendMailJob(key, dateLog.Value);
            }
            return message;
        }
        private JobLog InitJobLogAndExcuteJob(JobLogEnum key, bool status, DateTime dateLog, ref string message, ref bool sendMailFlag)
        {
            JobLog jobLog = new JobLog();
            switch (key)
            {
                case JobLogEnum.ImportTimeSheet:
                    jobLog.ImportTimeSheet = _scheduleService.ImportTimeSheet(out message, dateLog);
                    sendMailFlag = !jobLog.ImportTimeSheet;
                    jobLog.ChangeStatus = false;
                    jobLog.UpdateEntitleDay = false;
                    jobLog.ResetEntitleDay = false;
                    jobLog.Date = dateLog;
                    break;
                case JobLogEnum.ChangeStatus:
                    jobLog.ImportTimeSheet = false;
                    jobLog.ChangeStatus = _scheduleService.JobChangeStatus(out message);
                    sendMailFlag = !jobLog.ChangeStatus;
                    jobLog.UpdateEntitleDay = false;
                    jobLog.ResetEntitleDay = false;
                    jobLog.Date = dateLog;
                    break;
                case JobLogEnum.UpdateEntitleDay:
                    jobLog.ImportTimeSheet = false;
                    jobLog.ChangeStatus = false;
                    jobLog.UpdateEntitleDay = _scheduleService.JobUpdateEntitleDayByRequest(out message, dateLog);
                    sendMailFlag = !jobLog.UpdateEntitleDay;
                    jobLog.ResetEntitleDay = false;
                    jobLog.Date = dateLog;
                    break;
                case JobLogEnum.ResetEntitleDay:
                    jobLog.ImportTimeSheet = false;
                    jobLog.ChangeStatus = false;
                    jobLog.UpdateEntitleDay = false;
                    jobLog.ResetEntitleDay = _scheduleService.JobEntitleDay(out message, dateLog);
                    sendMailFlag = !jobLog.ResetEntitleDay;
                    jobLog.Date = dateLog;
                    break;
            }
            return jobLog;
        }
        private void SendMailJob(JobLogEnum key, DateTime dateLog)
        {
            string jobName = string.Empty;
            string jobLink = string.Empty;
            switch (key)
            {
                case JobLogEnum.ImportTimeSheet:
                    jobName = StringConstants.JobTimeSheet;
                    jobLink = CombineLink(JobLogEnum.ImportTimeSheet, dateLog);
                    break;
                case JobLogEnum.ChangeStatus:
                    jobName = StringConstants.JobChangeStatus;
                    jobLink = CombineLink(JobLogEnum.ChangeStatus, dateLog);
                    break;
                case JobLogEnum.UpdateEntitleDay:
                    jobName = StringConstants.JobUpdateEntitleDay;
                    jobLink = jobLink = CombineLink(JobLogEnum.UpdateEntitleDay, dateLog);
                    break;
                case JobLogEnum.ResetEntitleDay:
                    jobName = StringConstants.JobResetEntitleDay;
                    jobLink = CombineLink(JobLogEnum.ResetEntitleDay, dateLog);
                    break;
            }
            string[] emails = { Common.ConfigHelper.GetByKey("EmailAdmin") };
            string body = _systemService.getBodyMailNotificationJoblog(Common.Constants.MailConsstants.TemplateNotificationJob, jobName, jobLink);
            _systemService.SendMail(emails, null, Common.Constants.MailConsstants.SubjectMailJob, body);
        }
        private string CombineLink(JobLogEnum key, DateTime dateLog)
        {
            string api = string.Empty;
            switch (key)
            {
                case JobLogEnum.ImportTimeSheet:
                    api = RoutesConstant.AutoImportTimeSheet;
                    return ConfigHelper.GetByKey("TMSAPI") + "/" + RoutesConstant.Schedule + "/" + api + "?security=" + _commonService.CreateMD5(ConfigHelper.GetByKey("SecurityWindowService")) + "&date=" + dateLog.ToString("dd'/'MM'/'yyyy");
                case JobLogEnum.ChangeStatus:
                    api = RoutesConstant.JobChangeStatus;
                    return ConfigHelper.GetByKey("TMSAPI") + "/" + RoutesConstant.Schedule + "/" + api + "?security=" + _commonService.CreateMD5(ConfigHelper.GetByKey("SecurityWindowService"));
                case JobLogEnum.UpdateEntitleDay:
                    api = RoutesConstant.JobUpdateEntitleDayByRequest;
                    return ConfigHelper.GetByKey("TMSAPI") + "/" + RoutesConstant.Schedule + "/" + api + "?security=" + _commonService.CreateMD5(ConfigHelper.GetByKey("SecurityWindowService")) + "&date=" + dateLog.ToString("dd'/'MM'/'yyyy");
                case JobLogEnum.ResetEntitleDay:
                    api = RoutesConstant.JobEntitleDay;
                    return ConfigHelper.GetByKey("TMSAPI") + "/" + RoutesConstant.Schedule + "/" + api + "?security=" + _commonService.CreateMD5(ConfigHelper.GetByKey("SecurityWindowService")) + "&date=" + dateLog.ToString("dd'/'MM'/'yyyy");
                default:
                    return string.Empty;
            }
        }
    }
}
