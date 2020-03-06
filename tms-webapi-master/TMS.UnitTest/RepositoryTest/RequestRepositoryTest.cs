using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TMS.Common.Constants;
using TMS.Data;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Model.Models;

namespace TMS.UnitTest.RepositoryTest
{
    [TestClass]
    public class RequestRepositoryTest
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
            dbFactory = new DbFactory();
            DbContext = new TMSDbContext();
            objRequestRepository = new RequestRepository(dbFactory);
            unitOfWork = new UnitOfWork(dbFactory);
            userManager = new UserManager<AppUser>(new UserStore<AppUser>(DbContext));
            UserID1 = userManager.FindByName("vxthien").Id;
            UserID2 = userManager.FindByName("ltdat").Id;
        }
        /// <summary>
        /// get list request
        /// </summary>
        [TestMethod]
        public void RequestRepositoryGetAllUTCD1()
        {
            var listRequest = objRequestRepository.GetAll().ToList();
            Assert.IsNotNull(listRequest);
            Assert.AreEqual(5, listRequest.Count);
        }
        /// <summary>
        /// get list request by user id 1
        /// </summary>
        [TestMethod]
        public void RequestRepositoryGetMultiUTCD1()
        {
            var list = objRequestRepository.GetMulti(x => x.UserId.Equals(UserID1)).ToList();
            Assert.AreEqual(3, list.Count);
        }
        /// <summary>
        /// get list request by userId2
        /// </summary>
        [TestMethod]
        public void RequestRepositoryGetMultiUTCD2()
        {
            var list = objRequestRepository.GetMulti(x => x.UserId.Equals(UserID2)).ToList();
            Assert.AreEqual(2, list.Count);
        }
        /// <summary>
        /// check read all request by userid 1
        /// </summary>
        [TestMethod]
        public void RequestRepositoryIsReadAllUTCD1()
        {
            var result = objRequestRepository.IsReadAll(UserID1, CommonConstants.FunctionRequest);
            Assert.AreEqual(true, result);
        }
        /// <summary>
        /// check read all request by userid 2
        /// </summary>
        [TestMethod]
        public void RequestRepositoryIsReadAllUTCD2()
        {
            var result = objRequestRepository.IsReadAll(UserID2, CommonConstants.FunctionRequest);
            Assert.AreEqual(false, result);
        }
    }
}