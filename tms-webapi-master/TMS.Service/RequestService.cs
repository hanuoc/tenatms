using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TMS.Common.Constants;
using TMS.Common.Exceptions.Extensions;
using TMS.Common.ViewModels;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Model.Models;

namespace TMS.Service
{
    public interface IRequestService
    {
        IEnumerable<Request> GetAllRequestByUser(string userID, string groupId);

        IEnumerable<Request> GetAllRequest(string userID, string groupId, FilterModelRequest filter);

        bool Add(Request Request);

        void UpdateRequest(Request request);

        bool CheckUpdateRequest(Request request);

        Request GetById(int id);

        Request ChangeStatusRequest(Request request, string typeStatus, string userIdDelegate);

        void Save();

        IEnumerable<Request> GetDelegationRequest(string userID, string groupID, FilterDelegationAssignedModel filter);

        IEnumerable<string> GetListCreator(string userID, string groupId);

        Task<bool> ChangeStatusListRequest(List<Request> request, string typeStatus, string userIdDelegate);
        void UpdateEntitleDayID(int id);
        IEnumerable<RequestChart> RequestChart(int groupID);
        IEnumerable<RequestChart> RequestChartByUser(string userID);
        IEnumerable<Request> GetAll(int id);
        int RequestByUser(string userID);
        void UpdateDateRequest(DateTime dateTime);


        bool GetRequestAll(int entitledayId);

        int GetAllRequest(Request request);
        void CheckDelegateDefault(string groupId, int requestId, string ChangeStatusById);
        void CheckDataDelegationRequest(string groupId, Request requestEntity);
        void ChangeStatusDelegateDefault(string typeStatus, string userIdDelegate, DateTime startDate, DateTime endDate, string ChangeStatusById, string[] requestId);
        void CheckDataDelegationAllRequest(string groupId, Request requestEntity, string groupUserDelegateId);


        IEnumerable<Request> GetAllRequestSuperAdmin(FilterModelRequest filter);

        IEnumerable<Request> GetAllAssignedRequestForUser(string userID, string groupID);

        void checkDayBreak(Request request);

        bool checkUnitEntitleday(Request request);
		void HandleAddHoliday(Holiday holiday);
		void HandleUpdateHoliday_EntitleDay(Holiday holiday, Holiday existHoliday);
		void HandleDeleteHoliday(Holiday holiday);
	}

	public class RequestService : IRequestService
    {
        private IRequestRepository _requestRepository;
        private IStatusRequestRepository _statusRequestRepository;
        private IRequestTypeRepository _requestTypeRepository;
        private IRequestReasonTypeRepository _requestReasonTypeRepository;
        private IAppUserRepository _appUserRepository;
        private IEntitleDayRepository _entitleDayRepository;
        private IEntitleDayAppUserRepository _entitleDayAppUserRepository;
        private IFingerMachineUserService _fingermanchineUserRepository;
        private IGroupRepository _groupRepository;
        private IConfigDelegationService _configDelegationService;
		private IOTRequestService _otRequestService;
		private IStatusRequestService _statusRequestService;
		private IHolidayRepository _holidayRepository;
		private ITimeDayRepository _timeDayRepository;
		private IUnitOfWork _unitOfWork;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RequestService(IRequestRepository RequestRepository, IStatusRequestRepository statusRequestRepository, IRequestTypeRepository requestTypeRepository, IRequestReasonTypeRepository requestReasonTypeRepository, IAppUserRepository appUserRepository, IEntitleDayRepository entitleDayRepository, IEntitleDayAppUserRepository entitleDayAppUserRepository,
        IUnitOfWork unitOfWork, IFingerMachineUserService fingermanchineUserRepository, IGroupRepository groupRepository, IConfigDelegationService configDelegationService, IHolidayRepository holidayRepository, ITimeDayRepository timeDayRepository,IOTRequestService otRequestService, IStatusRequestService statusRequestService)
		{
			this._requestRepository = RequestRepository;
            this._statusRequestRepository = statusRequestRepository;
            this._requestTypeRepository = requestTypeRepository;
            this._requestReasonTypeRepository = requestReasonTypeRepository;
            this._appUserRepository = appUserRepository;
            this._entitleDayRepository = entitleDayRepository;
            this._entitleDayAppUserRepository = entitleDayAppUserRepository;
            this._groupRepository = groupRepository;
            this._unitOfWork = unitOfWork;
            this._configDelegationService = configDelegationService;
            _fingermanchineUserRepository = fingermanchineUserRepository;
			_otRequestService = otRequestService;
			_statusRequestService = statusRequestService;
            _holidayRepository = holidayRepository;
            _timeDayRepository = timeDayRepository;
        }

        /// <summary>
        /// get request by user and group
        /// </summary>
        /// <param name="userID">Id of user login</param>
        /// <param name="groupId">groupId of user login</param>
        /// <returns>return list request with user and group</returns>
        public IEnumerable<Request> GetAllRequestByUser(string userID, string groupId)
        {
            if (_requestRepository.IsReadAll(userID, CommonConstants.FunctionRequest))
            {
                return _requestRepository.GetMulti(x => x.AppUser.GroupId.ToString().Equals(groupId), new string[] { CommonConstants.RequestType
                    , CommonConstants.RequestReasonType, CommonConstants.StatusRequest, CommonConstants.AppUserGroup, CommonConstants.AppUserAssignGroup, CommonConstants.AppUserChangeStatusGroup }).OrderByDescending(x => x.CreatedDate);
            }
            return _requestRepository.GetMulti(x => x.AppUser.Id.Equals(userID)
                    , new string[] { CommonConstants.RequestType, CommonConstants.RequestReasonType, CommonConstants.StatusRequest, CommonConstants.AppUserGroup }).OrderByDescending(x => x.CreatedDate);
        }

        /// <summary>
        /// Get all request by user & group & filter this
        /// </summary>
        /// <param name="userId">Id of user login</param>
        /// <param name="groupId">groupId of user login</param>
        /// <param name="filter">list filter user want to filter</param>
        /// <returns></returns>
        public IEnumerable<Request> GetAllRequest(string userID, string groupId, FilterModelRequest filter)
        {
            var model = GetAllRequestByUser(userID, groupId);

            if (filter != null)
            {
                if (filter.Creators != null)
                {
                    model = filter.Creators.Count() > CommonConstants.ZERO ? model.Where(x => filter.Creators.Contains(x.AppUser.Id)) : model;
                }
                if (filter.RequestReasonType.Count() != CommonConstants.ZERO)
                {
                    model = model.Where(x => filter.RequestReasonType.Contains(x.EntitleDayId.ToString()));
                }
                if (filter.RequestType.Count() != CommonConstants.ZERO)
                {
                    model = model.Where(x => filter.RequestType.Contains(x.RequestTypeId.ToString()));
                }
                if (filter.StatusRequest.Count() != CommonConstants.ZERO)
                {
                    model = model.Where(x => filter.StatusRequest.Contains(x.RequestStatusId.ToString()));
                }
                if (!string.IsNullOrEmpty(filter.StartDate) && !string.IsNullOrEmpty(filter.EndDate) && model.Count() != CommonConstants.ZERO)
                {
                    //DateTime maxEndDate = model.OrderByField("EndDate", false).FirstOrDefault().EndDate;
                    //if (filter.StartDate == filter.EndDate)
                    //{
                    //    model = model.Where(x => x.StartDate == DateTime.ParseExact(filter.StartDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture)
                    //   || x.EndDate == DateTime.ParseExact(filter.EndDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture));
                    //}
                    //else
                    //{
                    //    model = maxEndDate < DateTime.ParseExact(filter.EndDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture) ? model.Where(x => x.StartDate >= DateTime.ParseExact(filter.StartDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture)
                    //&& x.EndDate <= maxEndDate) : model.Where(x => x.StartDate >= DateTime.ParseExact(filter.StartDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture)
                    // && x.EndDate <= DateTime.ParseExact(filter.EndDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture));
                    //}
                    model = model.Where(x => !(DateTime.ParseExact(filter.EndDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture) < x.StartDate || DateTime.ParseExact(filter.StartDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture) > x.EndDate));
                }
            }
            return model;
        }

        public IEnumerable<Request> GetAllRequestByUserSuperAdmin()
        {

            return _requestRepository.GetAll(new string[] { CommonConstants.RequestType
                    , CommonConstants.RequestReasonType, CommonConstants.StatusRequest, CommonConstants.AppUserGroup, CommonConstants.AppUserAssignGroup, CommonConstants.AppUserChangeStatusGroup }).OrderByDescending(x => x.CreatedDate);

        }

        public IEnumerable<Request> GetAllRequestSuperAdmin(FilterModelRequest filter)
        {
            var model = GetAllRequestByUserSuperAdmin();

            if (filter != null)
            {
                if (filter.Creators != null)
                {
                    model = filter.Creators.Count() > CommonConstants.ZERO ? model.Where(x => filter.Creators.Contains(x.AppUser.Id)) : model;
                }
                if (filter.RequestReasonType.Count() != CommonConstants.ZERO)
                {
                    model = model.Where(x => filter.RequestReasonType.Contains(x.EntitleDayId.ToString()));
                }
                if (filter.RequestType.Count() != CommonConstants.ZERO)
                {
                    model = model.Where(x => filter.RequestType.Contains(x.RequestTypeId.ToString()));
                }
                if (filter.StatusRequest.Count() != CommonConstants.ZERO)
                {
                    model = model.Where(x => filter.StatusRequest.Contains(x.RequestStatusId.ToString()));
                }
                if (!string.IsNullOrEmpty(filter.StartDate) && !string.IsNullOrEmpty(filter.EndDate) && model.Count() != CommonConstants.ZERO)
                {
                    //DateTime maxEndDate = model.OrderByField("EndDate", false).FirstOrDefault().EndDate;
                    //if (filter.StartDate == filter.EndDate)
                    //{
                    //    model = model.Where(x => x.StartDate == DateTime.ParseExact(filter.StartDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture)
                    //   || x.EndDate == DateTime.ParseExact(filter.EndDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture));
                    //}
                    //else
                    //{
                    //    model = maxEndDate < DateTime.ParseExact(filter.EndDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture) ? model.Where(x => x.StartDate >= DateTime.ParseExact(filter.StartDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture)
                    //&& x.EndDate <= maxEndDate) : model.Where(x => x.StartDate >= DateTime.ParseExact(filter.StartDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture)
                    // && x.EndDate <= DateTime.ParseExact(filter.EndDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture));
                    //}
                    model = model.Where(x => !(DateTime.ParseExact(filter.EndDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture) < x.StartDate || DateTime.ParseExact(filter.StartDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture) > x.EndDate));
                }
            }
            return model;
        }

        /// <summary>
        /// Get request by id
        /// </summary>
        /// <param name="id">id of request</param>
        /// <returns></returns>
        public Request GetById(int id)
        {
            return _requestRepository.GetMulti(x => x.ID == id, new string[] { CommonConstants.RequestType
                    , CommonConstants.RequestReasonType, CommonConstants.StatusRequest, CommonConstants.AppUserGroup, CommonConstants.AppUserAssignGroup, CommonConstants.AppUserChangeStatusGroup }).FirstOrDefault(); ;
        }

        /// <summary>
        /// Change status of request
        /// </summary>
        /// <param name="request">Request want to change</param>
        /// <param name="typeStatus">type status want to change</param>
        public Request ChangeStatusRequest(Request request, string typeStatus, string userIdDelegate)
        {
            int requestStatusId = _statusRequestRepository.GetMulti(status => status.Name.Equals(typeStatus)).FirstOrDefault().ID;
            string requestName = _requestTypeRepository.GetSingleById(request.RequestTypeId).Name;
            var result = false;
            switch (typeStatus)
            {
                case CommonConstants.StatusApproved:
                    result = ApproveRejectRequest(request, requestName, typeStatus);
                    break;

                case CommonConstants.StatusRejected:
                    result = ApproveRejectRequest(request, requestName, typeStatus);
                    break;

                case CommonConstants.StatusCancelled:
                    result = ApproveRejectRequest(request, requestName, typeStatus);
                    break;

                //Grouplead cancel delegated request then status request from delegated -> pending
                case CommonConstants.StatusPending:
                    return UpdateDelegate(request, requestStatusId);
                case CommonConstants.StatusDelegation:
                    request.AssignToId = userIdDelegate;
                    return UpdateStatusRequest(request, requestStatusId);
            }
            if (result)
            {
                request = _requestRepository.GetSingleByCondition(x => x.ID == request.ID);
                request.RequestStatusId = requestStatusId;
                _requestRepository.Update(request);
                _unitOfWork.Commit();
                //UpdateDateRequest();
                return request;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// method update status request
        /// code by nvthang
        /// </summary>
        /// <param name="request">object request</param>
        /// <param name="requestStatusId">status request</param>
        /// <returns></returns>
        private Request UpdateStatusRequest(Request request, int requestStatusId)
        {
            request = _requestRepository.GetSingleByCondition(x => x.ID == request.ID);
            request.RequestStatusId = requestStatusId;
            _requestRepository.Update(request);
            _unitOfWork.Commit();
            return request;
        }

        /// <summary>
        /// Save chagne
        /// </summary>
        public void Save()
        {
            _unitOfWork.Commit();
        }

        public Request UpdateDelegate(Request request, int requestStatusId)
        {
            request = _requestRepository.GetSingleByCondition(x => x.ID == request.ID);
            request.RequestStatusId = requestStatusId;
            request.AssignToId = null;
            _requestRepository.Update(request);
            try
            {
                _unitOfWork.Commit();
                log.Debug("Commit successfully!");
            }
            catch (Exception ex)
            {
                log.Error("Bug delegate:" + ex.Message);
            }
            return request;
        }

        /// <summary>
        /// get request is assign for user
        /// </summary>
        /// <param name="userID">userID of user</param>
        /// <param name="groupID">groupID of user</param>
        /// <returns>list request of user</returns>
        public IEnumerable<Request> GetAllAssignedRequestForUser(string userID, string groupID)
        {
            if (_requestRepository.IsReadAll(userID, CommonConstants.FunctionDelegationMemberRequest))
            {
                return _requestRepository.GetMulti((x => (x.AppUser.GroupId.ToString().Equals(groupID) && x.AssignToId != null)), new string[] { CommonConstants.RequestType
                    , CommonConstants.RequestReasonType, CommonConstants.StatusRequest, CommonConstants.AppUserGroup, CommonConstants.AppUserAssignGroup, CommonConstants.AppUserDelegate ,CommonConstants.AppUserChangeStatusGroup }).OrderByDescending(x => x.CreatedDate);
            }
            return _requestRepository.GetMulti(x => x.AssignToId.Equals(userID), new string[] { CommonConstants.RequestType
                    , CommonConstants.RequestReasonType, CommonConstants.StatusRequest, CommonConstants.AppUserGroup, CommonConstants.AppUserAssignGroup, CommonConstants.AppUserDelegate, CommonConstants.AppUserChangeStatusGroup
            }).OrderByDescending(x => x.CreatedDate);
        }

        /// <summary>
        /// Get list delegation request
        /// </summary>
        /// <param name="userID">ID of username login</param>
        /// <param name="filter">Data object want to filter</param>
        /// <returns>list model</returns>
        public IEnumerable<Request> GetDelegationRequest(string userID, string groupID, FilterDelegationAssignedModel filter)
        {
            var model = GetAllAssignedRequestForUser(userID, groupID);
            if (filter != null)
            {
                if (filter.usernameAssigned.Length > 0)
                {
                    model = model.Where(x => filter.usernameAssigned.Contains(x.UserId.ToString()));
                }
                if (filter.StatusRequestType.Count() != CommonConstants.ZERO)
                {
                    model = model.Where(x => filter.StatusRequestType.Contains(x.RequestStatusId.ToString()));
                }
                if (!string.IsNullOrEmpty(filter.startDate) && !string.IsNullOrEmpty(filter.endDate))
                {
                    model = model.Where(x => (x.StartDate >= DateTime.ParseExact(filter.startDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture))
                                             && (x.EndDate <= DateTime.ParseExact(filter.endDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture)));
                }
            }
            return model;
        }

        /// <summary>
        /// Get list creator
        /// </summary>
        /// <param name="userID">userid login</param>
        /// <param name="groupId">groupid of user login</param>
        /// <returns></returns>
        public IEnumerable<string> GetListCreator(string userID, string groupId)
        {
            var model = GetAllRequestByUser(userID, groupId).Select(x => x.AppUser.FullName).Distinct();

            return model;
        }

        public async Task<bool> ChangeStatusListRequest(List<Request> listRequest, string typeStatus, string userIdDelegate)
        {
            try
            {
                foreach (var request in listRequest)
                {
                    string result = "";
                    ChangeStatusRequest(request, typeStatus, userIdDelegate);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<Request> GetAll(int id)
        {
            return _requestRepository.GetMulti(x => x.EntitleDayId == id);
        }

        public void UpdateEntitleDayID(int id)
        {
            var request = _requestRepository.GetMulti(x => x.EntitleDayId == id);

            foreach (var item in request)
            {
                item.EntitleDayId = null;
                _requestRepository.Update(item);
            }

            Save();
        }

        public IEnumerable<RequestChart> RequestChart(int groupID)
        {
            var model = _requestRepository.GetMulti(x => x.AppUser.GroupId == groupID, new string[] { CommonConstants.StatusRequest, CommonConstants.AppUserGroup });
            double total = model.Count();
            double totaltemp = total;
            double totalpercent = 100;
            var allStatus = _statusRequestRepository.GetAll();
            List<RequestChart> ListRequestChart = new List<RequestChart>();
            foreach (var item in allStatus)
            {
                int amount = model.Count(x => x.StatusRequest.ID == item.ID);
                double rawPercent = totaltemp == 0 ? 0 : amount / totaltemp * totalpercent;
                totaltemp -= amount;
                double roundedPercent = Math.Round(rawPercent, 2);
                totalpercent -= roundedPercent;
                RequestChart requestChart = new RequestChart()
                {
                    TotalRequest = total,
                    StatusRequests = item.Name,
                    CountStatusRequest = model.Count(x => x.StatusRequest.ID == item.ID),
                    PercentStatusRequest = roundedPercent
                };
                ListRequestChart.Add(requestChart);
            }
            return ListRequestChart;
        }
        public int RequestByUser(string userID)
        {
            var model = _requestRepository.GetMulti(x => x.AppUser.Id == userID, new string[] { CommonConstants.AppUserGroup }).Count();
            return model;
        }

        public IEnumerable<RequestChart> RequestChartByUser(string userID)
        {
            var model = _requestRepository.GetMulti(x => x.AppUser.Id == userID, new string[] { CommonConstants.StatusRequest, CommonConstants.AppUserGroup });
            double total = model.Count();
            double totaltemp = total;
            var allStatus = _statusRequestRepository.GetAll();
            List<RequestChart> ListRequestChart = new List<RequestChart>();
            double totalpercent = 100;
            foreach (var item in allStatus)
            {
                int amount = model.Count(x => x.StatusRequest.ID == item.ID);
                double rawPercent = totaltemp == 0 ? 0 : amount / totaltemp * totalpercent;
                totaltemp -= amount;
                double roundedPercent = Math.Round(rawPercent, 2);
                totalpercent -= roundedPercent;
                RequestChart requestChart = new RequestChart()
                {
                    TotalRequest = total,
                    StatusRequests = item.Name,
                    CountStatusRequest = model.Count(x => x.StatusRequest.ID == item.ID),
                    PercentStatusRequest = roundedPercent
                };
                ListRequestChart.Add(requestChart);
            }
            return ListRequestChart;
        }



        /// <summary>
        /// Change status Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestName"></param>
        /// <param name="typeStatus"></param>
        /// <returns></returns>
        public bool ApproveRejectRequest(Request request, string requestName, string typeStatus)
        {
            //float dayBreak = _requestRepository.CalculateDateBreak(request.StartDate, request.EndDate, requestName);
            float dayBreak = this.CalculateDateBreak(request.StartDate, request.EndDate, requestName);
            string requestReason = request.EntitleDayId != null ? _entitleDayRepository.GetMulti(type => type.ID == request.EntitleDayId).FirstOrDefault().HolidayType : "";
            if (requestName.Contains(CommonConstants.Break) && !string.IsNullOrEmpty(requestReason) && !requestReason.Equals(CommonConstants.UnpaidLeave))
            {
                EntitledayModel entitleDayOfUser = _entitleDayRepository.GetAllEntitleDay(request.UserId, request.AppUser.GroupId.ToString(), false)
               .Where(x => x.HolidayType == requestReason).FirstOrDefault();
                Entitleday_AppUser entitledayAppUser = _entitleDayAppUserRepository.GetSingleById(entitleDayOfUser.EntitleDayAppUserId);

                if (typeStatus.Equals(CommonConstants.StatusApproved) && entitleDayOfUser.UnitType.Equals(CommonConstants.Day))
                {
                    if (request.StatusRequest.Name.Equals(CommonConstants.StatusRejected))
                    {
                        if (entitledayAppUser.TemporaryMaxEntitleDay + dayBreak <= entitledayAppUser.MaxEntitleDayAppUser)
                        {
                            entitledayAppUser.DayBreak += dayBreak;
                            entitledayAppUser.TemporaryMaxEntitleDay += dayBreak;
                            return true;
                        }
                        return false;
                    }
                    else
                    {
                        if (entitledayAppUser.TemporaryMaxEntitleDay <= entitledayAppUser.MaxEntitleDayAppUser)
                        {
                            return true;
                        }
                        return false;
                    }
                }
                if (typeStatus.Equals(CommonConstants.StatusRejected) && entitleDayOfUser.UnitType.Equals(CommonConstants.Day))
                {
                    if (request.StatusRequest.Name.Equals(CommonConstants.StatusPending))
                    {
                        entitledayAppUser.DayBreak -= dayBreak;
                        entitledayAppUser.TemporaryMaxEntitleDay -= dayBreak;
                        return true;
                    }
                    else
                    {
                        entitledayAppUser.DayBreak -= dayBreak;
                        entitledayAppUser.TemporaryMaxEntitleDay -= dayBreak;
                        return true;
                    }
                }
                if (typeStatus.Equals(CommonConstants.StatusCancelled))
                {
                    entitledayAppUser.DayBreak -= dayBreak;
                    entitledayAppUser.TemporaryMaxEntitleDay -= dayBreak;
                    return true;
                }
            }
            return true;
        }



        /// <summary>
        /// Add request to database
        /// </summary>
        /// <param name="request">Request want to add</param>
        /// <returns></returns>
        public bool Add(Request request)
        {
            if (request.RequestTypeId == CommonConstants.RequestTypeLateComming || request.RequestTypeId == CommonConstants.RequestTypeEarlyLeaving)
            {
                request.EntitleDayId = null;
            }
            string requestReason = request.EntitleDayId != null ? _entitleDayRepository.GetMulti(type => type.ID == request.EntitleDayId).FirstOrDefault().HolidayType : "";
            EntitledayModel entitleDayOfUser = _entitleDayRepository.GetAllEntitleDay(request.UserId, request.AppUser.GroupId.ToString(), false)
                .Where(x => x.HolidayType == requestReason).FirstOrDefault();
            string requestName = _requestTypeRepository.GetSingleById(request.RequestTypeId).Name;
            request.RequestStatusId = _statusRequestRepository.GetMulti(status => status.Name.Contains(CommonConstants.StatusPending)).FirstOrDefault().ID;
            //float dayBreak = _requestRepository.CalculateDateBreak(request.StartDate, request.EndDate, requestName);

            float dayBreak = this.CalculateDateBreak(request.StartDate, request.EndDate, requestName);

            if (entitleDayOfUser == null)
            {
                request.AppUser = null;
                _requestRepository.Add(request);
                Save();
                return true;
            }
            else
            {
                if (!(request.StartDate > request.EndDate)
                    && (!requestName.Contains(CommonConstants.Break) || requestReason.Equals(CommonConstants.UnpaidLeave) || (entitleDayOfUser != null
                    && requestName.Contains(CommonConstants.Break) && _appUserRepository.CheckEntitleDay(request, entitleDayOfUser) >= dayBreak)))
                {
                    request.AppUser = null;
                    _requestRepository.Add(request);
                    if (entitleDayOfUser != null)
                    {
                        Entitleday_AppUser entitledayAppUser = _entitleDayAppUserRepository.GetSingleById(entitleDayOfUser.EntitleDayAppUserId);
                        if (entitledayAppUser.EntitleDay.UnitType.Equals(CommonConstants.Day))
                        {
                            if (entitledayAppUser.TemporaryMaxEntitleDay + dayBreak <= entitleDayOfUser.MaxEntitleDay)
                            {
                                entitledayAppUser.DayBreak += dayBreak;
                                entitledayAppUser.TemporaryMaxEntitleDay += dayBreak;
                                _entitleDayAppUserRepository.Update(entitledayAppUser);
                                Save();
                                return true;
                            }
                        }
                        else if (entitledayAppUser.EntitleDay.UnitType.Equals(CommonConstants.DayPeriod))
                        {
                            Save();
                            return true;
                        }

                        return false;
                    }
                    return true;
                }
            }
            return false;
        }
        public float CalculateDateBreak(DateTime startDate, DateTime endDate, string requestName)
        {
            DayOfWeek[] daysOfWeek = { DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday };

            var allDays = Enumerable.Range(CommonConstants.ZERO, (endDate - startDate).Days + CommonConstants.ONE).Select(d => startDate.AddDays(d));
            
            var Dates = allDays.Where(dt => daysOfWeek.Contains(dt.DayOfWeek)).ToList();

            float dateBreak = CommonConstants.ZERO;
           
            if (requestName.Equals(CommonConstants.BreakMorning) || requestName.Equals(CommonConstants.BreakAfternoon))
            {

                // Nghỉ nửa ngày
                foreach (var item in Dates)
                {
                    if (_timeDayRepository.IsTimeDay(item))
                    {
                        dateBreak += (float)CommonConstants.ZERO_PONT_FIVE;
                    }
                    if (_holidayRepository.IsHoliday(item))
                    {
                        dateBreak -= (float)CommonConstants.ZERO_PONT_FIVE;
                    }
                    if (_holidayRepository.IsWorkingday(item))
                    {
                        dateBreak += (float)CommonConstants.ZERO_PONT_FIVE;
                    }
                }
            }
            else
            {
                // Nghỉ 1 ngày
                foreach (var item in Dates)
                {
                    if (_timeDayRepository.IsTimeDay(item))
                    {
                        dateBreak += (float)CommonConstants.ONE;
                    }
                    if (_holidayRepository.IsHoliday(item))
                    {
                        dateBreak -= (float)CommonConstants.ONE;
                    }
                    if (_holidayRepository.IsWorkingday(item))
                    {
                        dateBreak += (float)CommonConstants.ONE;
                    }
                }
            }
            return dateBreak;
        }

        /// <summary>
        /// Update Entitle Day Of Request
        /// </summary>
        public void UpdateDateRequest(DateTime datetime)
        {
            var requestTypeAbsent = new int[] { 1, 2, 3 };
            var model = _requestRepository.GetMulti(x => x.StartDate <= datetime.Date && x.EndDate >= datetime.Date && x.RequestStatusId == CommonConstants.StatusApprovedID && requestTypeAbsent.Contains(x.RequestTypeId), new string[] { CommonConstants.RequestType
                    , CommonConstants.RequestReasonType, CommonConstants.StatusRequest, CommonConstants.AppUserGroup, CommonConstants.AppUserAssignGroup, CommonConstants.AppUserChangeStatusGroup });
            foreach (var request in model)
            {
                var allDays = Enumerable.Range(CommonConstants.ZERO, (request.EndDate - request.StartDate).Days + CommonConstants.ONE).Select(d => request.StartDate.AddDays(d));
                float dateBreak = CommonConstants.ZERO;
                Entitleday_AppUser entitledayAppUser = _entitleDayAppUserRepository.GetMulti(x => x.UserId == request.UserId && x.EntitleDay.UnitType.Equals(CommonConstants.Day) && x.EntitleDayId == request.EntitleDayId).FirstOrDefault();
                if (request.EntitleDay.UnitType.Equals(CommonConstants.Day) && request.RequestTypeId == 1)
                {


                    if (_timeDayRepository.IsTimeDay(datetime))
                    {
                        dateBreak += (float)CommonConstants.ONE;
                    }
                    if (_holidayRepository.IsHoliday(datetime))
                    {
                        dateBreak -= (float)CommonConstants.ONE;
                    }
                    if (_holidayRepository.IsWorkingday(datetime))
                    {
                        dateBreak += (float)CommonConstants.ONE;
                    }
                    //if (datetime.DayOfWeek.Equals(DayOfWeek.Saturday) || datetime.DayOfWeek.Equals(DayOfWeek.Sunday))
                    //{
                    //    dateBreak = CommonConstants.ZERO;
                    //}
                    //else
                    //{
                    //    dateBreak = CommonConstants.ONE;
                    //}
                    entitledayAppUser.NumberDayOff = entitledayAppUser.NumberDayOff + dateBreak;
                    _entitleDayAppUserRepository.Update(entitledayAppUser);
                }
                if (request.EntitleDay.UnitType.Equals(CommonConstants.Day) && request.RequestTypeId == 2 || request.EntitleDay.UnitType.Equals(CommonConstants.Day) && request.RequestTypeId == 3)
                {
                    if (_timeDayRepository.IsTimeDay(datetime))
                    {
                        dateBreak += (float)CommonConstants.ZERO_PONT_FIVE;
                    }
                    if (_holidayRepository.IsHoliday(datetime))
                    {
                        dateBreak -= (float)CommonConstants.ZERO_PONT_FIVE;
                    }
                    if (_holidayRepository.IsWorkingday(datetime))
                    {
                        dateBreak += (float)CommonConstants.ZERO_PONT_FIVE;
                    }
                    //if (datetime.DayOfWeek.Equals(DayOfWeek.Saturday) || datetime.DayOfWeek.Equals(DayOfWeek.Sunday))
                    //{
                    //    dateBreak = CommonConstants.ZERO;
                    //}
                    //else
                    //{
                    //    dateBreak = (float)(CommonConstants.ZERO_PONT_FIVE);
                    //}
                    entitledayAppUser.NumberDayOff = entitledayAppUser.NumberDayOff + dateBreak;
                    _entitleDayAppUserRepository.Update(entitledayAppUser);
                }
            }
            Save();
        }
        /// <summary>
        /// check entitle day have request
        /// </summary>
        /// <param name="entitledayId"></param>
        /// <returns></returns>
        public bool GetRequestAll(int entitledayId)
        {
            var model = _requestRepository.GetAll();
            foreach (var item in model)
            {
                if (item.EntitleDayId == entitledayId)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Check Request Day Off
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public int GetAllRequest(Request request)
        {
            //List Date of Request
            var allDaysRequest = Enumerable.Range(CommonConstants.ZERO, (request.EndDate - request.StartDate).Days + CommonConstants.ONE).Select(d => request.StartDate.AddDays(d));

            //var model = _requestRepository.GetAll().Where(x => x.ID != request.ID);
            var lisUserRequest = _requestRepository.GetMulti(x => x.ID != request.ID && x.UserId == request.UserId && (x.StartDate >= request.StartDate && x.EndDate <= request.EndDate)).ToList();
            foreach (var item in lisUserRequest)
            {
                DateTime[] daysOfWeek = { item.StartDate, item.EndDate };
                var allDays = Enumerable.Range(CommonConstants.ZERO, (item.EndDate - item.StartDate).Days + CommonConstants.ONE).Select(d => item.StartDate.AddDays(d));

                foreach (var date in allDays)
                {
                    foreach (var daterequest in allDaysRequest)
                    {
                        if (date.Date.ToString(CommonConstants.FormatDate_DDMMYYY).Equals(daterequest.Date.ToString(CommonConstants.FormatDate_DDMMYYY)))
                        {

                            if (item.RequestTypeId == 1 && request.RequestTypeId == 1)
                            {
                                //kiểm tra trạng thái các request trước
                                if (item.RequestStatusId == 1 || item.RequestStatusId == 4 || item.RequestStatusId == 5)
                                {
                                    return 1;
                                }
                            }
                            if ((item.RequestTypeId == 1 && request.RequestTypeId == 2))
                            {
                                if (item.RequestStatusId == 1 || item.RequestStatusId == 4 || item.RequestStatusId == 5)
                                {
                                    return 2;
                                }
                            }
                            if ((item.RequestTypeId == 1 && request.RequestTypeId == 3))
                            {
                                if (item.RequestStatusId == 1 || item.RequestStatusId == 4 || item.RequestStatusId == 5)
                                {
                                    return 3;
                                }
                            }
                            if ((item.RequestTypeId == 1 && request.RequestTypeId == 4))
                            {
                                if (item.RequestStatusId == 1 || item.RequestStatusId == 4 || item.RequestStatusId == 5)
                                {
                                    return 4;
                                }
                            }
                            if ((item.RequestTypeId == 1 && request.RequestTypeId == 5))
                            {
                                if (item.RequestStatusId == 1 || item.RequestStatusId == 4 || item.RequestStatusId == 5)
                                {
                                    return 5;
                                }
                            }
                            if ((item.RequestTypeId == 2 && request.RequestTypeId == 1))
                            {
                                if (item.RequestStatusId == 1 || item.RequestStatusId == 4 || item.RequestStatusId == 5)
                                {
                                    return 6;
                                }
                            }
                            if ((item.RequestTypeId == 2 && request.RequestTypeId == 2))
                            {
                                if (item.RequestStatusId == 1 || item.RequestStatusId == 4 || item.RequestStatusId == 5)
                                {
                                    return 7;
                                }
                            }
                            if ((item.RequestTypeId == 2 && request.RequestTypeId == 4))
                            {
                                if (item.RequestStatusId == 1 || item.RequestStatusId == 4 || item.RequestStatusId == 5)
                                {
                                    return 8;
                                }
                            }
                            if ((item.RequestTypeId == 3 && request.RequestTypeId == 1))
                            {
                                if (item.RequestStatusId == 1 || item.RequestStatusId == 4 || item.RequestStatusId == 5)
                                {
                                    return 9;
                                }
                            }
                            if ((item.RequestTypeId == 3 && request.RequestTypeId == 3))
                            {
                                if (item.RequestStatusId == 1 || item.RequestStatusId == 4 || item.RequestStatusId == 5)
                                {
                                    return 10;
                                }
                            }
                            if ((item.RequestTypeId == 3 && request.RequestTypeId == 5))
                            {
                                if (item.RequestStatusId == 1 || item.RequestStatusId == 4 || item.RequestStatusId == 5)
                                {
                                    return 11;
                                }
                            }
                            if ((item.RequestTypeId == 4 && request.RequestTypeId == 1))
                            {
                                if (item.RequestStatusId == 1 || item.RequestStatusId == 4 || item.RequestStatusId == 5)
                                {
                                    return 12;
                                }
                            }
                            if ((item.RequestTypeId == 4 && request.RequestTypeId == 2))
                            {
                                if (item.RequestStatusId == 1 || item.RequestStatusId == 4 || item.RequestStatusId == 5)
                                {
                                    return 13;
                                }
                            }
                            if ((item.RequestTypeId == 4 && request.RequestTypeId == 4))
                            {
                                if (item.RequestStatusId == 1 || item.RequestStatusId == 4 || item.RequestStatusId == 5)
                                {
                                    return 14;
                                }
                            }
                            if ((item.RequestTypeId == 5 && request.RequestTypeId == 1))
                            {
                                if (item.RequestStatusId == 1 || item.RequestStatusId == 4 || item.RequestStatusId == 5)
                                {
                                    return 15;
                                }
                            }
                            if ((item.RequestTypeId == 5 && request.RequestTypeId == 3))
                            {
                                if (item.RequestStatusId == 1 || item.RequestStatusId == 4 || item.RequestStatusId == 5)
                                {
                                    return 16;
                                }
                            }
                            if ((item.RequestTypeId == 5 && request.RequestTypeId == 5))
                            {
                                if (item.RequestStatusId == 1 || item.RequestStatusId == 4 || item.RequestStatusId == 5)
                                {
                                    return 17;
                                }
                            }
                        }
                    }
                }
            }
            return 0;
        }
        public bool checkUnitEntitleday(Request request)
        {
            var entitleday = _entitleDayRepository.GetMulti(x => x.ID == request.EntitleDayId).FirstOrDefault();

            if (request.RequestTypeId == 1 || request.RequestTypeId == 4 || request.RequestTypeId == 5)
            {
                return true;
            }
            else if ((request.RequestTypeId == 2 || request.RequestTypeId == 3) && (entitleday.UnitType == "Day"))
            {
                return true;
            }
            return false;
        }
        //Lấy dữ liệu bảng tạm cũ và trừ bảng tạm
        public void checkDayBreak(Request request)
        {
            string requestReason = request.EntitleDayId != null ? _entitleDayRepository.GetMulti(type => type.ID == request.EntitleDayId).FirstOrDefault().HolidayType : "";
            string requestName = _requestTypeRepository.GetSingleById(request.RequestTypeId).Name;
            EntitledayModel entitleDayOfUser = _entitleDayRepository.GetAllEntitleDay(request.UserId, request.AppUser.GroupId.ToString(), false)
                .Where(x => x.HolidayType == requestReason).FirstOrDefault();
            //float olddayBreak = _requestRepository.CalculateDateBreak(request.StartDate, request.EndDate, requestName);
            float dayBreak = this.CalculateDateBreak(request.StartDate, request.EndDate, requestName);
            if (entitleDayOfUser != null)
            {
                Entitleday_AppUser entitledayAppUser = _entitleDayAppUserRepository.GetSingleById(entitleDayOfUser.EntitleDayAppUserId);
                if (entitleDayOfUser.UnitType.Equals(CommonConstants.Day))
                {
                    entitledayAppUser.DayBreak -= dayBreak;
                    entitledayAppUser.TemporaryMaxEntitleDay -= dayBreak;
                }
            }
        }
        // cộng vào bảng tạm
        public bool CheckUpdateRequest(Request request)
        {
            string requestReason = request.EntitleDayId != null ? _entitleDayRepository.GetMulti(type => type.ID == request.EntitleDayId).FirstOrDefault().HolidayType : "";
            string requestName = _requestTypeRepository.GetSingleById(request.RequestTypeId).Name;
            EntitledayModel entitleDayOfUser = _entitleDayRepository.GetAllEntitleDay(request.UserId, request.AppUser.GroupId.ToString(), false)
                .Where(x => x.HolidayType == requestReason).FirstOrDefault();
            //float dayBreak = _requestRepository.CalculateDateBreak(request.StartDate, request.EndDate, requestName);
            float dayBreak = this.CalculateDateBreak(request.StartDate, request.EndDate, requestName);

            if (entitleDayOfUser != null)
            {
                Entitleday_AppUser entitledayAppUser = _entitleDayAppUserRepository.GetSingleById(entitleDayOfUser.EntitleDayAppUserId);
                if (entitleDayOfUser.UnitType.Equals(CommonConstants.Day))
                {
                    if (entitledayAppUser.TemporaryMaxEntitleDay + dayBreak <= entitledayAppUser.MaxEntitleDayAppUser)
                    {
                        entitledayAppUser.DayBreak += dayBreak;
                        entitledayAppUser.TemporaryMaxEntitleDay += dayBreak;
                        _requestRepository.Update(request);
                        return true;
                    }
                }
                else if (entitleDayOfUser.UnitType.Equals(CommonConstants.DayPeriod))
                {
                    if (entitledayAppUser.TemporaryMaxEntitleDay + dayBreak <= entitledayAppUser.MaxEntitleDayAppUser)
                    {
                        entitledayAppUser.DayBreak += dayBreak;
                        entitledayAppUser.TemporaryMaxEntitleDay += dayBreak;
                        _requestRepository.Update(request);
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        public void UpdateRequest(Request request)
        {
            _requestRepository.Update(request);
            Save();
        }
        //Change status request by delegate default, when create request
        //Check if created date of request in about time set delegate default, Request is delegated by who grouplead choosen
        public void CheckDelegateDefault(string groupId, int requestId, string ChangeStatusById)
        {
            var group = _groupRepository.GetSingleById(Int32.Parse(groupId));
            Request requestEntity = GetById(requestId);
            if (group.StartDate <= requestEntity.CreatedDate.Value.Date && group.EndDate >= requestEntity.CreatedDate.Value.Date)
            {
                //requestEntity.ChangeStatusById = ChangeStatusById;
                requestEntity.UpdatedDate = DateTime.Now;
                ChangeStatusRequest(requestEntity, CommonConstants.StatusDelegation, group.DelegateId);
            }
        }

        //Check data in Delegation Assigned List, If date now > end time set delegate default but this request don't approve or reject.This request will manage of grouplead
        public void CheckDataDelegationRequest(string groupId, Request requestEntity)
        {
            var group = _groupRepository.GetSingleById(Int32.Parse(groupId));
            var dataDelegation = _configDelegationService.GetDelegationByUserId(requestEntity.UserId);
            //Request requestEntity = GetById(requestId);
            if (dataDelegation.AssignTo != null && dataDelegation.AssignTo.Equals(requestEntity.AssignToId))
            {
                if (dataDelegation.StartDate <= requestEntity.CreatedDate.Value.Date && dataDelegation.EndDate >= requestEntity.CreatedDate.Value.Date)
                {
                    if (dataDelegation.EndDate < DateTime.Now.Date)
                    {
                        ChangeStatusRequest(requestEntity, CommonConstants.StatusPending, null);
                    }
                }
            }
            else
            {
                if (group.StartDate <= requestEntity.CreatedDate.Value.Date && group.EndDate >= requestEntity.CreatedDate.Value.Date)
                {
                    if (group.EndDate < DateTime.Now.Date)
                    {
                        //requestEntity.ChangeStatusById = null;
                        //requestEntity.UpdatedDate = DateTime.Now;
                        ChangeStatusRequest(requestEntity, CommonConstants.StatusPending, null);
                    }
                }
            }


        }

        //Change status request by delegate default, when set delegate default in menu group
        public void ChangeStatusDelegateDefault(string typeStatus, string userIdDelegate, DateTime startDate, DateTime endDate, string ChangeStatusById, string[] requestId)
        {
            List<Request> listRequest = new List<Request>();
            foreach (var item in requestId)
            {
                Request requestEntity = GetById(int.Parse(item));
                if (startDate <= requestEntity.CreatedDate.Value.Date && requestEntity.CreatedDate.Value.Date <= endDate)
                {
                    requestEntity.ChangeStatusById = ChangeStatusById;
                    requestEntity.UpdatedDate = DateTime.Now;
                    listRequest.Add(requestEntity);
                }
            }
            ChangeStatusListRequest(listRequest, typeStatus, userIdDelegate);
        }

        //Check groupUserDelegateId have equal or not groupId
        //Check data in Delegation Assigned List, If date now > end time set delegate default but this request don't approve or reject.This request will manage of grouplead
        public void CheckDataDelegationAllRequest(string groupId, Request requestEntity, string groupUserDelegateId)
        {
            var group = _groupRepository.GetSingleById(Int32.Parse(groupId));
            //Request requestEntity = GetById(requestId);
            var dataDelegation = _configDelegationService.GetDelegationByUserId(requestEntity.UserId);
            if (dataDelegation.AssignTo != null && dataDelegation.AssignTo.Equals(requestEntity.AssignToId))
            {
                if (dataDelegation.StartDate <= requestEntity.CreatedDate.Value.Date && dataDelegation.EndDate >= requestEntity.CreatedDate.Value.Date)
                {
                    if (dataDelegation.EndDate < DateTime.Now.Date)
                    {
                        ChangeStatusRequest(requestEntity, CommonConstants.StatusPending, null);
                    }
                }
            }
            else
            {
                if (groupId.Equals(groupUserDelegateId))
                {
                    if (group.StartDate <= requestEntity.CreatedDate.Value.Date && group.EndDate >= requestEntity.CreatedDate.Value.Date)
                    {
                        if (group.EndDate < DateTime.Now.Date)
                        {
                            //requestEntity.ChangeStatusById = null;
                            //requestEntity.UpdatedDate = DateTime.Now;
                            ChangeStatusRequest(requestEntity, CommonConstants.StatusPending, null);
                        }
                    }
                }
                else
                {
                    var groupUserDelegate = _groupRepository.GetSingleById(Int32.Parse(groupUserDelegateId));
                    if (groupUserDelegate.StartDate <= requestEntity.CreatedDate.Value.Date && groupUserDelegate.EndDate >= requestEntity.CreatedDate.Value.Date)
                    {
                        if (groupUserDelegate.EndDate < DateTime.Now.Date)
                        {
                            //requestEntity.ChangeStatusById = null;
                            //requestEntity.UpdatedDate = DateTime.Now;
                            ChangeStatusRequest(requestEntity, CommonConstants.StatusPending, null);
                        }
                    }
                }
            }
        }

		/// <summary>
		/// get request by date holiday
		/// </summary>
		/// <returns>return list request</returns>
		public void HandleAddHoliday(Holiday holiday)
		{
			if (holiday.Workingday.HasValue)
			{
				AddTemporaryMaxEntitleDay(holiday.Workingday.Value);
				HandleRejectOTNormalAndUpdateType(holiday);
			}
			SubTemporaryMaxEntitleDay(holiday.Date);
			UpdateOTRequestType(holiday.Date.Date);
		}


		public void AddTemporaryMaxEntitleDay(DateTime date)
		{
			var lstRequest = _requestRepository.GetMulti(x => x.StartDate <= date.Date && date.Date <= x.EndDate && (x.RequestStatusId == CommonConstants.StatusApprovedID || x.RequestStatusId == CommonConstants.StatusPendingID || x.RequestStatusId == CommonConstants.StatusDelegateID) && x.EntitleDayId == 1).ToList();
			if (lstRequest.Count > 0)
			{
				foreach (var request in lstRequest)
				{
					Entitleday_AppUser entitledayAppUser = _entitleDayAppUserRepository.GetMulti(x => x.UserId == request.UserId).Where(z => z.EntitleDayId == 1).FirstOrDefault();
					if (request.RequestTypeId == 1)
					{
						entitledayAppUser.TemporaryMaxEntitleDay += 1;
					}
					if (request.RequestTypeId == 2 || request.RequestTypeId == 3)
					{
						entitledayAppUser.TemporaryMaxEntitleDay += (float)0.5;
					}
					_entitleDayAppUserRepository.Update(entitledayAppUser);
					Save();
				}
			}
		}

		public void SubTemporaryMaxEntitleDay(DateTime date)
		{
			var lstRequest = _requestRepository.GetMulti(x => x.StartDate <= date.Date && date.Date <= x.EndDate && (x.RequestStatusId == CommonConstants.StatusApprovedID || x.RequestStatusId == CommonConstants.StatusPendingID || x.RequestStatusId == CommonConstants.StatusDelegateID) && x.EntitleDayId == 1).ToList();
			if (lstRequest.Count > 0)
			{
				foreach (var request in lstRequest)
				{
					Entitleday_AppUser entitledayAppUser = _entitleDayAppUserRepository.GetMulti(x => x.UserId == request.UserId).Where(z => z.EntitleDayId == 1).FirstOrDefault();
					if (request.RequestTypeId == 1)
					{
						entitledayAppUser.TemporaryMaxEntitleDay -= 1;
					}
					if (request.RequestTypeId == 2 || request.RequestTypeId == 3)
					{
						entitledayAppUser.TemporaryMaxEntitleDay -= (float)0.5;
					}
					_entitleDayAppUserRepository.Update(entitledayAppUser);
					Save();
				}
			}

		}

		public void HandleUpdateHoliday_EntitleDay(Holiday holiday, Holiday existHoliday)
		{
			if (holiday.Date.Date != existHoliday.Date.Date)
			{
				AddTemporaryMaxEntitleDay(existHoliday.Date);
				SubTemporaryMaxEntitleDay(holiday.Date);
				UpdateDateToOtherDay(holiday, existHoliday);
			}

			if (holiday.Workingday.HasValue && existHoliday.Workingday.HasValue && holiday.Workingday.Value.Date != existHoliday.Workingday.Value.Date)
			{
				AddTemporaryMaxEntitleDay(holiday.Workingday.Value);
				SubTemporaryMaxEntitleDay(existHoliday.Workingday.Value);
				UpdateWorkingDayToOtherDay(holiday, existHoliday);
				
			}
			else if (!existHoliday.Workingday.HasValue && holiday.Workingday.HasValue)
			{
				AddTemporaryMaxEntitleDay(holiday.Workingday.Value);
				UpdateNewWorkingDay(holiday);
			}
			else if (existHoliday.Workingday.HasValue && !holiday.Workingday.HasValue)
			{
				UpdateTypeToWeekend(existHoliday);
				SubTemporaryMaxEntitleDay(existHoliday.Workingday.Value);
			}
		}

		public void HandleDeleteHoliday(Holiday holiday)
		{
			if (holiday.Workingday.HasValue)
			{
				SubTemporaryMaxEntitleDay(holiday.Workingday.Value);
				UpdateTypeToWeekend(holiday);
			}
			AddTemporaryMaxEntitleDay(holiday.Date);
			HandleRejectOTInTimeDay(holiday);

		}

		void UpdateWorkingDayToOtherDay(Holiday holiday, Holiday existHoliday)
		{
			UpdateTypeToWeekend(existHoliday);
			HandleRejectOTNormalAndUpdateType(holiday);

		}

		void UpdateTypeToWeekend(Holiday existHoliday)
		{
			var lstExistHolidayOT = _otRequestService.GetAllOTRequestByGeneralAdmin(null, null, null, true, null).Where(x => x.OTDate == existHoliday.Workingday).Where(x => x.OTTimeTypeID == CommonConstants.OTTimeTypeNight);
			foreach (var ot in lstExistHolidayOT)
			{
				var item = _otRequestService.GetById(ot.ID);
				item.OTDateTypeID = CommonConstants.OTDateTypeWeekend;
				_otRequestService.UpdateOTRequest(item);
			}
		}

		void HandleRejectOTNormalAndUpdateType(Holiday holiday)
		{
			var lstOT = _otRequestService.GetAllOTRequestByGeneralAdmin(null, null, null, true, null).Where(x => x.OTDate == holiday.Workingday.Value.Date);
			var lstOTNormal = lstOT.Where(x => x.OTTimeTypeID == CommonConstants.OTTimeTypeNormal);
			var lstOTNight = lstOT.Where(x => x.OTTimeTypeID == CommonConstants.OTTimeTypeNight);
			foreach (var ot in lstOTNormal)
			{
				var item = _otRequestService.GetById(ot.ID);
				item.StatusRequestID = _statusRequestService.getIDbyName(CommonConstants.StatusCancelled).ID;
				_otRequestService.UpdateOTRequest(item);
			}
			foreach (var ot in lstOTNight)
			{
				var item = _otRequestService.GetById(ot.ID);
				item.OTDateTypeID = CommonConstants.OTDateTypeNormal;
				_otRequestService.UpdateOTRequest(item);
			}
		}

		void UpdateNewWorkingDay(Holiday holiday)
		{
			HandleRejectOTNormalAndUpdateType(holiday);
		}

		void UpdateOTRequestType(DateTime date)
		{
			var lstholidayOT = _otRequestService.GetAllOTRequestByGeneralAdmin(null, null, null, true, null).Where(x => x.OTDate == date && x.OTTimeTypeID == CommonConstants.OTTimeTypeNight);
			foreach (var ot in lstholidayOT)
			{
				var item = _otRequestService.GetById(ot.ID);
				item.OTDateTypeID = CommonConstants.OTDateTypeHoliDay;
				_otRequestService.UpdateOTRequest(item);
			}
		}

		void UpdateDateToOtherDay(Holiday holiday, Holiday existHoliday)
		{
			UpdateOTRequestType(holiday.Date.Date);
			HandleRejectOTInTimeDay(existHoliday);
		}

		void HandleRejectOTInTimeDay(Holiday holiday)
		{
			var lstOT = _otRequestService.GetAllOTRequestByGeneralAdmin(null, null, null, true, null).Where(x => x.OTDate.Value.Date == holiday.Date.Date);
			var lstOTNormal = lstOT.Where(x => x.OTTimeTypeID == CommonConstants.OTTimeTypeNormal);
			var lstOTNight = lstOT.Where(x => x.OTTimeTypeID == CommonConstants.OTTimeTypeNight);
			foreach (var ot in lstOTNormal)
			{
				var item = _otRequestService.GetById(ot.ID);
				item.StatusRequestID = _statusRequestService.getIDbyName(CommonConstants.StatusCancelled).ID;
				_otRequestService.UpdateOTRequest(item);
			}
			foreach (var ot in lstOTNight)
			{
				var item = _otRequestService.GetById(ot.ID);
				item.OTDateTypeID = CommonConstants.OTDateTypeNormal;
				_otRequestService.UpdateOTRequest(item);
			}
		}
	}
}
