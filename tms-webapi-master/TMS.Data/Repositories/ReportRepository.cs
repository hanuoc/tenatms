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
    public interface IReportRepository : IRepository<Report>
    {
        IEnumerable<ReportModel> GetAllReport(string userId);
        IQueryable<ReportExModel> GetAllReportEx(string userId,int groupID);
        List<Group> GetAllGroup();
        int CountUserReportEx(string userId);
    }
    public class ReportRepository : RepositoryBase<Report>, IReportRepository
    {

        public ReportRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        public IEnumerable<ReportModel> GetAllReport(string userId)
        {
            if(IsReadAll(userId, CommonConstants.FunctionTimeSheet))
            {
                return (from appuser in DbContext.Users
                         join fingertimesheet in DbContext.FingerTimeSheets
                         on appuser.Id equals fingertimesheet.FingerMachineUsers.AppUser.Id
                         join report in DbContext.Reports
                         on fingertimesheet.ID equals report.TimeSheetID
                         into ex
                         from e in ex.DefaultIfEmpty()
                         join entitle in DbContext.EntitleDay_AppUsers
                         on appuser.Id equals entitle.AppUser.Id
                         join explanation in DbContext.ExplanationRequests
                         on fingertimesheet.ID equals explanation.TimeSheetId
                         into explan
                         from exp in explan.DefaultIfEmpty()
                         where entitle.EntitleDay.HolidayType == "Authorized Leave" && appuser.GroupId != null
                         select new ReportModel
                         {
                             UserID = fingertimesheet.FingerMachineUsers.AppUser.Id,
                             FullName = appuser.FullName,
                             TotalEntitleYear = entitle.MaxEntitleDayAppUser,
                             Remain = entitle.MaxEntitleDayAppUser - entitle.NumberDayOff
                         }).OrderByDescending(x=>x.FullName).Distinct();
            }
            return new List<ReportModel>().AsEnumerable();
        }

        public IQueryable<ReportExModel> GetAllReportEx(string userId, int groupID)
        {
            if (IsReadAll(userId, CommonConstants.FunctionTimeSheet))
            {
                return (from appuser in DbContext.Users
                        join fingertimesheet in DbContext.FingerTimeSheets
                        on appuser.Id equals fingertimesheet.FingerMachineUsers.AppUser.Id
                        join report in DbContext.Reports
                        on fingertimesheet.ID equals report.TimeSheetID
                        into ex
                        from e in ex.DefaultIfEmpty()
                        join entitle in DbContext.EntitleDay_AppUsers
                        on appuser.Id equals entitle.AppUser.Id
                        join explanation in DbContext.ExplanationRequests
                        on fingertimesheet.ID equals explanation.TimeSheetId
                        into explan
                        from exp in explan.DefaultIfEmpty()
                        where entitle.EntitleDay.HolidayType == "Authorized Leave" && appuser.GroupId == groupID
                        select new ReportExModel
                        {
                            UserID = appuser.Id,
                            EmployeeID = fingertimesheet.FingerMachineUsers.AppUser.EmployeeID,
                            FullName = appuser.FullName,
                            TotalEntitleDay = entitle.MaxEntitleDayAppUser.ToString(),
                            RemainEntitleDay = entitle.MaxEntitleDayAppUser - entitle.NumberDayOff,
                            //GroupID = appuser.GroupId,
                            //GroupName = appuser.Group.Name
                         }).OrderByDescending(x => x.FullName).Distinct().AsQueryable();
            }
            return new List<ReportExModel>().AsQueryable();
        }
        public int CountUserReportEx(string userId)
        {
            if (IsReadAll(userId, CommonConstants.FunctionTimeSheet))
            {
                return (from appuser in DbContext.Users
                        join fingertimesheet in DbContext.FingerTimeSheets
                        on appuser.Id equals fingertimesheet.FingerMachineUsers.AppUser.Id
                        join report in DbContext.Reports
                        on fingertimesheet.ID equals report.TimeSheetID
                        into ex
                        from e in ex.DefaultIfEmpty()
                        join entitle in DbContext.EntitleDay_AppUsers
                        on appuser.Id equals entitle.AppUser.Id
                        join explanation in DbContext.ExplanationRequests
                        on fingertimesheet.ID equals explanation.TimeSheetId
                        into explan
                        from exp in explan.DefaultIfEmpty()
                        where entitle.EntitleDay.HolidayType == "Authorized Leave" && appuser.GroupId!=null
                        select appuser).Distinct().Count();
            }
            return 0;
        }
        public List<Group> GetAllGroup()
        {
            return DbContext.Groups.ToList();
        }
    }
}
