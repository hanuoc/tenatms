using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Common.ViewModels;
using TMS.Data.Infrastructure;
using TMS.Model.Models;

namespace TMS.Data.Repositories
{
    public interface IExplanationRequestRepository : IRepository<ExplanationRequest>
    {
        /// <summary>
        /// Get abnormal cases in junction table by list abnormal reason
        /// </summary>
        /// <param name="reasonList">string arrays reasons to filter</param>
        /// <returns>List abnormal(string type)</returns>
        IEnumerable<AbnormalCase> GetAbnormalById(int id);
        List<string> GetAbnormalByReasonId(string[] id);
        void DeleteExplanation(int timeSheetId);
    }
    public class ExplanationRequestRepository : RepositoryBase<ExplanationRequest>, IExplanationRequestRepository
    {
        TMSDbContext dbContext;
        public ExplanationRequestRepository(IDbFactory dbFactory) : base(dbFactory)
        {
            dbContext = new TMSDbContext();
        }

        /// <summary>
        /// Get abnormal cases in junction table by list abnormal reason
        /// </summary>
        /// <param name="reasonList">string arrays reasons to filter</param>
        /// <returns>List abnormal(string type)</returns>
        //public List<string> GetAbnormalByReason(string[] reasonList)
        //{
        //    return dbContext.AbnormalCaseReasons.Where(m => reasonList.Contains(m.AbnormalReason.ID.ToString())).Select(m => m.AbnormalCase.ID.ToString()).ToList();
        //}
        
        /// <summary>
        /// Get AbnormalReason by timesheet id
        /// </summary>
        /// <param name="id">id of abnormal</param>
        /// <returns> AbnormalCaseReason </returns>
        public IEnumerable<AbnormalCase> GetAbnormalById(int id)
        {
            return dbContext.AbnormalCases.Where(m => m.TimeSheetID == id).Include(p => p.AbnormalReason).ToList();
        }

        public List<string> GetAbnormalByReasonId(string[] id)
        {
            return dbContext.AbnormalCases.Where(x => id.Contains(x.AbnormalReason.ID.ToString())).Select(p => p.TimeSheetID.ToString()).ToList();
        }

        public void DeleteExplanation(int timeSheetId)
        {
            Delete(DbContext.ExplanationRequests.Where(x => x.TimeSheetId == timeSheetId).FirstOrDefault());
        }
    }
}
