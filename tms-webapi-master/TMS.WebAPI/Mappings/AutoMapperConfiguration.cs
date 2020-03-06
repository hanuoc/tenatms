using AutoMapper;
using TMS.Common.ViewModels;
using TMS.Model.Models;
using TMS.Web.Models;
using TMS.Web.Models.AbnormalCase;
using TMS.Web.Models.Common;
using TMS.Web.Models.EntitleDay;
using TMS.Web.Models.EntitleDayManagement;
using TMS.Web.Models.Explanation;
using TMS.Web.Models.OTList;
using TMS.Web.Models.OTRequest;
using TMS.Web.Models.Request;
using TMS.Web.Models.TimeDay;
using TMS.Web.Models.TimeSheet;

namespace TMS.Web.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Group, GroupViewModel>();
                cfg.CreateMap<OTRequest, OTRequestViewModel>();
                cfg.CreateMap<OTDateType, OTDateTypeViewModel>();
                cfg.CreateMap<OTTimeType, OTTimeTypeViewModel>();
                cfg.CreateMap<OTRequestUser, OTRequestUserViewModel>();
                cfg.CreateMap<AppUser, AppUserViewModel>();
                cfg.CreateMap<AppRole, ApplicationRoleViewModel>();
                cfg.CreateMap<Function, FunctionViewModel>();
                cfg.CreateMap<Permission, PermissionViewModel>();
                cfg.CreateMap<Announcement, AnnouncementViewModel>();
                cfg.CreateMap<AnnouncementUser, AnnouncementUserViewModel>();
                cfg.CreateMap<Request, RequestViewModel>();
                cfg.CreateMap<EntitleDay, EntitleDayManagementViewModel>();
                cfg.CreateMap<RequestReasonType, RequestReasonTypeViewModel>();
                cfg.CreateMap<RequestType, RequestTypeViewModel>();
                cfg.CreateMap<StatusRequest, StatusRequestViewModel>();
                cfg.CreateMap<EntitleDay,EntitleDayViewModel>();
                cfg.CreateMap<ExplanationRequest, ExplanationRequestViewModel>();
                cfg.CreateMap<AbnormalReason, AbnormalReasonViewModel>();
                cfg.CreateMap<AbnormalCase, AbnormalCaseViewModel>();
                cfg.CreateMap<AbnormalCaseModel, AbnormalExcel>();
                cfg.CreateMap<TimeSheet, TimeSheetViewModel>();
                cfg.CreateMap<TimeDay, TimeDayViewModel>();
                cfg.CreateMap<ListOTModel, OTListViewModel>();
                cfg.CreateMap<EntitledayModel, EntitleDayViewModel>();
                cfg.CreateMap<FingerTimeSheetModel, FingerTimeSheetExcel>();
                cfg.CreateMap<EntitledayModel, Entitleday_AppUser>();
                cfg.CreateMap<ReportModel, ReportExcel>();
            });
        }
    }
}