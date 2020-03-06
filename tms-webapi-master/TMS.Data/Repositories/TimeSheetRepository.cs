using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Common.Constants;
using TMS.Common.ViewModels;
using TMS.Data.Infrastructure;
using TMS.Model.Models;

namespace TMS.Data.Repositories
{
    public interface ITimeSheetRepository : IRepository<TimeSheet>
    {
        /// <summary>
        /// get list time sheet model for 1 member, list contains all information to view 
        /// </summary>
        /// <param name="userID">id of user want to get time sheet</param>
        /// <param name="queryTimeSheet">query after filter</param>
        /// <returns>list TimeSheetModel contains all information to view</returns>
        IEnumerable<TimeSheetModel> GetPagingFilterForMemBer(string userID, IQueryable<TimeSheetModel> queryTimeSheet);
        /// <summary>
        /// get list time sheet model all member, list contains all information to view 
        /// </summary>
        /// <param name="queryTimeSheet">query after filter</param>
        /// <returns>list TimeSheetModel</returns>
        IEnumerable<TimeSheetModel> GetPagingFilterForAll(IQueryable<TimeSheetModel> queryTimeSheet);
        /// <summary>
        /// join tables to get TimeSheetModel
        /// - Join AbnormalCase table to get Explanation ID (use to join Explanation table)
        /// - Join Explanation table to get Approver, Status Explanation
        /// - Join User table to get Name of owner time sheet
        /// - Join User table to get Approver
        /// -Join Status Request to get status of explanation
        /// </summary>
        /// <returns>list TimeSheetModel contains all information to view</returns>
        IQueryable<TimeSheetModel> GetTimeSheetModel();
    }
    public class TimeSheetRepository : RepositoryBase<TimeSheet>, ITimeSheetRepository
    {
        /// <summary>
        /// Constructor of TimeSheet Repository
        /// </summary>
        /// <param name="dbFactory"></param>
        public TimeSheetRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        /// <summary>
        /// get list time sheet model all member, list contains all information to view 
        /// </summary>
        /// <param name="queryTimeSheet">query list timesheet after filter</param>
        /// <returns>list TimeSheetModel contains all information to view</returns>
        public IEnumerable<TimeSheetModel> GetPagingFilterForAll(IQueryable<TimeSheetModel> queryTimeSheet)
        {
            return queryTimeSheet.OrderByDescending(x => x.UserName).ThenByDescending(x => x.DayOfCheck);
        }
        /// <summary>
        /// get list time sheet model for 1 member, list contains all information to view 
        /// </summary>
        /// <param name="userID">id of user want to get time sheet</param>
        /// <param name="queryTimeSheet">query after filter</param>
        /// <returns>list TimeSheetModel contains all information to view</returns>
        public IEnumerable<TimeSheetModel> GetPagingFilterForMemBer(string userID, IQueryable<TimeSheetModel> queryTimeSheet)
        {
            return queryTimeSheet.OrderByDescending(x => x.DayOfCheck).Where(x => x.UserID == userID);
        }
        /// <summary>
        /// join tables to get TimeSheetModel
        /// - Join AbnormalCase table to get Explanation ID (use to join Explanation table)
        /// - Join Explanation table to get Approver, Status Explanation
        /// - Join User table to get Name of owner time sheet
        /// - Join User table to get Approver
        /// -Join Status Request to get status of explanation
        /// </summary>
        /// <returns>query list TimeSheetModel</returns>
        public IQueryable<TimeSheetModel> GetTimeSheetModel()
        {
            return (from timesheet in DbContext.TimeSheets
                    from abnormal in DbContext.AbnormalCases.Where(x => x.TimeSheetID == timesheet.ID).DefaultIfEmpty()
                    from explanation in DbContext.ExplanationRequests.Where(x => x.TimeSheetId == timesheet.ID).DefaultIfEmpty()
                    from appuser in DbContext.Users.Where(x => x.Id == timesheet.UserID)
                    from approver in DbContext.Users.Where(x => x.Id == explanation.UpdatedBy).DefaultIfEmpty()
                    from statusRequest in DbContext.StatusRequests.Where(x => x.ID == explanation.StatusRequestId).DefaultIfEmpty()
                    select new TimeSheetModel
                    {
                        ID = timesheet.ID,
                        UserID = appuser.Id,
                        UserName = appuser.FullName,
                        ApproverName = approver.FullName,
                        DayOfCheck = timesheet.DayOfCheck,
                        CheckIn = timesheet.CheckIn,
                        CheckOut = timesheet.CheckOut,
                        ComeLate = timesheet.ComeLate,
                        ComeBackSoon = timesheet.ComeBackSoon,
                        Absent = timesheet.Absent,
                        NumOfWorkingDay = timesheet.NumOfWorkingDay,
                        MinusAllowance = timesheet.MinusAllowance,
                        Explanation = explanation != null ? true : false,
                        StatusExplanation = statusRequest.Name,
                    });
        }
    }
}
