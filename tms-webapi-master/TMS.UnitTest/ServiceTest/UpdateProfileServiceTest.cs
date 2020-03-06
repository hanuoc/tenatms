using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMS.Data.Infrastructure;
using TMS.Data;
using TMS.Data.Repositories;
using System.Collections.Generic;
using TMS.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TMS.Model.Models;

namespace TMS.UnitTest.ServiceTest
{
    [TestClass]
    public class UpdateProfileServiceTest
    {
        private TMSDbContext DbContext;
        private AppUser user;
        [TestInitialize]
        public void Initialize()
        {
            user = new AppUser
            {
                UserName = "ltdat",
                FullName = "Le Van Dat",
                Email= "vxthient@cmc.com.vn"
            };
            DbContext = new TMSDbContext();
        }
        /// <summary>
        /// case update successfully
        /// </summary>
        [TestMethod]
        public void Update_UTC1()
        {
            var manager = new UserManager<AppUser>(new UserStore<AppUser>(DbContext));
            var appUser = manager.FindByName(user.UserName);
            appUser.FullName = user.FullName;
            var result= manager.Update(appUser);
            Assert.IsTrue(result.Succeeded);
        }
        /// <summary>
        /// case update successfully
        /// </summary>
        [TestMethod]
        public void Update_UTC2()
        {
            var manager = new UserManager<AppUser>(new UserStore<AppUser>(DbContext));
            var appUser = manager.FindByName(user.UserName);
            appUser.FullName = "Le Van Dat";
            appUser.Email = "ltdat@cmc.com.vn";
            var result = manager.Update(appUser);
            Assert.IsTrue(result.Succeeded);
        }
        /// <summary>
        /// case update successfully
        /// </summary>
        [TestMethod]
        public void Update_UTC3()
        {
            var manager = new UserManager<AppUser>(new UserStore<AppUser>(DbContext));
            var appUser = manager.FindByName(user.UserName);
            appUser.FullName = "Le Tien Dat";
            appUser.Email = "vxthient@cmc.com.vn";
            var result = manager.Update(appUser);
            Assert.IsTrue(result.Succeeded);
        }
        /// <summary>
        /// case update successfully
        /// </summary>
        [TestMethod]
        public void Update_UTC4()
        {
            var manager = new UserManager<AppUser>(new UserStore<AppUser>(DbContext));
            var appUser = manager.FindByName(user.UserName);
            appUser.FullName = "Le Tien Dat";
            appUser.Email = "vxthient@cmc.com.vn";
            var result = manager.Update(appUser);
            Assert.IsTrue(result.Succeeded);
        }
        /// <summary>
        /// case update fail
        /// </summary>
        [TestMethod]
        public void Update_UTC5()
        {
            var manager = new UserManager<AppUser>(new UserStore<AppUser>(DbContext));
            var appUser = manager.FindByName(user.UserName);
            appUser.FullName = "Le Tien Dat";
            appUser.Email = "";
            var result = manager.Update(user);
            Assert.IsFalse(result.Succeeded);
        }
    }
}