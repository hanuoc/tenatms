using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Common.ViewModels;
using TMS.Data.Infrastructure;
using TMS.Model.Models;

namespace TMS.Data.Repositories
{
    public interface IEntitleDayRepository : IRepository<EntitleDay>
    {
        List<EntitledayModel> GetAllEntitleDay(string userID, string groupID, bool isReadAll);
        IEnumerable<EntitledayModel> GetById(int id);
    }
    public class EntitleDayRepository : RepositoryBase<EntitleDay>, IEntitleDayRepository
    {
        /// <summary>
        /// Constructor of EntitleDay Repository
        /// </summary>
        /// <param name="dbFactory"></param>
        public EntitleDayRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        public List<EntitledayModel> GetAllEntitleDay(string userID, string groupID, bool isReadAll)
        {
            if (isReadAll)
            {
                return (from entitleDay in DbContext.EntitleDays
                        join entitleday_AppUser in DbContext.EntitleDay_AppUsers on entitleDay.ID equals (entitleday_AppUser.EntitleDayId)
                        where entitleDay.Status == true && ((entitleday_AppUser.AppUser.Gender == true && entitleDay.ID != 3) || entitleday_AppUser.AppUser.Gender == false && entitleDay.ID != 4)
                        //where entitleday_AppUser.AppUser.GroupId.ToString() == groupID 
                        select new EntitledayModel
                        {
                            IDEntitleday = entitleDay.ID,
                            IDUser = entitleday_AppUser.UserId,
                            EntitleDayAppUserId = entitleday_AppUser.ID,
                            FullName = entitleday_AppUser.AppUser.FullName,
                            UserName = entitleday_AppUser.AppUser.UserName,
                            HolidayType = entitleDay.HolidayType,
                            UnitType = entitleDay.UnitType,
                            MaxEntitleDay = entitleday_AppUser.MaxEntitleDayAppUser,
                            NumberDayOff = entitleday_AppUser.NumberDayOff,
                            RemainDayOff = entitleday_AppUser.MaxEntitleDayAppUser - entitleday_AppUser.NumberDayOff,
                            DayBreak = entitleday_AppUser.DayBreak,
                            Description = entitleDay.Description,
                            AuthorizedLeaveBonus = entitleday_AppUser.AuthorizedLeaveBonus,
                            Note = entitleday_AppUser.Note,
                            Gender = entitleday_AppUser.AppUser.Gender,
                        }).ToList();
            }
            else
            {
                return (from entitleDay in DbContext.EntitleDays
                        join entitleday_AppUser in DbContext.EntitleDay_AppUsers on entitleDay.ID equals (entitleday_AppUser.EntitleDayId)
                        join approver in DbContext.Users on entitleday_AppUser.UserId equals (approver.Id)
                        where entitleday_AppUser.UserId == userID && entitleday_AppUser.EntitleDayId == entitleDay.ID && entitleDay.Status == true && ((entitleday_AppUser.AppUser.Gender == true && entitleDay.ID != 3) || entitleday_AppUser.AppUser.Gender == false && entitleDay.ID != 4)
                        select new EntitledayModel
                        {
                            IDEntitleday = entitleDay.ID,
                            IDUser = entitleday_AppUser.UserId,
                            EntitleDayAppUserId = entitleday_AppUser.ID,
                            FullName = entitleday_AppUser.AppUser.FullName,
                            UserName = entitleday_AppUser.AppUser.UserName,
                            HolidayType = entitleDay.HolidayType,
                            UnitType = entitleDay.UnitType,
                            MaxEntitleDay = entitleday_AppUser.MaxEntitleDayAppUser,
                            NumberDayOff = entitleday_AppUser.NumberDayOff,
                            RemainDayOff = entitleday_AppUser.MaxEntitleDayAppUser - entitleday_AppUser.NumberDayOff,
                            DayBreak = entitleday_AppUser.DayBreak,
                            Description = entitleDay.Description,
                            AuthorizedLeaveBonus = entitleday_AppUser.AuthorizedLeaveBonus,
                            Note = entitleday_AppUser.Note,
                        }).ToList();
            }
        }

        public IEnumerable<EntitledayModel> GetById(int id)
        {
            return (from entitleDay in DbContext.EntitleDays
                    join entitleday_AppUser in DbContext.EntitleDay_AppUsers on entitleDay.ID equals (entitleday_AppUser.EntitleDayId)
                    join approver in DbContext.Users on entitleday_AppUser.UserId equals (approver.Id)
                    where entitleday_AppUser.EntitleDayId == entitleDay.ID && entitleDay.Status == true && entitleday_AppUser.ID == id
                    //where entitleday_AppUser.EntitleDayId == entitleDay.ID && entitleDay.Status == true
                    select new EntitledayModel
                    {
                        IDEntitleday = entitleDay.ID,
                        IDUser = entitleday_AppUser.UserId,
                        EntitleDayAppUserId = entitleday_AppUser.ID,
                        FullName = entitleday_AppUser.AppUser.FullName,
                        UserName = entitleday_AppUser.AppUser.UserName,
                        HolidayType = entitleDay.HolidayType,
                        UnitType = entitleDay.UnitType,
                        MaxEntitleDay = entitleday_AppUser.MaxEntitleDayAppUser,
                        NumberDayOff = entitleday_AppUser.NumberDayOff,
                        RemainDayOff = entitleday_AppUser.MaxEntitleDayAppUser - entitleday_AppUser.NumberDayOff,
                        DayBreak = entitleday_AppUser.DayBreak,
                        Description = entitleDay.Description,
                        AuthorizedLeaveBonus = entitleday_AppUser.AuthorizedLeaveBonus,
                        Note = entitleday_AppUser.Note,
                    }).ToList();
        }
    }
}
