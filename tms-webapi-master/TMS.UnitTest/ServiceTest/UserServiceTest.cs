using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Service;
using TMS.Data;

namespace TMS.UnitTest.ServiceTest
{
    [TestClass]
    public class UserServiceTest
    {

        private IDbFactory dbFactory;
        private IAppUserRepository objAppUserRepository;
        private IUserOnsiteRepository objUserOnsiteRepository;
        private IUnitOfWork unitOfWork;
        private IUserService objUserService;
        private TMSDbContext DbContext;
        private string UserID1;
        private string UserID2;

        /// <summary>
        /// setup data
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            DbContext = new TMSDbContext();
            dbFactory = new DbFactory();
            objAppUserRepository = new AppUserRepository(dbFactory);
            objUserOnsiteRepository = new UserOnsiteRepository(dbFactory);
            unitOfWork = new UnitOfWork(dbFactory);
            UserID1 = "79187af0-b27b-486c-bfab-394981fe2b9233";
            UserID2 = "79187af0-b27b-486c-bfab-394981fe2b9233";
        }

        /// <summary>
        /// Case: found user by UserId when UserId is valid
        /// </summary>
        [TestMethod]
        public void User_ViewProfile1()
        {
            var user = objUserService.FindUserById(UserID1).Result;
            Assert.IsNotNull(user);
        }

        /// <summary>
        /// Case: not found user by UserId when UserId is not available
        /// </summary>
        [TestMethod]
        public void User_ViewProfile2()
        {
            var user = objUserService.FindUserById(UserID2).Result;
            Assert.IsNull(user);
        }

        /// <summary>
        /// Case: not found user by UserId when UserId is empty
        /// </summary>
        [TestMethod]
        public void User_ViewProfile3()
        {
            var user = objUserService.FindUserById("").Result;
            Assert.IsNull(user);
        }

        /// <summary>
        /// Case: not found user by UserId when UserId is null
        /// </summary>
        [TestMethod]
        public void User_ViewProfile4()
        {
            var user = objUserService.FindUserById(null).Result;
            Assert.IsNull(user);
        }
    }
}
