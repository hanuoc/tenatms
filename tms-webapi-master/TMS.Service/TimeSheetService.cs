using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Common.Constants;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Model.Models;
using TMS.Data;
using TMS.Common.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Globalization;

namespace TMS.Service
{
    public interface ITimeSheetService
    {
        /// <summary>
        /// get list time sheet and fillter
        /// </summary>
        /// <param name="userID">id of logged in user</param>
        /// <param name="filter">filter timesheet</param>
        /// <returns>list time sheet model contains all information to view</returns>
        List<TimeSheetModel> GetListTimeSheetFilter(string userID, FilterModel filter);
        /// <summary>
        /// Map data field Absent type
        /// </summary>
        /// <param name="model">data time sheet to mapping</param>
        /// <returns>list time sheet model</returns>
        List<TimeSheetModel> MapAbsentField(List<TimeSheetModel> model);
    }
    public class TimeSheetService : ITimeSheetService
    {
        private ITimeSheetRepository _timeSheetRepository;
        private IAbnormalCaseRepository _abnormalCaseRepository;
        private IExplanationRequestRepository _explanationRequestRepository;
        private IUnitOfWork _unitOfWork;
        /// <summary>
        /// Constructor TimeSheet Service
        /// </summary>
        /// <param name="TimeSheetRepository"></param>
        /// <param name="unitOfWork"></param>
        public TimeSheetService(ITimeSheetRepository timeSheetRepository, IAbnormalCaseRepository abnormalCaseRepository,
            IExplanationRequestRepository explanationReQuestRepository, IUnitOfWork unitOfWork)
        {
            this._timeSheetRepository = timeSheetRepository;
            this._abnormalCaseRepository = abnormalCaseRepository;
            this._explanationRequestRepository = explanationReQuestRepository;
            this._unitOfWork = unitOfWork;
        }
        /// <summary>
        /// get list time sheet and fillter
        /// </summary>
        /// <param name="userID">id of logged in user</param>
        /// <param name="filter">filter timesheet</param>
        /// <returns>list time sheet model contains all information to view</returns>
        public List<TimeSheetModel> GetListTimeSheetFilter(string userID, FilterModel filter)
        {
            var model = _timeSheetRepository.GetTimeSheetModel();
            if (filter != null)
            {
                if (filter.FromDate != null && filter.ToDate != null)
                {
                    DateTime startDate = DateTime.ParseExact(filter.FromDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture);
                    DateTime endDate = DateTime.ParseExact(filter.ToDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture);
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
                        ((filter.AbnormalTimeSheetType.Contains(StringConstants.FilterTimeSheetComeLate)) ? x.ComeLate : false) ||
                        ((filter.AbnormalTimeSheetType.Contains(StringConstants.FilterTimeSheetComeBackSoon)) ? x.ComeBackSoon : false));
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
            if (_timeSheetRepository.IsReadAll(userID,CommonConstants.FunctionTimeSheet))
            {
                return _timeSheetRepository.GetPagingFilterForAll(model).ToList();
            }
            return _timeSheetRepository.GetPagingFilterForMemBer(userID, model).ToList();
        }
        /// <summary>
        /// Map data field Absent Type
        /// </summary>
        /// <param name="model">data time sheet model to mapping</param>
        /// <returns>list time sheet model contains all information to view</returns>
        public List<TimeSheetModel> MapAbsentField(List<TimeSheetModel> model)
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
    }
}
