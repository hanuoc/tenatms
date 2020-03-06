using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Data.Infrastructure;
using TMS.Model.Models;

namespace TMS.Data.Repositories
{
    public interface ITimeDayRepository : IRepository<TimeDay>
    {
        bool IsTimeDay(DateTime date);
    }
    public class TimeDayRepository : RepositoryBase<TimeDay>, ITimeDayRepository
    {
        public TimeDayRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        // check if is time day return true
        public bool IsTimeDay(DateTime date)
        {
            return GetSingleByCondition(x=>x.Workingday == date.DayOfWeek.ToString()) !=null;
        }
    }
}
