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
    public interface IHolidayRepository : IRepository<Holiday>
    {
        bool IsHoliday(DateTime date);
        bool IsWorkingday(DateTime date);
        Holiday GetHolidayForDateOffset(DateTime date);
    }

    public class HolidayRepository : RepositoryBase<Holiday>, IHolidayRepository
    {
        public HolidayRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public Holiday GetHolidayForDateOffset(DateTime date)
        {
            return GetSingleByCondition(x => x.Workingday == date);
        }

        // check if is time day return true
        public bool IsHoliday(DateTime date)
        {
            return GetSingleByCondition(x => x.Date.Day == date.Day && x.Date.Month == date.Month && x.Date.Year == date.Year) != null;
        }
        public bool IsWorkingday(DateTime date)
        {
            return GetSingleByCondition(x => x.Workingday.Value.Day == date.Day && x.Workingday.Value.Month == date.Month && x.Workingday.Value.Year == date.Year) != null;
        }
    }
}
