using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Data.Infrastructure;
using TMS.Model.Models;

namespace TMS.Data.Repositories
{
    public interface IStatusRequestRepository : IRepository<StatusRequest>
    {

    }
    public class StatusRequestRepository : RepositoryBase<StatusRequest>, IStatusRequestRepository
    {
        public StatusRequestRepository(IDbFactory dbFactory) : base(dbFactory)
        { }
    }
}