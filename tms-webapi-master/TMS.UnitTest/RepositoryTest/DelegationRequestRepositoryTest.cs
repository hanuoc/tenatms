using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Data;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Model.Models;

namespace TMS.UnitTest.RepositoryTest
{
    [TestClass]
    public class DelegationGroupLeadRequestRepositoryTestRepositoryTest
    {
        private TMSDbContext DbContext;
        private IDbFactory dbFactory;
        private IRequestRepository objRepository;
        private IUnitOfWork unitOfWork;
        private UserManager<AppUser> userManager;
        private string UserID1;
        private string UserID2;
        [TestInitialize]
        public void Initialize()
        {
            DbContext = new TMSDbContext();
            dbFactory = new DbFactory();
            objRepository = new RequestRepository(dbFactory);
            unitOfWork = new UnitOfWork(dbFactory);
            userManager = new UserManager<AppUser>(new UserStore<AppUser>(DbContext));
            UserID1 = userManager.FindByName("nvthang").Id;
            UserID2 = userManager.FindByName("admin").Id;
        }
        [TestMethod]
        public void RequestRepository_GetMultiUCTID01()
        {
            var list = objRepository.GetMulti(x => x.UserId.Equals(UserID1)).ToList();
            Assert.AreEqual(0, list.Count);
        }
        [TestMethod]
        public void RequestRepository_GetMultiUTCID02()
        {
            var list = objRepository.GetMulti(x => x.UserId.Equals(UserID2)).ToList();
            Assert.AreEqual(1, list.Count);
        }
    }
}
