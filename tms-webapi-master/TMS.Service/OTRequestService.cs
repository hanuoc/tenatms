using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using TMS.Common.Constants;
using TMS.Common.Exceptions.Extensions;
using TMS.Common.ViewModels;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Model.Models;
using TMS.Web.Models.Common;

namespace TMS.Service
{
    public interface IOTRequestService
    {
        OTRequest Add(OTRequest otRequest, string userID);
        void UpdateOTRequest(OTRequest oTRequest);
        OTRequest ChangeStatus(int otrequestID, string action,string userId);
        string ChangeStatusMulti(int[] otrequestID, string action);
        IEnumerable<string> GetCreatedByOTRequest(string groupID);
        OTRequest GetById(int id);
        IEnumerable<OTRequest> GetOTRequestFilter(string userID, string groupId, string column, bool isDesc, FilterOTRequestModel filter);
        IEnumerable<OTRequestChartModel> OTRequestChart(int groupID);
        IEnumerable<OTRequestChartModel> OTRequestChartByUser(string userID);
        string CheckConditionCreate(int OTDateTypeID, int OTTimeTypeID, string StartTime, string EndTime);
        OTRequest GetByTimeSheet(DateTime otDate, string user);
        int OTRequestByUser(string userID);
        bool CheckOtRequestUser(OTRequest otRequest, List<string> otRequestUser);
        bool CheckCreateDay(OTRequest oTRequest);
        IEnumerable<OTRequest> GetAllOTRequestByGeneralAdmin(string userID, string groupId, string column, bool isDesc, FilterOTRequestModel filter);

    }
    public class OTRequestService : IOTRequestService
    {
        private IOTRequestRepository _otrequestRepository;
        private IStatusRequestRepository _statusRequestRepository;
        private IOTRequestUserRepository _oTRequestUserRepository;
        private IUnitOfWork _unitOfWork;
        /// <summary>
        /// Constructor OT Request Service
        /// </summary>
        /// <param name="otrequestRepository"></param>
        /// <param name="unitOfWork"></param>
        public OTRequestService(IOTRequestUserRepository oTRequestUserRepository, IOTRequestRepository otrequestRepository, IStatusRequestRepository statusRequestRepository, IUnitOfWork unitOfWork)
        {
            this._otrequestRepository = otrequestRepository;
            _statusRequestRepository = statusRequestRepository;
            _oTRequestUserRepository = oTRequestUserRepository;
            this._unitOfWork = unitOfWork;
        }
        /// <summary>
        /// Get All With User or Group 
        /// Check if read all is true get all record in group else get only record of user with userID
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="groupId"></param>
        /// <returns>IEnumerable OTRequest</returns>
        private IEnumerable<OTRequest> GetAllOTRequestByUserOrGroup(string userID, string groupId)
        {
            if (_otrequestRepository.IsReadAll(userID,CommonConstants.FunctionOTRequest))
            {
                return _otrequestRepository.GetMulti(x => x.AppUserCreatedBy.GroupId.ToString().Equals(groupId), new string[] { CommonConstants.OTDateType, CommonConstants.OTTimeType, CommonConstants.AppUserCreateByGroup, CommonConstants.StatusRequest });
            }
            return _otrequestRepository.GetMulti(x => x.OTRequestUser.Any(y => y.UserID.Equals(userID)) || x.AppUserCreatedBy.Id.Equals(userID), new string[] { CommonConstants.OTDateType, CommonConstants.OTTimeType, CommonConstants.AppUserCreateByGroup, CommonConstants.StatusRequest, CommonConstants.OTRequestUser, CommonConstants.AppUserUpdatedBy }).OrderByDescending(x => x.CreatedDate);
        }
        private IEnumerable<OTRequest> GetAllOTRequestGeneralAdmin()
        {
            return _otrequestRepository.GetMulti(x=>true , new string[] { CommonConstants.OTDateType, CommonConstants.OTTimeType, CommonConstants.AppUserCreateByGroup, CommonConstants.StatusRequest, CommonConstants.OTRequestUser, CommonConstants.AppUserUpdatedBy }).OrderByDescending(x => x.CreatedDate);
            //if (_otrequestRepository.IsReadAll(userID, CommonConstants.FunctionOTRequest))
            //{
            //    return _otrequestRepository.GetMulti(x => x.AppUserCreatedBy.GroupId.ToString().Equals(groupId), new string[] { CommonConstants.OTDateType, CommonConstants.OTTimeType, CommonConstants.AppUserCreateByGroup, CommonConstants.StatusRequest });
            //}
            //return _otrequestRepository.GetMulti(x => x.OTRequestUser.Any(y => y.UserID.Equals(userID)) || x.AppUserCreatedBy.Id.Equals(userID), new string[] { CommonConstants.OTDateType, CommonConstants.OTTimeType, CommonConstants.AppUserCreateByGroup, CommonConstants.StatusRequest, CommonConstants.OTRequestUser, CommonConstants.AppUserUpdatedBy }).OrderByDescending(x => x.CreatedDate);
        }
        /// <summary>
        /// Get OT Request By Filter condition 
        /// 
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="groupId">Group ID</param>
        /// <param name="column">Colunm want to sort</param>
        /// <param name="isDesc">Asc or Desc</param>
        /// <param name="filter">Filter Condition</param>
        /// <returns>OT Request List</returns>
        public IEnumerable<OTRequest> GetOTRequestFilter(string userID, string groupId, string column, bool isDesc, FilterOTRequestModel filter)
        {   
            var model = GetAllOTRequestByUserOrGroup(userID, groupId);
            if (filter != null)
            {
                if (filter.StatusRequestType.Count() != 0) // Filter by Status Request ID
                {
                    model = model.Where(x => filter.StatusRequestType.Contains(x.StatusRequestID.ToString()));
                }
                if(filter.FullName.Count() != 0)
                {
                    model = model.Where(x => filter.FullName.Contains(x.AppUserCreatedBy.Id)); 
                }
                if (filter.OTDateType.Count() != 0) // Filter by OTDate Type ID
                {
                    model = model.Where(x => filter.OTDateType.Contains(x.OTDateTypeID.ToString()));
                }
                if (filter.OTTimeType.Count() != 0)
                {
                    model = model.Where(x => filter.OTTimeType.Contains(x.OTTimeTypeID.ToString()));
                }
                if (!string.IsNullOrEmpty(filter.startDate) && !string.IsNullOrEmpty(filter.endDate))
                {
                    model = model.Where(x => (x.OTDate.Value.Date >= DateTime.ParseExact(filter.startDate,CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture)) && 
                                             (x.OTDate.Value.Date <= DateTime.ParseExact(filter.endDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture)));
                }
            }
            if (column != null)
            {
                return model.OrderByField(column, isDesc);
            }
            return model;
        }
        //tuong test
        public IEnumerable<OTRequest> GetAllOTRequestByGeneralAdmin(string userID, string groupId, string column, bool isDesc, FilterOTRequestModel filter)
        {
            var model = GetAllOTRequestGeneralAdmin();
            if (filter != null)
            {
                if (filter.StatusRequestType.Count() != 0) // Filter by Status Request ID
                {
                    model = model.Where(x => filter.StatusRequestType.Contains(x.StatusRequestID.ToString()));
                }
                if (filter.FullName.Count() != 0)
                {
                    model = model.Where(x => filter.FullName.Contains(x.AppUserCreatedBy.Id));
                }
                if (filter.OTDateType.Count() != 0) // Filter by OTDate Type ID
                {
                    model = model.Where(x => filter.OTDateType.Contains(x.OTDateTypeID.ToString()));
                }
                if (filter.OTTimeType.Count() != 0)
                {
                    model = model.Where(x => filter.OTTimeType.Contains(x.OTTimeTypeID.ToString()));
                }
                if (!string.IsNullOrEmpty(filter.startDate) && !string.IsNullOrEmpty(filter.endDate))
                {
                    model = model.Where(x => (x.OTDate.Value.Date >= DateTime.ParseExact(filter.startDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture)) &&
                                             (x.OTDate.Value.Date <= DateTime.ParseExact(filter.endDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture)));
                }
            }
            if (column != null)
            {
                return model.OrderByField(column, isDesc);
            }
            return model;
        }
        /// <summary>
        /// Add OT request into database
        /// </summary>
        /// <param name="otRequest"></param>
        /// <param name="userID"></param>
        /// <returns>OTReuqest</returns>
        public OTRequest Add(OTRequest otRequest, string userID)
        {
            try {
                otRequest.StatusRequestID = _statusRequestRepository.GetAll().FirstOrDefault(x => x.Name.Equals(CommonConstants.StatusPending)).ID;
                otRequest.CreatedDate = DateTime.Now;
                otRequest.CreatedBy = userID;
                var OTRequest = _otrequestRepository.Add(otRequest);
                Save();
                return OTRequest;
            }
            catch (DbUpdateException e)
            {
                return null;
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
        }
        public bool CheckCreateDay(OTRequest oTRequest)
        {
            DateTime dateNow = DateTime.Now;
            if ((TimeSpan.ParseExact(oTRequest.StartTime, @"hh\:mm", CultureInfo.InvariantCulture)) < (TimeSpan.ParseExact(dateNow.ToString("HH:mm"), @"hh\:mm", CultureInfo.InvariantCulture)))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool CheckOtRequestUser(OTRequest otRequest,List<string> otRequestUser)
        {
            var lstOTRequestUser = _oTRequestUserRepository.GetMulti(x=>x.OTRequest.OTDate==otRequest.OTDate&&x.OTRequest.StatusRequest.Name!=CommonConstants.StatusCancelled).Select(x=>x.UserID).ToList();
            var lstOTRequest = _otrequestRepository.GetMulti(x => x.OTDate == otRequest.OTDate&&x.StatusRequest.Name!= CommonConstants.StatusCancelled);
            lstOTRequestUser.AddRange(lstOTRequest.Select(x => x.CreatedBy));
            lstOTRequestUser=  lstOTRequestUser.Distinct().ToList();
            otRequestUser = otRequestUser.Distinct().ToList();
            if (otRequestUser.Any(x => lstOTRequestUser.Contains(x)))
                return false;
            else
                return true;
        }
        private List<string> GetUserHasChanged(List<string> lstuser,List<string> lstUserChanged)
        {
            foreach (var user in lstuser)
            {
                lstUserChanged.Remove(user);
            }
            return lstUserChanged;
        }
        /// <summary>
        /// Commit to database
        /// </summary>
        private void Save()
        {
            _unitOfWork.Commit();
        }
        /// <summary>
        /// Get OT Reuqest by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OTRequest GetById(int id)
        {
            return _otrequestRepository.GetSingleByCondition(x => x.ID.Equals(id), new string[] {CommonConstants.AppUserCreateByGroup, CommonConstants.StatusRequest ,CommonConstants.AppUserUpdatedBy, CommonConstants.OTRequestUser });
        }
        /// <summary>
        /// Change Status Request of otrequest ID and action
        /// </summary>
        /// <param name="otrequestID"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public OTRequest ChangeStatus(int otrequestID, string action,string userId)
        {
            try
            {
                var model = GetById(otrequestID);
                var status = _statusRequestRepository.GetMulti(x => x.Name.Equals(action)).FirstOrDefault();
                model.StatusRequestID = status.ID;
                model.UpdatedBy = userId;
                model.UpdatedDate = DateTime.Now;
                _otrequestRepository.Update(model);
                Save();
                return model;
            }
            catch (NullReferenceException ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Get Created By of User
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetCreatedByOTRequest(string groupID)
        {
            var model = _otrequestRepository.GetMulti(x => x.AppUserCreatedBy.GroupId.ToString().Equals(groupID), new string[] { CommonConstants.AppUserCreateByGroup }).Select(x => x.AppUserCreatedBy.FullName).Distinct();
            //var model = _otrequestRepository.GetMulti(x => x.AppUserCreatedBy.GroupId.ToString().Equals(groupID), new string[] { CommonConstants.AppUserCreateByGroup }).Distinct();
            return model;
        }
        /// <summary>
        /// Change Status Request of otrequest ID and action
        /// </summary>
        /// <param name="otrequestID"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public string ChangeStatusMulti(int[] otrequestID, string action)
        {
            var dateNow = DateTime.Now.Date;
                foreach(var item in otrequestID)
                {
                    var model = GetById(item);
                if (model.OTDate.Value.AddDays(2) < dateNow)
                {
                    return MessageSystem.ERROR_CHANGESTATUS_OTREQUEST_IN_PAST_NOT_MSG;
                }
                var status = _statusRequestRepository.GetMulti(x => x.Name.Equals(action)).FirstOrDefault();
                    model.StatusRequestID = status.ID;
                    _otrequestRepository.Update(model);
                }
                Save();
            return null;
        }

        public string CheckConditionCreate(int OTDateTypeID, int OTTimeTypeID,string StartTime, string EndTime)
        {
            if (OTDateTypeID.Equals(CommonConstants.OTDateTypeNormal))
            {
                if (OTTimeTypeID.Equals(CommonConstants.OTTimeTypeNormal))
                {
                    return MessageSystem.NormalDayNormalTime;
                } 
                if(TimeSpan.Parse(StartTime) < TimeSpan.FromHours(CommonConstants.HourForOTTimeNightStart) ||
                    TimeSpan.Parse(EndTime) > TimeSpan.FromHours(CommonConstants.HourForOTTimeNightEnd))
                {
                    return MessageSystem.NormalDayNightTimePick;
                }
            }
            if (OTDateTypeID.Equals(CommonConstants.OTDateTypeWeekend) || OTDateTypeID.Equals(CommonConstants.OTDateTypeHoliDay))
            {
                if (OTTimeTypeID.Equals(CommonConstants.OTTimeTypeNormal))
                {
                    if (TimeSpan.Parse(StartTime) >= TimeSpan.FromHours(CommonConstants.HourForOTNormalStart) &&
                            TimeSpan.Parse(EndTime) <= TimeSpan.FromHours(CommonConstants.HourForOTNormalEnd))
                    {
                        return null;
                    }
                    return MessageSystem.NormalDayNormalTimePick;
                }
                if (TimeSpan.Parse(StartTime) >= TimeSpan.FromHours(CommonConstants.HourForOTTimeNightStart) &&
                    TimeSpan.Parse(EndTime) <= TimeSpan.FromHours(CommonConstants.HourForOTTimeNightEnd))
                {
                    return null;
                }
                return MessageSystem.NormalDayNightTimePick;
            }
            return null;
        }
        public OTRequest GetByTimeSheet(DateTime otDate, string user)
        {
            return _otrequestRepository.GetMulti(x => x.OTRequestUser.Any(y => y.UserID.Equals(user))  && x.OTDate == otDate  , new string[] { CommonConstants.OTDateType, CommonConstants.OTTimeType, CommonConstants.AppUserCreateByGroup, CommonConstants.StatusRequest, CommonConstants.OTRequestUser, CommonConstants.AppUserUpdatedBy }).FirstOrDefault();
        }
        public IEnumerable<OTRequestChartModel> OTRequestChart(int groupID)
        {
            var model = _otrequestRepository.GetMulti(x => x.AppUserCreatedBy.GroupId == groupID, new string[] { CommonConstants.StatusRequest, CommonConstants.AppUserCreatedBy });
            double total = model.Count();
            double totaltemp = total;
            var allStatus = _statusRequestRepository.GetAll();
            List<OTRequestChartModel> ListOTRequestChart = new List<OTRequestChartModel>();
            double totalpercent = 100;
            foreach (var item in allStatus)
            {
                int amount = model.Count(x => x.StatusRequest.ID == item.ID);
                double rawPercent = totaltemp == 0 ? 0 : amount / totaltemp * totalpercent;
                totaltemp -= amount;
                double roundedPercent = Math.Round(rawPercent, 2);
                totalpercent -= roundedPercent;
                OTRequestChartModel requestChart = new OTRequestChartModel()
                {
                    TotalOTRequest = total,
                    StatusOTRequests = item.Name,
                    CountStatusOTRequest = model.Count(x => x.StatusRequest.ID == item.ID),
                    PercentStatusOTRequest = roundedPercent
                };
                ListOTRequestChart.Add(requestChart);
            }
            return ListOTRequestChart;
        }

        public IEnumerable<OTRequestChartModel> OTRequestChartByUser(string userID)
        {
            var model = _otrequestRepository.GetMulti(x => x.AppUserCreatedBy.Id == userID, new string[] { CommonConstants.StatusRequest, CommonConstants.AppUserCreatedBy });
            double total = model.Count();
            double totaltemp = total;
            var allStatus = _statusRequestRepository.GetAll();
            List<OTRequestChartModel> ListOTRequestChart = new List<OTRequestChartModel>();
            double totalpercent = 100;
            foreach (var item in allStatus)
            {
                int amount = model.Count(x => x.StatusRequest.ID == item.ID);
                double rawPercent = totaltemp == 0 ? 0 : amount / totaltemp * totalpercent;
                totaltemp -= amount;
                double roundedPercent = Math.Round(rawPercent, 2);
                totalpercent -= roundedPercent;
                OTRequestChartModel requestChart = new OTRequestChartModel()
                {
                    TotalOTRequest = total,
                    StatusOTRequests = item.Name,
                    CountStatusOTRequest = model.Count(x => x.StatusRequest.ID == item.ID),
                    PercentStatusOTRequest  = roundedPercent
                };
                ListOTRequestChart.Add(requestChart);
            }
            return ListOTRequestChart;
        }

        public int OTRequestByUser(string userID)
        {
            //var model = _otrequestRepository.GetMulti(x => x.OTRequestUser. == userID, new string[] { CommonConstants.AppUserCreatedBy }).Count();
            var model = _oTRequestUserRepository.GetMulti(x => x.UserID == userID).Count();
            return model;
        }

        public void UpdateOTRequest(OTRequest oTRequest)
        {
            _otrequestRepository.Update(oTRequest);
            Save();
        }
    }
}
