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
    public class DelegationGroupLeadExplanationRequestServiceTest
    {
        private TMSDbContext DbContext;
        private IDbFactory dbFactory;
        private IExplanationRequestRepository objRequestRepository;
        private IStatusRequestRepository statusRequestRepository;
        private IAppUserRepository objAppUserRepository;
        private IUnitOfWork unitOfWork;
        private IDelegationExplanationRequestService contextServices;
        private IEnumerable<ExplanationRequest> listRequest;
        private UserManager<AppUser> userManager;
        private ExplanationRequest model;
        private string UserID1;
        private string UserID2;
        private string UserID3 = "null";
        private int DelegationRequestID2 = 5;
        private int DelegationRequestID3 = 4;
        private int DelegationRequestID4 = 2;

        [TestInitialize]
        public void Initialize()
        {
            DbContext = new TMSDbContext();
            dbFactory = new DbFactory();
            objRequestRepository = new ExplanationRequestRepository(dbFactory);
            statusRequestRepository = new StatusRequestRepository(dbFactory);
            objAppUserRepository = new AppUserRepository(dbFactory);
            unitOfWork = new UnitOfWork(dbFactory);
            contextServices = new DelegationExplanationRequestService(objRequestRepository, statusRequestRepository, unitOfWork);
            userManager = new UserManager<AppUser>(new UserStore<AppUser>(DbContext));
            UserID1 = userManager.FindByName("nvthang").Id;
            UserID2 = userManager.FindByName("dmtuong").Id;
        }

        [TestMethod]
        public void GetDelegatioExplanationRequestUTCID01()
        {
            listRequest = contextServices.GetAllDelegationExplanationRequest(UserID3, "null");
            Assert.AreEqual(0, listRequest.Count());
        }

        [TestMethod]
        public void GetDelegationExplanationRequestUTCID02()
        {
            listRequest = contextServices.GetAllDelegationExplanationRequest(UserID3, "null");
            Assert.AreEqual(null, listRequest.Count());
        }

        [TestMethod]
        public void GetDelegationExplanationRequestUTCID03()
        {
            listRequest = contextServices.GetAllDelegationExplanationRequest(UserID2, "null");
            Assert.AreEqual(0, listRequest.Count());
        }

        [TestMethod]
        public void GetDelegationExplanationRequestUTCID04()
        {
            listRequest = contextServices.GetAllDelegationExplanationRequest(UserID2, "1");
            Assert.AreEqual(7, listRequest.Count());
        }

        //test method change status
        //[TestMethod]
        //public void ChangeStatusExplanationUTCID01()
        //{
        //    string StatusApproved = CommonConstants.StatusApproved;
        //    model = contextServices.ChangeStatus(DelegationRequestID2, CommonConstants.StatusApproved);
        //    Assert.AreEqual(StatusApproved, model.StatusRequest.Name);
        //}

        //[TestMethod]
        //public void ChangeStatusExplanationUTCID02()
        //{
        //    string StatusApproved = CommonConstants.StatusRejected;
        //    model = contextServices.ChangeStatus(DelegationRequestID3, CommonConstants.StatusRejected);
        //    Assert.AreEqual(StatusApproved, model.StatusRequest.Name);
        //}

        //[TestMethod]
        //public void ChangeStatusExplanationUTCID03()
        //{
        //    string StatusPending = CommonConstants.StatusPending;
        //    model = contextServices.ChangeStatus(DelegationRequestID4, CommonConstants.StatusDelegation);
        //    Assert.AreEqual(StatusPending, model.StatusRequest.Name);
        //}
    }
}