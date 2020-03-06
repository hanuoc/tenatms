using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMS.Service;
using TMS.Data.Infrastructure;
using System.Collections.Generic;
using TMS.Model.Models;
using TMS.Data.Repositories;
using FluentAssertions;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TMS.Data;

namespace TMS.UnitTest.RepositoryTest
{
    /// <summary>
    /// Summary description for ChangePasswordTest
    /// </summary>
    [TestClass]
    public class ChangePasswordTest
    {
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

            DbContext = new TMSDbContext();
            userManager = new UserManager<AppUser>(new UserStore<AppUser>(DbContext));
            UserID1 = userManager.FindByName("lvtung").Id;
            UserID2 = "FakeID";

        }

        /// <summary>
        /// Test id is not null
        /// </summary>
        [TestMethod]
        public void ChangePasswordUT1()
        {
            var appUser = userManager.FindById(UserID1);
            Assert.IsNotNull(appUser);
        }

        /// <summary>
        /// Test Id is null
        /// </summary>
        [TestMethod]
        public void ChangePasswordUT2()
        {
            var appUser = userManager.FindById(UserID2);
            Assert.IsNull(appUser);
        }

        /// <summary>
        /// Test old password is valid
        /// </summary>
        [TestMethod]
        public void ChangePasswordUT3()
        {
            string password = "123456aA!@";
            var appUser = userManager.FindById(UserID1);
            bool passwordCheck = userManager.CheckPassword(appUser, password);
            Assert.AreEqual(true, passwordCheck);
        }

        /// <summary>
        /// Test old password is invalid
        /// </summary>
        [TestMethod]
        public void ChangePasswordUT4()
        {
            string password = "123456789";
            var appUser = userManager.FindById(UserID1);
            bool passwordCheck = userManager.CheckPassword(appUser, password);
            Assert.AreEqual(false, passwordCheck);
        }

        /// <summary>
        /// When old password is not equal new password
        /// </summary>
        [TestMethod]
        public void ChangePasswordUT5()
        {
            string password = "123456aA!@";
            string newPassword = "123456aA!";
            Assert.AreEqual((password != newPassword), true);
        }

        /// <summary>
        /// When old password is equal new password
        /// </summary>
        [TestMethod]
        public void ChangePasswordUT6()
        {
            string password = "123456aA!@";
            string newPassword = "123456aA!@";
            Assert.AreEqual((password.Equals(newPassword)), true);
        }

        /// <summary>
        /// When new password is equal confirm password
        /// </summary>
        [TestMethod]
        public void ChangePasswordUT7()
        {
            string newPassword = "123456aA!";
            string confirmPasswword = "123456aA!";
            Assert.AreEqual((newPassword.Equals(confirmPasswword)), true);
        }

        /// <summary>
        /// Check confirm password false
        /// </summary>
        [TestMethod]
        public void ChangePasswordUT8()
        {
            string newPassword = "123456aA!";
            string confirmPasswword = "123456aA!@";
            Assert.AreEqual(!(newPassword.Equals(confirmPasswword)), true);
        }

        /// <summary>
        /// Check password null
        /// </summary>
        [TestMethod]
        public void ChangePasswordUT9()
        {
            string password = "";
            var appUser = userManager.FindById(UserID1);
            bool passwordCheck = userManager.CheckPassword(appUser, password);
            Assert.AreEqual(false, passwordCheck);
        }
    }
}
