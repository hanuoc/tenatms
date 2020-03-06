using log4net;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using TMS.Common.Constants;
using TMS.Common.Exceptions.Extensions;
using TMS.Common.ViewModels;
using TMS.Data;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Model.Models;

namespace TMS.Service
{
    public interface IFingerTimeSheetService
    {
        bool ImportTimeSheet(out int success, TMSDbContext dbContext, out List<FingerTimeSheetTmpErrorModel> listFingerTimeSheetError);

        bool ReImportTimeSheet(out int success, TMSDbContext dbContext, out List<FingerTimeSheetTmpErrorModel> listFingerTimeSheetError);

        void Save();

        /// <summary>
        /// get list time sheet and fillter
        /// </summary>
        /// <param name="userID">id of logged in user</param>
        /// <param name="filter">filter timesheet</param>
        /// <returns>list time sheet model contains all information to view</returns>
        List<FingerTimeSheetModel> GetListTimeSheetFilter(string userID, FilterModel filter, string colunm, bool isAsc);

        /// <summary>
        /// Map data field Absent type
        /// </summary>
        /// <param name="model">data time sheet to mapping</param>
        /// <returns>list time sheet model</returns>
        List<FingerTimeSheetModel> MapAbsentField(List<FingerTimeSheetModel> model);
        bool IsUserNoExistTimeSheet(List<string> lstUserNo);
        FingerTimeSheet GetById(int Id);
        //IEnumerable<ReportModel> GetAllReport(string userId, FilterReport filter);
        IEnumerable<ReportModel> GetAllReportPaging(string userId, FilterReport filter, int page, int pageSize, ref int totalRow);
        Task GetAllReportEx(string userId, DateTime startDate, DateTime endDate);
        int CountUserReportEx(string userId);
    }

    public class FingerTimeSheetService : IFingerTimeSheetService
    {
        private IFingerTimeSheetRepository _fingerTimeSheetRepository;
        private IFingerTimeSheetTmpRepository _fingerTimeSheetTmpRepository;
        private ITimeDayRepository _timeDayRepository;
        private IFingerMachineUserRepository _fingerMachineUserRepository;
        private IOTRequestRepository _oTRequestRepository;
        private IOTRequestUserRepository _otrequestUserRepository;
        private IAppUserRepository _appUserRepository;
        private IChildcareLeaveRepository _childcareLeaveRepository;
        private IUnitOfWork _unitOfWork;
        private IReportRepository _reportRepository;
        private IRequestRepository _requestRepository;
        private IExplanationRequestRepository _explanationRepository;
        private IAbnormalCaseRepository _abnormalRepository;
        private IEntitleDayAppUserRepository _entitleDayRepository;
        private IUserOnsiteRepository _userOnsiteRepository;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ISystemService _systemservice;
        private ICommonService _commonService;
        public FingerTimeSheetService(IFingerTimeSheetRepository fingerTimeSheetRepository, IFingerTimeSheetTmpRepository fingerTimeSheetTmpRepository,
        ITimeDayRepository timeDayRepository, IOTRequestRepository oTRequestRepository, IFingerMachineUserRepository fingerMachineUserRepository,
        IOTRequestUserRepository otrequestUserRepository, IAppUserRepository appUserRepository, IChildcareLeaveRepository childcareLeaveRepository,
        IReportRepository reportRepository, IUnitOfWork unitOfWork, IRequestRepository requestRepository, IExplanationRequestRepository explanationRepository,
        IAbnormalCaseRepository abnormalRepository, IEntitleDayAppUserRepository entitleDayRepository, IUserOnsiteRepository userOnsiteRepository, ISystemService systemService,
            ICommonService commonService)
        {
            this._fingerTimeSheetRepository = fingerTimeSheetRepository;
            this._fingerTimeSheetTmpRepository = fingerTimeSheetTmpRepository;
            this._timeDayRepository = timeDayRepository;
            this._oTRequestRepository = oTRequestRepository;
            this._fingerMachineUserRepository = fingerMachineUserRepository;
            this._otrequestUserRepository = otrequestUserRepository;
            this._appUserRepository = appUserRepository;
            this._childcareLeaveRepository = childcareLeaveRepository;
            this._unitOfWork = unitOfWork;
            this._reportRepository = reportRepository;
            _requestRepository = requestRepository;
            this._explanationRepository = explanationRepository;
            this._abnormalRepository = abnormalRepository;
            this._entitleDayRepository = entitleDayRepository;
            this._systemservice = systemService;
            this._userOnsiteRepository = userOnsiteRepository;
            this._commonService = commonService;
        }

        public bool ImportTimeSheet(out int _countSuccess, TMSDbContext dbContext, out List<FingerTimeSheetTmpErrorModel> listFingerTimeSheetError)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            dbContext.Database.Connection.Open();
            dbContext.Database.CommandTimeout = CommonConstants.TimeExcuteSql;
            bool flag = false;
            string error = "";
            listFingerTimeSheetError = new List<FingerTimeSheetTmpErrorModel>();
            List<FingerTimeSheetTmp> listTimeSheetTmp = _fingerTimeSheetTmpRepository.GetAll().ToList();
            //list date from time sheet
            List<string> listDate = _fingerTimeSheetTmpRepository.GetAll().Select(x => x.Date.ToShortDateString()).Distinct().ToList();
            int count = 0;
            _countSuccess = 0;
            using (var dbTransaction = dbContext.Database.BeginTransaction())
            {
                foreach (var date in listDate)
                {
                    var shortDate = DateTime.Parse(date);
                    var otrequestAll = _oTRequestRepository.GetMulti(x => x.OTDate == shortDate.Date && x.StatusRequestID == CommonConstants.StatusApprovedID).ToList();
                    List<int> lstOTid = otrequestAll.Select(o => o.ID).ToList();
                    List<string> lstAllUserOT = _otrequestUserRepository.GetMulti(x => lstOTid.Contains(x.OTRequestID)).Select(x => x.UserID).ToList();
                    var listUserNo = listTimeSheetTmp.Where(x => x.Date.ToShortDateString() == date).Select(x => x.UserNo).Distinct().ToList();
                    var listUser = _fingerMachineUserRepository.GetMulti(x => listUserNo.Contains(x.ID)).Select(x => x.UserId).Distinct().ToList();
                    // timeDay include timeDay for DateOffset
                    var timeDay = _commonService.GetTimeDay(shortDate);
                    var isHolidayOrDayOff = _commonService.IsHolidayOrDayOff(shortDate);
                    foreach (var userID in listUser)
                    {
                        lstAllUserOT.Remove(userID);
                        #region Code
                        try
                        {
                            var lstUserNo = _fingerMachineUserRepository.GetMulti(x => x.UserId == userID).Select(x => x.ID);
                            if (lstUserNo.Count() == 0 || _userOnsiteRepository.isOnsite(userID, shortDate))
                                continue;
                            var listTimeSheetTmpUser = listTimeSheetTmp.Where(x => lstUserNo.Contains(x.UserNo) && x.Date.ToShortDateString() == date);
                            var otRequest = _oTRequestRepository.GetMulti(x => x.OTDate.Value == shortDate && x.StatusRequest.Name.Equals(CommonConstants.StatusApproved)
                                            && x.OTRequestUser.Where(y => y.UserID == userID).Count() > 0, new string[] { CommonConstants.OTRequestUser, CommonConstants.StatusRequest }).FirstOrDefault();
                            error = _fingerTimeSheetRepository.GetMulti(x => x.DayOfCheck == shortDate && lstUserNo.Contains(x.UserNo)).Count() > 0 ? MessageSystem.ErrorDuplicateData : string.Empty;
                            if (!string.IsNullOrEmpty(error))
                            {
                                FingerTimeSheetTmpErrorModel fingerTimeSheetTmpError = new FingerTimeSheetTmpErrorModel();
                                fingerTimeSheetTmpError.UserNo = CommonConstants.StringEmpty;
                                fingerTimeSheetTmpError.Date = date;
                                fingerTimeSheetTmpError.NumberFinger = listTimeSheetTmp.Where(x => lstUserNo.Contains(x.UserNo)).Select(x => x.NumberFinger.ToString()).FirstOrDefault();
                                fingerTimeSheetTmpError.UserName = listTimeSheetTmp.Where(x => lstUserNo.Contains(x.UserNo)).Select(x => x.AccName).Distinct().FirstOrDefault();
                                fingerTimeSheetTmpError.Error = error;
                                listFingerTimeSheetError.Add(fingerTimeSheetTmpError);
                                flag = false;
                                continue;
                            }
                            var timeSheetUserCheckIn = timeDay != null ? listTimeSheetTmpUser.Where(x => TimeSpan.Parse(x.Date.ToString(CommonConstants.FORMAT_HHMM)) <= TimeSpan.Parse(timeDay.CheckIn)).OrderByField(CommonConstants.ORDERBY_DATE, true).FirstOrDefault() : null;
                            timeSheetUserCheckIn = timeDay != null && timeSheetUserCheckIn == null ? listTimeSheetTmpUser.Where(x => TimeSpan.Parse(x.Date.ToString(CommonConstants.FORMAT_HHMM)) > TimeSpan.Parse(timeDay.CheckIn)
                                    && TimeSpan.Parse(x.Date.ToString(CommonConstants.FORMAT_HHMM)) < TimeSpan.Parse(CommonConstants.ABSENT_HOUR_AFTERNOON))
                                .OrderByField(CommonConstants.ORDERBY_DATE, true).FirstOrDefault() : timeSheetUserCheckIn;
                            FingerTimeSheetTmp timeSheetUserCheckOut = null;
                            TimeSpan OTCheckIn = TimeSpan.Zero;
                            TimeSpan OTCheckOut = TimeSpan.Zero;
                            if (isHolidayOrDayOff)
                            {
                                if (otRequest != null)
                                {
                                    otrequestAll.Remove(otRequest);
                                }
                            }
                            if (otRequest == null)
                            {
                                var list = listTimeSheetTmpUser.Where(x => TimeSpan.Parse(x.Date.ToString(CommonConstants.FORMAT_HHMM)) >= TimeSpan.Parse(timeDay.CheckOut)).OrderByField(CommonConstants.ORDERBY_DATE, false);
                                timeSheetUserCheckOut = timeDay != null ? listTimeSheetTmpUser.Where(x => TimeSpan.Parse(x.Date.ToString(CommonConstants.FORMAT_HHMM)) >= TimeSpan.Parse(timeDay.CheckOut)).OrderByField(CommonConstants.ORDERBY_DATE, false).FirstOrDefault() : null;

                            }
                            else
                            {
                                //get record userCheckout with case user register ot
                                timeSheetUserCheckOut = timeDay != null ? listTimeSheetTmpUser.Where(x => TimeSpan.Parse(x.Date.ToString(CommonConstants.FORMAT_HHMM)) >= TimeSpan.Parse(timeDay.CheckOut)
                                    && TimeSpan.Parse(x.Date.ToString(CommonConstants.FORMAT_HHMM)) < TimeSpan.Parse(otRequest.StartTime))
                                .OrderByField(CommonConstants.ORDERBY_DATE, true).FirstOrDefault() : null;
                                timeSheetUserCheckOut = timeDay != null && timeSheetUserCheckOut == null ? listTimeSheetTmpUser.Where(x => TimeSpan.Parse(x.Date.ToString(CommonConstants.FORMAT_HHMM)) < TimeSpan.Parse(timeDay.CheckOut)
                                    && TimeSpan.Parse(x.Date.ToString(CommonConstants.FORMAT_HHMM)) > TimeSpan.Parse(timeDay.CheckIn))
                                .OrderByField(CommonConstants.ORDERBY_DATE, false).FirstOrDefault() : timeSheetUserCheckOut;
                                var timeMidOT = TimeSpan.FromTicks(TimeSpan.Parse(otRequest.StartTime).Ticks + (TimeSpan.Parse(otRequest.EndTime) - TimeSpan.Parse(otRequest.StartTime)).Ticks / 2);


                                //Ot for dayoff
                                if (isHolidayOrDayOff)
                                {
                                    // only check in or check out
                                    if (listTimeSheetTmpUser.Count() == 1)
                                    {
                                        if (TimeSpan.Parse(listTimeSheetTmpUser.FirstOrDefault().Date.ToString(CommonConstants.FORMAT_HHMM)) <= TimeSpan.Parse(otRequest.StartTime))
                                        {
                                            OTCheckIn = TimeSpan.Parse(otRequest.StartTime);
                                        }
                                        else if (TimeSpan.Parse(listTimeSheetTmpUser.FirstOrDefault().Date.ToString(CommonConstants.FORMAT_HHMM)) > TimeSpan.Parse(otRequest.StartTime) && TimeSpan.Parse(listTimeSheetTmpUser.FirstOrDefault().Date.ToString(CommonConstants.FORMAT_HHMM)) <= timeMidOT)
                                        {
                                            OTCheckIn = TimeSpan.Parse(listTimeSheetTmpUser.FirstOrDefault().Date.ToString(CommonConstants.FORMAT_HHMM));
                                        }
                                        else if (TimeSpan.Parse(listTimeSheetTmpUser.FirstOrDefault().Date.ToString(CommonConstants.FORMAT_HHMM)) < TimeSpan.Parse(otRequest.EndTime) && TimeSpan.Parse(listTimeSheetTmpUser.FirstOrDefault().Date.ToString(CommonConstants.FORMAT_HHMM)) > timeMidOT)
                                        {
                                            OTCheckOut = TimeSpan.Parse(listTimeSheetTmpUser.FirstOrDefault().Date.ToString(CommonConstants.FORMAT_HHMM));
                                        }
                                        else if (TimeSpan.Parse(listTimeSheetTmpUser.FirstOrDefault().Date.ToString(CommonConstants.FORMAT_HHMM)) >= TimeSpan.Parse(otRequest.EndTime))
                                        {
                                            OTCheckOut = TimeSpan.Parse(otRequest.EndTime);
                                        }
                                    }
                                    else if (listTimeSheetTmpUser.Count() > 1)
                                    {
                                        TimeSpan firstTime = TimeSpan.Parse(listTimeSheetTmpUser.OrderByField(CommonConstants.ORDERBY_DATE, true).FirstOrDefault().Date.ToString(CommonConstants.FORMAT_HHMM));
                                        TimeSpan finalTime = TimeSpan.Parse(listTimeSheetTmpUser.OrderByField(CommonConstants.ORDERBY_DATE, false).FirstOrDefault().Date.ToString(CommonConstants.FORMAT_HHMM));
                                        if (firstTime <= TimeSpan.Parse(otRequest.StartTime))
                                        {
                                            OTCheckIn = TimeSpan.Parse(otRequest.StartTime);
                                        }
                                        else if (firstTime > TimeSpan.Parse(otRequest.StartTime) && firstTime <= timeMidOT)
                                        {
                                            OTCheckIn = firstTime;
                                        }
                                        if (finalTime > timeMidOT && finalTime < TimeSpan.Parse(otRequest.EndTime))
                                        {
                                            OTCheckOut = finalTime;
                                        }
                                        else if (finalTime >= TimeSpan.Parse(otRequest.EndTime))
                                        {
                                            OTCheckOut = TimeSpan.Parse(otRequest.EndTime);
                                        }
                                    }
                                }
                                // Ot normal day
                                else
                                {
                                    var finalTimeSheetTmp = listTimeSheetTmpUser.Where(x => timeDay != null && TimeSpan.Parse(x.Date.ToString(CommonConstants.FORMAT_HHMM)) >= TimeSpan.Parse(timeDay.CheckOut)).OrderByField(CommonConstants.ORDERBY_DATE, false).FirstOrDefault();
                                    var finalTimeSheet = finalTimeSheetTmp != null ? TimeSpan.Parse(finalTimeSheetTmp.Date.ToString(CommonConstants.FORMAT_HHMM)) : TimeSpan.Zero;
                                    if (finalTimeSheet != TimeSpan.Zero)
                                    {
                                        if (finalTimeSheet <= timeMidOT && finalTimeSheet >= TimeSpan.Parse(otRequest.StartTime))
                                        {
                                            OTCheckIn = TimeSpan.Parse(listTimeSheetTmpUser.OrderByField(CommonConstants.ORDERBY_DATE, true).FirstOrDefault().Date.ToString(CommonConstants.FORMAT_HHMM));
                                            if (OTCheckIn <= TimeSpan.Parse(otRequest.StartTime))
                                            {
                                                OTCheckIn = TimeSpan.Parse(otRequest.StartTime);
                                            }
                                        }
                                        else if (finalTimeSheet > timeMidOT)
                                        {
                                            if (finalTimeSheet >= TimeSpan.Parse(otRequest.EndTime))
                                            {
                                                OTCheckOut = TimeSpan.Parse(otRequest.EndTime);
                                            }
                                            else
                                            {
                                                OTCheckOut = finalTimeSheet;
                                            }
                                            OTCheckIn = TimeSpan.Parse(otRequest.StartTime);
                                        }
                                    }
                                }

                            }
                            timeSheetUserCheckOut = timeDay != null && timeSheetUserCheckOut == null ? listTimeSheetTmpUser.Where(x => TimeSpan.Parse(x.Date.ToString(CommonConstants.FORMAT_HHMM)) < TimeSpan.Parse(timeDay.CheckOut)
                                    && TimeSpan.Parse(x.Date.ToString(CommonConstants.FORMAT_HHMM)) > TimeSpan.Parse(timeDay.CheckIn))
                                .OrderByField(CommonConstants.ORDERBY_DATE, false).FirstOrDefault() : timeSheetUserCheckOut;
                            if (isHolidayOrDayOff && otRequest == null)
                                continue;
                            count++;
                            //Daclare new timesheet
                            FingerTimeSheet fingerTimeSheet = new FingerTimeSheet();
                            fingerTimeSheet.UserNo = _fingerMachineUserRepository.GetMulti(x => x.UserId == userID).FirstOrDefault().ID;
                            fingerTimeSheet.DayOfCheck = shortDate;
                            fingerTimeSheet.CheckIn = timeSheetUserCheckIn != null ? timeSheetUserCheckIn.Date.ToString(CommonConstants.FORMAT_HHMM) : null;
                            if (listTimeSheetTmpUser.Count() == 1)
                            {
                                fingerTimeSheet.CheckOut = TimeSpan.Parse(listTimeSheetTmpUser.OrderByField(CommonConstants.ORDERBY_DATE, false).FirstOrDefault().Date.ToString(CommonConstants.FORMAT_HHMM)) >= TimeSpan.Parse(CommonConstants.ABSENT_HOUR_AFTERNOON) ? listTimeSheetTmpUser.OrderByField(CommonConstants.ORDERBY_DATE, false).FirstOrDefault().Date.ToString(CommonConstants.FORMAT_HHMM) : null;
                            }
                            else if (listTimeSheetTmpUser.Count() > 1)
                            {
                                fingerTimeSheet.CheckOut = listTimeSheetTmpUser.OrderByField(CommonConstants.ORDERBY_DATE, false).FirstOrDefault().Date.ToString(CommonConstants.FORMAT_HHMM);
                            }
                            fingerTimeSheet.OTCheckIn = OTCheckIn != TimeSpan.Zero ? OTCheckIn.ToString(@"hh\:mm") : null;
                            fingerTimeSheet.OTCheckOut = OTCheckOut != TimeSpan.Zero ? OTCheckOut.ToString(@"hh\:mm") : null;
                            var childcareLeave = GetChildcareLeaveByUserID(userID);
                            if (fingerTimeSheet.CheckIn != null)
                            {
                                if ((childcareLeave == null || !childcareLeave.IsLateComing || shortDate < childcareLeave.StartDate || shortDate > childcareLeave.EndDate) && timeDay != null && fingerTimeSheet.CheckIn != null && TimeSpan.Parse(fingerTimeSheet.CheckIn) > TimeSpan.Parse(timeDay.CheckIn) && TimeSpan.Parse(fingerTimeSheet.CheckIn) <= TimeSpan.Parse(CommonConstants.ABSENT_HOUR_MORNING))
                                {
                                    fingerTimeSheet.Late = (TimeSpan.Parse(fingerTimeSheet.CheckIn) - TimeSpan.Parse(timeDay.CheckIn)).ToString(@"hh\:mm");
                                }
                                else if (childcareLeave != null && childcareLeave.IsLateComing && shortDate > childcareLeave.StartDate && shortDate < childcareLeave.EndDate && timeDay != null && fingerTimeSheet.CheckIn != null && TimeSpan.Parse(fingerTimeSheet.CheckIn) > TimeSpan.Parse(timeDay.CheckIn) + TimeSpan.FromHours(childcareLeave.Time) && TimeSpan.Parse(fingerTimeSheet.CheckIn) <= TimeSpan.Parse(CommonConstants.ABSENT_HOUR_MORNING))
                                {
                                    fingerTimeSheet.Late = (TimeSpan.Parse(fingerTimeSheet.CheckIn) - TimeSpan.Parse(timeDay.CheckIn) - TimeSpan.FromHours(childcareLeave.Time)).ToString(@"hh\:mm");
                                }
                            }
                            else
                            {
                                fingerTimeSheet.Absent = CommonConstants.TimeSheetAbsentMorning;
                                fingerTimeSheet.NumOfWorkingDay = CommonConstants.ZERO_PONT_FIVE;
                            }
                            if (fingerTimeSheet.CheckOut != null)
                            {
                                if ((childcareLeave == null || !childcareLeave.IsEarlyLeaving || shortDate < childcareLeave.StartDate || shortDate > childcareLeave.EndDate) && timeDay != null && fingerTimeSheet.CheckOut != null && TimeSpan.Parse(fingerTimeSheet.CheckOut) < TimeSpan.Parse(timeDay.CheckOut) && TimeSpan.Parse(fingerTimeSheet.CheckOut) >= TimeSpan.Parse(CommonConstants.EARLY_HOUR))
                                {
                                    fingerTimeSheet.LeaveEarly = (TimeSpan.Parse(timeDay.CheckOut) - TimeSpan.Parse(fingerTimeSheet.CheckOut)).ToString(@"hh\:mm");
                                }
                                else if (childcareLeave != null && childcareLeave.IsEarlyLeaving && shortDate >= childcareLeave.StartDate && shortDate <= childcareLeave.EndDate && timeDay != null && fingerTimeSheet.CheckOut != null && TimeSpan.Parse(fingerTimeSheet.CheckOut) + TimeSpan.FromHours(childcareLeave.Time) < TimeSpan.Parse(timeDay.CheckOut) && TimeSpan.Parse(fingerTimeSheet.CheckOut) >= TimeSpan.Parse(CommonConstants.EARLY_HOUR))
                                {
                                    fingerTimeSheet.LeaveEarly = (TimeSpan.Parse(timeDay.CheckOut) - TimeSpan.Parse(fingerTimeSheet.CheckOut) - TimeSpan.FromHours(childcareLeave.Time)).ToString(@"hh\:mm");
                                }
                            }
                            else
                            {
                                fingerTimeSheet.Absent = CommonConstants.TimeSheetAbsentAfternoon;
                                fingerTimeSheet.NumOfWorkingDay = CommonConstants.ZERO_PONT_FIVE;
                            }
                            if (timeDay != null && ((fingerTimeSheet.CheckIn != null && TimeSpan.Parse(fingerTimeSheet.CheckIn) > TimeSpan.Parse(CommonConstants.ABSENT_HOUR_MORNING)) || (fingerTimeSheet.CheckIn == null)))
                                fingerTimeSheet.Absent = CommonConstants.TimeSheetAbsentMorning;
                            if (fingerTimeSheet.CheckIn != null)
                            {
                                //Change  Check Out < 13h to CHeck OUt < 16h
                                if (timeDay != null && ((fingerTimeSheet.CheckIn != null && TimeSpan.Parse(fingerTimeSheet.CheckIn) > TimeSpan.Parse(CommonConstants.ABSENT_HOUR_AFTERNOON))))
                                {
                                    fingerTimeSheet.Absent = CommonConstants.TimeSheetAbsentAfternoon;
                                    fingerTimeSheet.NumOfWorkingDay = CommonConstants.ZERO_PONT_FIVE;
                                }
                            }
                            if (fingerTimeSheet.CheckOut != null)
                            {
                                //Change  Check Out < 13h to CHeck OUt < 16h
                                if (timeDay != null && ((fingerTimeSheet.CheckIn != null && TimeSpan.Parse(fingerTimeSheet.CheckIn) > TimeSpan.Parse(CommonConstants.ABSENT_HOUR_AFTERNOON)) || TimeSpan.Parse(fingerTimeSheet.CheckOut) < TimeSpan.Parse(CommonConstants.ABSENT_HOUR_AFTERNOON)))
                                    fingerTimeSheet.Absent = CommonConstants.TimeSheetAbsentAfternoon;
                                fingerTimeSheet.NumOfWorkingDay = CommonConstants.ZERO_PONT_FIVE;
                            }
                            if (timeDay != null && fingerTimeSheet.CheckIn != null && fingerTimeSheet.CheckOut != null)
                            {
                                if (((TimeSpan.Parse(fingerTimeSheet.CheckIn) > TimeSpan.Parse(CommonConstants.ABSENT_HOUR_MORNING)) && TimeSpan.Parse(fingerTimeSheet.CheckOut) < TimeSpan.Parse(CommonConstants.ABSENT_HOUR_AFTERNOON)))
                                {
                                    fingerTimeSheet.Absent = CommonConstants.TimeSheetAbsent;
                                    fingerTimeSheet.NumOfWorkingDay = CommonConstants.ZERO;
                                }
                            }
                            if (fingerTimeSheet.CheckIn != null)
                            {
                                if (timeDay != null && (TimeSpan.Parse(fingerTimeSheet.CheckIn) > TimeSpan.Parse(CommonConstants.ABSENT_HOUR_MORNING)) && fingerTimeSheet.CheckOut == null)
                                {
                                    fingerTimeSheet.Absent = CommonConstants.TimeSheetAbsent;
                                }
                            }
                            if (fingerTimeSheet.CheckOut != null)
                            {
                                if (timeDay != null && (TimeSpan.Parse(fingerTimeSheet.CheckOut) < TimeSpan.Parse(CommonConstants.ABSENT_HOUR_AFTERNOON)) && fingerTimeSheet.CheckIn == null)
                                {
                                    fingerTimeSheet.Absent = CommonConstants.TimeSheetAbsent;
                                }
                            }
                            if (timeDay != null && fingerTimeSheet.CheckIn == null && fingerTimeSheet.CheckOut == null)
                                fingerTimeSheet.Absent = CommonConstants.TimeSheetAbsent;

                            fingerTimeSheet.NumOfWorkingDay = timeDay != null ? CommonConstants.ONE : 0;
                            if (fingerTimeSheet.Absent == CommonConstants.TimeSheetAbsent)
                                fingerTimeSheet.NumOfWorkingDay = CommonConstants.ZERO;
                            if (timeDay != null && fingerTimeSheet.Absent == CommonConstants.TimeSheetAbsentMorning || fingerTimeSheet.Absent == CommonConstants.TimeSheetAbsentAfternoon)
                                fingerTimeSheet.NumOfWorkingDay = CommonConstants.ZERO_PONT_FIVE;
                            //Check allowance
                            var request = _requestRepository.GetMulti(x => x.UserId == userID && x.StartDate <= shortDate && x.EndDate >= shortDate && x.RequestStatusId == CommonConstants.StatusApprovedID);
                            fingerTimeSheet.MinusAllowance = null;
                            if (fingerTimeSheet.Late != null)
                            {
                                if (request.Count(x => x.RequestTypeId == CommonConstants.RequestTypeLateComming) == 0)
                                {
                                    fingerTimeSheet.MinusAllowance = CommonConstants.FOURTY_PERCENT;
                                }
                            }
                            if (fingerTimeSheet.LeaveEarly != null)
                            {
                                if (request.Count(x => x.RequestTypeId == CommonConstants.RequestTypeEarlyLeaving) == 0)
                                {
                                    fingerTimeSheet.MinusAllowance = CommonConstants.FOURTY_PERCENT;
                                }
                            }
                            if (_commonService.IsHolidayOrDayOff(fingerTimeSheet.DayOfCheck))
                            {
                                fingerTimeSheet.Absent = null;
                                fingerTimeSheet.NumOfWorkingDay = 0;
                                fingerTimeSheet.CheckIn = null;
                                fingerTimeSheet.CheckOut = null;
                            }
                            if (_fingerTimeSheetRepository.Add(fingerTimeSheet) != null)
                                _countSuccess++;
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            log.Info(ex.StackTrace);
                            dbTransaction.Rollback();
                            flag = false;
                        }
                        #endregion Code
                    }
                    //Add time sheet ot for dayoff or holiday but not go
                    if (isHolidayOrDayOff)
                    {
                        foreach (var userid in lstAllUserOT)
                        {
                            var fmUser = _fingerMachineUserRepository.GetMulti(x => x.UserId == userid).FirstOrDefault();
                            if (fmUser == null)
                                continue;
                            FingerTimeSheet fingerTime = new FingerTimeSheet
                            {
                                UserNo = fmUser.ID,
                                DayOfCheck = shortDate,
                                Absent = null,
                                NumOfWorkingDay = 0
                            };
                            _fingerTimeSheetRepository.Add(fingerTime);
                        }
                    }
                    var listUserNotEnroll = timeDay != null ? _fingerMachineUserRepository.GetMulti(x => !listUser.Contains(x.UserId) && x.AppUser.Status == true, new string[] { CommonConstants.AppUserGroup }).Select(x => x.UserId).Distinct() : new List<string>();
                    foreach (var item in listUserNotEnroll)
                    {
                        if (!_userOnsiteRepository.isOnsite(item, shortDate))
                        {
                            FingerTimeSheet fingerTimeSheet = new FingerTimeSheet
                            {
                                UserNo = _fingerMachineUserRepository.GetMulti(x => x.UserId == item).Select(x => x.ID).FirstOrDefault(),
                                DayOfCheck = shortDate,
                                Absent = CommonConstants.TimeSheetAbsent,
                                NumOfWorkingDay = CommonConstants.ZERO
                            };
                            _fingerTimeSheetRepository.Add(fingerTimeSheet);
                        }
                    }
                }
                if (_countSuccess == count && listFingerTimeSheetError.Count == 0)
                {
                    if (listDate.Count > 1)
                    {
                        listDate.OrderBy(x => x);
                        log.Info("Validate all time sheet " + listDate[0] + "-" + listDate[listDate.Count - 1] + " :Success");
                    }
                    else if (listDate.Count == 1)
                    {
                        log.Info("Validate all time sheet " + listDate[0] + " :Success");
                    }
                    try
                    {
                        Save();
                        dbTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        dbContext.Database.Connection.Close();
                        return false;
                    }
                    flag = true;
                    var shortDate = DateTime.Parse(listDate[0]);
                    if (_commonService.IsHolidayOrDayOff(shortDate))
                    {
                        try
                        {
                            dbContext.Database.ExecuteSqlCommand(AbnormalQuery.ExcuteStoreAbnormalDayOff);
                        }
                        catch (Exception ex)
                        {
                            log.Error("Excute query abnormal weekend:" + ex);
                            flag = false;
                        }
                    }
                    else
                    {
                        try
                        {
                            if (listDate.Count() > 0)
                            {
                                var minDate = listDate[0];
                                var maxDate = listDate[listDate.Count - 1];
                                dbContext.Database.ExecuteSqlCommand(AbnormalQuery.ExecuteStoreAbnormalCaseQuery, new SqlParameter("@minDate",minDate), new SqlParameter("@maxDate",maxDate));
                                dbContext.Database.ExecuteSqlCommand(AbnormalQuery.ExcuteReport);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("Excute query abnormal case and report:" + ex);
                            flag = false;
                        }
                    }
                    dbContext.Database.Connection.Close();
                    var lstdatetime = new List<DateTime>();
                    foreach (var item in listDate)
                    {
                        lstdatetime.Add(DateTime.Parse(item));

                    }
                    Task.Run(() => NotificationAbnormal(lstdatetime));
                }
                else
                {
                    dbContext.Database.Connection.Close();
                    return false;
                    //dbTransaction.Rollback();
                }
            }
            return flag;
        }

        private void NotificationAbnormal(List<DateTime> lstDate)
        {
            foreach (var item in lstDate)
            {
                SenmailAbnormal(item);
            }
        }

        private void SenmailAbnormal(DateTime item)
        {
            TMSDbContext DbContext = new TMSDbContext();
            var query = (from abnormal in DbContext.AbnormalCases
                         join finger in DbContext.FingerTimeSheets
                         on abnormal.TimeSheetID equals finger.ID
                         select new AbnormalCaseModel
                         {
                             TimeSheetID = abnormal.TimeSheetID,
                             FullName = abnormal.FingerTimeSheet.FingerMachineUsers.AppUser.FullName,
                             GroupName = abnormal.FingerTimeSheet.FingerMachineUsers.AppUser.Group.Name,
                             AbnormalDate = abnormal.FingerTimeSheet.DayOfCheck,
                             AbnormalReason = DbContext.AbnormalCases.Where(m => m.TimeSheetID == abnormal.TimeSheetID).Select(m => new AbnormalReasonModel
                             {
                                 ID = m.AbnormalReason.ID,
                                 Name = m.AbnormalReason.Name
                             }).Distinct().ToList(),
                             Email = abnormal.FingerTimeSheet.FingerMachineUsers.AppUser.Email
                         });
            var listabnormal = query.Where(x => x.AbnormalDate == item).AsEnumerable().Distinct().ToList();
            var lstAbnormalDistinct = new List<AbnormalCaseModel>();
            var id = 0;
            foreach (var abnormal in listabnormal)
            {
                if (abnormal.TimeSheetID != id)
                {
                    lstAbnormalDistinct.Add(new AbnormalCaseModel
                    {
                        ID = abnormal.ID,
                        UserName = abnormal.UserName,
                        FullName = abnormal.FullName,
                        GroupName = abnormal.GroupName,
                        AbnormalDate = abnormal.AbnormalDate,
                        AbnormalReason = abnormal.AbnormalReason,
                        Email = abnormal.Email
                    });
                }
                id = abnormal.TimeSheetID;
            }
            for (int j = 0; j < lstAbnormalDistinct.Count; j++)
            {
                string body = "";
                bool resultsendmailNotification = true;
                var Date = new DateTime();
                lstAbnormalDistinct[j] = ConvertSubstringToString(lstAbnormalDistinct[j]);
				var dateExplanation = _commonService.GetDateExRequestInPast(lstAbnormalDistinct[j].AbnormalDate.Value);
				body = _systemservice.getBodyMailNotificationAbnormal(MailConsstants.TemplateNotificationAbnormal, lstAbnormalDistinct[j].FullName, lstAbnormalDistinct[j].GroupName, lstAbnormalDistinct[j].ReasonList.ToString(), lstAbnormalDistinct[j].AbnormalDate.ToString(), "Abnormal Case List", "Time Management", dateExplanation.ToString());
				resultsendmailNotification = _systemservice.SendMail(new string[] { lstAbnormalDistinct[j].Email }, null, "[Abnormal Case - TMS]", body);
				Thread.Sleep(1 * 1000);
            }

        }
        public AbnormalCaseModel ConvertSubstringToString(AbnormalCaseModel model)
        {
            for (int i = 0; i < model.AbnormalReason.Count; i++)
            {
                if (i < model.AbnormalReason.Count - 1)
                {
                    model.ReasonList += model.AbnormalReason[i].Name + ", ";
                }
                else
                {
                    model.ReasonList += model.AbnormalReason[i].Name;
                }
            }
            return model;
        }

        private ChildcareLeave GetChildcareLeaveByUserID(string userID)
        {
            var user = _appUserRepository.GetSingleByCondition(x => x.Id == userID);
            if (user != null && user.ChildcareLeaveID != null)
            {
                return _childcareLeaveRepository.GetSingleByCondition(x => x.ID == user.ChildcareLeaveID);
            }
            return null;
        }
        public void Save()
        {
            this._unitOfWork.Commit();
        }

        /// <summary>
        /// get list time sheet and fillter
        /// </summary>
        /// <param name="userID">id of logged in user</param>
        /// <param name="filter">filter timesheet</param>
        /// <returns>list time sheet model contains all information to view</returns>
        public List<FingerTimeSheetModel> GetListTimeSheetFilter(string userID, FilterModel filter, string colunm, bool isAsc)
        {
            var model = _fingerTimeSheetRepository.GetTimeSheetModel();
            if (filter != null)
            {
                if (filter.FullName.Count() != 0)
                {
                    model = model.Where(x => filter.FullName.Contains(x.UserId));
                }
                if (filter.FromDate != null && filter.ToDate != null)
                {
                    DateTime startDate = DateTime.ParseExact(filter.FromDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture).Date;
                    DateTime endDate = DateTime.ParseExact(filter.ToDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture).Date;
                    model = model.Where(x => (x.DayOfCheck >= startDate) && (x.DayOfCheck <= endDate));
                }
                if (filter.AbnormalTimeSheetType != null)
                {
                    if (filter.AbnormalTimeSheetType.Contains(StringConstants.TimeSheetAbsentMorning) ||
                    filter.AbnormalTimeSheetType.Contains(StringConstants.TimeSheetAbsentAfternoon) ||
                    filter.AbnormalTimeSheetType.Contains(StringConstants.TimeSheetAbsent) ||
                    filter.AbnormalTimeSheetType.Contains(StringConstants.FilterTimeSheetComeLate) ||
                    filter.AbnormalTimeSheetType.Contains(StringConstants.FilterTimeSheetComeBackSoon))
                    {
                        model = model.Where(x => (filter.AbnormalTimeSheetType.Contains(x.Absent)) ||
                        ((filter.AbnormalTimeSheetType.Contains(StringConstants.FilterTimeSheetComeLate)) ? (!string.IsNullOrEmpty(x.Late)) : false) ||
                        ((filter.AbnormalTimeSheetType.Contains(StringConstants.FilterTimeSheetComeBackSoon)) ? (!string.IsNullOrEmpty(x.LeaveEarly)) : false));
                    }
                }
                if (filter.StatusExplanation != null)
                {
                    if (filter.StatusExplanation.Count() != 0)
                    {
                        model = model.Where(x => filter.StatusExplanation.Contains(x.StatusExplanation));
                    }
                }
            }
            if (_fingerTimeSheetRepository.IsReadAll(userID, CommonConstants.FunctionTimeSheet))
            {
                return _fingerTimeSheetRepository.GetPagingFilterForAll(model).OrderByField(colunm, isAsc).ToList();
            }
            return _fingerTimeSheetRepository.GetPagingFilterForMemBer(userID, model).OrderByField(colunm, isAsc).ToList();
        }

        /// <summary>
        /// Map data field Absent Type
        /// </summary>
        /// <param name="model">data time sheet model to mapping</param>
        /// <returns>list time sheet model contains all information to view</returns>
        public List<FingerTimeSheetModel> MapAbsentField(List<FingerTimeSheetModel> model)
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
        public bool IsUserNoExistTimeSheet(List<string> lstUserNo)
        {
            return _fingerTimeSheetRepository.GetMulti(x => lstUserNo.Contains(x.UserNo)).Count() > 0;
        }
        public FingerTimeSheet GetById(int Id)
        {
            return _fingerTimeSheetRepository.GetSingleById(Id);
        }
        public ExplanationRequest GetExplanationDetail(int id)
        {
            return _explanationRepository.GetSingleByCondition(x => x.ID.Equals(id), new string[] {
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
        private void CalculateReport(IEnumerable<string> listUserNo, IEnumerable<FingerTimeSheet> lstTimeSheet, string userID, out float TotalAuthorizedLeavesInPeriod, out float numOfUnusedAuthorizedLeaveNoRequest)
        {
            var requestTypeAbsent = new List<int> { 1, 2, 3 };
            TotalAuthorizedLeavesInPeriod = 0;
            numOfUnusedAuthorizedLeaveNoRequest = 0;
            foreach (var timeSheet in lstTimeSheet)
            {
                var explaination = _explanationRepository.GetSingleByCondition(x => x.TimeSheetId == timeSheet.ID);
                var lstRequestAbsent = _requestRepository.GetMulti(x => timeSheet.DayOfCheck >= x.StartDate && timeSheet.DayOfCheck <= x.EndDate && x.EntitleDayId == 1 && x.RequestStatusId == 4 && requestTypeAbsent.Contains(x.RequestTypeId) && x.UserId == userID);
                if (explaination != null && explaination.StatusRequestId == 4)
                {
                    var lstAbnormalUnauthorizedLeave = _abnormalRepository.GetAbnormalById(timeSheet.ID).Where(x => x.ReasonId == 5);
                    if (explaination.Actual == StringConstants.Leave)
                    {
                        foreach (var request in lstRequestAbsent)
                        {

                            switch (request.RequestTypeId)
                            {
                                case 2:
                                    if (lstAbnormalUnauthorizedLeave.Where(x => x.FingerTimeSheet.Absent == "V").ToList().Count() > 0)
                                    {
                                        TotalAuthorizedLeavesInPeriod += (float)0.5;
                                    }
                                    if (lstAbnormalUnauthorizedLeave.Where(x => x.FingerTimeSheet.Absent == "VC").ToList().Count() > 0)
                                    {
                                        TotalAuthorizedLeavesInPeriod += (float)0.5;
                                    }
                                    break;
                                case 3:
                                    if (lstAbnormalUnauthorizedLeave.Where(x => x.FingerTimeSheet.Absent == "V").ToList().Count() > 0)
                                    {
                                        TotalAuthorizedLeavesInPeriod += (float)0.5;
                                    }
                                    if (lstAbnormalUnauthorizedLeave.Where(x => x.FingerTimeSheet.Absent == "VS").ToList().Count() > 0)
                                    {
                                        TotalAuthorizedLeavesInPeriod += (float)0.5;
                                    }
                                    break;
                                default:
                                    break;
                            }

                        }
                        //case no request
                        //if (lstRequestAbsent.Count() == 0)
                        //{
                        //    switch (explaination.FingerTimeSheet.Absent)
                        //    {
                        //        case "V":
                        //            TotalAuthorizedLeavesInPeriod += 1;
                        //            break;
                        //        case "VS":
                        //            TotalAuthorizedLeavesInPeriod += (float)0.5;
                        //            break;
                        //        case "VC":
                        //            TotalAuthorizedLeavesInPeriod += (float)0.5;
                        //            break;
                        //    }
                        //}
                    }
                    else if (explaination.Actual == StringConstants.ForgetToCheck)
                    {
                        foreach (var request in lstRequestAbsent)
                        {
                            switch (request.RequestTypeId)
                            {
                                case 2:
                                    if (lstAbnormalUnauthorizedLeave.Where(x => x.FingerTimeSheet.Absent == "VC").ToList().Count() > 0)
                                    {
                                        TotalAuthorizedLeavesInPeriod -= (float)0.5;
                                    }
                                    break;
                                case 3:
                                    if (lstAbnormalUnauthorizedLeave.Where(x => x.FingerTimeSheet.Absent == "VS").ToList().Count() > 0)
                                    {
                                        TotalAuthorizedLeavesInPeriod -= (float)0.5;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        var lstAbnormalUnusedAuthorizedLeave = _abnormalRepository.GetAbnormalById(timeSheet.ID).Where(x => x.ReasonId == 6);
                        foreach (var request in lstRequestAbsent)
                        {
                            switch (request.RequestTypeId)
                            {
                                case 2:
                                    if (lstAbnormalUnusedAuthorizedLeave.Where(x => x.FingerTimeSheet.Absent == "VC").ToList().Count() > 0)
                                    {
                                        TotalAuthorizedLeavesInPeriod += (float)0.5;
                                    }
                                    break;
                                case 3:
                                    if (lstAbnormalUnusedAuthorizedLeave.Where(x => x.FingerTimeSheet.Absent == "VS").ToList().Count() > 0)
                                    {
                                        TotalAuthorizedLeavesInPeriod += (float)0.5;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    continue;
                }
                var lstAbnormalUnusedAuthorizedLeaveNoExplain = _abnormalRepository.GetAbnormalById(timeSheet.ID).Where(x => x.ReasonId == 6);
                foreach (var request in lstRequestAbsent)
                {
                    switch (request.RequestTypeId)
                    {
                        case 1:
                            if (lstAbnormalUnusedAuthorizedLeaveNoExplain.Where(x => string.IsNullOrEmpty(x.FingerTimeSheet.Absent)).ToList().Count() > 0)
                            {
                                TotalAuthorizedLeavesInPeriod += 1;
                                numOfUnusedAuthorizedLeaveNoRequest += 1;
                            }
                            else if (lstAbnormalUnusedAuthorizedLeaveNoExplain.Where(x => x.FingerTimeSheet.Absent == "VS").ToList().Count() > 0)
                            {
                                TotalAuthorizedLeavesInPeriod += (float)0.5;
                                numOfUnusedAuthorizedLeaveNoRequest += (float)0.5;
                            }
                            else if (lstAbnormalUnusedAuthorizedLeaveNoExplain.Where(x => x.FingerTimeSheet.Absent == "VC").ToList().Count() > 0)
                            {
                                TotalAuthorizedLeavesInPeriod += (float)0.5;
                                numOfUnusedAuthorizedLeaveNoRequest += (float)0.5;
                            }
                            break;
                        case 2:
                            if (lstAbnormalUnusedAuthorizedLeaveNoExplain.Where(x => string.IsNullOrEmpty(x.FingerTimeSheet.Absent)).ToList().Count() > 0)
                            {
                                TotalAuthorizedLeavesInPeriod += (float)0.5;
                                numOfUnusedAuthorizedLeaveNoRequest += (float)0.5;
                            }
                            else if (lstAbnormalUnusedAuthorizedLeaveNoExplain.Where(x => x.FingerTimeSheet.Absent == "VC").ToList().Count() > 0)
                            {
                                TotalAuthorizedLeavesInPeriod += (float)0.5;
                                numOfUnusedAuthorizedLeaveNoRequest += (float)0.5;
                            }
                            break;
                        case 3:
                            if (lstAbnormalUnusedAuthorizedLeaveNoExplain.Where(x => string.IsNullOrEmpty(x.FingerTimeSheet.Absent)).ToList().Count() > 0)
                            {
                                TotalAuthorizedLeavesInPeriod += (float)0.5;
                                numOfUnusedAuthorizedLeaveNoRequest += (float)0.5;
                            }
                            else if (lstAbnormalUnusedAuthorizedLeaveNoExplain.Where(x => x.FingerTimeSheet.Absent == "VS").ToList().Count() > 0)
                            {
                                TotalAuthorizedLeavesInPeriod += (float)0.5;
                                numOfUnusedAuthorizedLeaveNoRequest += (float)0.5;
                            }
                            break;
                    }

                }
            }
        }
        public IEnumerable<ReportModel> GetAllReportPaging(string userId, FilterReport filter, int page, int pageSize, ref int totalRow)
        {
            var model = _reportRepository.GetAllReport(userId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            totalRow = _reportRepository.GetAllReport(userId).Count();
            if (filter != null)
            {
                if (filter.StartDate != null && filter.EndDate != null)
                {
                    DateTime startDate = DateTime.ParseExact(filter.StartDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture).Date;
                    DateTime endDate = DateTime.ParseExact(filter.EndDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture).Date;
                    foreach (var item in model)
                    {
                        var listUserNo = _fingerMachineUserRepository.GetMulti(x => x.UserId == item.UserID).Select(x => x.ID);
                        item.WorkingDaysFromFingerPrint = _fingerTimeSheetRepository.GetMulti(x => listUserNo.Contains(x.UserNo) && x.DayOfCheck >= startDate && x.DayOfCheck <= endDate).Sum(x => x.NumOfWorkingDay);
                        var lstTimeSheetToNow = _fingerTimeSheetRepository.GetMulti(x => x.DayOfCheck >= startDate && listUserNo.Contains(x.UserNo));
                        var lstTimeSheetInPeriod = lstTimeSheetToNow.Where(x => x.DayOfCheck <= endDate && listUserNo.Contains(x.UserNo));
                        float numOfUnusedAuthorizedLeaveNoRequest = 0;
                        float TotalAuthorizedLeavesInPeriod = 0;
                        CalculateReport(listUserNo, lstTimeSheetInPeriod, item.UserID, out TotalAuthorizedLeavesInPeriod, out numOfUnusedAuthorizedLeaveNoRequest);
                        item.TotalAuthorizedLeavesInPeriod = TotalAuthorizedLeavesInPeriod + _reportRepository.GetMulti(x => listUserNo.Contains(x.FingerTimeSheet.UserNo) && x.DateCheckRequest >= startDate && x.DateCheckRequest <= endDate).Sum(x => x.LeaveMounth);
                        item.WorkingDaysToCalculateSalary = item.WorkingDaysFromFingerPrint + item.TotalAuthorizedLeavesInPeriod - numOfUnusedAuthorizedLeaveNoRequest;
                        float TotalAuthorizedLeaves = 0;
                        numOfUnusedAuthorizedLeaveNoRequest = 0;
                        CalculateReport(listUserNo, lstTimeSheetToNow, item.UserID, out TotalAuthorizedLeaves, out numOfUnusedAuthorizedLeaveNoRequest);
                        item.RemainEntitleDayAtBeginningOfPeriod = item.Remain + TotalAuthorizedLeaves + _reportRepository.GetMulti(x => listUserNo.Contains(x.FingerTimeSheet.UserNo) && x.DateCheckRequest >= startDate).Sum(x => x.LeaveMounth);
                    }
                }
            }
            else
            {
                foreach (var item in model)
                {
                    var listUserNo = _fingerMachineUserRepository.GetMulti(x => x.UserId == item.UserID).Select(x => x.ID);
                    var lstTimeSheet = _fingerTimeSheetRepository.GetMulti(x => listUserNo.Contains(x.UserNo));
                    item.WorkingDaysFromFingerPrint = _fingerTimeSheetRepository.GetMulti(x => listUserNo.Contains(x.UserNo)).Sum(x => x.NumOfWorkingDay);
                    float TotalAuthorizedLeaves = 0;
                    float numOfUnusedAuthorizedLeaveNoRequest = 0;
                    CalculateReport(listUserNo, lstTimeSheet, item.UserID, out TotalAuthorizedLeaves, out numOfUnusedAuthorizedLeaveNoRequest);
                    item.TotalAuthorizedLeavesInPeriod = TotalAuthorizedLeaves + _reportRepository.GetMulti(x => listUserNo.Contains(x.FingerTimeSheet.UserNo)).Sum(x => x.LeaveMounth);
                    item.WorkingDaysToCalculateSalary = item.WorkingDaysFromFingerPrint + item.TotalAuthorizedLeavesInPeriod - numOfUnusedAuthorizedLeaveNoRequest;
                    item.RemainEntitleDayAtBeginningOfPeriod = item.Remain + TotalAuthorizedLeaves + _reportRepository.GetMulti(x => listUserNo.Contains(x.FingerTimeSheet.UserNo)).Sum(x => x.LeaveMounth);
                }
                return model;
            }
            return model;
        }
        public Task GetAllReportEx(string userId, DateTime startDate, DateTime endDate)
        {
            return Task.Run(
                () =>
                {
                    FileInfo fi = new FileInfo(HttpRuntime.AppDomainAppPath + @"\Templates\ReportExTemplate.xlsx");
                    using (ExcelPackage package = new ExcelPackage(fi))
                    {
                        if (package.Workbook.Worksheets.Count < 1)
                        {
                            // Log - Không có sheet nào tồn tại trong file excel của bạn 
                            return;
                        }
                        ExcelWorksheet workSheet = package.Workbook.Worksheets.FirstOrDefault(x => x.Name == "Tháng 6.2018");

                        // Tạo author cho file Excel
                        package.Workbook.Properties.Author = "TMS";
                        workSheet.Name = startDate.ToString("dd/MM/yyyy") + " đến " + endDate.ToString("dd/MM/yyyy");
                        // Tạo title cho file Excel
                        package.Workbook.Properties.Title = "Report";
                        //package.Workbook.Properties.Comments = "This is my Comments";
                        WriteExcelWorkSheet(ref workSheet, startDate.Date, endDate.Date, userId);
                        FileInfo fileInfo = new FileInfo(HttpRuntime.AppDomainAppPath + @"\Reports\ReportEx.xlsx");
                        if (File.Exists(fileInfo.FullName))
                        {
                            File.Delete(fileInfo.FullName);
                        }
                        package.SaveAs(fileInfo);
                    }
                }
                );

        }
        private void WriteExcelWorkSheet(ref ExcelWorksheet workSheet, DateTime startDate, DateTime endDate, string userID)
        {
            var groups = _reportRepository.GetAllGroup().Where(x => x.Name != "SuperAdmin");
            List<int> lstRowGroup = new List<int>();
            //worksheet.Cells[FromRow, FromColumn, ToRow, ToColumn].Merge = true;
            var dates = new List<DateTime>();
            for (var dt = startDate; dt <= endDate; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }
            workSheet.InsertColumn(7, dates.Count - 1, 6);
            workSheet.Cells[3, 1, workSheet.Dimension.End.Row, workSheet.Dimension.End.Column].AutoFilter = true;
            //merge cell title
            workSheet.Cells[1, 1, 1, 17 + dates.Count].Merge = true;
            workSheet.Cells[1, 1, 1, 17 + dates.Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //merge cell list date
            workSheet.Cells[2, 6, 2, 6 + dates.Count - 1].Merge = true;
            workSheet.Cells[2, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            for (int i = 0; i < dates.Count; i++)
            {
                if (i > 0)
                {
                    workSheet.Cells[3, 6].Copy(workSheet.Cells[3, 6 + i]);
                    workSheet.Column(6 + i).Width = workSheet.Column(6).Width;
                }
                workSheet.Cells[3, 6 + i].Value = dates[i].ToString("dd/MM");
            }
            //config title
            SetTitle(ref workSheet, startDate, endDate, dates.Count);
            //fill data
            HttpRuntime.Cache["ProgressValue:" + userID] = 0;
            int k = 4;
            int stt = 1;
            foreach (var group in groups)
            {
                workSheet.Cells[k, 1, k, 6 + dates.Count + 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[k, 1, k, 6 + dates.Count + 5].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                workSheet.Cells[k, 3].Value = group.Name;
                lstRowGroup.Add(k);
                k++;
                var reportExModels = _reportRepository.GetAllReportEx(userID, group.ID);
                foreach (var report in reportExModels)
                {
                    var listTimeSheetEx = new List<TimeSheetEx>();
                    // get data
                    for (int i = 0; i < dates.Count; i++)
                    {
                        listTimeSheetEx.Add(new TimeSheetEx { Date = dates[i], Status = "" });
                    }
                    var listUserNo = _fingerMachineUserRepository.GetMulti(x => x.UserId == report.UserID).Select(x => x.ID);
                    var lstTimeSheetToNow = _fingerTimeSheetRepository.GetMulti(x => x.DayOfCheck >= startDate && listUserNo.Contains(x.UserNo));
                    //var lstTimeSheetInPeriod = lstTimeSheetToNow.Where(x => x.DayOfCheck <= endDate && listUserNo.Contains(x.UserNo));
                    float TotalAuthorizedLeaves = 0;
                    CalculateReportEx(listUserNo, lstTimeSheetToNow, report.UserID, out TotalAuthorizedLeaves, ref listTimeSheetEx);
                    report.RemainEntitleDayAtBeginningOfPeriod = report.RemainEntitleDay + TotalAuthorizedLeaves + _reportRepository.GetMulti(x => listUserNo.Contains(x.FingerTimeSheet.UserNo) && x.DateCheckRequest >= startDate).Sum(x => x.LeaveMounth);

                    for (int i = 0; i < dates.Count; i++)
                    {
                        //check onsite
                        if (CheckUserOnsite(report.UserID, dates[i]) && !_commonService.IsHolidayOrDayOff(dates[i]) /*_timeDayRepository.IsTimeDay(dates[i])*/)
                        {
                            listTimeSheetEx[i].Status = "x";
                        }
                        //end check onsite
                    }
                    //fill data to excel
                    workSheet.Cells[k, 1].Value = stt;
                    workSheet.Cells[k, 2].Value = report.EmployeeID;
                    workSheet.Cells[k, 3].Value = report.FullName;
                    workSheet.Cells[k, 4].Value = report.TotalEntitleDay;
                    workSheet.Cells[k, 5].Value = report.RemainEntitleDayAtBeginningOfPeriod;
                    workSheet.Cells[k, 6 + dates.Count + 5].Value = report.RemainEntitleDay;
                    //fill color to name
                    if (IsUserResign(report.UserID, DateTime.Now))
                    {
                        workSheet.Cells[k, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[k, 3].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(148, 138, 84));
                    }
                    // end fill color to name
                    for (int i = 0; i < listTimeSheetEx.Count; i++)
                    {
                        if (IsUserResign(report.UserID, dates[i]))
                        {
                            workSheet.Cells[k, 6 + i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            workSheet.Cells[k, 6 + i].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(148, 138, 84));
                        }
                        else
                            workSheet.Cells[k, 6 + i].Value = listTimeSheetEx[i].Status;
                    }
                    stt++;
                    k++;
                    var progressValue = HttpRuntime.Cache["ProgressValue:" + userID] != null ? (int)HttpRuntime.Cache["ProgressValue:" + userID] : 0;
                    progressValue += 1;
                    HttpRuntime.Cache["ProgressValue:" + userID] = progressValue;
                }
            }

            var allCells = workSheet.Cells[4, 1, workSheet.Dimension.End.Row, workSheet.Dimension.End.Column];
            var cellFont = allCells.Style.Font;
            for (int i = 4; i <= workSheet.Dimension.End.Row; i++)
            {
                workSheet.Row(i).Style.Font.Size = (float)25.80;
                for (int j = 1; j < workSheet.Dimension.End.Column; j++)
                {
                    if (j >= 6 && j < 6 + dates.Count)
                    {
                        // fill color for DayOff or Holiday
                        if (_commonService.IsHolidayOrDayOff(dates[j - 6]) /*!_timeDayRepository.IsTimeDay(dates[j - 6])*/)
                        {
                            workSheet.Cells[i, j].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            workSheet.Cells[i, j].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                        }
                    }
                    if (j == 3)
                    {
                        workSheet.Cells[i, j].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        workSheet.Cells[i, j].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }
                    else
                    {
                        workSheet.Cells[i, j].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        workSheet.Cells[i, j].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }
                    workSheet.Cells[i, j].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    // set formula
                    if (!lstRowGroup.Contains(i))
                    {
                        var cellRange = workSheet.Cells[i, 6, i, 6 + dates.Count - 1];
                        workSheet.Cells[i, 6 + dates.Count].Formula = "COUNTIF(" + cellRange.Address + ",\"x\")" + "+1/2*COUNTIF(" + cellRange.Address + ",\"Px\")" + "+1/2*COUNTIF(" + cellRange.Address + ",\"Rx\")" + "+1/2*COUNTIF(" + cellRange.Address + ",\"CDx\")";
                        workSheet.Cells[i, 6 + dates.Count + 1].Formula = "COUNTIF(" + cellRange.Address + ",\"P\")" + "+1/2*COUNTIF(" + cellRange.Address + ",\"Px\")" + "+1/2*COUNTIF(" + cellRange.Address + ",\"RxPx\")" + "+1/2*COUNTIF(" + cellRange.Address + ",\"PxCDx\")";
                        workSheet.Cells[i, 6 + dates.Count + 2].Formula = "COUNTIF(" + cellRange.Address + ",\"CD\")" + "+1/2*COUNTIF(" + cellRange.Address + ",\"CDx\")" + "+COUNTIF(" + cellRange.Address + ",\"NL\")" + "+1/2*COUNTIF(" + cellRange.Address + ",\"PxCDx\")" + "+1/2*COUNTIF(" + cellRange.Address + ",\"RxCDx\")";
                        workSheet.Cells[i, 6 + dates.Count + 3].Formula = "COUNTIF(" + cellRange.Address + ",\"R\")" + "+1/2*COUNTIF(" + cellRange.Address + ",\"Rx\")" + "+1/2*COUNTIF(" + cellRange.Address + ",\"RxPx\")" + "+1/2*COUNTIF(" + cellRange.Address + ",\"RxCDx\")";
                        workSheet.Cells[i, 6 + dates.Count + 4].Formula = workSheet.Cells[i, 6 + dates.Count].Address + "+" + workSheet.Cells[i, 6 + dates.Count + 1].Address + "+" + workSheet.Cells[i, 6 + dates.Count + 2].Address;
                    }
                }
            }
            cellFont.SetFromFont(new Font("Times New Roman", 10));
            foreach (var row in lstRowGroup)
            {
                workSheet.Cells[row, 3].Style.Font.Bold = true;
            }
        }

        private void SetTitle(ref ExcelWorksheet workSheet, DateTime startDate, DateTime endDate, int countDate)
        {
            workSheet.Cells[1, 1].Value = "CHẤM CÔNG VÀ PHÉP TỪ " + startDate.ToString("dd/MM/yyyy") + " ĐẾN " + endDate.ToString("dd/MM/yyyy");
            workSheet.Cells[2, 6].Value = "Chấm công từ " + startDate.ToString("dd/MM/yyyy") + " đến " + endDate.ToString("dd/MM/yyyy");
            workSheet.Cells[2, 4].Value = "Tổng phép năm " + endDate.Year;
            workSheet.Cells[2, 5].Value = "Phép còn từ " + startDate.ToString("dd/MM/yyyy");
            workSheet.Cells[2, 6 + countDate].Value = "Tổng ngày công từ " + startDate.ToString("dd/MM/yyyy") + "-" + endDate.ToString("dd/MM/yyyy");
            workSheet.Cells[2, 6 + countDate + 1].Value = "Tổng nghỉ phép có lương từ " + startDate.ToString("dd/MM/yyyy") + "-" + endDate.ToString("dd/MM/yyyy");
            workSheet.Cells[2, 6 + countDate + 2].Value = "Tổng nghỉ CD có lương từ " + startDate.ToString("dd/MM/yyyy") + "-" + endDate.ToString("dd/MM/yyyy");
            workSheet.Cells[2, 6 + countDate + 3].Value = "Tổng nghỉ việc riêng không lương từ " + startDate.ToString("dd/MM/yyyy") + "-" + endDate.ToString("dd/MM/yyyy");
            workSheet.Cells[2, 6 + countDate + 4].Value = "Số ngày công tính lương từ " + startDate.ToString("dd/MM/yyyy") + "-" + endDate.ToString("dd/MM/yyyy");
            workSheet.Cells[2, 6 + countDate + 5].Value = "Phép năm còn";
        }

        private bool IsUserResign(string userID, DateTime dateTime)
        {
            var user = _appUserRepository.GetSingleByCondition(x => x.Id == userID);
            return user.ResignationDate.HasValue && user.ResignationDate.Value <= dateTime.Date;
        }

        private void CalculateReportEx(IEnumerable<string> listUserNo, IEnumerable<FingerTimeSheet> lstTimeSheet, string userID, out float TotalAuthorizedLeavesInPeriod, ref List<TimeSheetEx> listTimeSheetEx)
        {
            int[] lstEntitleDayID = new int[] { 1, 3, 4, 5, 6 };
            TotalAuthorizedLeavesInPeriod = 0;
            foreach (var timeSheet in lstTimeSheet)
            {
                var explaination = _explanationRepository.GetSingleByCondition(x => x.TimeSheetId == timeSheet.ID && x.StatusRequestId == CommonConstants.StatusApprovedID);
                var lstRequestAbsent = _requestRepository.GetMulti(x => timeSheet.DayOfCheck >= x.StartDate && timeSheet.DayOfCheck <= x.EndDate && lstEntitleDayID.Contains(x.EntitleDayId.Value) && x.RequestStatusId == 4 && x.UserId == userID);
                var lstRequest = _requestRepository.GetMulti(x => timeSheet.DayOfCheck >= x.StartDate && timeSheet.DayOfCheck <= x.EndDate && lstEntitleDayID.Contains(x.EntitleDayId.Value) && x.UserId == userID);
                #region Fill to excel
                bool IsTimeSheetInListEx = listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault() != null && !_commonService.IsHolidayOrDayOff(timeSheet.DayOfCheck);
                if (IsTimeSheetInListEx)
                {
                    var lstAbnormal = _abnormalRepository.GetAbnormalById(timeSheet.ID).Where(x => x.ReasonId == 5 || x.ReasonId == 6);
                    // Có request vắng phép|chế độ và ko abnormal
                    if (lstRequestAbsent.Count() > 0 && lstAbnormal.Count() == 0)
                    {
                        switch (lstRequestAbsent.FirstOrDefault().RequestTypeId)
                        {
                            case 1:
                                //if (string.IsNullOrEmpty(timeSheet.Absent))
                                //{
                                //    listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "P";
                                //}
                                if (timeSheet.Absent == "V")
                                {
                                  
                                    listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = lstRequestAbsent.Where(x => x.EntitleDayId == 1).Count() > 0 ? "P" : "CD";
                                    
                                    //listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "P";

                                }
                                break;
                            case 2:
                                if (timeSheet.Absent == "VS")
                                {
                                    //listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Px";
                                    listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = lstRequestAbsent.Where(x => x.EntitleDayId == 1).Count() > 0 ? "Px" : "CDx";
                                }
                                break;
                            case 3:
                                if (timeSheet.Absent == "VC")
                                {
                                    //listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Px";
                                    listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = lstRequestAbsent.Where(x => x.EntitleDayId == 1).Count() > 0 ? "Px" : "CDx";
                                }
                                break;
                        }
                        int[] arr =new int[] {2,3 };
                    if (lstRequestAbsent.Count() > 1 && lstRequestAbsent.All(x=>arr.Contains(x.RequestTypeId)))
                        {
                            listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "P";
                        }
                    }
                    //có request vắng phép|chế độ và có abnormal chưa giải trình
                    else if (lstRequestAbsent.Count() > 0 && lstAbnormal.Count() > 0 && explaination == null)
                    {
                        int[] arr = new int[] { 2, 3 };
                        if (lstRequestAbsent.Count() > 1 && lstRequestAbsent.All(x => arr.Contains(x.RequestTypeId)))
                        {
                            if (string.IsNullOrEmpty(timeSheet.Absent))
                            {
                                listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = lstRequestAbsent.Where(x => x.EntitleDayId == 1).Count() > 0 ? "P" : "CD";
                            }
                            if (timeSheet.Absent == "VS")
                            {
                                listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = lstRequestAbsent.Where(x => x.EntitleDayId == 1).Count() > 0 ? "P" : "CD";
                            }
                            if (timeSheet.Absent == "VC")
                            {
                                listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = lstRequestAbsent.Where(x => x.EntitleDayId == 1).Count() > 0 ? "P" : "CD";
                            }
                        }
                        else
                        {
                            switch (lstRequestAbsent.FirstOrDefault().RequestTypeId)
                            {
                                case 1:
                                    if (string.IsNullOrEmpty(timeSheet.Absent))
                                    {
                                        //listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "P";
                                        listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = lstRequestAbsent.Where(x => x.EntitleDayId == 1).Count() > 0 ? "P" : "CD";
                                    }
                                    if (timeSheet.Absent == "VS")
                                    {
                                        //listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "P";
                                        listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = lstRequestAbsent.Where(x => x.EntitleDayId == 1).Count() > 0 ? "P" : "CD";
                                    }
                                    if (timeSheet.Absent == "VC")
                                    {
                                        //listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "P";
                                        listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = lstRequestAbsent.Where(x => x.EntitleDayId == 1).Count() > 0 ? "P" : "CD";
                                    }
                                    //if (timeSheet.Absent == "V")
                                    //{
                                    //    listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "P";
                                    //}
                                    break;
                                case 2:
                                    if (string.IsNullOrEmpty(timeSheet.Absent))
                                    {
                                        //listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Px";
                                        listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = lstRequestAbsent.Where(x => x.EntitleDayId == 1).Count() > 0 ? "Px" : "CDx";
                                    }
                                    if (timeSheet.Absent == "V")
                                    {
                                        //listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "RxPx";
                                        listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = lstRequestAbsent.Where(x => x.EntitleDayId == 1).Count() > 0 ? "RxPx" : "RxCDx";
                                    }
                                    if (timeSheet.Absent == "VC")
                                    {
                                        //listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "RxPx";
                                        listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = lstRequestAbsent.Where(x => x.EntitleDayId == 1).Count() > 0 ? "RxPx" : "RxCDx";
                                    }
                                    if (timeSheet.Absent == "VS")
                                    {
                                        listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Px";
                                    }
                                    break;
                                case 3:
                                    if (string.IsNullOrEmpty(timeSheet.Absent))
                                    {
                                        //listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Px";
                                        listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = lstRequestAbsent.Where(x => x.EntitleDayId == 1).Count() > 0 ? "Px" : "CDx";
                                    }
                                    if (timeSheet.Absent == "V")
                                    {
                                        //listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "RxPx";
                                        listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = lstRequestAbsent.Where(x => x.EntitleDayId == 1).Count() > 0 ? "RxPx" : "RxCDx";
                                    }
                                    if (timeSheet.Absent == "VS")
                                    {
                                        //listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "RxPx";
                                        listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = lstRequestAbsent.Where(x => x.EntitleDayId == 1).Count() > 0 ? "RxPx" : "RxCDx";
                                    }
                                    if (timeSheet.Absent == "VC")
                                    {
                                        listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Px";
                                    }
                                    break;
                            }
                        }
                        
                    }
                    //có request vắng phép|chế độ và đã giải trình đc approve
                    else if (lstRequestAbsent.Count() > 0 && lstAbnormal.Count() > 0 && explaination != null)
                    {
                        int[] arr = new int[] { 2, 3 };
                        if (lstRequestAbsent.Count() > 1 && lstRequestAbsent.All(x => arr.Contains(x.RequestTypeId)))
                        {
                            if (string.IsNullOrEmpty(timeSheet.Absent))
                            {
                                listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "x";
                            }
                            if (timeSheet.Absent == "VS")
                            {
                                listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Px";
                            }
                            if (timeSheet.Absent == "VC")
                            {
                                listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Px";
                            }
                        }
                        else {
                            switch (lstRequestAbsent.FirstOrDefault().RequestTypeId)
                            {
                                case 1:
                                    if (string.IsNullOrEmpty(timeSheet.Absent))
                                    {
                                        listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "x";
                                    }
                                    if (timeSheet.Absent == "VS")
                                    {
                                        //listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Px";
                                        listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = lstRequestAbsent.Where(x => x.EntitleDayId == 1).Count() > 0 ? "Px" : "CDx";
                                    }
                                    if (timeSheet.Absent == "VC")
                                    {
                                        //listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Px";
                                        listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = lstRequestAbsent.Where(x => x.EntitleDayId == 1).Count() > 0 ? "Px" : "CDx";
                                    }
                                    //if (timeSheet.Absent == "V")
                                    //{
                                    //    listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "P";
                                    //}
                                    break;
                                case 2:
                                    if (string.IsNullOrEmpty(timeSheet.Absent))
                                    {
                                        listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "x";
                                    }
                                    if (timeSheet.Absent == "V")
                                    {
                                        if (explaination.Actual == StringConstants.Leave)
                                        {
                                            //listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "P";
                                            listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = lstRequestAbsent.Where(x => x.EntitleDayId == 1).Count() > 0 ? "P" : "PxCDx";
                                        }
                                        else if (explaination.Actual == StringConstants.ForgetToCheck)
                                        {
                                            //listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Px";
                                            listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = lstRequestAbsent.Where(x => x.EntitleDayId == 1).Count() > 0 ? "Px" : "CDx";
                                        }
                                    }
                                    if (timeSheet.Absent == "VC")
                                    {
                                        if (explaination.Actual == StringConstants.Leave)
                                        {
                                            //listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "P";
                                            listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = lstRequestAbsent.Where(x => x.EntitleDayId == 1).Count() > 0 ? "Px" : "CDx";
                                        }
                                        else if (explaination.Actual == StringConstants.ForgetToCheck)
                                        {
                                            listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "x";
                                            //listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = lstRequestAbsent.Where(x => x.EntitleDayId == 1).Count() > 0 ? "x" : "CDx";
                                        }
                                    }
                                    //if (timeSheet.Absent == "VS")
                                    //{
                                    //    listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Px";
                                    //}
                                    break;
                                case 3:
                                    if (string.IsNullOrEmpty(timeSheet.Absent))
                                    {
                                        listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "x";
                                    }
                                    if (timeSheet.Absent == "V")
                                    {
                                        if (explaination.Actual == StringConstants.Leave)
                                        {
                                            //listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "P";
                                            listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = lstRequestAbsent.Where(x => x.EntitleDayId == 1).Count() > 0 ? "P" : "PxCDx";
                                        }
                                        else if (explaination.Actual == StringConstants.ForgetToCheck)
                                        {
                                            //listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Px";
                                            listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = lstRequestAbsent.Where(x => x.EntitleDayId == 1).Count() > 0 ? "Px" : "CDx";
                                        }
                                    }
                                    if (timeSheet.Absent == "VS")
                                    {
                                        if (explaination.Actual == StringConstants.Leave)
                                        {
                                            //listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "P";
                                            listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = lstRequestAbsent.Where(x => x.EntitleDayId == 1).Count() > 0 ? "Px" : "CDx";
                                        }
                                        else if (explaination.Actual == StringConstants.ForgetToCheck)
                                        {
                                            listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "x";
                                        }
                                    }
                                    //if (timeSheet.Absent == "VC")
                                    //{
                                    //    listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Px";
                                    //}
                                    break;
                            }
                        }
                    }
                    //không request, ko abnormal
                    else if (lstRequestAbsent.Count() == 0 && lstAbnormal.Count() == 0)
                    {
                        if (string.IsNullOrEmpty(timeSheet.Absent))
                        {
                            listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "x";
                        }
                        if (timeSheet.Absent == "V")
                        {
                            listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "R";
                        }
                        if (timeSheet.Absent == "VS")
                        {
                            listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Rx";
                        }
                        if (timeSheet.Absent == "VC")
                        {
                            listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Rx";
                        }
                    }
                    //ko request ,có abnormal giải trình chưa có hoặc ko approve
                    else if (lstRequestAbsent.Count() == 0 && lstAbnormal.Count() > 0 && explaination == null)
                    {
                        if (timeSheet.Absent == "V")
                        {
                            listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "R";
                        }
                        if (timeSheet.Absent == "VS")
                        {
                            listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Rx";
                        }
                        if (timeSheet.Absent == "VC")
                        {
                            listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Rx";
                        }
                    }
                    //ko request ,có abnormal giải trình approve
                    else if (lstRequestAbsent.Count() == 0 && lstAbnormal.Count() > 0 && explaination != null)
                    {
                        if (timeSheet.Absent == "V")
                        {
                            if (explaination.Actual == StringConstants.Leave)
                            {
                                listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "P";
                            }
                            else if (explaination.Actual == StringConstants.ForgetToCheck)
                            {
                                listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "x";
                            }
                        }
                        if (timeSheet.Absent == "VS")
                        {
                            if (explaination.Actual == StringConstants.Leave)
                            {
                                listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Px";
                            }
                            else if (explaination.Actual == StringConstants.ForgetToCheck)
                            {
                                listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "x";
                            }
                        }
                        if (timeSheet.Absent == "VC")
                        {
                            if (explaination.Actual == StringConstants.Leave)
                            {
                                listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Px";
                            }
                            else if (explaination.Actual == StringConstants.ForgetToCheck)
                            {
                                listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "x";
                            }
                        }
                    }  
                    //Request NoSalary
                    var lstRequestAbsentNoSalary = _requestRepository.GetMulti(x => timeSheet.DayOfCheck >= x.StartDate && timeSheet.DayOfCheck <= x.EndDate && x.EntitleDayId == 2 && x.RequestStatusId == 4 && x.UserId == userID);
                    if (lstRequestAbsentNoSalary.Count() > 0 && explaination == null)
                    {
                        switch (lstRequestAbsentNoSalary.FirstOrDefault().RequestTypeId)
                        {
                            case 1:
                                if (string.IsNullOrEmpty(timeSheet.Absent))
                                {
                                    listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "R";
                                }
                                if (timeSheet.Absent == "V")
                                {
                                    listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "R";
                                }
                                if (timeSheet.Absent == "VS")
                                {
                                    listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "R";
                                }
                                if (timeSheet.Absent == "VC")
                                {
                                    listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "R";
                                }
                                break;
                            case 2:
                                if (string.IsNullOrEmpty(timeSheet.Absent))
                                {
                                    listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Rx";
                                }
                                if (timeSheet.Absent == "V")
                                {
                                    listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "R";
                                }
                                if (timeSheet.Absent == "VS")
                                {
                                    listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Rx";
                                }
                                if (timeSheet.Absent == "VC")
                                {
                                    listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "R";
                                }
                                break;
                            case 3:
                                if (string.IsNullOrEmpty(timeSheet.Absent))
                                {
                                    listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Rx";
                                }
                                if (timeSheet.Absent == "V")
                                {
                                    listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "R";
                                }
                                if (timeSheet.Absent == "VS")
                                {
                                    listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Rx";
                                }
                                if (timeSheet.Absent == "VC")
                                {
                                    listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "R";
                                }
                                break;
                        }
                    }
                    //xin nosalary có giải trình approve
                    else if (lstRequestAbsentNoSalary.Count() > 0 && explaination != null)
                    {
                        switch (lstRequestAbsentNoSalary.FirstOrDefault().RequestTypeId)
                        {
                            case 1:
                                if (string.IsNullOrEmpty(timeSheet.Absent))
                                {
                                    listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "x";
                                }
                                if (timeSheet.Absent == "V")
                                {
                                    listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "R";
                                }
                                if (timeSheet.Absent == "VS")
                                {
                                    listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Rx";
                                }
                                if (timeSheet.Absent == "VC")
                                {
                                    listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Rx";
                                }
                                break;
                            case 2:
                                if (string.IsNullOrEmpty(timeSheet.Absent))
                                {
                                    listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "x";
                                }
                                if (timeSheet.Absent == "V")
                                {
                                    if (explaination.Actual == StringConstants.Leave)
                                    {
                                        listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "RxPx";
                                    }
                                    else if (explaination.Actual == StringConstants.ForgetToCheck)
                                    {
                                        listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Rx";
                                    }
                                }
                                if (timeSheet.Absent == "VS")
                                {
                                    listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Rx";
                                }
                                if (timeSheet.Absent == "VC")
                                {
                                    if (explaination.Actual == StringConstants.Leave)
                                    {
                                        listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Px";
                                    }
                                    else if (explaination.Actual == StringConstants.ForgetToCheck)
                                    {
                                        listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "x";
                                    }
                                }
                                break;
                            case 3:
                                if (string.IsNullOrEmpty(timeSheet.Absent))
                                {
                                    listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "x";
                                }
                                if (timeSheet.Absent == "V")
                                {
                                    if (explaination.Actual == StringConstants.Leave)
                                    {
                                        listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "RxPx";
                                    }
                                    else if (explaination.Actual == StringConstants.ForgetToCheck)
                                    {
                                        listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Rx";
                                    }
                                }
                                if (timeSheet.Absent == "VS")
                                {
                                    if (explaination.Actual == StringConstants.Leave)
                                    {
                                        listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Px";
                                    }
                                    else if (explaination.Actual == StringConstants.ForgetToCheck)
                                    {
                                        listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "x";
                                    }
                                }
                                if (timeSheet.Absent == "VC")
                                {
                                    listTimeSheetEx.Where(x => x.Date == timeSheet.DayOfCheck).FirstOrDefault().Status = "Rx";
                                }
                                break;
                        }
                    }
                }
                #endregion
                if (explaination != null && explaination.StatusRequestId == 4)
                {
                    var lstAbnormalUnauthorizedLeave = _abnormalRepository.GetAbnormalById(timeSheet.ID).Where(x => x.ReasonId == 5);
                    if (explaination.Actual == StringConstants.Leave)
                    {
                        foreach (var request in lstRequestAbsent)
                        {

                            switch (request.RequestTypeId)
                            {
                                case 2:
                                    if (lstAbnormalUnauthorizedLeave.Where(x => x.FingerTimeSheet.Absent == "V").ToList().Count() > 0)
                                    {
                                        TotalAuthorizedLeavesInPeriod += (float)0.5;
                                    }
                                    if (lstAbnormalUnauthorizedLeave.Where(x => x.FingerTimeSheet.Absent == "VC").ToList().Count() > 0)
                                    {
                                        TotalAuthorizedLeavesInPeriod += (float)0.5;
                                    }
                                    break;
                                case 3:
                                    if (lstAbnormalUnauthorizedLeave.Where(x => x.FingerTimeSheet.Absent == "V").ToList().Count() > 0)
                                    {
                                        TotalAuthorizedLeavesInPeriod += (float)0.5;
                                    }
                                    if (lstAbnormalUnauthorizedLeave.Where(x => x.FingerTimeSheet.Absent == "VS").ToList().Count() > 0)
                                    {
                                        TotalAuthorizedLeavesInPeriod += (float)0.5;
                                    }
                                    break;
                                default:
                                    break;
                            }

                        }
                    }
                    else if (explaination.Actual == StringConstants.ForgetToCheck)
                    {
                        foreach (var request in lstRequestAbsent)
                        {
                            switch (request.RequestTypeId)
                            {
                                case 2:
                                    if (lstAbnormalUnauthorizedLeave.Where(x => x.FingerTimeSheet.Absent == "VC").ToList().Count() > 0)
                                    {
                                        TotalAuthorizedLeavesInPeriod -= (float)0.5;
                                    }
                                    break;
                                case 3:
                                    if (lstAbnormalUnauthorizedLeave.Where(x => x.FingerTimeSheet.Absent == "VS").ToList().Count() > 0)
                                    {
                                        TotalAuthorizedLeavesInPeriod -= (float)0.5;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        var lstAbnormalUnusedAuthorizedLeave = _abnormalRepository.GetAbnormalById(timeSheet.ID).Where(x => x.ReasonId == 6);
                        foreach (var request in lstRequestAbsent)
                        {
                            switch (request.RequestTypeId)
                            {
                                case 2:
                                    if (lstAbnormalUnusedAuthorizedLeave.Where(x => x.FingerTimeSheet.Absent == "VC").ToList().Count() > 0)
                                    {
                                        TotalAuthorizedLeavesInPeriod += (float)0.5;
                                    }
                                    break;
                                case 3:
                                    if (lstAbnormalUnusedAuthorizedLeave.Where(x => x.FingerTimeSheet.Absent == "VS").ToList().Count() > 0)
                                    {
                                        TotalAuthorizedLeavesInPeriod += (float)0.5;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    continue;
                }
                var lstAbnormalUnusedAuthorizedLeaveNoExplain = _abnormalRepository.GetAbnormalById(timeSheet.ID).Where(x => x.ReasonId == 6);
                foreach (var request in lstRequestAbsent)
                {
                    switch (request.RequestTypeId)
                    {
                        case 1:
                            if (lstAbnormalUnusedAuthorizedLeaveNoExplain.Where(x => string.IsNullOrEmpty(x.FingerTimeSheet.Absent)).ToList().Count() > 0)
                            {
                                TotalAuthorizedLeavesInPeriod += 1;
                            }
                            else if (lstAbnormalUnusedAuthorizedLeaveNoExplain.Where(x => x.FingerTimeSheet.Absent == "VS").ToList().Count() > 0)
                            {
                                TotalAuthorizedLeavesInPeriod += (float)0.5;
                            }
                            else if (lstAbnormalUnusedAuthorizedLeaveNoExplain.Where(x => x.FingerTimeSheet.Absent == "VC").ToList().Count() > 0)
                            {
                                TotalAuthorizedLeavesInPeriod += (float)0.5;
                            }
                            break;
                        case 2:
                            if (lstAbnormalUnusedAuthorizedLeaveNoExplain.Where(x => string.IsNullOrEmpty(x.FingerTimeSheet.Absent)).ToList().Count() > 0)
                            {
                                TotalAuthorizedLeavesInPeriod += (float)0.5;
                            }
                            else if (lstAbnormalUnusedAuthorizedLeaveNoExplain.Where(x => x.FingerTimeSheet.Absent == "VC").ToList().Count() > 0)
                            {
                                TotalAuthorizedLeavesInPeriod += (float)0.5;
                            }
                            break;
                        case 3:
                            if (lstAbnormalUnusedAuthorizedLeaveNoExplain.Where(x => string.IsNullOrEmpty(x.FingerTimeSheet.Absent)).ToList().Count() > 0)
                            {
                                TotalAuthorizedLeavesInPeriod += (float)0.5;
                            }
                            else if (lstAbnormalUnusedAuthorizedLeaveNoExplain.Where(x => x.FingerTimeSheet.Absent == "VS").ToList().Count() > 0)
                            {
                                TotalAuthorizedLeavesInPeriod += (float)0.5;
                            }
                            break;
                    }

                }
            }
        }
        private bool CheckUserOnsite(string userID, DateTime date)
        {
            return _userOnsiteRepository.GetMulti(x => x.UserID == userID && x.StartDate <= date && date <= x.EndDate).Count() > 0;
        }
        public bool ReImportTimeSheet(out int _countSuccess, TMSDbContext dbContext, out List<FingerTimeSheetTmpErrorModel> listFingerTimeSheetError)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            dbContext.Database.Connection.Open();
            dbContext.Database.CommandTimeout = CommonConstants.TimeExcuteSql;
            bool flag = false;
            string error = "";
            listFingerTimeSheetError = new List<FingerTimeSheetTmpErrorModel>();
            List<FingerTimeSheetTmp> listTimeSheetTmp = _fingerTimeSheetTmpRepository.GetAll().ToList();
            //list date from time sheet
            List<string> listDate = _fingerTimeSheetTmpRepository.GetAll().Select(x => x.Date.ToShortDateString()).Distinct().ToList();
            int count = 0;
            _countSuccess = 0;
            using (var dbTransaction = dbContext.Database.BeginTransaction())
            {
                foreach (var date in listDate)
                {
                    var shortDate = DateTime.Parse(date);
                    var otrequestAll = _oTRequestRepository.GetMulti(x => x.OTDate == shortDate.Date && x.StatusRequestID == CommonConstants.StatusApprovedID).ToList();
                    var listUserNo = listTimeSheetTmp.Where(x => x.Date.ToShortDateString() == date).Select(x => x.UserNo).Distinct().ToList();

                    var listUser = _fingerMachineUserRepository.GetMulti(x => listUserNo.Contains(x.ID)).Select(x => x.UserId).Distinct().ToList();
                    // timeDay include timeDay for DateOffset
                    var timeDay = _commonService.GetTimeDay(shortDate);
                    var isHolidayOrDayOff = _commonService.IsHolidayOrDayOff(shortDate);
                    foreach (var userID in listUser)
                    {
                        #region Code
                        try
                        {
                            var lstUserNo = _fingerMachineUserRepository.GetMulti(x => x.UserId == userID).Select(x => x.ID);
                            if (lstUserNo.Count() == 0)
                                continue;
                            var listTimeSheetTmpUser = listTimeSheetTmp.Where(x => lstUserNo.Contains(x.UserNo) && x.Date.ToShortDateString() == date);
                            var otRequest = _oTRequestRepository.GetMulti(x => x.OTDate.Value == shortDate && x.StatusRequest.Name.Equals(CommonConstants.StatusApproved)
                                            && x.OTRequestUser.Where(y => y.UserID == userID).Count() > 0, new string[] { CommonConstants.OTRequestUser, CommonConstants.StatusRequest }).FirstOrDefault();
                            error = _fingerTimeSheetRepository.GetMulti(x => x.DayOfCheck == shortDate && lstUserNo.Contains(x.UserNo)).Count() > 0 ? MessageSystem.ErrorDuplicateData : string.Empty;
                            if (!string.IsNullOrEmpty(error))
                            {
                                FingerTimeSheetTmpErrorModel fingerTimeSheetTmpError = new FingerTimeSheetTmpErrorModel();
                                fingerTimeSheetTmpError.UserNo = CommonConstants.StringEmpty;
                                fingerTimeSheetTmpError.Date = date;
                                fingerTimeSheetTmpError.NumberFinger = listTimeSheetTmp.Where(x => lstUserNo.Contains(x.UserNo)).Select(x => x.NumberFinger.ToString()).FirstOrDefault();
                                fingerTimeSheetTmpError.UserName = listTimeSheetTmp.Where(x => lstUserNo.Contains(x.UserNo)).Select(x => x.AccName).Distinct().FirstOrDefault();
                                fingerTimeSheetTmpError.Error = error;
                                listFingerTimeSheetError.Add(fingerTimeSheetTmpError);
                                flag = false;
                                continue;
                            }
                            var timeSheetUserCheckIn = timeDay != null ? listTimeSheetTmpUser.Where(x => TimeSpan.Parse(x.Date.ToString(CommonConstants.FORMAT_HHMM)) <= TimeSpan.Parse(timeDay.CheckIn)).OrderByField(CommonConstants.ORDERBY_DATE, true).FirstOrDefault() : null;
                            timeSheetUserCheckIn = timeDay != null && timeSheetUserCheckIn == null ? listTimeSheetTmpUser.Where(x => TimeSpan.Parse(x.Date.ToString(CommonConstants.FORMAT_HHMM)) > TimeSpan.Parse(timeDay.CheckIn)
                                    && TimeSpan.Parse(x.Date.ToString(CommonConstants.FORMAT_HHMM)) < TimeSpan.Parse(CommonConstants.ABSENT_HOUR_AFTERNOON))
                                .OrderByField(CommonConstants.ORDERBY_DATE, true).FirstOrDefault() : timeSheetUserCheckIn;
                            FingerTimeSheetTmp timeSheetUserCheckOut = null;
                            TimeSpan OTCheckIn = TimeSpan.Zero;
                            TimeSpan OTCheckOut = TimeSpan.Zero;
                            if (isHolidayOrDayOff)
                            {
                                if (otRequest != null)
                                {
                                    otrequestAll.Remove(otRequest);
                                }
                            }
                            if (otRequest == null)
                            {
                                var list = listTimeSheetTmpUser.Where(x => TimeSpan.Parse(x.Date.ToString(CommonConstants.FORMAT_HHMM)) >= TimeSpan.Parse(timeDay.CheckOut)).OrderByField(CommonConstants.ORDERBY_DATE, false);
                                timeSheetUserCheckOut = timeDay != null ? listTimeSheetTmpUser.Where(x => TimeSpan.Parse(x.Date.ToString(CommonConstants.FORMAT_HHMM)) >= TimeSpan.Parse(timeDay.CheckOut)).OrderByField(CommonConstants.ORDERBY_DATE, false).FirstOrDefault() : null;

                            }
                            else
                            {
                                //get record userCheckout with case user register ot
                                timeSheetUserCheckOut = timeDay != null ? listTimeSheetTmpUser.Where(x => TimeSpan.Parse(x.Date.ToString(CommonConstants.FORMAT_HHMM)) >= TimeSpan.Parse(timeDay.CheckOut)
                                    && TimeSpan.Parse(x.Date.ToString(CommonConstants.FORMAT_HHMM)) < TimeSpan.Parse(otRequest.StartTime))
                                .OrderByField(CommonConstants.ORDERBY_DATE, true).FirstOrDefault() : null;
                                timeSheetUserCheckOut = timeDay != null && timeSheetUserCheckOut == null ? listTimeSheetTmpUser.Where(x => TimeSpan.Parse(x.Date.ToString(CommonConstants.FORMAT_HHMM)) < TimeSpan.Parse(timeDay.CheckOut)
                                    && TimeSpan.Parse(x.Date.ToString(CommonConstants.FORMAT_HHMM)) > TimeSpan.Parse(timeDay.CheckIn))
                                .OrderByField(CommonConstants.ORDERBY_DATE, false).FirstOrDefault() : timeSheetUserCheckOut;
                                var timeMidOT = TimeSpan.FromTicks(TimeSpan.Parse(otRequest.StartTime).Ticks + (TimeSpan.Parse(otRequest.EndTime) - TimeSpan.Parse(otRequest.StartTime)).Ticks / 2);
                                var finalTimeSheetTmp = listTimeSheetTmpUser.Where(x => timeDay != null && TimeSpan.Parse(x.Date.ToString(CommonConstants.FORMAT_HHMM)) >= TimeSpan.Parse(timeDay.CheckOut)).OrderByField(CommonConstants.ORDERBY_DATE, false).FirstOrDefault();
                                var finalTimeSheet = finalTimeSheetTmp != null ? TimeSpan.Parse(finalTimeSheetTmp.Date.ToString(CommonConstants.FORMAT_HHMM)) : TimeSpan.Zero;
                                if (finalTimeSheet != TimeSpan.Zero)
                                {
                                    if (finalTimeSheet <= timeMidOT)
                                    {
                                        OTCheckIn = TimeSpan.Parse(listTimeSheetTmpUser.OrderByField(CommonConstants.ORDERBY_DATE, true).FirstOrDefault().Date.ToString(CommonConstants.FORMAT_HHMM));
                                        if (OTCheckIn <= TimeSpan.Parse(otRequest.StartTime))
                                        {
                                            OTCheckIn = TimeSpan.Parse(otRequest.StartTime);
                                        }
                                    }
                                    else
                                    {
                                        if (finalTimeSheet >= TimeSpan.Parse(otRequest.EndTime))
                                        {
                                            OTCheckOut = TimeSpan.Parse(otRequest.EndTime);
                                        }
                                        else
                                        {
                                            OTCheckOut = finalTimeSheet;
                                        }
                                        OTCheckIn = TimeSpan.Parse(otRequest.StartTime);
                                    }
                                }
                            }
                            timeSheetUserCheckOut = timeDay != null && timeSheetUserCheckOut == null ? listTimeSheetTmpUser.Where(x => TimeSpan.Parse(x.Date.ToString(CommonConstants.FORMAT_HHMM)) < TimeSpan.Parse(timeDay.CheckOut)
                                    && TimeSpan.Parse(x.Date.ToString(CommonConstants.FORMAT_HHMM)) > TimeSpan.Parse(timeDay.CheckIn))
                                .OrderByField(CommonConstants.ORDERBY_DATE, false).FirstOrDefault() : timeSheetUserCheckOut;
                            if (timeDay == null && otRequest == null)
                                continue;
                            count++;
                            //Daclare new timesheet
                            FingerTimeSheet fingerTimeSheet = new FingerTimeSheet();
                            fingerTimeSheet.UserNo = _fingerMachineUserRepository.GetMulti(x => x.UserId == userID).FirstOrDefault().ID;
                            fingerTimeSheet.DayOfCheck = shortDate;
                            fingerTimeSheet.CheckIn = timeSheetUserCheckIn != null ? timeSheetUserCheckIn.Date.ToString(CommonConstants.FORMAT_HHMM) : null;
                            if (listTimeSheetTmpUser.Count() == 1)
                            {
                                fingerTimeSheet.CheckOut = TimeSpan.Parse(listTimeSheetTmpUser.OrderByField(CommonConstants.ORDERBY_DATE, false).FirstOrDefault().Date.ToString(CommonConstants.FORMAT_HHMM)) >= TimeSpan.Parse(CommonConstants.ABSENT_HOUR_AFTERNOON) ? listTimeSheetTmpUser.OrderByField(CommonConstants.ORDERBY_DATE, false).FirstOrDefault().Date.ToString(CommonConstants.FORMAT_HHMM) : null;
                            }
                            else if (listTimeSheetTmpUser.Count() > 1)
                            {
                                fingerTimeSheet.CheckOut = listTimeSheetTmpUser.OrderByField(CommonConstants.ORDERBY_DATE, false).FirstOrDefault().Date.ToString(CommonConstants.FORMAT_HHMM);
                            }
                            fingerTimeSheet.OTCheckIn = OTCheckIn != TimeSpan.Zero ? OTCheckIn.ToString(@"hh\:mm") : null;
                            fingerTimeSheet.OTCheckOut = OTCheckOut != TimeSpan.Zero ? OTCheckOut.ToString(@"hh\:mm") : null;
                            var childcareLeave = GetChildcareLeaveByUserID(userID);
                            if (fingerTimeSheet.CheckIn != null)
                            {
                                if ((childcareLeave == null || !childcareLeave.IsLateComing || shortDate < childcareLeave.StartDate || shortDate > childcareLeave.EndDate) && timeDay != null && fingerTimeSheet.CheckIn != null && TimeSpan.Parse(fingerTimeSheet.CheckIn) > TimeSpan.Parse(timeDay.CheckIn) && TimeSpan.Parse(fingerTimeSheet.CheckIn) <= TimeSpan.Parse(CommonConstants.ABSENT_HOUR_MORNING))
                                {
                                    fingerTimeSheet.Late = (TimeSpan.Parse(fingerTimeSheet.CheckIn) - TimeSpan.Parse(timeDay.CheckIn)).ToString(@"hh\:mm");
                                }
                                else if (childcareLeave != null && childcareLeave.IsLateComing && shortDate > childcareLeave.StartDate && shortDate < childcareLeave.EndDate && timeDay != null && fingerTimeSheet.CheckIn != null && TimeSpan.Parse(fingerTimeSheet.CheckIn) > TimeSpan.Parse(timeDay.CheckIn) + TimeSpan.FromHours(childcareLeave.Time) && TimeSpan.Parse(fingerTimeSheet.CheckIn) <= TimeSpan.Parse(CommonConstants.ABSENT_HOUR_MORNING))
                                {
                                    fingerTimeSheet.Late = (TimeSpan.Parse(fingerTimeSheet.CheckIn) - TimeSpan.Parse(timeDay.CheckIn) - TimeSpan.FromHours(childcareLeave.Time)).ToString(@"hh\:mm");
                                }
                            }
                            else
                            {
                                fingerTimeSheet.Absent = CommonConstants.TimeSheetAbsentMorning;
                                fingerTimeSheet.NumOfWorkingDay = CommonConstants.ZERO_PONT_FIVE;
                            }
                            if (fingerTimeSheet.CheckOut != null)
                            {
                                if ((childcareLeave == null || !childcareLeave.IsEarlyLeaving || shortDate < childcareLeave.StartDate || shortDate > childcareLeave.EndDate) && timeDay != null && fingerTimeSheet.CheckOut != null && TimeSpan.Parse(fingerTimeSheet.CheckOut) < TimeSpan.Parse(timeDay.CheckOut) && TimeSpan.Parse(fingerTimeSheet.CheckOut) >= TimeSpan.Parse(CommonConstants.EARLY_HOUR))
                                {
                                    fingerTimeSheet.LeaveEarly = (TimeSpan.Parse(timeDay.CheckOut) - TimeSpan.Parse(fingerTimeSheet.CheckOut)).ToString(@"hh\:mm");
                                }
                                else if (childcareLeave != null && childcareLeave.IsEarlyLeaving && shortDate >= childcareLeave.StartDate && shortDate <= childcareLeave.EndDate && timeDay != null && fingerTimeSheet.CheckOut != null && TimeSpan.Parse(fingerTimeSheet.CheckOut) + TimeSpan.FromHours(childcareLeave.Time) < TimeSpan.Parse(timeDay.CheckOut) && TimeSpan.Parse(fingerTimeSheet.CheckOut) >= TimeSpan.Parse(CommonConstants.EARLY_HOUR))
                                {
                                    fingerTimeSheet.LeaveEarly = (TimeSpan.Parse(timeDay.CheckOut) - TimeSpan.Parse(fingerTimeSheet.CheckOut) - TimeSpan.FromHours(childcareLeave.Time)).ToString(@"hh\:mm");
                                }
                            }
                            else
                            {
                                fingerTimeSheet.Absent = CommonConstants.TimeSheetAbsentAfternoon;
                                fingerTimeSheet.NumOfWorkingDay = CommonConstants.ZERO_PONT_FIVE;
                            }
                            if (timeDay != null && ((fingerTimeSheet.CheckIn != null && TimeSpan.Parse(fingerTimeSheet.CheckIn) > TimeSpan.Parse(CommonConstants.ABSENT_HOUR_MORNING)) || (fingerTimeSheet.CheckIn == null)))
                                fingerTimeSheet.Absent = CommonConstants.TimeSheetAbsentMorning;
                            if (fingerTimeSheet.CheckIn != null)
                            {
                                //Change  Check Out < 13h to CHeck OUt < 16h
                                if (timeDay != null && ((fingerTimeSheet.CheckIn != null && TimeSpan.Parse(fingerTimeSheet.CheckIn) > TimeSpan.Parse(CommonConstants.ABSENT_HOUR_AFTERNOON))))
                                {
                                    fingerTimeSheet.Absent = CommonConstants.TimeSheetAbsentAfternoon;
                                    fingerTimeSheet.NumOfWorkingDay = CommonConstants.ZERO_PONT_FIVE;
                                }
                            }
                            if (fingerTimeSheet.CheckOut != null)
                            {
                                //Change  Check Out < 13h to CHeck OUt < 16h
                                if (timeDay != null && ((fingerTimeSheet.CheckIn != null && TimeSpan.Parse(fingerTimeSheet.CheckIn) > TimeSpan.Parse(CommonConstants.ABSENT_HOUR_AFTERNOON)) || TimeSpan.Parse(fingerTimeSheet.CheckOut) < TimeSpan.Parse(CommonConstants.ABSENT_HOUR_AFTERNOON)))
                                    fingerTimeSheet.Absent = CommonConstants.TimeSheetAbsentAfternoon;
                                fingerTimeSheet.NumOfWorkingDay = CommonConstants.ZERO_PONT_FIVE;
                            }
                            if (timeDay != null && fingerTimeSheet.CheckIn != null && fingerTimeSheet.CheckOut != null)
                            {
                                if (((TimeSpan.Parse(fingerTimeSheet.CheckIn) > TimeSpan.Parse(CommonConstants.ABSENT_HOUR_MORNING)) && TimeSpan.Parse(fingerTimeSheet.CheckOut) < TimeSpan.Parse(CommonConstants.ABSENT_HOUR_AFTERNOON)))
                                {
                                    fingerTimeSheet.Absent = CommonConstants.TimeSheetAbsent;
                                    fingerTimeSheet.NumOfWorkingDay = CommonConstants.ZERO;
                                }
                            }
                            if (fingerTimeSheet.CheckIn != null)
                            {
                                if (timeDay != null && (TimeSpan.Parse(fingerTimeSheet.CheckIn) > TimeSpan.Parse(CommonConstants.ABSENT_HOUR_MORNING)) && fingerTimeSheet.CheckOut == null)
                                {
                                    fingerTimeSheet.Absent = CommonConstants.TimeSheetAbsent;
                                }
                            }
                            if (fingerTimeSheet.CheckOut != null)
                            {
                                if (timeDay != null && (TimeSpan.Parse(fingerTimeSheet.CheckOut) < TimeSpan.Parse(CommonConstants.ABSENT_HOUR_AFTERNOON)) && fingerTimeSheet.CheckIn == null)
                                {
                                    fingerTimeSheet.Absent = CommonConstants.TimeSheetAbsent;
                                }
                            }
                            if (timeDay != null && fingerTimeSheet.CheckIn == null && fingerTimeSheet.CheckOut == null)
                                fingerTimeSheet.Absent = CommonConstants.TimeSheetAbsent;

                            fingerTimeSheet.NumOfWorkingDay = timeDay != null ? CommonConstants.ONE : 0;
                            if (fingerTimeSheet.Absent == CommonConstants.TimeSheetAbsent)
                                fingerTimeSheet.NumOfWorkingDay = CommonConstants.ZERO;
                            if (timeDay != null && fingerTimeSheet.Absent == CommonConstants.TimeSheetAbsentMorning || fingerTimeSheet.Absent == CommonConstants.TimeSheetAbsentAfternoon)
                                fingerTimeSheet.NumOfWorkingDay = CommonConstants.ZERO_PONT_FIVE;
                            //Check allowance
                            var request = _requestRepository.GetMulti(x => x.UserId == userID && x.StartDate <= shortDate && x.EndDate >= shortDate && x.RequestStatusId == CommonConstants.StatusApprovedID);
                            fingerTimeSheet.MinusAllowance = null;
                            if (fingerTimeSheet.Late != null)
                            {
                                if (request.Count(x => x.RequestTypeId == CommonConstants.RequestTypeLateComming) == 0)
                                {
                                    fingerTimeSheet.MinusAllowance = CommonConstants.FOURTY_PERCENT;
                                }
                            }
                            if (fingerTimeSheet.LeaveEarly != null)
                            {
                                if (request.Count(x => x.RequestTypeId == CommonConstants.RequestTypeEarlyLeaving) == 0)
                                {
                                    fingerTimeSheet.MinusAllowance = CommonConstants.FOURTY_PERCENT;
                                }
                            }
                            if (_commonService.IsHolidayOrDayOff(fingerTimeSheet.DayOfCheck))
                            {
                                fingerTimeSheet.Absent = null;
                                fingerTimeSheet.NumOfWorkingDay = 0;
                                var ListUser = _otrequestUserRepository.GetMulti(x => x.OTRequestID.Equals(otRequest.ID)).ToList();
                                ListUser = ListUser.Where(x => x.UserID != userID).ToList();
                                foreach (var userFinger in ListUser)
                                {
                                    FingerTimeSheet fingerTime = new FingerTimeSheet
                                    {
                                        UserNo = _fingerMachineUserRepository.GetMulti(x => x.UserId == userFinger.UserID).FirstOrDefault().ID,
                                        DayOfCheck = shortDate,
                                        Absent = null,
                                        NumOfWorkingDay = 0
                                    };
                                    _fingerTimeSheetRepository.Add(fingerTime);

                                }

                            }
                            if (_fingerTimeSheetRepository.Add(fingerTimeSheet) != null)
                                _countSuccess++;
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            log.Info(ex.StackTrace);
                            dbTransaction.Rollback();
                            flag = false;
                        }
                        #endregion Code
                    }
                    //Add time sheet ot for dayoff but not go
                    if (isHolidayOrDayOff)
                    {
                        foreach (var item in otrequestAll)
                        {
                            var ListUser = _otrequestUserRepository.GetMulti(x => x.OTRequestID.Equals(item.ID));
                            foreach (var user in ListUser)
                            {
                                FingerTimeSheet fingerTime = new FingerTimeSheet
                                {
                                    UserNo = _fingerMachineUserRepository.GetMulti(x => x.UserId == user.UserID).FirstOrDefault().ID,
                                    DayOfCheck = shortDate,
                                    Absent = null,
                                    NumOfWorkingDay = 0
                                };
                                _fingerTimeSheetRepository.Add(fingerTime);
                            }
                        }
                    }
                }
                if (_countSuccess == count && listFingerTimeSheetError.Count == 0)
                {
                    if (listDate.Count > 1)
                    {
                        listDate.OrderBy(x => x);
                        log.Info("Validate all time sheet " + listDate[0] + "-" + listDate[listDate.Count - 1] + " :Success");
                    }
                    else if (listDate.Count == 1)
                    {
                        log.Info("Validate all time sheet " + listDate[0] + " :Success");
                    }
                    try
                    {
                        Save();
                        dbTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        dbContext.Database.Connection.Close();
                        return false;
                    }
                    flag = true;
                    var shortDate = DateTime.Parse(listDate[0]);
                    if (_commonService.IsHolidayOrDayOff(shortDate))
                    {
                        try
                        {
                            dbContext.Database.ExecuteSqlCommand(AbnormalQuery.ExcuteStoreAbnormalDayOff);
                        }
                        catch (Exception ex)
                        {
                            log.Error("Excute query abnormal weekend:" + ex);
                            flag= false;
                        }
                    }
                    else
                    {
                        try
                        {
                            if(listDate.Count() > 0)
                            {
                                var minDate = listDate[0];
                                var maxDate = listDate[listDate.Count - 1];
                                dbContext.Database.ExecuteSqlCommand(AbnormalQuery.ExecuteStoreAbnormalCaseQuery, new SqlParameter("@minDate", minDate), new SqlParameter("@maxDate", maxDate));
                                dbContext.Database.ExecuteSqlCommand(AbnormalQuery.ExcuteReport);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("Excute query abnormal case and report:" + ex);
                            flag = false;
                        }
                    }
                    dbContext.Database.Connection.Close();
                    var lstdatetime = new List<DateTime>();
                    foreach (var item in listDate)
                    {
                        lstdatetime.Add(DateTime.Parse(item));

                    }
                    Task.Run(() => NotificationAbnormal(lstdatetime));
                }
                else
                {
                    dbContext.Database.Connection.Close();
                    return false;
                    //dbTransaction.Rollback();
                }

            }
            return flag;
        }

        public int CountUserReportEx(string userId)
        {
            return _reportRepository.CountUserReportEx(userId);
        }
    }
}