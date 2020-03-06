using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using TMS.Common.Constants;
using TMS.Model.Models;
using TMS.Service;
using TMS.Web.Infrastructure.Core;
using TMS.Web.Infrastructure.Extensions;
using TMS.Web.Models.Common;
using TMS.Web.Models.OTRequest;

namespace TMS.Web.Controllers
{
    [Authorize]
    [RoutePrefix(RoutesConstant.OTRequestApi)]
    public class OTRequestController : ApiControllerBase
    {
        private IOTRequestService _otrequestService;
        private IOTTimeTypeService _ottimetypeService;
        private IOTDateTypeService _otdatetypeService;
        private IOTRequestUserService _otRequestUserService;
        private IStatusRequestService _statusRequestService;
        private IFingerTimeSheetService _fingerTimeSheetService;
        private ISystemService _systemService;
        private IUserService _userService;
        private ICommonService _commonService;
        /// <summary>
        /// Contructor of OTRequestController class
        /// </summary>
        /// <param name="errorService"></param>
        public OTRequestController(IErrorService errorService,
                                IOTRequestService otrequestService,
                                IOTTimeTypeService ottimetypeService,
                                IOTDateTypeService otdatetypeService,
                                IOTRequestUserService otRequestUserService,
                                IStatusRequestService statusRequestService,
                                IUserService userService,
                                IFingerTimeSheetService fingerTimeSheetService,
                                 ISystemService systemService,
                                ICommonService commonService
                                ) : base(errorService)
        {
            _otrequestService = otrequestService;
            _ottimetypeService = ottimetypeService;
            _otdatetypeService = otdatetypeService;
            _otRequestUserService = otRequestUserService;
            _statusRequestService = statusRequestService;
            _userService = userService;
            _fingerTimeSheetService = fingerTimeSheetService;
            _systemService = systemService;
            _commonService = commonService;
        }
        /// <summary>
        /// Get list ot request by userId and groupID
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userID">Descending or not</param>
        /// <param name="groupID">Number of page</param>
        /// <returns>Retern a list request</returns>
        [Route(RoutesConstant.OTRequestGetAll)]
        [HttpPost]
        public async Task<HttpResponseMessage> GetAll(HttpRequestMessage request, string userID, string groupID, string column, bool isDesc, int page, int pageSize, [FromBody]FilterOTRequestModel filter)
        {
            Func<HttpResponseMessage> func = () =>
            {
                if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(groupID))
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(userID) + MessageSystem.NoValues + nameof(groupID) + MessageSystem.NoValues);
                }
                var model = _otrequestService.GetOTRequestFilter(userID, groupID, column, isDesc, filter);
                var responseData = Mapper.Map<IEnumerable<OTRequest>, IEnumerable<OTRequestViewModel>>(model.Skip((page - 1) * pageSize).Take(pageSize));
                var paginationSet = new PaginationSet<OTRequestViewModel>()
                {
                    Items = responseData,
                    PageIndex = page,
                    TotalRows = model.Count(),
                    PageSize = pageSize
                };
                return request.CreateResponse(HttpStatusCode.OK, paginationSet);
            };
            return await CreateHttpResponse(request, func);
        }
        //Tuong test
        [Route(RoutesConstant.OTRequestGetAllGeneralAdmin)]
        [HttpPost]
        public async Task<HttpResponseMessage> GetAllGeneralAdmin(HttpRequestMessage request, string userID, string groupID, string column, bool isDesc, int page, int pageSize, [FromBody]FilterOTRequestModel filter)
        {
            Func<HttpResponseMessage> func = () =>
            {
                if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(groupID))
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(userID) + MessageSystem.NoValues + nameof(groupID) + MessageSystem.NoValues);
                }
                var model = _otrequestService.GetAllOTRequestByGeneralAdmin(userID, groupID, column, isDesc, filter);
                var responseData = Mapper.Map<IEnumerable<OTRequest>, IEnumerable<OTRequestViewModel>>(model.Skip((page - 1) * pageSize).Take(pageSize));
                var paginationSet = new PaginationSet<OTRequestViewModel>()
                {
                    Items = responseData,
                    PageIndex = page,
                    TotalRows = model.Count(),
                    PageSize = pageSize
                };
                return request.CreateResponse(HttpStatusCode.OK, paginationSet);
            };
            return await CreateHttpResponse(request, func);
        }

        /// <summary>
        /// Get OTReuqest by ID 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route(RoutesConstant.OTRequestDetail)]
        [HttpGet]
        public async Task<HttpResponseMessage> GetById(HttpRequestMessage request, int id)
        {
            return await CreateHttpResponse(request, () =>
            {
                var model = _otrequestService.GetById(id);

                var responseData = Mapper.Map<OTRequest, OTRequestViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }
        /// <summary>
        /// Get OT Request User
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestID"></param>
        /// <returns></returns>
        [Route(RoutesConstant.OTRequestUser)]
        [HttpGet]
        public async Task<HttpResponseMessage> GetOTRequestUser(HttpRequestMessage request, int requestID)
        {
            return await CreateHttpResponse(request, () =>
            {
                var model = _otRequestUserService.GetAll(requestID);

                var responseData = Mapper.Map<IEnumerable<OTRequestUser>, IEnumerable<OTRequestUserViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }
        /// <summary>
        /// Get All OT Date
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route(RoutesConstant.OTDateType)]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAllOTDateType(HttpRequestMessage request)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var model = _otdatetypeService.GetAllOTDateType();
                return request.CreateResponse(HttpStatusCode.OK, model);
            };
            return await CreateHttpResponse(request, func);
        }
        /// <summary>
        /// Get All OT Time
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route(RoutesConstant.OTTimeType)]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAllOTTimeType(HttpRequestMessage request)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var model = _ottimetypeService.GetAllOTTimeType();
                return request.CreateResponse(HttpStatusCode.OK, model);
            };
            return await CreateHttpResponse(request, func);
        }
        /// <summary>
        /// Create new Request 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otRequestVm"></param>
        /// <returns></returns>
        [Route(RoutesConstant.OTRequestCreate)]
        [HttpPost]
        public async Task<HttpResponseMessage> Create(HttpRequestMessage request, string userID, string groupId, OTRequestViewModel otRequestVm)
        {

            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                var tmp = otRequestVm.OTRequestUserID.ToList();
                tmp.Add(otRequestVm.UserID);
                otRequestVm.OTRequestUserID = tmp.Distinct().ToArray();
                var newOTRequest = new OTRequest();
                newOTRequest.UpdateOTRequest(otRequestVm);
                var dateNow = DateTime.Now.Date;
                if (otRequestVm.OTDate == dateNow && _otrequestService.CheckCreateDay(newOTRequest) == false)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_CREATE_OTREQUEST_IN_PAST_NOT_MSG);
                }

                if (_otrequestService.CheckOtRequestUser(newOTRequest, otRequestVm.OTRequestUserID.ToList()) == false)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_CANNOT_CREATE_OT_EXIST);
                }
                if (otRequestVm.OTDate < dateNow)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_CREATE_OTREQUEST_IN_PAST_NOT_MSG);
                }

                // nomal
                if (otRequestVm.OTDate != null && _commonService.isWorkingDay(otRequestVm.OTDate.Value) && otRequestVm.OTDateTypeID != CommonConstants.OTDateTypeNormal)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, otRequestVm.OTDate.Value.Date.ToString("dd/MM/yyyy",
                                CultureInfo.InvariantCulture) + " is " + otRequestVm.OTDate.Value.DayOfWeek + ", "+ MessageSystem.ERROR_MUST_SELECT_NOMAL);
                }

                // weekend
                if (otRequestVm.OTDate != null && _commonService.isWeekend(otRequestVm.OTDate.Value) && otRequestVm.OTDateTypeID != CommonConstants.OTDateTypeWeekend)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, otRequestVm.OTDate.Value.Date.ToString("dd/MM/yyyy",
                                CultureInfo.InvariantCulture) + " is " + otRequestVm.OTDate.Value.DayOfWeek + ", "+ MessageSystem.ERROR_MUST_SELECT_WEEKEND);
                }

                //holiday
                if (otRequestVm.OTDate != null && _commonService.isHoliday(otRequestVm.OTDate.Value) && otRequestVm.OTDateTypeID != CommonConstants.OTDateTypeHoliDay)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, otRequestVm.OTDate.Value.Date.ToString("dd/MM/yyyy",
                                    CultureInfo.InvariantCulture) + " is " + otRequestVm.OTDate.Value.DayOfWeek + ", "+ MessageSystem.ERROR_MUST_SELECT_HOLIDAY);
                }
                else
                {
                    if (TimeSpan.ParseExact(otRequestVm.StartTime, @"hh\:mm", CultureInfo.InvariantCulture) > TimeSpan.ParseExact(otRequestVm.EndTime, @"hh\:mm", CultureInfo.InvariantCulture))
                    {
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, MessageSystem.StartTimeCompareEndTime);
                    }
                    var checkCondition = _otrequestService.CheckConditionCreate(otRequestVm.OTDateTypeID, otRequestVm.OTTimeTypeID, otRequestVm.StartTime, otRequestVm.EndTime);
                    if (!string.IsNullOrEmpty(checkCondition))
                    {
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, checkCondition);
                    }
                    if (_otrequestService.CheckOtRequestUser(newOTRequest, otRequestVm.OTRequestUserID.ToList()) == true)
                    {
                        _otrequestService.Add(newOTRequest, userID);
                        var responseData = Mapper.Map<OTRequest, OTRequestViewModel>(newOTRequest);
                        _otRequestUserService.Add(otRequestVm.OTRequestUserID, newOTRequest.ID);
                        response = request.CreateResponse(HttpStatusCode.Created, responseData);
                    }
                    else
                    {
                        response = request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_CANNOT_CREATE_OT_EXIST);
                    }
                }
                return response;
            });
        }

        [Route(RoutesConstant.OTRequestUpdate)]
        [HttpPut]
        public async Task<HttpResponseMessage> Update(HttpRequestMessage request, OTRequestViewModel oTRequestViewModel)
        {
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                var tmp = oTRequestViewModel.OTRequestUserID.ToList();
                tmp.Add(oTRequestViewModel.UserID);
                oTRequestViewModel.OTRequestUserID = tmp.Distinct().ToArray();
                DateTime? dateViewModel = oTRequestViewModel.OTDate;
                var model = _otrequestService.GetById(oTRequestViewModel.ID);
                var dateNow = DateTime.Now.Date;
                if (oTRequestViewModel.OTDate == dateNow)
                {
                    if (_otrequestService.CheckCreateDay(model) == false)
                    {
                        return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_CREATE_OTREQUEST_IN_PAST_NOT_MSG);
                    }
                }
                if (oTRequestViewModel.OTDate < dateNow)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_CREATE_OTREQUEST_IN_PAST_NOT_MSG);
                }
                // nomal
                if (oTRequestViewModel.OTDate != null && _commonService.isWorkingDay(oTRequestViewModel.OTDate.Value) && oTRequestViewModel.OTDateTypeID != CommonConstants.OTDateTypeNormal)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, oTRequestViewModel.OTDate.Value.Date.ToString("dd/MM/yyyy",
                                                                                 CultureInfo.InvariantCulture) + " is " + oTRequestViewModel.OTDate.Value.DayOfWeek + ", " + MessageSystem.ERROR_MUST_SELECT_NOMAL);
                }

                // weekend
                if (oTRequestViewModel.OTDate != null && _commonService.isWeekend(oTRequestViewModel.OTDate.Value) && oTRequestViewModel.OTDateTypeID != CommonConstants.OTDateTypeWeekend)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, oTRequestViewModel.OTDate.Value.Date.ToString("dd/MM/yyyy",
                                                                                 CultureInfo.InvariantCulture) + " is " + oTRequestViewModel.OTDate.Value.DayOfWeek + ", " + MessageSystem.ERROR_MUST_SELECT_WEEKEND);
                }

                //holiday
                if (oTRequestViewModel.OTDate != null && _commonService.isHoliday(oTRequestViewModel.OTDate.Value) && oTRequestViewModel.OTDateTypeID != CommonConstants.OTDateTypeHoliDay)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, oTRequestViewModel.OTDate.Value.Date.ToString("dd/MM/yyyy",
                                                                                 CultureInfo.InvariantCulture) + " is " + oTRequestViewModel.OTDate.Value.DayOfWeek + ", " + MessageSystem.ERROR_MUST_SELECT_HOLIDAY);
                }
                else
                {
                    if (TimeSpan.ParseExact(oTRequestViewModel.StartTime, @"hh\:mm", CultureInfo.InvariantCulture) > TimeSpan.ParseExact(oTRequestViewModel.EndTime, @"hh\:mm", CultureInfo.InvariantCulture))
                    {
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, MessageSystem.StartTimeCompareEndTime);
                    }
                    var checkCondition = _otrequestService.CheckConditionCreate(oTRequestViewModel.OTDateTypeID, oTRequestViewModel.OTTimeTypeID, oTRequestViewModel.StartTime, oTRequestViewModel.EndTime);
                    if (!string.IsNullOrEmpty(checkCondition))
                    {
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, checkCondition);
                    }
                    else
                    {
                        if (model.OTDate == oTRequestViewModel.OTDate)
                        {
                            model.EditOTRequest(oTRequestViewModel);
                            _otrequestService.UpdateOTRequest(model);
                            response = request.CreateResponse(HttpStatusCode.OK,"done");
                            return response;
                        }
                        model.EditOTRequest(oTRequestViewModel);
                        if (_otrequestService.CheckOtRequestUser(model, oTRequestViewModel.OTRequestUserID.ToList()) == true)
                        {
                            _otrequestService.UpdateOTRequest(model);
                            response = request.CreateResponse(HttpStatusCode.OK, "done");
                            return response;
                        }
                        else
                        {
                            response = request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_CANNOT_CREATE_OT_EXIST);
                        }
                    }
                }
                return response;
            });
        }
        /// <summary>
        /// Change Status Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestID"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        [Route(RoutesConstant.OTRequestChangeStatus)]
        [HttpPost]
        public async Task<HttpResponseMessage> ChangeStatus(HttpRequestMessage request, int requestID, string action)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var user = User.Identity.GetUserId();
                var dateNow = DateTime.Now.Date;
                var otrequestDate = _otrequestService.GetById(requestID).OTDate.Value.AddDays(2);
                if (otrequestDate < dateNow)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_CHANGESTATUS_OTREQUEST_IN_PAST_NOT_MSG);
                }
                var model = _otrequestService.ChangeStatus(requestID, action, user);
                return request.CreateErrorResponse(HttpStatusCode.OK, MessageSystem.ChangeStatusSuccess);
            };
            return await CreateHttpResponse(request, func);
        }
        /// <summary>
        /// Change Status Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestID"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        [Route(RoutesConstant.OTRequestChangeStatusMulti)]
        [HttpPost]
        public async Task<HttpResponseMessage> ChangeStatusMulti(HttpRequestMessage request, int[] requestID, string action)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var result = _otrequestService.ChangeStatusMulti(requestID, action);
                if (!string.IsNullOrEmpty(result))
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_CHANGESTATUS_OTREQUEST_IN_PAST_NOT_MSG);
                }
                return request.CreateErrorResponse(HttpStatusCode.OK, MessageSystem.CancelFail);
            };
            return await CreateHttpResponse(request, func);
        }
        /// <summary>
        /// gett all create by of otrequest
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [Route(RoutesConstant.GetAllCreateByOTRequest)]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAllCreateByOTRequest(HttpRequestMessage request, string groupID)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var model = _otrequestService.GetCreatedByOTRequest(groupID);
                return request.CreateResponse(HttpStatusCode.OK, model);
            };
            return await CreateHttpResponse(request, func);
        }
        /// <summary>
        /// gett all create by of otrequest
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [Route(RoutesConstant.GetOTRequestByTimeSheet)]
        [HttpGet]
        public async Task<HttpResponseMessage> GetOTRequestByTimeSheet(HttpRequestMessage request, int timesheetID, string userId)
        {

            return await CreateHttpResponse(request, () =>
            {
                var timesheet = _fingerTimeSheetService.GetById(timesheetID);
                var model = _otrequestService.GetByTimeSheet(timesheet.DayOfCheck, userId);

                var responseData = Mapper.Map<OTRequest, OTRequestViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }

        /// <summary>
        /// get ot request chart
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [Route(RoutesConstant.OTRequestChart)]
        [HttpGet]
        public async Task<HttpResponseMessage> OTRequestChart(HttpRequestMessage request, int groupID)
        {

            return await CreateHttpResponse(request, () =>
            {
                var model = _otrequestService.OTRequestChart(groupID);
                var response = request.CreateResponse(HttpStatusCode.OK, model);

                return response;
            });
        }

        /// <summary>
        /// get number of ot reqeust by user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [Route(RoutesConstant.otrequestByUser)]
        [HttpGet]
        public async Task<HttpResponseMessage> OTRequestChart(HttpRequestMessage request, string userID)
        {

            return await CreateHttpResponse(request, () =>
            {
                var model = _otrequestService.OTRequestByUser(userID);
                var response = request.CreateResponse(HttpStatusCode.OK, model);

                return response;
            });
        }

        /// <summary>
        /// get ot reqeust chart by user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [Route(RoutesConstant.OTRequestChartByUser)]
        [HttpGet]
        public async Task<HttpResponseMessage> OTRequestChartByUser(HttpRequestMessage request, string userID)
        {

            return await CreateHttpResponse(request, () =>
            {
                var model = _otrequestService.OTRequestChartByUser(userID);
                var response = request.CreateResponse(HttpStatusCode.OK, model);

                return response;
            });
        }
    }
}
