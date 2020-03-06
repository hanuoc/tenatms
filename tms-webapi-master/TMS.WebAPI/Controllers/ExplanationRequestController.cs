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
using TMS.Common.ViewModels;
using TMS.Data;
using TMS.Model.Models;
using TMS.Service;
using TMS.Web.Infrastructure.Core;
using TMS.Web.Infrastructure.Extensions;
using TMS.Web.Models;
using TMS.Web.Models.AbnormalCase;
using TMS.Web.Models.Explanation;
using TMS.Web.Models.Request;

namespace TMS.Web.Controllers
{
    [RoutePrefix(RoutesConstant.ExplanationApi)]
    [Authorize]
    public class ExplanationRequestController : ApiControllerBase
    {
        private IExplanationRequestService _explanationRequestService;
        private IFingerTimeSheetService _fingertimesheetService;
        private ISystemService _systemService;
        private IGroupService __groupService;
        private IConfigDelegationService _configDelegationService;
		private ICommonService _commonService;
		public ExplanationRequestController(IErrorService errorService, IExplanationRequestService explanationRequestService, ISystemService systemService, IFingerTimeSheetService fingertimesheetService, IGroupService groupService, IConfigDelegationService configDelegationService, ICommonService commonService) : base(errorService)
        {
            this._explanationRequestService = explanationRequestService;
            this._systemService = systemService;
            _fingertimesheetService = fingertimesheetService;
            this.__groupService = groupService;
            this._configDelegationService = configDelegationService;
			_commonService = commonService;

		}

        /// <summary>
        /// Get explanation detail by id
        /// </summary>
        /// <param name="id">id of explanation</param>
        /// <returns> HttpResponseMessage </returns>
        [Route(RoutesConstant.ExplanationDetail)]
        [HttpGet]
        public async Task<HttpResponseMessage> GetExplanationDetail(HttpRequestMessage request, int id)
        {
            return await CreateHttpResponse(request, () =>
            {
                return request.CreateResponse(HttpStatusCode.OK, GetExplanationDetailViewModel(id));
            });
        }

        /// <summary>
        /// Get explanations list after sort, filter or get original list(without sort, filter)
        /// </summary>
        /// <param name="userId">id of user logged</param>
        /// <param name="groupId">groupId of user logged</param>
        /// <param name="column">name of column to sort</param>
        /// <param name="isDesc">parameter to check to sort by asc or desc</param>
        /// <param name="page">current page(use in paging)</param>
        /// <param name="pageSize">number of page showing(use in paging)</param>
        /// <param name="filter">list conditions to filter</param>
        /// <returns> HttpResponseMessage </returns>
        [Route(RoutesConstant.ExplanationList)]
        [HttpPost]
        public async Task<HttpResponseMessage> GetExplanationsList(HttpRequestMessage request, string userId,
            string groupId, string column, bool isDesc, int page, int pageSize, [FromBody]FilterExplanationViewModel filter)
        {
            return await CreateHttpResponse(request, () =>
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(groupId))
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(userId) + MessageSystem.NoValues + nameof(groupId) + MessageSystem.NoValues);
                }
                //var model = GetExplanationListViewModel(userId, groupId, column, isDesc, page, pageSize, filter);
                var model = _explanationRequestService.GetListOrigin(userId, groupId);
                var user = AppUserManager.Users.Where(x => x.Id.Equals(userId)).FirstOrDefault();
                foreach (var item in model)
                {
                    if (item.StatusRequest.Name == CommonConstants.StatusDelegation)
                    {
                        _explanationRequestService.CheckDataDelegationExplanationRequest(Convert.ToInt32(user.GroupId), item.ID);
                    }
                }
                _explanationRequestService.Save();
                var data = GetExplanationListViewModel(userId, groupId, column, isDesc, page, pageSize, filter);
				data = CheckExpireDate(data);
                PaginationSet<ExplanationRequestViewModel> pagedSet = new PaginationSet<ExplanationRequestViewModel>()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    TotalRows = _explanationRequestService.GetTotalEntries(),
                    Items = data,
                };
                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, pagedSet);
                return response;
            });
        }

        /// <summary>
        /// Bind data from explanation detail to view model
        /// </summary>
        /// <param name="id">id of explanation</param>
        /// <returns> ExplanationRequestViewModel </returns>
        public ExplanationRequestViewModel GetExplanationDetailViewModel(int id)
        {
            List<ExplanationRequestViewModel> listModel = new List<ExplanationRequestViewModel>();

            // get explanation single
            var model = _explanationRequestService.GetExplanationDetail(id);

            // bind data to view model
            ExplanationRequestViewModel modelView = new ExplanationRequestViewModel
            {
                ID = model.ID,
                Title = model.Title,
                Actual = model.Actual,
                GroupName = model.AppUserCreatedBy.Group.Name,
                FullNameDelegate = AppUserManager.Users.Where(x => x.Id.Equals(model.DelegateId)).FirstOrDefault() != null ? AppUserManager.Users.Where(x => x.Id.Equals(model.DelegateId)).FirstOrDefault().FullName : null,
                User = new AppUserViewModel
                {
                    Id = model.AppUserCreatedBy.Id,
                    FullName = model.AppUserCreatedBy.FullName,
                    Group = model.FingerTimeSheet.FingerMachineUsers.AppUser.Group.Name != null ? new GroupViewModel
                    {
                        Name = model.FingerTimeSheet.FingerMachineUsers.AppUser.Group.Name
                    } : new GroupViewModel { }
                },
                Receiver = new AppUserViewModel
                {
                    Id = model.Receiver.Id,
                    FullName = model.Receiver.FullName
                },
                ReasonDetail = model.ReasonDetail,
                CreatedDate = (DateTime)model.CreatedDate,
                CreatedBy = model.CreatedBy,
                FullName = model.AppUserCreatedBy.FullName,
                ExplanationDate = model.FingerTimeSheet.DayOfCheck,
                StatusRequest = new StatusRequestViewModel
                {
                    ID = model.StatusRequest.ID,
                    Name = model.StatusRequest.Name
                },
                Approver = model.UpdatedBy != null ? new AppUserViewModel
                {
                    Id = model.AppUserUpdatedBy.Id,
                    FullName = model.AppUserUpdatedBy.FullName
                } : new AppUserViewModel { },
                AbnormalReason = _explanationRequestService.GetAbnormalById(model.TimeSheetId).Select(m => new AbnormalCaseReasonViewModel
                {
                    ReasonId = m.AbnormalReason.ID,
                    ReasonName = m.AbnormalReason.Name
                }).ToList()
            };

            // convert reason name in list to string
            ConvertSubstringToString(modelView);
            return modelView;
        }

        /// <summary>
        /// Convert string in list into string
        /// </summary>
        /// <param name="model"><ExplanationRequestViewModel/param>
        /// <returns>ExplanationRequestViewModel</returns>
        public ExplanationRequestViewModel ConvertSubstringToString(ExplanationRequestViewModel model)
        {
            for (int i = 0; i < model.AbnormalReason.Count; i++)
            {
                if (i < model.AbnormalReason.Count - 1)
                {
                    model.ReasonList += model.AbnormalReason[i].ReasonName + ", ";
                }
                else
                {
                    model.ReasonList += model.AbnormalReason[i].ReasonName;
                }
            }
            return model;
        }

        /// <summary>
        /// Get list explanation request is assigned
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId">ID of username login</param>
        /// <param name="groupId">group of user</param>
        /// <param name="column">name of column to sort</param>
        /// <param name="isDesc">parameter to check to sort by asc or desc</param>
        /// <param name="page">page current of list</param>
        /// <param name="pageSize">total page of list</param>
        /// <param name="filter">parameters want to filter</param>
        /// <returns> HttpResponseMessage </returns>
        [Route(RoutesConstant.RequestAssignedList)]
        [HttpPost]
        public async Task<HttpResponseMessage> GetExplanationsRequestAssignedList(HttpRequestMessage request, string userId,
           string groupId, string column, bool isDesc, int page, int pageSize, [FromBody]FilterDelegationAssignedModel filter)
        {
            return await CreateHttpResponse(request, () =>
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(groupId))
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(userId) + MessageSystem.NoValues + nameof(groupId) + MessageSystem.NoValues);
                }
                var model = GetExplanationAssignedListViewModel(userId, groupId, column, isDesc, page, pageSize, filter);
                var totalRow = model.Count();
				model = CheckExpireDate(model);
				PaginationSet<ExplanationRequestViewModel> pagedSet = new PaginationSet<ExplanationRequestViewModel>()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    TotalRows = totalRow,
                    Items = model,
                };
                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, pagedSet);
                return response;
            });
        }

        /// <summary>
        /// Bind data explanation list to view model
        /// </summary>
        /// <param name="userId">id of user logged</param>
        /// <param name="groupId">groupId of user logged</param>
        /// <param name="column">name of column to sort</param>
        /// <param name="isDesc">parameter to check to sort by asc or desc</param>
        /// <param name="page">current page(use in paging)</param>
        /// <param name="pageSize">number of page showing(use in paging)</param>
        /// <param name="filter">list conditions to filter</param>
        /// <returns> explanations list as view model type </returns>
        public List<ExplanationRequestViewModel> GetExplanationAssignedListViewModel(string userId,
            string groupId, string column, bool isDesc, int page, int pageSize, FilterDelegationAssignedModel filter)
        {
            List<ExplanationRequestViewModel> listModel = new List<ExplanationRequestViewModel>();

            // get explanation list
            //var model = _explanationRequestService.GetExplanationsRequestAssignedList(userId, groupId, column, isDesc, page, pageSize, filter);
            var model = _explanationRequestService.GetListExplanationAssigned(userId, groupId);
            //Check Data have status delegated have date now > end date
            foreach (var item in model)
            {
                if (item.StatusRequest.Name == CommonConstants.StatusDelegation)
                {
                    var user = AppUserManager.Users.Where(x => x.Id == item.ReceiverId).FirstOrDefault();
                    //_explanationRequestService.CheckDataDelegationExplanationRequest(item.AppUserCreatedBy.Group.ID, item.ID);
                    _explanationRequestService.CheckDataDelegationAllExplanationRequest(item.AppUserCreatedBy.Group.ID, item.ID, Convert.ToInt32(user.GroupId));

                }
            }
            _explanationRequestService.Save();
            var data = _explanationRequestService.GetExplanationsRequestAssignedList(userId, groupId, column, isDesc, page, pageSize, filter);
            // bind data to view model
            foreach (var item in data)
            {
                listModel.Add(new ExplanationRequestViewModel
                {
                    ID = item.ID,
                    Title = item.Title,
                    User = new AppUserViewModel
                    {
                        Id = item.AppUserCreatedBy.Id,
                        FullName = item.AppUserCreatedBy.FullName,
                        Email = item.AppUserCreatedBy.Email,
                        Group = new GroupViewModel
                        {
                            Name = item.FingerTimeSheet.FingerMachineUsers.AppUser.Group.Name
                        }
                    },
                    Receiver = new AppUserViewModel
                    {
                        Id = item.Receiver.Id,
                        FullName = item.Receiver.FullName
                    },
                    ReasonDetail = item.ReasonDetail,
                    CreatedDate = (DateTime)item.CreatedDate,
                    UpdatedDate = item.UpdatedDate,
                    ExplanationDate = item.FingerTimeSheet.DayOfCheck,
                    StatusRequest = new StatusRequestViewModel
                    {
                        ID = item.StatusRequest.ID,
                        Name = item.StatusRequest.Name
                    },
                    Approver = item.UpdatedBy != null ? new AppUserViewModel
                    {
                        Id = item.AppUserUpdatedBy.Id,
                        FullName = item.AppUserUpdatedBy.FullName
                    } : new AppUserViewModel { },
                    Delegate = item.DelegateId != null ? new AppUserViewModel
                    {
                        Id = item.Delegate.Id,
                        FullName = item.Delegate.FullName
                    } : new AppUserViewModel { },
                    AbnormalReason = _explanationRequestService.GetAbnormalById(item.TimeSheetId).Select(m => new AbnormalCaseReasonViewModel
                    {
                        ReasonId = m.AbnormalReason.ID,
                        ReasonName = m.AbnormalReason.Name
                    }).ToList()
                });
            }
            return listModel;
        }

        /// <summary>
        /// Bind data explanation list to view model
        /// </summary>
        /// <param name="userId">id of user logged</param>
        /// <param name="groupId">groupId of user logged</param>
        /// <param name="column">name of column to sort</param>
        /// <param name="isDesc">parameter to check to sort by asc or desc</param>
        /// <param name="page">current page(use in paging)</param>
        /// <param name="pageSize">number of page showing(use in paging)</param>
        /// <param name="filter">list conditions to filter</param>
        /// <returns> explanations list as view model type </returns>
        public List<ExplanationRequestViewModel> GetExplanationListViewModel(string userId,
            string groupId, string column, bool isDesc, int page, int pageSize, FilterExplanationViewModel filter)
        {
            List<ExplanationRequestViewModel> listModel = new List<ExplanationRequestViewModel>();

            // get explanation list
            var model = _explanationRequestService.GetExplanationsList(userId, groupId, column, isDesc, page, pageSize, filter);

            // bind data to view model
            foreach (var item in model)
            {
                listModel.Add(new ExplanationRequestViewModel
                {
                    ID = item.ID,
                    Title = item.Title,
                    User = new AppUserViewModel
                    {
                        Id = item.AppUserCreatedBy.Id,
                        FullName = item.AppUserCreatedBy.FullName,
                        Email = item.AppUserCreatedBy.Email,
                        Group = new GroupViewModel
                        {
                            Name = item.FingerTimeSheet.FingerMachineUsers.AppUser.Group.Name
                        }
                    },
                    Receiver = new AppUserViewModel
                    {
                        Id = item.Receiver.Id,
                        FullName = item.Receiver.FullName
                    },
                    ReasonDetail = item.ReasonDetail,
                    CreatedDate = (DateTime)item.CreatedDate,
                    ExplanationDate = item.FingerTimeSheet.DayOfCheck,
                    StatusRequest = new StatusRequestViewModel
                    {
                        ID = item.StatusRequest.ID,
                        Name = item.StatusRequest.Name
                    },
                    Approver = item.UpdatedBy != null ? new AppUserViewModel
                    {
                        Id = item.AppUserUpdatedBy.Id,
                        FullName = item.AppUserUpdatedBy.FullName
                    } : new AppUserViewModel { },
                    AbnormalReason = _explanationRequestService.GetAbnormalById(item.TimeSheetId).Select(m => new AbnormalCaseReasonViewModel
                    {
                        ReasonId = m.AbnormalReason.ID,
                        ReasonName = m.AbnormalReason.Name
                    }).ToList()
                });
            }
            return listModel;
        }

        /// <summary>
        /// Change explanation status
        /// </summary>
        /// <param name="explanationId">id of explanation to change status</param>
        /// <param name="statusName">name of status to change</param>
        /// <returns> HttpResponseMessage </returns>
        [Route(RoutesConstant.ExplanationChangeStatus)]
        [HttpPost]
        public async Task<HttpResponseMessage> ChangeStatus(HttpRequestMessage request, int explanationId, string statusName, string delegateId)
        {
            return await CreateHttpResponse(request, () =>
            {
                var dateNow = DateTime.Now.Date;
                var explanationEntity = _explanationRequestService.GetExplanationDetail(explanationId);
				var DateExRequestInPast = _commonService.GetDateExRequestInPast(explanationEntity.CreatedDate.Value);

				if (DateExRequestInPast.Date < dateNow)
				{
					return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_CHANGESTATUS_EXREQUEST_IN_PAST_NOT_MSG);
				}
				
				//Check if explanation request in aboutime set delegate default and status is Delegated,will not set delegate 
				if (explanationEntity != null && statusName.Equals(CommonConstants.StatusDelegation))
                {
                    var group = __groupService.GetGroupById(explanationEntity.AppUserCreatedBy.GroupId.ToString());
                    if (group.EndDate != null && explanationEntity.CreatedDate != null)
                    {
                        if (group.EndDate.Value.Date < DateTime.Now.Date && group.StartDate <= explanationEntity.CreatedDate.Value.Date && group.EndDate >= explanationEntity.CreatedDate.Value.Date)
                        {
                            return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_CHANGESTATUS_REQUEST_NOT_IN_DELEGATEDEFAULT_TIME_MSG);
                        }
                    }
                }
                string error;
                bool rs = _explanationRequestService.ChangeStatus(explanationId, statusName, delegateId, out error);
                if (rs == true)
                {
                    _explanationRequestService.Save();
                    return request.CreateErrorResponse(HttpStatusCode.OK, MessageSystem.Change_Explanation_Status_Success);
                }
                else
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, error);
                }
            });
        }

        /// <summary>
        /// Adding a explanation request into db
        /// </summary>
        /// <param name="explanationViewModel">explanation request view model</param>
        /// <returns>HttpResponseMessage</returns>
        [Route(RoutesConstant.CreateExplanation)]
        [HttpPost]
        public async Task<HttpResponseMessage> Create(HttpRequestMessage request, string OTCheckIn, string OTCheckOut, ExplanationRequestViewModel explanationViewModel)
        {
            return await CreateHttpResponse(request, () =>
			{
				var checkDelegate = new CheckDelegateModel();
				var dateNow = DateTime.Now.Date;
				var timesheet = _fingertimesheetService.GetById(explanationViewModel.TimeSheetId);
				var DateExRequestInPast = _commonService.GetDateExRequestInPast(timesheet.DayOfCheck);
				if (DateExRequestInPast.Date < dateNow)
				{
					return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_CREATE_EXREQUEST_IN_PAST_NOT_MSG);
				}
				var explanation = new ExplanationRequest();
                explanation.UpdateExplanationRequest(explanationViewModel);
                if (explanation.Actual == "Leave")
                {
                    var entitleDayAppUser = _explanationRequestService.GetEntitleDayByUserID(explanation.CreatedBy);
                    var timeSheet = _fingertimesheetService.GetById(explanationViewModel.TimeSheetId);
                    if ((timeSheet.Absent == CommonConstants.TimeSheetAbsent && entitleDayAppUser.MaxEntitleDayAppUser - entitleDayAppUser.NumberDayOff >= 1)
                    || ((timeSheet.Absent == CommonConstants.TimeSheetAbsentAfternoon || timeSheet.Absent == CommonConstants.TimeSheetAbsentMorning)
                    && entitleDayAppUser.MaxEntitleDayAppUser - entitleDayAppUser.NumberDayOff >= 0.5))
                    {
                        if (_explanationRequestService.Add(explanation, OTCheckIn, OTCheckOut))
                        {
                            var group = __groupService.GetGroupById(explanationViewModel.GroupId.ToString());
                            var dataDelegation = _configDelegationService.GetDelegationByUserId(explanation.CreatedBy);
                            if (dataDelegation.StartDate <= explanation.CreatedDate.Value.Date && dataDelegation.EndDate >= explanation.CreatedDate.Value.Date)
                            {
                                checkDelegate.CheckConfigDelegateDefault = true;
                                checkDelegate.AssignConfigDelegate = dataDelegation.AssignTo;
                                _configDelegationService.ChangeStatusAfterAddExplanationRequest(dataDelegation.AssignTo, explanation);
                            }
                            else
                            {
                                if (explanation.CreatedDate != null)
                                {
                                    if (group.DelegateId != null && group.StartDate <= explanation.CreatedDate.Value.Date && group.EndDate >= explanation.CreatedDate.Value.Date)
                                    {
                                        checkDelegate.CheckGroupDelegateDefault = true;
                                        _explanationRequestService.AddDelegateDefault(explanationViewModel.GroupId, explanation.ID);
                                    }
                                }
                            }
                            return request.CreateResponse(HttpStatusCode.Created, checkDelegate);
                        }
                    }
                    else
                    {
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_CREATE_EXREQUEST_NOT_ENOUGH_ENTITLE_DAY);
                    }
                }
                if (_explanationRequestService.Add(explanation, OTCheckIn, OTCheckOut))
                {
                    var groupData = __groupService.GetGroupById(explanationViewModel.GroupId.ToString());
                    var dataDelegationConfig = _configDelegationService.GetDelegationByUserId(explanation.CreatedBy);
                    if (dataDelegationConfig.StartDate <= explanation.CreatedDate.Value.Date && dataDelegationConfig.EndDate >= explanation.CreatedDate.Value.Date)
                    {
                        checkDelegate.CheckConfigDelegateDefault = true;
                        checkDelegate.AssignConfigDelegate = dataDelegationConfig.AssignTo;
                        _configDelegationService.ChangeStatusAfterAddExplanationRequest(dataDelegationConfig.AssignTo, explanation);
                    }
                    else
                    {
                        if (explanation.CreatedDate != null)
                        {
                            if (groupData.DelegateId != null && groupData.StartDate <= explanation.CreatedDate.Value.Date && groupData.EndDate >= explanation.CreatedDate.Value.Date)
                            {
                                checkDelegate.CheckGroupDelegateDefault = true;
                                _explanationRequestService.AddDelegateDefault(explanationViewModel.GroupId, explanation.ID);
                            }
                        }
                    }
                    return request.CreateResponse(HttpStatusCode.Created, checkDelegate);
                }
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, MessageSystem.Create_Explanation_Error);
            });
        }

        /// <summary>
        /// Get list creator who created explanation
        /// </summary>
        /// <param name="userId">id of user logged</param>
        /// <param name="groupId">groudId of user logged</param>
        /// <returns>HttpResponseMessage</returns>
        [Route(RoutesConstant.GetExplanationCreator)]
        [HttpGet]
        public async Task<HttpResponseMessage> GetExplanationCreatorList(HttpRequestMessage request, string userId, string groupId)
        {
            return await CreateHttpResponse(request, () =>
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(groupId))
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, MessageSystem.NoValues);
                }
                var model = _explanationRequestService.GetListCreator(userId, groupId);
                List<AppUserViewModel> userModel = new List<AppUserViewModel>();
                foreach (var user in model)
                {
                    userModel.Add(new AppUserViewModel
                    {
                        Id = user.Id,
                        FullName = user.FullName
                    });
                }
                return request.CreateResponse(HttpStatusCode.OK, userModel);
            });
        }

        [Route(RoutesConstant.ExplanationChangeStatusMulti)]
        [HttpPost]
        public HttpResponseMessage ChangeStatusMulti(HttpRequestMessage request, string statusName, string delegateId, string[] explanationId)
        {
            if (string.IsNullOrEmpty(explanationId.ToString()) || string.IsNullOrEmpty(statusName))
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(explanationId) + MessageSystem.NoValues + nameof(statusName) + MessageSystem.NoValues);
            }
            if (ModelState.IsValid)
            {
                var dateNow = DateTime.Now.Date;
                foreach (var item in explanationId)
                {
                    var explanationEntity = _explanationRequestService.GetExplanationDetail(Int32.Parse(item));
                    var timesheet = _explanationRequestService.GetFingerTimeSheetByExplanationID(Int32.Parse(item)).DayOfCheck.AddDays(2);
					var DateExRequestInPast = _commonService.GetDateExRequestInPast(explanationEntity.CreatedDate.Value);
					if (DateExRequestInPast.Date < dateNow)
					{
						return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_CHANGESTATUS_EXREQUEST_IN_PAST_NOT_MSG);
					}
					//Check if explanation request in aboutime set delegate default and status is Delegated,will not set delegate
					if (explanationEntity != null && statusName.Equals(CommonConstants.StatusDelegation))
                    {
                        var group = __groupService.GetGroupById(explanationEntity.AppUserCreatedBy.GroupId.ToString());
                        if (group.EndDate != null && explanationEntity.CreatedDate != null)
                        {
                            if (group.EndDate.Value.Date < DateTime.Now.Date && group.StartDate <= explanationEntity.CreatedDate.Value.Date && group.EndDate >= explanationEntity.CreatedDate.Value.Date)
                            {
                                return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_CHANGESTATUS_REQUEST_NOT_IN_DELEGATEDEFAULT_TIME_MSG);
                            }
                        }

                    }
                }
                string error;
                if (_explanationRequestService.ChangeStatusMulti(explanationId, statusName, delegateId, out error))
                {
                    _explanationRequestService.Save();
                    return request.CreateResponse(HttpStatusCode.OK, MessageSystem.Create_Explanation_Success);
                }
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, error);
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [Route(RoutesConstant.ExRequestChart)]
        [HttpGet]
        public async Task<HttpResponseMessage> ChangeStatusMulti(HttpRequestMessage request, int groupID)
        {
            return await CreateHttpResponse(request, () =>
            {
                return request.CreateResponse(HttpStatusCode.OK, _explanationRequestService.ExRequestChart(groupID));
            });
        }
        [Route(RoutesConstant.exrequestByUser)]
        [HttpGet]
        public async Task<HttpResponseMessage> ExRequestByUser(HttpRequestMessage request, string userID)
        {

            return await CreateHttpResponse(request, () =>
            {
                return request.CreateResponse(HttpStatusCode.OK, _explanationRequestService.ExRequestByUser(userID));
            });
        }

        [Route(RoutesConstant.ExRequestChartByUser)]
        [HttpGet]
        public async Task<HttpResponseMessage> ExRequestChartByUser(HttpRequestMessage request, string userID)
        {

            return await CreateHttpResponse(request, () =>
            {
                return request.CreateResponse(HttpStatusCode.OK, _explanationRequestService.ExRequestChartByUser(userID));
            });
        }

        /// <summary>
        /// Get explanation detail by id
        /// </summary>
        /// <param name="lstExplanationid">id of explanation</param>
        /// <returns> HttpResponseMessage </returns>
        [Route(RoutesConstant.GetListExplanationDetail)]
        [HttpPut]
        public async Task<HttpResponseMessage> GetListExplanationDetail(HttpRequestMessage request, string[] explanationId)
        {
            return await CreateHttpResponse(request, () =>
            {
                List<ExplanationRequestViewModel> lstExplanationRequest = new List<ExplanationRequestViewModel>();

                if (explanationId != null)
                {
                    foreach (var item in explanationId)
                    {
                        var data = GetExplanationDetailViewModel(Convert.ToInt32(item));
                        lstExplanationRequest.Add(data);
                    }
                }
                return request.CreateResponse(HttpStatusCode.OK, lstExplanationRequest);
            });
        }


        /// <summary>
        /// Get explanations list after sort, filter or get original list(without sort, filter) for super admin
        /// </summary>
        /// <param name="column">name of column to sort</param>
        /// <param name="isDesc">parameter to check to sort by asc or desc</param>
        /// <param name="page">current page(use in paging)</param>
        /// <param name="pageSize">number of page showing(use in paging)</param>
        /// <param name="filter">list conditions to filter</param>
        /// <returns> HttpResponseMessage </returns>
        [Route(RoutesConstant.GetAllExplanationListRoleSuperAdmin)]
        [HttpPost]
        public async Task<HttpResponseMessage> GetAllExplanationsList(HttpRequestMessage request, string column, bool isDesc, int page, int pageSize, [FromBody]FilterExplanationViewModel filter)
        {
            return await CreateHttpResponse(request, () =>
            {

                var model = GetExplanationListForSuperAdmin(column, isDesc, page, pageSize, filter);
                PaginationSet<ExplanationRequestViewModel> pagedSet = new PaginationSet<ExplanationRequestViewModel>()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    TotalRows = _explanationRequestService.GetTotalEntries(),
                    Items = model,
                };
                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, pagedSet);
                return response;
            });
        }


        /// <summary>
        /// Bind data explanation list to view model
        /// </summary>
        /// <param name="userId">id of user logged</param>
        /// <param name="groupId">groupId of user logged</param>
        /// <param name="column">name of column to sort</param>
        /// <param name="isDesc">parameter to check to sort by asc or desc</param>
        /// <param name="page">current page(use in paging)</param>
        /// <param name="pageSize">number of page showing(use in paging)</param>
        /// <param name="filter">list conditions to filter</param>
        /// <returns> explanations list as view model type </returns>
        public List<ExplanationRequestViewModel> GetExplanationListForSuperAdmin(string column, bool isDesc, int page, int pageSize, FilterExplanationViewModel filter)
        {
            List<ExplanationRequestViewModel> listModel = new List<ExplanationRequestViewModel>();

            // get explanation list
            var model = _explanationRequestService.GetExplanationsListForSuperAdmin(column, isDesc, page, pageSize, filter);

            // bind data to view model
            foreach (var item in model)
            {
                listModel.Add(new ExplanationRequestViewModel
                {
                    ID = item.ID,
                    Title = item.Title,
                    User = new AppUserViewModel
                    {
                        Id = item.AppUserCreatedBy.Id,
                        FullName = item.AppUserCreatedBy.FullName,
                        Email = item.AppUserCreatedBy.Email,
                        Group = new GroupViewModel
                        {
                            Name = item.FingerTimeSheet.FingerMachineUsers.AppUser.Group.Name
                        }
                    },
                    Receiver = new AppUserViewModel
                    {
                        Id = item.Receiver.Id,
                        FullName = item.Receiver.FullName
                    },
                    ReasonDetail = item.ReasonDetail,
                    CreatedDate = (DateTime)item.CreatedDate,
                    ExplanationDate = item.FingerTimeSheet.DayOfCheck,
                    StatusRequest = new StatusRequestViewModel
                    {
                        ID = item.StatusRequest.ID,
                        Name = item.StatusRequest.Name
                    },
                    Approver = item.UpdatedBy != null ? new AppUserViewModel
                    {
                        Id = item.AppUserUpdatedBy.Id,
                        FullName = item.AppUserUpdatedBy.FullName
                    } : new AppUserViewModel { },
                    AbnormalReason = _explanationRequestService.GetAbnormalById(item.TimeSheetId).Select(m => new AbnormalCaseReasonViewModel
                    {
                        ReasonId = m.AbnormalReason.ID,
                        ReasonName = m.AbnormalReason.Name
                    }).ToList()
                });
            }
            return listModel;
        }

		public List<ExplanationRequestViewModel> CheckExpireDate(List<ExplanationRequestViewModel> lstExplanation)
		{
			foreach (var item in lstExplanation)
			{
				var DateExRequestInPast = _commonService.GetDateExRequestInPast(item.CreatedDate.Value);

				if (DateExRequestInPast.Date < DateTime.Now.Date)
				{
					item.IsExpiredDate = true;
				}
			}
			return lstExplanation;
		}

	}
}
