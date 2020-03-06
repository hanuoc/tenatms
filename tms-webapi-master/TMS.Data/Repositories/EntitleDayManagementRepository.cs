using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Common.ViewModels;
using TMS.Data.Infrastructure;
using TMS.Model.Models;

namespace TMS.Data.Repositories
{
    public interface IEntitleDayManagementRepository : IRepository<EntitleDay>
    {
       
    }
    public class EntitleDayManagementRepository : RepositoryBase<EntitleDay>, IEntitleDayManagementRepository
    {
        public EntitleDayManagementRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        
    }
}