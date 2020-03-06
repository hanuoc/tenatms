using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Service;
using TMS.Data;
using Microsoft.AspNet.Identity;
using TMS.Model.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using TMS.Common.ViewModels;
using System;

namespace TMS.UnitTest.ServiceTest
{
    [TestClass]
    public class ExplanationServiceTest
    {
        private IDbFactory dbFactory;
        private IExplanationRequestRepository explanationRequestRepository;
        private IFingerTimeSheetRepository timeSheetRepository;
        private IStatusRequestRepository statusRequestRepository;
        private IUnitOfWork unitOfWork;
        private IExplanationRequestService explanationService;
        private IRequestRepository requestRepository;
        private IEntitleDayAppUserRepository entitleDayAppUser;
        private IGroupRepository groupRepository;
        private TMSDbContext DbContext;
        private UserManager<AppUser> userManager;
        private string userId1;
        private string userId2;
        private string userId3;
        private string userId4;
        private string groupId1 = "1";
        private string groupId2 = "234324";
        private string column = "Title";
        private bool isDesc = false;
        private int page = 1;
        private int pageSize = 5;
        private string Status_Cancel = "Cancel";
        FilterExplanationViewModel filter;

        /// <summary>
        /// setup data
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            DbContext = new TMSDbContext();
            dbFactory = new DbFactory();
            explanationRequestRepository = new ExplanationRequestRepository(dbFactory);
            timeSheetRepository = new FingerTimeSheetRepository(dbFactory);
            statusRequestRepository = new StatusRequestRepository(dbFactory);
            requestRepository = new RequestRepository(dbFactory);
            entitleDayAppUser = new EntitleDayAppUserRepository(dbFactory);
            groupRepository = new GroupRepository(dbFactory);
            unitOfWork = new UnitOfWork(dbFactory);
            //explanationService = new ExplanationRequestService(explanationRequestRepository, timeSheetRepository, statusRequestRepository, requestRepository, entitleDayAppUser, unitOfWork, groupRepository);
            userManager = new UserManager<AppUser>(new UserStore<AppUser>(DbContext));
            userId1 = userManager.FindByName("tqhuy").Id;
            userId2 = userManager.FindByName("lvtung").Id;
            userId3 = "FakeId";
            userId4 = userManager.FindByName("dmtuong").Id;
        }

        #region Member
        /// <summary>
        /// When get all explanation and found some records (userid: valid, groupid: valid)
        /// </summary>
        [TestMethod]
        public void ExplanationListTest1()
        {
            var list = explanationService.GetExplanationsList(userId2, groupId1,
                column, isDesc, page, pageSize, null);
            Assert.AreEqual(5, list.Count());
        }

        /// <summary>
        /// When get all explanation and found nothing (userid: invalid, groupid: invalid)
        /// </summary>
        [TestMethod]
        public void ExplanationListTest2()
        {
            var list = explanationService.GetExplanationsList(userId3, groupId2,
                column, isDesc, page, pageSize, null);
            Assert.AreEqual(0, list.Count());
        }

        /// <summary>
        /// When get all explanation and found nothing (userid: valid, groupid: invalid)
        /// </summary>
        [TestMethod]
        public void ExplanationListTest3()
        {
            var list = explanationService.GetExplanationsList(userId1, groupId2,
                column, isDesc, page, pageSize, null);
            Assert.AreEqual(0, list.Count());
        }

        /// <summary>
        /// When get all explanation and found nothing (userid: invalid, groupid: valid)
        /// </summary>
        [TestMethod]
        public void ExplanationListTest4()
        {
            var list = explanationService.GetExplanationsList(userId3, groupId1,
                column, isDesc, page, pageSize, null);
            Assert.AreEqual(0, list.Count());
        }

        /// <summary>
        /// When get single explanation and found record (id: available)
        /// </summary>
        [TestMethod]
        public void ExplanationDetailTest1()
        {
            var explanation = explanationService.GetExplanationDetail(1);
            Assert.IsNotNull(explanation);
        }

        /// <summary>
        /// When get single explanation and found nothing (id: not available)
        /// </summary>
        [TestMethod]
        public void ExplanationDetailTest2()
        {
            var explanation = explanationService.GetExplanationDetail(95665);
            Assert.IsNull(explanation);
        }

        /// <summary>
        /// Test when filter not found any record
        /// </summary>
        [TestMethod]
        public void ExplanationListbyFilterTest1()
        {
            filter = new FilterExplanationViewModel
            {
                StatusRequest = new string[] { "1", "2" },
                ReasonRequest = new string[] { "3", "2" },
                FromDate = "02/03/2018",
                ToDate = "07/06/2018"
            };
            var list = explanationService.GetExplanationsList(userId1, groupId1,
                column, isDesc, page, pageSize, filter);
            Assert.AreEqual(0, list.Count());
        }

        /// <summary>
        /// When filter by status and found some records
        /// </summary>
        [TestMethod]
        public void ExplanationListbyFilterTest2()
        {
            filter = new FilterExplanationViewModel
            {
                StatusRequest = new string[] { "3", "2" },
                ReasonRequest = new string[] { },
                FromDate = "02/03/2018",
                ToDate = "07/06/2018"
            };
            var list = explanationService.GetExplanationsList(userId2, groupId1,
                column, isDesc, page, pageSize, filter);
            Assert.AreEqual(2, list.Count());
        }

        /// <summary>
        /// When filter by reason and found some records
        /// </summary>
        [TestMethod]
        public void ExplanationListbyFilterTest3()
        {
            filter = new FilterExplanationViewModel
            {
                StatusRequest = new string[] { },
                ReasonRequest = new string[] { "3", "2" },
                FromDate = "02/03/2018",
                ToDate = "07/06/2018"
            };
            var list = explanationService.GetExplanationsList(userId2, groupId1,
                column, isDesc, page, pageSize, filter);
            Assert.AreEqual(2, list.Count());
        }

        /// <summary>
        /// When filter by create date and found some records
        /// </summary>
        [TestMethod]
        public void ExplanationListbyFilterTest4()
        {
            filter = new FilterExplanationViewModel
            {
                StatusRequest = new string[] { },
                ReasonRequest = new string[] { },
                FromDate = "02/03/2018",
                ToDate = "07/06/2018"
            };
            var list = explanationService.GetExplanationsList(userId2, groupId1,
                column, isDesc, page, pageSize, filter);
            Assert.AreEqual(5, list.Count());
        }

        /// <summary>
        /// When filter by status and reason and found some records
        /// </summary>
        [TestMethod]
        public void ExplanationListbyFilterTest5()
        {
            filter = new FilterExplanationViewModel
            {
                StatusRequest = new string[] { "1" },
                ReasonRequest = new string[] { "3", "2" },
                FromDate = "02/03/2018",
                ToDate = "07/06/2018"
            };
            var list = explanationService.GetExplanationsList(userId2, groupId1,
                column, isDesc, page, pageSize, filter);
            Assert.AreEqual(1, list.Count());
        }

        /// <summary>
        /// When filter by status and created date and found some records
        /// </summary>
        [TestMethod]
        public void ExplanationListbyFilterTest6()
        {
            filter = new FilterExplanationViewModel
            {
                StatusRequest = new string[] { "1" },
                ReasonRequest = new string[] { },
                FromDate = "02/03/2018",
                ToDate = "07/06/2018"
            };
            var list = explanationService.GetExplanationsList(userId2, groupId1,
                column, isDesc, page, pageSize, filter);
            Assert.AreEqual(2, list.Count());
        }

        /// <summary>
        /// When filter by reason and created date and found some records
        /// </summary>
        [TestMethod]
        public void ExplanationListbyFilterTest7()
        {
            filter = new FilterExplanationViewModel
            {
                StatusRequest = new string[] { },
                ReasonRequest = new string[] { "3", "2" },
                FromDate = "01/01/2018",
                ToDate = "04/08/2018"
            };
            var list = explanationService.GetExplanationsList(userId2, groupId1,
                column, isDesc, page, pageSize, filter);
            Assert.AreEqual(2, list.Count());
        }

        /// <summary>
        /// When filter by status, reason and created date and found some records
        /// </summary>
        [TestMethod]
        public void ExplanationListbyFilterTest8()
        {
            filter = new FilterExplanationViewModel
            {
                StatusRequest = new string[] { "1", "2" },
                ReasonRequest = new string[] { "2", "3" },
                FromDate = "01/01/2018",
                ToDate = "04/08/2018"
            };
            var list = explanationService.GetExplanationsList(userId2, groupId1,
                column, isDesc, page, pageSize, filter);
            Assert.AreEqual(1, list.Count());
        }

        /// <summary>
        /// When filter by status, reason and created date and found nothing
        /// </summary>
        [TestMethod]
        public void ExplanationListbyFilterTest9()
        {
            filter = new FilterExplanationViewModel
            {
                StatusRequest = new string[] { "3" },
                ReasonRequest = new string[] { "4" },
                FromDate = "01/01/2018",
                ToDate = "08/04/2018"
            };
            var list = explanationService.GetExplanationsList(userId2, groupId1,
                column, isDesc, page, pageSize, filter);
            Assert.AreEqual(0, list.Count());
        }

        /// <summary>
        /// When get explanation list after sort by title (desc)
        /// </summary>
        [TestMethod]
        public void ExplanationListBySortTest1()
        {
            isDesc = true;
            var list = explanationService.GetExplanationsList(userId2, groupId1,
                "Title", isDesc, page, pageSize, null);
            Assert.AreEqual(5, list.Count());
        }


        /// <summary>
        /// When get explanation list after sort by title (asc)
        /// </summary>
        [TestMethod]
        public void ExplanationListBySortTest2()
        {
            isDesc = false;
            var list = explanationService.GetExplanationsList(userId2, groupId1,
                "Title", isDesc, page, pageSize, null);
            Assert.AreEqual(5, list.Count());
        }

        /// <summary>
        /// When get explanation list after sort by Created Date(desc)
        /// </summary>
        [TestMethod]
        public void ExplanationListBySortTest3()
        {
            isDesc = true;
            var list = explanationService.GetExplanationsList(userId2, groupId1,
                "CreatedDate", isDesc, page, pageSize, null);
            Assert.AreEqual(5, list.Count());
        }

        /// <summary>
        /// When get explanation list after sort by Created Date(asc)
        /// </summary>
        [TestMethod]
        public void ExplanationListBySortTest4()
        {
            isDesc = false;
            var list = explanationService.GetExplanationsList(userId2, groupId1,
                "CreatedDate", isDesc, page, pageSize, null);
            Assert.AreEqual(5, list.Count());
        }
        #endregion Member

        #region Group Lead
        /// <summary>
        /// When get all explanation and found some records (userid: valid, groupid: valid)
        /// </summary>
        [TestMethod]
        public void GL_ExplanationListTest1()
        {
            var list = explanationService.GetExplanationsList(userId4, groupId1,
                column, isDesc, page, pageSize, null);
            Assert.AreEqual(5, list.Count());
        }


        /// <summary>
        /// When get all explanation and found nothing (userid: invalid, groupid: invalid)
        /// </summary>
        [TestMethod]
        public void GL_ExplanationListTest2()
        {
            var list = explanationService.GetExplanationsList(userId3, groupId2,
                column, isDesc, page, pageSize, null);
            Assert.AreEqual(0, list.Count());
        }

        /// <summary>
        /// When get all explanation and found nothing (userid: valid, groupid: invalid)
        /// </summary>
        [TestMethod]
        public void GL_ExplanationListTest3()
        {
            var list = explanationService.GetExplanationsList(userId4, groupId2,
                column, isDesc, page, pageSize, null);
            Assert.AreEqual(0, list.Count());
        }

        /// <summary>
        /// When get all explanation and found nothing (userid: invalid, groupid: valid)
        /// </summary>
        [TestMethod]
        public void GL_ExplanationListTest4()
        {
            var list = explanationService.GetExplanationsList(userId3, groupId1,
                column, isDesc, page, pageSize, null);
            Assert.AreEqual(0, list.Count());
        }

        /// <summary>
        /// When cancel explanation and id invalid
        /// </summary>
        //[TestMethod]
        //public void GL_CancelExplanationTest1()
        //{
        //    var explanation = explanationService.ChangeStatus(2142144523, Status_Cancel);
        //    Assert.IsNull(explanation);
        //}

        /// <summary>
        /// When cancel explanation and id valid
        /// </summary>
        //[TestMethod]
        //public void GL_CancelExplanationTest2()
        //{
        //    var explanation = explanationService.ChangeStatus(1, Status_Cancel);
        //    Assert.IsNotNull(explanation);
        //}

        /// <summary>
        /// When create explanation and explanation request object is valid
        /// </summary>
        [TestMethod]
        public void GL_CreateExplanationTest1()
        {
            var explanation = new ExplanationRequest();
            explanation.Title = "Giải trình cho việc nghỉ sớm ngày 24//02/2018";
            explanation.StatusRequestId = 1;
            explanation.ReasonDetail = "có việc bận ở nhà";
            explanation.TimeSheetId = 7;
            explanation.CreatedBy = userId1;
            explanation.CreatedDate = DateTime.Now;
            explanation.ReceiverId = userId2;
            var isAdded = explanationService.Add(explanation, null, null);
            Assert.AreEqual(true, isAdded);
        }

        /// <summary>
        /// When create explanation and explanation request object is invalid
        /// </summary>
        [TestMethod]
        public void GL_CreateExplanationTest2()
        {
            var explanation = new ExplanationRequest();
            explanation.Title = "Giải trình cho việc nghỉ sớm ngày 24//02/2018";
            explanation.ReasonDetail = "có việc bận ở nhà";
            explanation.CreatedBy = userId1;
            explanation.CreatedDate = DateTime.Now;
            explanation.ReceiverId = userId2;
            var isAdded = explanationService.Add(explanation, null, null);
            Assert.AreEqual(false, isAdded);
        }

        #endregion Group Lead
    }
}
