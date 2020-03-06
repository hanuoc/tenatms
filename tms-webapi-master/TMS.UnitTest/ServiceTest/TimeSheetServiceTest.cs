using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMS.Data;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Service;
using Microsoft.AspNet.Identity;
using TMS.Model.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using TMS.Common.ViewModels;

namespace TMS.UnitTest.ServiceTest
{
    [TestClass]
    public class TimeSheetServiceTest
    {
        private TMSDbContext DbContext;
        private IDbFactory dbFactory;
        private ITimeSheetRepository timeSheetRepository;
        private IUnitOfWork unitOfWork;
        private IStatusRequestRepository statusRequestRepository;
        private IAbnormalCaseRepository AbnormalCaseRepository;
        private IExplanationRequestRepository ExplanationRepository;
        private TimeSheetService timeSheetService;
        private UserManager<AppUser> userManager;
        string memberID;
        string adminID;
        string startDate = "2018-02-01";
        string endDate = "2018-02-18";
        string approveID;
        string rejectID;
        string cancelID;
        string delegationID;
        string pendingID;
        FilterModel filter = new FilterModel();
        [TestInitialize]
        public void Initialize()
        {
            DbContext = new TMSDbContext();
            dbFactory = new DbFactory();
            timeSheetRepository = new TimeSheetRepository(dbFactory);
            statusRequestRepository = new StatusRequestRepository(dbFactory);
            AbnormalCaseRepository = new AbnormalCaseRepository(dbFactory);
            ExplanationRepository = new ExplanationRequestRepository(dbFactory);
            unitOfWork = new UnitOfWork(dbFactory);
            timeSheetService = new TimeSheetService(timeSheetRepository, AbnormalCaseRepository, ExplanationRepository, unitOfWork);
            userManager = new UserManager<AppUser>(new UserStore<AppUser>(DbContext));
            memberID = userManager.FindByName("tqhuy").Id;
            adminID = userManager.FindByName("dmtuong").Id;
            approveID = statusRequestRepository.GetSingleByCondition(x => x.Name == "Approved").ID.ToString();
            rejectID = statusRequestRepository.GetSingleByCondition(x => x.Name == "Approved").ID.ToString();
            cancelID = statusRequestRepository.GetSingleByCondition(x => x.Name == "Approved").ID.ToString();
            delegationID = statusRequestRepository.GetSingleByCondition(x => x.Name == "Approved").ID.ToString();
            pendingID = statusRequestRepository.GetSingleByCondition(x => x.Name == "Approved").ID.ToString();
        }
        [TestMethod]
        public void GetListTimeSheetFilter_UTC01()
        {
            filter = null;
            var list = timeSheetService.GetListTimeSheetFilter(adminID, filter);
            Assert.AreEqual(list.Count, 14);
        }
        [TestMethod]
        public void GetListTimeSheetFilter_UTC02()
        {
            filter.FromDate = startDate;
            filter.ToDate = endDate;
            filter.AbnormalTimeSheetType = new string[] { "DiMuon" };
            filter.StatusExplanation = new string[] { "Approved" };
            var list = timeSheetService.GetListTimeSheetFilter(adminID, filter);
            Assert.AreEqual(list.Count, 1);
        }
        [TestMethod]
        public void GetListTimeSheetFilter_UTC03()
        {
            filter.FromDate = startDate;
            filter.ToDate = endDate;
            filter.AbnormalTimeSheetType = new string[] { "VeSom" };
            filter.StatusExplanation = new string[] { "Delegation" };
            var list = timeSheetService.GetListTimeSheetFilter(adminID, filter);
            Assert.AreEqual(list.Count, 1);
        }
        [TestMethod]
        public void GetListTimeSheetFilter_UTC04()
        {
            filter.FromDate = null;
            filter.ToDate = null;
            filter.AbnormalTimeSheetType = new string[] { "VS" };
            filter.StatusExplanation = new string[] { "Cancelled" };
            var list = timeSheetService.GetListTimeSheetFilter(adminID, filter);
            Assert.AreEqual(list.Count, 0);
        }
        [TestMethod]
        public void GetListTimeSheetFilter_UTC05()
        {
            filter.FromDate = startDate;
            filter.ToDate = endDate;
            filter.AbnormalTimeSheetType = new string[] { "VC" };
            filter.StatusExplanation = new string[] { "Delegation" };
            var list = timeSheetService.GetListTimeSheetFilter(adminID, filter);
            Assert.AreEqual(list.Count, 1);
        }
        [TestMethod]
        public void GetListTimeSheetFilter_UTC06()
        {
            filter.FromDate = startDate;
            filter.ToDate = endDate;
            filter.AbnormalTimeSheetType = new string[] { "V" };
            filter.StatusExplanation = new string[] { "Pending" };
            var list = timeSheetService.GetListTimeSheetFilter(adminID, filter);
            Assert.AreEqual(list.Count, 0);
        }
        [TestMethod]
        public void GetListTimeSheetFilter_UTC07()
        {
            filter = null;
            var list = timeSheetService.GetListTimeSheetFilter(memberID, filter);
            Assert.AreEqual(list.Count, 2);
        }
        [TestMethod]
        public void GetListTimeSheetFilter_UTC08()
        {
            filter.FromDate = startDate;
            filter.ToDate = endDate;
            filter.AbnormalTimeSheetType = new string[] { "VC" };
            filter.StatusExplanation = new string[] { "Rejected" };
            var list = timeSheetService.GetListTimeSheetFilter(adminID, filter);
            Assert.AreEqual(list.Count, 0);
        }
        [TestMethod]
        public void GetListTimeSheetFilter_UTC09()
        {
            filter.FromDate = startDate;
            filter.ToDate = endDate;
            filter.AbnormalTimeSheetType = new string[] { "VeSom" };
            filter.StatusExplanation = new string[] { "Approved" };
            var list = timeSheetService.GetListTimeSheetFilter(adminID, filter);
            Assert.AreEqual(list.Count, 1);
        }
        [TestMethod]
        public void GetListTimeSheetFilter_UTC10()
        {
            filter.FromDate = startDate;
            filter.ToDate = endDate;
            var list = timeSheetService.GetListTimeSheetFilter(adminID, filter);
            Assert.AreEqual(list.Count, 8);
        }
        [TestMethod]
        public void GetListTimeSheetFilter_UTC11()
        {
            filter.FromDate = startDate;
            filter.ToDate = endDate;
            filter.AbnormalTimeSheetType = new string[] { "V" };
            filter.StatusExplanation = new string[] { "Cancelled" };
            var list = timeSheetService.GetListTimeSheetFilter(adminID, filter);
            Assert.AreEqual(list.Count, 0);
        }
    }
}
