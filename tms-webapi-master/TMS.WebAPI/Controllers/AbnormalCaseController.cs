using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TMS.Common;
using TMS.Common.Constants;
using TMS.Common.ViewModels;
using TMS.Model.Models;
using TMS.Service;
using TMS.Web.Infrastructure.Core;
using TMS.Web.Infrastructure.Extensions;
using TMS.Web.Models.AbnormalCase;

namespace TMS.Web.Controllers
{
    [Authorize]
    [RoutePrefix(RoutesConstant.AbnormalCaseApi)]

    public class AbnormalCaseController : ApiControllerBase
    {
        private IAbnormalCaseService _abnormalCaseService;
        public AbnormalCaseController(IErrorService errorService, IAbnormalCaseService abnormalCaseService) : base(errorService)
        {
            this._abnormalCaseService = abnormalCaseService;
        }
        /// <summary>
        /// Get All list abnormalcase after filter, paging,sort
        /// </summary>
        /// <param name="userID">id of user logged</param>
        /// <param name="groupID">groupId of user logged</param>
        /// <param name="column">name of column to sort</param>
        /// <param name="isDesc">parameter to check to sort by asc or desc</param>
        /// <param name="page">current page(use in paging)</param>
        /// <param name="pageSize">>number of page showing(use in paging)</param>
        /// <param name="filter">list conditions to filter</param>
        /// <returns> HttpResponseMessage </returns>
        [Route(RoutesConstant.GetAllAbnormalCaseByUser)]
        [HttpPost]
        public async Task <HttpResponseMessage> GetAllAbnormalCase(HttpRequestMessage request, string userID, string groupID, string column, bool isDesc, int page, int pageSize, [FromBody] FiterAbnormalViewModel filter)
        {
            Func<HttpResponseMessage> func = () =>
            {
                if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(groupID))
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(userID) + MessageSystem.NoValues + nameof(groupID) + MessageSystem.NoValues);
                }
                var model = _abnormalCaseService.GetAbnormalViewModel(userID, groupID, filter);
                var responseData = string.IsNullOrEmpty(column) ? model.Skip((page - 1) * pageSize).Take(pageSize) : model.OrderByField(column, isDesc).Skip((page - 1) * pageSize).Take(pageSize);
                var dataConverted = _abnormalCaseService.MapAbsentField(responseData);
                var paginationSet = new PaginationSet<AbnormalCaseModel>()
                {
                    Items = dataConverted,
                    PageIndex = page,
                    TotalRows = model.Count(),
                    PageSize = pageSize
                };
                return request.CreateResponse(HttpStatusCode.OK, paginationSet);
            };
            return await CreateHttpResponse(request, func);
        }


        [Route("getabnormalbyid/{id}")]
        [HttpGet]
        public async Task< HttpResponseMessage> GetAbnormalById(HttpRequestMessage request, int id)
        {
            return await CreateHttpResponse(request, () =>
            {
                return request.CreateResponse(HttpStatusCode.OK, GetAbnormalViewModel(id));
            });
        }

        public AbnormalCaseViewModel GetAbnormalViewModel(int id)
        {
            var model = _abnormalCaseService.GetById(id);
            AbnormalCaseViewModel abnormalModel = new AbnormalCaseViewModel
            {
                ID = model.ID,
                TimeSheetId = model.TimeSheetID,
                FullName = model.FingerTimeSheet.FingerMachineUsers.AppUser.FullName,
                GroupName = model.FingerTimeSheet.FingerMachineUsers.AppUser.Group.Name,
                AbnormalDate = model.FingerTimeSheet.DayOfCheck,
                Absent = model.FingerTimeSheet.Absent,
                CheckIn = model.FingerTimeSheet.CheckIn,
                CheckOut = model.FingerTimeSheet.CheckOut,
                AbnormalReason = _abnormalCaseService.GetAbnormalById(model.TimeSheetID).Select(m => new AbnormalReasonModel
                {
                    ID = m.AbnormalReason.ID,
                    Name = m.AbnormalReason.Name
                }).ToList()
            };
            return ConvertSubstringToString(abnormalModel);
        }

        public AbnormalCaseViewModel ConvertSubstringToString(AbnormalCaseViewModel model)
        {
            for (int i = 0; i < model.AbnormalReason.Count; i++)
            {
                if (i < model.AbnormalReason.Count - 1)
                {
                    model.ReasonList += model.AbnormalReason[i].Name + ", ";
                }
                else
                {
                    model.ReasonList += model.AbnormalReason[i].Name;
                }
            }
            return model;
        }
        [Route(RoutesConstant.ExportExcel)]
        [HttpPost]
        public async Task<HttpResponseMessage> ExportToExcel(HttpRequestMessage request, string userID, string groupID, string column, bool isDesc, int page, int pageSize, [FromBody] FiterAbnormalViewModel filter)
        {
            string fileName = string.Concat(CommonConstants.FunctionAbnormalCase + DateTime.Now.ToString(CommonConstants.dateExport) + CommonConstants.fileExport);
            var folderReport = ConfigHelper.GetByKey(CommonConstants.reportFolder);
            string fileTemplate = folderReport + CommonConstants.Link + fileName;
            string filePath = HttpContext.Current.Server.MapPath(folderReport);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fullPath = Path.Combine(filePath, fileName);
            try
            {
                var model = _abnormalCaseService.GetAbnormalViewModel(userID, groupID, filter);
                var responseData = Mapper.Map<List<AbnormalCaseModel>, List<AbnormalExcel>>(model);
                for (int i = 0; i < model.Count; i++)
                {
                    responseData[i].FullName = model[i].FullName;
                    responseData[i].Group = model[i].GroupName;
                    responseData[i].AbnormalDate = model[i].AbnormalDate.Value.Date.ToString(CommonConstants.FormatDate_DDMMYYY);
                    var abnormalreason = model[i].AbnormalReason;
                    foreach (var item in abnormalreason)
                    {
                        responseData[i].ReasonType += item.Name + "\n";
                    }
                    var absent = model[i].Absent;
                    if (absent == StringConstants.TimeSheetAbsentMorning)
                    {
                        absent = StringConstants.AbsentMorning;
                        responseData[i].AbsentType = absent;
                    }
                    if (absent == StringConstants.TimeSheetAbsentAfternoon)
                    {
                        absent = StringConstants.AbsentAfternoon;
                        responseData[i].AbsentType = absent;
                    }
                    if (absent == StringConstants.TimeSheetAbsent)
                    {
                        absent = StringConstants.Absent;
                        responseData[i].AbsentType = absent;
                    }
                    responseData[i].Status = model[i].StatusRequest;
                }
                await ReportHelper.GenerateXlsAbnormal(responseData, fullPath);
                return request.CreateResponse(HttpStatusCode.OK, fileTemplate);
            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Route(RoutesConstant.AbnormalChart)]
        [HttpPost]
        public async Task<HttpResponseMessage> AbnormalChart(HttpRequestMessage request,DateTime DateStart, DateTime DateEnd, [FromBody] List<string> ListGroupId)
        {
            return await CreateHttpResponse(request, () =>
            {
                var model = _abnormalCaseService.AbnormalChart(ListGroupId,DateStart, DateEnd);
                return request.CreateResponse(HttpStatusCode.OK, model);
            });
        }
        /// <summary>
        /// get chart abnormal
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route(RoutesConstant.AbnormalChartPercent)]
        [HttpGet]
        public async Task<HttpResponseMessage> AbnormalChartPercent(HttpRequestMessage request)
        {
            return await CreateHttpResponse(request, () =>
            {
                var model = _abnormalCaseService.AbnormalChartPercent();
                return request.CreateResponse(HttpStatusCode.OK, model);
            });
        }

        /// <summary>
        /// get number abnormal by user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route(RoutesConstant.abnormalByUser)]
        [HttpGet]
        public async Task<HttpResponseMessage> AbnormalByUser(HttpRequestMessage request,string userID)
        {
            return await CreateHttpResponse(request, () =>
            {
                var model = _abnormalCaseService.AbnormalByUser(userID);
                return request.CreateResponse(HttpStatusCode.OK, model);
            });
        }
    }
}