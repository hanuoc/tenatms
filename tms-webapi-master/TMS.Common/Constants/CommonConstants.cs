namespace TMS.Common.Constants
{
    public class CommonConstants
    {
        public const string DefaultFooterId = "default";
        public const string HomeTitle = "HomeTitle";
        public const string Administrator = "Administrator";

        //SignalR
        public const string AdminChannel = "AdminChannel";

        public const string AnnouncementChannel = "AnnouncementChannel";

        //Table
        public const string Announcements = "Announcements";

        public const string AnnouncementUsers = "AnnouncementUsers";
        public const string AppRoles = "AppRoles";

        public const string AppUsers = "AppUsers";
        public const string FingerMachineUsers = "FingerMachineUsers";
        public const string FingerTimeSheets = "FingerTimeSheets";
        public const string FingerTimeSheetTmps = "FingerTimeSheetTmps";
        public const string Errors = "Errors";
        public const string Functions = "Functions";
        public const string Groups = "Groups";
        public const string OTDateTypes = "OTDateTypes";
        public const string OTRequests = "OTRequests";
        public const string TimeSheets = "TimeSheets";
        public const string OTTimeTypes = "OTTimeTypes";
        public const string OTRequestUsers = "OTRequestUser";
        public const string Pages = "Pages";
        public const string Permissions = "Permissions";
        public const string Requests = "Requests";
        public const string RequestTypes = "RequestTypes";
        public const string RequestReasonTypes = "RequestReasonTypes";
        public const string StatusRequests = "StatusRequests";
        public const string ExplanationStatus = "ExplanationStatus";
        public const string SystemConfigs = "SystemConfigs";
        public const string ExplanationRequests = "ExplanationRequests";
        public const string ExplanationTypes = "ExplanationTypes";
        public const string AbnormalCases = "AbnormalCases";
        public const string AbnormalReasons = "AbnormalReasons";
        public const string AbnormalTimeSheetType = "AbnormalTimeSheetTypes";
        public const string AbnormalReasonTypes = "AbnormalReasonTypes";
        public const string EntitleDay = "EntitleDay";
        public const string EntitleDay_AppUser = "EntitleDay_AppUser";
        public const string TimeSheet = "TimeSheet";
        public const string AbnormalReason = "AbnormalReason";
        public const string ListOT = "ListOT";
        public const string Timedays = "Timedays";
        public const string CheckInOut = "CHECKINOUT";
        public const string USERINFO = "USERINFO";
        public const string ChildcareLeave = "ChildcareLeaves";
        public const string Report = "Reports";
        public const string ConfigDelegations = "ConfigDelegations";
        public const string JobLog = "JobLog";

        //Include object related
        public const string AppUserGroup = "AppUser.Group";

        public const string AppUserCreateByGroup = "AppUserCreatedBy.Group";
        public const string StatusRequest = "StatusRequest";
        public const string RequestType = "RequestType";
        public const string RequestReasonType = "EntitleDay";
        public const string AbnormalCase = "AbnormalCase";
        public const string AbnormalCaseAbnormalReason = "AbnormalCase.AbnormalReason";
        public const string OTDateType = "OTDateType";
        public const string OTTimeType = "OTTimeType";
        public const string OTRequestUser = "OTRequestUser";
        public const string ReasonType = "ReasonType";
        public const string AbnormalCaseAppUser = "AbnormalCase.AppUser";
        public const string AbnormalCaseReason = "AbnormalCaseReason";
        public const string ReceiverUser = "Receiver.Group";
        public const string DelegateUser = "Delegate.Group";
        public const string AppUserCreatedBy = "AppUserCreatedBy.Group";
        public const string AppUserUpdatedBy = "AppUserUpdatedBy.Group";
        //Entitle Day
        public const string HolidayType = "Authorized Leave";
        public const string Maternity = " Maternity";
        public const string EntitleDayType = "EntitleDayType";
        public const int MaxEntitleDay = 365;
        public const int MinEntitleDay = 0;
        public const string EntitleDayReasonType = "EntitleDayReasonType";
        public const string ExplanationRequestAppUser = "ExplanationRequest.AppUser";
        public const string ExplanationRequestAbnormalCaseTimeSheet = "ExplanationRequest.AbnormalCase.FingerTimeSheet";
        public const string ExplanationRequestExplanationStatus = "ExplanationRequest.ExplanationStatus";
        public const string AppUserAssignGroup = "AppUserAssign.Group";
        public const string AppUserChangeStatusGroup = "AppUserChangeStatus.Group";
        public const string ExplanationType = "ExplanationType";
        public const string AppUserAssigned = "AppUserAssigned";
        public const string AppUserDelegate = "AppUserDelegate.Group";
        public const string UserGroup = "Group";
        public const string Request_EntitleDay_Error = "Request of this Day-Off type exists already. Cannot delete this Day-off type.";
        //Function
        public const string FunctionAbnormalCase = "ABNORMALCASE_LIST";

        public const string FunctionOTRequest = "OTREQUEST_LIST";
        public const string FunctionEntitleDay = "ENTITLEDAY_LIST";
        public const string FunctionExplanationRequest = "EXPLANATION_LIST";
        public const string FunctionOTList = "OT_LIST";
        public const string FunctionRequest = "REQUEST_LIST";
        public const string FunctionTimeSheet = "TIMESHEET_LIST";
        public const string FunctionDelegationMemberRequest = "DELEGATION_LIST";
        public const string FunctionDelegationRequest = "DELEGATION_REQUEST_MANAGEMENT";
        public const string FunctionDelegationExplanationRequest = "DELEGATION_EXPLANATION_REQUEST_MANAGEMENT";
        public const string FunctionReport = "REPORT";
        public const string FingerTimeSheet = "FingerTimeSheet";
        public const string FingerMachineUser = "FingerTimeSheet.FingerMachineUsers";
        public const string TimeSheetAppUser = "FingerTimeSheet.FingerMachineUsers.AppUser";
        public const string TimeSheetAppUserGroup = "FingerTimeSheet.FingerMachineUsers.AppUser.Group";

        //Format data
        public const string FormatDate_MMDDYYY = "MM/dd/yyyy";
        public const string FormatDate_DDMMYYY = "dd/MM/yyyy";
        public const string FORMAT_DATE_MMDDYYYY = "MM/dd/yyyy";
        public const string FORMAT_local_EN = "en-gb";
        public const string FORMAT_HHMM = "HH:mm";


        //Timesheet
        public const string PathUploadTimeSheet = "~/UploadedFiles/TimeSheets/";
        public const string PathUploadTimeSheetError = "~/UploadedFiles/Errors/";
        public const string POINT = ". ";
        public const string REGEX = "\t";
        public const string FileTimeSheetSupport = ".txt";
        public const string TimeSheetAbsentMorning = "VS";
        public const string TimeSheetAbsentAfternoon = "VC";
        public const string TimeSheetAbsent = "V";
        public const string FilterTimeSheetComeLate = "DiMuon";
        public const string FilterTimeSheetComeBackSoon = "VeSom";
        public const string ABSENT_HOUR_MORNING = "10:00";
        public const string ABSENT_HOUR_AFTERNOON = "16:00";
        public const string EARLY_HOUR = "16:00";
        public const string LATE_AFTERNOON = "13:00";
        public const string BreakStart = "12:00";
        public const string ORDERBY_DATE = "Date";
        public const string DayOfWeekSaturday = "Saturday";
        public const string DayOfWeekSunDay = "Sunday";
        public const int ONE = 1;
        public const int ZERO = 0;
        public const int ONEHUNDRED = 100;
        public const double ZERO_PONT_FIVE = 0.5;
        public const double ZERO_PONT_FOUR = 0.4;
        public const string FOURTY_PERCENT = "40%";

        public const string FullTimeLeave = "Full-time Leave";
        //Constants Number
        public const int MinLenghtFullName = 3;
        public const int TimeLifeCache = -180;
        public const int TimeExcuteSql = 360;

        //Error constant
        public const string Invalid_Grant = "invalid_grant";

        public const string ServerError = "server_error";
        public const string UserOnlineError = "user_online_error";
        public const string Error_Edit_By_Admin = "Error_Edit_By_Admin";
        public const int MaxLenghtEmployeeID = 20;
        public const int MaxLenghtFullName = 50;
        public const int MaxLenghtEmail = 50;
        public const int MinLengthTitle = 10;
        public const int MaxLengthTitle = 100;
        public const string SEND_MAIL_NO_SPACE = "SEND_MAIL_NO_SPACE";
        //Regex
        public const string RegexEmail = @"^[a-z][a-z0-9_\.]{2,32}@[a-z0-9]{2,}(\.[a-z0-9]{2,4}){1,2}$";

        //Role
        public const string Admin = "Admin";
        public const string GroupLead = "GroupLead";

        //Type break
        public const string Break = "Leave";

        public const string BreakMorning = "Morning Leave";
        public const string BreakAfternoon = "Afternoon Leave";
        public const string UnpaidLeave = "No Salary";
        public const string AuthorizedLeave = "Authorized Leave";
        public const string NoSalary = "No Salary";
        //Status Request
        public const string StatusApproved = "Approved";
        public const int StatusApprovedID = 4;
		public const int StatusPendingID = 1;
		public const int StatusDelegateID = 5;
		public const string StatusRejected = "Rejected";
        public const string StatusPending = "Pending";
        public const string StatusDelegation = "Delegated";
        public const string StatusCancelled = "Cancelled";
        public const string StatusCreated = "Created";
        public const string StatusCancel = "Cancel";
        //Sort by column
        public const string OrderByCreatedBy = "CreatedBy";

        //common
        public const string Updated = "Updated";
        public const string StringEmpty = "";
        public const string DefaultGroupLeader = "The group has no group lead";
        public const string UserLoginCache = "LoggedInUsers";
        public const string ListUserEditByAdmin = "ListUserEditByAdmin";

        //List OT
        public const string WorkEndTime = "17:30";
        public const string dateExport = "yyyyMMddhhmmssfff";
        public const string dateExports = "yyyyMMdd";
        public const string fileExport = ".xlsx";
        public const string reportFolder = "ReportFolder";
        public const string downloadSuccess = "Download Success";
        public const string Link = "/";
        public const string Hours = "h";


        public const int OTDateTypeNormal = 1;
        public const int OTDateTypeWeekend = 2;
        public const int OTDateTypeHoliDay = 3;
        public const int OTTimeTypeNormal = 1;
        public const int OTTimeTypeNight = 2;
        public const double HourForOTTimeNightStart = 18;
        public const double HourForOTTimeNightEnd = 22;
        public const double HourForOTNormalStart = 6;
        public const double HourForOTNormalEnd = 18;
        public const double StartTimeday = 12;
        public const double EndTimeday = 13;

        public const string dateNowStartEntitleDay = "dd/MM";
        public const string dateStartEntitleDay = "01/04";

        //Abnormal Chart
        public const int dataStartInMonth = 25;

        //Abnormal Reason
        public const int UnauthorizedLateComing = 1;
        public const int UnauthorizedEarlyLeaving = 2;
        public const int UnauthorizedLeave = 5;

        public const int UnusedAuthorizedEarlyLeaving = 3;
        public const int UnusedAuthorizedLateComing = 4;
        public const int UnusedAuthorizedLeave = 6;
        public const int OTWithoutCheckIn = 7;
        public const int OTWithoutCheckOut = 8;
        public const int OTWithoutCheckInOut = 9;

        //RequestType
        public const int RequestTypeLateComming = 4;
        public const int RequestTypeEarlyLeaving = 5;

        //ChildCare
        public const int TimeLate = 1;

        //
        public const string Day = "Day";
        public const string DayPeriod = "Day/Period";

        //Super Admin Id
        public const string AdminUsername = "admin";
        public const string RoleAdminName = "SuperAdmin";

		public const int DateReject = 2;

	}
}