using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using TMS.Data;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Model.Models;
using TMS.Service;

namespace TMS.UnitTest.ServiceTest
{
    [TestClass]
    public class RequestServiceTest
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
        private IRequestService objRequestService;
        private IEnumerable<Request> listRequest;
        private Request request;
        private UserManager<AppUser> userManager;
        private string UserID1 = "d535c327";
        private string UserID2;
        private string UserID3;
        private string UserID4;
        private string UserID5;
        private string groupID1 = "1";
        private string groupID2 = "abc";
        //[TestInitialize]
        //public void Initialize()
        //{
        //    DbContext = new TMSDbContext();
        //    dbFactory = new DbFactory();
        //    objRequestRepository = new RequestRepository(dbFactory);
        //    statusRequestRepository = new StatusRequestRepository(dbFactory);
        //    objRequestTypeRepository = new RequestTypeRepository(dbFactory);
        //    objRequestReasonTypeRepository = new RequestReasonTypeRepository(dbFactory);
        //    objAppUserRepository = new AppUserRepository(dbFactory);
        //    objEntitleDayRepository = new EntitleDayRepository(dbFactory);
        //    objEntitleDayAppUserRepository = new EntitleDayAppUserRepository(dbFactory);
        //    unitOfWork = new UnitOfWork(dbFactory);
        //    objRequestService = new RequestService(objRequestRepository, statusRequestRepository, objRequestTypeRepository, objRequestReasonTypeRepository, objAppUserRepository, objEntitleDayRepository, objEntitleDayAppUserRepository, unitOfWork);
        //    userManager = new UserManager<AppUser>(new UserStore<AppUser>(DbContext));
        //    UserID2 = userManager.FindByName("vxthien").Id;
        //    UserID3 = userManager.FindByName("tqhuy").Id;
        //    UserID4 = userManager.FindByName("lvtung").Id;
        //    UserID4 = userManager.FindByName("dmtuong").Id;
        //}
        //[TestMethod]
        //public void RequestServiceGetByIdUT1()
        //{
        //    //call action
        //    request = objRequestService.GetById(1);
        //    // compare
        //    Assert.IsNotNull(request);
        //}
        //[TestMethod]
        //public void RequestServiceGetByIdUT2()
        //{
        //    //call action
        //    request = objRequestService.GetById(20);
        //    //compare
        //    Assert.IsNull(request);
        //}

        //[TestMethod]
        //public void RequestServiceAddUT1()
        //{
        //    request = new Request();
        //    request.Title = "test1";
        //    request.UserId = UserID3;
        //    request.RequestTypeId = 2;
        //    request.RequestReasonTypeId = 3;
        //    request.RequestStatusId = 1;
        //    request.CreatedDate = DateTime.Now;
        //    request.StartDate = DateTime.Now.AddDays(1);
        //    request.EndDate = DateTime.Now.AddDays(1);
        //    request.Status = true;
        //    //call action
        //    var result = objRequestService.Add(request);
        //    //compare
        //    Assert.IsFalse(result);
        //}
        //[TestMethod]
        //public void RequestServiceAddUT2()
        //{
        //    request = new Request();
        //    request.Title = "test1";
        //    request.UserId = UserID4;
        //    request.RequestTypeId = 4;
        //    request.RequestStatusId = 1;
        //    request.CreatedDate = DateTime.Now;
        //    request.StartDate = DateTime.Now.AddDays(1);
        //    request.EndDate = DateTime.Now.AddDays(1);
        //    request.Status = true;
        //    //call action
        //    var result = objRequestService.Add(request);
        //    //compare
        //    Assert.IsTrue(result);
        //}


        //[TestMethod]
        //public void RequestServiceAddUT5()
        //{
        //    request = new Request();
        //    request.Title = "test3";
        //    request.UserId = UserID4;
        //    request.RequestReasonTypeId = 1;
        //    request.RequestTypeId = 1;
        //    request.RequestStatusId = 1;
        //    request.CreatedDate = DateTime.Now;
        //    request.StartDate = DateTime.Now.AddDays(30);
        //    request.EndDate = DateTime.Now.AddDays(1);
        //    request.Status = true;
        //    //call action
        //    var result = objRequestService.Add(request);
        //    //compare
        //    Assert.IsFalse(result);
        //}
        //[TestMethod]
        //public void RequestServiceAddUT6()
        //{
        //    request = new Request();
        //    request.Title = "Xin nghỉ ngày 16/01/2018";
        //    request.UserId = UserID4;
        //    request.RequestReasonTypeId = 1;
        //    request.RequestTypeId = 1;
        //    request.RequestStatusId = 1;
        //    request.CreatedDate = DateTime.Now;
        //    request.StartDate = DateTime.Now.AddDays(10);
        //    request.EndDate = DateTime.Now.AddDays(2);
        //    request.Status = true;
        //    //call action
        //    var result = objRequestService.Add(request);
        //    //compare
        //    Assert.IsFalse(result);
        //}
        //[TestMethod]
        //public void RequestServiceAddUT7()
        //{
        //    request = new Request();
        //    request.Title = "Xin nghỉ ngày 16/01/2018";
        //    request.UserId = UserID4;
        //    request.RequestReasonTypeId = 1;
        //    request.RequestTypeId = 1;
        //    request.RequestStatusId = 1;
        //    request.CreatedDate = DateTime.Now;
        //    request.StartDate = DateTime.Now.AddDays(1);
        //    request.EndDate = DateTime.Now.AddDays(2);
        //    request.Status = true;
        //    //call action
        //    var result = objRequestService.Add(request);
        //    //compare
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void RequestServiceChangeStatusUT01()
        //{
        //    request = new Request();
        //    request.Title = "Xin nghỉ ngày 16/01/2018";
        //    request.UserId = UserID4;
        //    request.RequestReasonTypeId = 1;
        //    request.RequestTypeId = 1;
        //    request.RequestStatusId = 1;
        //    request.CreatedDate = DateTime.Now;
        //    request.StartDate = DateTime.Now.AddDays(1);
        //    request.EndDate = DateTime.Now.AddDays(2);
        //    request.Status = true;
        //    var result = objRequestService.ChangeStatusRequest(request,"Cancel");
        //    //compare
        //    Assert.IsNotNull(result);
        //}
        //[TestMethod]
        //public void RequestServiceChangeStatusUT02()
        //{
        //    request = new Request();
        //    request.Title = "Xin nghỉ ngày 16/01/2018";
        //    request.UserId = UserID4;
        //    request.RequestReasonTypeId = 1;
        //    request.RequestTypeId = 1;
        //    request.RequestStatusId = 1;
        //    request.CreatedDate = DateTime.Now;
        //    request.StartDate = DateTime.Now.AddDays(1);
        //    request.EndDate = DateTime.Now.AddDays(2);
        //    request.Status = true;
        //    var result = objRequestService.ChangeStatusRequest(request, "Cancelled");
        //    //compare
        //    Assert.IsNull(result);
        //}
        //[TestMethod]
        //public void RequestServiceChangeStatusUT03()
        //{
        //    request = new Request();
        //    request.Title = "Xin nghỉ ngày 16/01/2018";
        //    request.UserId = UserID4;
        //    request.RequestReasonTypeId = 1;
        //    request.RequestTypeId = 1;
        //    request.RequestStatusId = 1;
        //    request.CreatedDate = DateTime.Now;
        //    request.StartDate = DateTime.Now.AddDays(1);
        //    request.EndDate = DateTime.Now .AddDays(2);
        //    request.Status = true;
        //    var result = objRequestService.ChangeStatusRequest(request, "Cancel");
        //    //compare
        //    Assert.IsNull(result);
        //}
        //[TestMethod]
        //public void RequestServiceChangeStatusUT04()
        //{
        //    //Request = objServices.ChangeStatus(0, "Cancel");
        //    ////compare
        //    //Assert.IsNull(Request);
        //}
        /// <summary>
        /// test user and group fake
        /// </summary>
        //[TestMethod]
        //public void RequestServiceGetAllByUserUTCD1()
        //{
        //    //call action
        //    var listRequest = objRequestService.GetAllRequestByUser(userId1, groupId1).ToList();
        //    //compare
        //    Assert.AreEqual(0, listRequest.Count());
        //}
        ///// <summary>
        ///// test user fake
        ///// </summary>
        //[TestMethod]
        //public void RequestServiceGetAllByUserUTCD2()
        //{
        //    //call action
        //    var listRequest = objRequestService.GetAllRequestByUser(userId1, groupId2).ToList();
        //    //compare
        //    Assert.AreEqual(0, listRequest.Count());
        //}
        ///// <summary>
        ///// test user fake UTC3
        ///// </summary>
        //[TestMethod]
        //public void RequestServiceGetAllByUserUTCD3()
        //{
        //    //call action
        //    var listRequest = objRequestService.GetAllRequestByUser(userId1, groupId3).ToList() ;
        //    //compare
        //    Assert.IsNNull(listRequest);
        //    Assert.AreEqual(0, listRequest.Count());
        //}
        ///// <summary>
        ///// test user admin & group fake
        ///// </summary>
        //[TestMethod]
        //public void RequestServiceGetAllByUserUTCD4()
        //{
        //    //call action
        //    var listRequest = objRequestService.GetAllRequestByUser(userId2, groupId1);
        //    //compare
        //    Assert.IsNNull(listRequest);
        //    Assert.AreEqual(0, listRequest.Count());
        //}
        ///// <summary>
        ///// test user admin & group 1
        ///// </summary>
        //[TestMethod]
        //public void RequestServiceGetAllByUserUTCD5()
        //{
        //    //call action
        //    var listRequest = objRequestService.GetAllRequestByUser(userId2, groupId2);
        //    //compare
        //    Assert.IsNNull(listRequest);
        //    Assert.AreEqual(3, listRequest.Count());
        //}
        ///// <summary>
        ///// test user admin & group 2
        ///// </summary>
        //[TestMethod]
        //public void RequestServiceGetAllByUserUTCD6()
        //{
        //    //call action
        //    var listRequest = objRequestService.GetAllRequestByUser(userId2, groupId3);
        //    //compare
        //    Assert.IsNNull(listRequest);
        //    Assert.AreEqual(2, listRequest.Count());
        //}
        ///// <summary>
        ///// test user member & group fake
        ///// </summary>
        //[TestMethod]
        //public void RequestServiceGetAllByUserUTCD7()
        //{
        //    //call action
        //    var listRequest = objRequestService.GetAllRequestByUser(userId3, groupId1);
        //    //compare
        //    Assert.IsNNull(listRequest);
        //    Assert.AreEqual(0, listRequest.Count());
        //}
        ///// <summary>
        ///// test user member & group admin
        ///// </summary>
        //[TestMethod]
        //public void RequestServiceGetAllByUserUTCD8()
        //{
        //    //call action
        //    var listRequest = objRequestService.GetAllRequestByUser(userId3, groupId2);
        //    //compare
        //    Assert.IsNNull(listRequest);
        //    Assert.AreEqual(0, listRequest.Count());
        //}
        ///// <summary>
        ///// test user member & & group member
        ///// </summary>
        //[TestMethod]
        //public void RequestServiceGetAllByUserUTCD9()
        //{
        //    //call action
        //    var listRequest = objRequestService.GetAllRequestByUser(userId3, groupId3);
        //    //compare
        //    Assert.IsNNull(listRequest);
        //    Assert.AreEqual(2, listRequest.Count());
        //}
    }
}
