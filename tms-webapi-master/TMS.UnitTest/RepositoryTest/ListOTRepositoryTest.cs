using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Data;
using TMS.Model.Models;
using TMS.Data.Repositories;
using TMS.Data.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;
using TMS.Common.Constants;

namespace TMS.UnitTest.RepositoryTest
{
    [TestClass]
    public class ListOTRepositoryTest
    {
        private TMSDbContext DbContext;
        private IDbFactory dbFactory;
        private IListOTRepository listOTRepository;
        private IUnitOfWork unitOfWork;
        private UserManager<AppUser> userManager;
        private string UserID1;
        private string UserID2;
        private string groupID1;
        private string groupID2;
        [TestInitialize]
        public void Initialize()
        {
            DbContext = new TMSDbContext();
            dbFactory = new DbFactory();
            listOTRepository = new ListOTRepository(dbFactory);
            unitOfWork = new UnitOfWork(dbFactory);
            userManager = new UserManager<AppUser>(new UserStore<AppUser>(DbContext));
            UserID1 = userManager.FindByName("vxthien").Id;
            UserID2 = userManager.FindByName("tqhuy").Id;
            groupID1 = "1";
        }
        [TestMethod]
        public void ListOT_Repository_IsReadAllUT1()
        {
            var result = listOTRepository.IsReadAll(UserID1, CommonConstants.FunctionOTList);
            Assert.AreEqual(true, result);
        }
        [TestMethod]
        public void ListOT_Repository_IsReadAllUT2()
        {
            var result = listOTRepository.IsReadAll(UserID2, CommonConstants.FunctionOTList);
            Assert.AreEqual(false, result);
        }
        [TestMethod]
        public void ListOT_Repository_IsReadAllUT3()
        {
            var list = listOTRepository.GetAllUser(UserID1, groupID1 , true);
            Assert.AreEqual(13, list.Count);
        }
        [TestMethod]
        public void ListOT_Repository_IsReadAllUT4()
        {
            var list = listOTRepository.GetAllUser(UserID2, groupID1, false);
            Assert.AreEqual(12, list.Count);
        }
    }
}
