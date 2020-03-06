using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using TMS.Common;
using TMS.Common.Constants;
using TMS.Common.ViewModels;
using TMS.Data;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Model.Models;
using TMS.Service;

[assembly: OwinStartup(typeof(TMS.Web.App_Start.Startup))]

namespace TMS.Web.App_Start
{
    public partial class Startup
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void Configuration(IAppBuilder app)
        {
            ConfigAutofac(app);
            ConfigureAuth(app);
            Task.Run(() => ThreadManager());
        }
        static void ThreadManager()
        {
            while (true)
            {
                if (ConfigHelper.GetByKey("ImportTimeSheetFromDateToDate") == "Actived")
                {
                    var FromDate = DateTime.ParseExact(ConfigHelper.GetByKey("FromDateDDMMYYYY"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var ToDate = DateTime.ParseExact(ConfigHelper.GetByKey("ToDateDDMMYYYY"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (FromDate <= ToDate)
                    {
                        List<DateTime> lstDate = new List<DateTime>();
                        while (FromDate <= ToDate)
                        {
                            lstDate.Add(FromDate);
                            FromDate = FromDate.AddDays(1);
                        }
                        //import time sheet from date to date
                        log.Debug("Import timesheet with list date");
                        GetDataTimeSheetFromListDate(lstDate);
                    }
                    ConfigHelper.SetValue("ImportTimeSheetFromDateToDate", "Inactived");
                }

                if (ConfigHelper.GetByKey("ImportTimeSheetFromDateToDateByOnePerson") == "Actived" && ConfigHelper.GetByKey("Username") != "")
                {
                    var FromDateOnePerson = DateTime.ParseExact(ConfigHelper.GetByKey("FromDateDDMMYYYYOnePerson"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var ToDateOnePerson = DateTime.ParseExact(ConfigHelper.GetByKey("ToDateDDMMYYYYOnePerson"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var username = ConfigHelper.GetByKey("Username");
                    List<DateTime> lstDateOnePerson = new List<DateTime>();
                    if (FromDateOnePerson <= ToDateOnePerson)
                    {
                        while (FromDateOnePerson <= ToDateOnePerson)
                        {
                            lstDateOnePerson.Add(FromDateOnePerson);
                            FromDateOnePerson = FromDateOnePerson.AddDays(1);
                        }
                        //import time sheet from date to date
                        log.Debug("Import timesheet with list date");
                        GetDataTimeSheetFromListDateOnePerson(lstDateOnePerson, username);
                    }
                    ConfigHelper.SetValue("ImportTimeSheetFromDateToDateByOnePerson", "Inactived");
                    ConfigHelper.SetValue("Username", "");

                }

                if (ConfigHelper.GetByKey("ImportTimeSheetFromDateToDateAllPerson") == "Actived")
                {
                    var FromDate = DateTime.ParseExact(ConfigHelper.GetByKey("FromDateDDMMYYYY"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var ToDate = DateTime.ParseExact(ConfigHelper.GetByKey("ToDateDDMMYYYY"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    List<DateTime> lstDateAllPerson = new List<DateTime>();
                    if (FromDate <= ToDate)
                    {
                        while (FromDate <= ToDate)
                        {
                            lstDateAllPerson.Add(FromDate);
                            FromDate = FromDate.AddDays(1);
                        }
                        //import time sheet from date to date
                        log.Debug("Import timesheet with list date");
                        GetDataTimeSheetFromListDateAllPerson(lstDateAllPerson);
                    }
                    ConfigHelper.SetValue("ImportTimeSheetFromDateToDateAllPerson", "Inactived");
                }
                Thread.Sleep(5000);
            }
        }

        private static void GetDataTimeSheetFromListDateAllPerson(List<DateTime> lstDate)
        {
            TMSDbContext DbContext = new TMSDbContext();
            IDbFactory dbFactory = new DbFactory();
            IAppUserRepository appUserRepository = new AppUserRepository(dbFactory);
            IChildcareLeaveRepository childcareLeaveRepository = new ChildcareLeaveRepository(dbFactory);
            IFingerTimeSheetRepository fingerTimeSheetRepository = new FingerTimeSheetRepository(dbFactory);
            ITimeDayRepository timeDayRepository = new TimeDayRepository(dbFactory);
            IFingerTimeSheetTmpRepository tmpTimeSheetRepository = new FingerTimeSheetTmpRepository(dbFactory);
            IOTRequestRepository OTRequestRepository = new OTRequestRepository(dbFactory);
            IFingerMachineUserRepository fingerMachineUserRepository = new FingerMachineUserRepository(dbFactory);
            IOTRequestUserRepository otrequestUserRepository = new OTRequestUserRepository(dbFactory);
            IReportRepository reportRepository = new ReportRepository(dbFactory);
            UnitOfWork unitOfWork = new UnitOfWork(dbFactory);
            IRequestRepository requestRepository = new RequestRepository(dbFactory);
            IExplanationRequestRepository explanationRepository = new ExplanationRequestRepository(dbFactory);
            IAbnormalCaseRepository abnormalRepository = new AbnormalCaseRepository(dbFactory);
            IEntitleDayAppUserRepository entitledayRepository = new EntitleDayAppUserRepository(dbFactory);
            IUserOnsiteRepository userOnsiteRepository = new UserOnsiteRepository(dbFactory);
            ISystemService systemService = new SystemService();
            IHolidayRepository holidayRepository = new HolidayRepository(dbFactory);
            ISystemConfigRepository systemConfigRepository = new SystemConfigRepository(dbFactory);
            ICommonService commonService = new CommonService(systemConfigRepository,timeDayRepository, holidayRepository, unitOfWork);
            FingerTimeSheetService fingerTimeSheetService = new FingerTimeSheetService(fingerTimeSheetRepository, tmpTimeSheetRepository, timeDayRepository, OTRequestRepository, fingerMachineUserRepository, otrequestUserRepository, appUserRepository, childcareLeaveRepository, reportRepository, unitOfWork, requestRepository, explanationRepository, abnormalRepository, entitledayRepository, userOnsiteRepository, systemService, commonService);
            //var userId = DbContext.Users.Where(x => x.UserName.Equals(username)).Select(x => x.Id).FirstOrDefault();
            //var lstUserNo = DbContext.FingerMachineUsers.Where(x => x.UserId.Equals(userId)).Select(x => x.ID);
            //var userInfo = DbContext.USERINFO.Where(x => lstUserNo.Contains(x.Badgenumber)).FirstOrDefault();
            foreach (var datetime in lstDate)
            {
                var date = datetime.Date;
                var date1 = datetime.AddDays(1).Date;
                List<CHECKINOUT> listTimeSheet = new List<CHECKINOUT>();//= DbContext.CHECKINOUT.Where(x => x.CHECKTIME >= date && x.CHECKTIME < date1 ).ToList();
                var timeSheet = DbContext.FingerTimeSheets.Where(x => x.DayOfCheck == datetime).Select(x => new { x.ID,x.UserNo });
                foreach (var itemTimeSheet in timeSheet)
                {
                    if ((DbContext.ExplanationRequests.Where(x => x.TimeSheetId.Equals(itemTimeSheet.ID)).FirstOrDefault() == null))
                    {
                        var lstFingerCode = fingerMachineUserRepository.GetFingerCodeByUserNo(itemTimeSheet.UserNo);
                        var lstUserInfo = DbContext.USERINFO.Where(x => lstFingerCode.Contains(x.Badgenumber)).Select(x=>x.USERID);
                        var checkInOut = DbContext.CHECKINOUT.Where(x =>lstUserInfo.Contains(x.USERID) && x.CHECKTIME >= date && x.CHECKTIME < date1);
                        if(checkInOut.FirstOrDefault() != null)
                        {
                            if ((DbContext.AbnormalCases.Where(x => itemTimeSheet.ID.Equals(x.TimeSheetID)).FirstOrDefault() != null))
                            {
                                abnormalRepository.DeleteAbnormalCase(itemTimeSheet.ID);
                            }
                            if ((DbContext.FingerTimeSheets.Where(x => itemTimeSheet.ID.Equals(x.ID)).FirstOrDefault() != null))
                            {
                                fingerTimeSheetRepository.DeleteFingerTimeSheet(itemTimeSheet.ID);
                            }
                            listTimeSheet.AddRange(checkInOut);
                        }
                        //explanationRepository.DeleteExplanation(timeSheetId.FirstOrDefault());
                        
                    }
                }
                tmpTimeSheetRepository.RemoveAllData();
                foreach (var item in listTimeSheet)
                {
                    FingerTimeSheetTmp tmp = new FingerTimeSheetTmp();
                    var user = DbContext.USERINFO.FirstOrDefault(x => x.USERID == item.USERID);
                    if (user == null)
                    {
                        continue;
                    }
                    tmp.UserNo = user.Badgenumber;
                    tmp.Date = item.CHECKTIME;
                    tmp.AccName = DbContext.USERINFO.Where(x => x.USERID == item.USERID).Select(x => x.Name).FirstOrDefault();
                    tmpTimeSheetRepository.Add(tmp);
                }
                if(listTimeSheet.Count == 0)
                {
                    log.Info("No timesheet to reimport "+ datetime.ToShortDateString());
                    continue;
                }
                unitOfWork.Commit();
                int count = 0;
                List<FingerTimeSheetTmpErrorModel> listModel = new List<FingerTimeSheetTmpErrorModel>();
                var result = fingerTimeSheetService.ReImportTimeSheet(out count, DbContext, out listModel);
                if (result)
                {
                    log.Info("reimport all person " + datetime.ToShortDateString() + " success");
                }
                else
                {
                    log.Info("reimport  all person "+ datetime.ToShortDateString() + " fail");
                }
            }
        }

        private static void GetDataTimeSheetFromListDateOnePerson(List<DateTime> lstDate, string username)
        {
            TMSDbContext DbContext = new TMSDbContext();
            IDbFactory dbFactory = new DbFactory();
            IAppUserRepository appUserRepository = new AppUserRepository(dbFactory);
            IChildcareLeaveRepository childcareLeaveRepository = new ChildcareLeaveRepository(dbFactory);
            IFingerTimeSheetRepository fingerTimeSheetRepository = new FingerTimeSheetRepository(dbFactory);
            ITimeDayRepository timeDayRepository = new TimeDayRepository(dbFactory);
            IFingerTimeSheetTmpRepository tmpTimeSheetRepository = new FingerTimeSheetTmpRepository(dbFactory);
            IOTRequestRepository OTRequestRepository = new OTRequestRepository(dbFactory);
            IFingerMachineUserRepository fingerMachineUserRepository = new FingerMachineUserRepository(dbFactory);
            IOTRequestUserRepository otrequestUserRepository = new OTRequestUserRepository(dbFactory);
            IReportRepository reportRepository = new ReportRepository(dbFactory);
            UnitOfWork unitOfWork = new UnitOfWork(dbFactory);
            IRequestRepository requestRepository = new RequestRepository(dbFactory);
            IExplanationRequestRepository explanationRepository = new ExplanationRequestRepository(dbFactory);
            IAbnormalCaseRepository abnormalRepository = new AbnormalCaseRepository(dbFactory);
            IEntitleDayAppUserRepository entitledayRepository = new EntitleDayAppUserRepository(dbFactory);
            IUserOnsiteRepository userOnsiteRepository = new UserOnsiteRepository(dbFactory);
            ISystemConfigRepository systemConfigRepository = new SystemConfigRepository(dbFactory);
            IHolidayRepository holidayRepository = new HolidayRepository(dbFactory);
            ICommonService commonService = new CommonService(systemConfigRepository, timeDayRepository, holidayRepository, unitOfWork);
            ISystemService systemService = new SystemService();
            FingerTimeSheetService fingerTimeSheetService = new FingerTimeSheetService(fingerTimeSheetRepository, tmpTimeSheetRepository, timeDayRepository, 
                OTRequestRepository, fingerMachineUserRepository, otrequestUserRepository, appUserRepository, childcareLeaveRepository, reportRepository, unitOfWork, requestRepository, 
                explanationRepository, abnormalRepository, entitledayRepository, userOnsiteRepository, systemService, commonService);
            var userId = DbContext.Users.Where(x => x.UserName.Equals(username)).Select(x => x.Id).FirstOrDefault();
            var lstUserNo = DbContext.FingerMachineUsers.Where(x => x.UserId.Equals(userId)).Select(x => x.ID);
            var userInfo = DbContext.USERINFO.Where(x => lstUserNo.Contains(x.Badgenumber)).FirstOrDefault();
            foreach (var datetime in lstDate)
            {
                var date = datetime.Date;
                var date1 = datetime.AddDays(1).Date;
                var listTimeSheet = DbContext.CHECKINOUT.Where(x => x.CHECKTIME >= date && x.CHECKTIME < date1 && x.USERID.Equals(userInfo.USERID)).ToList();
                if (listTimeSheet.Count > 0)
                {
                    var timeSheetId = DbContext.FingerTimeSheets.Where(x => x.DayOfCheck == datetime && lstUserNo.Contains(x.UserNo)).Select(x => x.ID).AsEnumerable();
                    if ((DbContext.ExplanationRequests.Where(x => timeSheetId.Contains(x.TimeSheetId)).FirstOrDefault() != null))
                    {
                        explanationRepository.DeleteExplanation(timeSheetId.FirstOrDefault());
                    }
                    if ((DbContext.AbnormalCases.Where(x => timeSheetId.Contains(x.TimeSheetID)).FirstOrDefault() != null))
                    {
                        abnormalRepository.DeleteAbnormalCase(timeSheetId.FirstOrDefault());
                    }
                    if ((DbContext.FingerTimeSheets.Where(x => timeSheetId.Contains(x.ID)).FirstOrDefault() != null))
                    {
                        fingerTimeSheetRepository.DeleteFingerTimeSheet(timeSheetId.FirstOrDefault());
                    }
                    tmpTimeSheetRepository.RemoveAllData();
                    foreach (var item in listTimeSheet)
                    {
                        FingerTimeSheetTmp tmp = new FingerTimeSheetTmp();
                        var user = DbContext.USERINFO.FirstOrDefault(x => x.USERID == item.USERID);
                        if (user == null)
                        {
                            continue;
                        }
                        tmp.UserNo = user.Badgenumber;
                        tmp.Date = item.CHECKTIME;
                        tmp.AccName = DbContext.USERINFO.Where(x => x.USERID == item.USERID).Select(x => x.Name).FirstOrDefault();
                        tmpTimeSheetRepository.Add(tmp);
                    }
                    unitOfWork.Commit();
                    int count = 0;
                    List<FingerTimeSheetTmpErrorModel> listModel = new List<FingerTimeSheetTmpErrorModel>();
                    var result = fingerTimeSheetService.ReImportTimeSheet(out count, DbContext, out listModel);
                    if (result)
                    {
                        log.Info("reimport success");
                    }
                    else
                    {
                        log.Info("reimport fail");
                    }
                }
            }
        }

        private static void GetDataTimeSheetFromListDate(List<DateTime> lstDate)
        {
            TMSDbContext DbContext = new TMSDbContext();
            IDbFactory dbFactory = new DbFactory();
            IAppUserRepository appUserRepository = new AppUserRepository(dbFactory);
            IChildcareLeaveRepository childcareLeaveRepository = new ChildcareLeaveRepository(dbFactory);
            IFingerTimeSheetRepository fingerTimeSheetRepository = new FingerTimeSheetRepository(dbFactory);
            ITimeDayRepository timeDayRepository = new TimeDayRepository(dbFactory);
            IFingerTimeSheetTmpRepository tmpTimeSheetRepository = new FingerTimeSheetTmpRepository(dbFactory);
            IOTRequestRepository OTRequestRepository = new OTRequestRepository(dbFactory);
            IFingerMachineUserRepository fingerMachineUserRepository = new FingerMachineUserRepository(dbFactory);
            IOTRequestUserRepository otrequestUserRepository = new OTRequestUserRepository(dbFactory);
            IReportRepository reportRepository = new ReportRepository(dbFactory);
            UnitOfWork unitOfWork = new UnitOfWork(dbFactory);
            IRequestRepository requestRepository = new RequestRepository(dbFactory);
            IExplanationRequestRepository explanationRepository = new ExplanationRequestRepository(dbFactory);
            IAbnormalCaseRepository abnormalRepository = new AbnormalCaseRepository(dbFactory);
            IEntitleDayAppUserRepository entitledayRepository = new EntitleDayAppUserRepository(dbFactory);
            IUserOnsiteRepository userOnsiteRepository = new UserOnsiteRepository(dbFactory);
            ISystemService systemService = new SystemService();
            ISystemConfigRepository systemConfigRepository = new SystemConfigRepository(dbFactory);
            IHolidayRepository holidayRepository = new HolidayRepository(dbFactory);
            ICommonService commonService = new CommonService(systemConfigRepository, timeDayRepository, holidayRepository, unitOfWork);
            FingerTimeSheetService fingerTimeSheetService = new FingerTimeSheetService(fingerTimeSheetRepository, tmpTimeSheetRepository, timeDayRepository, OTRequestRepository, 
                fingerMachineUserRepository, otrequestUserRepository, 
                appUserRepository, childcareLeaveRepository, reportRepository, unitOfWork, 
                requestRepository, explanationRepository, abnormalRepository, entitledayRepository, userOnsiteRepository, systemService, commonService);
            var listDateImportTimeSheet = new List<DateTime>();
            try
            {
                foreach (var datetime in lstDate)
                {
                    var date = datetime.Date;
                    if (DbContext.FingerTimeSheets.Where(x => x.DayOfCheck == date).FirstOrDefault() == null)
                        listDateImportTimeSheet.Add(datetime);
                }
                foreach (var datetime in listDateImportTimeSheet)
                {
                    var date = datetime.Date;
                    var date1 = datetime.AddDays(1).Date;
                    var listTimeSheet = DbContext.CHECKINOUT.Where(x => x.CHECKTIME >= date && x.CHECKTIME < date1).ToList();
                    if (listTimeSheet.Count > 0)
                    {
                        tmpTimeSheetRepository.RemoveAllData();
                        List<FingerTimeSheetTmp> listTmp = new List<FingerTimeSheetTmp>();
                        foreach (var item in listTimeSheet)
                        {
                            FingerTimeSheetTmp tmp = new FingerTimeSheetTmp();
                            var user = DbContext.USERINFO.FirstOrDefault(x => x.USERID == item.USERID);
                            if (user == null)
                            {
                                continue;
                            }
                            tmp.UserNo = user.Badgenumber;
                            tmp.Date = item.CHECKTIME;
                            tmp.AccName = DbContext.USERINFO.Where(x => x.USERID == item.USERID).Select(x => x.Name).FirstOrDefault();
                            tmpTimeSheetRepository.Add(tmp);
                            listTmp.Add(tmp);
                        }
                        unitOfWork.Commit();
                        int count = 0;
                        List<FingerTimeSheetTmpErrorModel> listModel = new List<FingerTimeSheetTmpErrorModel>();
                        var result = fingerTimeSheetService.ImportTimeSheet(out count, DbContext, out listModel);
                    }
                }
            }
            catch (Exception e)
            {
                log.Error("ExData:" +e.Data);
                log.Error("ExMess:" + e.Message);
            }
            
        }
        private void ConfigAutofac(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest();

            builder.RegisterType<TMSDbContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<RoleStore<AppRole>>().As<IRoleStore<AppRole, string>>();
            //Asp.net Identity
            builder.RegisterType<ApplicationUserStore>().As<IUserStore<AppUser>>().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();

            builder.RegisterType<ApplicationRoleManager>().AsSelf().InstancePerRequest();

            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.Register(c => app.GetDataProtectionProvider()).InstancePerRequest();

            // Repositories
            builder.RegisterAssemblyTypes(typeof(FunctionRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();

            // Services
            builder.RegisterAssemblyTypes(typeof(FunctionService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();

            Autofac.IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            //Set the WebApi DependencyResolver
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)container);
        }
    }
}
