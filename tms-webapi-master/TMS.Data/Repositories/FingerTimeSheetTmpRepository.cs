using System;
using TMS.Common.Exceptions.Extensions;
using TMS.Data.Infrastructure;
using TMS.Model.Models;

namespace TMS.Data.Repositories
{
    public interface IFingerTimeSheetTmpRepository : IRepository<FingerTimeSheetTmp>
    {
        void RemoveAllData();
    }

    public class FingerTimeSheetTmpRepository : RepositoryBase<FingerTimeSheetTmp>, IFingerTimeSheetTmpRepository
    {
        public FingerTimeSheetTmpRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public void RemoveAllData()
        {
            DbContext.FingerTimeSheetTmps.Clear();
        }
    }
}