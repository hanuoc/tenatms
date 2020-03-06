using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Data.Infrastructure;
using TMS.Model.Models;

namespace TMS.Data.Repositories
{
    public interface IAbnormalReasonRepository : IRepository<AbnormalReason> { }
    public class AbnormalReasonRepository : RepositoryBase<AbnormalReason>, IAbnormalReasonRepository
    {
        public AbnormalReasonRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
