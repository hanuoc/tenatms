using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Data.Infrastructure;
using TMS.Model.Models;

namespace TMS.Data.Repositories
{
    public interface IUserOnsiteRepository : IRepository<UserOnsite>
    {
        IEnumerable<UserOnsite> GetUserOnsite(string userID);
        bool isOnsite(string userID, DateTime date);
        int CountUserOnsite();
    }
    public class UserOnsiteRepository : RepositoryBase<UserOnsite>, IUserOnsiteRepository
    {
        public UserOnsiteRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public int CountUserOnsite()
        {
            return GetAll().Select(x => x.UserID).Distinct().Count();
        }

        public IEnumerable<UserOnsite> GetUserOnsite(string userID)
        {
            return DbContext.UserOnsites.Where(x => x.UserID == userID);
        }
        public bool isOnsite(string userID, DateTime date)
        {
            return GetMulti(x => x.UserID == userID && x.StartDate <= date.Date && x.EndDate >= date.Date).ToList().Count > 0;
        }
    }
}
