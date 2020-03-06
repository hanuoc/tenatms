using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TMS.Data;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Model.Models;
using TMS.Service;

namespace TMS.UnitTest.ServiceTest
{
    [TestClass]
    public class DelegationRequestServiceTest
    {
        private TMSDbContext DbContext;
        private IDbFactory dbFactory;
        private IRequestRepository objRequestRepository;
        private IStatusRequestRepository statusRequestRepository;
        private IRequestTypeRepository objRequestTypeRepository;
        private IRequestReasonTypeRepository objRequestReasonTypeRepository;
        private IAppUserRepository objAppUserRepository;
        private IEntitleDayRepository objEntitleDayRepository;
        private IEntitleDayAppUserRepository objEntitleDayAppUserRepository;
        private IUnitOfWork unitOfWork;
        private IRequestService contextServices;
        private IEnumerable<Request> listRequest;
        private Request request;
        private UserManager<AppUser> userManager;
        private string UserID1 = "d535c327";
        private string UserID2;
        private string UserID3;

        [TestInitialize]
        public void Initialize()
        {
            DbContext = new TMSDbContext();
            dbFactory = new DbFactory();
            objRequestRepository = new RequestRepository(dbFactory);
            statusRequestRepository = new StatusRequestRepository(dbFactory);
            objRequestTypeRepository = new RequestTypeRepository(dbFactory);
            objRequestReasonTypeRepository = new RequestReasonTypeRepository(dbFactory);
            objAppUserRepository = new AppUserRepository(dbFactory);
            objEntitleDayRepository = new EntitleDayRepository(dbFactory);
            objEntitleDayAppUserRepository = new EntitleDayAppUserRepository(dbFactory);
            unitOfWork = new UnitOfWork(dbFactory);
            //contextServices = new RequestService(objRequestRepository, statusRequestRepository, objRequestTypeRepository, objRequestReasonTypeRepository, objAppUserRepository, objEntitleDayRepository,objEntitleDayAppUserRepository, unitOfWork);
            userManager = new UserManager<AppUser>(new UserStore<AppUser>(DbContext));
            UserID2 = userManager.FindByName("nvthang").Id;
            UserID3 = userManager.FindByName("tqhuy").Id;
        }
        [TestMethod]
        public void RequestFilterUTCID01()
        {
            listRequest = contextServices.GetDelegationRequest(UserID1, "2", null);
            Assert.AreEqual(0, listRequest.Count());
        }
        [TestMethod]
        public void RequestFilterUTCID02()
        {
            listRequest = contextServices.GetDelegationRequest(UserID3, "2", null);
            Assert.AreEqual(0, listRequest.Count());
        }
        [TestMethod]
        public void RequestFilterUTCID03()
        {
            listRequest = contextServices.GetDelegationRequest(UserID2, "1", null);
            Assert.AreEqual(1, listRequest.Count());
        }
        [TestMethod]
        public void RequestFilterUTCID04()
        {
            listRequest = contextServices.GetDelegationRequest(UserID3, "1", null);
            Assert.AreEqual(1, listRequest.Count());
        }
    }
}