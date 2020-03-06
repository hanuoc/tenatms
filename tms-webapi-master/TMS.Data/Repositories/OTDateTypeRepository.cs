using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Data.Infrastructure;
using TMS.Model.Models;

namespace TMS.Data.Repositories
{
    public interface IOTDateTypeRespository : IRepository<OTDateType>
    {
    }
    public class OTDateTypeRepository : RepositoryBase<OTDateType>, IOTDateTypeRespository
    {
        public OTDateTypeRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        public IEnumerable<OTDateType> GetListOTDateType()
        {
            return this.DbContext.OTDateTypes;
        }
    }
}
