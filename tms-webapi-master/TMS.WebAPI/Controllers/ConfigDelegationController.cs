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
using TMS.Common.ViewModels;
using TMS.Web.Models.Group;

namespace TMS.Web.Controllers
{
    [Authorize]
    [RoutePrefix(RoutesConstant.ConfigDelegationApi)]
    public class ConfigDelegationController : ApiControllerBase
    {
        private IConfigDelegationService _configDelegationService;
        private IRequestService _requestService;
        private IExplanationRequestService _explanationRequestService;
        public ConfigDelegationController(IErrorService errorService, IConfigDelegationService configDelegationService, IRequestService requestService, IExplanationRequestService explanationRequestService) : base(errorService)
        {
            _configDelegationService = configDelegationService;
            _requestService = requestService;
            _explanationRequestService = explanationRequestService;
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
        [Route(RoutesConstant.ConfigDelegationGetAll)]
        [HttpPost]
        public async Task<HttpResponseMessage> GetAllUserConfigDelegation(HttpRequestMessage request, int page, int pageSize, string groupID,string[] lstFullName)
        {
            Func<HttpResponseMessage> func = () =>
            {
                if ( string.IsNullOrEmpty(groupID))
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest,nameof(groupID) + MessageSystem.NoValues);
                }
                var groupleaderID = AppRoleManager.FindByNameAsync(CommonConstants.GroupLead).Result.Id;
                var convertGroupID = Int32.Parse(groupID);
                var user = AppUserManager.Users.Where(x => x.Roles.Any(r => r.RoleId.Equals(groupleaderID)) && x.GroupId == convertGroupID).ToList();
                var model = _configDelegationService.GetListUserConfigDelegationFilter(groupID,lstFullName).Where(x=>!x.UserId.Equals(user[0].Id));
                var data = model.Skip((page - 1) * pageSize).Take(pageSize);
                var paginationSet = new PaginationSet<ConfigDelegationModel>()
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
        /// Set delegate default (function for grouplead)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ConfigDelegationModel">Set delegate default has updated</param>
        /// <returns>Return a message success or not</returns>
        [HttpPut]
        [Route(Common.Constants.RoutesConstant.AddConfigDelegation)]
        public HttpResponseMessage AddConfigDelegation(HttpRequestMessage request, string userId, string groupId, ConfigDelegationModel delegationVm)
        {
            if (!ModelState.IsValid)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            else
            {
                //List<ExplanationRequest> lstExplanationRequest = new List<ExplanationRequest>();
                //var ChangeStatusById = AppUserManager.FindByNameAsync(User.Identity.Name).Result.Id;
                List<object> lstData = new List<object>();
                var delegationData = _configDelegationService.GetDataDelegationById(delegationVm.ID.ToString());
                delegationData.UpdateConfigDelegation(delegationVm);
                _configDelegationService.Update(delegationData);
                _configDelegationService.SaveChange();

                //Get request by user and group.Filter by StatusRequest is Pendding or Delegate in about time
                var model = _requestService.GetAllRequestByUser(userId, groupId);
                var data = model.Where(x => (x.StatusRequest.Name.Equals(CommonConstants.StatusPending)
                || x.StatusRequest.Name.Equals(CommonConstants.StatusDelegation))
                && x.CreatedDate.Value.Date >= delegationData.StartDate
                && x.CreatedDate.Value.Date <= delegationData.EndDate);
                lstData.AddRange(data);
                _configDelegationService.ChangeStatusRequestConfigDelegate(delegationData.AssignTo, data.ToList());

                //Get explanation by userid and groupid.Filter by StatusRequest is Pending or Delegate
                var explanation = _explanationRequestService.GetListExplanationByUser(userId, groupId);
                var dataExplanation = explanation.Where(a => (a.StatusRequest.Name.Equals(CommonConstants.StatusPending)
                || a.StatusRequest.Name.Equals(CommonConstants.StatusDelegation))
                && a.CreatedDate.Value.Date >= delegationData.StartDate
                && a.CreatedDate.Value.Date <= delegationData.EndDate);
                lstData.AddRange(explanation);
                _configDelegationService.ChangeStatusExplanationRequestConfigDelegate(delegationData.AssignTo, dataExplanation.ToList());
                return request.CreateResponse(HttpStatusCode.Created, lstData);
            }
        }

        /// <summary>
        /// delete delegation by id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(RoutesConstant.DeleteConfigDelegation)]
        public async Task<HttpResponseMessage> Delete(HttpRequestMessage request, int id)
        {

            var delegationData = _configDelegationService.GetDataDelegationById(id.ToString());
            delegationData.DeleteConfigDelegation();
            _configDelegationService.Update(delegationData);
            _configDelegationService.SaveChange();
            return request.CreateResponse(HttpStatusCode.OK, delegationData);
        }

    }
}