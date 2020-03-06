using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TMS.Common.Constants;
using TMS.Common.Enums;
using TMS.Common.ViewModels;
using TMS.Service;
using TMS.Web.Infrastructure.Core;

namespace TMS.Web.Controllers
{
    [RoutePrefix(RoutesConstant.Schedule)]
    public class ScheduleController : ApiControllerBase
    {
        private IScheduleService _scheduleService;
        private IJobLogService _jobLogService;
        private ICommonService _commonService;
        public ScheduleController(IScheduleService scheduleService, IJobLogService jobLogService,ICommonService commonService, IErrorService errorService) : base(errorService)
        {
            _scheduleService = scheduleService;
            _jobLogService = jobLogService;
            _commonService = commonService;
        }
        [Route(RoutesConstant.AutoImportTimeSheet)]
        [HttpGet]
        public async Task<HttpResponseMessage> AutoImportTimeSheet(HttpRequestMessage request, string security, string date)
        {
            return await CreateHttpResponse(request, () =>
            {
                DateTime dateTime = DateTime.ParseExact(date, CommonConstants.FormatDate_DDMMYYY, CultureInfo.InvariantCulture);
                if (security == _commonService.CreateMD5(Common.ConfigHelper.GetByKey("SecurityWindowService")))
                {
                    string messageLog = _jobLogService.AddLogAndExcuteJob(JobLogEnum.ImportTimeSheet, true, dateTime);
                    return request.CreateResponse(HttpStatusCode.OK, "Run import time sheet - Message:" + messageLog);
                }
                else
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, "Access denied !");
                }
            });
        }
        [Route(RoutesConstant.JobChangeStatus)]
        [HttpGet]
        public async Task<HttpResponseMessage> AutoChangeStatus(HttpRequestMessage request, string security)
        {
            return await CreateHttpResponse(request, () =>
            {
                if (security == _commonService.CreateMD5(Common.ConfigHelper.GetByKey("SecurityWindowService")))
                {
                    string messageLog = _jobLogService.AddLogAndExcuteJob(JobLogEnum.ChangeStatus, true, null);
                    return request.CreateResponse(HttpStatusCode.OK, "Run job change status -" + messageLog);
                }
                else
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, "Access denied !");
                }
            });
        }
        [Route(RoutesConstant.JobEntitleDay)]
        [HttpGet]
        public async Task<HttpResponseMessage> JobEntitleDay(HttpRequestMessage request, string security, string date)
        {
            return await CreateHttpResponse(request, () =>
            {
                DateTime dateTime = DateTime.ParseExact(date, CommonConstants.FormatDate_DDMMYYY, CultureInfo.InvariantCulture);
                if (security == _commonService.CreateMD5(Common.ConfigHelper.GetByKey("SecurityWindowService")))
                {
                    string messageLog = _jobLogService.AddLogAndExcuteJob(JobLogEnum.ResetEntitleDay, true, dateTime);
                    return request.CreateResponse(HttpStatusCode.OK, "Run job entitle day " + messageLog);
                }
                else
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, "Access denied !");
                }
            });
        }
        [Route(RoutesConstant.JobUpdateEntitleDayByRequest)]
        [HttpGet]
        public async Task<HttpResponseMessage> JobUpdateEntitleDayByRequest(HttpRequestMessage request, string security, string date)
        {
            return await CreateHttpResponse(request, () =>
            {
                if (security == _commonService.CreateMD5(Common.ConfigHelper.GetByKey("SecurityWindowService")))
                {
                    DateTime dateTime = DateTime.ParseExact(date, CommonConstants.FormatDate_DDMMYYY, CultureInfo.InvariantCulture);
                    try
                    {
                        string messageLog = _jobLogService.AddLogAndExcuteJob(JobLogEnum.UpdateEntitleDay, true, dateTime.Date);
                        return request.CreateResponse(HttpStatusCode.OK, "Run job update entitle day by request " + messageLog);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, "Access denied !");
                }
            });
        }
    }
}
