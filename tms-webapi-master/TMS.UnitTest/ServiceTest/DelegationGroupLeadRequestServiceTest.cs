using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TMS.Common.Constants;
using TMS.Data;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Model.Models;
using TMS.Service;

namespace TMS.UnitTest.ServiceTest
{
    [TestClass]
    public class DelegationGroupLeadRequestServiceTest
    {
        private TMSDbContext DbContext;
        private IDbFactory dbFactory;
        private IRequestRepository objRequestRepository;
        private IStatusRequestRepository statusRequestRepository;
        private IAppUserRepository objAppUserRepository;
        private IUnitOfWork unitOfWork;
        private IDelegationRequestService contextServices;
        private IEnumerable<Request> listRequest;
        private UserManager<AppUser> userManager;
        private Request model;
        private string UserID1;
        private string UserID2;
        private string UserID3 = "null";
        private int DelegationRequestID2 = 13;
        private int DelegationRequestID3 = 14;
        private int DelegationRequestID4 = 12;

        [TestInitialize]
        public void Initialize()
        {
            DbContext = new TMSDbContext();
            dbFactory = new DbFactory();
            objRequestRepository = new RequestRepository(dbFactory);
            statusRequestRepository = new StatusRequestRepository(dbFactory);
            objAppUserRepository = new AppUserRepository(dbFactory);
            unitOfWork = new UnitOfWork(dbFactory);
            contextServices = new DelegationRequestService(objRequestRepository, statusRequestRepository, unitOfWork);
            userManager = new UserManager<AppUser>(new UserStore<AppUser>(DbContext));
            UserID1 = userManager.FindByName("nvthang").Id;
            UserID2 = userManager.FindByName("dmtuong").Id;
        }

        [TestMethod]
        public void GetDelegationRequestUTCID01()
        {
            listRequest = contextServices.GetAllDelegationRequest(UserID3, "null");
            Assert.AreEqual(0, listRequest.Count());
        }

        [TestMethod]
        public void GetDelegationRequestUTCID02()
        {
            listRequest = contextServices.GetAllDelegationRequest(UserID2, "null");
            Assert.AreEqual(0, listRequest.Count());
        }

        [TestMethod]
        public void GetDelegationRequestUTCID03()
        {
            listRequest = contextServices.GetAllDelegationRequest(UserID2, "1");
            Assert.AreEqual(7, listRequest.Count());
        }

        ////test method change status
        //[TestMethod]
        //public void ChangeStatusRequestUTCID01()
        //{
        //    string StatusApproved = CommonConstants.StatusApproved;
        //    model = contextServices.ChangeStatus(DelegationRequestID2, CommonConstants.StatusApproved);
        //    Assert.AreEqual(StatusApproved, model.StatusRequest.Name);
        //}

        //[TestMethod]
        //public void ChangeStatusRequestUTCID02()
        //{
        //    string StatusApproved = CommonConstants.StatusRejected;
        //    model = contextServices.ChangeStatus(DelegationRequestID3, CommonConstants.StatusRejected);
        //    Assert.AreEqual(StatusApproved, model.StatusRequest.Name);
        //}

        //[TestMethod]
        //public void ChangeStatusRequestUTCID03()
        //{
        //    string StatusPending = CommonConstants.StatusPending;
        //    model = contextServices.ChangeStatus(DelegationRequestID4, CommonConstants.StatusDelegation);
        //    Assert.AreEqual(StatusPending, model.StatusRequest.Name);
        //}
    }
}