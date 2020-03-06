using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Data.Infrastructure;
using TMS.Model.Models;

namespace TMS.Data.Repositories
{
    public interface IOTRequestUserRepository : IRepository<OTRequestUser>
    {
    }
    public class OTRequestUserRepository : RepositoryBase<OTRequestUser>, IOTRequestUserRepository
    {
        public OTRequestUserRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
