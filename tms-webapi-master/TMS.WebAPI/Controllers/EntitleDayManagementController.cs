using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TMS.Common.Constants;
using TMS.Common.Exceptions.Extensions;
using TMS.Common.ViewModels;
using TMS.Model.Models;
using TMS.Service;
using TMS.Web.Infrastructure.Core;
using TMS.Web.Infrastructure.Extensions;
using TMS.Web.Models.EntitleDayManagement;

namespace TMS.Web.Controllers
{
    [Authorize]
    [RoutePrefix(RoutesConstant.EntitleDayManagementApi)]
    public class EntitleDayManagementController : ApiControllerBase
    {
        private IEntitleDayManagemantService _entitleDayManagemantService;
        private IEntitleDayService _entitleDayService;
        private IEntitleDayAppUserService _entitleDayAppUserService;
        private IRequestService _requestService;

        public EntitleDayManagementController(IRequestService requestService, IEntitleDayAppUserService entitleDayAppUserService, IErrorService errorService, IEntitleDayManagemantService entitleDayManagemantService, IEntitleDayService entitleDayService) : base(errorService)
        {
            this._entitleDayManagemantService = entitleDayManagemantService;
            _entitleDayService = entitleDayService;
            _entitleDayAppUserService = entitleDayAppUserService;
            _requestService = requestService;
        }
        [Route(RoutesConstant.EntitleDayManagementGetAll)]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAllEntitleDayManagement(HttpRequestMessage request)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var model = _entitleDayManagemantService.GetAllEntitleDayManagement();
                return request.CreateResponse(HttpStatusCode.OK, model);
            };
            return await CreateHttpResponse(request, func);
        }
        /// <summary>
        /// Function Create
        /// - Check Holiday type
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        [Route(RoutesConstant.EntitleDayManagementAdd)]
        [HttpPost]
        public async Task<HttpResponseMessage> Create(HttpRequestMessage request, EntitleDayManagementViewModel entitleDayManagementViewModel)
        {
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else if (_entitleDayManagemantService.CheckHolidayType(entitleDayManagementViewModel.HolidayType, entitleDayManagementViewModel.ID))
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, Common.Constants.MessageSystem.Create_EntitleDayManagement_Error);
                }
                else if (entitleDayManagementViewModel.MaxEntitleDay == CommonConstants.MinEntitleDay || entitleDayManagementViewModel.MaxEntitleDay > CommonConstants.MaxEntitleDay)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, Common.Constants.MessageSystem.MaxEntitleDay_Error);
                }
                else if (entitleDayManagementViewModel.HolidayType == CommonConstants.UnpaidLeave)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, Common.Constants.MessageSystem.NoSalary_Error);
                }
                else
                {
                    EntitleDay newEntitleDay = new EntitleDay();
                    entitleDayManagementViewModel.Status = true;
                    if (entitleDayManagementViewModel.HolidayType == CommonConstants.UnpaidLeave)
                    {
                        entitleDayManagementViewModel.Status = false;
                    }
                    newEntitleDay.UpdateEntitleDayManagement(entitleDayManagementViewModel);
                    var model = _entitleDayManagemantService.Add(newEntitleDay);
                    var responseData = Mapper.Map<EntitleDay, EntitleDayManagementViewModel>(newEntitleDay);

                    _entitleDayService.Add(model);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });

        }
        /// <summary>
        /// Function Update
        /// - Get id Update
        /// - Check Holiday type
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entitleDayManagementViewModel"></param>
        /// <returns></returns>
        [Route(RoutesConstant.EntitleDayManagementUpdate)]
        [HttpPut]
        public async Task<HttpResponseMessage> Update(HttpRequestMessage request, EntitleDayManagementViewModel entitleDayManagementViewModel)
        {
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else if (_entitleDayManagemantService.CheckHolidayType(entitleDayManagementViewModel.HolidayType, entitleDayManagementViewModel.ID))
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, Common.Constants.MessageSystem.Create_EntitleDayManagement_Error);
                }
                else if (entitleDayManagementViewModel.MaxEntitleDay == CommonConstants.MinEntitleDay || entitleDayManagementViewModel.MaxEntitleDay > CommonConstants.MaxEntitleDay)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, Common.Constants.MessageSystem.MaxEntitleDay_Error);
                }
                else
                {
                    var dbEntitleDay = _entitleDayManagemantService.GetByIdEntitleDay(entitleDayManagementViewModel.ID);
                    float numberDayOff = entitleDayManagementViewModel.MaxEntitleDay - dbEntitleDay.MaxEntitleDay;
                    dbEntitleDay.UpdateEntitleDayManagement(entitleDayManagementViewModel);
                    _entitleDayManagemantService.Update(dbEntitleDay);
                    var model = _entitleDayAppUserService.GetAll();
                    foreach (var item in model)
                    {
                        if (item.EntitleDayId == dbEntitleDay.ID)
                        {
                            item.MaxEntitleDayAppUser = item.MaxEntitleDayAppUser + numberDayOff;
                            item.MaxEntitleDayAppUser = (item.MaxEntitleDayAppUser < 0) ? 0 : item.MaxEntitleDayAppUser;
                            _entitleDayAppUserService.Update(item);
                        }
                    }
                    _entitleDayAppUserService.Save();
                    response = request.CreateResponse(HttpStatusCode.Created, dbEntitleDay);
                }
                return response;
            });

        }


        /// <summary>
        /// Function Delete
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route(RoutesConstant.EntitleDayManagementDelete)]
        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(HttpRequestMessage request, int id)
        {
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var requestEntitleDay = _requestService.GetRequestAll(id);
                    if (requestEntitleDay == true)
                    {
                        _entitleDayManagemantService.Delete(id);
                        var model = _entitleDayAppUserService.GetAll();
                        foreach (var item in model)
                        {
                            if (item.EntitleDayId == id)
                            {
                                _entitleDayAppUserService.Delete(item.EntitleDayId);
                            }
                        }
                        _entitleDayAppUserService.Save();
                        response = request.CreateResponse(HttpStatusCode.OK, "OK");
                    }
                    else
                    {
                        response = request.CreateResponse(HttpStatusCode.BadRequest, CommonConstants.Request_EntitleDay_Error);
                    }
                }
                return response;
            });
        }
        /// <summary>
        /// Function get id page detail
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route(RoutesConstant.EntitleDayManagementDetail)]
        [HttpGet]
        public async Task<HttpResponseMessage> GetDetailEntitleDayManagement(HttpRequestMessage request, int id)
        {
            return await CreateHttpResponse(request, () =>
            {
                var model = _entitleDayManagemantService.GetByIdEntitleDay(id);
                var responseData = Mapper.Map<EntitleDay, EntitleDayManagementViewModel>(model);
                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }
    }
}