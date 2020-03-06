using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Data.Infrastructure;
using TMS.Model.Models;

namespace TMS.Data.Repositories
{
    public interface IJobLogRepository : IRepository<JobLog>
    {
    }
    public class JobLogRepository : RepositoryBase<JobLog>, IJobLogRepository
    {
        public JobLogRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
