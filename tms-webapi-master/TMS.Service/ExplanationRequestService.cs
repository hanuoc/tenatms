using System;
using System.Collections.Generic;
using System.Linq;
using TMS.Data.Infrastructure;
using System.Globalization;
using TMS.Common.Constants;
using TMS.Common.Exceptions.Extensions;
using TMS.Common.ViewModels;
using TMS.Data;
using TMS.Data.Repositories;
using TMS.Model.Models;
using System.Data.Entity.Infrastructure;

namespace TMS.Service
{
    public interface IExplanationRequestService
    {
        /// <summary>
        /// Get total explanation elements
        /// </summary>
        /// <returns> int </returns>
        int GetTotalEntries();

        /// <summary>
        /// Get explanations list after sort, filter
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="groupId"></param>
        /// <param name="column"></param>
        /// <param name="isDesc"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="filter"></param>
        /// <returns>explanations list</returns>
        /// 
        IEnumerable<ExplanationRequest> GetExplanationsRequestAssignedList(string userID, string groupId, string column, bool isDesc, int page, int pageSize, FilterDelegationAssignedModel filter);

        /// Get list original explanation request without sort, filter
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="group"></param>
        /// <returns>explanation request list</returns>
        IEnumerable<ExplanationRequest> GetListExplanationAssigned(string userID, string groupId);

        /// <summary>
        /// list explanation request is assigned
        /// </summary>
        /// <param name="query"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        IEnumerable<ExplanationRequest> GetListRequestAssignedFilter(IEnumerable<ExplanationRequest> query, FilterDelegationAssignedModel filter);

        /// <summary>
        /// Get list original explanation request without sort, filter
        /// </summary>
        /// <param name="userId">id of user logged</param>
        /// <param name="groupId">groupId of user logged</param>
        /// <returns> explanations list </returns>
        IEnumerable<ExplanationRequest> GetListOrigin(string userId, string groupId);

        /// <summary>
        /// Get explanation detail by id
        /// </summary>
        /// <param name="id">id of explanation</param>
        /// <returns> ExplanationRequest </returns>
        ExplanationRequest GetExplanationDetail(int id);

        /// <summary>
        /// Get explanations list after sort, filter
        /// </summary>
        /// <param name="userId">id of user logged</param>
        /// <param name="groupId">groupId of user logged</param>
        /// <param name="column">name of column to sort</param>
        /// <param name="isDesc">parameter to check to sort by asc or desc</param>
        /// <param name="page">current page(use in paging)</param>
        /// <param name="pageSize">number of page showing(use in paging)</param>
        /// <param name="filter">list conditions to filter</param>
        /// <returns> explanations list </returns>
        IEnumerable<ExplanationRequest> GetExplanationsList(string userId, string groupId, string column, bool isDesc, int page, int pageSize, FilterExplanationViewModel filter);

        /// <summary>
        /// Get Explanation request after filter
        /// </summary>
        /// <param name="query">list explanations</param>
        /// <param name="filter">list filter conditions</param>
        /// <returns> explanations list </returns>
        IEnumerable<ExplanationRequest> GetListByFilter(IEnumerable<ExplanationRequest> query, FilterExplanationViewModel filter);

        /// Get AbnormalCaseReason by abnormal id
        /// </summary>
        /// <param name="id">id of abnormal</param>
        /// <returns> AbnormalCaseReason </returns>
        //IQueryable<AbnormalCaseReason> GetAbnormalById(int id);

        /// <summary>
        /// Change status of an explanation
        /// </summary>
        /// <param name="explanationId">id of explanation to change status</param>
        /// <param name="statusName">name of status to change</param>
        /// <returns>true/false</returns>
        bool ChangeStatus(int explanationId, string status, string delegateId,out string error);

        /// <summary>
        /// Add new explanation request
        /// </summary>
        /// <param name="explanation">ExplanationRequest object</param>
        /// <returns>true/false</returns>
        bool Add(ExplanationRequest explanation, string OTCheckIn, string OTCheckOut);

        /// <summary>
        /// Get list after sort if column = CreatedBy
        /// </summary>
        /// <param name="query">list explanations</param>
        /// <param name="column">name of column to sort</param>
        /// <param name="isDesc">true: sort by desc, else: sort by asc</param>
        /// <returns>explanations list</returns>
        IEnumerable<ExplanationRequest> GetListBySort(IEnumerable<ExplanationRequest> query, bool isDesc);

        /// <summary>
        /// Get list creator who created explanation
        /// </summary>
        /// <param name="userId">id of user logged</param>
        /// <param name="groupId">groudId of user logged</param>
        /// <returns>list AppUser object</returns>
        IEnumerable<AppUser> GetListCreator(string userId, string groupId);
        FingerTimeSheet GetFingerTimeSheetByExplanationID(int ExplantionID);
        bool ChangeStatusMulti(string[] explanationId, string statusName, string delegateId,out string error);
        IEnumerable<AbnormalCase> GetAbnormalById(int id);
        IEnumerable<ExplanationRequestChartModel> ExRequestChart(int groupID);
        IEnumerable<ExplanationRequestChartModel> ExRequestChartByUser(string userID);
        int ExRequestByUser(string userID);
        Entitleday_AppUser GetEntitleDayByUserID(string userID);
        void Save();
        /// Get List explanation by user
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="group"></param>
        /// <returns>explanation request list</returns>
        IEnumerable<ExplanationRequest> GetListExplanationByUser(string userID, string groupId);
        void ChangeStatusExplanationDelegateDefault(string[] explanationId, string statusName, string delegateId, DateTime startDate, DateTime endDate);
        void AddDelegateDefault(string groupId, int requestId);
        void CheckDataDelegationExplanationRequest(int groupId, int explanationrequestId);
        void CheckDataDelegationAllExplanationRequest(int groupId, int explanationrequestId, int groupUserDelegateId);

        /// <summary>
        /// Get explanations list for super admin after sort, filter
        /// </summary>
        /// <param name="column">name of column to sort</param>
        /// <param name="isDesc">parameter to check to sort by asc or desc</param>
        /// <param name="page">current page(use in paging)</param>
        /// <param name="pageSize">number of page showing(use in paging)</param>
        /// <param name="filter">list conditions to filter</param>
        /// <returns> explanations list </returns>
        IEnumerable<ExplanationRequest> GetExplanationsListForSuperAdmin(string column, bool isDesc, int page, int pageSize, FilterExplanationViewModel filter);

        /// <summary>
        /// Get list original explanation request without sort, filter for role is superadmin
        /// </summary>
        /// <returns> explanations list </returns>
        IEnumerable<ExplanationRequest> GetListOriginForSuperAdmin();
    }
    public class ExplanationRequestService : IExplanationRequestService
    {
        private IExplanationRequestRepository _explanationRequestRepository;
        private IFingerTimeSheetRepository _timeSheetRepository;
        private IStatusRequestRepository _statusRequestRepository;
        private IRequestRepository _requestRepository;
        private IEntitleDayAppUserRepository _entitleDayAppUserRepository;
        private IUnitOfWork _unitOfWork;
        private IGroupRepository _groupRepository;
        private IConfigDelegationService _configDelegationService;
        private int totalEntries = 0;       
        public ExplanationRequestService(IExplanationRequestRepository explanationRequestRepository, IFingerTimeSheetRepository timeSheetRepository, IStatusRequestRepository statusRequestRepository, IRequestRepository requestRepository, IEntitleDayAppUserRepository entitleDayAppUserRepository, IUnitOfWork unitOfWork, IGroupRepository groupRepository, IConfigDelegationService configDelegationService)
        {
            this._entitleDayAppUserRepository = entitleDayAppUserRepository;
            this._requestRepository = requestRepository;
            this._explanationRequestRepository = explanationRequestRepository;
            this._timeSheetRepository = timeSheetRepository;
            this._statusRequestRepository = statusRequestRepository;
            this._unitOfWork = unitOfWork;
            this._groupRepository = groupRepository;
            this._configDelegationService = configDelegationService;
        }

        /// <summary>
        /// Get list original explanation request without sort, filter
        /// </summary>
        /// <param name="userID">id of user logged</param>
        /// <param name="groupId">groupId of user logged</param>
        /// <returns> explanation request list </returns>
        public IEnumerable<ExplanationRequest> GetListOrigin(string userId, string groupId)
        {
            if (_explanationRequestRepository.IsReadAll(userId, CommonConstants.FunctionExplanationRequest))
            {
                return _explanationRequestRepository.GetMulti(x => x.FingerTimeSheet.FingerMachineUsers.AppUser.GroupId.ToString().Equals(groupId),
                    new string[] {
                    CommonConstants.AppUserCreatedBy,
                    CommonConstants.AppUserUpdatedBy,
                    CommonConstants.ReceiverUser,
                    CommonConstants.DelegateUser,
                    CommonConstants.FingerTimeSheet,
                    CommonConstants.FingerMachineUser,
                    CommonConstants.TimeSheetAppUser,
                    CommonConstants.TimeSheetAppUserGroup,
                    CommonConstants.StatusRequest}).OrderByDescending(x => x.CreatedDate);
            }
            return _explanationRequestRepository.GetMulti(x => x.FingerTimeSheet.FingerMachineUsers.AppUser.Id.Equals(userId), new string[] {
                CommonConstants.AppUserCreatedBy,
                CommonConstants.AppUserUpdatedBy,
                CommonConstants.ReceiverUser,
                CommonConstants.DelegateUser,
                CommonConstants.FingerTimeSheet,
                CommonConstants.FingerMachineUser,
                CommonConstants.TimeSheetAppUser,
                CommonConstants.TimeSheetAppUserGroup,
                CommonConstants.StatusRequest}).OrderByDescending(x => x.CreatedDate);
        }

        /// <summary>
        /// Get explanation detail by id
        /// </summary>
        /// <param name="id">id of explanation</param>
        /// <returns> ExplanationRequest </returns>
        public ExplanationRequest GetExplanationDetail(int id)
        {
            return _explanationRequestRepository.GetSingleByCondition(x => x.ID.Equals(id), new string[] {
                CommonConstants.AppUserCreatedBy,
                CommonConstants.AppUserUpdatedBy,
                CommonConstants.ReceiverUser,
                CommonConstants.DelegateUser,
                CommonConstants.FingerTimeSheet,
                CommonConstants.FingerMachineUser,
                CommonConstants.TimeSheetAppUser,
                CommonConstants.TimeSheetAppUserGroup,
                CommonConstants.StatusRequest});
        }
        
        /// <summary>
        /// Get explanations list after sort, filter
        /// </summary>
        /// <param name="userID">id of user logged</param>
        /// <param name="groupId">groupId of user logged</param>
        /// <param name="column">name of column to sort</param>
        /// <param name="isDesc">parameter to check to sort by asc or desc</param>
        /// <param name="page">current page(use in paging)</param>
        /// <param name="pageSize">number of page showing(use in paging)</param>
        /// <param name="filter">list conditions to filter</param>
        /// <returns> explanations list </returns>
        public IEnumerable<ExplanationRequest> GetExplanationsList(string userId, string groupId, string column,
            bool isDesc, int page, int pageSize, FilterExplanationViewModel filter)
        {
            var query = GetListOrigin(userId, groupId);

            // get list after filter if filter list != null
            if (filter != null)
            {
                query = GetListByFilter(query, filter);
            }
            totalEntries = query.Count();

            // get list after sort if column = CreatedBy
            if (column == CommonConstants.OrderByCreatedBy)
            {
                return GetListBySort(query, isDesc).Skip((page - 1) * pageSize).Take(pageSize);
            }
            return string.IsNullOrEmpty(column) ? query.Skip((page - 1) * pageSize).Take(pageSize) : query.OrderByField(column, isDesc).Skip((page - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Get list after sort if column = CreatedBy
        /// </summary>
        /// <param name="query">list explanations</param>
        /// <param name="column">name of column to sort</param>
        /// <param name="isDesc">true: sort by desc, else: sort by asc</param>
        /// <returns>explanations list</returns>
        public IEnumerable<ExplanationRequest> GetListBySort(IEnumerable<ExplanationRequest> query, bool isDesc)
        {
            if (isDesc == false)
            {
                return query.OrderBy(x => x.AppUserCreatedBy.FullName.Substring(x.AppUserCreatedBy.FullName.LastIndexOf(" ")));
            }
            return query.OrderByDescending(x => x.AppUserCreatedBy.FullName.Substring(x.AppUserCreatedBy.FullName.LastIndexOf(" ")));

        }

        /// <summary>
        /// Get total explanation elements
        /// </summary>
        /// <returns> int </returns>
        public int GetTotalEntries()
        {
            return totalEntries;
        }

        /// <summary>
        /// Get explanations list after filter
        /// </summary>
        /// <param name="query">list explanations</param>
        /// <param name="filter">list filter conditions</param>
        /// <returns> explanations list </returns>
        public IEnumerable<ExplanationRequest> GetListByFilter(IEnumerable<ExplanationRequest> query, FilterExplanationViewModel filter)
        {
            // filter by creators
            if (filter.Creators.Count() != 0)
            {
                query = query.Where(x => filter.Creators.Contains(x.AppUserCreatedBy.Id.ToString()));
            }else if (filter.ChosenCreatorFilterSuperAdmin.Count() != 0)
            {
                query = query.Where(x => filter.ChosenCreatorFilterSuperAdmin.Contains(x.AppUserCreatedBy.Id.ToString()));
            }
            // filter by explanation status
            if (filter.StatusRequest.Count() != 0)
            {
                query = query.Where(x => filter.StatusRequest.Contains(x.StatusRequestId.ToString()));
            }
            // filter by explanation reason
            if (filter.ReasonRequest.Count() != 0)
            {
                // call function to get list timesheet id by reason id
                List<string> AbnormalList = _explanationRequestRepository.GetAbnormalByReasonId(filter.ReasonRequest);

                // get explanations by timesheet id
                query = query.Where(x => AbnormalList.Contains(x.TimeSheetId.ToString()));
            }
            //filter by created date
            if (!string.IsNullOrEmpty(filter.FromDate) && !string.IsNullOrEmpty(filter.ToDate))
            {
                query = query.Where(x => (x.CreatedDate.Value.Date >= DateTime.ParseExact(filter.FromDate, CommonConstants.FORMAT_DATE_MMDDYYYY, CultureInfo.InvariantCulture))
                && (x.CreatedDate.Value.Date <= DateTime.ParseExact(filter.ToDate, CommonConstants.FORMAT_DATE_MMDDYYYY, CultureInfo.InvariantCulture)));
            }
            return query;
        }

        /// <summary>
        /// Get list original explanation request without sort, filter
        /// </summary>
        /// <param name="userID">ID of username login</param>
        /// <param name="groupId">group of username</param>
        /// <returns>explanation request list</returns>
        public IEnumerable<ExplanationRequest> GetListExplanationAssigned(string userID, string groupId)
        {
            if (_explanationRequestRepository.IsReadAll(userID, CommonConstants.FunctionDelegationMemberRequest))
            {
                return _explanationRequestRepository.GetMulti(x => (x.FingerTimeSheet.FingerMachineUsers.AppUser.GroupId.ToString().Equals(groupId) && x.DelegateId != null),
                    new string[] {
                    CommonConstants.FingerTimeSheet,
                    CommonConstants.FingerMachineUser,
                    CommonConstants.TimeSheetAppUser,
                    CommonConstants.TimeSheetAppUserGroup,
                    CommonConstants.StatusRequest,
                    CommonConstants.ReceiverUser,
                    CommonConstants.DelegateUser,
                    CommonConstants.AppUserUpdatedBy}).OrderByDescending(x => x.CreatedDate);
            }
            return _explanationRequestRepository.GetMulti(x => (x.DelegateId.Equals(userID)), new string[] {
                CommonConstants.FingerTimeSheet,
                CommonConstants.FingerMachineUser,
                CommonConstants.TimeSheetAppUser,
                CommonConstants.TimeSheetAppUserGroup,
                CommonConstants.StatusRequest,
                CommonConstants.ReceiverUser,
                CommonConstants.DelegateUser,
                CommonConstants.AppUserUpdatedBy}).OrderByDescending(x => x.CreatedDate);
        }

        /// <summary>
        /// Get explanations list after sort, filter
        /// </summary>
        /// <param name="userID">Id of username login</param>
        /// <param name="groupId">groupd of username</param>
        /// <param name="column">name column want to sort</param>
        /// <param name="isDesc">parameter to check to sort by asc or desc</param>
        /// <param name="page">number page current</param>
        /// 
        /// <param name="pageSize"></param>
        /// <param name="filter"></param>
        /// <returns>explanations list</returns>
        public IEnumerable<ExplanationRequest> GetExplanationsRequestAssignedList(string userID, string groupId, string column,
            bool isDesc, int page, int pageSize, FilterDelegationAssignedModel filter)
        {
            var query = GetListExplanationAssigned(userID, groupId);
            if (filter != null)
            {
                query = GetListRequestAssignedFilter(query, filter);
            }
            return query.OrderByField(column, isDesc).Skip((page - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Get Explanation request after filter
        /// </summary>
        /// <param name="query">list explanation is assigned</param>
        /// <param name="filter">parameters want to filter</param>
        /// <returns>list filter after</returns>
        public IEnumerable<ExplanationRequest> GetListRequestAssignedFilter(IEnumerable<ExplanationRequest> query, FilterDelegationAssignedModel filter)
        {
            // filter by explanation status
            if (filter.usernameAssigned.Length > 0)
            {
                query = query.Where(x => filter.usernameAssigned.Contains(x.CreatedBy.ToString()));
            }
            if (filter.StatusRequestType.Count() != 0)
            {
                query = query.Where(x => filter.StatusRequestType.Contains(x.StatusRequestId.ToString()));
            }
            if (!string.IsNullOrEmpty(filter.startDate) && !string.IsNullOrEmpty(filter.endDate))
            {
                DateTime dtStart = DateTime.ParseExact(filter.startDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture);
                DateTime dtEnd = DateTime.ParseExact(filter.endDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture);
                query = query.Where(x => (x.CreatedDate >= dtStart) && (x.CreatedDate <= dtEnd));
            }
            return query;
        }

        /// <summary>
        /// Change status of an explanation
        /// </summary>
        /// <param name="explanationId">id of explanation to change status</param>
        /// <param name="statusName">name of status to change</param>
        /// <returns>true/false</returns>
        public bool ChangeStatus(int explanationId, string statusName, string delegateId, out string error)
        {
            error = "";
            try
            {
                var explanation = GetExplanationDetail(explanationId);
                var status = _statusRequestRepository.GetMulti(x => x.Name.Contains(statusName)).FirstOrDefault();
                explanation.StatusRequestId = status.ID;
                if (statusName.Equals(CommonConstants.StatusApproved) || statusName.Equals(CommonConstants.StatusRejected))
                {
                    explanation.UpdatedBy = delegateId;
                    if (statusName.Equals(CommonConstants.StatusApproved))
                    {
                        //update after aprroved
                        if (!UpdateAfterApprove(explanation,out error))
                        {
                            return false;
                        }
                    }
                    else if (statusName.Equals(CommonConstants.StatusRejected))
                    {
                        //update after rejected
                        if (!UpdateAfterReject(explanation,out error))
                        {
                            return false;
                        }
                    }
                }
                if (statusName.Equals(CommonConstants.StatusDelegation))
                {
                    explanation.DelegateId = delegateId;
                }
                explanation.UpdatedDate = DateTime.Now;
                _explanationRequestRepository.Update(explanation);
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }
        public bool UpdateAfterApprove(ExplanationRequest explanationRequest,out string error)
        {
            error = CommonConstants.StringEmpty;
            var abnormal= _explanationRequestRepository.GetAbnormalById(explanationRequest.TimeSheetId);
            if (abnormal.Count() > 0)
            {
                foreach (var item in abnormal)
                {
                    switch (item.AbnormalReason.Name)
                    {
                        case StringConstants.UnauthorizedLateComing:
                            // add allowance to timesheet
                            explanationRequest.FingerTimeSheet.MinusAllowance = null;
                            _timeSheetRepository.Update(explanationRequest.FingerTimeSheet);
                            break;
                        case StringConstants.UnauthorizedEarlyLeaving:
                            // add allowance to timesheet
                            explanationRequest.FingerTimeSheet.MinusAllowance = null;
                            _timeSheetRepository.Update(explanationRequest.FingerTimeSheet);
                            break;
                        case StringConstants.UnusedAuthorizedLeave:
                            AddEntitleDayAfterExplanation(explanationRequest);
                            break;
                        case StringConstants.UnauthorizedLeave:
                            if (!UpdateAfterApproveUnauthorizedLeave(explanationRequest, out error))
                                return false;
                            break;
                        default:
                            break;
                    }
                }
            }
            return true;
        }

        private bool UpdateAfterApproveUnauthorizedLeave(ExplanationRequest explanationRequest,out string error)
        {
            error = "";
            var lstRequest = _requestRepository.GetMulti(x => x.StatusRequest.Name == CommonConstants.StatusApproved
                            && x.RequestType.Name.Contains("Leave") && x.StartDate <= explanationRequest.FingerTimeSheet.DayOfCheck
                            && x.EndDate >= explanationRequest.FingerTimeSheet.DayOfCheck && explanationRequest.CreatedBy == x.UserId, new string[] { CommonConstants.RequestType });
            var entitleDay = _entitleDayAppUserRepository.GetSingleByCondition(x => x.UserId == explanationRequest.AppUserCreatedBy.Id);
            if (explanationRequest.Actual == StringConstants.Leave)
            {
                //sub entitle day
                foreach (var request in lstRequest)
                {
                    if (request.EntitleDayId != 1)
                        continue;
                    switch (request.RequestTypeId)
                    {
                        case 2:
                            if (explanationRequest.FingerTimeSheet.Absent == "V")
                            {
                                entitleDay.NumberDayOff = (entitleDay.MaxEntitleDayAppUser - entitleDay.NumberDayOff >= 0.5) ? entitleDay.NumberDayOff + (float)0.5 : entitleDay.NumberDayOff;
                                entitleDay.TemporaryMaxEntitleDay = (entitleDay.MaxEntitleDayAppUser - entitleDay.NumberDayOff >= 0.5) ? entitleDay.TemporaryMaxEntitleDay + (float)0.5 : entitleDay.TemporaryMaxEntitleDay;
                                _entitleDayAppUserRepository.Update(entitleDay);
                            }
                            if (explanationRequest.FingerTimeSheet.Absent == "VC")
                            {
                                entitleDay.NumberDayOff = (entitleDay.MaxEntitleDayAppUser - entitleDay.NumberDayOff >= 0.5) ? entitleDay.NumberDayOff + (float)0.5 : entitleDay.NumberDayOff;
                                entitleDay.TemporaryMaxEntitleDay = (entitleDay.MaxEntitleDayAppUser - entitleDay.NumberDayOff >= 0.5) ? entitleDay.TemporaryMaxEntitleDay + (float)0.5 : entitleDay.TemporaryMaxEntitleDay;
                                _entitleDayAppUserRepository.Update(entitleDay);
                            }
                            break;
                        case 3:
                            if(explanationRequest.FingerTimeSheet.Absent == "V")
                            {
                                entitleDay.NumberDayOff = (entitleDay.MaxEntitleDayAppUser - entitleDay.NumberDayOff >= 0.5) ? entitleDay.NumberDayOff + (float)0.5 : entitleDay.NumberDayOff;
                                entitleDay.TemporaryMaxEntitleDay = (entitleDay.MaxEntitleDayAppUser - entitleDay.NumberDayOff >= 0.5) ? entitleDay.TemporaryMaxEntitleDay + (float)0.5 : entitleDay.TemporaryMaxEntitleDay;
                                _entitleDayAppUserRepository.Update(entitleDay);
                            }
                            if (explanationRequest.FingerTimeSheet.Absent == "VS")
                            {
                                entitleDay.NumberDayOff = (entitleDay.MaxEntitleDayAppUser - entitleDay.NumberDayOff >= 0.5) ? entitleDay.NumberDayOff + (float)0.5 : entitleDay.NumberDayOff;
                                entitleDay.TemporaryMaxEntitleDay = (entitleDay.MaxEntitleDayAppUser - entitleDay.NumberDayOff >= 0.5) ? entitleDay.TemporaryMaxEntitleDay + (float)0.5 : entitleDay.TemporaryMaxEntitleDay;
                                _entitleDayAppUserRepository.Update(entitleDay);
                            }
                            break;
                    }
                }
                if (lstRequest.Count() == 0)
                {
                    switch (explanationRequest.FingerTimeSheet.Absent)
                    {
                        case "V":
                            entitleDay.NumberDayOff = (entitleDay.MaxEntitleDayAppUser - entitleDay.NumberDayOff >= 1) ? entitleDay.NumberDayOff + 1 : entitleDay.NumberDayOff;
                            entitleDay.TemporaryMaxEntitleDay = (entitleDay.MaxEntitleDayAppUser - entitleDay.NumberDayOff >= 1) ? entitleDay.TemporaryMaxEntitleDay + 1 : entitleDay.TemporaryMaxEntitleDay;
                            _entitleDayAppUserRepository.Update(entitleDay);
                            break;
                        case "VS":
                            entitleDay.NumberDayOff = (entitleDay.MaxEntitleDayAppUser - entitleDay.NumberDayOff >= 0.5) ? entitleDay.NumberDayOff + (float)0.5 : entitleDay.NumberDayOff;
                            entitleDay.TemporaryMaxEntitleDay = (entitleDay.MaxEntitleDayAppUser - entitleDay.NumberDayOff >= 0.5) ? entitleDay.TemporaryMaxEntitleDay + (float)0.5 : entitleDay.TemporaryMaxEntitleDay;
                            _entitleDayAppUserRepository.Update(entitleDay);
                            break;
                        case "VC":
                            entitleDay.NumberDayOff = (entitleDay.MaxEntitleDayAppUser - entitleDay.NumberDayOff >= 0.5) ? entitleDay.NumberDayOff + (float)0.5 : entitleDay.NumberDayOff;
                            entitleDay.TemporaryMaxEntitleDay = (entitleDay.MaxEntitleDayAppUser - entitleDay.NumberDayOff >= 0.5) ? entitleDay.TemporaryMaxEntitleDay + (float)0.5 : entitleDay.TemporaryMaxEntitleDay;
                            _entitleDayAppUserRepository.Update(entitleDay);
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (explanationRequest.Actual == StringConstants.ForgetToCheck)
            {
                foreach (var request in lstRequest)
                {
                    if (request.EntitleDayId != 1)
                        continue;
                    switch (request.RequestTypeId)
                    {
                        case 2:
                            if (explanationRequest.FingerTimeSheet.Absent == "V")
                            {
                                explanationRequest.FingerTimeSheet.NumOfWorkingDay += 0.5;
                                _timeSheetRepository.Update(explanationRequest.FingerTimeSheet);
                            }else if(explanationRequest.FingerTimeSheet.Absent == "VC")
                            {
                                explanationRequest.FingerTimeSheet.NumOfWorkingDay += 0.5;
                                _timeSheetRepository.Update(explanationRequest.FingerTimeSheet);
                                //+0.5E
                                //entitleDay.NumberDayOff = entitleDay.NumberDayOff >= 0.5 ? entitleDay.NumberDayOff - (float)0.5 : entitleDay.NumberDayOff;
                                //entitleDay.TemporaryMaxEntitleDay = entitleDay.NumberDayOff >= 0.5 ? entitleDay.TemporaryMaxEntitleDay - (float)0.5 : entitleDay.TemporaryMaxEntitleDay;
                                //_entitleDayAppUserRepository.Update(entitleDay);
                            }
                            break;
                        case 3:
                            if (explanationRequest.FingerTimeSheet.Absent == "V")
                            {
                                explanationRequest.FingerTimeSheet.NumOfWorkingDay += 0.5;
                                _timeSheetRepository.Update(explanationRequest.FingerTimeSheet);
                            }else if(explanationRequest.FingerTimeSheet.Absent == "VS")
                            {
                                explanationRequest.FingerTimeSheet.NumOfWorkingDay += 0.5;
                                _timeSheetRepository.Update(explanationRequest.FingerTimeSheet);
                                //+0.5E
                                //entitleDay.NumberDayOff = entitleDay.NumberDayOff >= 0.5 ? entitleDay.NumberDayOff - (float)0.5 : entitleDay.NumberDayOff;
                                //entitleDay.TemporaryMaxEntitleDay = entitleDay.NumberDayOff >= 0.5 ? entitleDay.TemporaryMaxEntitleDay - (float)0.5 : entitleDay.TemporaryMaxEntitleDay;
                                //_entitleDayAppUserRepository.Update(entitleDay);
                            }
                            break;
                    }
                }
                if (lstRequest.Count() == 0)
                {
                    switch (explanationRequest.FingerTimeSheet.Absent)
                    {
                        case "V":
                            explanationRequest.FingerTimeSheet.NumOfWorkingDay += 1;
                            _timeSheetRepository.Update(explanationRequest.FingerTimeSheet);
                            break;
                        case "VS":
                            explanationRequest.FingerTimeSheet.NumOfWorkingDay += 0.5;
                            _timeSheetRepository.Update(explanationRequest.FingerTimeSheet);
                            break;
                        case "VC":
                            explanationRequest.FingerTimeSheet.NumOfWorkingDay += 0.5;
                            _timeSheetRepository.Update(explanationRequest.FingerTimeSheet);
                            break;
                        default:
                            break;
                    }
                }
            }
            return true;
        }

        private void AddEntitleDayAfterExplanation(ExplanationRequest explanationRequest)
        {
            var lstRequest = _requestRepository.GetMulti(x => x.StatusRequest.Name == CommonConstants.StatusApproved
                             && x.RequestType.Name.Contains("Leave") && x.StartDate <= explanationRequest.FingerTimeSheet.DayOfCheck
                             && x.EndDate >= explanationRequest.FingerTimeSheet.DayOfCheck && explanationRequest.CreatedBy == x.UserId, new string[] { CommonConstants.RequestType });
            foreach (var request in lstRequest)
            {
                if (request.EntitleDayId != 1)
                    continue;
                var entitleDay = _entitleDayAppUserRepository.GetSingleByCondition(x => x.UserId == explanationRequest.AppUserCreatedBy.Id);
                switch (request.RequestTypeId)
                {
                    case 1:
                        if (string.IsNullOrEmpty(explanationRequest.FingerTimeSheet.Absent))
                        {
                            if (entitleDay.NumberDayOff >= 1)
                            {
                                entitleDay.NumberDayOff = entitleDay.NumberDayOff - 1;
                                entitleDay.TemporaryMaxEntitleDay = entitleDay.TemporaryMaxEntitleDay - 1;
                                _entitleDayAppUserRepository.Update(entitleDay);
                            }
                        }
                        else if(explanationRequest.FingerTimeSheet.Absent.Equals("VS"))
                        {
                            if (entitleDay.NumberDayOff >= 0.5)
                            {
                                entitleDay.NumberDayOff = entitleDay.NumberDayOff - (float)0.5;
                                entitleDay.TemporaryMaxEntitleDay = entitleDay.TemporaryMaxEntitleDay - (float)0.5;
                                _entitleDayAppUserRepository.Update(entitleDay);
                            }
                        }
                        else if (explanationRequest.FingerTimeSheet.Absent.Equals("VC"))
                        {
                            if (entitleDay.NumberDayOff >= 0.5)
                            {
                                entitleDay.NumberDayOff = entitleDay.NumberDayOff - (float)0.5;
                                entitleDay.TemporaryMaxEntitleDay = entitleDay.TemporaryMaxEntitleDay - (float)0.5;
                                _entitleDayAppUserRepository.Update(entitleDay);
                            }
                        }
                            break;
                    case 2:
                        if (string.IsNullOrEmpty(explanationRequest.FingerTimeSheet.Absent)||explanationRequest.FingerTimeSheet.Absent.Equals("VC"))
                        {
                            if (entitleDay.NumberDayOff >= 0.5)
                            {
                                entitleDay.NumberDayOff = entitleDay.NumberDayOff - (float)0.5;
                                entitleDay.TemporaryMaxEntitleDay = entitleDay.TemporaryMaxEntitleDay - (float)0.5;
                                _entitleDayAppUserRepository.Update(entitleDay);
                            }
                        }
                        break;
                    case 3:
                        if (string.IsNullOrEmpty(explanationRequest.FingerTimeSheet.Absent) || explanationRequest.FingerTimeSheet.Absent.Equals("VS"))
                        {
                            if (entitleDay.NumberDayOff >= 0.5)
                            {
                                entitleDay.NumberDayOff = entitleDay.NumberDayOff - (float)0.5;
                                entitleDay.TemporaryMaxEntitleDay = entitleDay.TemporaryMaxEntitleDay - (float)0.5;
                                _entitleDayAppUserRepository.Update(entitleDay);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        private void SubEntitleDayAfterExplanation(ExplanationRequest explanationRequest)
        {
            var lstRequest = _requestRepository.GetMulti(x => x.StatusRequest.Name == CommonConstants.StatusApproved
                             && x.RequestType.Name.Contains(StringConstants.Leave) && x.StartDate <= explanationRequest.FingerTimeSheet.DayOfCheck
                             && x.EndDate >= explanationRequest.FingerTimeSheet.DayOfCheck && explanationRequest.CreatedBy==x.UserId, new string[] { CommonConstants.RequestType }).ToList();
            foreach (var request in lstRequest)
            {
                if (request.EntitleDayId != 1)
                    continue;
                var entitleDay = _entitleDayAppUserRepository.GetSingleByCondition(x => x.UserId == explanationRequest.AppUserCreatedBy.Id);
                switch (request.RequestTypeId)
                {
                    case 1:
                        if (string.IsNullOrEmpty(explanationRequest.FingerTimeSheet.Absent))
                        {
                            if (entitleDay.MaxEntitleDayAppUser - entitleDay.NumberDayOff >= 1)
                            {
                                entitleDay.NumberDayOff = entitleDay.NumberDayOff + 1;
                                entitleDay.TemporaryMaxEntitleDay = entitleDay.TemporaryMaxEntitleDay + 1;
                                _entitleDayAppUserRepository.Update(entitleDay);
                            }
                        }
                        else if (explanationRequest.FingerTimeSheet.Absent.Equals("VS"))
                        {
                            if (entitleDay.MaxEntitleDayAppUser - entitleDay.NumberDayOff >= 0.5)
                            {
                                entitleDay.NumberDayOff = entitleDay.NumberDayOff + (float)0.5;
                                entitleDay.TemporaryMaxEntitleDay = entitleDay.TemporaryMaxEntitleDay + (float)0.5;
                                _entitleDayAppUserRepository.Update(entitleDay);
                            }
                        }
                        else if (explanationRequest.FingerTimeSheet.Absent.Equals("VC"))
                        {
                            if (entitleDay.MaxEntitleDayAppUser - entitleDay.NumberDayOff >= 0.5)
                            {
                                entitleDay.NumberDayOff = entitleDay.NumberDayOff + (float)0.5;
                                entitleDay.TemporaryMaxEntitleDay = entitleDay.TemporaryMaxEntitleDay + (float)0.5;
                                _entitleDayAppUserRepository.Update(entitleDay);
                            }
                        }
                        break;
                    case 2:
                        if (string.IsNullOrEmpty(explanationRequest.FingerTimeSheet.Absent) || explanationRequest.FingerTimeSheet.Absent.Equals("VC"))
                        {
                            if (entitleDay.MaxEntitleDayAppUser - entitleDay.NumberDayOff >= 0.5)
                            {
                                entitleDay.NumberDayOff = entitleDay.NumberDayOff + (float)0.5;
                                entitleDay.TemporaryMaxEntitleDay = entitleDay.TemporaryMaxEntitleDay + (float)0.5;
                                _entitleDayAppUserRepository.Update(entitleDay);
                            }
                        }
                        break;
                    case 3:
                        if (string.IsNullOrEmpty(explanationRequest.FingerTimeSheet.Absent) || explanationRequest.FingerTimeSheet.Absent.Equals("VS"))
                        {
                            if (entitleDay.MaxEntitleDayAppUser - entitleDay.NumberDayOff >= 0.5)
                            {
                                entitleDay.NumberDayOff = entitleDay.NumberDayOff + (float)0.5;
                                entitleDay.TemporaryMaxEntitleDay = entitleDay.TemporaryMaxEntitleDay + (float)0.5;
                                _entitleDayAppUserRepository.Update(entitleDay);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        public bool UpdateAfterReject(ExplanationRequest explanationRequest,out string error)
        {
            error = CommonConstants.StringEmpty;
            var abnormal = _explanationRequestRepository.GetAbnormalById(explanationRequest.TimeSheetId);
            if (abnormal.Count() > 0 && explanationRequest.StatusRequest.Name== CommonConstants.StatusApproved)
            {
                foreach (var item in abnormal)
                {
                    switch (item.AbnormalReason.Name)
                    {
                        case StringConstants.UnauthorizedLateComing:
                            // sub allowance to timesheet
                            explanationRequest.FingerTimeSheet.MinusAllowance = StringConstants.MinusAllowance_40Percent;
                            _timeSheetRepository.Update(explanationRequest.FingerTimeSheet);
                            break;
                        case StringConstants.UnauthorizedEarlyLeaving:
                            // sub allowance to timesheet
                            explanationRequest.FingerTimeSheet.MinusAllowance = StringConstants.MinusAllowance_40Percent;
                            _timeSheetRepository.Update(explanationRequest.FingerTimeSheet);
                            break;
                        case StringConstants.UnusedAuthorizedLeave:
                            SubEntitleDayAfterExplanation(explanationRequest);
                            break;
                        case StringConstants.UnauthorizedLeave:
                            if (!UpdateAfterRejectUnauthorizedLeave(explanationRequest,out error))
                                return false;
                            break;
                        default:
                            break;
                    }
                }
            }
            return true;
        }

        private bool UpdateAfterRejectUnauthorizedLeave(ExplanationRequest explanationRequest, out string error)
        {
            error = "";
            var lstRequest = _requestRepository.GetMulti(x => x.StatusRequest.Name == CommonConstants.StatusApproved
                            && x.RequestType.Name.Contains("Leave") && x.StartDate <= explanationRequest.FingerTimeSheet.DayOfCheck
                            && x.EndDate >= explanationRequest.FingerTimeSheet.DayOfCheck && explanationRequest.CreatedBy == x.UserId, new string[] { CommonConstants.RequestType });
            var entitleDay = _entitleDayAppUserRepository.GetSingleByCondition(x => x.UserId == explanationRequest.AppUserCreatedBy.Id);
            if (explanationRequest.Actual == StringConstants.Leave)
            {
                //add entitle day
                foreach (var request in lstRequest)
                {
                    if (request.EntitleDayId != 1)
                        continue;
                    switch (request.RequestTypeId)
                    {
                        case 2:
                            if (explanationRequest.FingerTimeSheet.Absent == "V")
                            {
                                entitleDay.NumberDayOff = entitleDay.NumberDayOff >= 0.5 ? entitleDay.NumberDayOff - (float)0.5 : entitleDay.NumberDayOff;
                                entitleDay.TemporaryMaxEntitleDay = entitleDay.NumberDayOff >= 0.5 ? entitleDay.TemporaryMaxEntitleDay - (float)0.5 : entitleDay.TemporaryMaxEntitleDay;
                                _entitleDayAppUserRepository.Update(entitleDay);
                            }
                            if (explanationRequest.FingerTimeSheet.Absent == "VC")
                            {
                                entitleDay.NumberDayOff = entitleDay.NumberDayOff >= 0.5 ? entitleDay.NumberDayOff - (float)0.5 : entitleDay.NumberDayOff;
                                entitleDay.TemporaryMaxEntitleDay = entitleDay.NumberDayOff >= 0.5 ? entitleDay.TemporaryMaxEntitleDay - (float)0.5 : entitleDay.TemporaryMaxEntitleDay;
                                _entitleDayAppUserRepository.Update(entitleDay);
                            }
                            break;
                        case 3:
                            if (explanationRequest.FingerTimeSheet.Absent == "V")
                            {
                                entitleDay.NumberDayOff = entitleDay.NumberDayOff >= 0.5 ? entitleDay.NumberDayOff - (float)0.5 : entitleDay.NumberDayOff;
                                entitleDay.TemporaryMaxEntitleDay = entitleDay.NumberDayOff >= 0.5 ? entitleDay.TemporaryMaxEntitleDay - (float)0.5 : entitleDay.TemporaryMaxEntitleDay;
                                _entitleDayAppUserRepository.Update(entitleDay);
                            }
                            if (explanationRequest.FingerTimeSheet.Absent == "VS")
                            {
                                entitleDay.NumberDayOff = entitleDay.NumberDayOff >= 0.5 ? entitleDay.NumberDayOff - (float)0.5 : entitleDay.NumberDayOff;
                                entitleDay.TemporaryMaxEntitleDay = entitleDay.NumberDayOff >= 0.5 ? entitleDay.TemporaryMaxEntitleDay - (float)0.5 : entitleDay.TemporaryMaxEntitleDay;
                                _entitleDayAppUserRepository.Update(entitleDay);
                            }
                            break;
                    }
                }
                if (lstRequest.Count() == 0)
                {
                    switch (explanationRequest.FingerTimeSheet.Absent)
                    {
                        case "V":
                            entitleDay.NumberDayOff = entitleDay.NumberDayOff >= 1 ? entitleDay.NumberDayOff - 1 : entitleDay.NumberDayOff;
                            entitleDay.TemporaryMaxEntitleDay = entitleDay.NumberDayOff >= 1 ? entitleDay.TemporaryMaxEntitleDay - 1 : entitleDay.TemporaryMaxEntitleDay;
                            _entitleDayAppUserRepository.Update(entitleDay);
                            break;
                        case "VS":
                            entitleDay.NumberDayOff = entitleDay.NumberDayOff >= 0.5 ? entitleDay.NumberDayOff - (float)0.5 : entitleDay.NumberDayOff;
                            entitleDay.TemporaryMaxEntitleDay = entitleDay.NumberDayOff >= 0.5 ? entitleDay.TemporaryMaxEntitleDay - (float)0.5 : entitleDay.TemporaryMaxEntitleDay;
                            _entitleDayAppUserRepository.Update(entitleDay);
                            break;
                        case "VC":
                            entitleDay.NumberDayOff = entitleDay.NumberDayOff >= 0.5 ? entitleDay.NumberDayOff - (float)0.5 : entitleDay.NumberDayOff;
                            entitleDay.TemporaryMaxEntitleDay = entitleDay.NumberDayOff >= 0.5 ? entitleDay.TemporaryMaxEntitleDay - (float)0.5 : entitleDay.TemporaryMaxEntitleDay;
                            _entitleDayAppUserRepository.Update(entitleDay);
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (explanationRequest.Actual == StringConstants.ForgetToCheck)
            {
                foreach (var request in lstRequest)
                {
                    if (request.EntitleDayId != 1)
                        continue;
                    switch (request.RequestTypeId)
                    {
                        case 2:
                            if (explanationRequest.FingerTimeSheet.Absent == "V")
                            {
                                explanationRequest.FingerTimeSheet.NumOfWorkingDay -= 0.5;
                                _timeSheetRepository.Update(explanationRequest.FingerTimeSheet);
                            }
                            else if (explanationRequest.FingerTimeSheet.Absent == "VC")
                            {
                                explanationRequest.FingerTimeSheet.NumOfWorkingDay -= 0.5;
                                _timeSheetRepository.Update(explanationRequest.FingerTimeSheet);
                                ////-0.5E
                                //entitleDay.NumberDayOff = (entitleDay.MaxEntitleDayAppUser - entitleDay.NumberDayOff >= 0.5) ? entitleDay.NumberDayOff + (float)0.5 : entitleDay.NumberDayOff;
                                //entitleDay.TemporaryMaxEntitleDay = (entitleDay.MaxEntitleDayAppUser - entitleDay.NumberDayOff >= 0.5) ? entitleDay.TemporaryMaxEntitleDay + (float)0.5 : entitleDay.TemporaryMaxEntitleDay;
                                //_entitleDayAppUserRepository.Update(entitleDay);
                            }
                            break;
                        case 3:
                            if (explanationRequest.FingerTimeSheet.Absent == "V")
                            {
                                explanationRequest.FingerTimeSheet.NumOfWorkingDay -= 0.5;
                                _timeSheetRepository.Update(explanationRequest.FingerTimeSheet);
                            }
                            else if (explanationRequest.FingerTimeSheet.Absent == "VS")
                            {
                                explanationRequest.FingerTimeSheet.NumOfWorkingDay -= 0.5;
                                _timeSheetRepository.Update(explanationRequest.FingerTimeSheet);
                                //-0.5E
                                //entitleDay.NumberDayOff = (entitleDay.MaxEntitleDayAppUser - entitleDay.NumberDayOff >= 0.5) ? entitleDay.NumberDayOff + (float)0.5 : entitleDay.NumberDayOff;
                                //entitleDay.TemporaryMaxEntitleDay = (entitleDay.MaxEntitleDayAppUser - entitleDay.NumberDayOff >= 0.5) ? entitleDay.TemporaryMaxEntitleDay + (float)0.5 : entitleDay.TemporaryMaxEntitleDay;
                                //_entitleDayAppUserRepository.Update(entitleDay);
                            }
                            break;
                    }
                }
                if (lstRequest.Count() == 0)
                {
                    switch (explanationRequest.FingerTimeSheet.Absent)
                    {
                        case "V":
                            explanationRequest.FingerTimeSheet.NumOfWorkingDay -= 1;
                            _timeSheetRepository.Update(explanationRequest.FingerTimeSheet);
                            break;
                        case "VS":
                            explanationRequest.FingerTimeSheet.NumOfWorkingDay -= 0.5;
                            _timeSheetRepository.Update(explanationRequest.FingerTimeSheet);
                            break;
                        case "VC":
                            explanationRequest.FingerTimeSheet.NumOfWorkingDay -= 0.5;
                            _timeSheetRepository.Update(explanationRequest.FingerTimeSheet);
                            break;
                        default:
                            break;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Save change when update data in db
        /// </summary>
        public void Save()
        {
            _unitOfWork.Commit();
        }

        /// <summary>
        /// Add new explanation request
        /// </summary>
        /// <param name="explanation">ExplanationRequest object</param>
        /// <returns>true/false</returns>
        public bool Add(ExplanationRequest explanation, string OTCheckIn, string OTCheckOut)
        {
            try
            {
                _explanationRequestRepository.Add(explanation);
                Save();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        /// <summary>
        /// Get list creator who created explanation
        /// </summary>
        /// <param name="userId">id of user logged</param>
        /// <param name="groupId">groudId of user logged</param>
        /// <returns>list AppUser object</returns>
        public IEnumerable<AppUser> GetListCreator(string userId, string groupId)
        {
            return GetListOrigin(userId, groupId).Select(x => x.AppUserCreatedBy).Distinct();
        }

        public bool ChangeStatusMulti(string[] explanationId, string statusName, string delegateId,out string error)
        {
            error = CommonConstants.StringEmpty;
            foreach (var id in explanationId)
            {
                if (!ChangeStatus(Convert.ToInt32(id), statusName, delegateId,out error))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Get AbnormalCaseReason by abnormal id
        /// </summary>
        /// <param name="id">id of abnormal case</param>
        /// <returns> AbnormalCaseReason </returns>
        public IEnumerable<AbnormalCase> GetAbnormalById(int id)
        {
            return _explanationRequestRepository.GetAbnormalById(id);
        }
        public IEnumerable<ExplanationRequestChartModel> ExRequestChart(int groupID)
        {
             var model = _explanationRequestRepository.GetMulti(x => x.AppUserCreatedBy.GroupId == groupID, new string[] { CommonConstants.StatusRequest, CommonConstants.AppUserCreatedBy });
            double total = model.Count();
            double totaltemp = total;
            var allStatus = _statusRequestRepository.GetAll();
            var totalStatus = allStatus.Count();
            List<ExplanationRequestChartModel> ListExRequestChart = new List<ExplanationRequestChartModel>();
            double totalpercent = 100;
            foreach (var item in allStatus)
            {
                int amount = model.Count(x => x.StatusRequest.ID == item.ID);
                double rawPercent = totaltemp == 0 ? 0 : amount / totaltemp * totalpercent;
                totaltemp -= amount;
                double roundedPercent = Math.Round(rawPercent, 2);
                totalpercent -= roundedPercent;
                ExplanationRequestChartModel requestChart = new ExplanationRequestChartModel()
                {
                    TotalExRequest = total,
                    StatusExRequests = item.Name,
                    CountStatusExRequest = model.Count(x => x.StatusRequest.ID == item.ID),
                    PercentStatusExRequest = roundedPercent
                };
                ListExRequestChart.Add(requestChart);
            }
            return ListExRequestChart;
        }

        public IEnumerable<ExplanationRequestChartModel> ExRequestChartByUser(string userID)
        {
            var model = _explanationRequestRepository.GetMulti(x => x.AppUserCreatedBy.Id == userID, new string[] { CommonConstants.StatusRequest, CommonConstants.AppUserCreatedBy });
            double total = model.Count();
            double totaltemp = total;
            var allStatus = _statusRequestRepository.GetAll();
            List<ExplanationRequestChartModel> ListExRequestChart = new List<ExplanationRequestChartModel>();
            double totalpercent = 100;
            foreach (var item in allStatus)
            {
                int amount = model.Count(x => x.StatusRequest.ID == item.ID);
                double rawPercent = totaltemp == 0 ? 0 : amount / totaltemp * totalpercent;
                totaltemp -= amount;
                double roundedPercent = Math.Round(rawPercent, 2);
                totalpercent -= roundedPercent;
                ExplanationRequestChartModel requestChart = new ExplanationRequestChartModel()
                {
                    TotalExRequest = total,
                    StatusExRequests = item.Name,
                    CountStatusExRequest = model.Count(x => x.StatusRequest.ID == item.ID),
                    PercentStatusExRequest = roundedPercent
                };
                ListExRequestChart.Add(requestChart);
            }
            return ListExRequestChart;
        }

        public int ExRequestByUser(string userID)
        {
            return _explanationRequestRepository.GetMulti(x => x.AppUserCreatedBy.Id == userID,new string[] { CommonConstants.AppUserCreatedBy }).Count();
        }
        public FingerTimeSheet GetFingerTimeSheetByExplanationID(int ExplantionID)
        {
            var explanation = _explanationRequestRepository.GetSingleById(ExplantionID);
            return _timeSheetRepository.GetSingleById(explanation.TimeSheetId);
        }

        public Entitleday_AppUser GetEntitleDayByUserID(string userID)
        {
            return _entitleDayAppUserRepository.GetSingleByCondition(x => x.UserId == userID);
        }

        //Get List explanation by user
        public IEnumerable<ExplanationRequest> GetListExplanationByUser(string userID, string groupId)
        {
            if (_explanationRequestRepository.IsReadAll(userID, CommonConstants.FunctionExplanationRequest))
            {
                return _explanationRequestRepository.GetMulti(x => (x.FingerTimeSheet.FingerMachineUsers.AppUser.GroupId.ToString().Equals(groupId) ),
                    new string[] {
                    CommonConstants.FingerTimeSheet,
                    CommonConstants.FingerMachineUser,
                    CommonConstants.TimeSheetAppUser,
                    CommonConstants.TimeSheetAppUserGroup,
                    CommonConstants.StatusRequest,
                    CommonConstants.ReceiverUser,
                    CommonConstants.DelegateUser,
                    CommonConstants.AppUserUpdatedBy});
            }
            return _explanationRequestRepository.GetMulti(x => (x.CreatedBy.Equals(userID)), new string[] {
                CommonConstants.FingerTimeSheet,
                CommonConstants.FingerMachineUser,
                CommonConstants.TimeSheetAppUser,
                CommonConstants.TimeSheetAppUserGroup,
                CommonConstants.StatusRequest,
                CommonConstants.ReceiverUser,
                CommonConstants.DelegateUser,
                CommonConstants.AppUserUpdatedBy});
        }
        //Change status explanation request by delegate default, when set delegate default in menu group
        public void ChangeStatusExplanationDelegateDefault(string[] explanationId, string statusName, string delegateId, DateTime startDate, DateTime endDate)
        {
            var status = _statusRequestRepository.GetMulti(x => x.Name.Contains(statusName)).FirstOrDefault();
            foreach (var id in explanationId)
            {
                var explanation = GetExplanationDetail(Convert.ToInt32(id));
                if(startDate <= explanation.CreatedDate.Value.Date && explanation.CreatedDate.Value.Date <= endDate)
                {
                    explanation.StatusRequestId = status.ID;
                    explanation.DelegateId = delegateId;
                    explanation.UpdatedDate = DateTime.Now;
                    _explanationRequestRepository.Update(explanation);
                }
            }
            Save();
        }

        //Change status explanation request by delegate default, when create explanation request
        //Check if created date of explanation request in about time set delegate default,Explanation Request is delegated by person who grouplead choosen
        public void AddDelegateDefault(string groupId, int expanationrequestId)
        {
            var status = _statusRequestRepository.GetMulti(x => x.Name.Contains(CommonConstants.StatusDelegation)).FirstOrDefault();
            var group = _groupRepository.GetSingleById(Int32.Parse(groupId));
            ExplanationRequest explanationrequestEntity = GetExplanationDetail(expanationrequestId);
            if (group.StartDate <= explanationrequestEntity.CreatedDate.Value.Date && group.EndDate >= explanationrequestEntity.CreatedDate.Value.Date)
            {
                explanationrequestEntity.StatusRequestId = status.ID;
                if(group.DelegateId != null)
                {
                    explanationrequestEntity.DelegateId = group.DelegateId;
                }
                _explanationRequestRepository.Update(explanationrequestEntity);
                _unitOfWork.Commit();
            }
        }

        //Check data in Delegation Assigned List, If date now > end time set delegate default but this explanation request don't approve or reject.This explanation request will manage of grouplead
        public void CheckDataDelegationExplanationRequest(int groupId, int explanationrequestId)
        {
            //var status = _statusRequestRepository.GetMulti(x => x.Name.Contains(CommonConstants.StatusPending)).FirstOrDefault();
            //var group = _groupRepository.GetSingleById(groupId);
            ExplanationRequest explanationrequestEntity = GetExplanationDetail(explanationrequestId);
            var dataDelegation = _configDelegationService.GetDelegationByUserId(explanationrequestEntity.CreatedBy);
            if (dataDelegation.AssignTo != null && dataDelegation.AssignTo.Equals(explanationrequestEntity.DelegateId))
            {
                    if (dataDelegation.StartDate <= explanationrequestEntity.CreatedDate.Value.Date && dataDelegation.EndDate >= explanationrequestEntity.CreatedDate.Value.Date)
                    {
                        if (dataDelegation.EndDate < DateTime.Now.Date)
                        {
                            var statusDelegation = _statusRequestRepository.GetMulti(x => x.Name.Contains(CommonConstants.StatusPending)).FirstOrDefault();
                            explanationrequestEntity.StatusRequestId = statusDelegation.ID;
                            explanationrequestEntity.DelegateId = null;
                            _explanationRequestRepository.Update(explanationrequestEntity);
                        }
                    }
            }
            else
            {
                var group = _groupRepository.GetSingleById(groupId);
                if (group.StartDate <= explanationrequestEntity.CreatedDate.Value.Date && group.EndDate >= explanationrequestEntity.CreatedDate.Value.Date)
                {
                    if (group.EndDate < DateTime.Now.Date)
                    {
                        var status = _statusRequestRepository.GetMulti(x => x.Name.Contains(CommonConstants.StatusPending)).FirstOrDefault();
                        explanationrequestEntity.StatusRequestId = status.ID;
                        explanationrequestEntity.DelegateId = null;
                        _explanationRequestRepository.Update(explanationrequestEntity);
                    }
                }
            }

            
        }

        //Check groupUserDelegateId have equal or not groupId
        //Check data in Delegation Assigned List, If date now > end time set delegate default but this explanation request don't approve or reject.This explanation request will manage of grouplead
        public void CheckDataDelegationAllExplanationRequest(int groupId, int explanationrequestId, int groupUserDelegateId)
        {
            var status = _statusRequestRepository.GetMulti(x => x.Name.Contains(CommonConstants.StatusPending)).FirstOrDefault();
            var group = _groupRepository.GetSingleById(groupId);
            ExplanationRequest explanationrequestEntity = GetExplanationDetail(explanationrequestId);
            var dataDelegation = _configDelegationService.GetDelegationByUserId(explanationrequestEntity.CreatedBy);
            if (dataDelegation.AssignTo != null && dataDelegation.AssignTo.Equals(explanationrequestEntity.DelegateId))
            {
                    if (dataDelegation.StartDate <= explanationrequestEntity.CreatedDate.Value.Date && dataDelegation.EndDate >= explanationrequestEntity.CreatedDate.Value.Date)
                    {
                        if (dataDelegation.EndDate < DateTime.Now.Date)
                        {
                            explanationrequestEntity.StatusRequestId = status.ID;
                            explanationrequestEntity.DelegateId = null;
                            _explanationRequestRepository.Update(explanationrequestEntity);
                        }
                    }
            }
            else
            {
                if (groupId == groupUserDelegateId)
                {
                    if (group.StartDate <= explanationrequestEntity.CreatedDate.Value.Date && group.EndDate >= explanationrequestEntity.CreatedDate.Value.Date)
                    {
                        if (group.EndDate < DateTime.Now.Date)
                        {
                            explanationrequestEntity.StatusRequestId = status.ID;
                            explanationrequestEntity.DelegateId = null;
                            _explanationRequestRepository.Update(explanationrequestEntity);
                        }
                    }
                }
                else
                {
                    var groupUserDelegate = _groupRepository.GetSingleById(groupUserDelegateId);
                    if (groupUserDelegate.StartDate <= explanationrequestEntity.CreatedDate.Value.Date && groupUserDelegate.EndDate >= explanationrequestEntity.CreatedDate.Value.Date)
                    {
                        if (groupUserDelegate.EndDate < DateTime.Now.Date)
                        {
                            explanationrequestEntity.StatusRequestId = status.ID;
                            explanationrequestEntity.DelegateId = null;
                            _explanationRequestRepository.Update(explanationrequestEntity);
                        }
                    }
                }
            }
                     
        }

        /// <summary>
        /// Get explanations list after sort, filter for super admin
        /// </summary>
        /// <param name="column">name of column to sort</param>
        /// <param name="isDesc">parameter to check to sort by asc or desc</param>
        /// <param name="page">current page(use in paging)</param>
        /// <param name="pageSize">number of page showing(use in paging)</param>
        /// <param name="filter">list conditions to filter</param>
        /// <returns> explanations list </returns>
        public IEnumerable<ExplanationRequest> GetExplanationsListForSuperAdmin(string column,
            bool isDesc, int page, int pageSize, FilterExplanationViewModel filter)
        {
            var query = GetListOriginForSuperAdmin();

            // get list after filter if filter list != null
            if (filter != null)
            {
                query = GetListByFilter(query, filter);
            }
            totalEntries = query.Count();

            // get list after sort if column = CreatedBy
            if (column == CommonConstants.OrderByCreatedBy)
            {
                return GetListBySort(query, isDesc).Skip((page - 1) * pageSize).Take(pageSize);
            }
            return string.IsNullOrEmpty(column) ? query.Skip((page - 1) * pageSize).Take(pageSize) : query.OrderByField(column, isDesc).Skip((page - 1) * pageSize).Take(pageSize);
        }


        /// <summary>
        /// Get list original explanation request without sort, filter for super admin
        /// </summary>
        /// <returns> explanation request list </returns>
        public IEnumerable<ExplanationRequest> GetListOriginForSuperAdmin()
        {
                return _explanationRequestRepository.GetMulti(x => true,
                    new string[] {
                    CommonConstants.AppUserCreatedBy,
                    CommonConstants.AppUserUpdatedBy,
                    CommonConstants.ReceiverUser,
                    CommonConstants.DelegateUser,
                    CommonConstants.FingerTimeSheet,
                    CommonConstants.FingerMachineUser,
                    CommonConstants.TimeSheetAppUser,
                    CommonConstants.TimeSheetAppUserGroup,
                    CommonConstants.StatusRequest}).OrderByDescending(x => x.CreatedDate);
        }

    }
}
