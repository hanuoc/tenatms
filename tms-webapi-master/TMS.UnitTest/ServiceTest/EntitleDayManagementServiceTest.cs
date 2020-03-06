using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Data;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Model.Models;
using TMS.Service;

namespace TMS.UnitTest.ServiceTest
{
    public class EntitleDayManagementServiceTest
    {
        private TMSDbContext DbContext;
        private IDbFactory dbFactory;
        private IUnitOfWork unitOfWork;
        private IEntitleDayManagementRepository entitleDayManagementRepository;
        private IEntitleDayManagemantService entitledayService;
        private IRequestService requestService;
        private UserManager<AppUser> userManager;
        private EntitleDay entitleDay;
        private IEnumerable<EntitleDay> _entitleDay;
        private string UserID1 = "d535c327";
        private string UserID2;
        private string UserID3;

        private IRequestRepository _requestRepository;
        private IStatusRequestRepository _statusRequestRepository;
        private IRequestTypeRepository _requestTypeRepository;
        private IRequestReasonTypeRepository _requestReasonTypeRepository;
        private IAppUserRepository _appUserRepository;
        private IEntitleDayRepository _entitleDayRepository;
        private IEntitleDayAppUserRepository _entitleDayAppUserRepository;
        private IUnitOfWork _unitOfWork;

        [TestInitialize]
        public void Initialize()
        {
            //DbContext = new TMSDbContext();
            //dbFactory = new DbFactory();
            //unitOfWork = new UnitOfWork(dbFactory);
            //requestService = new RequestService(_requestRepository, _statusRequestRepository, _requestTypeRepository, _requestReasonTypeRepository, _appUserRepository, _entitleDayRepository, _entitleDayAppUserRepository, _unitOfWork);
            //entitleDayManagementRepository = new EntitleDayManagementRepository(dbFactory);
            //entitledayService = new EntitleDayManagemantService(requestService,entitleDayManagementRepository, unitOfWork);
            //userManager = new UserManager<AppUser>(new UserStore<AppUser>(DbContext));
            //UserID2 = userManager.FindByName("vxthien").Id;
            //UserID3 = userManager.FindByName("lvtung").Id;
        }
        [TestMethod]
        public void UCT01()
        {
            var listEntitleDay = entitledayService.GetAllEntitleDayManagement();
            Assert.AreEqual(5, listEntitleDay.Count());
        }
        [TestMethod]
        public void UCT02()
        {
            entitleDay = entitledayService.GetByIdEntitleDay(2);
            Assert.IsNotNull(entitleDay);
        }
        [TestMethod]
        public void UCT03()
        {
            entitleDay = entitledayService.GetByIdEntitleDay(10);
            Assert.IsNull(entitleDay);
        }
        [TestMethod]
        public void UCT04()
        {
            EntitleDay entitleDayManagement = new EntitleDay();
            entitleDayManagement.HolidayType = "Nghỉ tết âm";
            entitleDayManagement.UnitType = "Ngày";
            entitleDayManagement.MaxEntitleDay = 7;
            entitleDayManagement.Description = "Nghỉ theo lịch nhà nước";
            entitleDay = entitledayService.Add(entitleDay);
            Assert.IsNotNull(entitleDay);
        }
        [TestMethod]
        public void UCT05()
        {
            EntitleDay entitleDayManagement = new EntitleDay();
            entitleDayManagement.ID = 1;
            var id = entitleDayManagement.ID;
            entitleDay = entitledayService.Delete(id);
            Assert.IsNotNull(entitleDay);
        }
    }
}
