using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Data.Infrastructure;
using TMS.Model.Models;

namespace TMS.Data.Repositories
{
    public interface IRequestTypeRepository : IRepository<RequestType>
    {

    }
    public class RequestTypeRepository : RepositoryBase<RequestType>, IRequestTypeRepository
    {
        public RequestTypeRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
