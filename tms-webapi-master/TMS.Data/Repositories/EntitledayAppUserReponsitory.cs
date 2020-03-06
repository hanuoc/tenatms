using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Data.Infrastructure;
using TMS.Model.Models;

namespace TMS.Data.Repositories
{
    public interface IEntitledayAppUserReponsitory : IRepository<Entitleday_AppUser>
    {

    }
    public class EntitledayAppUserReponsitory : RepositoryBase<Entitleday_AppUser>, IEntitledayAppUserReponsitory
    {
        public EntitledayAppUserReponsitory(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
