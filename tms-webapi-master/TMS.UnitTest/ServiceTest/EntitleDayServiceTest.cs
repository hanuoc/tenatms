using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Common.Constants;
using TMS.Data;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Model.Models;

namespace TMS.UnitTest.ServiceTest
{
    [TestClass]
    public class EntitleDayServiceTest
    {
        private TMSDbContext DbContext;
        private IDbFactory dbFactory;
        private UserManager<AppUser> userManager;
        private IEntitleDayRepository entitleDayRepository;
        private IUnitOfWork unitOfWork;
        private string UserID1;
        private string UserID2;
        private string groupID1;
        [TestInitialize]
        public void Initialize()
        {
            DbContext = new TMSDbContext();
            dbFactory = new DbFactory();
            unitOfWork = new UnitOfWork(dbFactory);
            entitleDayRepository = new EntitleDayRepository(dbFactory);
            userManager = new UserManager<AppUser>(new UserStore<AppUser>(DbContext));
            UserID1 = userManager.FindByName("vxthien").Id;
            UserID2 = userManager.FindByName("tqhuy").Id;
            groupID1 = "1";
        }
        [TestMethod]
        public void EntitleDay_Repository_IsReadAllUT1()
        {
            var result = entitleDayRepository.IsReadAll(UserID1, CommonConstants.FunctionEntitleDay);
            Assert.AreEqual(true, result);
        }
        [TestMethod]
        public void EntitleDay_Repository_IsReadAllUT2()
        {
            var result = entitleDayRepository.IsReadAll(UserID2, CommonConstants.FunctionEntitleDay);
            Assert.AreEqual(false, result);
        }
        //[TestMethod]
        //public void EntitleDay_Repository_IsReadAllUT3()
        //{
        //    var list = entitleDayRepository.GetMulti(x => x.UserId.Equals(UserID1)).ToList();
        //    Assert.AreEqual(5, list.Count);
        //}
        //[TestMethod]
        //public void EntitleDay_Repository_IsReadAllUT4()
        //{
        //    var list = entitleDayRepository.GetMulti(x => x.UserId.Equals(UserID2)).ToList();
        //    Assert.AreEqual(1, list.Count);
        //}
    }
}
