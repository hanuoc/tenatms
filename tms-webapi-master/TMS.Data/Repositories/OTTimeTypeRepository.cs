using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Data.Infrastructure;
using TMS.Model.Models;

namespace TMS.Data.Repositories
{
    public interface IOTTimeTypeRepository : IRepository<OTTimeType>
    {
    }
    public class OTTimeTypeRepository : RepositoryBase<OTTimeType>, IOTTimeTypeRepository
    {
        public OTTimeTypeRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        public IEnumerable<OTTimeType> GetListOTTimeType()
        {
            return this.DbContext.OTTimeTypes;
        }
    }
}
