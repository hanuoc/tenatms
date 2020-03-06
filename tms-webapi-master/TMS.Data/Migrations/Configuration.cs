namespace TMS.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Model.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TMS.Data.Infrastructure;
    using TMS.Data.Repositories;

    internal sealed class Configuration : DbMigrationsConfiguration<TMS.Data.TMSDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(TMS.Data.TMSDbContext context)
        {
            string sql = "CREATE TRIGGER UpdateAppUserRole ON  AppUserRoles AFTER INSERT, UPDATE AS BEGIN  SET NOCOUNT ON; IF UPDATE(UserId) BEGIN UPDATE  AppUserRoles set AppUser_Id = COALESCE(AppUser_Id, UserId) where UserId in (select UserId from inserted) END END";
            //string sql1 = "CREATE INDEX IX_AppUser_Acc ON AppUsers(EmployeeNo)";
            context.Database.ExecuteSqlCommand(sql);
            //context.Database.ExecuteSqlCommand(sql1);
            CreateGroup(context);
            CreateUser(context);
            CreateFingerMachineUser(context);
            CreateFunction(context);
            CreatePermission(context);
            CreateStatusRequest(context);
            CreateRequestReasonType(context);
            CreateRequesType(context);
            CreateEntitleDay(context);
            CreateOTDateTypes(context);
            CreateOTTimeTypes(context);
            CreateTimeSheet(context);
            CreateAbnormalReason(context);
            //CreateAbnormalCase(context);
            CreateAbnormalTimeSheetTypes(context);
            //CreateAbnormalCaseReason(context);
            CreateTimeDay(context);
            CreateEntitDay_AppUser(context);
            //CreateFingerTimeSheet(context);
            context.Database.ExecuteSqlCommand(Common.Constants.AbnormalQuery.CreateStoreAbnormalCaseQuery);
            //context.Database.ExecuteSqlCommand(Common.Constants.AbnormalQuery.ExecuteStoreAbnormalCaseQuery);
            //context.Database.ExecuteSqlCommand(Common.Constants.AbnormalQuery.CreateStoreAbnormalReasonQuery);
            //context.Database.ExecuteSqlCommand(Common.Constants.AbnormalQuery.ExecuteStoreAbnormalReasonQuery);
            // context.Database.ExecuteSqlCommand(Common.Constants.AbnormalQuery.TriggerOTRequest);
            context.Database.ExecuteSqlCommand(Common.Constants.AbnormalQuery.StoreAbnormalDayOff);
            context.Database.ExecuteSqlCommand(Common.Constants.AbnormalQuery.StoreProcedureCheckTimeOut);
            context.Database.ExecuteSqlCommand(Common.Constants.AbnormalQuery.ExcuteCheckTimeOut);
            context.Database.ExecuteSqlCommand(Common.Constants.AbnormalQuery.StoreProcedureReport);
            //context.Database.ExecuteSqlCommand(Common.Constants.AbnormalQuery.ExcuteReport);
            // context.Database.ExecuteSqlCommand(Common.Constants.AbnormalQuery.TriggerRequest);
            //CreateExplanationRequest(context);

        }

        private void CreateFunction(TMSDbContext context)
        {
            if (context.Functions.Count() == 0)
            {
                context.Functions.AddRange(new List<Function>()
                {
                    new Function() {ID = "SYSTEM", Name = "System Management",ParentId = null,DisplayOrder = 1,Status = true,URL = "/",IconCss = "fa-desktop"  },
                    new Function() {ID = "ROLE", Name = "Role List",ParentId = "SYSTEM",DisplayOrder = 1,Status = false,URL = "/main/role/index",IconCss = "fa-home"  },
                    new Function() {ID = "FUNCTION", Name = "Function",ParentId = "SYSTEM",DisplayOrder = 2,Status = false,URL = "/main/function/index",IconCss = "fa-home"  },
                    new Function() {ID = "USER", Name = "User List",ParentId = "SYSTEM",DisplayOrder =3,Status = true,URL = "/main/user/index",IconCss = "fa-home"  },
                    new Function() {ID = "GROUP_LIST",Name = "Group List",ParentId = "SYSTEM",DisplayOrder = 12,Status = true,URL = "/main/group/index",IconCss = "fa-server"  },
                    new Function() {ID = "SUPER_ADMIN",Name = "Super Admin",ParentId = "SYSTEM",DisplayOrder = 12,Status = true,URL = "/main/superadmin/index",IconCss = "fa-server"  },


                    new Function() {ID = "UTILITY",Name = "Utility Management",ParentId = null,DisplayOrder = 4,Status = false,URL = "/",IconCss = "fa-clone"  },
                    new Function() {ID = "ANNOUNCEMENT",Name = "Announcement List",ParentId = "UTILITY",DisplayOrder = 3,Status = false,URL = "/main/announcement/index",IconCss = "fa-server"  },

                    new Function() {ID = "REQUEST_MANAGER",Name = "Request Management",ParentId = null,DisplayOrder = 5,Status = true,URL = "/",IconCss = "fa-edit"  },
                    new Function() {ID = "OTREQUEST_LIST",Name = "OT Request List",ParentId = "REQUEST_MANAGER",DisplayOrder = 6,Status = true,URL = "/main/ot-request/index",IconCss = "fa-server"  },
                    new Function() {ID = "REQUEST_LIST",Name = "Request List",ParentId = "REQUEST_MANAGER",DisplayOrder = 7,Status = true,URL = "/main/request/index",IconCss = "fa-server"  },
                    new Function() {ID = "WORKING-TIME_MANAGER",Name = "Time Management",ParentId = null,DisplayOrder = 8,Status = true,URL = "/",IconCss = "fa-list-alt"  },

                    new Function() {ID = "TIMESHEET_LIST",Name = "Time Sheet List",ParentId = "WORKING-TIME_MANAGER",DisplayOrder = 9,Status = true,URL = "/main/time-sheet/index",IconCss = "fa-server"  },

                    new Function() {ID = "ABNORMALCASE_LIST",Name = "Abnormal Case List",ParentId = "WORKING-TIME_MANAGER",DisplayOrder = 9,Status = true,URL = "/main/abnormalcase/index",IconCss = "fa-server"  },

                    new Function() {ID = "REPORT",Name = "Report",ParentId = "WORKING-TIME_MANAGER",DisplayOrder = 9,Status = true,URL = "/main/report/index",IconCss = "fa-server"  },

                    new Function() {ID = "EXPLANATION_MANAGER",Name = "Explanation Management",ParentId = null,DisplayOrder = 8,Status = true,URL = "/",IconCss = "fa-file"  },
                    new Function() {ID = "EXPLANATION_LIST",Name = "Explanation List",ParentId = "EXPLANATION_MANAGER",DisplayOrder = 9,Status = true,URL = "/main/explanation/index",IconCss = "fa-server" },

                    new Function() {ID = "DELEGATION_MANAGEMENT",Name = "Delegation Management",ParentId = null,DisplayOrder = 8,Status = true,URL = "/",IconCss = "fa-wpforms"  },
                    new Function() {ID = "DELEGATION_LIST",Name = "Delegation Assigned List",ParentId = "DELEGATION_MANAGEMENT",DisplayOrder = 9,Status = true,URL = "/main/delegation-request/index",IconCss = "fa-server"  },

                    new Function() {ID = "ENTITLEDAY_LIST",Name = "Entitle Day List",ParentId = "WORKING-TIME_MANAGER",DisplayOrder = 8,Status = true,URL = "/main/entitle-day/index",IconCss = "fa-server"  },
                    new Function() {ID = "OT_LIST",Name = "Approved OT List",ParentId = "WORKING-TIME_MANAGER",DisplayOrder = 9,Status = true,URL = "/main/ot-list/index",IconCss = "fa-server"  },

                    new Function() {ID = "DELEGATION_REQUEST_MANAGEMENT",Name = "Delegation Request List",ParentId = "DELEGATION_MANAGEMENT",DisplayOrder = 9,Status = true,URL = "/main/management-delegation-request/index",IconCss = "fa-server"  },
                    new Function() {ID = "DELEGATION_EXPLANATION_REQUEST_MANAGEMENT",Name = "Delegation Explanation Request List",ParentId = "DELEGATION_MANAGEMENT",DisplayOrder = 9,Status = true,URL = "/main/management-delegation-explanation-request/index",IconCss = "fa-server"  },
                    new Function() {ID = "CONFIG_DELEGATION",Name = "Config Delegation",ParentId = "DELEGATION_MANAGEMENT",DisplayOrder = 9,Status = true,URL = "/main/management-config-delegation/index",IconCss = "fa-server"  },

                    new Function() {ID = "ENTITLEDAY_MANAGEMENT_LIST",Name = "Entitle Day List (Admin)",ParentId = "WORKING-TIME_MANAGER",DisplayOrder = 10,Status = true,URL = "/main/entitle-day-management/index",IconCss = "fa-server"  },

                    new Function() {ID = "TIME_DAY_MANAGER",Name = "Time Day Management",ParentId = null,DisplayOrder = 8,Status = true,URL = "/",IconCss = "fa-clone"  },

                    new Function() {ID = "TIME_DAY",Name = "Time Day List",ParentId = "TIME_DAY_MANAGER",DisplayOrder = 9,Status = true,URL = "/main/timeday/index",IconCss = "fa-server"  },

                    new Function() {ID = "HUMAN_RESOURCE",Name = "Human Resource",ParentId = null,DisplayOrder = 8,Status = true,URL = "/",IconCss = "fa-clone"  },
                    new Function() {ID = "SEND_MAIL",Name = "Send Mail",ParentId = "HUMAN_RESOURCE",DisplayOrder = 9,Status = true,URL = "/main/sendmail/index",IconCss = "fa-server"  },
                    new Function() {ID = "HOLIDAY",Name = "Holiday List",ParentId = "TIME_DAY_MANAGER",DisplayOrder = 10,Status = true,URL = "/main/holiday/index",IconCss = "fa-server"  },

                });
                context.SaveChanges();
            }
        }

        private void CreateConfigTitle(TMSDbContext context)
        {
            if (!context.SystemConfigs.Any(x => x.Code == "HomeTitle"))
            {
                context.SystemConfigs.Add(new SystemConfig()
                {
                    Code = "HomeTitle",
                    ValueString = "Home TMS",
                });
            }
            if (!context.SystemConfigs.Any(x => x.Code == "HomeMetaKeyword"))
            {
                context.SystemConfigs.Add(new SystemConfig()
                {
                    Code = "HomeMetaKeyword",
                    ValueString = "Home TMS",
                });
            }
            if (!context.SystemConfigs.Any(x => x.Code == "HomeMetaDescription"))
            {
                context.SystemConfigs.Add(new SystemConfig()
                {
                    Code = "HomeMetaDescription",
                    ValueString = "Home TMS",
                });
            }
        }

        private void CreateGroup(TMSDbContext context)
        {
            if (context.Groups.Count() == 0)
            {
                context.Groups.AddRange(new List<Group>()
                {
                    new Group() { Name = "DU1",Description="Deliver unit 1"},
                    new Group() { Name = "DU2",Description="Deliver unit 2"  },
                    new Group() { Name = "DU3",Description="Deliver unit 3" },
                    new Group() { Name = "BU1" ,Description="Business unit 1" },
                    new Group() { Name = "BU2" ,Description="Business unit 2" },
                    new Group() { Name = "BU3" ,Description="Business unit 3" },
                    new Group() { Name = "HR", Description="Human Resource" },
                    new Group() { Name = "Admin", Description="Admin Officer" },
                    new Group() { Name = "BOM" , Description="Management" },
                    new Group() { Name = "ACCOUNTING" , Description="ACCOUNTING" },
                    new Group() { Name = "MKT" , Description="MKT" },
                    new Group() { Name = "QA" , Description="Quality Assurance" },
                    new Group() { Name = "QC" , Description="Quality Control" },
                    new Group() { Name = "RRC" , Description="Resource Reserve Center" },
                    new Group() { Name = "BU" , Description="Business unit" },
                    new Group() { Name = "SuperAdmin" , Description="Super Admin" },
                });
                context.SaveChanges();
            }
        }

        private void CreateUser(TMSDbContext context)
        {
            var manager = new UserManager<AppUser>(new UserStore<AppUser>(new TMSDbContext()));
            if (manager.Users.Count() == 0)
            {
                var roleManager = new RoleManager<AppRole>(new RoleStore<AppRole>(new TMSDbContext()));
                var user2 = new AppUser() { UserName = "txlam", EmployeeID = "00300", Email = "txlam@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("03/12/1972"), FullName = "Trần Xuân Lâm", Gender = true, Status = true, GroupId = 9, PhoneNumber = " ", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user3 = new AppUser() { UserName = "ntnhan", EmployeeID = "00002", Email = "ntnhan@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("09/29/1980"), FullName = "Nguyễn Thanh Nhàn", Gender = false, Status = true, GroupId = 7, PhoneNumber = "0915090499", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user4 = new AppUser() { UserName = "ntanh11", EmployeeID = "00003", Email = "ntanh11@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("11/26/1986"), FullName = "Ngô Thị Anh", Gender = false, Status = true, GroupId = 7, PhoneNumber = "0986636114", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user5 = new AppUser() { UserName = "ptnanh", EmployeeID = "00192", Email = "ptnanh@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("11/02/1989"), FullName = "Phạm Thị Ngọc Anh", Gender = false, Status = true, GroupId = 7, PhoneNumber = "0977112656", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user6 = new AppUser() { UserName = "nttrang1", EmployeeID = "00191", Email = "nttrang1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("05/29/1991"), FullName = "Nguyễn Thu Trang", Gender = false, Status = true, GroupId = 7, PhoneNumber = "0974512195", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user7 = new AppUser() { UserName = "ttpthao2", EmployeeID = "00005", Email = "ttpthao2@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("08/24/1991"), FullName = "Thiều Thị Phương Thảo", Gender = false, Status = true, GroupId = 7, PhoneNumber = "01649582482", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user8 = new AppUser() { UserName = "vthnhung1", EmployeeID = "00270", Email = "vthnhung1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("10/28/1991"), FullName = "Vũ Thị Hồng Nhung", Gender = false, Status = true, GroupId = 7, PhoneNumber = "01699 000 181", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user9 = new AppUser() { UserName = "ltha", EmployeeID = "00001", Email = "ltha@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("10/01/1995"), FullName = "Lê Thị Hà", Gender = false, Status = true, GroupId = 7, PhoneNumber = "0969347095", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user10 = new AppUser() { UserName = "dhthao1", EmployeeID = "00001", Email = "dhthao1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("04/06/1987"), FullName = "Dương Hương Thảo", Gender = false, Status = true, GroupId = 8, PhoneNumber = "0966366333", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user11 = new AppUser() { UserName = "pmdung", EmployeeID = "00014", Email = "pmdung@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("04/07/1991"), FullName = "Phạm Mỹ Dung", Gender = false, Status = true, GroupId = 8, PhoneNumber = "0928186898", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user12 = new AppUser() { UserName = "hgiap", EmployeeID = "00001", Email = "hgiap@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("07/21/1984"), FullName = "Hoàng Giáp", Gender = true, Status = true, GroupId = 8, PhoneNumber = "0983725915", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user13 = new AppUser() { UserName = "ntngoc", EmployeeID = "00001", Email = "ntngoc@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("10/05/1976"), FullName = "Nguyễn Thị Ngọc", Gender = false, Status = true, GroupId = 8, PhoneNumber = "0912948455", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user14 = new AppUser() { UserName = "bmthin", EmployeeID = "00001", Email = "bmthin@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("01/15/1976"), FullName = "Bế Minh Thìn", Gender = true, Status = true, GroupId = 8, PhoneNumber = "0904250066", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user15 = new AppUser() { UserName = "ntthuan", EmployeeID = "00286", Email = "ntthuan@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("06/03/1992"), FullName = "Nguyễn Thị Thuấn", Gender = false, Status = true, GroupId = 8, PhoneNumber = "01678723919", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user16 = new AppUser() { UserName = "nahoa", EmployeeID = "00013", Email = "nahoa@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("07/28/1976"), FullName = "Nguyễn Anh Hoa", Gender = false, Status = true, GroupId = 10, PhoneNumber = "01666949321", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user17 = new AppUser() { UserName = "lttthuy", EmployeeID = "00004", Email = "lttthuy@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("05/25/1981"), FullName = "Lê Thị Thanh Thủy", Gender = false, Status = true, GroupId = 10, PhoneNumber = "0906183036", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user18 = new AppUser() { UserName = "ptmngoc", EmployeeID = "00001", Email = "ptmngoc@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("04/11/1989"), FullName = "Phạm Thị Minh Ngọc ", Gender = false, Status = true, GroupId = 11, PhoneNumber = "0989834811", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user19 = new AppUser() { UserName = "nmnguyet", EmployeeID = "00001", Email = "nmnguyet@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("05/27/1996"), FullName = "Nguyễn Minh Nguyệt", Gender = false, Status = true, GroupId = 11, PhoneNumber = "01682925996", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user20 = new AppUser() { UserName = "ddanh1", EmployeeID = "00001", Email = "ddanh1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("02/01/1992"), FullName = "Đặng Duy Anh", Gender = true, Status = true, GroupId = 11, PhoneNumber = "0869036392", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user21 = new AppUser() { UserName = "lvtuong", EmployeeID = "00008", Email = "lvtuong@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("04/04/1986"), FullName = "Lăng Vĩnh Tường", Gender = true, Status = true, GroupId = 4, PhoneNumber = "0912965611", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user22 = new AppUser() { UserName = "dtthang", EmployeeID = "000012", Email = "dtthang@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("10/30/1989"), FullName = "Đinh Thị Thu Hằng", Gender = false, Status = true, GroupId = 4, PhoneNumber = "0936638866", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user23 = new AppUser() { UserName = "hmy", EmployeeID = "00016", Email = "hmy@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("01/13/1993"), FullName = "Hà My", Gender = false, Status = true, GroupId = 4, PhoneNumber = "0975275917", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user24 = new AppUser() { UserName = "ttsen", EmployeeID = "00010", Email = "ttsen@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("11/05/1991"), FullName = "Trần Thị Sen", Gender = false, Status = true, GroupId = 4, PhoneNumber = "0901770555", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user25 = new AppUser() { UserName = "nttrang", EmployeeID = "00011", Email = "nttrang@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("06/02/1986"), FullName = "Nguyễn Thu Trang", Gender = false, Status = true, GroupId = 4, PhoneNumber = "0973192828", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user26 = new AppUser() { UserName = "ntttruc", EmployeeID = "00001", Email = "ntttruc@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("06/23/1995"), FullName = "Nguyễn Thị Thanh Trúc", Gender = false, Status = true, GroupId = 4, PhoneNumber = "0984816541", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user27 = new AppUser() { UserName = "dlchi", EmployeeID = "00273", Email = "dlchi@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("04/15/1989"), FullName = "Đậu Linh Chi ", Gender = false, Status = true, GroupId = 4, PhoneNumber = "0934589669 ", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user28 = new AppUser() { UserName = "lhhoang", EmployeeID = "00296", Email = "lhhoang@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("01/26/1990"), FullName = "Lê Huy Hoàng", Gender = true, Status = true, GroupId = 4, PhoneNumber = "0988925022", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user29 = new AppUser() { UserName = "ntquynh", EmployeeID = "00001", Email = "ntquynh@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("02/27/1986"), FullName = "Nguyễn Thu Quỳnh", Gender = false, Status = true, GroupId = 5, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user30 = new AppUser() { UserName = "ttnhien", EmployeeID = "00001", Email = "ttnhien@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("07/29/1990"), FullName = "Trần Thị Ngọc Hiền", Gender = false, Status = true, GroupId = 5, PhoneNumber = "0986240518", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user31 = new AppUser() { UserName = "nthnhung", EmployeeID = "00143", Email = "nthnhung@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("02/16/1991"), FullName = "Nguyễn Thị Hồng Nhung", Gender = false, Status = true, GroupId = 5, PhoneNumber = "0969391618", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user32 = new AppUser() { UserName = "tttvan", EmployeeID = "00152", Email = "tttvan@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("07/31/1995"), FullName = "Trần Thị Thanh Vân", Gender = false, Status = true, GroupId = 5, PhoneNumber = "0914573195", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user33 = new AppUser() { UserName = "nctson", EmployeeID = "00001", Email = "nctson@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("10/08/1995"), FullName = "Nguyễn Công Thái Sơn", Gender = true, Status = true, GroupId = 5, PhoneNumber = "0978383514", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user34 = new AppUser() { UserName = "nmlong", EmployeeID = "00001", Email = "nmlong@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("06/11/1984"), FullName = "Nguyễn Mạnh Long", Gender = true, Status = true, GroupId = 6, PhoneNumber = "0912858966", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user35 = new AppUser() { UserName = "dpanh", EmployeeID = "00001", Email = "dpanh@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("07/27/1993"), FullName = "Đặng Phương Anh", Gender = false, Status = true, GroupId = 6, PhoneNumber = "0968215205", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user36 = new AppUser() { UserName = "lahphuong", EmployeeID = "00001", Email = "lahphuong@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("01/17/1992"), FullName = "Lê Anh Hoàng Phương", Gender = false, Status = true, GroupId = 6, PhoneNumber = "0978777734", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user37 = new AppUser() { UserName = "nahung", EmployeeID = "00001", Email = "nahung@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("05/20/1980"), FullName = "Nguyễn An Hưng", Gender = true, Status = true, GroupId = 1, PhoneNumber = "0983600990", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user38 = new AppUser() { UserName = "dmthong", EmployeeID = "00001", Email = "dmthong@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("04/05/1987"), FullName = "Đỗ Minh Thông", Gender = true, Status = true, GroupId = 1, PhoneNumber = "0906246222", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user39 = new AppUser() { UserName = "ndthien", EmployeeID = "00001", Email = "ndthien@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("03/15/1993"), FullName = "Nguyễn Đức Thiện", Gender = true, Status = true, GroupId = 1, PhoneNumber = "01662447297", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user40 = new AppUser() { UserName = "ndtai", EmployeeID = "00232", Email = "ndtai@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("09/14/1992"), FullName = "Nguyễn Đình Tài", Gender = true, Status = true, GroupId = 1, PhoneNumber = "0971518688", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user41 = new AppUser() { UserName = "nhson2", EmployeeID = "00001", Email = "nhson2@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("09/13/1988"), FullName = "Nguyễn Hải Sơn", Gender = true, Status = true, GroupId = 1, PhoneNumber = "0987888827", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user42 = new AppUser() { UserName = "nvluan", EmployeeID = "00001", Email = "nvluan@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("12/24/1990"), FullName = "Nguyễn Văn Luân", Gender = true, Status = true, GroupId = 1, PhoneNumber = "01674520840", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user43 = new AppUser() { UserName = "nhlong", EmployeeID = "00001", Email = "nhlong@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("11/29/1988"), FullName = "Nguyễn Hải Long", Gender = true, Status = true, GroupId = 1, PhoneNumber = "0974420963", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user44 = new AppUser() { UserName = "dthien1", EmployeeID = "00030", Email = "dthien1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("10/15/1983"), FullName = "Đinh Thanh Hiền", Gender = false, Status = true, GroupId = 1, PhoneNumber = "0914880018", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user45 = new AppUser() { UserName = "nvan1", EmployeeID = "00001", Email = "nvan1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("04/02/1980"), FullName = "Nguyễn Văn An", Gender = true, Status = true, GroupId = 1, PhoneNumber = "0945358725", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user46 = new AppUser() { UserName = "bthoang", EmployeeID = "00001", Email = "bthoang@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("01/17/1987"), FullName = "Bùi Trọng Hoàng", Gender = true, Status = true, GroupId = 1, PhoneNumber = "0972138633", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user50 = new AppUser() { UserName = "nddang", EmployeeID = "00043", Email = "nddang@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("10/28/1993"), FullName = "Nguyễn Duy Đăng", Gender = true, Status = true, GroupId = 1, PhoneNumber = "0974249033", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user47 = new AppUser() { UserName = "hnhieu", EmployeeID = "00039", Email = "hnhieu@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("01/29/1981"), FullName = "Hà Ngọc Hiệu", Gender = true, Status = true, GroupId = 1, PhoneNumber = null, StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user48 = new AppUser() { UserName = "dbthan", EmployeeID = "00040", Email = "dbthan@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("08/25/1990"), FullName = "Đỗ Bá Thản", Gender = true, Status = true, GroupId = 1, PhoneNumber = "0914832690", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user49 = new AppUser() { UserName = "ptlam", EmployeeID = "00041", Email = "ptlam@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("05/26/1981"), FullName = "Phan Trọng Lam", Gender = true, Status = true, GroupId = 1, PhoneNumber = "0936164430", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user51 = new AppUser() { UserName = "ntdung", EmployeeID = "00001", Email = "ntdung@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("07/03/1991"), FullName = "Nguyễn Tiến Dũng", Gender = true, Status = true, GroupId = 1, PhoneNumber = "01636913333", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user52 = new AppUser() { UserName = "ltthoa1", EmployeeID = "00001", Email = "ltthoa1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("09/30/1995"), FullName = "Lê Thanh Thỏa", Gender = true, Status = true, GroupId = 1, PhoneNumber = "01687551733", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user53 = new AppUser() { UserName = "nhphuong1", EmployeeID = "00001", Email = "nhphuong1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("09/27/1990"), FullName = "Nguyễn Hòa Phương", Gender = true, Status = true, GroupId = 1, PhoneNumber = "0936137425", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user54 = new AppUser() { UserName = "txtung", EmployeeID = "00051", Email = "txtung@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("04/25/1991"), FullName = "Trần Xuân Tùng", Gender = true, Status = true, GroupId = 1, PhoneNumber = "0933449254", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user55 = new AppUser() { UserName = "nntrong", EmployeeID = "00053", Email = "nntrong@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("05/05/1987"), FullName = "Nguyễn Ngọc Trọng", Gender = true, Status = true, GroupId = 1, PhoneNumber = "0969296668", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user56 = new AppUser() { UserName = "lvhuong", EmployeeID = "00060", Email = "lvhuong@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("11/08/1975"), FullName = "Lê Văn Hưởng", Gender = true, Status = true, GroupId = 1, PhoneNumber = "0936333479", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user57 = new AppUser() { UserName = "lvlong", EmployeeID = "00001", Email = "lvlong@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("10/08/1989"), FullName = "Lê Văn Long", Gender = true, Status = true, GroupId = 1, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user58 = new AppUser() { UserName = "nman", EmployeeID = "00077", Email = "nman@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("08/29/1992"), FullName = "Nguyễn Minh An", Gender = true, Status = true, GroupId = 1, PhoneNumber = "01635850584", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user59 = new AppUser() { UserName = "nxthoi", EmployeeID = "00001", Email = "nxthoi@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("11/04/1990"), FullName = "Nguyễn Xuân Thời", Gender = true, Status = true, GroupId = 1, PhoneNumber = "0906641190", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user60 = new AppUser() { UserName = "mctu", EmployeeID = "00001", Email = "mctu@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("02/16/1991"), FullName = "Mai Công Tú", Gender = true, Status = true, GroupId = 1, PhoneNumber = "01636246232", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user61 = new AppUser() { UserName = "pdquan", EmployeeID = "00001", Email = "pdquan@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("03/18/1992"), FullName = "Phạm Đức Quân", Gender = true, Status = true, GroupId = 1, PhoneNumber = "0979254414", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user62 = new AppUser() { UserName = "pvkhuong1", EmployeeID = "00001", Email = "pvkhuong1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("01/28/1987"), FullName = "Phạm Văn Khương", Gender = true, Status = true, GroupId = 1, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user63 = new AppUser() { UserName = "nvanh", EmployeeID = "00001", Email = "nvanh@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("09/29/1995"), FullName = "Nguyễn Việt Anh", Gender = true, Status = true, GroupId = 1, PhoneNumber = "01648272828", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user64 = new AppUser() { UserName = "thdu", EmployeeID = "00001", Email = "thdu@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("02/10/1984"), FullName = "Trần Hữu Dự", Gender = true, Status = true, GroupId = 1, PhoneNumber = "0943306655", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user65 = new AppUser() { UserName = "ntdung31", EmployeeID = "00272", Email = "ntdung31@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("01/10/1991"), FullName = "Nguyễn Trung Dũng", Gender = true, Status = true, GroupId = 1, PhoneNumber = "0973725984", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user66 = new AppUser() { UserName = "nvduy", EmployeeID = "00001", Email = "nvduy@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("08/02/1988"), FullName = "Nguyễn Văn Duy", Gender = true, Status = true, GroupId = 1, PhoneNumber = "0972417288", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user67 = new AppUser() { UserName = "nxcanh", EmployeeID = "00101", Email = "nxcanh@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("04/30/1982"), FullName = "Ngô Xuân Cảnh", Gender = true, Status = true, GroupId = 2, PhoneNumber = "0983308204", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user68 = new AppUser() { UserName = "bmhung", EmployeeID = "00001", Email = "bmhung@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("08/07/1983"), FullName = "Bùi Mạnh Hùng", Gender = true, Status = true, GroupId = 2, PhoneNumber = "01694141983", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user69 = new AppUser() { UserName = "hmduc", EmployeeID = "00107", Email = "hmduc@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("05/28/1979"), FullName = "Hoàng Minh Đức", Gender = true, Status = true, GroupId = 2, PhoneNumber = "01254321795", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user70 = new AppUser() { UserName = "mtanh", EmployeeID = "00112", Email = "mtanh@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("12/02/1993"), FullName = "Mai Trung Anh", Gender = false, Status = true, GroupId = 2, PhoneNumber = "01698006541", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user71 = new AppUser() { UserName = "nvnam", EmployeeID = "00113", Email = "nvnam@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("09/20/1984"), FullName = "Nguyễn Văn Nam", Gender = true, Status = true, GroupId = 2, PhoneNumber = "0964932426", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user72 = new AppUser() { UserName = "ldquan", EmployeeID = "00114", Email = "ldquan@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("02/09/1986"), FullName = "Lê Đức Quân", Gender = true, Status = true, GroupId = 2, PhoneNumber = "0978849286", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user73 = new AppUser() { UserName = "ddviet", EmployeeID = "00108", Email = "ddviet@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("09/06/1990"), FullName = "Đỗ Đức Việt", Gender = true, Status = true, GroupId = 2, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user74 = new AppUser() { UserName = "nccong1", EmployeeID = "00109", Email = "nccong1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("09/27/1985"), FullName = "Nguyễn Chí Công", Gender = true, Status = true, GroupId = 2, PhoneNumber = "0903283007", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user75 = new AppUser() { UserName = "vdthuan", EmployeeID = "00110", Email = "vdthuan@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("08/28/1980"), FullName = "Vũ Đức Thuần", Gender = true, Status = true, GroupId = 2, PhoneNumber = "097135618", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user76 = new AppUser() { UserName = "nbhai", EmployeeID = "00122", Email = "nbhai@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("06/24/1986"), FullName = "Nguyễn Bá Hải", Gender = true, Status = true, GroupId = 2, PhoneNumber = "0976998086", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user77 = new AppUser() { UserName = "ctpmai", EmployeeID = "00001", Email = "ctpmai@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("04/15/1993"), FullName = "Cao Thị Phương Mai", Gender = false, Status = true, GroupId = 2, PhoneNumber = "0981061548", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user78 = new AppUser() { UserName = "tvhai", EmployeeID = "00117", Email = "tvhai@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("11/02/1993"), FullName = "Tô Văn Hải", Gender = true, Status = true, GroupId = 2, PhoneNumber = "01667018850", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user79 = new AppUser() { UserName = "tkien", EmployeeID = "00119", Email = "tkien@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("12/30/1994"), FullName = "Trịnh Kiên", Gender = true, Status = true, GroupId = 2, PhoneNumber = "0974011703", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user80 = new AppUser() { UserName = "nhbac", EmployeeID = "00133", Email = "nhbac@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("04/18/1990"), FullName = "Nguyễn Hoàng Bắc", Gender = true, Status = true, GroupId = 2, PhoneNumber = "0985641269", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user81 = new AppUser() { UserName = "nqminh", EmployeeID = "00121", Email = "nqminh@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("08/27/1994"), FullName = "Nguyễn Quang Minh", Gender = true, Status = true, GroupId = 2, PhoneNumber = "01686196483", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user82 = new AppUser() { UserName = "ndphong", EmployeeID = "00150", Email = "ndphong@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("12/15/1993"), FullName = "Nguyễn Đức Phong", Gender = true, Status = true, GroupId = 2, PhoneNumber = "0988273630", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user83 = new AppUser() { UserName = "hvtrung", EmployeeID = "00120", Email = "hvtrung@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("11/29/1994"), FullName = "Hoàng Văn Trung", Gender = true, Status = true, GroupId = 2, PhoneNumber = "01656562899", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user84 = new AppUser() { UserName = "hxduc", EmployeeID = "00001", Email = "hxduc@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("02/17/1995"), FullName = "Hồ Xuân Đức", Gender = true, Status = true, GroupId = 2, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user85 = new AppUser() { UserName = "ndhoang1", EmployeeID = "00001", Email = "ndhoang1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("01/10/1995"), FullName = "Nguyễn Đức Hoàng", Gender = true, Status = true, GroupId = 2, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user86 = new AppUser() { UserName = "ndhung", EmployeeID = "00128", Email = "ndhung@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("07/16/1989"), FullName = "Nguyễn Đình Hùng", Gender = true, Status = true, GroupId = 2, PhoneNumber = "01656054391", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user87 = new AppUser() { UserName = "ntkien12", EmployeeID = "00001", Email = "ntkien12@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("07/28/1995"), FullName = "Nguyễn Trung Kiên", Gender = true, Status = true, GroupId = 2, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user88 = new AppUser() { UserName = "nhtoan1", EmployeeID = "00001", Email = "nhtoan1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("11/06/1995"), FullName = "Nguyễn Hữu Toàn", Gender = true, Status = true, GroupId = 2, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user89 = new AppUser() { UserName = "dxtho", EmployeeID = "00001", Email = "dxtho@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("10/02/1995"), FullName = "Đỗ Xuân Thọ", Gender = true, Status = true, GroupId = 2, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user90 = new AppUser() { UserName = "pghuu", EmployeeID = "00229", Email = "pghuu@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("08/03/1993"), FullName = "Phạm Gia Hữu", Gender = true, Status = true, GroupId = 2, PhoneNumber = "01699317037", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user91 = new AppUser() { UserName = "haminh", EmployeeID = "00001", Email = "haminh@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("01/19/1985"), FullName = "Hoàng Anh Minh", Gender = true, Status = true, GroupId = 2, PhoneNumber = "0987765522", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user92 = new AppUser() { UserName = "nvcuong4", EmployeeID = "00281", Email = "nvcuong4@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("05/29/1993"), FullName = "Nguyễn Văn Cường", Gender = true, Status = true, GroupId = 2, PhoneNumber = "01656513903", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user93 = new AppUser() { UserName = "dvbau", EmployeeID = "00001", Email = "dvbau@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("01/23/1992"), FullName = "Đào Văn Báu", Gender = true, Status = true, GroupId = 2, PhoneNumber = "01694644366", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user94 = new AppUser() { UserName = "dthai", EmployeeID = "00001", Email = "dthai@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("05/21/1992"), FullName = "Đỗ Trọng Hải", Gender = true, Status = true, GroupId = 2, PhoneNumber = "8026781144", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user95 = new AppUser() { UserName = "pbvu", EmployeeID = "00001", Email = "pbvu@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("01/16/1992"), FullName = "Phạm Bá Vũ", Gender = true, Status = true, GroupId = 2, PhoneNumber = "01649778859", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user96 = new AppUser() { UserName = "dtanh", EmployeeID = "00020", Email = "dtanh@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("10/11/1987"), FullName = "Đào Tuấn Anh", Gender = true, Status = true, GroupId = 3, PhoneNumber = "0906075499", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user97 = new AppUser() { UserName = "ntanh2", EmployeeID = "00151", Email = "ntanh2@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("08/12/1984"), FullName = "Nguyễn Thế Anh", Gender = true, Status = true, GroupId = 3, PhoneNumber = "0915324667", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user98 = new AppUser() { UserName = "tthuyen1", EmployeeID = "00203", Email = "tthuyen1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("12/29/1987"), FullName = "Trần Thị Huyền", Gender = false, Status = true, GroupId = 3, PhoneNumber = "01638386368", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user99 = new AppUser() { UserName = "dttlinh", EmployeeID = "00209", Email = "dttlinh@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("04/25/1991"), FullName = "Đàm Thị Thùy Linh", Gender = false, Status = true, GroupId = 3, PhoneNumber = "0912973391", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user100 = new AppUser() { UserName = "tnluan", EmployeeID = "00221", Email = "tnluan@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("11/12/1991"), FullName = "Trần Ngọc Luân", Gender = true, Status = true, GroupId = 3, PhoneNumber = "0976876663", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user101 = new AppUser() { UserName = "ddhung", EmployeeID = "00223", Email = "ddhung@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("07/04/1988"), FullName = "Dư Duy Hưng", Gender = true, Status = true, GroupId = 3, PhoneNumber = "0979828288", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user102 = new AppUser() { UserName = "nmhoang", EmployeeID = "00230", Email = "nmhoang@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("05/14/1990"), FullName = "Nguyễn Minh Hoàng", Gender = true, Status = true, GroupId = 3, PhoneNumber = "0982 412 101 ", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user103 = new AppUser() { UserName = "ptdat", EmployeeID = "00266", Email = "ptdat@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("07/13/1984"), FullName = "Phan Thành Đạt", Gender = true, Status = true, GroupId = 3, PhoneNumber = "0932 229 218", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user104 = new AppUser() { UserName = "nqanh", EmployeeID = "00267", Email = "nqanh@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("02/08/1990"), FullName = "Ngô Quang Anh", Gender = true, Status = true, GroupId = 3, PhoneNumber = "0979111686", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user105 = new AppUser() { UserName = "dtluc", EmployeeID = "00022", Email = "dtluc@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("03/02/1981"), FullName = "Đỗ Trọng Lực", Gender = true, Status = true, GroupId = 3, PhoneNumber = "0986188787", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user106 = new AppUser() { UserName = "bvtung", EmployeeID = "00031", Email = "bvtung@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("08/14/1988"), FullName = "Bùi Văn Tùng", Gender = true, Status = true, GroupId = 3, PhoneNumber = "0977.456.159", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user107 = new AppUser() { UserName = "nmphuong1", EmployeeID = "00029", Email = "nmphuong1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("07/10/1987"), FullName = "Nguyễn Minh Phương", Gender = true, Status = true, GroupId = 3, PhoneNumber = "01233446615", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user108 = new AppUser() { UserName = "tvthinh", EmployeeID = "00032", Email = "tvthinh@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("09/27/1988"), FullName = "Trần Văn Thịnh", Gender = true, Status = true, GroupId = 3, PhoneNumber = "0946572510", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user109 = new AppUser() { UserName = "nhnam1", EmployeeID = "00033", Email = "nhnam1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("04/21/1990"), FullName = "Nguyễn Hoàng Nam", Gender = true, Status = true, GroupId = 3, PhoneNumber = "0963693130", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user110 = new AppUser() { UserName = "nsha", EmployeeID = "00035", Email = "nsha@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("10/21/1983"), FullName = "Nguyễn Sơn Hà", Gender = true, Status = true, GroupId = 3, PhoneNumber = "0167.877.4459", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user111 = new AppUser() { UserName = "nbtrung", EmployeeID = "00042", Email = "nbtrung@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("10/04/1990"), FullName = "Nguyễn Bá Trung", Gender = true, Status = true, GroupId = 3, PhoneNumber = "0165.466.0010", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user112 = new AppUser() { UserName = "tvhung11", EmployeeID = "00061", Email = "tvhung11@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("02/20/1992"), FullName = "Trương Văn Hưng", Gender = true, Status = true, GroupId = 3, PhoneNumber = "01266055213", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user113 = new AppUser() { UserName = "dvvinh", EmployeeID = "00086", Email = "dvvinh@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("03/03/1992"), FullName = "Dương Văn Vĩnh", Gender = true, Status = true, GroupId = 3, PhoneNumber = "01665610722", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user114 = new AppUser() { UserName = "nvthanh", EmployeeID = "00001", Email = "nvthanh@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("09/01/1989"), FullName = "Nguyễn Văn Thành", Gender = true, Status = true, GroupId = 3, PhoneNumber = "0978222550", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user115 = new AppUser() { UserName = "nvthong", EmployeeID = "00001", Email = "nvthong@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("05/15/1987"), FullName = "Nguyễn Văn Thông", Gender = true, Status = true, GroupId = 3, PhoneNumber = "0976463151", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user116 = new AppUser() { UserName = "ntdung22", EmployeeID = "00093", Email = "ntdung22@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("06/22/1991"), FullName = "Nguyễn Thế Dũng", Gender = true, Status = true, GroupId = 3, PhoneNumber = "0976696235", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user117 = new AppUser() { UserName = "dddoi", EmployeeID = "00001", Email = "dddoi@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("01/13/1989"), FullName = "Đỗ Đình Đối", Gender = true, Status = true, GroupId = 3, PhoneNumber = "0986629496", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user118 = new AppUser() { UserName = "nmtien", EmployeeID = "00025", Email = "nmtien@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("12/11/1985"), FullName = "Nguyễn Mạnh Tiến", Gender = true, Status = true, GroupId = 3, PhoneNumber = "0966009738", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user119 = new AppUser() { UserName = "phnhung", EmployeeID = "00139", Email = "phnhung@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("08/06/1986"), FullName = "Phạm Hồng Nhung", Gender = false, Status = true, GroupId = 3, PhoneNumber = "0934566941", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user120 = new AppUser() { UserName = "nttrung1", EmployeeID = "00145", Email = "nttrung1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("12/06/1992"), FullName = "Nguyễn Tiến Trung", Gender = true, Status = true, GroupId = 3, PhoneNumber = "01649701685", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user121 = new AppUser() { UserName = "pvoanh", EmployeeID = "00146", Email = "pvoanh@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("10/02/1991"), FullName = "Phạm Văn Oanh", Gender = true, Status = true, GroupId = 3, PhoneNumber = "0962367493", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user122 = new AppUser() { UserName = "ntpthanh", EmployeeID = "00178", Email = "ntpthanh@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("04/09/1991"), FullName = "Nguyễn Thị Phương Thanh", Gender = false, Status = true, GroupId = 3, PhoneNumber = "0975808327", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user123 = new AppUser() { UserName = "dnthien", EmployeeID = "00179", Email = "dnthien@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("02/16/1989"), FullName = "Đinh Ngọc Thiện", Gender = true, Status = true, GroupId = 3, PhoneNumber = "0968981115", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user124 = new AppUser() { UserName = "vndinh", EmployeeID = "00197", Email = "vndinh@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("03/14/1993"), FullName = "Vũ Nam Định", Gender = true, Status = true, GroupId = 3, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user125 = new AppUser() { UserName = "laduc", EmployeeID = "00194", Email = "laduc@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("03/26/1988"), FullName = "Lê Anh Đức", Gender = true, Status = true, GroupId = 3, PhoneNumber = "0934570060", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user126 = new AppUser() { UserName = "ltson2", EmployeeID = "00188", Email = "ltson2@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("09/22/1992"), FullName = "Lê Tuấn Sơn", Gender = true, Status = true, GroupId = 3, PhoneNumber = "01677975848", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user127 = new AppUser() { UserName = "ntgiang2", EmployeeID = "00193", Email = "ntgiang2@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("10/22/1993"), FullName = "Ngô Thanh Giang", Gender = true, Status = true, GroupId = 3, PhoneNumber = "01692201266", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user128 = new AppUser() { UserName = "ngphuong", EmployeeID = "00196", Email = "ngphuong@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("03/20/1994"), FullName = "Nguyễn Giản Phương", Gender = true, Status = true, GroupId = 3, PhoneNumber = "0983.911.746", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user129 = new AppUser() { UserName = "nhviet", EmployeeID = "00001", Email = "nhviet@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("05/16/1987"), FullName = "Nguyễn Hoàng Việt", Gender = true, Status = true, GroupId = 3, PhoneNumber = "0967643587", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user130 = new AppUser() { UserName = "tvtien", EmployeeID = "00271", Email = "tvtien@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("06/04/1996"), FullName = "Trần Văn Tiến", Gender = true, Status = true, GroupId = 3, PhoneNumber = "0984426506", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user131 = new AppUser() { UserName = "ltngoc", EmployeeID = "00212", Email = "ltngoc@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("04/19/1991"), FullName = "Lê Thị Ngọc", Gender = false, Status = true, GroupId = 3, PhoneNumber = "0968 541 542", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user132 = new AppUser() { UserName = "nmhanh", EmployeeID = "00218", Email = "nmhanh@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("03/17/1989"), FullName = "Ngô Mỹ Hạnh", Gender = false, Status = true, GroupId = 3, PhoneNumber = "094 820 3838", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user133 = new AppUser() { UserName = "nmtuyen", EmployeeID = "00001", Email = "nmtuyen@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("02/18/1988"), FullName = "Ngô Minh Tuyên", Gender = true, Status = true, GroupId = 3, PhoneNumber = "0973654331", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user134 = new AppUser() { UserName = "vtathu", EmployeeID = "00231", Email = "vtathu@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("09/03/1991"), FullName = "Vũ Thị Anh Thư", Gender = false, Status = true, GroupId = 3, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user135 = new AppUser() { UserName = "vdtrung", EmployeeID = "00001", Email = "vdtrung@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("07/09/1991"), FullName = "Vũ Đức Trung", Gender = true, Status = true, GroupId = 3, PhoneNumber = "01696723220", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user136 = new AppUser() { UserName = "nvhai", EmployeeID = "00279", Email = "nvhai@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("04/01/1993"), FullName = "Nguyễn Văn Hải", Gender = true, Status = true, GroupId = 3, PhoneNumber = "01658858889", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user137 = new AppUser() { UserName = "nvtrung1", EmployeeID = "00280", Email = "nvtrung1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("02/16/1991"), FullName = "Nguyễn Văn Trung", Gender = true, Status = true, GroupId = 3, PhoneNumber = "01645​ ​588​ ​678", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user138 = new AppUser() { UserName = "lhtuyen", EmployeeID = "00284", Email = "lhtuyen@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("01/09/1991"), FullName = "Lê Hữu Tuyên", Gender = true, Status = true, GroupId = 3, PhoneNumber = "0969563222", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user139 = new AppUser() { UserName = "bxtrung", EmployeeID = "00293", Email = "bxtrung@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("02/06/1984"), FullName = "Bùi Xuân Trung", Gender = true, Status = true, GroupId = 3, PhoneNumber = "0913291911", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user140 = new AppUser() { UserName = "vvbinh", EmployeeID = "00290", Email = "vvbinh@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("12/25/1990"), FullName = "Vũ Văn Bình", Gender = true, Status = true, GroupId = 3, PhoneNumber = "0981036486", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user141 = new AppUser() { UserName = "dtnghia", EmployeeID = "00074", Email = "dtnghia@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("11/01/1984"), FullName = "Đỗ Trọng Nghĩa", Gender = true, Status = true, GroupId = 3, PhoneNumber = "0944000035", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user142 = new AppUser() { UserName = "hthoa1", EmployeeID = "00176", Email = "hthoa1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("03/23/1985"), FullName = "Hoàng Thị Hòa", Gender = false, Status = true, GroupId = 12, PhoneNumber = "0966801338", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user143 = new AppUser() { UserName = "nthuyen", EmployeeID = "00216", Email = "nthuyen@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("08/18/1995"), FullName = "Nguyễn Thị Huyền", Gender = false, Status = true, GroupId = 12, PhoneNumber = "0962409596", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user144 = new AppUser() { UserName = "lmlinh", EmployeeID = "00210", Email = "lmlinh@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("02/06/1985"), FullName = "Lý Mai Linh", Gender = false, Status = true, GroupId = 12, PhoneNumber = "01238217932", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user145 = new AppUser() { UserName = "ntthuy1", EmployeeID = "00001", Email = "ntthuy1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("01/14/1991"), FullName = "Nguyễn Thị Thúy", Gender = false, Status = true, GroupId = 12, PhoneNumber = "01674548732", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user146 = new AppUser() { UserName = "nhha", EmployeeID = "00228", Email = "nhha@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("11/05/1987"), FullName = "Nguyễn Hải Hà", Gender = false, Status = true, GroupId = 12, PhoneNumber = "0987.0334.87", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user147 = new AppUser() { UserName = "dtxtuoi", EmployeeID = "00297", Email = "dtxtuoi@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("11/21/1990"), FullName = "Đặng Thị Xuân Tươi", Gender = false, Status = true, GroupId = 12, PhoneNumber = "0979368981", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user148 = new AppUser() { UserName = "thngoc", EmployeeID = "00285", Email = "thngoc@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("12/21/1990"), FullName = "Trần Hồng Ngọc ", Gender = false, Status = true, GroupId = 12, PhoneNumber = "0942814548", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user149 = new AppUser() { UserName = "dtpthanh", EmployeeID = "00308", Email = "dtpthanh@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("09/13/1989"), FullName = "Đặng Thị Phương Thanh", Gender = false, Status = true, GroupId = 12, PhoneNumber = "0948046115", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user150 = new AppUser() { UserName = "btthoa", EmployeeID = "00001", Email = "btthoa@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("06/06/1985"), FullName = "Bùi Thị Thoa", Gender = false, Status = true, GroupId = 12, PhoneNumber = "01676005188", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user151 = new AppUser() { UserName = "ntluyen1", EmployeeID = "00026", Email = "ntluyen1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("07/07/1989"), FullName = "Nguyễn Thị Luyến", Gender = false, Status = true, GroupId = 13, PhoneNumber = "0972197589", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user152 = new AppUser() { UserName = "ntcuc", EmployeeID = "00001", Email = "ntcuc@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("09/28/1983"), FullName = "Nguyễn Thị Cúc", Gender = false, Status = true, GroupId = 13, PhoneNumber = "01222285549", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user153 = new AppUser() { UserName = "lthngoc", EmployeeID = "00052", Email = "lthngoc@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("02/19/1984"), FullName = "Lưu Thị Hồng Ngọc", Gender = false, Status = true, GroupId = 13, PhoneNumber = "0904366223", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user154 = new AppUser() { UserName = "ttyen", EmployeeID = "00001", Email = "ttyen@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("03/10/1989"), FullName = "Trần Thị Yên", Gender = false, Status = true, GroupId = 13, PhoneNumber = "0944379236", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user155 = new AppUser() { UserName = "nthong", EmployeeID = "00066", Email = "nthong@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("01/20/1991"), FullName = "Ngô Thúy Hồng", Gender = false, Status = true, GroupId = 13, PhoneNumber = "01674533403", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user156 = new AppUser() { UserName = "ptnu", EmployeeID = "00067", Email = "ptnu@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("02/05/1992"), FullName = "Phạm Thị Nụ", Gender = false, Status = true, GroupId = 13, PhoneNumber = "01695892048", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user157 = new AppUser() { UserName = "ttdung1", EmployeeID = "00070", Email = "ttdung1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("02/26/1987"), FullName = "Trần Thị Dung", Gender = false, Status = true, GroupId = 13, PhoneNumber = "0978580688", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user158 = new AppUser() { UserName = "btxuan", EmployeeID = "00084", Email = "btxuan@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("07/21/1989"), FullName = "Bùi Thị Xuân", Gender = false, Status = true, GroupId = 13, PhoneNumber = "01664378427", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user159 = new AppUser() { UserName = "tttien", EmployeeID = "00068", Email = "tttien@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("05/20/1989"), FullName = "Trương Thị Tiến", Gender = false, Status = true, GroupId = 13, PhoneNumber = "01674692751", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user160 = new AppUser() { UserName = "ntbang", EmployeeID = "00148", Email = "ntbang@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("12/26/1993"), FullName = "Nguyễn Thị Bằng", Gender = false, Status = true, GroupId = 13, PhoneNumber = "01684191392", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user161 = new AppUser() { UserName = "dtphuong", EmployeeID = "00149", Email = "dtphuong@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("10/26/1994"), FullName = "Dương Thị Phượng", Gender = false, Status = true, GroupId = 13, PhoneNumber = "0973026365", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user162 = new AppUser() { UserName = "tththuong", EmployeeID = "00190", Email = "tththuong@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("06/09/1992"), FullName = "Trần Thị Hoài Thương", Gender = false, Status = true, GroupId = 13, PhoneNumber = "0912279692", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user163 = new AppUser() { UserName = "ltvdung", EmployeeID = "00201", Email = "ltvdung@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("08/02/1994"), FullName = "Lâm Thị Việt Dung", Gender = false, Status = true, GroupId = 13, PhoneNumber = "01636750167", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user164 = new AppUser() { UserName = "htquynh", EmployeeID = "00200", Email = "htquynh@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("11/12/1989"), FullName = "Hoàng Thị Quỳnh", Gender = false, Status = true, GroupId = 13, PhoneNumber = "01655461368", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user165 = new AppUser() { UserName = "ptthuy", EmployeeID = "00202", Email = "ptthuy@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("11/25/1995"), FullName = "Phạm Thị Thùy", Gender = false, Status = true, GroupId = 13, PhoneNumber = "01657248973 ", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user166 = new AppUser() { UserName = "ntmua", EmployeeID = "00206", Email = "ntmua@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("02/05/1995"), FullName = "Ngọc Thị Mùa", Gender = false, Status = true, GroupId = 13, PhoneNumber = "01634357804", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user167 = new AppUser() { UserName = "nttgiang", EmployeeID = "00215", Email = "nttgiang@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("11/16/1990"), FullName = "Nguyễn Thị Thu Giang", Gender = false, Status = true, GroupId = 13, PhoneNumber = "0984 740 467", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user168 = new AppUser() { UserName = "ttthien", EmployeeID = "00211", Email = "ttthien@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("08/19/1994"), FullName = "Trịnh Thị Thu Hiền", Gender = false, Status = true, GroupId = 13, PhoneNumber = "0978026128", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user169 = new AppUser() { UserName = "pthuyen1", EmployeeID = "00219", Email = "pthuyen1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("05/15/1985"), FullName = "Phạm Thị Huyền", Gender = false, Status = true, GroupId = 13, PhoneNumber = "0914363085", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user170 = new AppUser() { UserName = "lthoa", EmployeeID = "00001", Email = "lthoa@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("06/07/1990"), FullName = "Lê Thị Hoa", Gender = false, Status = true, GroupId = 13, PhoneNumber = "0985217005", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user171 = new AppUser() { UserName = "ptttrang", EmployeeID = "00001", Email = "ptttrang@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("10/22/1987"), FullName = "Phạm Thị Thu Trang", Gender = false, Status = true, GroupId = 13, PhoneNumber = "0868463606", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user172 = new AppUser() { UserName = "dnbao", EmployeeID = "00136", Email = "dnbao@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("11/10/1983"), FullName = "Đặng Ngọc Bảo", Gender = true, Status = true, GroupId = 9, PhoneNumber = "0913088897", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user173 = new AppUser() { UserName = "nntam", EmployeeID = "00227", Email = "nntam@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("01/08/1982"), FullName = "Nguyễn Ngọc Tâm", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0936128336", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user174 = new AppUser() { UserName = "dkhuyen", EmployeeID = "00159", Email = "dkhuyen@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("02/04/1995"), FullName = "Đinh Khánh Huyền", Gender = false, Status = true, GroupId = 14, PhoneNumber = "01277718282", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user175 = new AppUser() { UserName = "tvhuan", EmployeeID = "00164", Email = "tvhuan@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("11/13/1992"), FullName = "Trần Văn Huấn", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0977236681", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user176 = new AppUser() { UserName = "tvdung1", EmployeeID = "00166", Email = "tvdung1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("09/16/1995"), FullName = "Triệu Văn Dũng", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0974267432", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user177 = new AppUser() { UserName = "ldkien", EmployeeID = "00001", Email = "ldkien@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("10/04/1995"), FullName = "Lê Doãn Kiên", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0969139254", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user178 = new AppUser() { UserName = "nthoan", EmployeeID = "00163", Email = "nthoan@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("04/05/1995"), FullName = "Nguyễn Thị Hoan", Gender = false, Status = true, GroupId = 14, PhoneNumber = "01635558342", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user179 = new AppUser() { UserName = "nvbao", EmployeeID = "00001", Email = "nvbao@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("11/17/1994"), FullName = "Nguyễn Văn Bảo", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0943229095", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user180 = new AppUser() { UserName = "thnam", EmployeeID = "00165", Email = "thnam@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("12/29/1995"), FullName = "Trịnh Hoài Nam", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0985931484", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user181 = new AppUser() { UserName = "ddquy", EmployeeID = "00162", Email = "ddquy@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("04/23/1995"), FullName = "Dương Đình Quý", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0976187423", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user182 = new AppUser() { UserName = "nvlong", EmployeeID = "00169", Email = "nvlong@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("08/29/1996"), FullName = "Nguyễn Văn Long", Gender = true, Status = true, GroupId = 14, PhoneNumber = "01663085890", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user183 = new AppUser() { UserName = "dtthuyen1", EmployeeID = "00173", Email = "dtthuyen1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("12/01/1995"), FullName = "Đào Thị Thanh Huyền", Gender = false, Status = true, GroupId = 14, PhoneNumber = "0978566639", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user184 = new AppUser() { UserName = "bvquyet", EmployeeID = "00001", Email = "bvquyet@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("11/12/1994"), FullName = "Bùi Văn Quyết", Gender = true, Status = true, GroupId = 14, PhoneNumber = "01227943681", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user185 = new AppUser() { UserName = "ldcuong1", EmployeeID = "00155", Email = "ldcuong1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("07/13/1994"), FullName = "Lưu Đức Cương", Gender = true, Status = true, GroupId = 14, PhoneNumber = "01674313908 ", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user186 = new AppUser() { UserName = "dhgiang1", EmployeeID = "00170", Email = "dhgiang1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("09/17/1994"), FullName = "Dương Hoàng Giang", Gender = true, Status = true, GroupId = 14, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user187 = new AppUser() { UserName = "ntgiang1", EmployeeID = "00167", Email = "ntgiang1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("09/11/1995"), FullName = "Nguyễn Thị Giang", Gender = false, Status = true, GroupId = 14, PhoneNumber = "0968128976", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user188 = new AppUser() { UserName = "nvkhoa", EmployeeID = "00156", Email = "nvkhoa@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("12/01/1995"), FullName = "Nguyễn Văn Khoa", Gender = true, Status = true, GroupId = 14, PhoneNumber = "01686715123", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user189 = new AppUser() { UserName = "tdninh", EmployeeID = "00171", Email = "tdninh@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("01/09/1995"), FullName = "Trần Đình Ninh", Gender = true, Status = true, GroupId = 14, PhoneNumber = "01682980886", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user190 = new AppUser() { UserName = "dvngoc", EmployeeID = "00161", Email = "dvngoc@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("11/17/1991"), FullName = "Đoàn Văn Ngọc", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0903469861", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user191 = new AppUser() { UserName = "vcthanh", EmployeeID = "00001", Email = "vcthanh@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("03/31/1994"), FullName = "Vũ Công Thành", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0968525931", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user192 = new AppUser() { UserName = "nttrung2", EmployeeID = "00157", Email = "nttrung2@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("07/09/1994"), FullName = "Nguyễn Thế Trung", Gender = true, Status = true, GroupId = 14, PhoneNumber = "01655960450", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user193 = new AppUser() { UserName = "ntvanh1", EmployeeID = "00001", Email = "ntvanh1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("07/13/1996"), FullName = "Nguyễn Thị Vân Anh", Gender = false, Status = true, GroupId = 14, PhoneNumber = "01633388315", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user194 = new AppUser() { UserName = "vtson", EmployeeID = "00175", Email = "vtson@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("12/25/1995"), FullName = "Vũ Tùng Sơn", Gender = true, Status = true, GroupId = 14, PhoneNumber = "01658605213", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user195 = new AppUser() { UserName = "lvtrung", EmployeeID = "00213", Email = "lvtrung@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("01/12/1989"), FullName = "Lê Việt Trung", Gender = true, Status = true, GroupId = 14, PhoneNumber = "01646537743", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user196 = new AppUser() { UserName = "phgiang", EmployeeID = "00277", Email = "phgiang@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("04/04/1991"), FullName = "Phạm Hương Giang", Gender = false, Status = true, GroupId = 14, PhoneNumber = "0932323069", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user197 = new AppUser() { UserName = "hhanh", EmployeeID = "00278", Email = "hhanh@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("12/04/1994"), FullName = "Hoàng Hà Anh", Gender = false, Status = true, GroupId = 14, PhoneNumber = "0983641994", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user198 = new AppUser() { UserName = "nhthanh1", EmployeeID = "00001", Email = "nhthanh@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("12/17/1989"), FullName = "Nguyễn Huy Thành", Gender = true, Status = true, GroupId = 14, PhoneNumber = "01682844486", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user199 = new AppUser() { UserName = "pvcanh", EmployeeID = "00301", Email = "pvcanh@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("09/02/1981"), FullName = "Phạm Văn Cảnh", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0975363518", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user200 = new AppUser() { UserName = "vthung", EmployeeID = "00001", Email = "vthung@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("07/01/1996"), FullName = "Vũ Thế Hùng", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0989501034", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user201 = new AppUser() { UserName = "ntquy", EmployeeID = "00234", Email = "ntquy@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("08/30/1992"), FullName = "Nguyễn Tất Quý", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0978308929", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user202 = new AppUser() { UserName = "pxhoang", EmployeeID = "00235", Email = "pxhoang@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("11/05/1992"), FullName = "Phùng Xuân Hoàng", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0974404620 ", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user203 = new AppUser() { UserName = "nhphong", EmployeeID = "00236", Email = "nhphong@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("09/18/1991"), FullName = "Nguyễn Hồng Phong", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0969789918", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user204 = new AppUser() { UserName = "lxlinh", EmployeeID = "00237", Email = "lxlinh@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("11/25/1994"), FullName = "Lê Xuân Linh", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0962919015", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user205 = new AppUser() { UserName = "tmgiang", EmployeeID = "00238", Email = "tmgiang@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("05/21/1992"), FullName = "Tống Minh Giang", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0949348386", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user206 = new AppUser() { UserName = "ddhieu", EmployeeID = "00239", Email = "ddhieu@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("07/28/1995"), FullName = "Đỗ Duy Hiếu", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0942008783", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user207 = new AppUser() { UserName = "nvangoc", EmployeeID = "00240", Email = "nvangoc@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("02/13/1991"), FullName = "Nguyễn Văn Anh Ngọc", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0906586300", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user208 = new AppUser() { UserName = "lhdoan", EmployeeID = "00241", Email = "lhdoan@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("02/04/1995"), FullName = "Lê Hữu Đoàn", Gender = true, Status = true, GroupId = 14, PhoneNumber = "01697970132", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user209 = new AppUser() { UserName = "nvquy1", EmployeeID = "00242", Email = "nvquy1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("11/14/1995"), FullName = "Ngô Văn Quý", Gender = true, Status = true, GroupId = 14, PhoneNumber = "01686120577", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user210 = new AppUser() { UserName = "nchung", EmployeeID = "00243", Email = "nchung@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("05/22/1992"), FullName = "Nguyễn Cảnh Hưng", Gender = true, Status = true, GroupId = 14, PhoneNumber = "01675.777.523", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user211 = new AppUser() { UserName = "ldthien", EmployeeID = "00244", Email = "ldthien@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("04/28/1988"), FullName = "Lê Đức Thiện", Gender = true, Status = true, GroupId = 14, PhoneNumber = "01639 614 640", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user212 = new AppUser() { UserName = "lcnguyen", EmployeeID = "00245", Email = "lcnguyen@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("06/27/1995"), FullName = "Lê Cao Nguyên", Gender = true, Status = true, GroupId = 14, PhoneNumber = "01638189359", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user213 = new AppUser() { UserName = "nnduy", EmployeeID = "00246", Email = "nnduy@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("12/20/1995"), FullName = "Nguyễn Ngọc Duy", Gender = true, Status = true, GroupId = 14, PhoneNumber = "01632782798", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user214 = new AppUser() { UserName = "nhanh3", EmployeeID = "00247", Email = "nhanh3@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("08/25/1994"), FullName = "Nguyễn Huy Anh", Gender = true, Status = true, GroupId = 14, PhoneNumber = "01696348522", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user215 = new AppUser() { UserName = "vdhao", EmployeeID = "00248", Email = "vdhao@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("04/10/1994"), FullName = "Vũ Đình Hào", Gender = true, Status = true, GroupId = 14, PhoneNumber = null, StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user216 = new AppUser() { UserName = "dqthang", EmployeeID = "00250", Email = "dqthang@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("11/05/1995"), FullName = "Đào Quang Thắng", Gender = true, Status = true, GroupId = 14, PhoneNumber = "01648957552", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user217 = new AppUser() { UserName = "nvtruong1", EmployeeID = "00251", Email = "nvtruong1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("05/05/1992"), FullName = "Nguyễn Văn Trường", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0972677751", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user218 = new AppUser() { UserName = "tqhuy2", EmployeeID = "00252", Email = "tqhuy2@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("06/25/1995"), FullName = "Tạ Quang Huy", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0966236917", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user219 = new AppUser() { UserName = "dnduc", EmployeeID = "00253", Email = "dnduc@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("03/15/1994"), FullName = "Đoàn Ngọc Đức", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0902150394", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user220 = new AppUser() { UserName = "nbson", EmployeeID = "00254", Email = "nbson@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("10/01/1990"), FullName = "Nguyễn Bá Sơn", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0963402025", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user221 = new AppUser() { UserName = "lvtung1", EmployeeID = "00255", Email = "lvtung1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("03/17/1993"), FullName = "Lăng Vĩnh Tùng", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0976125333", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user222 = new AppUser() { UserName = "nvphuc", EmployeeID = "00256", Email = "nvphuc@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("11/18/1995"), FullName = "Nguyễn Văn Phúc", Gender = true, Status = true, GroupId = 14, PhoneNumber = "01645922048", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user223 = new AppUser() { UserName = "nvthang", EmployeeID = "00257", Email = "nvthang@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("01/12/1994"), FullName = "Nguyễn Văn Thắng", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0979656931", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user224 = new AppUser() { UserName = "dmtuong", EmployeeID = "00258", Email = "dmtuong@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("10/13/1993"), FullName = "Đặng Mạnh Tường", Gender = true, Status = true, GroupId = 14, PhoneNumber = "01674187929", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user225 = new AppUser() { UserName = "nvcau", EmployeeID = "00259", Email = "nvcau@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("05/30/1997"), FullName = "Nguyễn Văn Cầu", Gender = true, Status = true, GroupId = 14, PhoneNumber = "01635293688", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user226 = new AppUser() { UserName = "vxthien", EmployeeID = "00260", Email = "vxthien@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("09/04/1995"), FullName = "Vũ Xuân Thiện", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0975578454", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user227 = new AppUser() { UserName = "hnquyen", EmployeeID = "00261", Email = "hnquyen@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("02/16/1991"), FullName = "Hoàng Nghĩa Quyền", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0908530337", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user228 = new AppUser() { UserName = "ltdat1", EmployeeID = "00262", Email = "ltdat1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("08/28/1995"), FullName = "Lê Tiến Đạt", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0978845253", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user229 = new AppUser() { UserName = "Natuan1", EmployeeID = "00001", Email = "Natuan1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("05/09/1996"), FullName = "Nguyễn Anh Tuấn", Gender = true, Status = true, GroupId = 14, PhoneNumber = "01255481020", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user230 = new AppUser() { UserName = "vdhoan", EmployeeID = "00264", Email = "vdhoan@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("07/01/1994"), FullName = "Vũ Đình Hoàn", Gender = true, Status = true, GroupId = 14, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user231 = new AppUser() { UserName = "ttson", EmployeeID = "00269", Email = "ttson@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("10/25/1990"), FullName = "Trương Thái Sơn", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0964238004", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user232 = new AppUser() { UserName = "nqthang", EmployeeID = "00283", Email = "nqthang@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("04/14/1988"), FullName = "Nguyễn Quyết Thăng", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0983659699", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user233 = new AppUser() { UserName = "cvhieu", EmployeeID = "00001", Email = "cvhieu@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("11/03/1993"), FullName = "Chu Văn Hiếu", Gender = true, Status = true, GroupId = 14, PhoneNumber = "01644990935", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user234 = new AppUser() { UserName = "ntduy", EmployeeID = "00001", Email = "ntduy@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("05/08/1994"), FullName = "Nguyễn Trọng Duy ", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0975572524", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user235 = new AppUser() { UserName = "htdung1", EmployeeID = "00291", Email = "htdung1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("03/31/1992"), FullName = "Hồ Tiến Dũng", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0912631392", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user236 = new AppUser() { UserName = "nhnam2", EmployeeID = "00001", Email = "nhnam2@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("11/27/1993"), FullName = "Nguyễn Hoàng Nam", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0931118493", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user237 = new AppUser() { UserName = "ntluan", EmployeeID = "00001", Email = "ntluan@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("10/24/1990"), FullName = "Nguyễn Thành Luân", Gender = true, Status = true, GroupId = 14, PhoneNumber = "1693251061", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user238 = new AppUser() { UserName = "nvdong", EmployeeID = "00001", Email = "nvdong@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("09/10/1992"), FullName = "Nguyễn Văn Đông", Gender = true, Status = true, GroupId = 14, PhoneNumber = "01674599982", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user239 = new AppUser() { UserName = "vbhuan", EmployeeID = "00305", Email = "vbhuan@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("07/15/1979"), FullName = "Vũ Bá Huấn", Gender = true, Status = true, GroupId = 14, PhoneNumber = "0985238383", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user240 = new AppUser() { UserName = "ptuyen", EmployeeID = "00306", Email = "ptuyen@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("07/15/1989"), FullName = "Phạm Tố Uyên", Gender = false, Status = true, GroupId = 14, PhoneNumber = "0963150789", StartWorkingDay = DateTime.Parse("2017-03-31") };
                var user241 = new AppUser() { UserName = "cmquang", EmployeeID = "00341", Email = "cmquang@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("01/01/2001"), FullName = "Chu Mạnh Quang", Gender = true, Status = true, GroupId = 14, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2018-04-01") };
                var user242 = new AppUser() { UserName = "hnhung", EmployeeID = "00001", Email = "hnhung@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("09/02/1961"), FullName = "Hoàng Ngọc Hùng", Gender = true, Status = true, GroupId = 9, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2017-04-01") };
                var user243 = new AppUser() { UserName = "mkumeda", EmployeeID = "00177", Email = "mkumeda@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("04/29/1953"), FullName = "Masakuni Kumeda", Gender = true, Status = true, GroupId = null, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2017-04-01") };
                var user244 = new AppUser() { UserName = "nmha", EmployeeID = "00349", Email = "nmha@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("03/23/1987"), FullName = "Nguyễn Minh Hà", Gender = true, Status = true, GroupId = 8, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2018-06-10") };
                var user245 = new AppUser() { UserName = "nxhuy", EmployeeID = "00318", Email = "nxhuy@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("06/01/1990"), FullName = "Nguyễn Xuân Huy", Gender = true, Status = true, GroupId = 8, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2018-06-10") };
                var user246 = new AppUser() { UserName = "vnmanh", EmployeeID = "00327", Email = "vnmanh@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("02/16/1994"), FullName = "Vũ Ngọc Mai Anh", Gender = true, Status = true, GroupId = 8, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2018-03-13") };
                var user247 = new AppUser() { UserName = "htthoa", EmployeeID = "00325", Email = "htthoa@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("04/26/1980"), FullName = "Hoàng Thị Thanh Hoa", Gender = true, Status = true, GroupId = 8, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2018-03-07") };
                //tuong them
                var user248 = new AppUser() { UserName = "nttrang2", EmployeeID = "00338", Email = "nttrang2@cmc.com", EmailConfirmed = true, BirthDay = DateTime.Parse("03/14/1993"), FullName = "Nguyễn Thu Trang", Gender = true, Status = true, GroupId = 15, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2018-05-02") };
                var user249 = new AppUser() { UserName = "dnanh1", EmployeeID = "00340", Email = "dnanh1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("10/13/1991"), FullName = "Phạm Thị Nhâm", Gender = true, Status = true, GroupId = 15, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2018-05-02") };
                var user250 = new AppUser() { UserName = "ptnham", EmployeeID = "00353", Email = "ptnham@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("04/04/1992"), FullName = "Đặng Ngọc Anh", Gender = true, Status = true, GroupId = 15, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2018-06-19") };
                var user254 = new AppUser() { UserName = "hha", EmployeeID = "00079", Email = "hha@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("07/12/1990"), FullName = "Hoàng Hà", Gender = true, Status = true, GroupId = 1, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2018-06-18") };
                var user259 = new AppUser() { UserName = "tttthuy", EmployeeID = "00326", Email = "tttthuy@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("10/15/1993"), FullName = "Trần Thị Thanh Thúy", Gender = true, Status = true, GroupId = 1, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2018-05-02") };
                var user261 = new AppUser() { UserName = "byloan", EmployeeID = "00352", Email = "byloan@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("02/07/1992"), FullName = "Bùi Yến Loan", Gender = true, Status = true, GroupId = 1, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2018-06-18") };
                var user266 = new AppUser() { UserName = "dthai2", EmployeeID = "00302", Email = "dthai2@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("02/16/1992"), FullName = "Đỗ Trọng Hải", Gender = true, Status = true, GroupId = 2, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2018-07-11") };



                //đức thêm
                var user251 = new AppUser() { UserName = "nhanh5", EmployeeID = "00314", Email = "nhanh5@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("07/20/1988"), FullName = "Nguyễn Huy Anh", Gender = true, Status = true, GroupId = 3, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2018-02-26") };
                var user252 = new AppUser() { UserName = "ttvanh1", EmployeeID = "00315", Email = "ttvanh1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("09/02/1993"), FullName = "Trịnh Thị Vân Anh", Gender = true, Status = true, GroupId = 3, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2018-03-01") };
                var user253 = new AppUser() { UserName = "bndhieu", EmployeeID = "00319", Email = "bndhieu@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("09/14/1992"), FullName = "Bùi Nguyễn Duy Hiêu", Gender = true, Status = true, GroupId = 3, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2018-03-05") };
                var user255 = new AppUser() { UserName = "ltlthao", EmployeeID = "00321", Email = "ltlthao@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("08/25/1991"), FullName = "Lương Tống Lan Thảo", Gender = true, Status = true, GroupId = 3, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2018-03-05") };
                var user256 = new AppUser() { UserName = "vhtuan", EmployeeID = "00329", Email = "vhtuan@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("08/16/1995"), FullName = "Vũ Hoàng Tuấn", Gender = true, Status = true, GroupId = 3, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2018-03-26") };
                var user257 = new AppUser() { UserName = "lksy", EmployeeID = "00333", Email = "lksy@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("06/02/1994"), FullName = "Lê Khả Sỹ", Gender = true, Status = true, GroupId = 3, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2018-04-02") };
                var user258 = new AppUser() { UserName = "dthung", EmployeeID = "00332", Email = "dthung@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("09/20/1992"), FullName = "Đổng Trọng Hùng", Gender = true, Status = true, GroupId = 3, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2018-04-04") };
                var user260 = new AppUser() { UserName = "lqkhang", EmployeeID = "00313", Email = "lqkhang@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("10/29/1982"), FullName = "Lê Quang Khang", Gender = true, Status = true, GroupId = 3, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2018-02-23") };
                var user262 = new AppUser() { UserName = "nthung1", EmployeeID = "00336", Email = "nthung1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("10/04/1992"), FullName = "Nguyễn Thế Hưng", Gender = true, Status = true, GroupId = 3, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2018-04-17") };
                var user263 = new AppUser() { UserName = "ldphu", EmployeeID = "00339", Email = "ldphu@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("12/12/1990"), FullName = "Lê Duy Phú", Gender = true, Status = true, GroupId = 3, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2018-05-02") };
                var user264 = new AppUser() { UserName = "nthong1", EmployeeID = "00344", Email = "nthong1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("06/02/1988"), FullName = "Nguyễn Thanh Hồng", Gender = true, Status = true, GroupId = 3, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2018-06-01") };
                var user265 = new AppUser() { UserName = "ndkhanh1", EmployeeID = "00354", Email = "ndkhanh1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("11/11/1990"), FullName = "Nhữ Đình Khánh", Gender = true, Status = true, GroupId = 3, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2018-06-20") };
                var user267 = new AppUser() { UserName = "pthuong", EmployeeID = "00309", Email = "pthuong@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("07/01/1996"), FullName = "Phạm Thị Hương", Gender = true, Status = true, GroupId = 12, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2018-02-26") };
                var user268 = new AppUser() { UserName = "dhyen", EmployeeID = "00311", Email = "dhyen@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("04/27/1995"), FullName = "Đào Hải Yến", Gender = true, Status = true, GroupId = 12, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2018-02-26") };
                var user269 = new AppUser() { UserName = "vhha", EmployeeID = "00345", Email = "vhha@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("07/28/1991"), FullName = "Vũ Hải Hà", Gender = true, Status = true, GroupId = 12, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2018-06-01") };
                var user270 = new AppUser() { UserName = "nttha1", EmployeeID = "00334", Email = "nttha1@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("05/25/1992"), FullName = "Nguyễn Thị Thu Hà", Gender = true, Status = true, GroupId = 14, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2018-04-09") };
                var user271 = new AppUser() { UserName = "dttrung", EmployeeID = "00335", Email = "dttrung@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("05/24/1988"), FullName = "Đặng Thế Trung", Gender = true, Status = true, GroupId = 14, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2018-04-16") };
                var user272 = new AppUser() { UserName = "ththao", EmployeeID = "00337", Email = "ththao@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("02/14/1985"), FullName = "Trần Hữu Thảo", Gender = true, Status = true, GroupId = 14, PhoneNumber = "", StartWorkingDay = DateTime.Parse("2018-05-02") };

                //Tùng thêm
                var user999 = new AppUser() { UserName = "admin", Email = "admin@cmc.com.vn", EmailConfirmed = true, BirthDay = DateTime.Parse("04/01/1993"), FullName = "Super Admin", Gender = true, Status = true, GroupId = 16, PhoneNumber = "", StartWorkingDay = DateTime.Parse("04/01/1993") };
                if (!roleManager.Roles.Any())
                {
                    roleManager.Create(new AppRole { Name = "Admin", Description = "Admin Officer" });
                    roleManager.Create(new AppRole { Name = "Member", Description = "Member" });
                    roleManager.Create(new AppRole { Name = "GroupLead", Description = "Group Lead" });
                    roleManager.Create(new AppRole { Name = "HR", Description = "Human Resource" });
                    roleManager.Create(new AppRole { Name = "SuperAdmin", Description = "Super Admin" });
                }
                if (manager.Users.Count(x => x.UserName == "txlam") == 0) { manager.Create(user2, "123456a@"); var adminUser = manager.FindByName("txlam"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ntnhan") == 0) { manager.Create(user3, "123456a@"); var adminUser = manager.FindByName("ntnhan"); manager.AddToRoles(adminUser.Id, new string[] { "HR" }); }
                if (manager.Users.Count(x => x.UserName == "ntanh11") == 0) { manager.Create(user4, "123456a@"); var adminUser = manager.FindByName("ntanh11"); manager.AddToRoles(adminUser.Id, new string[] { "HR" }); }
                if (manager.Users.Count(x => x.UserName == "ptnanh") == 0) { manager.Create(user5, "123456a@"); var adminUser = manager.FindByName("ptnanh"); manager.AddToRoles(adminUser.Id, new string[] { "HR" }); }
                if (manager.Users.Count(x => x.UserName == "nttrang1") == 0) { manager.Create(user6, "123456a@"); var adminUser = manager.FindByName("nttrang1"); manager.AddToRoles(adminUser.Id, new string[] { "HR" }); }
                if (manager.Users.Count(x => x.UserName == "ttpthao2") == 0) { manager.Create(user7, "123456a@"); var adminUser = manager.FindByName("ttpthao2"); manager.AddToRoles(adminUser.Id, new string[] { "HR" }); }
                if (manager.Users.Count(x => x.UserName == "vthnhung1") == 0) { manager.Create(user8, "123456a@"); var adminUser = manager.FindByName("vthnhung1"); manager.AddToRoles(adminUser.Id, new string[] { "HR" }); }
                if (manager.Users.Count(x => x.UserName == "ltha") == 0) { manager.Create(user9, "123456a@"); var adminUser = manager.FindByName("ltha"); manager.AddToRoles(adminUser.Id, new string[] { "HR" }); }
                if (manager.Users.Count(x => x.UserName == "dhthao1") == 0) { manager.Create(user10, "123456a@"); var adminUser = manager.FindByName("dhthao1"); manager.AddToRoles(adminUser.Id, new string[] { "Admin" }); }
                if (manager.Users.Count(x => x.UserName == "pmdung") == 0) { manager.Create(user11, "123456a@"); var adminUser = manager.FindByName("pmdung"); manager.AddToRoles(adminUser.Id, new string[] { "Admin" }); }
                if (manager.Users.Count(x => x.UserName == "hgiap") == 0) { manager.Create(user12, "123456a@"); var adminUser = manager.FindByName("hgiap"); manager.AddToRoles(adminUser.Id, new string[] { "Admin" }); }
                if (manager.Users.Count(x => x.UserName == "ntngoc") == 0) { manager.Create(user13, "123456a@"); var adminUser = manager.FindByName("ntngoc"); manager.AddToRoles(adminUser.Id, new string[] { "Admin" }); }
                if (manager.Users.Count(x => x.UserName == "bmthin") == 0) { manager.Create(user14, "123456a@"); var adminUser = manager.FindByName("bmthin"); manager.AddToRoles(adminUser.Id, new string[] { "Admin" }); }
                if (manager.Users.Count(x => x.UserName == "ntthuan") == 0) { manager.Create(user15, "123456a@"); var adminUser = manager.FindByName("ntthuan"); manager.AddToRoles(adminUser.Id, new string[] { "Admin" }); }
                if (manager.Users.Count(x => x.UserName == "nahoa") == 0) { manager.Create(user16, "123456a@"); var adminUser = manager.FindByName("nahoa"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "lttthuy") == 0) { manager.Create(user17, "123456a@"); var adminUser = manager.FindByName("lttthuy"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ptmngoc") == 0) { manager.Create(user18, "123456a@"); var adminUser = manager.FindByName("ptmngoc"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nmnguyet") == 0) { manager.Create(user19, "123456a@"); var adminUser = manager.FindByName("nmnguyet"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ddanh1") == 0) { manager.Create(user20, "123456a@"); var adminUser = manager.FindByName("ddanh1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "lvtuong") == 0) { manager.Create(user21, "123456a@"); var adminUser = manager.FindByName("lvtuong"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dtthang") == 0) { manager.Create(user22, "123456a@"); var adminUser = manager.FindByName("dtthang"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "hmy") == 0) { manager.Create(user23, "123456a@"); var adminUser = manager.FindByName("hmy"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ttsen") == 0) { manager.Create(user24, "123456a@"); var adminUser = manager.FindByName("ttsen"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nttrang") == 0) { manager.Create(user25, "123456a@"); var adminUser = manager.FindByName("nttrang"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ntttruc") == 0) { manager.Create(user26, "123456a@"); var adminUser = manager.FindByName("ntttruc"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dlchi") == 0) { manager.Create(user27, "123456a@"); var adminUser = manager.FindByName("dlchi"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "lhhoang") == 0) { manager.Create(user28, "123456a@"); var adminUser = manager.FindByName("lhhoang"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ntquynh") == 0) { manager.Create(user29, "123456a@"); var adminUser = manager.FindByName("ntquynh"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ttnhien") == 0) { manager.Create(user30, "123456a@"); var adminUser = manager.FindByName("ttnhien"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nthnhung") == 0) { manager.Create(user31, "123456a@"); var adminUser = manager.FindByName("nthnhung"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "tttvan") == 0) { manager.Create(user32, "123456a@"); var adminUser = manager.FindByName("tttvan"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nctson") == 0) { manager.Create(user33, "123456a@"); var adminUser = manager.FindByName("nctson"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nmlong") == 0) { manager.Create(user34, "123456a@"); var adminUser = manager.FindByName("nmlong"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dpanh") == 0) { manager.Create(user35, "123456a@"); var adminUser = manager.FindByName("dpanh"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "lahphuong") == 0) { manager.Create(user36, "123456a@"); var adminUser = manager.FindByName("lahphuong"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nahung") == 0) { manager.Create(user37, "123456a@"); var adminUser = manager.FindByName("nahung"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dmthong") == 0) { manager.Create(user38, "123456a@"); var adminUser = manager.FindByName("dmthong"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ndthien") == 0) { manager.Create(user39, "123456a@"); var adminUser = manager.FindByName("ndthien"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ndtai") == 0) { manager.Create(user40, "123456a@"); var adminUser = manager.FindByName("ndtai"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nhson2") == 0) { manager.Create(user41, "123456a@"); var adminUser = manager.FindByName("nhson2"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nvluan") == 0) { manager.Create(user42, "123456a@"); var adminUser = manager.FindByName("nvluan"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nhlong") == 0) { manager.Create(user43, "123456a@"); var adminUser = manager.FindByName("nhlong"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dthien1") == 0) { manager.Create(user44, "123456a@"); var adminUser = manager.FindByName("dthien1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nvan1") == 0) { manager.Create(user45, "123456a@"); var adminUser = manager.FindByName("nvan1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "bthoang") == 0) { manager.Create(user46, "123456a@"); var adminUser = manager.FindByName("bthoang"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "hnhieu") == 0) { manager.Create(user47, "123456a@"); var adminUser = manager.FindByName("hnhieu"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dbthan") == 0) { manager.Create(user48, "123456a@"); var adminUser = manager.FindByName("dbthan"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ptlam") == 0) { manager.Create(user49, "123456a@"); var adminUser = manager.FindByName("ptlam"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nddang") == 0) { manager.Create(user50, "123456a@"); var adminUser = manager.FindByName("nddang"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ntdung") == 0) { manager.Create(user51, "123456a@"); var adminUser = manager.FindByName("ntdung"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ltthoa1") == 0) { manager.Create(user52, "123456a@"); var adminUser = manager.FindByName("ltthoa1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nhphuong1") == 0) { manager.Create(user53, "123456a@"); var adminUser = manager.FindByName("nhphuong1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "txtung") == 0) { manager.Create(user54, "123456a@"); var adminUser = manager.FindByName("txtung"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nntrong") == 0) { manager.Create(user55, "123456a@"); var adminUser = manager.FindByName("nntrong"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "lvhuong") == 0) { manager.Create(user56, "123456a@"); var adminUser = manager.FindByName("lvhuong"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "lvlong") == 0) { manager.Create(user57, "123456a@"); var adminUser = manager.FindByName("lvlong"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nman") == 0) { manager.Create(user58, "123456a@"); var adminUser = manager.FindByName("nman"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nxthoi") == 0) { manager.Create(user59, "123456a@"); var adminUser = manager.FindByName("nxthoi"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "mctu") == 0) { manager.Create(user60, "123456a@"); var adminUser = manager.FindByName("mctu"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "pdquan") == 0) { manager.Create(user61, "123456a@"); var adminUser = manager.FindByName("pdquan"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "pvkhuong1") == 0) { manager.Create(user62, "123456a@"); var adminUser = manager.FindByName("pvkhuong1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nvanh") == 0) { manager.Create(user63, "123456a@"); var adminUser = manager.FindByName("nvanh"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "thdu") == 0) { manager.Create(user64, "123456a@"); var adminUser = manager.FindByName("thdu"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ntdung31") == 0) { manager.Create(user65, "123456a@"); var adminUser = manager.FindByName("ntdung31"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nvduy") == 0) { manager.Create(user66, "123456a@"); var adminUser = manager.FindByName("nvduy"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nxcanh") == 0) { manager.Create(user67, "123456a@"); var adminUser = manager.FindByName("nxcanh"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "bmhung") == 0) { manager.Create(user68, "123456a@"); var adminUser = manager.FindByName("bmhung"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "hmduc") == 0) { manager.Create(user69, "123456a@"); var adminUser = manager.FindByName("hmduc"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "mtanh") == 0) { manager.Create(user70, "123456a@"); var adminUser = manager.FindByName("mtanh"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nvnam") == 0) { manager.Create(user71, "123456a@"); var adminUser = manager.FindByName("nvnam"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ldquan") == 0) { manager.Create(user72, "123456a@"); var adminUser = manager.FindByName("ldquan"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ddviet") == 0) { manager.Create(user73, "123456a@"); var adminUser = manager.FindByName("ddviet"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nccong1") == 0) { manager.Create(user74, "123456a@"); var adminUser = manager.FindByName("nccong1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "vdthuan") == 0) { manager.Create(user75, "123456a@"); var adminUser = manager.FindByName("vdthuan"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nbhai") == 0) { manager.Create(user76, "123456a@"); var adminUser = manager.FindByName("nbhai"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ctpmai") == 0) { manager.Create(user77, "123456a@"); var adminUser = manager.FindByName("ctpmai"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "tvhai") == 0) { manager.Create(user78, "123456a@"); var adminUser = manager.FindByName("tvhai"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "tkien") == 0) { manager.Create(user79, "123456a@"); var adminUser = manager.FindByName("tkien"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nhbac") == 0) { manager.Create(user80, "123456a@"); var adminUser = manager.FindByName("nhbac"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nqminh") == 0) { manager.Create(user81, "123456a@"); var adminUser = manager.FindByName("nqminh"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ndphong") == 0) { manager.Create(user82, "123456a@"); var adminUser = manager.FindByName("ndphong"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "hvtrung") == 0) { manager.Create(user83, "123456a@"); var adminUser = manager.FindByName("hvtrung"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "hxduc") == 0) { manager.Create(user84, "123456a@"); var adminUser = manager.FindByName("hxduc"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ndhoang1") == 0) { manager.Create(user85, "123456a@"); var adminUser = manager.FindByName("ndhoang1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ndhung") == 0) { manager.Create(user86, "123456a@"); var adminUser = manager.FindByName("ndhung"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ntkien12") == 0) { manager.Create(user87, "123456a@"); var adminUser = manager.FindByName("ntkien12"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nhtoan1") == 0) { manager.Create(user88, "123456a@"); var adminUser = manager.FindByName("nhtoan1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dxtho") == 0) { manager.Create(user89, "123456a@"); var adminUser = manager.FindByName("dxtho"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "pghuu") == 0) { manager.Create(user90, "123456a@"); var adminUser = manager.FindByName("pghuu"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "haminh") == 0) { manager.Create(user91, "123456a@"); var adminUser = manager.FindByName("haminh"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nvcuong4") == 0) { manager.Create(user92, "123456a@"); var adminUser = manager.FindByName("nvcuong4"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dvbau") == 0) { manager.Create(user93, "123456a@"); var adminUser = manager.FindByName("dvbau"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dthai") == 0) { manager.Create(user94, "123456a@"); var adminUser = manager.FindByName("dthai"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "pbvu") == 0) { manager.Create(user95, "123456a@"); var adminUser = manager.FindByName("pbvu"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dtanh") == 0) { manager.Create(user96, "123456a@"); var adminUser = manager.FindByName("dtanh"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ntanh2") == 0) { manager.Create(user97, "123456a@"); var adminUser = manager.FindByName("ntanh2"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "tthuyen1") == 0) { manager.Create(user98, "123456a@"); var adminUser = manager.FindByName("tthuyen1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dttlinh") == 0) { manager.Create(user99, "123456a@"); var adminUser = manager.FindByName("dttlinh"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "tnluan") == 0) { manager.Create(user100, "123456a@"); var adminUser = manager.FindByName("tnluan"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ddhung") == 0) { manager.Create(user101, "123456a@"); var adminUser = manager.FindByName("ddhung"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nmhoang") == 0) { manager.Create(user102, "123456a@"); var adminUser = manager.FindByName("nmhoang"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ptdat") == 0) { manager.Create(user103, "123456a@"); var adminUser = manager.FindByName("ptdat"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nqanh") == 0) { manager.Create(user104, "123456a@"); var adminUser = manager.FindByName("nqanh"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dtluc") == 0) { manager.Create(user105, "123456a@"); var adminUser = manager.FindByName("dtluc"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "bvtung") == 0) { manager.Create(user106, "123456a@"); var adminUser = manager.FindByName("bvtung"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nmphuong1") == 0) { manager.Create(user107, "123456a@"); var adminUser = manager.FindByName("nmphuong1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "tvthinh") == 0) { manager.Create(user108, "123456a@"); var adminUser = manager.FindByName("tvthinh"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nhnam1") == 0) { manager.Create(user109, "123456a@"); var adminUser = manager.FindByName("nhnam1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nsha") == 0) { manager.Create(user110, "123456a@"); var adminUser = manager.FindByName("nsha"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nbtrung") == 0) { manager.Create(user111, "123456a@"); var adminUser = manager.FindByName("nbtrung"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "tvhung11") == 0) { manager.Create(user112, "123456a@"); var adminUser = manager.FindByName("tvhung11"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dvvinh") == 0) { manager.Create(user113, "123456a@"); var adminUser = manager.FindByName("dvvinh"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nvthanh") == 0) { manager.Create(user114, "123456a@"); var adminUser = manager.FindByName("nvthanh"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nvthong") == 0) { manager.Create(user115, "123456a@"); var adminUser = manager.FindByName("nvthong"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ntdung22") == 0) { manager.Create(user116, "123456a@"); var adminUser = manager.FindByName("ntdung22"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dddoi") == 0) { manager.Create(user117, "123456a@"); var adminUser = manager.FindByName("dddoi"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nmtien") == 0) { manager.Create(user118, "123456a@"); var adminUser = manager.FindByName("nmtien"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "phnhung") == 0) { manager.Create(user119, "123456a@"); var adminUser = manager.FindByName("phnhung"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nttrung1") == 0) { manager.Create(user120, "123456a@"); var adminUser = manager.FindByName("nttrung1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "pvoanh") == 0) { manager.Create(user121, "123456a@"); var adminUser = manager.FindByName("pvoanh"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ntpthanh") == 0) { manager.Create(user122, "123456a@"); var adminUser = manager.FindByName("ntpthanh"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dnthien") == 0) { manager.Create(user123, "123456a@"); var adminUser = manager.FindByName("dnthien"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "vndinh") == 0) { manager.Create(user124, "123456a@"); var adminUser = manager.FindByName("vndinh"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "laduc") == 0) { manager.Create(user125, "123456a@"); var adminUser = manager.FindByName("laduc"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ltson2") == 0) { manager.Create(user126, "123456a@"); var adminUser = manager.FindByName("ltson2"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ntgiang2") == 0) { manager.Create(user127, "123456a@"); var adminUser = manager.FindByName("ntgiang2"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ngphuong") == 0) { manager.Create(user128, "123456a@"); var adminUser = manager.FindByName("ngphuong"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nhviet") == 0) { manager.Create(user129, "123456a@"); var adminUser = manager.FindByName("nhviet"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "tvtien") == 0) { manager.Create(user130, "123456a@"); var adminUser = manager.FindByName("tvtien"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ltngoc") == 0) { manager.Create(user131, "123456a@"); var adminUser = manager.FindByName("ltngoc"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nmhanh") == 0) { manager.Create(user132, "123456a@"); var adminUser = manager.FindByName("nmhanh"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nmtuyen") == 0) { manager.Create(user133, "123456a@"); var adminUser = manager.FindByName("nmtuyen"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "vtathu") == 0) { manager.Create(user134, "123456a@"); var adminUser = manager.FindByName("vtathu"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "vdtrung") == 0) { manager.Create(user135, "123456a@"); var adminUser = manager.FindByName("vdtrung"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nvhai") == 0) { manager.Create(user136, "123456a@"); var adminUser = manager.FindByName("nvhai"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nvtrung1") == 0) { manager.Create(user137, "123456a@"); var adminUser = manager.FindByName("nvtrung1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "lhtuyen") == 0) { manager.Create(user138, "123456a@"); var adminUser = manager.FindByName("lhtuyen"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "bxtrung") == 0) { manager.Create(user139, "123456a@"); var adminUser = manager.FindByName("bxtrung"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "vvbinh") == 0) { manager.Create(user140, "123456a@"); var adminUser = manager.FindByName("vvbinh"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dtnghia") == 0) { manager.Create(user141, "123456a@"); var adminUser = manager.FindByName("dtnghia"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "hthoa1") == 0) { manager.Create(user142, "123456a@"); var adminUser = manager.FindByName("hthoa1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nthuyen") == 0) { manager.Create(user143, "123456a@"); var adminUser = manager.FindByName("nthuyen"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "lmlinh") == 0) { manager.Create(user144, "123456a@"); var adminUser = manager.FindByName("lmlinh"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ntthuy1") == 0) { manager.Create(user145, "123456a@"); var adminUser = manager.FindByName("ntthuy1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nhha") == 0) { manager.Create(user146, "123456a@"); var adminUser = manager.FindByName("nhha"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dtxtuoi") == 0) { manager.Create(user147, "123456a@"); var adminUser = manager.FindByName("dtxtuoi"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "thngoc") == 0) { manager.Create(user148, "123456a@"); var adminUser = manager.FindByName("thngoc"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dtpthanh") == 0) { manager.Create(user149, "123456a@"); var adminUser = manager.FindByName("dtpthanh"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "btthoa") == 0) { manager.Create(user150, "123456a@"); var adminUser = manager.FindByName("btthoa"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ntluyen1") == 0) { manager.Create(user151, "123456a@"); var adminUser = manager.FindByName("ntluyen1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ntcuc") == 0) { manager.Create(user152, "123456a@"); var adminUser = manager.FindByName("ntcuc"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "lthngoc") == 0) { manager.Create(user153, "123456a@"); var adminUser = manager.FindByName("lthngoc"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ttyen") == 0) { manager.Create(user154, "123456a@"); var adminUser = manager.FindByName("ttyen"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nthong") == 0) { manager.Create(user155, "123456a@"); var adminUser = manager.FindByName("nthong"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ptnu") == 0) { manager.Create(user156, "123456a@"); var adminUser = manager.FindByName("ptnu"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ttdung1") == 0) { manager.Create(user157, "123456a@"); var adminUser = manager.FindByName("ttdung1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "btxuan") == 0) { manager.Create(user158, "123456a@"); var adminUser = manager.FindByName("btxuan"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "tttien") == 0) { manager.Create(user159, "123456a@"); var adminUser = manager.FindByName("tttien"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ntbang") == 0) { manager.Create(user160, "123456a@"); var adminUser = manager.FindByName("ntbang"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dtphuong") == 0) { manager.Create(user161, "123456a@"); var adminUser = manager.FindByName("dtphuong"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "tththuong") == 0) { manager.Create(user162, "123456a@"); var adminUser = manager.FindByName("tththuong"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ltvdung") == 0) { manager.Create(user163, "123456a@"); var adminUser = manager.FindByName("ltvdung"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "htquynh") == 0) { manager.Create(user164, "123456a@"); var adminUser = manager.FindByName("htquynh"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ptthuy") == 0) { manager.Create(user165, "123456a@"); var adminUser = manager.FindByName("ptthuy"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ntmua") == 0) { manager.Create(user166, "123456a@"); var adminUser = manager.FindByName("ntmua"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nttgiang") == 0) { manager.Create(user167, "123456a@"); var adminUser = manager.FindByName("nttgiang"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ttthien") == 0) { manager.Create(user168, "123456a@"); var adminUser = manager.FindByName("ttthien"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "pthuyen1") == 0) { manager.Create(user169, "123456a@"); var adminUser = manager.FindByName("pthuyen1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "lthoa") == 0) { manager.Create(user170, "123456a@"); var adminUser = manager.FindByName("lthoa"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ptttrang") == 0) { manager.Create(user171, "123456a@"); var adminUser = manager.FindByName("ptttrang"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dnbao") == 0) { manager.Create(user172, "123456a@"); var adminUser = manager.FindByName("dnbao"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nntam") == 0) { manager.Create(user173, "123456a@"); var adminUser = manager.FindByName("nntam"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dkhuyen") == 0) { manager.Create(user174, "123456a@"); var adminUser = manager.FindByName("dkhuyen"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "tvhuan") == 0) { manager.Create(user175, "123456a@"); var adminUser = manager.FindByName("tvhuan"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "tvdung1") == 0) { manager.Create(user176, "123456a@"); var adminUser = manager.FindByName("tvdung1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ldkien") == 0) { manager.Create(user177, "123456a@"); var adminUser = manager.FindByName("ldkien"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nthoan") == 0) { manager.Create(user178, "123456a@"); var adminUser = manager.FindByName("nthoan"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nvbao") == 0) { manager.Create(user179, "123456a@"); var adminUser = manager.FindByName("nvbao"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "thnam") == 0) { manager.Create(user180, "123456a@"); var adminUser = manager.FindByName("thnam"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ddquy") == 0) { manager.Create(user181, "123456a@"); var adminUser = manager.FindByName("ddquy"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nvlong") == 0) { manager.Create(user182, "123456a@"); var adminUser = manager.FindByName("nvlong"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dtthuyen1") == 0) { manager.Create(user183, "123456a@"); var adminUser = manager.FindByName("dtthuyen1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "bvquyet") == 0) { manager.Create(user184, "123456a@"); var adminUser = manager.FindByName("bvquyet"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ldcuong1") == 0) { manager.Create(user185, "123456a@"); var adminUser = manager.FindByName("ldcuong1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dhgiang1") == 0) { manager.Create(user186, "123456a@"); var adminUser = manager.FindByName("dhgiang1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ntgiang1") == 0) { manager.Create(user187, "123456a@"); var adminUser = manager.FindByName("ntgiang1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nvkhoa") == 0) { manager.Create(user188, "123456a@"); var adminUser = manager.FindByName("nvkhoa"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "tdninh") == 0) { manager.Create(user189, "123456a@"); var adminUser = manager.FindByName("tdninh"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dvngoc") == 0) { manager.Create(user190, "123456a@"); var adminUser = manager.FindByName("dvngoc"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "vcthanh") == 0) { manager.Create(user191, "123456a@"); var adminUser = manager.FindByName("vcthanh"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nttrung2") == 0) { manager.Create(user192, "123456a@"); var adminUser = manager.FindByName("nttrung2"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ntvanh1") == 0) { manager.Create(user193, "123456a@"); var adminUser = manager.FindByName("ntvanh1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "vtson") == 0) { manager.Create(user194, "123456a@"); var adminUser = manager.FindByName("vtson"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "lvtrung") == 0) { manager.Create(user195, "123456a@"); var adminUser = manager.FindByName("lvtrung"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "phgiang") == 0) { manager.Create(user196, "123456a@"); var adminUser = manager.FindByName("phgiang"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "hhanh") == 0) { manager.Create(user197, "123456a@"); var adminUser = manager.FindByName("hhanh"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nhthanh1") == 0) { manager.Create(user198, "123456a@"); var adminUser = manager.FindByName("nhthanh1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "pvcanh") == 0) { manager.Create(user199, "123456a@"); var adminUser = manager.FindByName("pvcanh"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "vthung") == 0) { manager.Create(user200, "123456a@"); var adminUser = manager.FindByName("vthung"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ntquy") == 0) { manager.Create(user201, "123456a@"); var adminUser = manager.FindByName("ntquy"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "pxhoang") == 0) { manager.Create(user202, "123456a@"); var adminUser = manager.FindByName("pxhoang"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nhphong") == 0) { manager.Create(user203, "123456a@"); var adminUser = manager.FindByName("nhphong"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "lxlinh") == 0) { manager.Create(user204, "123456a@"); var adminUser = manager.FindByName("lxlinh"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "tmgiang") == 0) { manager.Create(user205, "123456a@"); var adminUser = manager.FindByName("tmgiang"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ddhieu") == 0) { manager.Create(user206, "123456a@"); var adminUser = manager.FindByName("ddhieu"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nvangoc") == 0) { manager.Create(user207, "123456a@"); var adminUser = manager.FindByName("nvangoc"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "lhdoan") == 0) { manager.Create(user208, "123456a@"); var adminUser = manager.FindByName("lhdoan"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nvquy1") == 0) { manager.Create(user209, "123456a@"); var adminUser = manager.FindByName("nvquy1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nchung") == 0) { manager.Create(user210, "123456a@"); var adminUser = manager.FindByName("nchung"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ldthien") == 0) { manager.Create(user211, "123456a@"); var adminUser = manager.FindByName("ldthien"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "lcnguyen") == 0) { manager.Create(user212, "123456a@"); var adminUser = manager.FindByName("lcnguyen"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nnduy") == 0) { manager.Create(user213, "123456a@"); var adminUser = manager.FindByName("nnduy"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nhanh3") == 0) { manager.Create(user214, "123456a@"); var adminUser = manager.FindByName("nhanh3"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "vdhao") == 0) { manager.Create(user215, "123456a@"); var adminUser = manager.FindByName("vdhao"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dqthang") == 0) { manager.Create(user216, "123456a@"); var adminUser = manager.FindByName("dqthang"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nvtruong1") == 0) { manager.Create(user217, "123456a@"); var adminUser = manager.FindByName("Nvtruong1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "tqhuy2") == 0) { manager.Create(user218, "123456a@"); var adminUser = manager.FindByName("tqhuy2"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dnduc") == 0) { manager.Create(user219, "123456a@"); var adminUser = manager.FindByName("dnduc"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nbson") == 0) { manager.Create(user220, "123456a@"); var adminUser = manager.FindByName("nbson"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "lvtung1") == 0) { manager.Create(user221, "123456a@"); var adminUser = manager.FindByName("lvtung1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nvphuc") == 0) { manager.Create(user222, "123456a@"); var adminUser = manager.FindByName("nvphuc"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nvthang") == 0) { manager.Create(user223, "123456a@"); var adminUser = manager.FindByName("nvthang"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dmtuong") == 0) { manager.Create(user224, "123456a@"); var adminUser = manager.FindByName("dmtuong"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nvcau") == 0) { manager.Create(user225, "123456a@"); var adminUser = manager.FindByName("nvcau"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "vxthien") == 0) { manager.Create(user226, "123456a@"); var adminUser = manager.FindByName("vxthien"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "hnquyen") == 0) { manager.Create(user227, "123456a@"); var adminUser = manager.FindByName("hnquyen"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ltdat1") == 0) { manager.Create(user228, "123456a@"); var adminUser = manager.FindByName("ltdat1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "Natuan1") == 0) { manager.Create(user229, "123456a@"); var adminUser = manager.FindByName("Natuan1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "vdhoan") == 0) { manager.Create(user230, "123456a@"); var adminUser = manager.FindByName("vdhoan"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ttson") == 0) { manager.Create(user231, "123456a@"); var adminUser = manager.FindByName("ttson"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nqthang") == 0) { manager.Create(user232, "123456a@"); var adminUser = manager.FindByName("nqthang"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "cvhieu") == 0) { manager.Create(user233, "123456a@"); var adminUser = manager.FindByName("cvhieu"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ntduy") == 0) { manager.Create(user234, "123456a@"); var adminUser = manager.FindByName("ntduy"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "htdung1") == 0) { manager.Create(user235, "123456a@"); var adminUser = manager.FindByName("htdung1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nhnam2") == 0) { manager.Create(user236, "123456a@"); var adminUser = manager.FindByName("nhnam2"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ntluan") == 0) { manager.Create(user237, "123456a@"); var adminUser = manager.FindByName("ntluan"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nvdong") == 0) { manager.Create(user238, "123456a@"); var adminUser = manager.FindByName("nvdong"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "vbhuatn") == 0) { manager.Create(user239, "123456a@"); var adminUser = manager.FindByName("vbhuan"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ptuyen") == 0) { manager.Create(user240, "123456a@"); var adminUser = manager.FindByName("ptuyen"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "cmquang") == 0) { manager.Create(user241, "123456a@"); var adminUser = manager.FindByName("cmquang"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "hnhung") == 0) { manager.Create(user242, "123456a@"); var adminUser = manager.FindByName("hnhung"); manager.AddToRoles(adminUser.Id, new string[] { "GroupLead" }); }
                if (manager.Users.Count(x => x.UserName == "mkumeda") == 0) { manager.Create(user243, "123456a@"); var adminUser = manager.FindByName("mkumeda"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nmha") == 0) { manager.Create(user244, "123456a@"); var adminUser = manager.FindByName("nmha"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nxhuy") == 0) { manager.Create(user245, "123456a@"); var adminUser = manager.FindByName("nxhuy"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "vnmanh") == 0) { manager.Create(user246, "123456a@"); var adminUser = manager.FindByName("vnmanh"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "htthoa") == 0) { manager.Create(user247, "123456a@"); var adminUser = manager.FindByName("htthoa"); manager.AddToRoles(adminUser.Id, new string[] { "GroupLead" }); }
                //đức thêm
                if (manager.Users.Count(x => x.UserName == "nhanh5") == 0) { manager.Create(user251, "123456a@"); var adminUser = manager.FindByName("nhanh5"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ttvanh1") == 0) { manager.Create(user252, "123456a@"); var adminUser = manager.FindByName("ttvanh1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "bndhieu") == 0) { manager.Create(user253, "123456a@"); var adminUser = manager.FindByName("bndhieu"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ltlthao") == 0) { manager.Create(user255, "123456a@"); var adminUser = manager.FindByName("ltlthao"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "vhtuan") == 0) { manager.Create(user256, "123456a@"); var adminUser = manager.FindByName("vhtuan"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "lksy") == 0) { manager.Create(user257, "123456a@"); var adminUser = manager.FindByName("lksy"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dthung") == 0) { manager.Create(user258, "123456a@"); var adminUser = manager.FindByName("dthung"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "lqkhang") == 0) { manager.Create(user260, "123456a@"); var adminUser = manager.FindByName("lqkhang"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nthung1") == 0) { manager.Create(user262, "123456a@"); var adminUser = manager.FindByName("nthung1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ldphu") == 0) { manager.Create(user263, "123456a@"); var adminUser = manager.FindByName("ldphu"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nthong1") == 0) { manager.Create(user264, "123456a@"); var adminUser = manager.FindByName("nthong1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ndkhanh1") == 0) { manager.Create(user265, "123456a@"); var adminUser = manager.FindByName("ndkhanh1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "pthuong") == 0) { manager.Create(user267, "123456a@"); var adminUser = manager.FindByName("pthuong"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dhyen") == 0) { manager.Create(user268, "123456a@"); var adminUser = manager.FindByName("dhyen"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "vhha") == 0) { manager.Create(user269, "123456a@"); var adminUser = manager.FindByName("vhha"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "nttha1") == 0) { manager.Create(user270, "123456a@"); var adminUser = manager.FindByName("nttha1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dttrung") == 0) { manager.Create(user271, "123456a@"); var adminUser = manager.FindByName("dttrung"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ththao") == 0) { manager.Create(user272, "123456a@"); var adminUser = manager.FindByName("ththao"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                //tuong them
                if (manager.Users.Count(x => x.UserName == "nttrang2") == 0) { manager.Create(user248, "123456a@"); var adminUser = manager.FindByName("nttrang2"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dnanh1") == 0) { manager.Create(user249, "123456a@"); var adminUser = manager.FindByName("dnanh1"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "ptnham") == 0) { manager.Create(user250, "123456a@"); var adminUser = manager.FindByName("ptnham"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "hha") == 0) { manager.Create(user254, "123456a@"); var adminUser = manager.FindByName("hha"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "byloan") == 0) { manager.Create(user261, "123456a@"); var adminUser = manager.FindByName("hha"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "tttthuy") == 0) { manager.Create(user259, "123456a@"); var adminUser = manager.FindByName("tttthuy"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }
                if (manager.Users.Count(x => x.UserName == "dthai2") == 0) { manager.Create(user266, "123456a@"); var adminUser = manager.FindByName("dthai2"); manager.AddToRoles(adminUser.Id, new string[] { "Member" }); }

                // Tùng thêm
                if (manager.Users.Count(x => x.UserName == "admin") == 0) { manager.Create(user999, "123456a@"); var adminUser = manager.FindByName("admin"); manager.AddToRoles(adminUser.Id, new string[] { "SuperAdmin" }); }

            }
        }

        private void CreateFingerMachineUser(TMSDbContext context)
        {
            var manager = new UserManager<AppUser>(new UserStore<AppUser>(new TMSDbContext()));
            //var userID1 = manager.FindByName("nqthang").Id;
            //var UserID2 = manager.FindByName("vxthien").Id;
            //var UserID3 = manager.FindByName("ltdat").Id;
            //var UserID4 = manager.FindByName("dmtuong").Id;
            //var userID5 = manager.FindByName("nvthang").Id;
            //var userID6 = manager.FindByName("tqhuy2").Id;
            //var userID7 = manager.FindByName("lvtung1").Id;
            //var userID8 = manager.FindByName("nvphuc").Id;
            //var userID9 = manager.FindByName("ptuyen").Id;
            //context.FingerMachineUsers.AddRange(new List<FingerMachineUser>()
            //    {
            //        new FingerMachineUser() { ID = "302", UserId = userID1},
            //        new FingerMachineUser() { ID = "251", UserId = UserID2 },
            //        new FingerMachineUser() { ID = "253", UserId = UserID3 },
            //        new FingerMachineUser() { ID = "249", UserId = UserID4 },
            //        new FingerMachineUser() { ID = "248", UserId = userID5 },
            //        new FingerMachineUser() { ID = "243", UserId = userID6 },
            //        new FingerMachineUser() { ID = "246", UserId = userID7 },
            //        new FingerMachineUser() { ID = "247", UserId =userID8},
            //        new FingerMachineUser() { ID = "344", UserId =userID9},
            //    });
            //context.SaveChanges();

            //#region Insert to FingerMachineUser table

            if (context.FingerMachineUsers.Count() == 0)
            {
                context.FingerMachineUsers.AddRange(new List<FingerMachineUser>()
            {
                    new FingerMachineUser() { ID = "246", UserId = manager.FindByName("lvtung1").Id},
                    new FingerMachineUser() { ID = "253", UserId = manager.FindByName("ltdat1").Id},
                    new FingerMachineUser() { ID = "243", UserId = manager.FindByName("tqhuy2").Id},
                    new FingerMachineUser() { ID = "248", UserId = manager.FindByName("nvthang").Id},
                    new FingerMachineUser() { ID = "251", UserId = manager.FindByName("vxthien").Id},
                    new FingerMachineUser() { ID = "249", UserId = manager.FindByName("dmtuong").Id},
                    new FingerMachineUser() { ID = "247", UserId = manager.FindByName("nvphuc").Id},
                    new FingerMachineUser() { ID = "302", UserId = manager.FindByName("nqthang").Id},
                    new FingerMachineUser() { ID = "345", UserId = manager.FindByName("ptuyen").Id},
                    new FingerMachineUser() { ID = "344", UserId = manager.FindByName("ptuyen").Id},


//new FingerMachineUser() { ID = "3", UserId = manager.FindByName("nmtien").Id},
//new FingerMachineUser() { ID = "4", UserId = manager.FindByName("nxcanh").Id},
//new FingerMachineUser() { ID = "5", UserId = manager.FindByName("dvvinh").Id},
//new FingerMachineUser() { ID = "6", UserId = manager.FindByName("ldquan").Id},
//new FingerMachineUser() { ID = "9", UserId = manager.FindByName("nqminh").Id},
//new FingerMachineUser() { ID = "13", UserId = manager.FindByName("hmduc").Id},
//new FingerMachineUser() { ID = "14", UserId = manager.FindByName("mtanh").Id},
//new FingerMachineUser() { ID = "21", UserId = manager.FindByName("bmthin").Id},
//new FingerMachineUser() { ID = "24", UserId = manager.FindByName("nvnam").Id},
//new FingerMachineUser() { ID = "25", UserId = manager.FindByName("tvhai").Id},
//new FingerMachineUser() { ID = "32", UserId = manager.FindByName("tkien").Id},
//new FingerMachineUser() { ID = "33", UserId = manager.FindByName("hvtrung").Id},
//new FingerMachineUser() { ID = "49", UserId = manager.FindByName("ttpthao2").Id},
//new FingerMachineUser() { ID = "50", UserId = manager.FindByName("ntanh11").Id},
//new FingerMachineUser() { ID = "52", UserId = manager.FindByName("pmdung").Id},
//new FingerMachineUser() { ID = "53", UserId = manager.FindByName("lvtuong").Id},
//new FingerMachineUser() { ID = "56", UserId = manager.FindByName("dtluc").Id},
//new FingerMachineUser() { ID = "59", UserId = manager.FindByName("nhson2").Id},
//new FingerMachineUser() { ID = "60", UserId = manager.FindByName("dddoi").Id},
//new FingerMachineUser() { ID = "61", UserId = manager.FindByName("ntquynh").Id},

//new FingerMachineUser() { ID = "72", UserId = manager.FindByName("ttsen").Id},
//new FingerMachineUser() { ID = "73", UserId = manager.FindByName("nttrang").Id},
//new FingerMachineUser() { ID = "74", UserId = manager.FindByName("dtthang").Id},
//new FingerMachineUser() { ID = "75", UserId = manager.FindByName("hmy").Id},
//new FingerMachineUser() { ID = "78", UserId = manager.FindByName("nccong1").Id},
//new FingerMachineUser() { ID = "81", UserId = manager.FindByName("vdthuan").Id},
//new FingerMachineUser() { ID = "82", UserId = manager.FindByName("nbhai").Id},
//new FingerMachineUser() { ID = "84", UserId = manager.FindByName("hnhieu").Id},
//new FingerMachineUser() { ID = "85", UserId = manager.FindByName("dthien1").Id},
//new FingerMachineUser() { ID = "86", UserId = manager.FindByName("ptlam").Id},
//new FingerMachineUser() { ID = "87", UserId = manager.FindByName("dbthan").Id},
//new FingerMachineUser() { ID = "88", UserId = manager.FindByName("nman").Id},
//new FingerMachineUser() { ID = "90", UserId = manager.FindByName("nvthong").Id},
//new FingerMachineUser() { ID = "93", UserId = manager.FindByName("ntngoc").Id},
//new FingerMachineUser() { ID = "97", UserId = manager.FindByName("nvanh").Id},
//new FingerMachineUser() { ID = "98", UserId = manager.FindByName("nvthanh").Id},

//new FingerMachineUser() { ID = "99", UserId = manager.FindByName("dtanh").Id},
//new FingerMachineUser() { ID = "100", UserId = manager.FindByName("phnhung").Id},
//new FingerMachineUser() { ID = "101", UserId = manager.FindByName("nhlong").Id},
////new FingerMachineUser() { ID = "102", UserId = manager.FindByName("lxthinh").Id},
//new FingerMachineUser() { ID = "30", UserId = manager.FindByName("ntnhan").Id},
//new FingerMachineUser() { ID = "104", UserId = manager.FindByName("dhthao1").Id},
//new FingerMachineUser() { ID = "105", UserId = manager.FindByName("nahoa").Id},
//new FingerMachineUser() { ID = "106", UserId = manager.FindByName("lttthuy").Id},
//new FingerMachineUser() { ID = "109", UserId = manager.FindByName("pvoanh").Id},
//new FingerMachineUser() { ID = "113", UserId = manager.FindByName("dtnghia").Id},
//new FingerMachineUser() { ID = "116", UserId = manager.FindByName("nvluan").Id},
//new FingerMachineUser() { ID = "118", UserId = manager.FindByName("txtung").Id},
//new FingerMachineUser() { ID = "120", UserId = manager.FindByName("lvlong").Id},
//new FingerMachineUser() { ID = "121", UserId = manager.FindByName("nhphuong1").Id},
//new FingerMachineUser() { ID = "122", UserId = manager.FindByName("nxthoi").Id},
//new FingerMachineUser() { ID = "123", UserId = manager.FindByName("ntluyen1").Id},
//new FingerMachineUser() { ID = "125", UserId = manager.FindByName("tttien").Id},
//new FingerMachineUser() { ID = "128", UserId = manager.FindByName("nntrong").Id},
//new FingerMachineUser() { ID = "129", UserId = manager.FindByName("nthong").Id},
//new FingerMachineUser() { ID = "130", UserId = manager.FindByName("lthngoc").Id},
//new FingerMachineUser() { ID = "131", UserId = manager.FindByName("bthoang").Id},
//new FingerMachineUser() { ID = "132", UserId = manager.FindByName("nttrung1").Id},
//new FingerMachineUser() { ID = "134", UserId = manager.FindByName("nhbac").Id},
//new FingerMachineUser() { ID = "136", UserId = manager.FindByName("nmlong").Id},
//new FingerMachineUser() { ID = "137", UserId = manager.FindByName("dtphuong").Id},
//new FingerMachineUser() { ID = "138", UserId = manager.FindByName("ntbang").Id},
//new FingerMachineUser() { ID = "139", UserId = manager.FindByName("btxuan").Id},
//new FingerMachineUser() { ID = "141", UserId = manager.FindByName("dnbao").Id},
//new FingerMachineUser() { ID = "142", UserId = manager.FindByName("thnam").Id},
//new FingerMachineUser() { ID = "143", UserId = manager.FindByName("ntgiang1").Id},
//new FingerMachineUser() { ID = "145", UserId = manager.FindByName("vcthanh").Id},
//new FingerMachineUser() { ID = "146", UserId = manager.FindByName("dvngoc").Id},
//new FingerMachineUser() { ID = "148", UserId = manager.FindByName("tvdung1").Id},
//new FingerMachineUser() { ID = "149", UserId = manager.FindByName("dhgiang1").Id},
//new FingerMachineUser() { ID = "150", UserId = manager.FindByName("nvlong").Id},
//new FingerMachineUser() { ID = "152", UserId = manager.FindByName("tvhuan").Id},
//new FingerMachineUser() { ID = "153", UserId = manager.FindByName("bvquyet").Id},
//new FingerMachineUser() { ID = "154", UserId = manager.FindByName("nthoan").Id},
//new FingerMachineUser() { ID = "156", UserId = manager.FindByName("ldkien").Id},
//new FingerMachineUser() { ID = "157", UserId = manager.FindByName("ddquy").Id},
//new FingerMachineUser() { ID = "158", UserId = manager.FindByName("nvkhoa").Id},
//new FingerMachineUser() { ID = "160", UserId = manager.FindByName("tdninh").Id},
//new FingerMachineUser() { ID = "161", UserId = manager.FindByName("nvbao").Id},
//new FingerMachineUser() { ID = "162", UserId = manager.FindByName("nttrung2").Id},
//new FingerMachineUser() { ID = "163", UserId = manager.FindByName("ndphong").Id},
//new FingerMachineUser() { ID = "165", UserId = manager.FindByName("nahung").Id},
//new FingerMachineUser() { ID = "166", UserId = manager.FindByName("ntanh2").Id},
//new FingerMachineUser() { ID = "167", UserId = manager.FindByName("dkhuyen").Id},
//new FingerMachineUser() { ID = "168", UserId = manager.FindByName("hthoa1").Id},
//new FingerMachineUser() { ID = "169", UserId = manager.FindByName("lahphuong").Id},
//new FingerMachineUser() { ID = "171", UserId = manager.FindByName("dnthien").Id},
//new FingerMachineUser() { ID = "172", UserId = manager.FindByName("ntpthanh").Id},
//new FingerMachineUser() { ID = "173", UserId = manager.FindByName("dpanh").Id},
//new FingerMachineUser() { ID = "174", UserId = manager.FindByName("ntttruc").Id},
//new FingerMachineUser() { ID = "178", UserId = manager.FindByName("ntkien12").Id},
//new FingerMachineUser() { ID = "179", UserId = manager.FindByName("dxtho").Id},
//new FingerMachineUser() { ID = "180", UserId = manager.FindByName("ndhoang1").Id},
//new FingerMachineUser() { ID = "181", UserId = manager.FindByName("ltson2").Id},
//new FingerMachineUser() { ID = "182", UserId = manager.FindByName("ndhung").Id},
//new FingerMachineUser() { ID = "184", UserId = manager.FindByName("hxduc").Id},
//new FingerMachineUser() { ID = "186", UserId = manager.FindByName("ddanh1").Id},
//new FingerMachineUser() { ID = "188", UserId = manager.FindByName("dtthuyen1").Id},
//new FingerMachineUser() { ID = "189", UserId = manager.FindByName("nthnhung").Id},
//new FingerMachineUser() { ID = "191", UserId = manager.FindByName("tththuong").Id},
//new FingerMachineUser() { ID = "193", UserId = manager.FindByName("nhviet").Id},
//new FingerMachineUser() { ID = "194", UserId = manager.FindByName("vndinh").Id},
//new FingerMachineUser() { ID = "195", UserId = manager.FindByName("laduc").Id},
//new FingerMachineUser() { ID = "196", UserId = manager.FindByName("ngphuong").Id},
//new FingerMachineUser() { ID = "197", UserId = manager.FindByName("ntgiang2").Id},
//new FingerMachineUser() { ID = "198", UserId = manager.FindByName("ptnanh").Id},
//new FingerMachineUser() { ID = "200", UserId = manager.FindByName("nttrang1").Id},
//new FingerMachineUser() { ID = "204", UserId = manager.FindByName("ltvdung").Id},
//new FingerMachineUser() { ID = "205", UserId = manager.FindByName("ptthuy").Id},
//new FingerMachineUser() { ID = "206", UserId = manager.FindByName("htquynh").Id},
//new FingerMachineUser() { ID = "207", UserId = manager.FindByName("ntmua").Id},
//new FingerMachineUser() { ID = "209", UserId = manager.FindByName("tthuyen1").Id},
//new FingerMachineUser() { ID = "210", UserId = manager.FindByName("ntdung").Id},
//new FingerMachineUser() { ID = "211", UserId = manager.FindByName("mctu").Id},
//new FingerMachineUser() { ID = "212", UserId = manager.FindByName("pvkhuong1").Id},
////new FingerMachineUser() { ID = "213", UserId = manager.FindByName("ndhoang1").Id},
//new FingerMachineUser() { ID = "214", UserId = manager.FindByName("lmlinh").Id},
//new FingerMachineUser() { ID = "215", UserId = manager.FindByName("ntdung22").Id},
//new FingerMachineUser() { ID = "217", UserId = manager.FindByName("ltngoc").Id},
//new FingerMachineUser() { ID = "219", UserId = manager.FindByName("ttthien").Id},
//new FingerMachineUser() { ID = "220", UserId = manager.FindByName("dttlinh").Id},
//new FingerMachineUser() { ID = "222", UserId = manager.FindByName("lvtrung").Id},
//new FingerMachineUser() { ID = "223", UserId = manager.FindByName("nttgiang").Id},
//new FingerMachineUser() { ID = "224", UserId = manager.FindByName("nthuyen").Id},
//new FingerMachineUser() { ID = "226", UserId = manager.FindByName("nmhanh").Id},
//new FingerMachineUser() { ID = "227", UserId = manager.FindByName("ndthien").Id},
//new FingerMachineUser() { ID = "228", UserId = manager.FindByName("nmtuyen").Id},
//new FingerMachineUser() { ID = "229", UserId = manager.FindByName("tnluan").Id},
//new FingerMachineUser() { ID = "230", UserId = manager.FindByName("pthuyen1").Id},
//new FingerMachineUser() { ID = "231", UserId = manager.FindByName("vthung").Id},
//new FingerMachineUser() { ID = "232", UserId = manager.FindByName("nhphong").Id},
//new FingerMachineUser() { ID = "233", UserId = manager.FindByName("lxlinh").Id},
//new FingerMachineUser() { ID = "234", UserId = manager.FindByName("ddhieu").Id},
//new FingerMachineUser() { ID = "235", UserId = manager.FindByName("nvquy1").Id},
//new FingerMachineUser() { ID = "236", UserId = manager.FindByName("nchung").Id},
//new FingerMachineUser() { ID = "237", UserId = manager.FindByName("ldthien").Id},
//new FingerMachineUser() { ID = "238", UserId = manager.FindByName("lcnguyen").Id},
//new FingerMachineUser() { ID = "239", UserId = manager.FindByName("nnduy").Id},
//new FingerMachineUser() { ID = "241", UserId = manager.FindByName("dqthang").Id},
//new FingerMachineUser() { ID = "242", UserId = manager.FindByName("nvtruong1").Id},

//new FingerMachineUser() { ID = "244", UserId = manager.FindByName("dnduc").Id},
//new FingerMachineUser() { ID = "245", UserId = manager.FindByName("nbson").Id},
//new FingerMachineUser() { ID = "250", UserId = manager.FindByName("nvcau").Id},
//new FingerMachineUser() { ID = "252", UserId = manager.FindByName("hnquyen").Id},
//new FingerMachineUser() { ID = "254", UserId = manager.FindByName("natuan1").Id},
//new FingerMachineUser() { ID = "255", UserId = manager.FindByName("ntquy").Id},
//new FingerMachineUser() { ID = "256", UserId = manager.FindByName("tmgiang").Id},
//new FingerMachineUser() { ID = "257", UserId = manager.FindByName("nvangoc").Id},
//new FingerMachineUser() { ID = "258", UserId = manager.FindByName("lhdoan").Id},
//new FingerMachineUser() { ID = "259", UserId = manager.FindByName("pxhoang").Id},
//new FingerMachineUser() { ID = "260", UserId = manager.FindByName("vdhao").Id},
//new FingerMachineUser() { ID = "261", UserId = manager.FindByName("nhanh3").Id},
//new FingerMachineUser() { ID = "262", UserId = manager.FindByName("vdhoan").Id},
//new FingerMachineUser() { ID = "264", UserId = manager.FindByName("nmhoang").Id},
//new FingerMachineUser() { ID = "265", UserId = manager.FindByName("ddhung").Id},
//new FingerMachineUser() { ID = "266", UserId = manager.FindByName("ntthuy1").Id},
//new FingerMachineUser() { ID = "268", UserId = manager.FindByName("ndtai").Id},
//new FingerMachineUser() { ID = "269", UserId = manager.FindByName("nnduy").Id},
//new FingerMachineUser() { ID = "270", UserId = manager.FindByName("nqanh").Id},
//new FingerMachineUser() { ID = "271", UserId = manager.FindByName("ptdat").Id},
//new FingerMachineUser() { ID = "272", UserId = manager.FindByName("tttvan").Id},
//new FingerMachineUser() { ID = "274", UserId = manager.FindByName("nhtoan1").Id},
//new FingerMachineUser() { ID = "275", UserId = manager.FindByName("nntam").Id},
//new FingerMachineUser() { ID = "276", UserId = manager.FindByName("vtson").Id},
//new FingerMachineUser() { ID = "278", UserId = manager.FindByName("vthnhung1").Id},
//new FingerMachineUser() { ID = "279", UserId = manager.FindByName("nhha").Id},
////new FingerMachineUser() { ID = "281", UserId = manager.FindByName("nttrang").Id},
////new FingerMachineUser() { ID = "284", UserId = manager.FindByName("lvhuong").Id},
//new FingerMachineUser() { ID = "283", UserId = manager.FindByName("ttson").Id},
//new FingerMachineUser() { ID = "284", UserId = manager.FindByName("lvhuong").Id},
//new FingerMachineUser() { ID = "285", UserId = manager.FindByName("lvhuong").Id},
//new FingerMachineUser() { ID = "147", UserId = manager.FindByName("ldcuong1").Id},
//new FingerMachineUser() { ID = "287", UserId = manager.FindByName("nmnguyet").Id},
//new FingerMachineUser() { ID = "288", UserId = manager.FindByName("ntdung31").Id},
//new FingerMachineUser() { ID = "289", UserId = manager.FindByName("dlchi").Id},
//new FingerMachineUser() { ID = "290", UserId = manager.FindByName("tvtien").Id},
//new FingerMachineUser() { ID = "294", UserId = manager.FindByName("haminh").Id},
//new FingerMachineUser() { ID = "295", UserId = manager.FindByName("vdtrung").Id},
//new FingerMachineUser() { ID = "296", UserId = manager.FindByName("phgiang").Id},
//new FingerMachineUser() { ID = "297", UserId = manager.FindByName("hhanh").Id},
//new FingerMachineUser() { ID = "299", UserId = manager.FindByName("nvhai").Id},
//new FingerMachineUser() { ID = "300", UserId = manager.FindByName("nmhanh").Id},

//new FingerMachineUser() { ID = "303", UserId = manager.FindByName("nvcuong4").Id},
//new FingerMachineUser() { ID = "308", UserId = manager.FindByName("lhtuyen").Id},
//new FingerMachineUser() { ID = "309", UserId = manager.FindByName("dmthong").Id},
//new FingerMachineUser() { ID = "293", UserId = manager.FindByName("nvtrung1").Id},
//new FingerMachineUser() { ID = "313", UserId = manager.FindByName("ntthuan").Id},
//new FingerMachineUser() { ID = "314", UserId = manager.FindByName("dvbau").Id},
//new FingerMachineUser() { ID = "315", UserId = manager.FindByName("thngoc").Id},
//new FingerMachineUser() { ID = "317", UserId = manager.FindByName("ntduy").Id},
//new FingerMachineUser() { ID = "318", UserId = manager.FindByName("vvbinh").Id},
//new FingerMachineUser() { ID = "319", UserId = manager.FindByName("vtathu").Id},
//new FingerMachineUser() { ID = "320", UserId = manager.FindByName("ptttrang").Id},
//new FingerMachineUser() { ID = "322", UserId = manager.FindByName("nntam").Id},
//new FingerMachineUser() { ID = "323", UserId = manager.FindByName("nhnam2").Id},
//new FingerMachineUser() { ID = "324", UserId = manager.FindByName("bxtrung").Id},
//new FingerMachineUser() { ID = "325", UserId = manager.FindByName("ltha").Id},
//new FingerMachineUser() { ID = "326", UserId = manager.FindByName("ntluan").Id},
//new FingerMachineUser() { ID = "327", UserId = manager.FindByName("dtxtuoi").Id},
//new FingerMachineUser() { ID = "329", UserId = manager.FindByName("lhhoang").Id},
//new FingerMachineUser() { ID = "330", UserId = manager.FindByName("nhthanh1").Id},
//new FingerMachineUser() { ID = "331", UserId = manager.FindByName("txlam").Id},
//new FingerMachineUser() { ID = "332", UserId = manager.FindByName("nddang").Id},
////new FingerMachineUser() { ID = "333", UserId = manager.FindByName("lthoa").Id},
//new FingerMachineUser() { ID = "338", UserId = manager.FindByName("pvcanh").Id},
//new FingerMachineUser() { ID = "340", UserId = manager.FindByName("nvdong").Id},
//new FingerMachineUser() { ID = "341", UserId = manager.FindByName("lthoa").Id},
//new FingerMachineUser() { ID = "342", UserId = manager.FindByName("vbhuan").Id},
//new FingerMachineUser() { ID = "343", UserId = manager.FindByName("pbvu").Id},

//new FingerMachineUser() { ID = "316", UserId = manager.FindByName("htdung1").Id},
//new FingerMachineUser() { ID = "348", UserId = manager.FindByName("btthoa").Id},
//new FingerMachineUser() { ID = "349", UserId = manager.FindByName("dtpthanh").Id},
//new FingerMachineUser() { ID = "350", UserId = manager.FindByName("ntvanh1").Id},
//new FingerMachineUser() { ID = "388", UserId = manager.FindByName("cmquang").Id},
////đức thêm
//new FingerMachineUser() { ID = "352", UserId = manager.FindByName("nhanh5").Id},
//new FingerMachineUser() { ID = "358", UserId = manager.FindByName("ttvanh1").Id},
//new FingerMachineUser() { ID = "364", UserId = manager.FindByName("bndhieu").Id},
//new FingerMachineUser() { ID = "362", UserId = manager.FindByName("ltlthao").Id},
//new FingerMachineUser() { ID = "377", UserId = manager.FindByName("vhtuan").Id},
//new FingerMachineUser() { ID = "379", UserId = manager.FindByName("lksy").Id},
//new FingerMachineUser() { ID = "380", UserId = manager.FindByName("dthung").Id},
//new FingerMachineUser() { ID = "351", UserId = manager.FindByName("lqkhang").Id},
//new FingerMachineUser() { ID = "310", UserId = manager.FindByName("nddang").Id},
//new FingerMachineUser() { ID = "411", UserId = manager.FindByName("ndkhanh1").Id},
//new FingerMachineUser() { ID = "395", UserId = manager.FindByName("nqthang").Id},
//new FingerMachineUser() { ID = "353", UserId = manager.FindByName("pthuong").Id},
//new FingerMachineUser() { ID = "354", UserId = manager.FindByName("dhyen").Id},
//new FingerMachineUser() { ID = "401", UserId = manager.FindByName("vhha").Id},
//new FingerMachineUser() { ID = "383", UserId = manager.FindByName("nttha1").Id},
//new FingerMachineUser() { ID = "384", UserId = manager.FindByName("dttrung").Id},
//new FingerMachineUser() { ID = "387", UserId = manager.FindByName("ththao").Id},
//new FingerMachineUser() { ID = "389", UserId = manager.FindByName("cmquang").Id},
//new FingerMachineUser() { ID = "390", UserId = manager.FindByName("cmquang").Id},

////tuong them
//new FingerMachineUser() { ID = "385", UserId = manager.FindByName("nttrang2").Id},
//new FingerMachineUser() { ID = "386", UserId = manager.FindByName("dnanh1").Id},
//new FingerMachineUser() { ID = "410", UserId = manager.FindByName("ptnham").Id},
//new FingerMachineUser() { ID = "119", UserId = manager.FindByName("hha").Id},
//new FingerMachineUser() { ID = "408", UserId = manager.FindByName("byloan").Id},
//new FingerMachineUser() { ID = "371", UserId = manager.FindByName("tttthuy").Id},
//new FingerMachineUser() { ID = "110", UserId = manager.FindByName("nthnhung").Id},
            });

                //    #endregion Insert to FingerMachineUser table

                context.SaveChanges();
            }
        }

        private void CreatePermission(TMSDbContext context)
        {
            var roleManager = new RoleManager<AppRole>(new RoleStore<AppRole>(new TMSDbContext()));
            var adminID = roleManager.FindByName("Admin").Id;
            var memberID = roleManager.FindByName("Member").Id;
            var groupleadID = roleManager.FindByName("GroupLead").Id;
            var HRID = roleManager.FindByName("HR").Id;
            var SuperAdminID = roleManager.FindByName("SuperAdmin").Id;
            if (context.Permissions.Count() == 0)
            {
                context.Permissions.AddRange(new List<Permission>()
                {
                    new Permission { RoleId = adminID, FunctionId = "GROUP_LIST", CanRead = true, CanReadAll = false, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = true },
                    new Permission { RoleId = groupleadID, FunctionId = "GROUP_LIST", CanRead = true, CanReadAll = false, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = true },
                    new Permission { RoleId = memberID, FunctionId = "GROUP_LIST", CanRead = false, CanReadAll = false, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = true },
                    new Permission { RoleId = HRID, FunctionId = "GROUP_LIST", CanRead = false, CanReadAll = false, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = true },
                    new Permission { RoleId = SuperAdminID, FunctionId = "GROUP_LIST", CanRead = true, CanReadAll = false, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = true },


                    new Permission { RoleId = adminID, FunctionId = "USER", CanRead = true, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = true },
                    new Permission { RoleId = groupleadID, FunctionId = "USER", CanRead = false, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = true },
                    new Permission { RoleId = memberID, FunctionId = "USER", CanRead = false, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = true },
                    new Permission { RoleId = HRID, FunctionId = "USER", CanRead = true, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = true },
                    new Permission { RoleId = SuperAdminID, FunctionId = "USER", CanRead = true, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = true },


                    new Permission { RoleId = adminID, FunctionId = "OTREQUEST_LIST", CanRead = true, CanReadAll = false, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = groupleadID, FunctionId = "OTREQUEST_LIST", CanRead = true, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = false, CanDelete = false },
                    new Permission { RoleId = memberID, FunctionId = "OTREQUEST_LIST", CanRead = true, CanReadAll = false, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = HRID, FunctionId = "OTREQUEST_LIST", CanRead = true, CanReadAll = false, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = SuperAdminID, FunctionId = "OTREQUEST_LIST", CanRead = true, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = false, CanDelete = false },


                    new Permission { RoleId = adminID, FunctionId = "REQUEST_LIST", CanRead = true, CanReadAll = false, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = groupleadID, FunctionId = "REQUEST_LIST", CanRead = true, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = memberID, FunctionId = "REQUEST_LIST", CanRead = true, CanReadAll = false, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = HRID, FunctionId = "REQUEST_LIST", CanRead = true, CanReadAll = false, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = SuperAdminID, FunctionId = "REQUEST_LIST", CanRead = true, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },


                    new Permission { RoleId = adminID, FunctionId = "TIMESHEET_LIST", CanRead = true, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = memberID, FunctionId = "TIMESHEET_LIST", CanRead = true, CanReadAll = false, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = groupleadID, FunctionId = "TIMESHEET_LIST", CanRead = true, CanReadAll = false, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = HRID, FunctionId = "TIMESHEET_LIST", CanRead = true, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = SuperAdminID, FunctionId = "TIMESHEET_LIST", CanRead = true, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },


                    new Permission { RoleId = adminID, FunctionId = "ABNORMALCASE_LIST", CanRead = true, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = memberID, FunctionId = "ABNORMALCASE_LIST", CanRead = true, CanReadAll = false, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = groupleadID, FunctionId = "ABNORMALCASE_LIST", CanRead = true, CanReadAll = false, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = HRID, FunctionId = "ABNORMALCASE_LIST", CanRead = true, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = SuperAdminID, FunctionId = "ABNORMALCASE_LIST", CanRead = true, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },


                    new Permission { RoleId = adminID, FunctionId = "EXPLANATION_LIST", CanRead = true, CanReadAll = false, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = groupleadID, FunctionId = "EXPLANATION_LIST", CanRead = true, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = memberID, FunctionId = "EXPLANATION_LIST", CanRead = true, CanReadAll = false, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = HRID, FunctionId = "EXPLANATION_LIST", CanRead = true, CanReadAll = false, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = SuperAdminID, FunctionId = "EXPLANATION_LIST", CanRead = true, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },

                    new Permission { RoleId = adminID, FunctionId = "ENTITLEDAY_LIST", CanRead = true, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = groupleadID, FunctionId = "ENTITLEDAY_LIST", CanRead = true, CanReadAll = false, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = memberID, FunctionId = "ENTITLEDAY_LIST", CanRead = true, CanReadAll = false, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = HRID, FunctionId = "ENTITLEDAY_LIST", CanRead = true, CanReadAll = false, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = SuperAdminID, FunctionId = "ENTITLEDAY_LIST", CanRead = true, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },


                    new Permission { RoleId = adminID, FunctionId = "OT_LIST", CanRead = true, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = HRID, FunctionId = "OT_LIST", CanRead = true, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = groupleadID, FunctionId = "OT_LIST", CanRead = false, CanReadAll = false, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = memberID, FunctionId = "OT_LIST", CanRead = true, CanReadAll = false, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = SuperAdminID, FunctionId = "OT_LIST", CanRead = true, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },


                    new Permission { RoleId = adminID, FunctionId = "DELEGATION_LIST", CanRead = true, CanReadAll = false, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = HRID, FunctionId = "DELEGATION_LIST", CanRead = true, CanReadAll = false, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = groupleadID, FunctionId = "DELEGATION_LIST", CanRead = true, CanReadAll = false, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = memberID, FunctionId = "DELEGATION_LIST", CanRead = true, CanReadAll = false, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    //new Permission { RoleId = SuperAdminID, FunctionId = "DELEGATION_LIST", CanRead = true, CanReadAll = false, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },


                    new Permission { RoleId = adminID, FunctionId = "DELEGATION_REQUEST_MANAGEMENT", CanRead = false, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = HRID, FunctionId = "DELEGATION_REQUEST_MANAGEMENT", CanRead = false, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = groupleadID, FunctionId = "DELEGATION_REQUEST_MANAGEMENT", CanRead = true, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = memberID, FunctionId = "DELEGATION_REQUEST_MANAGEMENT", CanRead = false, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    //new Permission { RoleId = SuperAdminID, FunctionId = "DELEGATION_REQUEST_MANAGEMENT", CanRead = true, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },


                    new Permission { RoleId = adminID, FunctionId = "DELEGATION_EXPLANATION_REQUEST_MANAGEMENT", CanRead = false, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = HRID, FunctionId = "DELEGATION_EXPLANATION_REQUEST_MANAGEMENT", CanRead = false, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = groupleadID, FunctionId = "DELEGATION_EXPLANATION_REQUEST_MANAGEMENT", CanRead = true, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = memberID, FunctionId = "DELEGATION_EXPLANATION_REQUEST_MANAGEMENT", CanRead = false, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    //new Permission { RoleId = SuperAdminID, FunctionId = "DELEGATION_EXPLANATION_REQUEST_MANAGEMENT", CanRead = true, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },

                    new Permission { RoleId = adminID, FunctionId = "CONFIG_DELEGATION", CanRead = false, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = HRID, FunctionId = "CONFIG_DELEGATION", CanRead = false, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = groupleadID, FunctionId = "CONFIG_DELEGATION", CanRead = true, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },
                    new Permission { RoleId = memberID, FunctionId = "CONFIG_DELEGATION", CanRead = false, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = false },

                    new Permission { RoleId = adminID, FunctionId = "SEND_MAIL", CanRead = false, CanReadAll = false, CanUpdate = false, CanCancel = false, CanCreate = false, CanDelete = false },
                    new Permission { RoleId = HRID, FunctionId = "SEND_MAIL", CanRead = true, CanReadAll = false, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = true },
                    new Permission { RoleId = groupleadID, FunctionId = "SEND_MAIL", CanRead = false, CanReadAll = false, CanUpdate = false, CanCancel = false, CanCreate = false, CanDelete = false },
                    new Permission { RoleId = memberID, FunctionId = "SEND_MAIL", CanRead = false, CanReadAll = false, CanUpdate = false, CanCancel = false, CanCreate = false, CanDelete = false },
                    //new Permission { RoleId = SuperAdminID, FunctionId = "SEND_MAIL", CanRead = true, CanReadAll = false, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = true },


                    new Permission { RoleId = adminID, FunctionId = "TIME_DAY", CanRead = true, CanReadAll = false, CanUpdate = false, CanCancel = false, CanCreate = false, CanDelete = false },
                    new Permission { RoleId = HRID, FunctionId = "TIME_DAY", CanRead = false, CanReadAll = false, CanUpdate = false, CanCancel = false, CanCreate = false, CanDelete = false },
                    new Permission { RoleId = groupleadID, FunctionId = "TIME_DAY", CanRead = false, CanReadAll = false, CanUpdate = false, CanCancel = false, CanCreate = false, CanDelete = false },
                    new Permission { RoleId = memberID, FunctionId = "TIME_DAY", CanRead = false, CanReadAll = false, CanUpdate = false, CanCancel = false, CanCreate = false, CanDelete = false },
                    new Permission { RoleId = SuperAdminID, FunctionId = "TIME_DAY", CanRead = true, CanReadAll = false, CanUpdate = false, CanCancel = false, CanCreate = false, CanDelete = false },


                    new Permission { RoleId = adminID, FunctionId = "ENTITLEDAY_MANAGEMENT_LIST", CanRead = true, CanReadAll = false, CanUpdate = false, CanCancel = false, CanCreate = false, CanDelete = false },
                    new Permission { RoleId = HRID, FunctionId = "ENTITLEDAY_MANAGEMENT_LIST", CanRead = false, CanReadAll = false, CanUpdate = false, CanCancel = false, CanCreate = false, CanDelete = false },
                    new Permission { RoleId = groupleadID, FunctionId = "ENTITLEDAY_MANAGEMENT_LIST", CanRead = false, CanReadAll = false, CanUpdate = false, CanCancel = false, CanCreate = false, CanDelete = false },
                    new Permission { RoleId = memberID, FunctionId = "ENTITLEDAY_MANAGEMENT_LIST", CanRead = false, CanReadAll = false, CanUpdate = false, CanCancel = false, CanCreate = false, CanDelete = false },
                    new Permission { RoleId = SuperAdminID, FunctionId = "ENTITLEDAY_MANAGEMENT_LIST", CanRead = true, CanReadAll = false, CanUpdate = false, CanCancel = false, CanCreate = false, CanDelete = false },

                    new Permission { RoleId = adminID, FunctionId = "REPORT", CanRead = true, CanReadAll = false, CanUpdate = false, CanCancel = false, CanCreate = false, CanDelete = false },
                    new Permission { RoleId = SuperAdminID, FunctionId = "REPORT", CanRead = true, CanReadAll = false, CanUpdate = false, CanCancel = false, CanCreate = false, CanDelete = false },
                    // holiday 
                    
                    new Permission { RoleId = adminID, FunctionId = "HOLIDAY", CanRead = true, CanReadAll = true, CanUpdate = true, CanCancel = true, CanCreate = true, CanDelete = true },
                    new Permission { RoleId = HRID, FunctionId = "HOLIDAY", CanRead = true, CanReadAll = false, CanUpdate = false, CanCancel = false, CanCreate = false, CanDelete = false },
                    new Permission { RoleId = groupleadID, FunctionId = "HOLIDAY", CanRead = true, CanReadAll = false, CanUpdate = false, CanCancel = false, CanCreate = false, CanDelete = false },
                    new Permission { RoleId = memberID, FunctionId = "HOLIDAY", CanRead = true, CanReadAll = false, CanUpdate = false, CanCancel = false, CanCreate = false, CanDelete = false },
                    new Permission { RoleId = SuperAdminID, FunctionId = "HOLIDAY", CanRead = true, CanReadAll = false, CanUpdate = false, CanCancel = false, CanCreate = false, CanDelete = false },

                });
                context.SaveChanges();
            }
        }

        private void CreateStatusRequest(TMSDbContext context)
        {
            if (context.StatusRequests.Count() == 0)
            {
                context.StatusRequests.AddRange(new List<StatusRequest>()
                {
                    new StatusRequest() { Name = "Pending"},
                    new StatusRequest() { Name = "Rejected" },
                    new StatusRequest() { Name = "Cancelled"  },
                    new StatusRequest() { Name = "Approved"  },
                   new StatusRequest() { Name = "Delegated"  }
                });
                context.SaveChanges();
            }
        }

        private void CreateRequestReasonType(TMSDbContext context)
        {
            if (context.RequestReasonTypes.Count() == 0)
            {
                context.RequestReasonTypes.AddRange(new List<RequestReasonType>()
                {
                    new RequestReasonType() { Name = "Wedding",CreatedDate=DateTime.Now,Status=true},
                    new RequestReasonType() { Name = "Annual leave",CreatedDate=DateTime.Now,Status=true },
                    new RequestReasonType() { Name = "Maternity" ,CreatedDate=DateTime.Now,Status=true },
                    new RequestReasonType() { Name = "Holiday" ,CreatedDate=DateTime.Now,Status=true },
                    new RequestReasonType() { Name = "Bereavement" ,CreatedDate=DateTime.Now,Status=true },
                    new RequestReasonType() { Name = "No salary",CreatedDate=DateTime.Now,Status=true  }
                });
                context.SaveChanges();
            }
        }

        private void CreateEntitleDay(TMSDbContext context)
        {
            var manager = new UserManager<AppUser>(new UserStore<AppUser>(new TMSDbContext()));
            if (context.EntitleDays.Count() == 0)
            {
                context.EntitleDays.AddRange(new List<EntitleDay>()
                {
                    new EntitleDay() {HolidayType= "Authorized Leave",UnitType ="Day",MaxEntitleDay = 12,Description ="Description Authorized Leave",Status=true},
                    new EntitleDay() {HolidayType= "No Salary",UnitType ="Day",MaxEntitleDay = 365,Description ="Description No Salary",Status=false},
                    new EntitleDay() {HolidayType= "Maternity for female",UnitType ="Day/Period",MaxEntitleDay = 180,Description ="Description Maternity",Status=true},
                    new EntitleDay() {HolidayType= "Maternity for male",UnitType ="Day/Period",MaxEntitleDay = 7,Description ="Description Maternity",Status=true},
                    new EntitleDay() {HolidayType= "Wedding holiday",UnitType ="Day/Period",MaxEntitleDay = 3,Description ="Description Maternity",Status=true},
                    new EntitleDay() {HolidayType= "Attend funeral",UnitType ="Day/Period",MaxEntitleDay = 3,Description ="Description Maternity",Status=true},
                });
                context.SaveChanges();
            }
        }

        private void CreateRequesType(TMSDbContext context)
        {
            if (context.RequestTypes.Count() == 0)
            {
                context.RequestTypes.AddRange(new List<RequestType>()
                {
                    new RequestType() { Name = "Full-time Leave",CreatedDate=DateTime.Now,Status=true},
                    new RequestType() { Name = "Morning Leave",CreatedDate=DateTime.Now,Status=true  },
                    new RequestType() { Name = "Afternoon Leave",CreatedDate=DateTime.Now,Status=true },
                    new RequestType() { Name = "Late Coming",CreatedDate=DateTime.Now,Status=true  },
                    new RequestType() { Name = "Early Leaving",CreatedDate=DateTime.Now,Status=true  }
                });
                context.SaveChanges();
            }
        }


        private void CreateOTDateTypes(TMSDbContext context)
        {
            if (context.OTDateTypes.Count() == 0)
            {
                context.OTDateTypes.AddRange(new List<OTDateType>()
                {
                    new OTDateType() { Name = "Normal"},
                    new OTDateType() { Name = "Weekend"  },
                    new OTDateType() { Name = "Holiday" },
                });
                context.SaveChanges();
            }
        }

        private void CreateOTTimeTypes(TMSDbContext context)
        {
            if (context.OTTimeTypes.Count() == 0)
            {
                context.OTTimeTypes.AddRange(new List<OTTimeType>()
                {
                    new OTTimeType() { Name = "Normal"},
                    new OTTimeType() { Name = "Night"  },
                });
                context.SaveChanges();
            }
        }


        private void CreateTimeSheet(TMSDbContext context)
        {
            var manager = new UserManager<AppUser>(new UserStore<AppUser>(new TMSDbContext()));
            var userID1 = manager.FindByName("lvtung1").Id;
            var UserID2 = manager.FindByName("ltdat1").Id;
            var UserID3 = manager.FindByName("tqhuy2").Id;
            var UserID4 = manager.FindByName("nvthang").Id;
            if (context.TimeSheets.Count() == 0)
            {
                context.TimeSheets.AddRange(new List<TimeSheet>()
                {
                    new TimeSheet() { UserID = userID1, DayOfCheck = DateTime.Now, CheckIn = "8:30", CheckOut = "17:11", ComeLate = true, ComeBackSoon = true, Absent = null, NumOfWorkingDay = 1, MinusAllowance = "0%" },
                    new TimeSheet() { UserID = userID1, DayOfCheck = DateTime.Parse("2018-02-02"), CheckIn = null, CheckOut = null, ComeLate = false, ComeBackSoon = false, Absent = "V", NumOfWorkingDay = 0, MinusAllowance = "100%" },
                    new TimeSheet() { UserID = userID1, DayOfCheck = DateTime.Parse("2018-02-03"), CheckIn = "8:22", CheckOut = "17:55", ComeLate = true, ComeBackSoon = false, Absent = null, NumOfWorkingDay = 1, MinusAllowance = "0%" },
                    new TimeSheet() { UserID = userID1, DayOfCheck = DateTime.Parse("2018-02-04"), CheckIn = "8:13", CheckOut = "17:55", ComeLate = false, ComeBackSoon = false, Absent = null, NumOfWorkingDay = 1, MinusAllowance = "0%" },
                    new TimeSheet() { UserID = userID1, DayOfCheck = DateTime.Parse("2018-02-08"), CheckIn = "13:33", CheckOut = "17:59", ComeLate = true, ComeBackSoon = false, Absent = "VS", NumOfWorkingDay = 0.6, MinusAllowance = "40%" },
                    new TimeSheet() { UserID = userID1, DayOfCheck = DateTime.Parse("2018-02-09"), CheckIn = "8:11", CheckOut = "11:33", ComeLate = false, ComeBackSoon = true, Absent = "VC", NumOfWorkingDay = 0.6, MinusAllowance = "40%" },
                    new TimeSheet() { UserID = userID1, DayOfCheck = DateTime.Parse("2018-02-10"), CheckIn = "13:33", CheckOut = "17:59", ComeLate = true, ComeBackSoon = false, Absent = "VS", NumOfWorkingDay = 0.6, MinusAllowance = "40%" },
                    new TimeSheet() { UserID = userID1, DayOfCheck = DateTime.Parse("2018-01-01"), CheckIn = "8:11", CheckOut = "11:33", ComeLate = false, ComeBackSoon = true, Absent = "VC", NumOfWorkingDay = 1, MinusAllowance = "0%" },
                    new TimeSheet() { UserID=userID1 , DayOfCheck = DateTime.Parse("2018-01-02"), CheckIn="8:30" , CheckOut = "17:11" , ComeLate = true ,ComeBackSoon = false ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new TimeSheet() { UserID=userID1 , DayOfCheck = DateTime.Parse("2018-01-03"), CheckIn="8:20" , CheckOut = "17:20" , ComeLate = false ,ComeBackSoon = true ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new TimeSheet() { UserID=UserID2 , DayOfCheck = DateTime.Parse("2018-01-04"), CheckIn="8:22" , CheckOut = "17:55" , ComeLate = true ,ComeBackSoon = false ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new TimeSheet() { UserID=UserID2 , DayOfCheck = DateTime.Parse("2018-01-05"), CheckIn="8:32" , CheckOut = "17:22" , ComeLate = true ,ComeBackSoon = true ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new TimeSheet() { UserID=UserID2 , DayOfCheck = DateTime.Parse("2018-01-06"), CheckIn="8:33" , CheckOut = "17:59" , ComeLate = true ,ComeBackSoon = false ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new TimeSheet() { UserID=UserID2 , DayOfCheck = DateTime.Parse("2018-01-07"), CheckIn="8:11" , CheckOut = "17:54" , ComeLate = false ,ComeBackSoon = false ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new TimeSheet() { UserID=UserID2 , DayOfCheck = DateTime.Parse("2018-01-08"), CheckIn="8:11" , CheckOut = "17:54" , ComeLate = false ,ComeBackSoon = false ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new TimeSheet() { UserID=UserID2 , DayOfCheck = DateTime.Parse("2018-01-09"), CheckIn="8:00" , CheckOut = "17:54" , ComeLate = false ,ComeBackSoon = false ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new TimeSheet() { UserID=UserID2 , DayOfCheck = DateTime.Parse("2018-01-11"), CheckIn="8:00" , CheckOut = "17:54" , ComeLate = false ,ComeBackSoon = false ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new TimeSheet() { UserID=UserID2 , DayOfCheck = DateTime.Parse("2018-01-12"), CheckIn="8:00" , CheckOut = "17:54" , ComeLate = false ,ComeBackSoon = false ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new TimeSheet() { UserID=UserID3 , DayOfCheck = DateTime.Parse("2018-02-03"), CheckIn="8:00" , CheckOut = "17:54" , ComeLate = false ,ComeBackSoon = false ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new TimeSheet() { UserID=UserID3 , DayOfCheck = DateTime.Parse("2018-02-04"), CheckIn="8:00" , CheckOut = "17:54" , ComeLate = false ,ComeBackSoon = false ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new TimeSheet() { UserID=UserID3 , DayOfCheck = DateTime.Parse("2018-02-05"), CheckIn="8:00" , CheckOut = "17:54" , ComeLate = false ,ComeBackSoon = false ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new TimeSheet() { UserID=UserID3 , DayOfCheck = DateTime.Parse("2018-02-06"), CheckIn="8:00" , CheckOut = "17:54" , ComeLate = false ,ComeBackSoon = false ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new TimeSheet() { UserID=UserID3 , DayOfCheck = DateTime.Parse("2018-02-07"), CheckIn="8:00" , CheckOut = "17:54" , ComeLate = false ,ComeBackSoon = false ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new TimeSheet() { UserID=UserID3 , DayOfCheck = DateTime.Parse("2018-02-08"), CheckIn="8:00" , CheckOut = "17:54" , ComeLate = false ,ComeBackSoon = false ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new TimeSheet() { UserID=UserID4 , DayOfCheck = DateTime.Parse("2018-01-19"), CheckIn="8:00" , CheckOut = "17:54" , ComeLate = false ,ComeBackSoon = false ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new TimeSheet() { UserID=UserID4 , DayOfCheck = DateTime.Parse("2018-09-20"), CheckIn="8:00" , CheckOut = "17:54" , ComeLate = false ,ComeBackSoon = false ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new TimeSheet() { UserID=UserID4 , DayOfCheck = DateTime.Parse("2018-01-21"), CheckIn="8:00" , CheckOut = "17:54" , ComeLate = false ,ComeBackSoon = false ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new TimeSheet() { UserID=UserID4 , DayOfCheck = DateTime.Parse("2018-01-22"), CheckIn="8:00" , CheckOut = "17:54" , ComeLate = false ,ComeBackSoon = false ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new TimeSheet() { UserID=UserID4 , DayOfCheck = DateTime.Parse("2018-01-23"), CheckIn="8:00" , CheckOut = "17:54" , ComeLate = false ,ComeBackSoon = false ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new TimeSheet() { UserID=UserID4 , DayOfCheck = DateTime.Parse("2018-01-24"), CheckIn="8:00" , CheckOut = "17:54" , ComeLate = false ,ComeBackSoon = false ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new TimeSheet() { UserID=userID1 , DayOfCheck = DateTime.Parse("2018-01-25"), CheckIn="9:00" , CheckOut = "18:54" , ComeLate = true ,ComeBackSoon = false ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new TimeSheet() { UserID=userID1 , DayOfCheck = DateTime.Parse("2018-03-06"), CheckIn="8:00" , CheckOut = "14:54" , ComeLate = false ,ComeBackSoon = true ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new TimeSheet() { UserID=UserID3 , DayOfCheck = DateTime.Parse("2018-03-06"), CheckIn="8:00" , CheckOut = "14:54" , ComeLate = false ,ComeBackSoon = true ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new TimeSheet() { UserID=UserID4 , DayOfCheck = DateTime.Parse("2018-03-06"), CheckIn="8:00" , CheckOut = "14:54" , ComeLate = false ,ComeBackSoon = true ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                });
                context.SaveChanges();
            }
        }

        private void CreateFingerTimeSheet(TMSDbContext context)
        {
            IDbFactory dbFactory = new DbFactory(); ;
            var fingerMachineUserRepository = new FingerMachineUserRepository(dbFactory);
            var manager = new UserManager<AppUser>(new UserStore<AppUser>(new TMSDbContext()));
            var UserID1 = manager.FindByName("lvtung1").Id;
            var UserID2 = manager.FindByName("ltdat1").Id;
            var UserID3 = manager.FindByName("tqhuy2").Id;
            var UserID4 = manager.FindByName("nvthang").Id;
            var UserID5 = manager.FindByName("vxthien").Id;
            var UserID6 = manager.FindByName("dmtuong").Id;
            var UserID7 = manager.FindByName("nvphuc").Id;
            var UserID8 = manager.FindByName("nqthang").Id;
            var UserID9 = manager.FindByName("ptuyen").Id;
            var UserNo1 = fingerMachineUserRepository.GetMulti(x => x.UserId == UserID1).FirstOrDefault().ID;
            var UserNo2 = fingerMachineUserRepository.GetMulti(x => x.UserId == UserID2).FirstOrDefault().ID;
            var UserNo3 = fingerMachineUserRepository.GetMulti(x => x.UserId == UserID3).FirstOrDefault().ID;
            var UserNo4 = fingerMachineUserRepository.GetMulti(x => x.UserId == UserID4).FirstOrDefault().ID;
            var UserNo5 = fingerMachineUserRepository.GetMulti(x => x.UserId == UserID5).FirstOrDefault().ID;
            var UserNo6 = fingerMachineUserRepository.GetMulti(x => x.UserId == UserID6).FirstOrDefault().ID;
            var UserNo7 = fingerMachineUserRepository.GetMulti(x => x.UserId == UserID7).FirstOrDefault().ID;
            var UserNo8 = fingerMachineUserRepository.GetMulti(x => x.UserId == UserID8).FirstOrDefault().ID;
            var UserNo9 = fingerMachineUserRepository.GetMulti(x => x.UserId == UserID9).FirstOrDefault().ID;
            if (context.FingerTimeSheets.Count() == 0)
            {
                context.FingerTimeSheets.AddRange(new List<FingerTimeSheet>()
                {
                    new FingerTimeSheet() { UserNo = UserNo1, DayOfCheck = DateTime.Parse("2018-02-02"), CheckIn = "8:30", CheckOut = "17:11", Late = "0:10", LeaveEarly = "0:19", OTCheckIn = "17:45", OTCheckOut= "20:05", Absent = null, NumOfWorkingDay = 1, MinusAllowance = "40%" },
                    new FingerTimeSheet() { UserNo = UserNo1, DayOfCheck = DateTime.Parse("2018-02-03"), CheckIn = null, CheckOut = null, OTCheckIn = "18:00", OTCheckOut= "19:05", Absent = "V", NumOfWorkingDay = 0, MinusAllowance = "100%" },
                    new FingerTimeSheet() { UserNo = UserNo1, DayOfCheck = DateTime.Parse("2018-02-04"), CheckIn = "8:22", CheckOut = "17:55", Late = "0:02", OTCheckIn = "19:00", OTCheckOut= "20:05", Absent = null, NumOfWorkingDay = 1, MinusAllowance = "40%" },
                    new FingerTimeSheet() { UserNo = UserNo1, DayOfCheck = DateTime.Parse("2018-02-05"), CheckIn = "8:13", CheckOut = "17:55", OTCheckOut= "20:05", Absent = null, NumOfWorkingDay = 1, MinusAllowance = "0%" },
                    new FingerTimeSheet() { UserNo = UserNo1, DayOfCheck = DateTime.Parse("2018-02-06"), CheckIn = "13:33", CheckOut = "17:59", OTCheckIn = "17:45", OTCheckOut= "20:05", Absent = "VS", NumOfWorkingDay = 0.5, MinusAllowance = "0%" },
                    new FingerTimeSheet() { UserNo = UserNo1, DayOfCheck = DateTime.Parse("2018-02-07"), CheckIn = "8:11", CheckOut = "11:33", OTCheckIn = "20:00", OTCheckOut= "20:35", Absent = "VC", NumOfWorkingDay = 0.5, MinusAllowance = "0%" },
                    new FingerTimeSheet() { UserNo = UserNo1, DayOfCheck = DateTime.Parse("2018-02-09"), CheckIn = "13:33", CheckOut = "17:59", Late = "0:13", Absent = "VS", NumOfWorkingDay = 0.5, MinusAllowance = "0%" },
                    new FingerTimeSheet() { UserNo = UserNo1, DayOfCheck = DateTime.Parse("2018-01-01"), CheckIn = "8:11", CheckOut = "11:33", Absent = "VC", NumOfWorkingDay = 0.5, MinusAllowance = "0%" },
                    new FingerTimeSheet() { UserNo=UserNo1 , DayOfCheck = DateTime.Parse("2018-01-02"), CheckIn="8:30" , CheckOut = "17:11" , Late = "0:10" ,LeaveEarly = "0:19" ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new FingerTimeSheet() { UserNo=UserNo1 , DayOfCheck = DateTime.Parse("2018-01-03"), CheckIn="8:20" , CheckOut = "17:20" ,LeaveEarly = "0:10" ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new FingerTimeSheet() { UserNo=UserNo2 , DayOfCheck = DateTime.Parse("2018-01-04"), CheckIn="8:22" , CheckOut = "17:55" , Late = "0:10" ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new FingerTimeSheet() { UserNo=UserNo2 , DayOfCheck = DateTime.Parse("2018-01-05"), CheckIn="8:32" , CheckOut = "17:22" , Late = "0:12" ,LeaveEarly = "0:08" ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new FingerTimeSheet() { UserNo=UserNo2 , DayOfCheck = DateTime.Parse("2018-01-06"), CheckIn="8:33" , CheckOut = "17:59" , Late = "0:13"  ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new FingerTimeSheet() { UserNo=UserNo2 , DayOfCheck = DateTime.Parse("2018-01-07"), CheckIn="8:11" , CheckOut = "17:54"  ,Absent=null,NumOfWorkingDay=1 },
                    new FingerTimeSheet() { UserNo=UserNo2 , DayOfCheck = DateTime.Parse("2018-01-08"), CheckIn="8:11" , CheckOut = "17:54"  ,Absent=null,NumOfWorkingDay=1 },
                    new FingerTimeSheet() { UserNo=UserNo2 , DayOfCheck = DateTime.Parse("2018-01-09"), CheckIn="8:00" , CheckOut = "17:54" , OTCheckIn = "17:45", OTCheckOut= "20:05" ,Absent=null,NumOfWorkingDay=1 },
                    new FingerTimeSheet() { UserNo=UserNo2 , DayOfCheck = DateTime.Parse("2018-01-11"), CheckIn="8:00" , CheckOut = "17:54" ,Absent=null,NumOfWorkingDay=1},
                    new FingerTimeSheet() { UserNo=UserNo2 , DayOfCheck = DateTime.Parse("2018-01-12"), CheckIn="8:00" , CheckOut = "17:54" ,Absent=null,NumOfWorkingDay=1},
                    new FingerTimeSheet() { UserNo=UserNo3 , DayOfCheck = DateTime.Parse("2018-02-03"), CheckIn="8:00" , CheckOut = "17:54", OTCheckIn = "17:45" ,Absent=null,NumOfWorkingDay=1 },
                    new FingerTimeSheet() { UserNo=UserNo3 , DayOfCheck = DateTime.Parse("2018-02-04"), CheckIn="8:00" , CheckOut = "17:54",Absent=null,NumOfWorkingDay=1 },
                    new FingerTimeSheet() { UserNo=UserNo3 , DayOfCheck = DateTime.Parse("2018-02-05"), CheckIn="8:00" , CheckOut = "17:54",Absent=null,NumOfWorkingDay=1},
                    new FingerTimeSheet() { UserNo=UserNo3 , DayOfCheck = DateTime.Parse("2018-02-06"), CheckIn="8:00" , CheckOut = "17:54" ,Absent=null,NumOfWorkingDay=1 },
                    new FingerTimeSheet() { UserNo=UserNo3 , DayOfCheck = DateTime.Parse("2018-02-07"), CheckIn="8:00" , CheckOut = "17:54" , OTCheckIn = "17:41", OTCheckOut= "21:05",Absent=null,NumOfWorkingDay=1},
                    new FingerTimeSheet() { UserNo=UserNo3 , DayOfCheck = DateTime.Parse("2018-02-08"), CheckIn="8:00" , CheckOut = "17:54",Absent=null,NumOfWorkingDay=1 },
                    new FingerTimeSheet() { UserNo=UserNo4 , DayOfCheck = DateTime.Parse("2018-01-19"), CheckIn="8:00" , CheckOut = "17:54" ,Absent=null,NumOfWorkingDay=1},
                    new FingerTimeSheet() { UserNo=UserNo4 , DayOfCheck = DateTime.Parse("2018-09-20"), CheckIn="8:00" , CheckOut = "17:54" ,Absent=null,NumOfWorkingDay=1 },
                    new FingerTimeSheet() { UserNo=UserNo4 , DayOfCheck = DateTime.Parse("2018-01-21"), CheckIn="8:00" , CheckOut = "17:54" , OTCheckIn = "17:45", OTCheckOut= "21:55" ,Absent=null,NumOfWorkingDay=1},
                    new FingerTimeSheet() { UserNo=UserNo4 , DayOfCheck = DateTime.Parse("2018-01-22"), CheckIn="8:00" , CheckOut = "17:54", OTCheckIn = "17:45", Absent=null,NumOfWorkingDay=1},
                    new FingerTimeSheet() { UserNo=UserNo4 , DayOfCheck = DateTime.Parse("2018-01-23"), CheckIn="8:00" , CheckOut = "17:54" ,Absent=null,NumOfWorkingDay=1},
                    new FingerTimeSheet() { UserNo=UserNo4 , DayOfCheck = DateTime.Parse("2018-01-24"), CheckIn="8:00" , CheckOut = "17:54", OTCheckOut= "20:05" ,Absent=null,NumOfWorkingDay=1 },
                    new FingerTimeSheet() { UserNo=UserNo1 , DayOfCheck = DateTime.Parse("2018-01-25"), CheckIn="9:00" , CheckOut = "18:54" , Late = "1:00"  ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new FingerTimeSheet() { UserNo=UserNo1 , DayOfCheck = DateTime.Parse("2018-01-26"), CheckIn="8:00" , CheckOut = "14:54"  ,LeaveEarly = "2:36" ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new FingerTimeSheet() { UserNo=UserNo1 , DayOfCheck = DateTime.Parse("2018-01-26"), CheckIn="8:00" , CheckOut = "14:54"  ,LeaveEarly = "2:36" ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },

                    new FingerTimeSheet() { UserNo=UserNo1 , DayOfCheck = DateTime.Parse("2018-03-19"), CheckIn=null , CheckOut = null, OTCheckOut= "20:05" ,Absent="V",NumOfWorkingDay=0 },
                    new FingerTimeSheet() { UserNo=UserNo2 , DayOfCheck = DateTime.Parse("2018-03-19"), CheckIn="9:00" , CheckOut = "18:54" , Late = "0:40"  ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new FingerTimeSheet() { UserNo=UserNo3 , DayOfCheck = DateTime.Parse("2018-03-19"), CheckIn="8:00" , CheckOut = "16:54"  ,LeaveEarly = "00:36" ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new FingerTimeSheet() { UserNo=UserNo4 , DayOfCheck = DateTime.Parse("2018-03-19"), CheckIn="9:00" , CheckOut = "16:54"  ,Late="00:40", LeaveEarly = "00:36" ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new FingerTimeSheet() { UserNo=UserNo5 , DayOfCheck = DateTime.Parse("2018-03-19"), CheckIn="8:00" , CheckOut = "17:54"   ,Absent=null,NumOfWorkingDay=1},
                    new FingerTimeSheet() { UserNo=UserNo6 , DayOfCheck = DateTime.Parse("2018-03-19"), CheckIn="8:00" , CheckOut = "17:54"  , OTCheckOut="22:00" ,Absent=null,NumOfWorkingDay=1},
                    new FingerTimeSheet() { UserNo=UserNo7 , DayOfCheck = DateTime.Parse("2018-03-19"), CheckIn="8:00" , CheckOut = "17:54"  , OTCheckIn="20:00" ,Absent=null,NumOfWorkingDay=1},
                    new FingerTimeSheet() { UserNo=UserNo8 , DayOfCheck = DateTime.Parse("2018-03-19"), CheckIn="8:00" , CheckOut = "17:54"  ,Absent=null,NumOfWorkingDay=1},
                    new FingerTimeSheet() { UserNo=UserNo9 , DayOfCheck = DateTime.Parse("2018-03-19"), CheckIn="8:00" , CheckOut = "17:54"  ,Absent=null,NumOfWorkingDay=1},

                    new FingerTimeSheet() { UserNo=UserNo1 , DayOfCheck = DateTime.Parse("2018-03-20"), CheckIn="8:00" , CheckOut = "17:54", OTCheckOut= "20:05" ,Absent=null,NumOfWorkingDay=1, MinusAllowance="40%" },
                    new FingerTimeSheet() { UserNo=UserNo2 , DayOfCheck =DateTime.Parse("2018-03-20") , CheckIn="9:10" , CheckOut = "17:54" , Late = "1:00"  ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new FingerTimeSheet() { UserNo=UserNo3 , DayOfCheck =DateTime.Parse("2018-03-20") , CheckIn="8:00" , CheckOut = "16:54",LeaveEarly = "2:36" ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new FingerTimeSheet() { UserNo=UserNo4 , DayOfCheck = DateTime.Parse("2018-03-20"), CheckIn="8:20" , CheckOut = "14:54" ,LeaveEarly = "2:36" ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new FingerTimeSheet() { UserNo=UserNo6 , DayOfCheck =DateTime.Parse("2018-03-20") , CheckIn="9:10" , CheckOut = "17:54" , Late = "1:00"  ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },

                    new FingerTimeSheet() { UserNo=UserNo1 , DayOfCheck =DateTime.Parse("2018-03-21") , CheckIn="9:10" , CheckOut = "17:54" , Late = "1:00"  ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new FingerTimeSheet() { UserNo=UserNo2 , DayOfCheck =DateTime.Parse("2018-03-21") , CheckIn="9:10" , CheckOut = "17:54" , Late = "1:00"  ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new FingerTimeSheet() { UserNo=UserNo3 , DayOfCheck =DateTime.Parse("2018-03-21") , CheckIn="9:10" , CheckOut = "17:54" , Late = "1:00"  ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new FingerTimeSheet() { UserNo=UserNo4 , DayOfCheck =DateTime.Parse("2018-03-21") , CheckIn="9:10" , CheckOut = "17:54" , Late = "1:00"  ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new FingerTimeSheet() { UserNo=UserNo5 , DayOfCheck =DateTime.Parse("2018-03-21") , CheckIn="9:10" , CheckOut = "17:54" , Late = "1:00"  ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new FingerTimeSheet() { UserNo=UserNo6 , DayOfCheck =DateTime.Parse("2018-03-21") , CheckIn="9:10" , CheckOut = "17:54" , Late = "1:00"  ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new FingerTimeSheet() { UserNo=UserNo7 , DayOfCheck =DateTime.Parse("2018-03-21") , CheckIn="9:10" , CheckOut = "17:54" , Late = "1:00"  ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new FingerTimeSheet() { UserNo=UserNo8 , DayOfCheck =DateTime.Parse("2018-03-21") , CheckIn="9:10" , CheckOut = "17:54" , Late = "1:00"  ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },
                    new FingerTimeSheet() { UserNo=UserNo9 , DayOfCheck =DateTime.Parse("2018-03-21") , CheckIn="9:10" , CheckOut = "17:54" , Late = "1:00"  ,Absent=null,NumOfWorkingDay=1,MinusAllowance="40%" },

                    new FingerTimeSheet() { UserNo=UserNo3 , DayOfCheck = DateTime.Parse("2018-03-03"), CheckIn="8:00" , CheckOut = "17:54", OTCheckIn = "17:45" ,Absent=null,NumOfWorkingDay=1 },
                    new FingerTimeSheet() { UserNo=UserNo3 , DayOfCheck = DateTime.Parse("2018-03-04"), CheckIn="8:00" , CheckOut = "17:54",Absent=null,NumOfWorkingDay=1 },
                    new FingerTimeSheet() { UserNo=UserNo3 , DayOfCheck = DateTime.Parse("2018-03-05"), CheckIn="8:00" , CheckOut = "17:54",Absent=null,NumOfWorkingDay=1},
                    new FingerTimeSheet() { UserNo=UserNo3 , DayOfCheck = DateTime.Parse("2018-03-09"), CheckIn="8:00" , CheckOut = "17:54" ,Absent=null,NumOfWorkingDay=1 },
                    new FingerTimeSheet() { UserNo=UserNo3 , DayOfCheck = DateTime.Parse("2018-03-07"), CheckIn="8:00" , CheckOut = "17:54" , OTCheckIn = "17:41", OTCheckOut= "21:05",Absent=null,NumOfWorkingDay=1},
                    new FingerTimeSheet() { UserNo=UserNo3 , DayOfCheck = DateTime.Parse("2018-03-08"), CheckIn="8:00" , CheckOut = "17:54",Absent=null,NumOfWorkingDay=1 },

                    new FingerTimeSheet() { UserNo=UserNo4 , DayOfCheck = DateTime.Parse("2018-03-03"), CheckIn="8:00" , CheckOut = "17:54", OTCheckIn = "17:45" ,Absent=null,NumOfWorkingDay=1 },
                    new FingerTimeSheet() { UserNo=UserNo4 , DayOfCheck = DateTime.Parse("2018-03-04"), CheckIn="8:00" , CheckOut = "17:54",Absent=null,NumOfWorkingDay=1 },
                    new FingerTimeSheet() { UserNo=UserNo4 , DayOfCheck = DateTime.Parse("2018-03-05"), CheckIn="8:00" , CheckOut = "17:54",Absent=null,NumOfWorkingDay=1},
                    new FingerTimeSheet() { UserNo=UserNo4 , DayOfCheck = DateTime.Parse("2018-03-09"), CheckIn="8:00" , CheckOut = "17:54" ,Absent=null,NumOfWorkingDay=1 },
                    new FingerTimeSheet() { UserNo=UserNo4 , DayOfCheck = DateTime.Parse("2018-03-07"), CheckIn="8:00" , CheckOut = "17:54" , OTCheckIn = "17:41", OTCheckOut= "21:05",Absent=null,NumOfWorkingDay=1},
                    new FingerTimeSheet() { UserNo=UserNo4 , DayOfCheck = DateTime.Parse("2018-03-08"), CheckIn="8:00" , CheckOut = "17:54",Absent=null,NumOfWorkingDay=1 },

                    new FingerTimeSheet() { UserNo=UserNo2 , DayOfCheck = DateTime.Parse("2018-03-03"), CheckIn="8:00" , CheckOut = "17:54", OTCheckIn = "17:45" ,Absent=null,NumOfWorkingDay=1 },
                    new FingerTimeSheet() { UserNo=UserNo2 , DayOfCheck = DateTime.Parse("2018-03-04"), CheckIn="8:00" , CheckOut = "17:54",Absent=null,NumOfWorkingDay=1 },
                    new FingerTimeSheet() { UserNo=UserNo2 , DayOfCheck = DateTime.Parse("2018-03-05"), CheckIn="8:00" , CheckOut = "17:54",Absent=null,NumOfWorkingDay=1},
                    new FingerTimeSheet() { UserNo=UserNo2 , DayOfCheck = DateTime.Parse("2018-03-09"), CheckIn="8:00" , CheckOut = "17:54" ,Absent=null,NumOfWorkingDay=1 },
                    new FingerTimeSheet() { UserNo=UserNo2 , DayOfCheck = DateTime.Parse("2018-03-07"), CheckIn="8:00" , CheckOut = "17:54" , OTCheckIn = "17:41", OTCheckOut= "21:05",Absent=null,NumOfWorkingDay=1},
                    new FingerTimeSheet() { UserNo=UserNo2 , DayOfCheck = DateTime.Parse("2018-03-08"), CheckIn="8:00" , CheckOut = "17:54",Absent=null,NumOfWorkingDay=1 },
                });
                context.SaveChanges();
            }
        }

        public void CreateEntitDay_AppUser(TMSDbContext context)
        {
            var manager = new UserManager<AppUser>(new UserStore<AppUser>(new TMSDbContext()));
            var allUser = manager.Users;
            foreach (var item in allUser)
            {

                context.ConfigDelegations.Add(
                new ConfigDelegation()
                {
                    UserId = item.Id,
                });
            }
            context.SaveChanges();
            if (context.EntitleDay_AppUsers.Count() == 0)
            {
                context.EntitleDay_AppUsers.AddRange(new List<Entitleday_AppUser>()
                {

                    new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("txlam").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ntngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ntnhan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ntanh11").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ptnanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nttrang1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ttpthao2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("vthnhung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ltha").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dhthao1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("pmdung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("hgiap").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ntthuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("bmthin").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nahoa").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("lttthuy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ptmngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nmnguyet").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ddanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("lvtuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dtthang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("hmy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ttsen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nttrang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ntttruc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dlchi").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("lhhoang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ntquynh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ttnhien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nthnhung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("tttvan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nctson").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nmlong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dpanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("lahphuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nahung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dmthong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ndthien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ndtai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nhson2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nvluan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nhlong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dthien1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nvan1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("bthoang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("hnhieu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dbthan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ptlam").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nddang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ntdung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ltthoa1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nhphuong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("txtung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nntrong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("lvhuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("lvlong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nman").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nxthoi").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("mctu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("pdquan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("pvkhuong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nvanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("thdu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ntdung31").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nvduy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nxcanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("bmhung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("hmduc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("mtanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nvnam").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ldquan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ddviet").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nccong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("vdthuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nbhai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ctpmai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("tvhai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("tkien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nhbac").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nqminh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ndphong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("hvtrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("hxduc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ndhoang1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ndhung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ntkien12").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nhtoan1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dxtho").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("pghuu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("haminh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nvcuong4").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dvbau").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dthai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("pbvu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dtanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ntanh2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("tthuyen1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dttlinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("tnluan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ddhung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nmhoang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ptdat").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nqanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dtluc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("bvtung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nmphuong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("tvthinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nhnam1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nsha").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nbtrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("tvhung11").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dvvinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nvthanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nvthong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ntdung22").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dddoi").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nmtien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("phnhung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nttrung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("pvoanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ntpthanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dnthien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("vndinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("laduc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ltson2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ntgiang2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ngphuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nhviet").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("tvtien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ltngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nmhanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nmtuyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("vtathu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("vdtrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nvhai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nvtrung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("lhtuyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("bxtrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("vvbinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dtnghia").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("hthoa1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nthuyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("lmlinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ntthuy1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nhha").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dtxtuoi").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("thngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dtpthanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("btthoa").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ntluyen1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ntcuc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("lthngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ttyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nthong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ptnu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ttdung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("btxuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("tttien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ntbang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dtphuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("tththuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ltvdung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("htquynh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ptthuy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ntmua").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nttgiang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ttthien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("pthuyen1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("lthoa").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ptttrang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dnbao").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nntam").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dkhuyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("tvhuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("tvdung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ldkien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nthoan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nvbao").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("thnam").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ddquy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nvlong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dtthuyen1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("bvquyet").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ldcuong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dhgiang1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ntgiang1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nvkhoa").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("tdninh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dvngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("vcthanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nttrung2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ntvanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("vtson").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("lvtrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("phgiang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("hhanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nhthanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("pvcanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("vthung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ntquy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("pxhoang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nhphong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("lxlinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("tmgiang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ddhieu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nvangoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("lhdoan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nvquy1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nchung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ldthien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("lcnguyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nnduy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nhanh3").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("vdhao").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dqthang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nvtruong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("tqhuy2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dnduc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nbson").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("lvtung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nvphuc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nvthang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dmtuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nvcau").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("vxthien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("hnquyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ltdat1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("natuan1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("vdhoan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ttson").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nqthang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("cvhieu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ntduy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("htdung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nhnam2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ntluan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nvdong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("vbhuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ptuyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
//duc them entitle day
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nttrang2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dnanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ptnham").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("hha").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("tttthuy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("byloan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dthai2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nhanh5").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ttvanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("bndhieu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ltlthao").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("vhtuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("lksy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dthung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("lqkhang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nthung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ldphu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nthong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ndkhanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("pthuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dhyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("vhha").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("nttha1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("dttrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 1,MaxEntitleDayAppUser= 12, UserId = manager.FindByName("ththao").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},



new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ntngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("txlam").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ntnhan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ntanh11").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ptnanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nttrang1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ttpthao2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("vthnhung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ltha").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dhthao1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("pmdung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("hgiap").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ntthuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("bmthin").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nahoa").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("lttthuy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ptmngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nmnguyet").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ddanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("lvtuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dtthang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("hmy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ttsen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nttrang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ntttruc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dlchi").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("lhhoang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ntquynh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ttnhien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nthnhung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("tttvan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nctson").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nmlong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dpanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("lahphuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nahung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dmthong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ndthien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ndtai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nhson2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nvluan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nhlong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dthien1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nvan1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("bthoang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("hnhieu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dbthan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ptlam").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nddang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ntdung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ltthoa1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nhphuong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("txtung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nntrong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("lvhuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("lvlong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nman").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nxthoi").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("mctu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("pdquan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("pvkhuong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nvanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("thdu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ntdung31").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nvduy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nxcanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("bmhung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("hmduc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("mtanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nvnam").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ldquan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ddviet").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nccong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("vdthuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nbhai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ctpmai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("tvhai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("tkien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nhbac").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nqminh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ndphong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("hvtrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("hxduc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ndhoang1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ndhung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ntkien12").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nhtoan1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dxtho").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("pghuu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("haminh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nvcuong4").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dvbau").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dthai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("pbvu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dtanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ntanh2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("tthuyen1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dttlinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("tnluan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ddhung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nmhoang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ptdat").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nqanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dtluc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("bvtung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nmphuong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("tvthinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nhnam1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nsha").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nbtrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("tvhung11").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dvvinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nvthanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nvthong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ntdung22").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dddoi").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nmtien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("phnhung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nttrung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("pvoanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ntpthanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dnthien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("vndinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("laduc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ltson2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ntgiang2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ngphuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nhviet").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("tvtien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ltngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nmhanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nmtuyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("vtathu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("vdtrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nvhai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nvtrung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("lhtuyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("bxtrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("vvbinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dtnghia").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("hthoa1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nthuyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("lmlinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ntthuy1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nhha").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dtxtuoi").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("thngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dtpthanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("btthoa").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ntluyen1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ntcuc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("lthngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ttyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nthong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ptnu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ttdung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("btxuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("tttien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ntbang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dtphuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("tththuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ltvdung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("htquynh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ptthuy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ntmua").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nttgiang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ttthien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("pthuyen1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("lthoa").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ptttrang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dnbao").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nntam").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dkhuyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("tvhuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("tvdung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ldkien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nthoan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nvbao").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("thnam").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ddquy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nvlong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dtthuyen1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("bvquyet").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ldcuong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dhgiang1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ntgiang1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nvkhoa").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("tdninh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dvngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("vcthanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nttrung2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ntvanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("vtson").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("lvtrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("phgiang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("hhanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nhthanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("pvcanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("vthung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ntquy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("pxhoang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nhphong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("lxlinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("tmgiang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ddhieu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nvangoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("lhdoan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nvquy1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nchung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ldthien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("lcnguyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nnduy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nhanh3").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("vdhao").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dqthang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nvtruong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("tqhuy2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dnduc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nbson").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("lvtung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nvphuc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nvthang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dmtuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nvcau").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("vxthien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("hnquyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ltdat1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("natuan1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("vdhoan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ttson").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nqthang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("cvhieu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ntduy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("htdung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nhnam2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ntluan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nvdong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("vbhuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ptuyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
//duc them
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nttrang2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dnanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ptnham").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("hha").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("tttthuy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("byloan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dthai2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nhanh5").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ttvanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("bndhieu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ltlthao").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("vhtuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("lksy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dthung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("lqkhang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nthung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ldphu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nthong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ndkhanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("pthuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dhyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("vhha").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("nttha1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("dttrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 2,MaxEntitleDayAppUser= 365, UserId = manager.FindByName("ththao").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},




new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("txlam").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ntngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ntnhan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ntanh11").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ptnanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nttrang1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ttpthao2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("vthnhung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ltha").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dhthao1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("pmdung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("hgiap").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ntthuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("bmthin").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nahoa").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("lttthuy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ptmngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nmnguyet").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ddanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("lvtuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dtthang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("hmy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ttsen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nttrang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ntttruc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dlchi").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("lhhoang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ntquynh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ttnhien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nthnhung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("tttvan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nctson").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nmlong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dpanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("lahphuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nahung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dmthong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ndthien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ndtai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nhson2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nvluan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nhlong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dthien1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nvan1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("bthoang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("hnhieu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dbthan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ptlam").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nddang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ntdung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ltthoa1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nhphuong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("txtung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nntrong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("lvhuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("lvlong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nman").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nxthoi").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("mctu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("pdquan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("pvkhuong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nvanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("thdu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ntdung31").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nvduy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nxcanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("bmhung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("hmduc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("mtanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nvnam").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ldquan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ddviet").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nccong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("vdthuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nbhai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ctpmai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("tvhai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("tkien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nhbac").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nqminh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ndphong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("hvtrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("hxduc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ndhoang1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ndhung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ntkien12").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nhtoan1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dxtho").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("pghuu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("haminh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nvcuong4").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dvbau").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dthai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("pbvu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dtanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ntanh2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("tthuyen1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dttlinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("tnluan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ddhung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nmhoang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ptdat").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nqanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dtluc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("bvtung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nmphuong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("tvthinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nhnam1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nsha").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nbtrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("tvhung11").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dvvinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nvthanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nvthong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ntdung22").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dddoi").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nmtien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("phnhung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nttrung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("pvoanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ntpthanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dnthien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("vndinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("laduc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ltson2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ntgiang2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ngphuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nhviet").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("tvtien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ltngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nmhanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nmtuyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("vtathu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("vdtrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nvhai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nvtrung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("lhtuyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("bxtrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("vvbinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dtnghia").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("hthoa1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nthuyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("lmlinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ntthuy1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nhha").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dtxtuoi").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("thngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dtpthanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("btthoa").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ntluyen1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ntcuc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("lthngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ttyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nthong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ptnu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ttdung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("btxuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("tttien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ntbang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dtphuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("tththuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ltvdung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("htquynh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ptthuy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ntmua").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nttgiang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ttthien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("pthuyen1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("lthoa").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ptttrang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dnbao").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nntam").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dkhuyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("tvhuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("tvdung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ldkien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nthoan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nvbao").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("thnam").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ddquy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nvlong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dtthuyen1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("bvquyet").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ldcuong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dhgiang1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ntgiang1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nvkhoa").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("tdninh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dvngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("vcthanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nttrung2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ntvanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("vtson").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("lvtrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("phgiang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("hhanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nhthanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("pvcanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("vthung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ntquy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("pxhoang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nhphong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("lxlinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("tmgiang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ddhieu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nvangoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("lhdoan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nvquy1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nchung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ldthien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("lcnguyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nnduy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nhanh3").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("vdhao").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dqthang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nvtruong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("tqhuy2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dnduc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nbson").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("lvtung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nvphuc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nvthang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dmtuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nvcau").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("vxthien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("hnquyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ltdat1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("natuan1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("vdhoan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ttson").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nqthang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("cvhieu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ntduy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("htdung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nhnam2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ntluan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nvdong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("vbhuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ptuyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nttrang2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dnanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ptnham").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("hha").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("tttthuy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("byloan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dthai2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nhanh5").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ttvanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("bndhieu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ltlthao").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("vhtuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("lksy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dthung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("lqkhang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nthung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ldphu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nthong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ndkhanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("pthuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dhyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("vhha").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("nttha1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("dttrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 3,MaxEntitleDayAppUser= 180, UserId = manager.FindByName("ththao").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},





new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("txlam").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ntngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ntnhan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ntanh11").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ptnanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nttrang1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ttpthao2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("vthnhung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ltha").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dhthao1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("pmdung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("hgiap").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ntthuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("bmthin").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nahoa").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("lttthuy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ptmngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nmnguyet").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ddanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("lvtuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dtthang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("hmy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ttsen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nttrang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ntttruc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dlchi").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("lhhoang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ntquynh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ttnhien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nthnhung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("tttvan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nctson").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nmlong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dpanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("lahphuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nahung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dmthong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ndthien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ndtai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nhson2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nvluan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nhlong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dthien1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nvan1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("bthoang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("hnhieu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dbthan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ptlam").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nddang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ntdung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ltthoa1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nhphuong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("txtung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nntrong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("lvhuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("lvlong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nman").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nxthoi").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("mctu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("pdquan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("pvkhuong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nvanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("thdu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ntdung31").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nvduy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nxcanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("bmhung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("hmduc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("mtanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nvnam").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ldquan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ddviet").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nccong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("vdthuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nbhai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ctpmai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("tvhai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("tkien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nhbac").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nqminh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ndphong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("hvtrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("hxduc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ndhoang1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ndhung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ntkien12").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nhtoan1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dxtho").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("pghuu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("haminh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nvcuong4").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dvbau").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dthai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("pbvu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dtanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ntanh2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("tthuyen1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dttlinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("tnluan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ddhung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nmhoang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ptdat").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nqanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dtluc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("bvtung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nmphuong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("tvthinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nhnam1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nsha").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nbtrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("tvhung11").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dvvinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nvthanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nvthong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ntdung22").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dddoi").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nmtien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("phnhung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nttrung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("pvoanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ntpthanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dnthien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("vndinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("laduc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ltson2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ntgiang2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ngphuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nhviet").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("tvtien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ltngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nmhanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nmtuyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("vtathu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("vdtrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nvhai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nvtrung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("lhtuyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("bxtrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("vvbinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dtnghia").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("hthoa1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nthuyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("lmlinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ntthuy1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nhha").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dtxtuoi").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("thngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dtpthanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("btthoa").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ntluyen1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ntcuc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("lthngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ttyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nthong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ptnu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ttdung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("btxuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("tttien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ntbang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dtphuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("tththuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ltvdung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("htquynh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ptthuy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ntmua").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nttgiang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ttthien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("pthuyen1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("lthoa").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ptttrang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dnbao").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nntam").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dkhuyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("tvhuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("tvdung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ldkien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nthoan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nvbao").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("thnam").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ddquy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nvlong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dtthuyen1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("bvquyet").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ldcuong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dhgiang1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ntgiang1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nvkhoa").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("tdninh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dvngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("vcthanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nttrung2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ntvanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("vtson").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("lvtrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("phgiang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("hhanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nhthanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("pvcanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("vthung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ntquy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("pxhoang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nhphong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("lxlinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("tmgiang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ddhieu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nvangoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("lhdoan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nvquy1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nchung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ldthien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("lcnguyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nnduy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nhanh3").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("vdhao").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dqthang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nvtruong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("tqhuy2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dnduc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nbson").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("lvtung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nvphuc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nvthang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dmtuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nvcau").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("vxthien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("hnquyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ltdat1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("natuan1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("vdhoan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ttson").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nqthang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("cvhieu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ntduy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("htdung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nhnam2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ntluan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nvdong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("vbhuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ptuyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nttrang2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dnanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ptnham").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("hha").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("tttthuy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("byloan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dthai2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nhanh5").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ttvanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("bndhieu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ltlthao").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("vhtuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("lksy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dthung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("lqkhang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nthung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ldphu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nthong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ndkhanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("pthuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dhyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("vhha").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("nttha1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("dttrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 4,MaxEntitleDayAppUser= 7, UserId = manager.FindByName("ththao").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},





new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("txlam").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntnhan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntanh11").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ptnanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nttrang1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ttpthao2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("vthnhung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ltha").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dhthao1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("pmdung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("hgiap").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntthuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("bmthin").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nahoa").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lttthuy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ptmngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nmnguyet").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ddanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lvtuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dtthang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("hmy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ttsen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nttrang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntttruc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dlchi").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lhhoang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntquynh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ttnhien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nthnhung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tttvan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nctson").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nmlong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dpanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lahphuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nahung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dmthong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ndthien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ndtai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nhson2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvluan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nhlong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dthien1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvan1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("bthoang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("hnhieu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dbthan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ptlam").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nddang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntdung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ltthoa1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nhphuong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("txtung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nntrong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lvhuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lvlong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nman").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nxthoi").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("mctu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("pdquan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("pvkhuong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("thdu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntdung31").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvduy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nxcanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("bmhung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("hmduc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("mtanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvnam").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ldquan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ddviet").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nccong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("vdthuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nbhai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ctpmai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tvhai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tkien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nhbac").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nqminh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ndphong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("hvtrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("hxduc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ndhoang1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ndhung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntkien12").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nhtoan1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dxtho").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("pghuu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("haminh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvcuong4").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dvbau").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dthai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("pbvu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dtanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntanh2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tthuyen1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dttlinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tnluan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ddhung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nmhoang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ptdat").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nqanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dtluc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("bvtung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nmphuong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tvthinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nhnam1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nsha").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nbtrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tvhung11").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dvvinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvthanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvthong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntdung22").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dddoi").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nmtien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("phnhung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nttrung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("pvoanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntpthanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dnthien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("vndinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("laduc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ltson2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntgiang2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ngphuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nhviet").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tvtien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ltngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nmhanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nmtuyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("vtathu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("vdtrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvhai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvtrung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lhtuyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("bxtrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("vvbinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dtnghia").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("hthoa1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nthuyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lmlinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntthuy1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nhha").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dtxtuoi").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("thngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dtpthanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("btthoa").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntluyen1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntcuc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lthngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ttyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nthong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ptnu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ttdung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("btxuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tttien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntbang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dtphuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tththuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ltvdung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("htquynh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ptthuy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntmua").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nttgiang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ttthien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("pthuyen1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lthoa").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ptttrang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dnbao").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nntam").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dkhuyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tvhuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tvdung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ldkien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nthoan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvbao").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("thnam").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ddquy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvlong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dtthuyen1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("bvquyet").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ldcuong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dhgiang1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntgiang1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvkhoa").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tdninh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dvngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("vcthanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nttrung2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntvanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("vtson").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lvtrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("phgiang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("hhanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nhthanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("pvcanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("vthung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntquy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("pxhoang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nhphong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lxlinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tmgiang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ddhieu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvangoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lhdoan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvquy1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nchung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ldthien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lcnguyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nnduy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nhanh3").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("vdhao").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dqthang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvtruong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tqhuy2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dnduc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nbson").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lvtung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvphuc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvthang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dmtuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvcau").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("vxthien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("hnquyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ltdat1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("natuan1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("vdhoan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ttson").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nqthang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("cvhieu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntduy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("htdung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nhnam2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntluan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvdong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("vbhuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ptuyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nttrang2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dnanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ptnham").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("hha").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tttthuy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("byloan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dthai2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nhanh5").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ttvanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("bndhieu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ltlthao").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("vhtuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lksy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dthung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lqkhang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nthung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ldphu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nthong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ndkhanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("pthuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dhyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("vhha").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nttha1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dttrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 5,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ththao").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},





new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("txlam").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntnhan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntanh11").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ptnanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nttrang1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ttpthao2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("vthnhung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ltha").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dhthao1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("pmdung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("hgiap").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntthuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("bmthin").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nahoa").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lttthuy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ptmngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nmnguyet").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ddanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lvtuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dtthang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("hmy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ttsen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nttrang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntttruc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dlchi").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lhhoang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntquynh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ttnhien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nthnhung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tttvan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nctson").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nmlong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dpanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lahphuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nahung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dmthong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ndthien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ndtai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nhson2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvluan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nhlong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dthien1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvan1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("bthoang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("hnhieu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dbthan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ptlam").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nddang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntdung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ltthoa1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nhphuong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("txtung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nntrong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lvhuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lvlong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nman").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nxthoi").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("mctu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("pdquan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("pvkhuong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("thdu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntdung31").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvduy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nxcanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("bmhung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("hmduc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("mtanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvnam").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ldquan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ddviet").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nccong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("vdthuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nbhai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ctpmai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tvhai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tkien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nhbac").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nqminh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ndphong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("hvtrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("hxduc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ndhoang1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ndhung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntkien12").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nhtoan1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dxtho").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("pghuu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("haminh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvcuong4").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dvbau").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dthai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("pbvu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dtanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntanh2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tthuyen1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dttlinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tnluan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ddhung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nmhoang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ptdat").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nqanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dtluc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("bvtung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nmphuong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tvthinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nhnam1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nsha").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nbtrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tvhung11").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dvvinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvthanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvthong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntdung22").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dddoi").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nmtien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("phnhung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nttrung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("pvoanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntpthanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dnthien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("vndinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("laduc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ltson2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntgiang2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ngphuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nhviet").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tvtien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ltngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nmhanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nmtuyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("vtathu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("vdtrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvhai").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvtrung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lhtuyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("bxtrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("vvbinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dtnghia").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("hthoa1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nthuyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lmlinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntthuy1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nhha").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dtxtuoi").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("thngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dtpthanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("btthoa").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntluyen1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntcuc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lthngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ttyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nthong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ptnu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ttdung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("btxuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tttien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntbang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dtphuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tththuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ltvdung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("htquynh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ptthuy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntmua").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nttgiang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ttthien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("pthuyen1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lthoa").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ptttrang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dnbao").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nntam").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dkhuyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tvhuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tvdung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ldkien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nthoan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvbao").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("thnam").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ddquy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvlong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dtthuyen1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("bvquyet").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ldcuong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dhgiang1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntgiang1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvkhoa").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tdninh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dvngoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("vcthanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nttrung2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntvanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("vtson").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lvtrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("phgiang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("hhanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nhthanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("pvcanh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("vthung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntquy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("pxhoang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nhphong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lxlinh").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tmgiang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ddhieu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvangoc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lhdoan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvquy1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nchung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ldthien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lcnguyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nnduy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nhanh3").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("vdhao").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dqthang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvtruong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tqhuy2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dnduc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nbson").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lvtung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvphuc").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvthang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dmtuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvcau").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("vxthien").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("hnquyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ltdat1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("natuan1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("vdhoan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ttson").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nqthang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("cvhieu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntduy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("htdung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nhnam2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ntluan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nvdong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("vbhuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ptuyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nttrang2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dnanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ptnham").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("hha").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("tttthuy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("byloan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dthai2").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nhanh5").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ttvanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("bndhieu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ltlthao").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("vhtuan").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lksy").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dthung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("lqkhang").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nthung1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ldphu").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nthong1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ndkhanh1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("pthuong").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dhyen").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("vhha").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("nttha1").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("dttrung").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},
new Entitleday_AppUser() {TemporaryMaxEntitleDay = 0,EntitleDayId = 6,MaxEntitleDayAppUser= 3, UserId = manager.FindByName("ththao").Id, NumberDayOff = 0,RemainDayOfBeforeYear=0,DayBreak = 0,AuthorizedLeaveBonus = 0},

                });
                context.SaveChanges();
            }
        }

        //private void CreateAbnormalCase(TMSDbContext context)
        //{
        //    var manager = new UserManager<AppUser>(new UserStore<AppUser>(new TMSDbContext()));
        //    var userID1 = manager.FindByName("ltdat").Id;
        //    var UserID2 = manager.FindByName("admin").Id;
        //    var UserID3 = manager.FindByName("nvthang").Id;

        //    if (context.AbnormalCases.Count() == 0)
        //    {
        //        context.AbnormalCases.AddRange(new List<AbnormalCase>()
        //        {
        //            new AbnormalCase() {TimeSheetID=1, CreatedDate=DateTime.Now,Status=true},
        //            new AbnormalCase() {TimeSheetID=2, CreatedDate=DateTime.Now,Status=true},
        //            new AbnormalCase() { TimeSheetID=3, CreatedDate=DateTime.Now,Status=true},
        //            new AbnormalCase() {TimeSheetID=4,CreatedDate=DateTime.Now,Status=true},
        //            new AbnormalCase() { TimeSheetID=5, CreatedDate=DateTime.Now,Status=true},
        //            new AbnormalCase() {TimeSheetID=6,CreatedDate=DateTime.Now,Status=true},
        //            new AbnormalCase() {TimeSheetID=7, CreatedDate=DateTime.Now,Status=true},
        //            new AbnormalCase() { TimeSheetID=8 ,CreatedDate=DateTime.Now,Status=true},

        //            new AbnormalCase() {TimeSheetID=26,CreatedDate=DateTime.Now,Status=true},
        //            new AbnormalCase() {TimeSheetID=27, CreatedDate=DateTime.Now,Status=true},
        //            new AbnormalCase() { TimeSheetID=28 ,CreatedDate=DateTime.Now,Status=true},
        //            new AbnormalCase() {TimeSheetID=29, CreatedDate=DateTime.Now,Status=true},
        //            new AbnormalCase() { TimeSheetID=30 ,CreatedDate=DateTime.Now,Status=true},

        //            new AbnormalCase() {TimeSheetID=15, CreatedDate=DateTime.Now,Status=true},
        //            new AbnormalCase() { TimeSheetID=16 ,CreatedDate=DateTime.Now,Status=true},
        //            new AbnormalCase() {TimeSheetID=20, CreatedDate=DateTime.Now,Status=true},
        //            new AbnormalCase() { TimeSheetID=21 ,CreatedDate=DateTime.Now,Status=true},
        //            new AbnormalCase() { TimeSheetID=22 ,CreatedDate=DateTime.Now,Status=true},
        //            new AbnormalCase() { TimeSheetID=23 ,CreatedDate=DateTime.Now,Status=true},
        //            new AbnormalCase() { TimeSheetID=24 ,CreatedDate=DateTime.Now,Status=true},
        //            new AbnormalCase() { TimeSheetID=25 ,CreatedDate=DateTime.Now,Status=true}
        //        });
        //        context.SaveChanges();
        //    }
        //}

        private void CreateAbnormalTimeSheetTypes(TMSDbContext context)
        {
            if (context.AbnormalTimeSheetTypes.Count() == 0)
            {
                context.AbnormalTimeSheetTypes.AddRange(new List<AbnormalTimeSheetType>()
                {
                   new  AbnormalTimeSheetType() {Name = "DiMuon" ,Description="Late Coming" },
                    new  AbnormalTimeSheetType() {Name = "VeSom",Description="Early Leaving" },
                    new  AbnormalTimeSheetType() {Name = "VS",Description="Morning Leave" },
                    new  AbnormalTimeSheetType() {Name = "VC",Description="Afternoon Leave" },
                    new  AbnormalTimeSheetType() {Name = "V",Description="Full-time Leave" }
                });
                context.SaveChanges();
            }
        }

        private void CreateExplanationRequest(TMSDbContext context)
        {
            var manager = new UserManager<AppUser>(new UserStore<AppUser>(new TMSDbContext()));
            var userID1 = manager.FindByName("tqhuy2").Id;
            var UserID2 = manager.FindByName("dnduc").Id;
            var userID3 = manager.FindByName("ltdat1").Id;
            var userID4 = manager.FindByName("lvtung1").Id;
            var userID5 = manager.FindByName("nvthang").Id;
            var userID6 = manager.FindByName("dmtuong").Id;

            if (context.ExplanationRequests.Count() == 0)
            {
                context.ExplanationRequests.AddRange(new List<ExplanationRequest>()
                {
                    new ExplanationRequest() { Title = "Giải trình về sớm vì việc quan trọng" ,ReceiverId=userID6, TimeSheetId = 1, ReasonDetail = "Đi lấy bằng tốt nghiệp" , StatusRequestId = 1 ,CreatedDate =  DateTime.Now.AddDays(-1), CreatedBy = userID4, UpdatedDate = DateTime.Parse("2018-02-13"), Status = true  },
                    new ExplanationRequest() { Title = "Giải trình đi muộn hôm thứ 2" ,ReceiverId=userID6, TimeSheetId = 2,ReasonDetail = "Bị ốm đột xuất", StatusRequestId = 1 ,CreatedDate =  DateTime.Now.AddDays(-1), CreatedBy = userID4,UpdatedDate = DateTime.Parse("2018-02-12"), Status = true  },
                    new ExplanationRequest() { Title = "Giải trình về việc OT ko checkout" ,ReceiverId=userID6,DelegateId = userID5 ,TimeSheetId = 3, ReasonDetail ="Không nhớ lý do", StatusRequestId = 2 ,CreatedDate = DateTime.Parse("2018-02-10"),CreatedBy = userID4, UpdatedDate = DateTime.Parse("2018-02-12"), Status = true  },
                    new ExplanationRequest() { Title = "Giải trình cho việc nghỉ không phép" ,ReceiverId=userID6 , TimeSheetId = 5, ReasonDetail = "Trí nhớ mất tạm thời" , StatusRequestId = 1 ,CreatedDate =  DateTime.Now.AddDays(-1), CreatedBy = userID4,UpdatedDate = DateTime.Parse("2018-01-25"), Status = true  },
                    new ExplanationRequest() { Title = "Giải trình về sớm vì về quê cưới vợ" ,ReceiverId=userID6,UpdatedBy = userID4 , TimeSheetId = 6, ReasonDetail = "Đi lấy bằng tốt nghiệp" , StatusRequestId = 3 ,CreatedDate =  DateTime.Parse("2017-12-25"), CreatedBy = userID4,UpdatedDate = DateTime.Parse("2017-12-25"), Status = true  },
                    new ExplanationRequest() { Title = "Giải trình cho việc nghỉ hôm thứ 6" ,ReceiverId=userID6,UpdatedBy = userID6 , TimeSheetId = 7, ReasonDetail = "Vợ đẻ"  , StatusRequestId = 5 ,CreatedDate = DateTime.Parse("2018-02-12"), CreatedBy = userID4,UpdatedDate = DateTime.Parse("2018-02-14"), Status = true  },
                    new ExplanationRequest() { Title = "Giải trình đi muộn" ,ReceiverId=userID6,DelegateId = userID5 , TimeSheetId = 11,ReasonDetail = "Bị ốm đột xuất", StatusRequestId = 2,CreatedDate =  DateTime.Parse("2018-02-11"),CreatedBy = userID3, UpdatedDate = DateTime.Parse("2018-02-13"), Status = true  },
                    new ExplanationRequest() { Title = "Giải trình đi muộn" ,ReceiverId=userID6,DelegateId = userID5 , TimeSheetId = 12,ReasonDetail = "Bị ốm đột xuất", StatusRequestId = 2,CreatedDate =  DateTime.Parse("2018-02-27"),CreatedBy = userID3, UpdatedDate = DateTime.Parse("2018-02-27"), Status = true  },
                    new ExplanationRequest() { Title = "Giải trình cho việc nghỉ ngày 23/03/2018" ,ReceiverId=userID6, DelegateId = userID5, TimeSheetId = 13, ReasonDetail = "Đi ăn tết nguyên tiêu"  , StatusRequestId = 2 ,CreatedDate = DateTime.Parse("2018-02-26"), CreatedBy = userID3,UpdatedDate = DateTime.Parse("2018-02-14"), Status = true  },
                    new ExplanationRequest() { Title = "Giải trình nghỉ hôm thứ 4" ,ReceiverId=userID6, TimeSheetId = 31,ReasonDetail = "Có một chút vấn đề về giao thông", StatusRequestId = 1,CreatedDate =  DateTime.Now.AddDays(-1),CreatedBy = userID4, UpdatedDate = DateTime.Parse("2018-02-13"), Status = true  },
                    new ExplanationRequest() { Title = "Giải trình đến muộn hôm thứ 5" ,ReceiverId=userID6, TimeSheetId = 32,ReasonDetail = "Có một chút vấn đề về giao thông", StatusRequestId = 1,CreatedDate =  DateTime.Now.AddDays(-1),CreatedBy = userID4, UpdatedDate = DateTime.Parse("2018-02-13"), Status = true  },
                    new ExplanationRequest() { Title = "Giải trình nghỉ không phép ngày 12/03/2018" ,ReceiverId=userID6, TimeSheetId = 33,ReasonDetail = "Có một chút vấn đề về giao thông", StatusRequestId = 1,CreatedDate =  DateTime.Now.AddDays(-1),CreatedBy = userID4, UpdatedDate = DateTime.Parse("2018-02-13"), Status = true  },
                    new ExplanationRequest() { Title = "Giải trình về sớm hôm thứ 2" ,ReceiverId=userID6, TimeSheetId = 34,ReasonDetail = "Có một chút vấn đề về giao thông", StatusRequestId = 1,CreatedDate =  DateTime.Now.AddDays(-1),CreatedBy = userID4, UpdatedDate = DateTime.Parse("2018-02-13"), Status = true  }
                });
                context.SaveChanges();
            }
        }

        private void CreateAbnormalReason(TMSDbContext context)
        {
            if (context.AbnormalReasons.Count() == 0)
            {
                context.AbnormalReasons.AddRange(new List<AbnormalReason>()
                {
                    new AbnormalReason() { Name = "Unauthorized Late-Coming",CreatedDate=DateTime.Now,Status=true},
                    new AbnormalReason() { Name = "Unauthorized Early-Leaving",CreatedDate=DateTime.Now,Status=true},
                    new AbnormalReason() { Name = "Unused Authorized Early-Leaving",CreatedDate=DateTime.Now,Status=true},
                    new AbnormalReason() { Name = "Unused Authorized Late-Coming",CreatedDate=DateTime.Now,Status=true},
                    new AbnormalReason() { Name = "Unauthorized Leave",CreatedDate=DateTime.Now,Status=true},
                    new AbnormalReason() { Name = "Unused Authorized Leave",CreatedDate=DateTime.Now,Status=true},
                    new AbnormalReason() { Name = "OT Without Check-In",CreatedDate=DateTime.Now,Status=true},
                    new AbnormalReason() { Name = "OT Without Check-Out",CreatedDate=DateTime.Now,Status=true},
                    new AbnormalReason() { Name = "OT Without Check-In/Out",CreatedDate=DateTime.Now,Status=true}
                });
                context.SaveChanges();
            }
        }

        //private void CreateAbnormalCaseReason(TMSDbContext context)
        //{
        //    if (context.AbnormalCaseReasons.Count() == 0)
        //    {
        //        context.AbnormalCaseReasons.AddRange(new List<AbnormalCaseReason>()
        //        {
        //            new AbnormalCaseReason() { AbnormalId = 1, AbnormalReasonId = 1, Status=true},
        //            new AbnormalCaseReason() { AbnormalId = 1, AbnormalReasonId = 2, Status=true},
        //            new AbnormalCaseReason() { AbnormalId = 2, AbnormalReasonId = 3, Status=true},
        //            new AbnormalCaseReason() { AbnormalId = 4, AbnormalReasonId = 5, Status=true},
        //            new AbnormalCaseReason() { AbnormalId = 5, AbnormalReasonId = 5, Status=true},
        //            new AbnormalCaseReason() { AbnormalId = 6, AbnormalReasonId = 6, Status=true},
        //            new AbnormalCaseReason() { AbnormalId = 6, AbnormalReasonId = 7, Status=true},
        //            new AbnormalCaseReason() { AbnormalId = 3, AbnormalReasonId = 8 ,Status=true},
        //            new AbnormalCaseReason() { AbnormalId = 3, AbnormalReasonId = 9, Status=true},
        //            new AbnormalCaseReason() { AbnormalId = 9, AbnormalReasonId = 7, Status=true},
        //            new AbnormalCaseReason() { AbnormalId = 10, AbnormalReasonId = 8 ,Status=true},
        //            new AbnormalCaseReason() { AbnormalId = 7, AbnormalReasonId = 4, Status=true},
        //            new AbnormalCaseReason() { AbnormalId = 11, AbnormalReasonId = 6 ,Status=true},
        //            new AbnormalCaseReason() { AbnormalId = 12, AbnormalReasonId = 4, Status=true},
        //            new AbnormalCaseReason() { AbnormalId = 13, AbnormalReasonId = 1 ,Status=true},

        //            new AbnormalCaseReason() { AbnormalId = 14, AbnormalReasonId = 1, Status=true},
        //            new AbnormalCaseReason() { AbnormalId = 15, AbnormalReasonId = 2,Status=true},
        //            new AbnormalCaseReason() { AbnormalId = 16, AbnormalReasonId = 3, Status=true},
        //            new AbnormalCaseReason() { AbnormalId = 17, AbnormalReasonId = 4,Status=true},
        //            new AbnormalCaseReason() { AbnormalId = 18, AbnormalReasonId = 6,Status=true},
        //            new AbnormalCaseReason() { AbnormalId = 19, AbnormalReasonId = 6, Status=true},
        //            new AbnormalCaseReason() { AbnormalId = 20, AbnormalReasonId = 7,Status=true},
        //            new AbnormalCaseReason() { AbnormalId = 21, AbnormalReasonId = 8,Status=true},
        //            new AbnormalCaseReason() { AbnormalId = 21, AbnormalReasonId = 9,Status=true},
        //            new AbnormalCaseReason() { AbnormalId = 21, AbnormalReasonId = 1,Status=true},
        //            new AbnormalCaseReason() { AbnormalId = 20, AbnormalReasonId = 2,Status=true}
        //        });
        //        context.SaveChanges();
        //    }
        //}

        private void CreateTimeDay(TMSDbContext context)
        {
            if (context.TimeDays.Count() == 0)
            {
                context.TimeDays.AddRange(new List<TimeDay>()
                {
                    new TimeDay () {Workingday = "Monday",CheckIn="08:20",CheckOut= "17:30"},
                    new TimeDay () {Workingday = "Tuesday",CheckIn="08:20",CheckOut= "17:30" },
                    new TimeDay () {Workingday = "Wednesday",CheckIn="08:20",CheckOut= "17:30"},
                    new TimeDay () {Workingday = "Thursday",CheckIn="08:20",CheckOut= "17:30" },
                    new TimeDay () {Workingday = "Friday",CheckIn="08:20",CheckOut= "17:30"},
                });
            }
        }
    }
}
