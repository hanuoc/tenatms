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
using TMS.Web.Models.OTRequest;
using TMS.Common.ViewModels;
using TMS.Web.Models.Common;
using TMS.Web.Models.OTList;
using System.Globalization;
using System.Net.Http.Headers;

namespace TMS.Web.Controllers
{
    [Authorize]
    [RoutePrefix(RoutesConstant.OTList)]
    public class ListOTController : ApiControllerBase
    {
        private IListOTService _otrequestService;

        public ListOTController(IErrorService errorService, IListOTService otrequestService) : base(errorService)
        {
            this._otrequestService = otrequestService;
        }
        /// <summary>
        /// Function Get All OT List 
        /// Paging & Sort & Filter
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userID"></param>
        /// <param name="groupID"></param>
        /// <param name="column"></param>
        /// <param name="isDesc"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [Route(RoutesConstant.OTListGetAll)]
        [HttpPost]
        public async Task<HttpResponseMessage> GetAllOtList(HttpRequestMessage request, string userID, string groupID, string column, bool isDesc, int page, int pageSize, [FromBody] FilterOTRequestModel filter)
        {
            Func<HttpResponseMessage> func = () =>
            {
                if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(groupID))
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(userID) + MessageSystem.NoValues + nameof(groupID) + MessageSystem.NoValues);
                }
                var model = _otrequestService.GetAllOTFilter(userID, groupID, column, isDesc, filter);
                var data = model.Skip((page - 1) * pageSize).Take(pageSize);
                //var data = string.IsNullOrEmpty(column) ? model.Skip((page - 1) * pageSize).Take(pageSize) : model.OrderByField(column, isDesc).Skip((page - 1) * pageSize).Take(pageSize);
                var paginationSet = new PaginationSet<ListOTModel>()
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
        /// <summary>
        /// Export Excel
        /// Download Excel
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userID"></param>
        /// <param name="groupID"></param>
        /// <param name="column"></param>
        /// <param name="isDesc"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [Route(RoutesConstant.ExportExcel)]
        [HttpPost]
        public async Task<HttpResponseMessage> ExportToExcel(HttpRequestMessage request, string userID, string groupID, string column, bool isDesc, int page, int pageSize, [FromBody] FilterOTRequestModel filter)
        {
            string fileName = string.Concat(CommonConstants.FunctionOTList + DateTime.Now.ToString(CommonConstants.dateExport) + CommonConstants.fileExport);         
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
                var model = _otrequestService.GetAllOTFilter(userID, groupID, column, isDesc, filter);
                var responseData = Mapper.Map<List<ListOTModel>, List<OTListViewModel>>(model);
                for (int i = 0; i < model.Count; i++)
                {
                    responseData[i].FullName = model[i].FullName;
                    responseData[i].Account = model[i].UserName;
                    responseData[i].Group = model[i].GroupName;
                    responseData[i].OTDate = model[i].OTDate.Value.Date.ToString(CommonConstants.FormatDate_DDMMYYY);
                    responseData[i].OTDayType = model[i].NameOTDateType;
                    responseData[i].OTTimeType = model[i].NameOTDateTime;
                    responseData[i].OTCheckIn = model[i].OTCheckIn;
                    responseData[i].OTCheckOut = model[i].OTCheckOut;
                    responseData[i].WorkingTime = model[i].WorkingTime + CommonConstants.Hours;
                    responseData[i].Approver = model[i].UpdatedByName;
                    responseData[i].Status = model[i].StatusRequest;   
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