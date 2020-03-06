using AutoMapper;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using TMS.Common;
using TMS.Common.Constants;
using TMS.Model.Models;
using TMS.Service;
using TMS.Web.Infrastructure.Core;
using TMS.Web.Infrastructure.Extensions;
using TMS.Web.Models;
using TMS.Web.Models.EntitleDay;
using TMS.Common.ViewModels;
using TMS.Web.Models.EntitleDayManagement;

namespace TMS.Web.Controllers
{
    [Authorize]
    [RoutePrefix(RoutesConstant.EntitleApi)]
    public class EntitleDayController : ApiControllerBase
    {
        private IEntitleDayService _entitleDayService;
        private IEntitleDayAppUserService _entitleDayAppUserService;

        public EntitleDayController(IErrorService errorService, IEntitleDayService entitleDayService,IEntitleDayAppUserService entitleDayAppUserService) : base(errorService)
        {
            _entitleDayService = entitleDayService;
            _entitleDayAppUserService = entitleDayAppUserService;
        }

        /// <summary>
        /// Function Get All Entitle Day
        /// Paging & Sort
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userID"></param>
        /// <param name="groupID"></param>
        /// <param name="column"></param>
        /// <param name="isDesc"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [Route(RoutesConstant.EntitleGetAll)]
        [HttpPost]
        public async Task<HttpResponseMessage> GetAllEntitle(HttpRequestMessage request, string userID, string groupID, string column, bool isDesc, int page, int pageSize, [FromBody] FilterEntitleDay filter)
        {
            Func<HttpResponseMessage> func = () =>
            {
                if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(groupID))
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(userID) + MessageSystem.NoValues + nameof(groupID) + MessageSystem.NoValues);
                }
                var model = _entitleDayService.GetAllOTFilter(userID, groupID, column, isDesc, filter);
                var data = model.Skip((page - 1) * pageSize).Take(pageSize);
                var paginationSet = new PaginationSet<EntitledayModel>()    
                {
                    Items = data,
                    PageIndex = page,
                    TotalRows = model.Count(),
                    PageSize = pageSize
                };
                return request.CreateResponse(HttpStatusCode.OK, paginationSet);

            };
            return await CreateHttpResponse(request, func);
        }
        [Route(RoutesConstant.GetAllType)]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAll(HttpRequestMessage request, string UserID)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var responseData = Mapper.Map<IEnumerable<EntitleDay>, IEnumerable<EntitleDayManagementViewModel>>(_entitleDayService.GetAllTypeUser(UserID));
                return request.CreateResponse(HttpStatusCode.OK, responseData);
            };
            return await CreateHttpResponse(request, func);
        }
        [Route(RoutesConstant.GetAllTypeFilter)]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAllFilterRequest(HttpRequestMessage request, string userID)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var responseData = Mapper.Map<IEnumerable<EntitleDay>, IEnumerable<EntitleDayManagementViewModel>>(_entitleDayService.GetAllType());
                return request.CreateResponse(HttpStatusCode.OK, responseData);
            };
            return await CreateHttpResponse(request, func);
        }

        [Route(RoutesConstant.ExportExcel)]
        [HttpPost]
        public async Task<HttpResponseMessage> ExportToExcel(HttpRequestMessage request, string userID, string groupID, string column, bool isDesc, int page, int pageSize, [FromBody] FilterEntitleDay filter)
        {
            string fileName = string.Concat(CommonConstants.EntitleDay + DateTime.Now.ToString(CommonConstants.dateExport) + CommonConstants.fileExport);
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
                var model = _entitleDayService.GetAllOTFilter(userID, groupID, column, isDesc, filter);
                var responseData = Mapper.Map<List<EntitledayModel>, List<EntitleDayViewModel>>(model);
                for (int i = 0; i < model.Count; i++)
                {
                    responseData[i].FullName = model[i].FullName;
                    responseData[i].Account = model[i].UserName;
                    responseData[i].DayOffType = model[i].HolidayType;
                    responseData[i].Unit = model[i].UnitType;
                    responseData[i].MaximumAllowed = model[i].MaxEntitleDay;
                    responseData[i].Approved = model[i].NumberDayOff;
                    responseData[i].Remain = model[i].RemainDayOff;
                }
                await ReportHelper.GenerateXls(responseData, fullPath);
                return request.CreateResponse(HttpStatusCode.OK, fileTemplate);
            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }


    }
}