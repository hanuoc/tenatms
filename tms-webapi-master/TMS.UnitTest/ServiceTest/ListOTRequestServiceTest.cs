using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Common.ViewModels;
using TMS.Data;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Model.Models;
using TMS.Service;
using TMS.Web.Models.Common;

namespace TMS.UnitTest.ServiceTest
{
    [TestClass]
    public class ListOTRequestServiceTest
    {
        private TMSDbContext DbContext;
        private IDbFactory dbFactory;
        private UserManager<AppUser> userManager;
        private IListOTRepository listOTRepository;
        private IListOTService objServices;
        private IUnitOfWork unitOfWork;
        private ListOTModel listOTModel;
        private IEnumerable<ListOTModel> listOTRequest;
        private string UserID1 = "d535c327";
        private string UserID2;
        private string UserID3;
        private string groupID1 = "1";
        private string groupID2 = "abc";
        private string column = "OTDate";
        private bool isDesc = true;
        private FilterOTRequestModel filter;
        //private string startdateTime = "2018-01-01";
        //private string enddateTime = "2018-01-02";

        private string[] statusRequest = { "Pending" };
        [TestInitialize]
        public void Initialize()
        {
            DbContext = new TMSDbContext();
            dbFactory = new DbFactory();
            listOTRepository = new ListOTRepository(dbFactory);
            unitOfWork = new UnitOfWork(dbFactory);
            objServices = new ListOTService(listOTRepository, unitOfWork);
            userManager = new UserManager<AppUser>(new UserStore<AppUser>(DbContext));
            UserID2 = userManager.FindByName("vxthien").Id;
            UserID3 = userManager.FindByName("tqhuy").Id;
        }
        [TestMethod]
        public void ListOT_Service_GetAllWithUserUT1()
        {
            listOTRequest = objServices.GetAllOTFilter(UserID1, groupID1, column, isDesc, filter);
            Assert.AreEqual(0, listOTRequest.Count());
        }
        [TestMethod]
        public void ListOT_Service_GetAllWithUserUT2()
        {
            listOTRequest = objServices.GetAllOTFilter(UserID2, groupID1, column, isDesc, filter);
            Assert.AreEqual(7, listOTRequest.Count());
        }
        [TestMethod]
        public void ListOT_Service_GetAllWithUserUT3()
        {
            listOTRequest = objServices.GetAllOTFilter(UserID3, groupID1, column, isDesc, filter);
            Assert.AreEqual(6, listOTRequest.Count());
        }
        [TestMethod]
        public void ListOT_Service_GetAllWithUserUT4()
        {
            listOTRequest = objServices.GetAllOTFilter(UserID1, groupID2, column, isDesc, filter);
            Assert.AreEqual(0, listOTRequest.Count());
        }
        [TestMethod]
        public void ListOT_Service_GetAllWithUserUT5()
        {
            listOTRequest = objServices.GetAllOTFilter(UserID2, groupID2, column, isDesc, filter);
            Assert.AreEqual(0, listOTRequest.Count());
        }
        [TestMethod]
        public void ListOT_Service_GetAllWithUserUT6()
        {
            listOTRequest = objServices.GetAllOTFilter(UserID3, groupID2, column, isDesc, filter);
            Assert.AreEqual(0, listOTRequest.Count());
        }
        [TestMethod]
        public void ListOT_Service_GetAllWithUserUT7()
        {
            filter.startDate = "2018-01-01";
            filter.endDate = "2018-01-02";
            filter.StatusRequestType = statusRequest;
            listOTRequest = objServices.GetAllOTFilter(UserID2, groupID1, column, isDesc, filter);
            Assert.AreEqual(0, listOTRequest.Count());
        }
    }
}
