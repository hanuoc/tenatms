using System.Collections.Generic;
using System.Data.Entity;
using TMS.Common.ViewModels;
using TMS.Data.Infrastructure;
using TMS.Model.Models;
using System.Linq;

namespace TMS.Data.Repositories
{
    public interface IFingerTimeSheetRepository : IRepository<FingerTimeSheet>
    {
        DbContextTransaction BeginTransaction();
        /// <summary>
        /// get list time sheet model for 1 member, list contains all information to view 
        /// </summary>
        /// <param name="userID">id of user want to get time sheet</param>
        /// <param name="queryTimeSheet">query after filter</param>
        /// <returns>list TimeSheetModel contains all information to view</returns>
        IEnumerable<FingerTimeSheetModel> GetPagingFilterForMemBer(string userID, IQueryable<FingerTimeSheetModel> queryTimeSheet);
        /// <summary>
        /// get list time sheet model all member, list contains all information to view 
        /// </summary>
        /// <param name="queryTimeSheet">query after filter</param>
        /// <returns>list TimeSheetModel</returns>
        IEnumerable<FingerTimeSheetModel> GetPagingFilterForAll(IQueryable<FingerTimeSheetModel> queryTimeSheet);
        /// <summary>
        /// join tables to get TimeSheetModel
        /// - Join AbnormalCase table to get Explanation ID (use to join Explanation table)
        /// - Join Explanation table to get Approver, Status Explanation
        /// - Join User table to get Name of owner time sheet
        /// - Join User table to get Approver
        /// -Join Status Request to get status of explanation
        /// </summary>
        /// <returns>list TimeSheetModel contains all information to view</returns>
        IQueryable<FingerTimeSheetModel> GetTimeSheetModel();

        void DeleteFingerTimeSheet(int timeSheetId);
    }

    public class FingerTimeSheetRepository : RepositoryBase<FingerTimeSheet>, IFingerTimeSheetRepository
    {
        public FingerTimeSheetRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public DbContextTransaction BeginTransaction()
        {
            return DbContext.Database.BeginTransaction();
        }
        /// <summary>
        /// get list time sheet model all member, list contains all information to view 
        /// </summary>
        /// <param name="queryTimeSheet">query list timesheet after filter</param>
        /// <returns>list TimeSheetModel contains all information to view</returns>
        public IEnumerable<FingerTimeSheetModel> GetPagingFilterForAll(IQueryable<FingerTimeSheetModel> queryTimeSheet)
        {
            return queryTimeSheet.OrderByDescending(x => x.UserNo).ThenByDescending(x => x.DayOfCheck);
        }
        /// <summary>
        /// get list time sheet model for 1 member, list contains all information to view 
        /// </summary>
        /// <param name="userID">id of user want to get time sheet</param>
        /// <param name="queryTimeSheet">query after filter</param>
        /// <returns>list TimeSheetModel contains all information to view</returns>
        public IEnumerable<FingerTimeSheetModel> GetPagingFilterForMemBer(string userID, IQueryable<FingerTimeSheetModel> queryTimeSheet)
        {
            return queryTimeSheet.OrderByDescending(x => x.DayOfCheck).Where(x => x.UserId == userID);
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
        public IQueryable<FingerTimeSheetModel> GetTimeSheetModel()
        {
            return (from fingerTimesheet in DbContext.FingerTimeSheets
                    from abnormal in DbContext.AbnormalCases.Where(x => x.TimeSheetID == fingerTimesheet.ID).DefaultIfEmpty()
                    from explanation in DbContext.ExplanationRequests.Where(x => x.TimeSheetId == fingerTimesheet.ID).DefaultIfEmpty()
                    from fingerUser in DbContext.FingerMachineUsers.Where(x => x.ID == fingerTimesheet.UserNo)
                    from appuser in DbContext.Users.Where(x => x.Id == fingerUser.UserId)
                    from approver in DbContext.Users.Where(x => x.Id == explanation.UpdatedBy).DefaultIfEmpty()
                    from statusRequest in DbContext.StatusRequests.Where(x => x.ID == explanation.StatusRequestId).DefaultIfEmpty()
                    select new FingerTimeSheetModel
                    {
                        ID = fingerTimesheet.ID,
                        UserNo = fingerUser.ID,
                        UserName = appuser.FullName,
                        UserId = appuser.Id,
                        ApproverName = approver.FullName,
                        DayOfCheck = fingerTimesheet.DayOfCheck,
                        CheckIn = fingerTimesheet.CheckIn,
                        CheckOut = fingerTimesheet.CheckOut,
                        OTCheckIn = fingerTimesheet.OTCheckIn,
                        OTCheckOut = fingerTimesheet.OTCheckOut,
                        Late = fingerTimesheet.Late,
                        LeaveEarly = fingerTimesheet.LeaveEarly,
                        Absent = fingerTimesheet.Absent,
                        NumOfWorkingDay = fingerTimesheet.NumOfWorkingDay,
                        MinusAllowance = fingerTimesheet.MinusAllowance,
                        Explanation = explanation != null ? true : false,
                        StatusExplanation = statusRequest.Name,
                    }).Distinct();
        }

        public void DeleteFingerTimeSheet(int timeSheetId)
        {
            Delete(DbContext.FingerTimeSheets.Where(x => x.ID == timeSheetId).FirstOrDefault());
        }
    }
}