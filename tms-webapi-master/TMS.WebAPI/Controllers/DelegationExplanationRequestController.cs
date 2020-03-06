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
using TMS.Web.Models.AbnormalCase;
using TMS.Web.Models.Explanation;
using TMS.Web.Models.Request;

namespace TMS.Web.Controllers
{
    [Authorize]
    [RoutePrefix(RoutesConstant.DelegationExplanationRequestApi)]
    public class DelegationExplanationRequestController : ApiControllerBase
    {
        #region Initialize

        private IExplanationRequestService _explanationRequestService;
        private IDelegationExplanationRequestService _delegationExplanationRequestService;

        /// <summary>
        /// Contructor of DelegationExplanationRequest
        /// </summary>
        /// <param name="errorService">error service</param>
        /// <param name="requestService">request service</param>
        /// <param name="statusRequestService">status request service</param>
        public DelegationExplanationRequestController(IErrorService errorService, IDelegationExplanationRequestService delegationExplanationRequestService, IExplanationRequestService explanationRequestService)
            : base(errorService)
        {
            this._delegationExplanationRequestService = delegationExplanationRequestService;
            this._explanationRequestService = explanationRequestService;
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
        [Route(RoutesConstant.GetAllDelegationExplanationRequest)]
        [HttpPost]
        public async Task<HttpResponseMessage> GetAllDelegationExplanationRequest(HttpRequestMessage request, string userID, string groupID, string column, bool isDesc, int page, int pageSize)
        {
            Func<HttpResponseMessage> func = () =>
            {
                if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(groupID))
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(userID) + MessageSystem.NoValues + nameof(groupID) + MessageSystem.NoValues);
                }
                var model = GetExplanationRequestViewModel(userID, groupID, column, isDesc, page, pageSize);
                foreach (var item in model)
                {
                    if (item.StatusRequest.Name == CommonConstants.StatusDelegation)
                    {
                        _explanationRequestService.CheckDataDelegationExplanationRequest(Convert.ToInt32(groupID), item.ID);
                    }
                }
                _explanationRequestService.Save();
                var lstDelegationExplanationRequest = GetExplanationRequestViewModel(userID, groupID, column, isDesc, page, pageSize);
                var data = lstDelegationExplanationRequest.OrderByField(column, isDesc).Skip((page - 1) * pageSize).Take(pageSize);
                var paginationSet = new PaginationSet<ExplanationRequestViewModel>()
                {
                    Items = data,
                    PageIndex = page,
                    TotalRows = lstDelegationExplanationRequest.Count(),
                    PageSize = pageSize
                };
                return request.CreateResponse(HttpStatusCode.OK, paginationSet);
            };
            return await CreateHttpResponse(request, func);
        }

        /// <summary>
        /// get list explanation request
        /// </summary>
        /// <param name="userID">ID of Username</param>
        /// <param name="groupID">ID of groupd</param>
        /// <param name="column">sort by colums</param>
        /// <param name="isDesc"></param>
        /// <param name="page">page number</param>
        /// <param name="pageSize">total page</param>
        /// <returns></returns>
        private List<ExplanationRequestViewModel> GetExplanationRequestViewModel(string userID, string groupID, string column, bool isDesc, int page, int pageSize)
        {
            List<ExplanationRequestViewModel> listExplanationRequest = new List<ExplanationRequestViewModel>();

            var model = _delegationExplanationRequestService.GetAllDelegationExplanationRequest(userID, groupID);

            foreach(var items in model)
            {
                listExplanationRequest.Add(new ExplanationRequestViewModel
                {
                    ID = items.ID,
                    User = new AppUserViewModel
                    {
                        Id = items.FingerTimeSheet.FingerMachineUsers.AppUser.Id,
                        FullName = items.FingerTimeSheet.FingerMachineUsers.AppUser.FullName
                    },
                    Title = items.Title,
                    CreatedDate = (DateTime)items.CreatedDate,
                    ReasonDetail = items.ReasonDetail,
                    StatusRequest = new StatusRequestViewModel
                    {
                        ID = items.StatusRequest.ID,
                        Name = items.StatusRequest.Name
                    },
                    AbnormalReason = _delegationExplanationRequestService.GetAbnormalById(items.TimeSheetId).Select(m => new AbnormalCaseReasonViewModel
                    {
                        ReasonId = m.AbnormalReason.ID,
                        ReasonName = m.AbnormalReason.Name
                    }).ToList(),
                    Receiver = new AppUserViewModel
                    {
                        Id = items.Receiver.Id,
                        FullName = items.Receiver.FullName,
                        Email = items.Receiver.Email
                    },
                    Delegate = new AppUserViewModel
                    {
                        Id = items.Delegate.Id,
                        FullName = items.Delegate.FullName,
                        Email = items.Delegate.Email
                    },
                    UpdatedDate = items.UpdatedDate
                });
            }
            return listExplanationRequest;
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
        public async Task<HttpResponseMessage> ChangeStatus(HttpRequestMessage request, int requestID, string action, string changeStatusBy)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var rs = _delegationExplanationRequestService.ChangeStatus(requestID, action, changeStatusBy);
                if (rs.StatusRequest.Name == CommonConstants.StatusDelegation)
                    return request.CreateErrorResponse(HttpStatusCode.OK, MessageSystem.Pending);
                else if (rs.StatusRequest.Name == CommonConstants.StatusRejected)
                    return request.CreateErrorResponse(HttpStatusCode.RequestedRangeNotSatisfiable, MessageSystem.Rejected);
                else if (rs.StatusRequest.Name == CommonConstants.StatusApproved)
                    return request.CreateErrorResponse(HttpStatusCode.RequestUriTooLong, MessageSystem.Approved);
                return request.CreateErrorResponse(HttpStatusCode.OK, MessageSystem.CancelFail);
            };
            return await CreateHttpResponse(request, func);
        }
    }
}