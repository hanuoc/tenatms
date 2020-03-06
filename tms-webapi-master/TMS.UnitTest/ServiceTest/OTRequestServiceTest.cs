using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    [TestClass]
    public class OTRequestServiceTest
    {
        private TMSDbContext DbContext;
        private IDbFactory dbFactory;
        private IOTRequestRepository objRepository;
        private IUnitOfWork unitOfWork;
        private IOTRequestService objServices;
        private IStatusRequestRepository statusRequestRepository;
        private IEnumerable<OTRequest> listOTRequest;
        private IOTRequestUserRepository oTRequestUserRepository;
        private OTRequest otRequest;
        private UserManager<AppUser> userManager;
        private string UserID1 = "d535c327";
        private string UserID2;
        private string UserID3;
        private string UserID4;
        private string groupID1 = "1";
        private string groupID2 = "abc";

        [TestInitialize]
        public void Initialize()
        {
            DbContext = new TMSDbContext();
            dbFactory = new DbFactory();
            objRepository = new OTRequestRepository(dbFactory);
            statusRequestRepository = new StatusRequestRepository(dbFactory);
            unitOfWork = new UnitOfWork(dbFactory);
            oTRequestUserRepository = new OTRequestUserRepository(dbFactory);
            objServices = new OTRequestService(oTRequestUserRepository, objRepository, statusRequestRepository, unitOfWork);
            userManager = new UserManager<AppUser>(new UserStore<AppUser>(DbContext));
            UserID2 = userManager.FindByName("vxthien").Id;
            UserID3 = userManager.FindByName("tqhuy").Id;
            UserID4 = userManager.FindByName("ltdat").Id;
        }
        [TestMethod]
        public void OTRequest_Service_GetByIdUT1()
        {
            //call action
            otRequest = objServices.GetById(1);
            // compare
            Assert.IsNotNull(otRequest);
        }
        [TestMethod]
        public void OTRequest_Service_GetByIdUT2()
        {
            //call action
            otRequest = objServices.GetById(20);
            //compare
            Assert.IsNull(otRequest);
        }

        [TestMethod]
        public void OTRequest_Service_AddUT1()
        {
            OTRequest OTRequest = new OTRequest();
            OTRequest.Title = "test1";
            OTRequest.StatusRequestID = 1;
            OTRequest.OTTimeTypeID = 1;
            OTRequest.OTDateTypeID = 1;
            OTRequest.CreatedBy = UserID2;
            OTRequest.CreatedDate = DateTime.Now;
            OTRequest.OTDate = DateTime.Now.AddDays(1);
            //call action
            otRequest = objServices.Add(OTRequest, UserID2);
            //compare
            Assert.IsNotNull(otRequest);
        }
        [TestMethod]
        public void OTRequest_Service_AddUT2()
        {
            OTRequest OTRequest = new OTRequest();
            OTRequest.Title = null;
            OTRequest.StatusRequestID = 1;
            OTRequest.OTTimeTypeID = 1;
            OTRequest.OTDateTypeID = 1;
            OTRequest.CreatedBy = UserID2;
            OTRequest.CreatedDate = DateTime.Now;
            OTRequest.OTDate = DateTime.Now.AddDays(1);
            //call action
            otRequest = objServices.Add(OTRequest, UserID2);
            //compare
            Assert.IsNull(otRequest);
        }
        [TestMethod]
        public void OTRequest_Service_AddUT3()
        {
            OTRequest OTRequest = new OTRequest();
            OTRequest.Title = "test3";
            OTRequest.StatusRequestID = 0;
            OTRequest.OTTimeTypeID = 1;
            OTRequest.OTDateTypeID = 1;
            OTRequest.CreatedBy = UserID2;
            OTRequest.CreatedDate = DateTime.Now;
            OTRequest.OTDate = DateTime.Now.AddDays(1);
            //call action
            otRequest = objServices.Add(OTRequest, UserID2);
            //compare
            Assert.IsNotNull(otRequest);
        }
        [TestMethod]
        public void OTRequest_Service_AddUT4()
        {
            OTRequest OTRequest = new OTRequest();
            OTRequest.Title = "test3";
            OTRequest.StatusRequestID = 1;
            OTRequest.OTTimeTypeID = 0;
            OTRequest.OTDateTypeID = 1;
            OTRequest.CreatedBy = UserID2;
            OTRequest.CreatedDate = DateTime.Now;
            OTRequest.OTDate = DateTime.Now.AddDays(1);
            //call action
            otRequest = objServices.Add(OTRequest, UserID2);
            //compare
            Assert.IsNull(otRequest);
        }
        [TestMethod]
        public void OTRequest_Service_AddUT5()
        {
            OTRequest OTRequest = new OTRequest();
            OTRequest.Title = "test3";
            OTRequest.StatusRequestID = 1;
            OTRequest.OTTimeTypeID = 1;
            OTRequest.OTDateTypeID = 0;
            OTRequest.CreatedBy = UserID2;
            OTRequest.CreatedDate = DateTime.Now;
            OTRequest.OTDate = DateTime.Now.AddDays(1);
            //call action
            otRequest = objServices.Add(OTRequest, UserID2);
            //compare
            Assert.IsNull(otRequest);
        }
        [TestMethod]
        public void OTRequest_Service_AddUT6()
        {
            OTRequest OTRequest = new OTRequest();
            OTRequest.Title = "test3";
            OTRequest.StatusRequestID = 1;
            OTRequest.OTTimeTypeID = 1;
            OTRequest.OTDateTypeID = 1;
            OTRequest.CreatedBy = UserID2;
            OTRequest.CreatedDate = DateTime.Now;
            OTRequest.OTDate = null;
            //call action
            otRequest = objServices.Add(OTRequest, UserID2);
            //compare
            Assert.IsNull(otRequest);
        }
        //[TestMethod]
        //public void OTRequest_Service_ChangeStatusUT01()
        //{
        //    otRequest = objServices.ChangeStatus(1, "Cancelled");
        //    //compare
        //    Assert.IsNotNull(otRequest);
        //}
        //[TestMethod]
        //public void OTRequest_Service_ChangeStatusUT02()
        //{
        //    otRequest = objServices.ChangeStatus(0, "Cancelled");
        //    //compare
        //    Assert.IsNull(otRequest);
        //}
        //[TestMethod]
        //public void OTRequest_Service_ChangeStatusUT03()
        //{
        //    otRequest = objServices.ChangeStatus(1, "Cancel");
        //    //compare
        //    Assert.IsNull(otRequest);
        //}
        //[TestMethod]
        //public void OTRequest_Service_ChangeStatusUT04()
        //{
        //    otRequest = objServices.ChangeStatus(0, "Cancel");
        //    //compare
        //    Assert.IsNull(otRequest);
        //}

        [TestMethod]
        public void OTRequest_Service_GetOTRequestFilterUT01()
        {
            //call action
            listOTRequest = objServices.GetOTRequestFilter(UserID3, groupID1, "CreatedDate", true, null);
            //compare
            Assert.AreEqual(2, listOTRequest.Count());
        }
        [TestMethod]
        public void OTRequest_Service_GetOTRequestFilterUT02()
        {
            //call action
            listOTRequest = objServices.GetOTRequestFilter(UserID3, groupID2, "CreatedDate", true, null);
            //compare
            Assert.AreEqual(2, listOTRequest.Count());
        }
        [TestMethod]
        public void OTRequest_Service_GetOTRequestFilterUT03()
        {
            //call action
            listOTRequest = objServices.GetOTRequestFilter(UserID3, groupID2, null, true, null);
            //compare
            Assert.AreEqual(2, listOTRequest.Count());
        }

        [TestMethod]
        public void OTRequest_Service_GetOTRequestFilterUT04()
        {
            //call action
            listOTRequest = objServices.GetOTRequestFilter(UserID4, groupID1, "CreatedDate", true, null);
            //compare
            Assert.AreEqual(0, listOTRequest.Count());
        }
        [TestMethod]
        public void OTRequest_Service_GetOTRequestFilterUT05()
        {
            //call action
            listOTRequest = objServices.GetOTRequestFilter(UserID4, groupID2, "CreatedDate", true, null);
            //compare
            Assert.AreEqual(0, listOTRequest.Count());
        }
        [TestMethod]
        public void OTRequest_Service_GetOTRequestFilterUT06()
        {
            //call action
            listOTRequest = objServices.GetOTRequestFilter(UserID4, groupID2, null, true, null);
            //compare
            Assert.AreEqual(0, listOTRequest.Count());
        }
    }
}
