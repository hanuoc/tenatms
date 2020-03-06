using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TMS.Common.Constants;
using TMS.Model.Models;
using TMS.Service;
using TMS.Web.Infrastructure.Core;
using TMS.Web.Infrastructure.Extensions;
using TMS.Web.Models.TimeDay;

namespace TMS.Web.Controllers
{
    [Authorize]
    [RoutePrefix(RoutesConstant.TimeDay)]
    public class TimeDayController : ApiControllerBase
    {
        private ITimeDayService _timeDayService;
        public TimeDayController(IErrorService errorService , ITimeDayService timeDayService) : base(errorService)
        {
            this._timeDayService = timeDayService;
        }
        /// <summary>
        /// Get all timeday
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route(RoutesConstant.TimeDayList)]
        [HttpPost]
        public async Task<HttpResponseMessage> GetAll(HttpRequestMessage request)
        {
            return await CreateHttpResponse(request, () =>
            {
                var listTimeDay = _timeDayService.GetAllTimeDay();
                foreach (var item in listTimeDay)
                {
                    item.CheckIn = Convert.ToDateTime(item.CheckIn).ToString(Common.Constants.StringConstants.FormatTime12)  ;
                    item.CheckOut = Convert.ToDateTime(item.CheckOut).ToString(Common.Constants.StringConstants.FormatTime12) ;
                }

                var listTimeDayVm = Mapper.Map<List<TimeDayViewModel>>(listTimeDay);

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, listTimeDayVm);

                return response;
            });
        }
        /// <summary>
        /// Create timeday
        /// </summary>
        /// <param name="request"></param>
        /// <param name="timedayVm"></param>
        /// <returns></returns>
        [Route(RoutesConstant.Add)]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateTimeDay(HttpRequestMessage request, TimeDayViewModel timedayVm)
        {
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    if (_timeDayService.CheckEqual(timedayVm.Workingday, timedayVm.ID))
                    {
                        return request.CreateResponse(HttpStatusCode.BadRequest, Common.Constants.MessageSystem.CheckExits);
                    }
                    var checkConditon = _timeDayService.CheckConditionTime(timedayVm.CheckIn, timedayVm.CheckOut);
                    if (!string.IsNullOrEmpty(checkConditon))
                    {
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, checkConditon);
                    }
                    TimeDay newTimeday = new TimeDay();
                    newTimeday.UpdateTimeday(timedayVm);
                    var timeDay = _timeDayService.Add(newTimeday);
                    var responseData = Mapper.Map<TimeDay, TimeDayViewModel>(timeDay);
                    _timeDayService.SaveChange();

                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }
        /// <summary>
        /// Update time day
        /// </summary>
        /// <param name="request"></param>
        /// <param name="timedayVm"></param>
        /// <returns></returns>
        [Route(RoutesConstant.Update)]
        [HttpPut]
        public async Task<HttpResponseMessage> UpdateTimeday(HttpRequestMessage request, TimeDayViewModel timedayVm)
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
                    if (_timeDayService.CheckEqual(timedayVm.Workingday, timedayVm.ID))
                    {
                        return request.CreateResponse(HttpStatusCode.BadRequest, Common.Constants.MessageSystem.CheckExits);
                    }
                    var checkConditon = _timeDayService.CheckConditionTime(timedayVm.CheckIn, timedayVm.CheckOut);
                    if (!string.IsNullOrEmpty(checkConditon))
                    {
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, checkConditon);
                    }
                    var dbTimeday = _timeDayService.GetbyId(timedayVm.ID);
                    dbTimeday.UpdateTimeday(timedayVm);
                    _timeDayService.Update(dbTimeday);
                    _timeDayService.SaveChange();
                    response = request.CreateResponse(HttpStatusCode.OK,"Okie");
                }
                return response;
            });
        }
        /// <summary>
        /// delete time day
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route(RoutesConstant.Delete)]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteTimeday(HttpRequestMessage request, int id)
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
                    _timeDayService.Delete(id);
                    _timeDayService.SaveChange();
                    response = request.CreateResponse(HttpStatusCode.OK, "OK");
                }
                return response;
            });
        }
        /// <summary>
        /// Get time day detail
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route(RoutesConstant.TimeDayDetail)]
        [HttpGet]
        public async Task<HttpResponseMessage> GetById(HttpRequestMessage request, int id)
        {
            return await CreateHttpResponse(request, () =>
            {
                var model = _timeDayService.GetbyId(id);

                var responseData = Mapper.Map<TimeDay, TimeDayViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }
    }
}