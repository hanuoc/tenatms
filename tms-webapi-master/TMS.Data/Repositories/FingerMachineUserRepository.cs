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
	public interface IFingerMachineUserRepository : IRepository<FingerMachineUser>
	{
		IEnumerable<string> GetFingerCodeByUserNo(string userNo);
		List<ReImportModel> ReImportTimeSheet();
	}

	public class FingerMachineUserRepository : RepositoryBase<FingerMachineUser>, IFingerMachineUserRepository
	{
		public FingerMachineUserRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}
		public IEnumerable<string> GetFingerCodeByUserNo(string userNo)
		{
			var data = GetMulti(x => x.ID == userNo).ToList();
			if (data.Count() > 0)
			{
				var userId = data[0].UserId;
				return GetMulti(x => x.UserId == userId).Select(x => x.ID);
			}
			return new List<string>();
		}

		public List<ReImportModel> ReImportTimeSheet()
		{
			var data = DbContext.Database.SqlQuery<ReImportModel>(@"select distinct app1.Id, checkin.CHECKTIME,checkin.USERID from AppUsers as app1
		 Join dbo.FingerMachineUsers as machine1 on app1.Id = machine1.UserId
		 Join dbo.USERINFO as uss on machine1.ID = uss.Badgenumber
		 Join dbo.CHECKINOUT as checkin on checkin.USERID = uss.USERID
		 where checkin.CHECKTIME >= '2019-02-25' and checkin.CHECKTIME < '2019-02-26'  and app1.id not in	(select app.Id from dbo.AppUsers as app
		 Join dbo.FingerMachineUsers as machine on app.Id = machine.UserId
		 Join dbo.FingerTimeSheets as finger on finger.UserNo = machine.ID
		 where  finger.DayOfCheck = '2019-02-25 00:00:00.000'
		 union 
		 (
		 select  appusers.Id from AppUsers
		 join dbo.UserOnsites as useron on appusers.Id = useron.UserID where '2019-02-25 00:00:00.000' Between useron.StartDate and useron.EndDate
		 )
		 union
		(select AppUsers.Id from AppUsers where AppUsers.Id not in  (select distinct AppUsers.Id from AppUsers join dbo.FingerMachineUsers on AppUsers.Id = FingerMachineUsers.UserId))
		 ) and Status =1 ");
			return data.ToList();
		}
	}
}
