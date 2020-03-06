using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TMS.Common;
using TMS.Common.Constants;
using TMS.Common.Exceptions.Extensions;
using TMS.Model.Models;
using TMS.Service;
using TMS.Web.Infrastructure.Core;
using TMS.Web.Models;
using TMS.Web.Models.Request;

namespace TMS.Web.Controllers
{
    [Authorize]
    [RoutePrefix(RoutesConstant.DelegationRequestApi)]
    public class DelegationRequestController : ApiControllerBase
    {
        #region Initialize

        private IDelegationRequestService _delegationRequestService;
        private IRequestService _requestService;
        private IPermissionService _permissionService;
        /// <summary>
        /// Contructor of DelegationRequest
        /// </summary>
        /// <param name="errorService">error service</param>
        /// <param name="requestService">request service</param>
        /// <param name="statusRequestService">status request service</param>
        public DelegationRequestController(IErrorService errorService, IDelegationRequestService delegationRequestService, IRequestService requestService, ISystemService systemService, IPermissionService permissionService)
            : base(errorService)
        {
            this._delegationRequestService = delegationRequestService;
            this._requestService = requestService;
            this._permissionService = permissionService;
        }

        #endregion Initialize

        /// <summary>
        /// get all request is assigned for group lead
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userID">ID of username</param>
        /// <param name="groupID">ID of group</param>
        /// <param name="column">number colums of list</param>
        /// <param name="isDesc"></param>
        /// <param name="page">number page</param>
        /// <param name="pageSize">total page</param>
        /// <returns></returns>
        [Route(RoutesConstant.GetAllDelegationRequest)]
        [HttpPost]
        public async Task<HttpResponseMessage> GetAllDelegationRequest(HttpRequestMessage request, string userID, string groupID, string column, bool isDesc, int page, int pageSize)
        {
            Func<HttpResponseMessage> func = () =>
            {
                if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(groupID))
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(userID) + MessageSystem.NoValues + nameof(groupID) + MessageSystem.NoValues);
                }
                var model = _delegationRequestService.GetAllDelegationRequest(userID, groupID);
                foreach (var item in model)
                {
                    if (item.StatusRequest.Name == CommonConstants.StatusDelegation)
                    {
                        _requestService.CheckDataDelegationRequest(groupID, item);
                    }
                }
                var lstDelegationRequest = _delegationRequestService.GetAllDelegationRequest(userID, groupID);
                var data = lstDelegationRequest.OrderByField(column, isDesc).Skip((page - 1) * pageSize).Take(pageSize);
                var responseData = Mapper.Map<IEnumerable<Request>, IEnumerable<RequestViewModel>>(data);
                var paginationSet = new PaginationSet<RequestViewModel>()
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
        /// change status of request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestID">Id of request</param>
        /// <param name="action"></param>
        /// <returns></returns>
        [Route(RoutesConstant.ChangeStatus)]
        [HttpPost]
        public async Task<HttpResponseMessage> ChangeStatus(HttpRequestMessage request, string action, string changeStatusBy, Request objectRequest)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var rs = _requestService.ChangeStatusRequest(objectRequest, action, changeStatusBy);
                if(rs == null)
                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.APPROVED_ERROR_HNN);
                else if (rs.StatusRequest.Name == CommonConstants.StatusPending)
                    return request.CreateErrorResponse(HttpStatusCode.OK, MessageSystem.Pending);
                else if (rs.StatusRequest.Name == CommonConstants.StatusRejected)
                    return request.CreateErrorResponse(HttpStatusCode.OK, MessageSystem.Rejected);
                else if (rs.StatusRequest.Name == CommonConstants.StatusApproved)
                    return request.CreateErrorResponse(HttpStatusCode.OK, MessageSystem.Approved);
                return request.CreateErrorResponse(HttpStatusCode.OK, MessageSystem.CancelFail);
            };
            return await CreateHttpResponse(request, func);
        }
        [Route(RoutesConstant.Active_InactiveDelegation)]
        [HttpGet]
        public async Task<HttpResponseMessage> Active_InactiveDelegation(HttpRequestMessage request, string action)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var lstRole = AppRoleManager.Roles.ToList();
                _permissionService.ChangeDelegatePermission(lstRole, action);
                return request.CreateResponse(HttpStatusCode.OK, lstRole);
            };
            return await CreateHttpResponse(request, func);
        }
        [Route(RoutesConstant.CheckDelegationStatus)]
        [HttpGet]
        public async Task<HttpResponseMessage> CheckDelegationStatus(HttpRequestMessage request)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var DelegationStatus= ConfigHelper.GetByKey("DelegationStatus");
                return request.CreateResponse(HttpStatusCode.OK, DelegationStatus);
            };
            return await CreateHttpResponse(request, func);
        }
    }
}