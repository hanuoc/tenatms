using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using TMS.Common.Constants;
using TMS.Common.ViewModels;
using TMS.Data.Infrastructure;
using TMS.Model.Models;

namespace TMS.Data.Repositories
{
    public interface IAppUserRepository : IRepository<AppUser>
    {
        AppUser GetUserByEmail(string email);

        float CheckEntitleDay(Request request, EntitledayModel entitleDay);
        List<AppUser> GetAllUserByGroup(int groupId);
        AppUser GetGroupLeadByGroup(int groupId);
    }

    public class AppUserRepository : RepositoryBase<AppUser>, IAppUserRepository
    {
        public AppUserRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        //public float CheckEntitleDay(Request request, EntitledayModel entitleDay)
        //{
        //    var user = new UserManager<AppUser>(new UserStore<AppUser>(DbContext)).FindById(request.UserId);
        //    float numberDay = entitleDay.HolidayType.Equals(CommonConstants.AuthorizedLeave) ?((DateTime.Now - user.StartWorkingDay).Days / (365 * 5) + entitleDay.MaxEntitleDay) - entitleDay.NumberDayOff - (float)entitleDay.DayBreak :entitleDay.MaxEntitleDay;
        //    return numberDay;
        //}
        public float CheckEntitleDay(Request request, EntitledayModel entitleDay)
        {
            var user = new UserManager<AppUser>(new UserStore<AppUser>(DbContext)).FindById(request.UserId);
            float numberDay = entitleDay.HolidayType.Equals(CommonConstants.AuthorizedLeave) ? ((DateTime.Now - user.StartWorkingDay).Days / (365 * 5) + entitleDay.MaxEntitleDay) - entitleDay.NumberDayOff : entitleDay.MaxEntitleDay;
            return numberDay;
        }

        public List<AppUser> GetAllUserByGroup(int groupId)
        {
            return DbContext.Users.Where(x => x.GroupId == groupId).ToList();
        }

        public AppUser GetGroupLeadByGroup(int groupId)
        {
            var roleManager= new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(DbContext));
            var groupLeadID = roleManager.FindByName(CommonConstants.GroupLead).Id;
            var groupLead= DbContext.Users.Where(x => x.GroupId == groupId && x.Roles.Any(r => r.RoleId.Equals(groupLeadID))).SingleOrDefault();
            return groupLead;
        }
        public AppUser GetUserByEmail(string email)
        {
            return DbContext.Users.FirstOrDefault(u => u.Email == email);
        }
    }
}