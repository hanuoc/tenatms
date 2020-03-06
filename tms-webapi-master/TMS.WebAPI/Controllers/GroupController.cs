using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TMS.Common.Constants;
using TMS.Service;
using TMS.Web.Infrastructure.Core;
using TMS.Web.Models;
using TMS.Model.Models;
using TMS.Web.Infrastructure.Extensions;
using TMS.Web.Providers;
using System.Threading.Tasks;
using TMS.Web.Models.Group;
using TMS.Common.ViewModels;
using TMS.Web.Models.Explanation;
using TMS.Web.Models.Request;
using TMS.Web.Models.AbnormalCase;
using System.Threading;

namespace TMS.Web.Controllers
{
    [Authorize]
    [RoutePrefix(RoutesConstant.GroupApi)]
    public class GroupController : ApiControllerBase
    {
        private IGroupService _groupService;
        private IRequestService _requestService;
        private IExplanationRequestService _explanationRequestService;
        private ISystemService _systemService;
        private IRequestTypeService _requestTypeService;
        /// <summary>
        /// Contructor of OTRequestController class
        /// </summary>
        /// <param name="errorService"></param>
        public GroupController(IErrorService errorService, IGroupService groupService, IRequestService requestService,IExplanationRequestService explanationRequestService, ISystemService systemService, IRequestTypeService requestTypeService) : base(errorService)
        {
            _groupService = groupService;
            _requestService = requestService;
            _explanationRequestService = explanationRequestService;
            this._systemService = systemService;
            this._requestTypeService = requestTypeService;
        }
        /// <summary>
        /// get all request type
        /// </summary>
        /// <param name="request">Http request</param>
        /// <returns></returns>
        [Route(RoutesConstant.GetGroupById)]
        [HttpGet]
        public async Task<HttpResponseMessage> GetGroupById(HttpRequestMessage request, string groupId)
        {
            Func<HttpResponseMessage> func = () =>
            {
                return request.CreateResponse(HttpStatusCode.OK, Mapper.Map<Group, GroupViewModel>(_groupService.GetGroupById(groupId)));
            };
            return await CreateHttpResponse(request, func);
        }
        /// <summary>
        /// get all group and paging
        /// </summary>
        /// <param name="request">Http request</param>
        /// <returns>list group</returns>
        [Route(RoutesConstant.GetAllGroup)]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAllPaging(HttpRequestMessage request, int page, int pageSize ,string roleName, string userID)
        {
            Func<HttpResponseMessage> func = () =>
            {
                if(roleName.Equals(CommonConstants.GroupLead))
                {
                    var lstGr = _groupService.GetAllGroup();
                    var listGroup = Mapper.Map<List<Group>, List<GroupViewModel>>(lstGr);
                    List<GroupViewModel> listGroupTmp = new List<GroupViewModel>();
                    var users = AppUserManager.Users.Where(x => x.Id.Equals(userID)).ToList();
                    foreach (var item in listGroup)
                    {
                        var user = users.Where(x => x.GroupId == item.ID).FirstOrDefault();
                        if(user != null)
                        {
                            item.GroupLeadID = user != null ? user.Id : CommonConstants.StringEmpty;
                            item.GroupLead = user != null ? user.FullName : CommonConstants.DefaultGroupLeader;
                            item.GroupLeadAccount = user != null ? user.UserName : CommonConstants.DefaultGroupLeader;
                            listGroupTmp.Add(item);
                        }
                    }
                    int totalRow = listGroupTmp.Count;
                    PaginationSet<GroupViewModel> pagedSet = new PaginationSet<GroupViewModel>()
                    {
                        PageIndex = page,
                        PageSize = pageSize,
                        TotalRows = totalRow,
                        Items = listGroupTmp,
                    };
                    return request.CreateResponse(HttpStatusCode.OK, pagedSet);
                }
                else
                {
                    var lstGr = _groupService.GetAllGroup();
                    var listGroup = Mapper.Map<List<Group>, List<GroupViewModel>>(lstGr);
                    int totalRow = listGroup.Count;
                    
                    listGroup = listGroup.Where(x=>!x.Name.Equals("SuperAdmin")).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    var groupleaderID = AppRoleManager.FindByNameAsync(CommonConstants.GroupLead).Result.Id;
                    var users = AppUserManager.Users.Where(x => x.Roles.Any(r => r.RoleId.Equals(groupleaderID))).ToList();
                    foreach (var item in listGroup)
                    {
                        var user = users.Where(x => x.GroupId == item.ID).FirstOrDefault();
                        item.GroupLeadID = user != null ? user.Id : CommonConstants.StringEmpty;
                        item.GroupLead = user != null ? user.FullName : CommonConstants.DefaultGroupLeader;
                        item.GroupLeadAccount = user != null ? user.UserName : CommonConstants.DefaultGroupLeader;
                    }
                    PaginationSet<GroupViewModel> pagedSet = new PaginationSet<GroupViewModel>()
                    {
                        PageIndex = page,
                        PageSize = pageSize,
                        TotalRows = totalRow,
                        Items = listGroup,
                    };
                    return request.CreateResponse(HttpStatusCode.OK, pagedSet);
                }
                
                
            };
            return await CreateHttpResponse(request, func);
        }
        [Route(RoutesConstant.GetAllGroup)]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAllGroup(HttpRequestMessage request)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var lstGr = _groupService.GetAllGroup();
                var listGroup = Mapper.Map<List<Group>, List<GroupViewModel>>(lstGr);
                return request.CreateResponse(HttpStatusCode.OK, listGroup);
            };
            return await CreateHttpResponse(request, func);
        }
        [Route(RoutesConstant.Add)]
        [HttpPost]
        public async Task<HttpResponseMessage> Create(HttpRequestMessage request, GroupCreateUpdateModel groupVM)
        {
            if (ModelState.IsValid)
            {
                var newGroup = new Group();
                GroupCreateUpdateModel responseData = new GroupCreateUpdateModel();
                newGroup.UpdateGroup(groupVM);
                if (_groupService.IsDuplicateGroup(groupVM.Name, groupVM.ID))
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, Common.Constants.MessageSystem.GroupExist);
                }
                else
                {
                    if (_groupService.Add(newGroup) != null)
                    {
                        _groupService.SaveChange();
                        var groupId = _groupService.GetGroupByName(groupVM.Name).ID;
                        var appUser = await AppUserManager.FindByIdAsync(groupVM.GroupLeadID);
                        appUser.GroupId = groupId;
                        var result = await AppUserManager.UpdateAsync(appUser);
                        if (result.Succeeded)
                        {
                            return request.CreateResponse(HttpStatusCode.Created, responseData);
                        }
                        else
                        {
                            _groupService.Delete(groupId);
                            return request.CreateResponse(HttpStatusCode.BadRequest, responseData);
                        }
                    }
                    responseData = Mapper.Map<Group, GroupCreateUpdateModel>(newGroup);
                    return request.CreateResponse(HttpStatusCode.BadRequest, responseData);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
        /// <summary>
        /// delete a group by id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(RoutesConstant.DeleteGroup)]
        public async Task<HttpResponseMessage> Delete(HttpRequestMessage request, int id)
        {
            if (_groupService.IsGroupEmpty(id, _groupService.GetGroupLeadIdByGroup(id)))
            {
                var result = _groupService.Delete(id);
                if (result != null)
                {
                    var groupleaderID = AppRoleManager.FindByNameAsync(CommonConstants.GroupLead).Result.Id;
                    var user = AppUserManager.Users.Where(x => x.Roles.Any(r => r.RoleId.Equals(groupleaderID) && x.GroupId == id)).FirstOrDefault();
                    if (user != null)
                    {
                        user.GroupId = null;
                        var resultUpdate = await AppUserManager.UpdateAsync(user);
                    }
                    _groupService.SaveChange();
                    return request.CreateResponse(HttpStatusCode.OK, id);
                }
                else
                    return request.CreateResponse(HttpStatusCode.BadRequest, id);
            }
            else
            {
                Func<HttpResponseMessage> func = () =>
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_ExistMemberInGroup);
                };
                return await CreateHttpResponse(request, func);
            }
        }
        /// <summary>
        /// Update group
        /// </summary>
        /// <param name="request"></param>
        /// <param name="applicationUserViewModel">User infomation has updated</param>
        /// <returns>Return a message success or not</returns>
        [HttpPut]
        [Route(Common.Constants.RoutesConstant.UpdateGroup)]
        public async Task<HttpResponseMessage> Update(HttpRequestMessage request, GroupCreateUpdateModel groupVm)
        {
            if (!ModelState.IsValid)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            else
            {
                var newGroup = _groupService.GetGroupById(groupVm.ID.ToString());
                if (_groupService.IsDuplicateGroup(groupVm.Name, groupVm.ID))
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.GroupExist);
                }
                else
                {
                    newGroup.UpdateGroup(groupVm);
                    _groupService.Update(newGroup);
                    _groupService.SaveChange();
                    var groupId = _groupService.GetGroupByName(groupVm.Name).ID;
                    var oldLeader = await AppUserManager.FindByIdAsync(_groupService.GetGroupLeadIdByGroup(groupVm.ID));
                    var newLeader = await AppUserManager.FindByIdAsync(groupVm.GroupLeadID);
                    if(oldLeader != null)
                    {
                        if (newLeader.Equals(oldLeader))
                        {
                            return request.CreateResponse(HttpStatusCode.Created, newGroup);
                        }
                        else
                        {
                            newLeader.GroupId = groupId;
                            oldLeader.GroupId = null;
                            var resultUpdateNewLeader = await AppUserManager.UpdateAsync(newLeader);
                            var resultUpdateOldLeader = await AppUserManager.UpdateAsync(oldLeader);
                            if (resultUpdateNewLeader.Succeeded && resultUpdateOldLeader.Succeeded)
                            {
                                return request.CreateResponse(HttpStatusCode.Created, newGroup);
                            }
                            else
                            {
                                return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_UPDATE_LEADER);
                            }
                        }
                    }
                    else
                    {
                        newLeader.GroupId = groupId;
                        var resultUpdateNewLeader = await AppUserManager.UpdateAsync(newLeader);
                        if (resultUpdateNewLeader.Succeeded)
                        {
                            return request.CreateResponse(HttpStatusCode.Created, newGroup);
                        }
                        else
                        {
                            return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_UPDATE_LEADER);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Delete multi
        /// </summary>
        /// <param name="request"></param>
        /// <param name="listGroupDelete">list group id to delete</param>
        /// <returns>Return a message success or not</returns>
        [HttpPost]
        [Route(Common.Constants.RoutesConstant.DeleteMultiGroup)]
        public async Task<HttpResponseMessage> DeleteMulti(HttpRequestMessage request, List<GroupCreateUpdateModel> listGroupDelete)
        {

            if (listGroupDelete.Count!=0)
            {
                int numberOfGroupInvalid = 0;
                foreach (var group in listGroupDelete)
                {
                    if (!_groupService.IsGroupEmpty(group.ID, group.GroupLeadID))
                    {
                        numberOfGroupInvalid++;
                    }
                }
                if (numberOfGroupInvalid == 0)
                {
                    foreach (var group in listGroupDelete)
                    {
                        if (_groupService.Delete(group.ID) != null)
                        {
                            var groupleaderID = AppRoleManager.FindByNameAsync(CommonConstants.GroupLead).Result.Id;
                            var user = AppUserManager.Users.Where(x => x.Roles.Any(r => r.RoleId.Equals(groupleaderID) && x.GroupId == group.ID)).FirstOrDefault();
                            if (user != null)
                            {
                                user.GroupId = null;
                                var resultUpdate = await AppUserManager.UpdateAsync(user);
                            }
                            _groupService.SaveChange();
                        }
                    }
                    return request.CreateResponse(HttpStatusCode.OK, listGroupDelete);
                }
                else
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_ExistMemberInGroup);
                }
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.SelectGroupToDelete);
        }
        /// <summary>
        /// View group detail by id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id">group id of a group</param>
        /// <returns>Return group information</returns>
        [Route(Common.Constants.RoutesConstant.DetailsGroup)]
        [HttpGet]
        public HttpResponseMessage Details(HttpRequestMessage request, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return request.CreateResponse(HttpStatusCode.BadRequest, nameof(id) + Common.Constants.MessageSystem.NoValues);
            }
            var group = _groupService.GetGroupById(id);
            if (group == null)
            {
                return request.CreateResponse(HttpStatusCode.NoContent, Common.Constants.MessageSystem.NoData);
            }
            else
            {
                var groupViewModel = Mapper.Map<Group, GroupViewModel>(group);
                var groupLeadID = _groupService.GetGroupLeadIdByGroup(group.ID);
                if (groupLeadID != null)
                {
                    groupViewModel.GroupLead = AppUserManager.FindByIdAsync(groupLeadID).Result.FullName;
                }
                else
                {
                    groupViewModel.GroupLead = CommonConstants.DefaultGroupLeader;
                }
                return request.CreateResponse(HttpStatusCode.OK, groupViewModel);
            }
        }


        /// <summary>
        /// Set delegate default (function for grouplead)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="applicationUserViewModel">Set delegate default has updated</param>
        /// <returns>Return a message success or not</returns>
        [HttpPut]
        [Route(Common.Constants.RoutesConstant.SetDelegateDefault)]
        public async Task<HttpResponseMessage> SetDelegateDefault(HttpRequestMessage request, string userId, string groupId, GroupCreateUpdateModel groupVm)
        {
            if (!ModelState.IsValid)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            else
            {
                List<string> lstRequestId = new List<string>();
                List<string> lstExplanationRequestId = new List<string>();
                List<object> lstData = new List<object>();
                List<Request> lstRequest = new List<Request>();
                List<ExplanationRequest> lstExplanationRequest = new List<ExplanationRequest>();
                var ChangeStatusById = AppUserManager.FindByNameAsync(User.Identity.Name).Result.Id;
                var newGroup = _groupService.GetGroupById(groupVm.ID.ToString());
                newGroup.UpdateGroup(groupVm);
                _groupService.Update(newGroup);
                _groupService.SaveChange();
                
                //Get request by user and group.Filter by StatusRequest is Pendding
                var model = _requestService.GetAllRequestByUser(userId, groupId);
                var group = _groupService.GetGroupById(groupId);
                var data = model.Where(x => x.StatusRequest.Name.Equals(CommonConstants.StatusPending));
                if (group.DelegateId != null)
                {
                    foreach (var item in data)
                    {
                        if (item.CreatedDate != null)
                        {
                            if (group.StartDate <= item.CreatedDate.Value.Date && group.EndDate >= item.CreatedDate.Value.Date)
                            {
                                lstRequestId.Add(item.ID.ToString());
                                lstRequest.Add(item);                 
                            }
                        }
                    }
                    lstData.AddRange(lstRequest);
                    //Change status request by delegate default
                    _requestService.ChangeStatusDelegateDefault(CommonConstants.StatusDelegation, newGroup.DelegateId, newGroup.StartDate.Value, newGroup.EndDate.Value, ChangeStatusById, lstRequestId.ToArray());
                }
                

                //Get explanation by userid and groupid.Filter by StatusRequest is Pending
                var explanation = _explanationRequestService.GetListExplanationByUser(userId, groupId).Where(a => a.StatusRequest.Name.Equals(CommonConstants.StatusPending));
               
                if (explanation.Count() > 0  && group.DelegateId != null)
                {
                    foreach (var explanationItem in explanation)
                    {
                        if (explanationItem.CreatedDate != null)
                        {
                            if (group.StartDate <= explanationItem.CreatedDate.Value.Date && group.EndDate >= explanationItem.CreatedDate.Value.Date )
                            {
                                lstExplanationRequest.Add(explanationItem);
                                lstExplanationRequestId.Add(explanationItem.ID.ToString());
                            }
                        }
                    }
                    lstData.AddRange(lstExplanationRequest);
                    //Change status explanation request by delegate default
                    _explanationRequestService.ChangeStatusExplanationDelegateDefault(lstExplanationRequestId.ToArray(), CommonConstants.StatusDelegation, newGroup.DelegateId, newGroup.StartDate.Value, newGroup.EndDate.Value);
                }

                return request.CreateResponse(HttpStatusCode.Created, lstData);
            }
        }


        /// <summary>
        /// reset delegate default by id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(RoutesConstant.ReSetDelegateDefault)]
        public async Task<HttpResponseMessage> ReSetDelegateDefault(HttpRequestMessage request, int id)
        {

            var groupData = _groupService.GetGroupById(id.ToString());
            groupData.ResetConfigDelegate();
            _groupService.Update(groupData);
            _groupService.SaveChange();
            return request.CreateResponse(HttpStatusCode.OK, groupData);
        }
    }
}
