using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Common.Constants;
using TMS.Common.ViewModels;
using TMS.Data.Infrastructure;
using TMS.Model.Models;

namespace TMS.Data.Repositories
{
    public interface IAbnormalCaseRepository : IRepository<AbnormalCase>
    {
        IEnumerable<AbnormalCase> GetAbnormalById(int timesheetId);
        IQueryable<AbnormalCaseModel> GetAllAbnormal(string userId);
        void DeleteAbnormalCase(int timeSheetId);
        IEnumerable<AbnormalChartPercentModel> GetAbnormalChart();
        IEnumerable<AbnormalChartModel> GetDataAbnormal();
        
    }
    public class AbnormalCaseRepository : RepositoryBase<AbnormalCase>, IAbnormalCaseRepository
    {
        public AbnormalCaseRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
        public IQueryable<AbnormalCaseModel> GetAllAbnormal(string userId)
        {
            if (IsReadAll(userId, CommonConstants.FunctionAbnormalCase))
            {
                return  (from abnormal in DbContext.AbnormalCases
                          join explan in DbContext.ExplanationRequests
                          on abnormal.TimeSheetID equals explan.TimeSheetId
                          into ex
                          from e in ex.DefaultIfEmpty()
                          select new AbnormalCaseModel
                          {
                              ID = abnormal.ID,
                              TimeSheetID = abnormal.TimeSheetID,
                              UserName = abnormal.FingerTimeSheet.FingerMachineUsers.AppUser.UserName,
                              FullName = abnormal.FingerTimeSheet.FingerMachineUsers.AppUser.FullName,
                              GroupName = abnormal.FingerTimeSheet.FingerMachineUsers.AppUser.Group.Name,
                              AbnormalDate = abnormal.FingerTimeSheet.DayOfCheck,
                              Absent = abnormal.FingerTimeSheet.Absent,
                              CheckIn = abnormal.FingerTimeSheet.CheckIn,
                              CheckOut = abnormal.FingerTimeSheet.CheckOut,
                              AbnormalReason = DbContext.AbnormalCases.Where(m => m.TimeSheetID == abnormal.TimeSheetID).Select(m => new AbnormalReasonModel
                              {
                                  ID = m.AbnormalReason.ID,
                                  Name = m.AbnormalReason.Name
                              }).OrderBy(x => x.Name).ToList(),
                              Approver = e.UpdatedBy == null ? "" : e.AppUserUpdatedBy.FullName,
                              StatusRequest = e.StatusRequest.Name
                          }).OrderBy(x => x.FullName).ThenByDescending(x=>x.AbnormalDate).AsQueryable();
            }
            else
            {
                return (from abnormal in DbContext.AbnormalCases
                        join explan in DbContext.ExplanationRequests
                        on abnormal.TimeSheetID equals explan.TimeSheetId
                        into ex
                        from e in ex.DefaultIfEmpty()
                        where abnormal.FingerTimeSheet.FingerMachineUsers.AppUser.Id == userId
                        select new AbnormalCaseModel
                        {
                            ID = abnormal.ID,
                            TimeSheetID = abnormal.TimeSheetID,
                            UserName = abnormal.FingerTimeSheet.FingerMachineUsers.AppUser.UserName,
                            FullName = abnormal.FingerTimeSheet.FingerMachineUsers.AppUser.FullName,
                            GroupName = abnormal.FingerTimeSheet.FingerMachineUsers.AppUser.Group.Name,
                            AbnormalDate = abnormal.FingerTimeSheet.DayOfCheck,
                            Absent = abnormal.FingerTimeSheet.Absent,
                            CheckIn = abnormal.FingerTimeSheet.CheckIn,
                            CheckOut = abnormal.FingerTimeSheet.CheckOut,
                            AbnormalReason = DbContext.AbnormalCases.Where(m => m.TimeSheetID == abnormal.TimeSheetID).Select(m => new AbnormalReasonModel
                            {
                                ID = m.AbnormalReason.ID,
                                Name = m.AbnormalReason.Name
                            }).OrderBy(x => x.Name).ToList(),
                            Approver = e.UpdatedBy == null ? "" : e.AppUserUpdatedBy.FullName,
                            StatusRequest = e.StatusRequest.Name
                        }).OrderByDescending(x => x.AbnormalDate).AsQueryable();
            }
        }
        public IEnumerable<AbnormalCase> GetAbnormalById(int timesheetId)
        {
            return DbContext.AbnormalCases.Where(m => m.TimeSheetID == timesheetId).Include(p => p.AbnormalReason).ToList();
        }

        public void DeleteAbnormalCase(int timeSheetId)
        {
            IEnumerable<AbnormalCase> abnormals = DbContext.AbnormalCases.Where(x => x.TimeSheetID == timeSheetId);
            foreach (var abnormal in abnormals)
            {
                Delete(abnormal);
            }
        }

        public IEnumerable<AbnormalChartPercentModel> GetAbnormalChart()
        {
            var DateNow = DateTime.Now;
            var firstDayOfMonth = new DateTime(DateNow.Year, DateNow.Month, 1);
            var query = (from abnormal in DbContext.AbnormalCases
                         join abnormalReason in DbContext.AbnormalReasons on abnormal.ReasonId equals abnormalReason.ID
                         join finger in DbContext.FingerTimeSheets on abnormal.TimeSheetID equals finger.ID
                         join explanation in DbContext.ExplanationRequests on finger.ID equals explanation.TimeSheetId
                         where finger.DayOfCheck > firstDayOfMonth && finger.DayOfCheck < DateNow
                         select new AbnormalChartPercentModel
                         {
                             ReasonID = abnormal.ReasonId,
                             StatusRequestID = explanation.StatusRequestId
                         }
                         );
                         
            return query;
        }

        public IEnumerable<AbnormalChartModel> GetDataAbnormal()
        {
            var query = (from abnormal in DbContext.AbnormalCases
                         join fingertimesheet in DbContext.FingerTimeSheets on abnormal.TimeSheetID equals fingertimesheet.ID
                         join fingermachine in DbContext.FingerMachineUsers on fingertimesheet.UserNo equals fingermachine.ID
                         join user in DbContext.Users on fingermachine.UserId equals user.Id
                         select new AbnormalChartModel
                         {
                             ReasonId = abnormal.ReasonId,
                             GroupId = user.GroupId,
                             FingerTimeSheet = fingertimesheet.DayOfCheck
                         });

            return query;
        }
    }
}