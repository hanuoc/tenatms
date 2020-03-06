using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMS.Service;
using TMS.Data.Infrastructure;
using System.Collections.Generic;
using TMS.Model.Models;
using TMS.Data.Repositories;
using FluentAssertions;
using System.Linq;
using TMS.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TMS.UnitTest.RepositoryTest
{

    [TestClass]
    public class UserRepositoryTest
    {
        private IDbFactory dbFactory;
        private IAppUserRepository objAppUserRepository;
        private IUnitOfWork unitOfWork;
        private TMSDbContext DbContext;
        private UserManager<AppUser> userManager;
        private string UserID1;
        private string UserID2;

        /// <summary>
        /// setup data
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            dbFactory = new DbFactory();
            objAppUserRepository = new AppUserRepository(dbFactory);
            unitOfWork = new UnitOfWork(dbFactory);
            DbContext = new TMSDbContext();
            userManager = new UserManager<AppUser>(new UserStore<AppUser>(DbContext));
            UserID1 = userManager.FindByName("tqhuy").Id;
            UserID2 = "79187af0-b27b-486c-bfab-394981fe2b9233";
        }

        /// <summary>
        /// Case: found user by UserId when UserId is valid
        /// </summary>
        [TestMethod]
        public void User_FindUserById_Repository1()
        {
            var user = objAppUserRepository.GetMulti(x => x.Id.Equals(UserID1), new string[] { "Group" }).SingleOrDefault();
            Assert.IsNotNull(user);
        }

        /// <summary>
        /// Case: not found user by UserId when UserId is not available
        /// </summary>
        [TestMethod]
        public void User_FindUserById_Repository2()
        {
            var user = objAppUserRepository.GetMulti(x => x.Id.Equals(UserID2), new string[] { "Group" }).SingleOrDefault();
            Assert.IsNull(user);
        }

        /// <summary>
        /// Case: not found user by UserId when UserId is empty
        /// </summary>
        [TestMethod]
        public void User_ViewProfile3()
        {
            var user = objAppUserRepository.GetMulti(x => x.Id.Equals(""), new string[] { "Group" }).SingleOrDefault();
            Assert.IsNull(user);
        }

        /// <summary>
        /// Case: not found user by UserId when UserId is null
        /// </summary>
        [TestMethod]
        public void User_ViewProfile4()
        {
            var user = objAppUserRepository.GetMulti(x => x.Id.Equals(null), new string[] { "Group" }).SingleOrDefault();
            Assert.IsNull(user);
        }
    }
}
