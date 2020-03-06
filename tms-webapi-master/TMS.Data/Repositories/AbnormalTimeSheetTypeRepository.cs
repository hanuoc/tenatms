using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Data.Infrastructure;
using TMS.Model.Models;

namespace TMS.Data.Repositories
{
    public interface IAbnormalTimeSheetTypeRepository : IRepository<AbnormalTimeSheetType>
    {
    }

    public class AbnormalTimeSheetTypeRepository : RepositoryBase<AbnormalTimeSheetType>, IAbnormalTimeSheetTypeRepository
    {
        /// <summary>
        /// Constructor of OT Request Repository
        /// </summary>
        /// <param name="dbFactory"></param>
        public AbnormalTimeSheetTypeRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
