using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Data.Infrastructure;
using TMS.Model.Models;

namespace TMS.Data.Repositories
{
    public interface IRequestReasonTypeRepository : IRepository<RequestReasonType>
    {

    }
    public class RequestReasonTypeRepository : RepositoryBase<RequestReasonType>, IRequestReasonTypeRepository
    {
        public RequestReasonTypeRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
