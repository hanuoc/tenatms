using System;
using System.Linq;
using TMS.Common.ViewModels;
using TMS.Data.Infrastructure;
using TMS.Model.Models;

namespace TMS.Data.Repositories
{
    public interface IConfigDelegationRepository : IRepository<ConfigDelegation>
    {
        IQueryable<ConfigDelegationModel> GetUserConfigDelegateModel();
        ConfigDelegation GetDataDelegationByUserId(string userId);
    }

    public class ConfigDelegationRepository : RepositoryBase<ConfigDelegation>, IConfigDelegationRepository
    {
        public ConfigDelegationRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public ConfigDelegation GetDataDelegationByUserId(string userId)
        {
            var model = from configDelgation in DbContext.ConfigDelegations
                        where configDelgation.UserId.Equals(userId)
                        select configDelgation;
            return model.FirstOrDefault();
        }

        public IQueryable<ConfigDelegationModel> GetUserConfigDelegateModel()
        {
            return (from user in DbContext.Users
                    join configDelegation in DbContext.ConfigDelegations on user.Id equals configDelegation.UserId
                    where user.Status == true
                    from userAssign in DbContext.Users.Where(x => x.Id.Equals(configDelegation.AssignTo)).DefaultIfEmpty()
                    select new ConfigDelegationModel
                    {
                        ID = configDelegation.ID,
                        UserId = user.Id,
                        FullName = user.FullName,
                        UserName = user.UserName,
                        AssignTo = configDelegation.AssignTo,
                        EndDate = configDelegation.EndDate,
                        StartDate = configDelegation.StartDate,
                        GroupID = user.GroupId,
                        AssignName = userAssign.FullName
                    }).Distinct();

        }
    }
}