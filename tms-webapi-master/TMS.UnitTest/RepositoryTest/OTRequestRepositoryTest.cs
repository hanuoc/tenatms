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

namespace TMS.UnitTest.RepositoryTest
{
    [TestClass]
    public class OTRequestRepositoryTest
    {
        private TMSDbContext DbContext;
        private IDbFactory dbFactory;
        private IOTRequestRepository objRepository;
        private IUnitOfWork unitOfWork;
        private UserManager<AppUser> userManager;
        private string UserID1;
        private string UserID2;
        [TestInitialize]
        public void Initialize()
        {
            DbContext = new TMSDbContext();
            dbFactory = new DbFactory();
            objRepository = new OTRequestRepository(dbFactory);
            unitOfWork = new UnitOfWork(dbFactory);
            userManager = new UserManager<AppUser>(new UserStore<AppUser>(DbContext));
            UserID1 = userManager.FindByName("dmtuong").Id;
            UserID2 = userManager.FindByName("tqhuy").Id;
        }

        [TestMethod]
        public void OTRequest_Repository_IsReadAllUT1()
        {
            var result = objRepository.IsReadAll(UserID1,CommonConstants.FunctionOTRequest);
            Assert.AreEqual(true, result);
        }
        [TestMethod]
        public void OTRequest_Repository_IsReadAllUT2()
        {
            var result = objRepository.IsReadAll(UserID2, CommonConstants.FunctionOTRequest);
            Assert.AreEqual(false, result);
        }
        [TestMethod]
        public void OTRequest_Repository_GetMultiUT1()
        {
            var list = objRepository.GetMulti(x => x.CreatedBy.Equals(UserID1)).ToList();
            Assert.AreEqual(0, list.Count);
        }
        [TestMethod]
        public void OTRequest_Repository_GetMultiUT2()
        {
            var list = objRepository.GetMulti(x => x.CreatedBy.Equals(UserID2)).ToList();
            Assert.AreEqual(1, list.Count);
        }   
        //[TestMethod]
        //public void OTRequest_Repository_GetMultiUT1()
        //{
        //    var list = objRepository.GetMulti(x => x.UserId.Equals(UserID1)).ToList();
        //    Assert.AreEqual(5, list.Count);
        //}
        //[TestMethod]
        //public void OTRequest_Repository_GetMultiUT2()
        //{
        //    var list = objRepository.GetMulti(x => x.UserId.Equals(UserID2)).ToList();
        //    Assert.AreEqual(1, list.Count);
        //}
    }
}
