using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using TMS.Model.Models;

namespace TMS.Data
{
    public class TMSDbContext : IdentityDbContext<AppUser>
    {
        public TMSDbContext() : base("TMSConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<Function> Functions { set; get; }
        public DbSet<Permission> Permissions { set; get; }
        public DbSet<AppRole> AppRoles { set; get; }
        public DbSet<IdentityUserRole> UserRoles { set; get; }
        public DbSet<Group> Groups { set; get; }
        public DbSet<OTRequest> OTRequests { set; get; }
        public DbSet<OTDateType> OTDateTypes { set; get; }
        public DbSet<OTTimeType> OTTimeTypes { set; get; }
        public DbSet<TimeSheet> TimeSheets { set; get; }
        public DbSet<ExplanationRequest> ExplanationRequests { set; get; }
        public DbSet<AbnormalCase> AbnormalCases { set; get; }
        public DbSet<AbnormalReason> AbnormalReasons { set; get; }
        public DbSet<AbnormalTimeSheetType> AbnormalTimeSheetTypes { set; get; }
        public DbSet<OTRequestUser> OTRequestUsers { get; set; }
        public DbSet<SystemConfig> SystemConfigs { set; get; }
        public DbSet<Error> Errors { set; get; }
        public DbSet<Announcement> Announcements { set; get; }
        public DbSet<AnnouncementUser> AnnouncementUsers { set; get; }
        public DbSet<RequestReasonType> RequestReasonTypes { set; get; }
        public DbSet<RequestType> RequestTypes { set; get; }
        public DbSet<StatusRequest> StatusRequests { set; get; }
        public DbSet<Request> Requests { set; get; }
        public DbSet<EntitleDay> EntitleDays { set; get; }
        public DbSet<TimeDay> TimeDays { set; get; }

        public DbSet<Entitleday_AppUser> EntitleDay_AppUsers { set; get; }
        public DbSet<FingerMachineUser> FingerMachineUsers { set; get; }
        public DbSet<FingerTimeSheetTmp> FingerTimeSheetTmps { set; get; }
        public DbSet<FingerTimeSheet> FingerTimeSheets { set; get; }
        public DbSet<CHECKINOUT> CHECKINOUT { set; get; }
        public DbSet<ChildcareLeave> ChildcareLeaves { set; get; }
        public DbSet<Report> Reports { set; get; }
        public DbSet<USERINFO> USERINFO { set; get; }
        public DbSet<ConfigDelegation> ConfigDelegations { set; get; }
        public DbSet<UserOnsite> UserOnsites { set; get; }
        public DbSet<Holiday> Holidays { set; get; }
        public DbSet<JobLog> JobLogs { set; get; }
        public static TMSDbContext Create()
        {
            return new TMSDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Entity<EntitleDay>().Property(p => p.MaxEntitleDay).HasColumnType("float");
            builder.Entity<Entitleday_AppUser>().Property(p => p.NumberDayOff ).HasColumnType("float");
            builder.Entity<Entitleday_AppUser>().Property(p => p.RemainDayOfBeforeYear).HasColumnType("float");
            builder.Entity<Entitleday_AppUser>().Property(p => p.DayBreak).HasColumnType("float");
            builder.Entity<IdentityRole>().HasKey<string>(r => r.Id).ToTable("AppRoles");
            builder.Entity<IdentityUserRole>().HasKey(i => new { i.UserId, i.RoleId }).ToTable("AppUserRoles");
            builder.Entity<IdentityUserLogin>().HasKey(i => i.UserId).ToTable("AppUserLogins");
            builder.Entity<IdentityUserClaim>().HasKey(i => i.UserId).ToTable("AppUserClaims");
            builder.Entity<TimeSheet>()
            .HasRequired(c => c.AppUser)
            .WithMany()
            .WillCascadeOnDelete(false);
            builder.Entity<ExplanationRequest>()
                .HasRequired(s => s.FingerTimeSheet)
                .WithMany()
                .WillCascadeOnDelete(false);
            builder.Entity<AbnormalCase>()
               .HasRequired(s => s.FingerTimeSheet)
               .WithMany()
               .WillCascadeOnDelete(false);
        }
    }
}