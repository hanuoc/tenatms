//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Linq;
//using TMS.Data;
//using TMS.Data.Infrastructure;
//using TMS.Data.Repositories;
//using TMS.Model.Models;

//namespace TMS.UnitTest.RepositoryTest
//{
//    [TestClass]
//    public class ExplanationRepositoryTest
//    {
//        private IDbFactory dbFactory;
//        private IExplanationRequestRepository explanationRequestRepository;
//        private IStatusRequestRepository statusRequestRepository;
//        private IUnitOfWork unitOfWork;
//        private TMSDbContext DbContext;
//        private UserManager<AppUser> userManager;
//        private string userId1;
//        private string userId2;
//        private string userId3;
//        private string userId4;
//        private string groupId1 = "1";
//        private string groupId2 = "234324";

//        /// <summary>
//        /// setup data
//        /// </summary>
//        [TestInitialize]
//        public void Initialize()
//        {
//            dbFactory = new DbFactory();
//            explanationRequestRepository = new ExplanationRequestRepository(dbFactory);
//            statusRequestRepository = new StatusRequestRepository(dbFactory);
//            unitOfWork = new UnitOfWork(dbFactory);
//            DbContext = new TMSDbContext();
//            userManager = new UserManager<AppUser>(new UserStore<AppUser>(DbContext));
//            userId1 = userManager.FindByName("lvtung").Id;
//            userId2 = userManager.FindByName("ltdat").Id;
//            userId3 = "FakeId";
//            userId4 = userManager.FindByName("dmtuong").Id;
//        }

//        #region Member
//        /// <summary>
//        /// When get all explanation and found some records (userid: valid, groupid: valid)
//        /// </summary>
//        [TestMethod]
//        public void GetMultiTest1()
//        {
//            var list = explanationRequestRepository.GetMulti(
//                x => x.AbnormalCase.TimeSheet.AppUser.GroupId.ToString().Equals(groupId1));
//            Assert.AreEqual(8, list.Count());
//        }

//        /// <summary>
//        /// When get all explanation and found nothing (userid: invalid, groupid: invalid)
//        /// </summary>
//        [TestMethod]
//        public void GetMultiTest2()
//        {
//            var list = explanationRequestRepository.GetMulti(
//                x => x.AbnormalCase.TimeSheet.AppUser.Id.Equals(userId3));
//            Assert.AreEqual(0, list.Count());
//        }

//        /// <summary>
//        /// When get all explanation and found nothing (userid: valid, groupid: invalid)
//        /// </summary>
//        [TestMethod]
//        public void GetMultiTest3()
//        {
//            var list = explanationRequestRepository.GetMulti(
//                x => x.AbnormalCase.TimeSheet.AppUser.Id.Equals(userId1));
//            Assert.AreEqual(7, list.Count());
//        }

//        /// <summary>
//        /// When get all explanation and found nothing (userid: invalid, groupid: valid)
//        /// </summary>
//        [TestMethod]
//        public void GetMultiTest4()
//        {
//            var list = explanationRequestRepository.GetMulti(
//                x => x.AbnormalCase.TimeSheet.AppUser.Id.Equals(userId3));
//            Assert.AreEqual(0, list.Count());
//        }

//        /// <summary>
//        /// When get single explanation and found record (id: available)
//        /// </summary>
//        [TestMethod]
//        public void GetSingleByConditionTest1()
//        {
//            var explanation = explanationRequestRepository.GetSingleByCondition(x => x.ID == 1);
//            Assert.IsNotNull(explanation);
//        }

//        /// <summary>
//        /// When get single explanation and found nothing (id: not available)
//        /// </summary>
//        [TestMethod]
//        public void GetSingleByConditionTest2()
//        {
//            var explanation = explanationRequestRepository.GetSingleByCondition(x => x.ID == 195665);
//            Assert.IsNull(explanation);
//        }
//        #endregion Member

//        #region Group Lead

//        /// <summary>
//        /// When create explanation and explanation request object is valid
//        /// </summary>
//        [TestMethod]
//        public void GL_CreateExplanationTest1()
//        {
//            var explanation = new ExplanationRequest();
//            explanation.Title = "Giải trình cho việc nghỉ sớm ngày 24//02/2018";
//            explanation.StatusRequestId = 1;
//            explanation.ReasonDetail = "có việc bận ở nhà";
//            explanation.AbnormalCaseId = 7;
//            explanation.CreatedBy = userId1;
//            explanation.CreatedDate = DateTime.Now;
//            explanation.ReceiverId = userId2;
//            var isAdded = explanationRequestRepository.Add(explanation);
//            //unitOfWork.Commit();
//            Assert.IsNotNull(isAdded);
//        }

//        #endregion Group Lead
//    }
//}
