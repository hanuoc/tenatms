using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TMS.Common.Constants;
using TMS.Common.ViewModels;
using TMS.Model.Models;
using TMS.Service;
using TMS.Web.Infrastructure.Core;
using TMS.Web.Infrastructure.Extensions;
using TMS.Web.Models.EntitleDay;

namespace TMS.Web.Controllers
{
    [Authorize]
    [RoutePrefix(RoutesConstant.EntitleDayAppUserApi)]
    public class EntitleDayAppUserController : ApiControllerBase
    {
        private IEntitleDayAppUserService _entitleDayAppUserService;
        private IEntitleDayService _entitleDayService;
        public EntitleDayAppUserController(IErrorService errorService, IEntitleDayAppUserService entitleDayAppUserService, IEntitleDayService entitleDayService) : base(errorService)
        {
            _entitleDayAppUserService = entitleDayAppUserService;
            _entitleDayService = entitleDayService;
        }
        [Route(RoutesConstant.EntitleDayAppUserDetail)]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAllEntitleDayAppUser(HttpRequestMessage request, int id)
        {
            Func<HttpResponseMessage> func = () =>
            {
                //var model = _entitleDayAppUserService.GetByID(id);
                var model = _entitleDayAppUserService.GetModelById(id);
                return request.CreateResponse(HttpStatusCode.OK, model);
            };
            return await CreateHttpResponse(request, func);

        }

        [Route(RoutesConstant.EntitleDayAppUserUpdate)]
        [HttpPut]
        public async Task<HttpResponseMessage> Update(HttpRequestMessage request, EntitleDay_AppUserModel entitleDay_AppUserModel)
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

                    var dbEntitleDay = _entitleDayAppUserService.GetById(entitleDay_AppUserModel.EntitleDayAppUserId);
                    //if (entitleDay_AppUserModel.MaxEntitleDay == dbEntitleDay.MaxEntitleDayAppUser)
                    //{
                        dbEntitleDay.UpdateEntitleDayAppUser(entitleDay_AppUserModel);
                        if (entitleDay_AppUserModel.NumberDayOff > entitleDay_AppUserModel.MaxEntitleDay)
                        {
                            return request.CreateResponse(HttpStatusCode.BadRequest, Common.Constants.MessageSystem.MaxEntitleDayNumberDay_Error);
                        }
                        else if (entitleDay_AppUserModel.UnitType.Equals(CommonConstants.DayPeriod))
                        {
                            return request.CreateResponse(HttpStatusCode.BadRequest, Common.Constants.MessageSystem.MaxEntitleDayNumberDay_Error);
                        }
                        else
                        {
                            if (entitleDay_AppUserModel.NumberDayOff % CommonConstants.ZERO_PONT_FIVE == (CommonConstants.ZERO))
                            {
                                _entitleDayAppUserService.UpdateEntitleDayAppUser(dbEntitleDay);
                                response = request.CreateResponse(HttpStatusCode.Created, dbEntitleDay);
                            }
                            else
                            {
                                return request.CreateResponse(HttpStatusCode.BadRequest, Common.Constants.MessageSystem.NumberDayOff_Error);
                            }
                        //}
                    }
                }
                return response;
            });

        }
    }
}