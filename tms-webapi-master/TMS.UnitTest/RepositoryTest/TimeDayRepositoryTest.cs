using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMS.Data;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using Microsoft.AspNet.Identity.EntityFramework;
using TMS.Model.Models;
using System.Linq;

namespace TMS.UnitTest.RepositoryTest
{
    [TestClass]
    public class TimeDayRepositoryTest
    {
        private TMSDbContext DbContext;
        private IDbFactory dbFactory;
        private ITimeDayRepository timeDayRepository;
        private IUnitOfWork unitOfWork;

        [TestInitialize]
        public void Initialize()
        {
            DbContext = new TMSDbContext();
            dbFactory = new DbFactory();
            timeDayRepository = new TimeDayRepository(dbFactory);
            unitOfWork = new UnitOfWork(dbFactory);
        }
        [TestMethod]
        public void UTC01()
        {
            var listtimeday = timeDayRepository.GetAll();
            Assert.IsNotNull(listtimeday);
            Assert.AreEqual(5, listtimeday.Count());
        }
    }
}
