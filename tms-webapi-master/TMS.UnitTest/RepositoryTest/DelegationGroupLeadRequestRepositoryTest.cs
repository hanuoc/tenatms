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
    public class DelegationRequestRepositoryTest
    {
        private TMSDbContext DbContext;
        private IDbFactory dbFactory;
        private IRequestRepository objRequestRepository;
        private IUnitOfWork unitOfWork;
        private UserManager<AppUser> userManager;
        private string UserID1;
        private string UserID2;
        [TestInitialize]
        public void Initialize()
        {
            DbContext = new TMSDbContext();
            dbFactory = new DbFactory();
            objRequestRepository = new RequestRepository(dbFactory);
            unitOfWork = new UnitOfWork(dbFactory);
            userManager = new UserManager<AppUser>(new UserStore<AppUser>(DbContext));
            UserID1 = userManager.FindByName("dmtuong").Id;
            UserID2 = userManager.FindByName("admin").Id;
        }

        /// <summary>
        /// get list request
        /// </summary>
        [TestMethod]
        public void RequestRepositoryGetAllUTCD1()
        {
            var listRequest = objRequestRepository.GetAll().ToList();
            Assert.IsNotNull(listRequest);
            Assert.AreEqual(20, listRequest.Count);
        }

        /// <summary>
        /// get list request by user id 1 grouplead
        /// </summary>
        [TestMethod]
        public void RequestRepositoryGetMultiUTCD1()
        {
            var list = objRequestRepository.GetMulti(x => x.UserId.Equals(UserID1)).ToList();
            Assert.AreEqual(20, list.Count);
        }
    }
}
