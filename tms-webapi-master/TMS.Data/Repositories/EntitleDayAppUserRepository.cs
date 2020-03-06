using System.Collections.Generic;
using TMS.Data.Infrastructure;
using TMS.Model.Models;

namespace TMS.Data.Repositories
{
    public interface IEntitleDayAppUserRepository : IRepository<Entitleday_AppUser>
    {
    }
    public class EntitleDayAppUserRepository : RepositoryBase<Entitleday_AppUser>, IEntitleDayAppUserRepository
    {
        public EntitleDayAppUserRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}