using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TMS.Common;
using TMS.Common.Constants;
using TMS.Common.ViewModels;
using TMS.Model.Models;
using TMS.Service;
using TMS.Web.Infrastructure.Core;
using TMS.Web.Infrastructure.Extensions;
using TMS.Web.Models;
using TMS.Web.Models.Request;

namespace TMS.Web.Controllers
{
    [RoutePrefix(RoutesConstant.RequestApi)]
    [Authorize]
    public class RequestController : ApiControllerBase
    {
        #region Initialize

        private IRequestService _requestService;
        private IStatusRequestService _statusRequestService;
        private ISystemService _systemService;
        private IGroupService __groupService;
        private IRequestTypeService _requestTypeService;
        private IConfigDelegationService _configDelegationService;
        /// <summary>
        /// constructor request controller
        /// </summary>
        /// <param name="errorService"> error service</param>
        /// <param name="requestService">request service</param>
        public RequestController(IErrorService errorService, IRequestService requestService, IStatusRequestService statusRequestService, ISystemService systemService,IGroupService groupService, IRequestTypeService requestTypeService, IConfigDelegationService configDelegationService)
            : base(errorService)
        {
            this._requestService = requestService;
            this._statusRequestService = statusRequestService;
            this._systemService = systemService;
            this.__groupService = groupService;
            this._requestTypeService = requestTypeService;
            this._configDelegationService = configDelegationService;
        }

        #endregion Initialize

        /// <summary>
        /// get request by user and group
        /// </summary>
        /// <param name="request">Http request</param>
        /// <param name="userId">id of user login</param>
        /// <param name="groupId">groupid of user login</param>
        /// <returns></returns>
        [Route(RoutesConstant.RequestGetAllRequestByUser)]
        [HttpPost]
        public async Task<HttpResponseMessage> GetAllByUser(HttpRequestMessage request, string userId, string groupId, string column, bool isDesc, int page, int pageSize, [FromBody] FilterModelRequest filter)
        {
            Func<HttpResponseMessage> func = () =>
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(groupId))
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(userId) + MessageSystem.NoValues + nameof(groupId) + MessageSystem.NoValues);
                }
                //var model = _requestService.GetAllRequest(userId, groupId, filter);
                var model = _requestService.GetAllRequestByUser(userId, groupId);
                foreach (var item in model)
                {
                    if (item.StatusRequest.Name == CommonConstants.StatusDelegation)
                    {
                        _requestService.CheckDataDelegationRequest(groupId, item);
                    }
                }
                var dataRequest = _requestService.GetAllRequest(userId, groupId, filter);
                var data = string.IsNullOrEmpty(column) ? dataRequest.Skip((page - 1) * pageSize).Take(pageSize) : dataRequest.OrderByField(column, isDesc).Skip((page - 1) * pageSize).Take(pageSize);
                var responseData = Mapper.Map<IEnumerable<Request>, IEnumerable<RequestViewModel>>(data);
                var paginationSet = new PaginationSet<RequestViewModel>()
                {
                    Items = responseData,
                    PageIndex = page,
                    TotalRows = dataRequest.Count(),
                    PageSize = pageSize
                };
                return request.CreateResponse(HttpStatusCode.OK, paginationSet);
            };
            return await CreateHttpResponse(request, func);
        }



        [Route(RoutesConstant.RequestGetAllRequestByUserSuperAdmin)]
        [HttpPost]
        public async Task<HttpResponseMessage> GetAllByUserSuperAdmin(HttpRequestMessage request, string column, bool isDesc, int page, int pageSize, [FromBody] FilterModelRequest filter)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var dataRequest = _requestService.GetAllRequestSuperAdmin(filter);
                var data = string.IsNullOrEmpty(column) ? dataRequest.Skip((page - 1) * pageSize).Take(pageSize) : dataRequest.OrderByField(column, isDesc).Skip((page - 1) * pageSize).Take(pageSize);
                var responseData = Mapper.Map<IEnumerable<Request>, IEnumerable<RequestViewModel>>(data);
                var paginationSet = new PaginationSet<RequestViewModel>()
                {
                    Items = responseData,
                    PageIndex = page,
                    TotalRows = dataRequest.Count(),
                    PageSize = pageSize
                };
                return request.CreateResponse(HttpStatusCode.OK, paginationSet);
            };
            return await CreateHttpResponse(request, func);
        }
        /// <summary>
        /// change status multi request
        /// </summary>
        /// <param name="request">Http request</param>
        /// <param name="typeStatus">type status want to change</param>
        /// <param name="userIdDelegate">user id want to delegate</param>
        /// <param name="requestId">array request id want to change status</param>
        /// <returns></returns>
        [Route(RoutesConstant.ChangeStatusMulti)]
        [HttpPost]
        public HttpResponseMessage ChangeStatusMultiRequest(HttpRequestMessage request, string typeStatus, string userIdDelegate, string[] requestId)
        {
            if (string.IsNullOrEmpty(requestId.ToString()) || string.IsNullOrEmpty(typeStatus))
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(requestId) + MessageSystem.NoValues + nameof(typeStatus) + MessageSystem.NoValues);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var dateNow = DateTime.Now;
                    List<Request> listRequest = new List<Request>();
                    foreach (var item in requestId)
                    {
                        Request requestEntity = _requestService.GetById(int.Parse(item));
                        if (requestEntity.StartDate.AddHours(23) < dateNow)
                        {
                            return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_CHANGESTATUS_REQUEST_IN_PAST_NOT_MSG);
                        }
                        var user = AppUserManager.Users.Where(x => x.Id.Equals(requestEntity.UserId)).FirstOrDefault();
                        //Check if request in about set delegate default time that status is "Delegated",will not set delegate
                        if (requestEntity != null && typeStatus.Equals(CommonConstants.StatusDelegation))
                        {
                            var group = __groupService.GetGroupById(user.GroupId.ToString());
                            if (group.EndDate != null && requestEntity.CreatedDate != null)
                            {
                                if (group.EndDate.Value.Date < DateTime.Now.Date && group.StartDate.Value.Date <= requestEntity.CreatedDate.Value.Date && requestEntity.CreatedDate.Value.Date <= group.EndDate.Value.Date)
                                {
                                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_CHANGESTATUS_REQUEST_NOT_IN_DELEGATEDEFAULT_TIME_MSG);
                                }
                            }
                              
                        }
                        requestEntity.ChangeStatusById = AppUserManager.FindByNameAsync(User.Identity.Name).Result.Id;
                        requestEntity.UpdatedDate = DateTime.Now;
                        listRequest.Add(requestEntity);
                    }
                    return request.CreateResponse(HttpStatusCode.OK, _requestService.ChangeStatusListRequest(listRequest, typeStatus, userIdDelegate).IsCompleted);
                }
                catch (Exception dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
        /// <summary>
        /// Change status request action
        /// </summary>
        /// <param name="request">http request</param>
        /// <param name="requestId">Id of request want to change</param>
        /// <param name="typeStatus">type status want to change</param>
        /// <returns></returns>
        [Route(RoutesConstant.ChangeStatus)]
        [HttpPost]
        public HttpResponseMessage ChangeStatusRequest(HttpRequestMessage request, int requestId, string typeStatus, string userIdDelegate)
        {
            if (string.IsNullOrEmpty(requestId.ToString()) || string.IsNullOrEmpty(typeStatus))
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(requestId) + MessageSystem.NoValues + nameof(typeStatus) + MessageSystem.NoValues);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    Request requestEntity = _requestService.GetById(requestId);
                    var dateNow = DateTime.Now;
                    if(requestEntity.StartDate.AddHours(23) < dateNow)
                    {
                        return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_CHANGESTATUS_REQUEST_IN_PAST_NOT_MSG);
                    }
                    var user = AppUserManager.Users.Where(x => x.Id.Equals(requestEntity.UserId)).FirstOrDefault();
                    //Check if request in about set delegate default time that status is "Delegated",will not set delegate
                    if (requestEntity!= null && typeStatus.Equals(CommonConstants.StatusDelegation))
                    {
                        var group = __groupService.GetGroupById(user.GroupId.ToString());
                        if(group.EndDate != null && requestEntity.CreatedDate != null)
                        {
                            
                            if (group.EndDate.Value.Date < DateTime.Now.Date && group.StartDate.Value.Date <= requestEntity.CreatedDate.Value.Date && requestEntity.CreatedDate.Value.Date <= group.EndDate.Value.Date)
                            {
                                return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_CHANGESTATUS_REQUEST_NOT_IN_DELEGATEDEFAULT_TIME_MSG);
                            }
                        }
                    }
                    requestEntity.ChangeStatusById = AppUserManager.FindByNameAsync(User.Identity.Name).Result.Id;
                    requestEntity.UpdatedDate = DateTime.Now;
                    if (_requestService.ChangeStatusRequest(requestEntity, typeStatus, userIdDelegate)==null)
                    {
                        return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_HNN);
                    } 
                    _requestService.Save();
                    return request.CreateResponse(HttpStatusCode.OK, Mapper.Map<TMS.Model.Models.Request, RequestViewModel>(requestEntity));

                }
                catch (Exception dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
        /// <summary>
        /// Create request action
        /// </summary>
        /// <param name="request">http request</param>
        /// <param name="requestViewModel">Request want to add</param>
        /// <returns></returns>
        [Route(RoutesConstant.Add)]
        [HttpPost]
        public async Task<HttpResponseMessage> Create(HttpRequestMessage request, RequestViewModel requestViewModel)
        {
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                var dateNow = DateTime.Now.Date;
                if(requestViewModel.StartDate < dateNow)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_CREATE_REQUEST_IN_PAST_NOT_MSG);
                }
                else
                {
                    requestViewModel.AppUser = new Models.AppUserViewModel();
                    requestViewModel.CreatedDate = DateTime.Now;
                    requestViewModel.UserId = AppUserManager.FindByNameAsync(User.Identity.Name).Result.Id;
                    requestViewModel.AppUser.GroupId = AppUserManager.FindByNameAsync(User.Identity.Name).Result.GroupId ?? 0;
                    requestViewModel.Status = true;
                    //requestViewModel.ChangeStatusById = __groupService.GetGroupLeadIdByGroup(requestViewModel.AppUser.GroupId.Value);
                    Request newRequest = new Request();
                    newRequest.UpdateRequest(requestViewModel);
                    //Check Request Day Off
                    var model = _requestService.GetAllRequest(newRequest);
                    if (model == 0)
                    {

                        if (_requestService.checkUnitEntitleday(newRequest))
                        {
                            if (_requestService.Add(newRequest))
                            {
                                var responseData = Mapper.Map<Request, RequestViewModel>(newRequest);
                                var group = __groupService.GetGroupById(requestViewModel.AppUser.GroupId.ToString());
                                responseData.GroupName = group.Name;
                                var dataDelegation = _configDelegationService.GetDelegationByUserId(newRequest.UserId);
                                if (dataDelegation.StartDate <= newRequest.CreatedDate.Value.Date && dataDelegation.EndDate >= newRequest.CreatedDate.Value.Date)
                                {
                                    responseData.CheckConfigDelegateDefault = true;
                                    responseData.AssignConfigDelegate = dataDelegation.AssignTo;
                                    _configDelegationService.ChangeStatusAfterAddRequest(dataDelegation.AssignTo, newRequest);
                                }
                                else
                                {
                                    if (newRequest.CreatedDate != null)
                                    {
                                        if (group.DelegateId != null && group.StartDate <= newRequest.CreatedDate.Value.Date && group.EndDate >= newRequest.CreatedDate.Value.Date)
                                        {
                                            responseData.CheckGroupDelegateDefault = true;
                                            responseData.AssignGroupDelegate = group.DelegateId;
                                            _requestService.CheckDelegateDefault(requestViewModel.AppUser.GroupId.ToString(), newRequest.ID, newRequest.ChangeStatusById);
                                        }
                                    }
                                }
                                return request.CreateResponse(HttpStatusCode.Created, responseData);
                            }
                        }
                        else
                        {
                            return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_ENTITLEDAY);
                        }
                        return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_CREATE_REQUEST_NOT_MSG);
                    }
                    if (model == 1)
                    {
                        return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_FULLTIME);
                    }
                    if (model == 2)
                    {
                        return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_FULLTIME_MORNING);
                    }
                    if (model == 3)
                    {
                        return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_FULLTIME_AFTERNOON);
                    }
                    if (model == 4) 
                    {
                        return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_FULLTIME_LATECOMING);
                    }
                    if (model == 5)
                    {
                        return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_FULLTIME_EARLYLEAVING);
                    }
                    if (model == 6)
                    {
                        return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_FULLTIME);
                    }
                    if (model == 7)
                    {
                        return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_MORNING);
                    }
                    if (model == 8)
                    {
                        return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_MORNING_LATECOMING);
                    }
                    if (model == 9)
                    {
                        return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_FULLTIME);
                    }
                    if (model == 10)
                    {
                        return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_AFTERNOONLEAVE);
                    }
                    if (model == 11)
                    {
                        return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_AFTERNOONLEAVE_EARLYLEAVING);
                    }
                    if (model == 12)
                    {
                        return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_FULLTIME);
                    }
                    if (model == 13)
                    {
                        return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_MORNING_LATECOMING);
                    }
                    if (model == 14)
                    {
                        return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_LATECOMING_EARLYLEAVING);
                    }
                    if (model == 15)
                    {
                        return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_FULLTIME);
                    }
                    if (model == 16)
                    {
                        return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_EARLYLEAVING_AFTERNOONLEAVE);
                    }
                    if (model == 17)
                    {
                        return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_EARLYLEAVING);
                    }
                }
                return response;
            });
        }

        /// <summary>
        /// Get detail request
        /// </summary>
        /// <param name="request">http request</param>
        /// <param name="id">id of request want to get detail</param>
        /// <returns></returns>
        [Route(RoutesConstant.Detail)]
        [HttpGet]
        public async Task<HttpResponseMessage> GetById(HttpRequestMessage request, int id)
        {
            return await CreateHttpResponse(request, () =>
            {
                var model = _requestService.GetById(id);
                var responseData = Mapper.Map<Request, RequestViewModel>(model);
                responseData.FullNameDelegate = AppUserManager.Users.Where(x => x.Id.Equals(model.AssignToId)).FirstOrDefault() != null ? AppUserManager.Users.Where(x => x.Id.Equals(model.AssignToId)).FirstOrDefault().FullName : null;
                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        /// <summary>
        /// Update Request User
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route(RoutesConstant.UpdateRequest)]
        [HttpPut]
        public async Task<HttpResponseMessage> Update (HttpRequestMessage request,RequestViewModel requestViewModel)
        {
            return await CreateHttpResponse(request, () =>
            {
                var dbRequest = _requestService.GetById(requestViewModel.ID);
                //Check Old Day Breack
                _requestService.checkDayBreak(dbRequest);
                dbRequest.EditRequest(requestViewModel);
                //Check Request Day Off
                var model = _requestService.GetAllRequest(dbRequest);
                if (model == 0)
                {
                    if (_requestService.checkUnitEntitleday(dbRequest))
                    {
                        if (_requestService.CheckUpdateRequest(dbRequest))
                        {
                            _requestService.UpdateRequest(dbRequest);
                            return request.CreateResponse(HttpStatusCode.OK, "OK");
                        }
                        else
                        {
                            return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_CREATE_REQUEST_NOT_MSG);
                        }
                    }
                    else
                    {
                        return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_ENTITLEDAY);
                    }
                }
                if (model == 1)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_FULLTIME);
                }
                if (model == 2)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_FULLTIME_MORNING);
                }
                if (model == 3)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_FULLTIME_AFTERNOON);
                }
                if (model == 4)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_FULLTIME_LATECOMING);
                }
                if (model == 5)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_FULLTIME_EARLYLEAVING);
                }
                if (model == 6)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_FULLTIME);
                }
                if (model == 7)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_MORNING);
                }
                if (model == 8)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_MORNING_LATECOMING);
                }
                if (model == 9)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_FULLTIME);
                }
                if (model == 10)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_AFTERNOONLEAVE);
                }
                if (model == 11)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_AFTERNOONLEAVE_EARLYLEAVING);
                }
                if (model == 12)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_FULLTIME);
                }
                if (model == 13)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_MORNING_LATECOMING);
                }
                if (model == 14)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_LATECOMING_EARLYLEAVING);
                }
                if (model == 15)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_FULLTIME);
                }
                if (model == 16)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_EARLYLEAVING_AFTERNOONLEAVE);
                }
                if (model == 17)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_REQUEST_EARLYLEAVING);
                }

                
                var response = request.CreateResponse(HttpStatusCode.OK, "Xong");

                return response;
            });
        }

        
        /// <summary>
        /// method get request is assigned
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userID">ID username login</param>
        /// <param name="column">number colums of list</param>
        /// <param name="isDesc"></param>
        /// <param name="page">number page</param>
        /// <param name="pageSize">total page</param>
        /// <param name="filter">paramester want to filter</param>
        /// <returns>HttpResponseMessage</returns>
        [Route(RoutesConstant.GetAllRequestIsAssignedForUser)]
        [HttpPost]
        public async Task<HttpResponseMessage> GetAllRequestIsAssignedForUser(HttpRequestMessage request, string userID, string groupID, string column, bool isDesc, int page, int pageSize, [FromBody]FilterDelegationAssignedModel filter)
        {
            Func<HttpResponseMessage> func = () =>
            {
                if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(groupID))
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(userID) + MessageSystem.NoValues + nameof(groupID) + MessageSystem.NoValues);
                }
                //var model = _requestService.GetDelegationRequest(userID, groupID, filter);
                var model = _requestService.GetAllAssignedRequestForUser(userID, groupID);
                var groupLeadId = __groupService.GetGroupLeadIdByGroup(Int32.Parse(groupID));
                //Check Data have status delegated have date now > end date
                foreach (var item in model)
                {
                    if (item.StatusRequest.Name == CommonConstants.StatusDelegation)
                    {
                        var user = AppUserManager.Users.Where(x => x.Id == item.DelegateId).FirstOrDefault();
                        //_requestService.CheckDataDelegationRequest(groupID, item.ID); 
                        _requestService.CheckDataDelegationAllRequest(groupID, item, user.GroupId.ToString());
                    }
                }
                var requestData = _requestService.GetDelegationRequest(userID, groupID, filter);
                var data = requestData.OrderByField(column, isDesc).Skip((page - 1) * pageSize).Take(pageSize);
                var responseData = Mapper.Map<IEnumerable<Request>, IEnumerable<RequestViewModel>>(data);
                
                var paginationSet = new PaginationSet<RequestViewModel>()
                {
                    Items = responseData,
                    PageIndex = page,
                    TotalRows = requestData.Count(),
                    PageSize = pageSize
                };
                return request.CreateResponse(HttpStatusCode.OK, paginationSet);
            };
            return await CreateHttpResponse(request, func);
        }
        /// <summary>
        /// Get list creator request
        /// </summary>
        /// <param name="request">http request</param>
        /// <param name="userId">user id login</param>
        /// <param name="groupId">group id of user login</param>
        /// <returns></returns>
        [Route(RoutesConstant.GetListCreator)]
        [HttpGet]
        public async Task<HttpResponseMessage> GetCreator(HttpRequestMessage request, string userId, string groupId)
        {
            Func<HttpResponseMessage> func = () =>
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(groupId))
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(userId) + MessageSystem.NoValues + nameof(groupId) + MessageSystem.NoValues);
                }
                var model = _requestService.GetListCreator(userId, groupId);

                return request.CreateResponse(HttpStatusCode.OK, model);
            };
            return await CreateHttpResponse(request, func);
        }

        /// <summary>
        /// Get request chart by group
        /// </summary>
        /// <param name="request">http request</param>
        /// <param name="userId">user id login</param>
        /// <param name="groupId">group id of user login</param>
        /// <returns></returns>
        [Route(RoutesConstant.requestChart)]
        [HttpGet]
        public async Task<HttpResponseMessage> RequestChart(HttpRequestMessage request,int groupID)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var model = _requestService.RequestChart(groupID);
                return request.CreateResponse(HttpStatusCode.OK, model);
            };
            return await CreateHttpResponse(request, func);
        }

        /// <summary>
        /// get request chart by userid
        /// </summary>
        /// <param name="request">http request</param>
        /// <param name="userId">user id login</param>
        /// <param name="groupId">group id of user login</param>
        /// <returns></returns>
        [Route(RoutesConstant.requestChartByUser)]
        [HttpGet]
        public async Task<HttpResponseMessage> RequestChartByUser(HttpRequestMessage request, string userID)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var model = _requestService.RequestChartByUser(userID);
                return request.CreateResponse(HttpStatusCode.OK, model);
            };
            return await CreateHttpResponse(request, func);
        }


        /// <summary>
        /// Get number of request by user
        /// </summary>
        /// <param name="request">http request</param>
        /// <param name="userId">user id login</param>
        /// <param name="groupId">group id of user login</param>
        /// <returns></returns>
        [Route(RoutesConstant.requestByUser)]
        [HttpGet]
        public async Task<HttpResponseMessage> RequestByUser(HttpRequestMessage request, string userID)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var model = _requestService.RequestByUser(userID);
                return request.CreateResponse(HttpStatusCode.OK, model);
            };
            return await CreateHttpResponse(request, func);
        }

        /// <summary>
        /// Get detail request
        /// </summary>
        /// <param name="request">http request</param>
        /// <param name="id">id of request want to get detail</param>
        /// <returns></returns>
        [Route(RoutesConstant.GetAllRequestByUser)]
        [HttpPut]
        public async Task<HttpResponseMessage> GetAllRequestByUser(HttpRequestMessage request, string userId, string groupId,string[] requestId)
        {
            return await CreateHttpResponse(request, () =>
            {
                List<Request> lstRequest = new List<Request>();
                var model = _requestService.GetAllRequestByUser(userId, groupId);
                if(requestId != null)
                {
                    foreach (var item in requestId)
                    {
                        var data = model.Where(x => x.ID == Convert.ToInt32(item)).FirstOrDefault();
                        lstRequest.Add(data);
                    }
                }
                var responseData = Mapper.Map<IEnumerable<Request>, List<RequestViewModel>>(lstRequest);
                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

    }
}