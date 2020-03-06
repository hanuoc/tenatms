using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.Constants
{
    public class RoutesConstant
    {
        //OT Request
        public const string OTRequestApi = "api/otrequest";
        public const string OTRequestGetAll = "getall";
        public const string OTRequestGetAllGeneralAdmin = "getallGeneralAdmin";
        public const string OTTimeType = "getallOTTimeType";
        public const string OTDateType = "getallOTDateType";
        public const string OTRequestCreate = "add";
        public const string OTRequestUpdate = "updateotRequest";
        public const string OTRequestDetail = "detail/{id:int}";
        public const string OTRequestUser = "getOTRequestUser";
        public const string OTRequestChangeStatus = "changeStatus";
        public const string OTRequestChangeStatusMulti = "changeStatusMulti";
        public const string GetAllCreateByOTRequest = "getAllCreateByOTRequest";
        public const string GetOTRequestByTimeSheet = "getOTRequestByTimeSheet";
        public const string OTRequestChart = "otrequestchart";
        public const string OTRequestChartByUser = "otrequestchartbyuser";
        public const string otrequestByUser = "otrequestbyuser";
        //Request
        public const string RequestApi = "api/request";
        public const string RequestGetAllRequestByUser = "getallbyuser";
        public const string RequestGetAllRequestByUserSuperAdmin = "getallbyusersuperadmin";
        public const string GetAllRequestFilterByUser = "getallrequestfilterbyuser";
        public const string ChangeStatus = "changeStatus";
        public const string ChangeStatusDelegateRequest = "changeStatusDelegate";
        public const string ChangeStatusMulti = "changeStatusMulti";
        public const string ChangeStatusMultiDelegateDefault = "changeStatusMultiDelegateDefault";
        public const string Add = "add";
        public const string UpdateRequest = "updaterequest";
        public const string Detail = "detail/{id:int}";
        public const string GetListCreator = "getlistcreator";
        public const string requestChart = "requestChart";
        public const string requestChartByUser = "requestChartByUser";
        public const string requestByUser = "requestbyuser";
        public const string GetAllRequestByUser = "getallrequestbyuser";
        //TimeSheet
        public const string TimeSheetAPI = "api/timesheet";
        public const string TimeSheetGetAll = "getall";
        public const string GetListError = "getlisterror";
        public const string GetAllReport = "getallreport";
        public const string GetProgressValue = "get-progress-value";
        public const string RemoveProgressValue = "remove-progress-value";
        public const string CountUserReportEx = "count-user-reportex";
        //AbnormalRequest
        public const string AbnormalCaseApi = "api/abnormalcase";
        public const string GetAllAbnormalCaseByUser = "getallabnormalcasebyuser";
        public const string GetAllAbnormalReason = "getallabnormalreason";
        public const string abnormalByUser = "abnormalbyuser";
        //StatusRequest
        public const string StatusRequestApi = "api/statusrequest";
        public const string GetAll = "getall";
        //Request Type
        public const string RequestTypeApi = "api/requesttype";
        //Request reason Type
        public const string RequestReasonTypeApi = "api/requestreasontype";
        //Group
        public const string GroupApi = "api/group";
        public const string GetGroupById = "getgroupbyid";
        public const string GetAllGroup = "getallgroup";
        public const string AddGroup = "add";
        public const string StatusRequest = "api/statusrequest";
        public const string StatusRequestGetAll = "getall";
        public const string DeleteGroup = "delete";
        public const string DeleteMultiGroup = "delete-multi";
        public const string UpdateGroup = "update";
        public const string DetailsGroup = "detail/{id}";
        public const string SetDelegateDefault = "setDelegateDefault";
        public const string ReSetDelegateDefault = "resetDelegation";
        //AppUser
        public const string GetAllStatus = "getallstatus";
        public const string User = "api/appUser";
        public const string AppUsserAdd = "add";
        public const string GetUserByGroup = "getuserbygroup";
        public const string GetUserAllRequestByRoleAdmin = "getuserbyroleadmin";
        public const string GetUserAccount = "getuseraccount";
        public const string GetAllUserNameByUser = "getusername";
        public const string GetUserByDelegate = "getuserbydelegate";
        public const string GetGroupLeadByGroup = "getGroupLeadByGroup";
        public const string ChangePassword = "changepassword";
        public const string UpdateUser = "update";
        public const string UpdateUserProfile = "update-profile";
        public const string DetailsUser = "detail/{id}";
        public const string GetUserActive = "getuseractive";
        public const string GetUserInActive = "getuserinactive";
        public const string GetUserOnSite = "getuseronsite";
        public const string GetGroupLeadByGroupId = "getgrouplead";
        public const string GetGroupLeadByGroupSuperAdmin = "getgroupleadsuperadmin";
        public const string GetGroupLeadToAssign = "getgroupleadtoassign";
        public const string GetAllUser = "getAll";
        public const string UpdateUserOld = "updateuserold";
        //public const string GetAllUserSuperAdmin = "getallsuperadmin";
        public const string GetUserByAll = "GetUserByAll";
        public const string LogOut = "logout";
        public const string ChangeStatusUserMulti = "changestatusMulti";
        public const string GetFingerUserIdByUserID = "getfingeruseridbyuserid";
        public const string GetTotalUser = "gettotaluser";
        public const string ChangeStatusUser = "changestatus";
        public const string GetAllUserSuperAdmin = "getalluserbysuperadmin";
        public const string Resign = "resign";
        //Request Is Assigned
        public const string GetAllRequestIsAssignedForUser = "getallrequestisassignedforuser";
        public const string GetUserNameAssigned = "getusernameassigned";
        public const string GetListUserDelegation = "getlistuserdelegation";
        public const string GetAllOTRequestIsAssignedForUser = "getallotrequestisassignedforuser";
        public const string GetAllExplanationRequestIsAssignedForUser = "getallexplanationrequestisassignedforuser";
        public const string ListExplanationAssigned = "listexplanationassigned";
        //Explanation Request
        public const string ExplanationApi = "api/explanation";
        public const string ExplanationList = "getall";
        public const string ExplanationDetail = "detail/{id}";
        public const string ExplanationChangeStatus = "changestatus";
        public const string CreateExplanation = "add";
        public const string GetExplanationCreator = "getlistcreator";
        public const string ExplanationChangeStatusMulti = "changestatusmulti";
        public const string ExRequestChart = "exrequestchart";
        public const string ExRequestChartByUser = "exrequestchartbyuser";
        public const string exrequestByUser = "exrequestbyuser";
        public const string GetListExplanationDetail = "getlistexplanationdetail";
        public const string GetAllExplanationListRoleSuperAdmin = "getallexplanation";
        //Abnormal Reason Type
        public const string AbnormalReasonApi = "api/abnormalreason";
        public const string AbnormalReasonsList = "getall";
        //Entitle Day
        public const string EntitleApi = "api/entitleday";
        public const string EntitleGetAll = "getallentitle";
        public const string UserType = "getalluser";
        public const string EntitleDayManagementApi = "api/entitle-day-management";
        public const string EntitleDayManagementGetAll = "getallentitledaymanagement";
        public const string EntitleDayManagementAdd = "add";
        public const string EntitleDayManagementUpdate = "update";
        public const string EntitleDayManagementDelete = "delete";
        public const string EntitleDayManagementDetail = "detail/{id:int}";
        public const string GetAllType = "getalltype";
        public const string GetAllTypeFilter = "getalltypefilter";
        //EntitleDay AppUser
        public const string EntitleDayAppUserApi = "api/entitledayappuser";
        public const string EntitleDayAppUser = "getall";
        public const string EntitleDayAppUserDetail = "detail/{id}";
        public const string EntitleDayAppUserUpdate = "update";

        // List OT Member
        public const string OTList = "api/ot-list";
        public const string OTListGetAll = "getallotlist";
        public const string RequestAssignedList = "requestassigned";
        public const string GroupGetByID = "groupgetbyid";
        public const string GetAllUserName = "getallusername";
        public const string ExportExcel = "exportexcel";
        public const string ExportExcelEx = "exportexcelex";

        //AbnormalTimeSheetType
        public const string AbnormalTimeSheetType = "api/abnormal-timesheet-type";
        public const string AbnormalTimeSheetabset = "api/abnormal-timesheet-abset";
        public const string GetAbsent = "getall";
        public const string AbnormalChart = "abnormalchart";
        public const string AbnormalChartPercent = "abnormalchartpercent";
        //DelegationRequest
        public const string DelegationRequestApi = "api/delegationrequest";
        public const string GetAllDelegationRequest = "getalldelegationrequest";
        public const string Active_InactiveDelegation = "active_inactive-delegation";
        public const string CheckDelegationStatus = "check-delegation-status";
        //Timeday
        public const string TimeDay = "api/timeday";
        public const string TimeDayList = "getall";
        public const string Update = "update";
        public const string Delete = "delete";
        public const string TimeDayDetail = "detail/{id:int}";
        //System
        public const string System = "api/system";
        public const string SendMail = "sendMail";
        public const string SendMailMulti = "sendMailMulti";
        public const string SendMailMultiFix = "sendMailMultiFix";
        //DelegationExplanationRequest
        public const string DelegationExplanationRequestApi = "api/delegationexplanationrequest";
        public const string GetAllDelegationExplanationRequest = "getalldelegationexplanationrequest";

        //Human
        public const string humanApi = "api/human";
        public const string SendEmail = "sendemail";
        public const string GetAllMember = "allmember";
        public const string GetGroup = "allgroup";
        public const string SendFile = "sendfile";
        // ChildcareLeave
        public const string ChildcareLeave = "api/childcare-leave";
        public const string GetAllAppUser = "getallappuser";
        // Schedule
        public const string Schedule = "api/schedule";
        public const string AutoImportTimeSheet = "auto-import-timesheet";
        public const string JobChangeStatus = "job-change-status";
        public const string JobEntitleDay = "job-entitle-day";
        public const string JobUpdateEntitleDayByRequest = "job-entitle-day-by-request";
        //Config Delegation
        public const string ConfigDelegationApi = "api/configdelegation";
        public const string ConfigDelegationGetAll = "getalluserconfigdelegation";
        public const string AddConfigDelegation = "addconfigdelegation";
        public const string DeleteConfigDelegation = "deleteDelegation";
        //user onsite
        public const string UserOnsite = "api/user-onsite";
        public const string GetUserOnsiteInfo = "getinfo";
        public const string AddUserOnsiteInfo = "add";
        public const string UpdateUserOnsiteInfo = "update";
        public const string DeleteUserOnsiteInfo = "delete";
        public const string GetUserOnsiteInfoInTime = "getinfointime";
    }
}
