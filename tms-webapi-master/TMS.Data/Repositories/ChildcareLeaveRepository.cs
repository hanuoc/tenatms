using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Data.Infrastructure;
using TMS.Model.Models;

namespace TMS.Data.Repositories
{
    public interface IChildcareLeaveRepository : IRepository<ChildcareLeave>
    {
    }
    public class ChildcareLeaveRepository : RepositoryBase<ChildcareLeave>, IChildcareLeaveRepository
    {
        public ChildcareLeaveRepository(IDbFactory dbFactory) : base(dbFactory)
        { }
    }
}
