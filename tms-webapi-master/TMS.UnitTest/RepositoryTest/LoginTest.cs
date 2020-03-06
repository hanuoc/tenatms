using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMS.Data;
using TMS.Data.Infrastructure;
using TMS.Model.Models;

namespace TMS.UnitTest.RepositoryTest
{
    [TestClass]
    public class LoginTest
    {
        private TMSDbContext DbContext;
        private UserManager<AppUser> userManager;

        [TestInitialize]
        public void Initialize()
        {
            DbContext = new TMSDbContext();
            userManager = new UserManager<AppUser>(new UserStore<AppUser>(DbContext));
        }

        [TestMethod]
        public async Task LoginUTCD01()
        {
            var user = userManager.Find("", "");
            Assert.IsNull(user);
        }

        [TestMethod]
        public async Task LoginUTCD02()
        {
            var user = userManager.Find("abcd", "");
            Assert.IsNull(user);
        }

        [TestMethod]
        public async Task LoginUTCD03()
        {
            var user = userManager.Find("", "123456");
            Assert.IsNull(user);
        }

        [TestMethod]
        public async Task LoginUTCD04()
        {
            var user = userManager.Find("123456", "132456");
            Assert.IsNull(user);
        }

        [TestMethod]
        public async Task LoginUTCD05()
        {
            var user = userManager.Find("admin", "");
            Assert.IsNull(user);
        }

        [TestMethod]
        public async Task LoginUTCD06()
        {
            var user = userManager.Find("admin", "123456@");
            Assert.IsNotNull(user);
        }
    }
}