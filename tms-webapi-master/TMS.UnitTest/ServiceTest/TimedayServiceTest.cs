using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMS.Data;
using TMS.Data.Infrastructure;
using Microsoft.AspNet.Identity;
using TMS.Model.Models;
using TMS.Data.Repositories;
using TMS.Service;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;

namespace TMS.UnitTest.ServiceTest
{
    [TestClass]
    public class TimedayServiceTest
    {
        private TMSDbContext DbContext;
        private IDbFactory dbFactory;
        private IUnitOfWork unitOfWork;
        private ITimeDayRepository timeDayRepository;
        private ITimeDayService timeDayService;
        private UserManager<AppUser> userManager;
        private TimeDay timeday;
        private IEnumerable<TimeDay> listimeday;
        private string UserID1 = "d535c327";
        private string UserID2;
        private string UserID3;
        [TestInitialize]
        public void Initialize()
        {
            DbContext = new TMSDbContext();
            dbFactory = new DbFactory();
            unitOfWork = new UnitOfWork(dbFactory);
            timeDayRepository = new TimeDayRepository(dbFactory);
            timeDayService = new TimeDayService(timeDayRepository, unitOfWork);
            userManager = new UserManager<AppUser>(new UserStore<AppUser>(DbContext));
            UserID2 = userManager.FindByName("nvthang").Id;
            UserID3 = userManager.FindByName("tqhuy").Id;
        }
        [TestMethod]
        public void UCT01()
        {
             var listimeday = timeDayService.GetAllTimeDay();
             Assert.AreEqual(5,listimeday.Count());
        }
        [TestMethod]
        public void UCT02()
        {
            timeday = timeDayService.GetbyId(2);
            Assert.IsNotNull(timeday);
        }
        [TestMethod]
        public void UCT03()
        {
            timeday = timeDayService.GetbyId(10);
            Assert.IsNull(timeday);
        }
        //[TestMethod]
        //public void UCT04()
        //{
        //    TimeDay timedays = new TimeDay();
        //    timedays.Workingday = "Chủ Nhật";
        //    timedays.CheckIn = "08:30";
        //    timedays.CheckOut = "17:30";
        //    timeday = timeDayService.Add(timedays);
        //    Assert.IsNotNull(timeday);
        //}
        [TestMethod]
        public void UCT05()
        {
            TimeDay timedays = new TimeDay();
            timedays.ID = 1;
            var id = timedays.ID;
            timeday = timeDayService.Delete(id);
            Assert.IsNotNull(timeday);
        }


    }
}
