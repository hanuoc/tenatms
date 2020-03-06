using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TMS.Data.Infrastructure;
using TMS.Model.Models;

namespace TMS.Data.Repositories
{
    public interface IOTRequestRepository : IRepository<OTRequest>
    {
    }
    public class OTRequestRepository : RepositoryBase<OTRequest>, IOTRequestRepository
    {
        /// <summary>
        /// Constructor of OT Request Repository
        /// </summary>
        /// <param name="dbFactory"></param>
        public OTRequestRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
