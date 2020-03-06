using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Data;
using Microsoft.AspNet.Identity;
using TMS.Model.Models;
using TMS.Service;
using Microsoft.AspNet.Identity.EntityFramework;
using TMS.Common.ViewModels;
using System.Linq;

namespace TMS.UnitTest.ServiceTest
{
    /// <summary>
    /// Summary description for AbnormalCaseServiceTest
    /// </summary>
    [TestClass]
    public class AbnormalCaseServiceTest
    {
        private IDbFactory dbFactory;
        private IExplanationRequestRepository explanationRequestRepository;
        private IAbnormalCaseRepository abnormalcaseRepository;
        private IAbnormalCaseService abnormalcaseService;
        private IStatusRequestRepository statusRequestRepository;
        private IUnitOfWork unitOfWork;
        private TMSDbContext DbContext;
        private UserManager<AppUser> userManager;
        private string userId1;
        private string userId2;
        private string userId3;
        private string groupId1 = "1";
        private string groupId2 = "234324";
        FiterAbnormalViewModel filter;
        string startDate = "01/01/2018";
        string endDate = "28/02/2018";
        string approveID;
        string rejectID;
        string cancelID;
        string delegationID;
        string pendingID;
        public AbnormalCaseServiceTest()
        {
            //
            // TODO: Add constructor logic here
            
        }

        [TestInitialize]
        public void Initialize()
        {
            dbFactory = new DbFactory();
            DbContext = new TMSDbContext();
            explanationRequestRepository = new ExplanationRequestRepository(dbFactory);
            abnormalcaseRepository = new AbnormalCaseRepository(dbFactory);
            unitOfWork = new UnitOfWork(dbFactory);
            //abnormalcaseService = new AbnormalCaseService(abnormalcaseRepository, explanationRequestRepository, unitOfWork);
            userManager = new UserManager<AppUser>(new UserStore<AppUser>(DbContext));
            userId1 = userManager.FindByName("admin").Id;
            userId2 = userManager.FindByName("ltdat").Id;
      
            //approveID = statusRequestRepository.GetSingleByCondition(x => x.Name == "Approved").ID.ToString();
            //rejectID = statusRequestRepository.GetSingleByCondition(x => x.Name == "Reject").ID.ToString();
            //cancelID = statusRequestRepository.GetSingleByCondition(x => x.Name == "Cancel").ID.ToString();
            //delegationID = statusRequestRepository.GetSingleByCondition(x => x.Name == "Delegation").ID.ToString();
            //pendingID = statusRequestRepository.GetSingleByCondition(x => x.Name == "Pending").ID.ToString();
        }
        //[TestMethod]
        //public void AbnormalcaseTest1()
        //{
        //    filter = null;
        //    var list = abnormalcaseService.GetAllFilter(userId1, groupId1, filter);
        //    Assert.AreEqual(6, list.Count());
        //}
        //[TestMethod]
        //public void AbnormalTest2()
        //{
        //    filter = null;
        //    var list = abnormalcaseService.GetAllFilter(userId2, groupId1, filter);
        //    Assert.AreEqual(0, list.Count());
        //}
        //[TestMethod]
        //public void AbnormalTest3()
        //{
        //    filter = new FiterAbnormalViewModel()
        //    {
        //        StartDate = startDate,
        //        EndDate = endDate,
        //        AbnormalReason = new string [] { },
        //        StatusRequestsss = new string[] {},
        //        AbnormalReasonTypeFilter = new string[] {}      
        //    };
            
        //    var list = abnormalcaseService.GetAllFilter(userId1, groupId1, filter);
        //    Assert.AreEqual(0, list.Count());
        //}
    }
}
