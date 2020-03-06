using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Common.Constants;
using TMS.Common.Exceptions.Extensions;
using TMS.Common.ViewModels;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Model.Models;

namespace TMS.Service
{
    public interface IAbnormalCaseService
    {
        IEnumerable<AbnormalCaseModel> GetAllFilter(string userID, string groupId, FiterAbnormalViewModel filter);
        IEnumerable<AbnormalCaseModel> MapAbsentField(IEnumerable<AbnormalCaseModel> model);
        IEnumerable<AbnormalCase> GetAll(string userId, string groupId);
        AbnormalCase GetById(int id);
        IEnumerable<AbnormalCase> GetAbnormalById(int id);
        IEnumerable<AbnormalCase> GetListProduct(string keyword);
        IEnumerable<AbnormalChartModel> AbnormalChart(List<string> ListGroupId,DateTime a,DateTime b);
        IEnumerable<AbnormalChartPercentModel> AbnormalChartPercent();
        int AbnormalByUser(string userID);
        List<AbnormalCaseModel> GetAbnormalViewModel(string userId, string groupId, FiterAbnormalViewModel filter);
    }
    public class AbnormalCaseService : IAbnormalCaseService
    {
        private IAbnormalCaseRepository _abnormalCaseRepository;
        private IAbnormalReasonRepository _abnormalReasonRepository;
        private IUnitOfWork _unitOfWork;
        private IExplanationRequestRepository explanationRequestRepository;

        public AbnormalCaseService(IAbnormalCaseRepository AbnormalCaseRepository, IExplanationRequestRepository explanationRequestRepository,
            IUnitOfWork unitOfWork, IAbnormalReasonRepository abnormalReasonRepository)
        {
            this.explanationRequestRepository = explanationRequestRepository;
            this._abnormalCaseRepository = AbnormalCaseRepository;
            this._abnormalReasonRepository = abnormalReasonRepository;
            this._unitOfWork = unitOfWork;
        }/// <summary>
        /// Get all AbnormalCase after fiter
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="groupId"></param>
        /// <param name="filter"></param>
        /// <returns>list abnormalCase</returns>
        public IEnumerable<AbnormalCaseModel> GetAllFilter(string userID, string groupId, FiterAbnormalViewModel filter)
        {
            var model = _abnormalCaseRepository.GetAllAbnormal(userID);
            if (filter != null)
            {
                if (filter.AbnormalReasonTypeFilter.Count() != 0)
                {
                    if (filter.AbnormalReasonTypeFilter.Contains(StringConstants.TimeSheetAbsentMorning) ||
                    filter.AbnormalReasonTypeFilter.Contains(StringConstants.TimeSheetAbsentAfternoon) ||
                    filter.AbnormalReasonTypeFilter.Contains(StringConstants.TimeSheetAbsent))
                    {
                        model = model.Where(x => (filter.AbnormalReasonTypeFilter.Contains(x.Absent)));
                    }
                }
                if (filter.StatusRequestsss.Count() != 0)
                {
                    model = model.Where(x => filter.StatusRequestsss.Contains(x.StatusRequest));
                }
                if (filter.FullName.Count() != 0)
                {
                    model = model.Where(x => filter.FullName.Contains(x.UserName));
                }
                if (filter.AbnormalReason.Count() != 0)
                {
                    model = model.Where(x => x.AbnormalReason.Any(y => filter.AbnormalReason.Contains(y.Name)));
                }
                if (!string.IsNullOrEmpty(filter.StartDate) && !string.IsNullOrEmpty(filter.EndDate))
                {
                    DateTime dtFrom = DateTime.ParseExact(filter.StartDate, CommonConstants.FORMAT_DATE_MMDDYYYY, CultureInfo.InvariantCulture); 
                    DateTime dtTo = DateTime.ParseExact(filter.EndDate, CommonConstants.FORMAT_DATE_MMDDYYYY, CultureInfo.InvariantCulture);

                    model = model.Where(x => (x.AbnormalDate.Value>= dtFrom) && (x.AbnormalDate.Value <= dtTo));
                }
                if (!string.IsNullOrEmpty(filter.StartDate) && string.IsNullOrEmpty(filter.EndDate))
                {
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Tuesday)
                    {
                        var Date = DateTime.Now.AddDays(-4).Date;
                        model = model.Where(x => x.AbnormalDate.Value >= Date && string.IsNullOrEmpty(x.StatusRequest));
                    }
                    else
                    {
                        var Date = DateTime.Now.AddDays(-2).Date;
                        model = model.Where(x => x.AbnormalDate.Value>= Date && string.IsNullOrEmpty(x.StatusRequest));
                    }
                    
                }
            }
            //foreach (var item in model)
            //{
            //    if (item.CheckIn == "null" || item.CheckOut == "null" || string.IsNullOrEmpty(item.CheckIn) || string.IsNullOrEmpty(item.CheckOut))
            //    {
            //        item.Workingtime = 0;
            //    }
            //    else
            //    {
            //        item.Workingtime = Math.Round((Convert.ToDouble((TimeSpan.Parse(item.CheckOut)).TotalHours - TimeSpan.Parse(item.CheckIn).TotalHours)), 2);
            //    }
            //}
            return model;
        }
        /// <summary>
        /// Maping absent field
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IEnumerable<AbnormalCaseModel> MapAbsentField(IEnumerable<AbnormalCaseModel> model)
        {
            foreach (var item in model)
            {
                if (item.Absent == StringConstants.TimeSheetAbsentMorning)
                {
                    item.Absent = StringConstants.AbsentMorning;
                }
                if (item.Absent == StringConstants.TimeSheetAbsentAfternoon)
                {
                    item.Absent = StringConstants.AbsentAfternoon;
                }
                if (item.Absent == StringConstants.TimeSheetAbsent)
                {
                    item.Absent = StringConstants.Absent;
                }
            }
            return model;
        }
        public IEnumerable<AbnormalCase> GetAll(string userId, string groupId)
        {
            if (_abnormalCaseRepository.IsReadAll(userId, CommonConstants.FunctionAbnormalCase))
            {
                return _abnormalCaseRepository.GetMulti(x => x.FingerTimeSheet.FingerMachineUsers.AppUser.GroupId.ToString().Equals(groupId),
                    new string[] {CommonConstants.AbnormalReason,
                    CommonConstants.FingerTimeSheet,CommonConstants.FingerMachineUser, CommonConstants.TimeSheetAppUser, CommonConstants.TimeSheetAppUserGroup
                }).OrderByDescending(x => x.ID);
            }
            return _abnormalCaseRepository.GetMulti(x => x.FingerTimeSheet.FingerMachineUsers.AppUser.Id.Equals(userId), new string[] {
               CommonConstants.AbnormalReason, CommonConstants.FingerTimeSheet,CommonConstants.FingerMachineUser, CommonConstants.TimeSheetAppUser, CommonConstants.TimeSheetAppUserGroup
            }).OrderByDescending(x => x.ID);
        }
        public IEnumerable<AbnormalCase> GetAbnormalById(int id)
        {
            return _abnormalCaseRepository.GetAbnormalById(id);
        }
        public AbnormalCase GetById(int id)
        {
            return _abnormalCaseRepository.GetSingleByCondition(x => x.ID.Equals(id), new string[] {
                CommonConstants.AbnormalReason,
                CommonConstants.FingerTimeSheet,CommonConstants.FingerMachineUser, CommonConstants.TimeSheetAppUser, CommonConstants.TimeSheetAppUserGroup});
        }
        public IEnumerable<AbnormalCase> GetListProduct(string keyword)
        {
            IEnumerable<AbnormalCase> query;
            if (!string.IsNullOrEmpty(keyword))
                query = _abnormalCaseRepository.GetMulti(x => x.FingerTimeSheet.FingerMachineUsers.AppUser.FullName.Contains(keyword));
            else
                query = _abnormalCaseRepository.GetAll();
            return query;
        }

        public IEnumerable<AbnormalChartModel> AbnormalChart(List<string> ListGroupId,DateTime StartDate, DateTime EndDate)
         {
            var data = _abnormalCaseRepository.GetDataAbnormal();
            //var model = _abnormalCaseRepository.GetAll(new string[] { CommonConstants.FingerTimeSheet });
            if (ListGroupId.Count() != 0 )
            {
                data = data.Where(x => ListGroupId.Contains(x.GroupId.ToString()));
            }
            //var EndDate = DateTime.Today;
            ////var StartDate = EndDate.AddDays(-(int)EndDate.DayOfWeek);
            //var StartDate = EndDate.AddDays(-7);
            var listDate = new List<DateTime>();
            //var listDate = AllDatesInWeek(); /*_abnormalCaseRepository.GetMulti(x => x.FingerTimeSheet.DayOfCheck >= StartDate && x.FingerTimeSheet.DayOfCheck <= EndDate,new string[] { CommonConstants.FingerTimeSheet }).Select(y => y.FingerTimeSheet.DayOfCheck).Distinct();*/
            for (DateTime i = StartDate; i <= EndDate; i = i.AddDays(1))
            {
                listDate.Add(i);
            }
            List<AbnormalChartModel> ListabnormalChart = new List<AbnormalChartModel>();
            foreach (var item in listDate)
            {
                AbnormalChartModel abnormalChart = new AbnormalChartModel()
                {
                    AbnormalDate = item,
                    LateComing = data.Count(x => x.FingerTimeSheet == item && x.ReasonId == CommonConstants.UnauthorizedLateComing),
                    EarlyLeaving = data.Count(x => x.FingerTimeSheet == item && x.ReasonId == CommonConstants.UnauthorizedEarlyLeaving),
                    UnauthorizedLeave = data.Count(x => x.FingerTimeSheet == item && x.ReasonId == CommonConstants.UnauthorizedLeave),

                    UnusedAuthorizedEarlyLeaving = data.Count(x => x.FingerTimeSheet == item && x.ReasonId == CommonConstants.UnusedAuthorizedEarlyLeaving),
                    UnusedAuthorizedLateComing = data.Count(x => x.FingerTimeSheet == item && x.ReasonId == CommonConstants.UnusedAuthorizedLateComing),
                    UnusedAuthorizedLeave = data.Count(x => x.FingerTimeSheet == item && x.ReasonId == CommonConstants.UnusedAuthorizedLeave),
                    OTWithoutCheckIn = data.Count(x => x.FingerTimeSheet == item && x.ReasonId == CommonConstants.OTWithoutCheckIn),
                    OTWithoutCheckOut = data.Count(x => x.FingerTimeSheet == item && x.ReasonId == CommonConstants.OTWithoutCheckOut),
                    OTWithoutCheckInOut = data.Count(x => x.FingerTimeSheet == item && x.ReasonId == CommonConstants.OTWithoutCheckInOut),

                };
                    ListabnormalChart.Add(abnormalChart);
            }

            return ListabnormalChart;
        }
        public IEnumerable<AbnormalChartPercentModel> AbnormalChartPercent()
        {
            var reasonType = _abnormalReasonRepository.GetAll();
            var DateNow = DateTime.Now;
            var firstDayOfMonth = new DateTime(DateNow.Year, DateNow.Month, 1);
            var abnomarData = _abnormalCaseRepository.GetAbnormalChart();
            var model = _abnormalCaseRepository.GetMulti(x => x.FingerTimeSheet.DayOfCheck > firstDayOfMonth && x.FingerTimeSheet.DayOfCheck < DateNow ,  new string[] { CommonConstants.AbnormalReason , CommonConstants.FingerTimeSheet });
            var total = model.Count();
            List<AbnormalChartPercentModel> ListabnormalChartPercent = new List<AbnormalChartPercentModel>();
            foreach (var item in reasonType) {
                AbnormalChartPercentModel abnormalchartpercent = new AbnormalChartPercentModel()
                {
                    ReasonType = item.Name,
                    ReasonTypeID = item.ID,
                    Percentage = model.Count(x => x.ReasonId == item.ID),
                    ApprovePercent = (abnomarData.Count(a => a.ReasonID == item.ID && a.StatusRequestID == 4)) == 0 ? 0: Math.Round(((double)(abnomarData.Count(a => a.ReasonID == item.ID && a.StatusRequestID == 4)) / (double)(model.Count(x => x.ReasonId == item.ID))) * 100,1),
                    RejectPercent = (abnomarData.Count(a => a.ReasonID == item.ID && a.StatusRequestID == 2)) == 0 ? 0: Math.Round(((double)(abnomarData.Count(a => a.ReasonID == item.ID && a.StatusRequestID == 2)) / (double)(model.Count(x => x.ReasonId == item.ID))) * 100,1) 
                };
                ListabnormalChartPercent.Add(abnormalchartpercent);
            }
            return ListabnormalChartPercent;
        }

        public static IEnumerable<DateTime> AllDatesInWeek()
        {
            var EndDate = DateTime.Today.AddDays(-1);
            for (int i = 0; i <= 7; i++)
            {
                yield return EndDate.AddDays(-i);
            }
        }
        public int AbnormalByUser(string userID)
        {
            if(DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Tuesday)
            {
                var Date = DateTime.Now.AddDays(-4).Date;
                return GetAbnormalViewModel(userID, null, null).Count(x => string.IsNullOrEmpty(x.StatusRequest) && x.AbnormalDate >= Date);
            }
            else
            {
                var Date = DateTime.Now.AddDays(-2).Date;
                return GetAbnormalViewModel(userID, null, null).Count(x => string.IsNullOrEmpty(x.StatusRequest) && x.AbnormalDate >= Date);
            }
        }

        /// <summary>
        /// Get abnormal case map model
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<AbnormalCaseModel> GetAbnormalViewModel(string userId, string groupId, FiterAbnormalViewModel filter)
        {
            List<AbnormalCaseModel> listModel = new List<AbnormalCaseModel>();

            //var model = _abnormalCaseService.GetAll(userId, groupId);
            var model = GetAllFilter(userId, groupId, filter);
            var id = 0;
            foreach (var item in model)
            {
                if (item.TimeSheetID != id)
                {
                    listModel.Add(new AbnormalCaseModel
                    {
                        ID = item.ID,
                        UserName = item.UserName,
                        FullName = item.FullName,
                        GroupName = item.GroupName,
                        AbnormalDate = item.AbnormalDate,
                        Absent = item.Absent,
                        CheckIn = item.CheckIn,
                        CheckOut = item.CheckOut,
                        Workingtime = item.Workingtime,
                        AbnormalReason = item.AbnormalReason,
                        Approver = item.Approver,
                        StatusRequest = item.StatusRequest
                    });
                }
                id = item.TimeSheetID;
            }
            return listModel;
        }
    }
}
