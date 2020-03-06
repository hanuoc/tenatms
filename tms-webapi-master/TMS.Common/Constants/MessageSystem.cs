using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.Constants
{
    public class MessageSystem
    {
        public const string NoValues = " No value. ";
        public const string NoData = "No data";
        public const string MessageDuplicateEmail = "Email already exists!";
        public const string MessageDuplicateUserName = "Account already exists!";
        public const string MessageErrorUserFullName = "Name invalid!";
        public const string WorngUserNameAndPassWord = "Your username or password is not correct!";
        public const string ServerProcessingError = "The connection with the server has been terminated!";
        public const string ValidateEmail = "Require input email !";
        public const string RegexEmail = "Email invalid!";
        public const string ValidateEmployeeID = "Require input EmployeeID !";
        public const string ValidateFullName = "Require input name";
        public const string ValidateFullNameMinLength = "Full name length must be more than 3 character !";
        public const string ValidateFullNameMaxLength = "Full name length must not be more than 50 character!";
        public const string ValidateEmployeeIDMaxLength = "EmployeeID length must not be more than 20 character!";
        public const string ValidateEmailMaxLength = "Email length must not be more than 50 character!";
        public const string ChangePasswordSuccess = "Change password succesfully!";
        public const string ErrorOldPassword = "Old Password is incorrect!";
        public const string UserNotFound = "Không tìm thấy người dùng";
        public const string PasswordNotMatch = "Xác nhận mật khẩu không khớp với mật khẩu mới";
        public const string DuplicateOldPassword = "Mật khẩu mới không được trùng mật khẩu cũ";
        public const string ContraintNewPassword = "Mật khẩu ít nhất 8 đến 16 ký tự, Chữ hoa, Chư Thường và ít nhất 1 ký tự đặc biệt";
        public const string ErrorIdNull = "Không tim thấy ID";
        public const string CancelFail = "Bạn không thể hủy yêu cầu này";
        public const string Pending = "Request had been Pending";
        public const string Approved = "Request had been Approved";
        public const string Rejected = "Request had been Rejected";
        public const string Error_User_Online = "This user is online !";
        public const string BlockAccount = "Your account is blocked";
        public const string Unable_Update_Role_YourSelf = "Unable to update role yourself!";
        public const string Error_Exist_Group_Lead = "Cannot update. This group already has Group Lead!";
        public const string Error_Create_Exist_Group_Lead = "Can not create ! This group has existed group lead !";
        public const string MessageUserNoExist = "UserNo already exists!";
        public const string MessageExistTimeSheetEmpNo = "Exist record with this EmployeeNo in TimeSheet!";
        public const string MessageUserNoNotValid = "UserNo not valid!";
        //OTRequest
        public const string ValidateTitle = "Bạn phải nhập tiêu đề";
        public const string ValidateOTDateType = "Bạn phải chọn loại ngày OT";
        public const string ValidateOTTimeType = "Bạn phải chọn loại giờ OT";
        public const string ValidateOTRequestUser = "Bạn phải chọn người OT";
        public const string ValidateTitleMinLength = "Tiêu đề phải ít nhất 10 ký tự";
        public const string ValidateTitleMaxLength = "Tiêu đề phải không quá 100 ký tự";
        public const string StartTimeCompareEndTime = "Start Time should be sooner than End Time!";
        public const string NormalDayNormalTime = "Can not choose Normal OT Date Type combined with Normal OT Time Type!";
        public const string NormalDayNightTimePick = "Start Time and End Time should be 6:00 PM to 10:00 PM";
        public const string NormalDayNormalTimePick = "Start Time and End Time should be between 6:00 AM to 6:00 PM";
        public const string ERROR_CREATE_OTREQUEST_IN_PAST_NOT_MSG = "Can not add ot request in the past!";
        public const string ERROR_CHANGESTATUS_OTREQUEST_IN_PAST_NOT_MSG = "Can not change status ot request in the past!";
        public const string ERROR_CANNOT_CREATE_OT_EXIST = "Cannot create. OT Request on this day already exist.";
        public const string ERROR_MUST_SELECT_NOMAL = "You must select OT Date Type : Normal!";
        public const string ERROR_MUST_SELECT_WEEKEND = "You must select OT Date Type : Weekend!";
        public const string ERROR_MUST_SELECT_HOLIDAY = "You must select OT Date Type : Holiday!";

        //Request
        public const string ERROR_CREATE_REQUEST_NOT_MSG = "Can not add request. You don't have enough day off";
        public const string ERROR_CREATE_REQUEST_IN_PAST_NOT_MSG = "Can not add request in the past!";
        public const string ERROR_CHANGESTATUS_REQUEST_IN_PAST_NOT_MSG = "Can not change status request in the past!";
        public const string ERROR_HNN = "HNN";
        public const string APPROVED_ERROR_HNN = "APPROVED_ERROR";
        public const string ERROR_CHANGESTATUS_REQUEST_NOT_IN_DELEGATEDEFAULT_TIME_MSG = "You can't delegate to other member. Please change delegated default date!";
        //Group
        public const string RequireGroupLead = "Please select a Group Lead!";
        public const string RequireGroupName = "Require input group name";
        public const string GroupExist = "This group already exists";
        public const string ERROR_ExistMemberInGroup = "Exist member in group ! Can not delete groups !";
        public const string ERROR_UPDATE_LEADER = "Can not update group lead !";
        public const string SelectGroupToDelete = "Select group to delete !";

        //Explanation Request
        public const string Create_Explanation_Error = "Something wrong. Please try again!";
        public const string Create_Explanation_Success = "Create explanation request successfully.";
        public const string Change_Explanation_Status_Error = "The data has not added yet. Please try again!";
        public const string Change_Explanation_Status_Success = "Change explanation status successfully.";
        public const string ERROR_CREATE_EXREQUEST_IN_PAST_NOT_MSG = "Can not add explanation request in the past!";
        public const string ERROR_CHANGESTATUS_EXREQUEST_IN_PAST_NOT_MSG = "Can not change status  explanation request in the past!";
        public const string ERROR_CREATE_EXREQUEST_NOT_ENOUGH_ENTITLE_DAY = "Cannot add explanation. You do not have enough day off.!";

        //Entitle Day Management
        public const string Create_EntitleDayManagement_Error = "Holidays already exists";
        public const string MaxEntitleDay_Error = "Date must be greater than 0 and less than 366";
        public const string NoSalary_Error = "Cannot create No-Salary Day-off";
        // EntitleDayAppUser
        public const string MaxEntitleDayNumberDay_Error = "Approved Days cannot be greater than Allowed Maximum Days";
        public const string NumberDayOff_Error = "Please enter a common multiple of 0.5; 1- 5 numeric characters in length";
        /// Timeday request
        public const string CheckExits = "Timeday is already existed.";
        public const string CheckTimeday = "Check-In should be AM. Check-Out should be PM";
        public const string CheckTime = "Check-In time should be sooner than Check-Out time!";
        // Create User
        public const string CheckBirthDayAndStartDate = "Please select Starting Date again!";
        // Update file
        public const string UpdatedFail = "Error updated file ";

        //Content Email
        public const string TitleCreateOTRequest = "OT Request";
        public const string CreateOTRequest = "OT Request ";
        public const string CreateString = "is created by ";

        //Error import file
        public const string ErrorImportFile = "Error importing file. Please see details below!";
        public const string ErrorInvalidDataFile = "The data in the file is invalid in the line ";
        public const string ErrorDuplicateData = "Duplicate data for this UserNo and this Date ";
        public const string InvalidDataAccNo = "The data in Acccount No column is invalid";
        public const string InvalidDate = "The data in Date column is invalid";
        public const string InvalidFingerNumber = "The data in Finger Number column is invalid";
        public const string InvalidAccountName = "The data in Account Name column is invalid";
        public const string InvalidDataAccNoNotExits = "The data in Acccount No column not exits";
        public const string InvalidDataAccNamNotExits = "The data in Acccount Name column not exits";
        public const string FormatIsNotSupport = "Format is not supported. Please upload file .txt";
        public const string ErrorTimeDay = "Don't have working time for this date ";

        public const string ChangeStatusSuccess = "Change status successfully!";

        //Explanation Message
        public const string ERROR_EXPLANATION_NOT_ENOUGH_ENTITLE_DAY = " not enough entitle day , can not approve !";
        public const string ERROR_EXPLANATION_CONTACT_ADMIN= "Number day off has changed, can not reject ! Please contact admin to resolve this problem!";

        //Request
        public const string ERROR_REQUEST_FULLTIME = "Cannot create request of Full time Leave.";
        public const string ERROR_REQUEST_FULLTIME_MORNING = "Cannot create request of Full time Leave and Morning Leave on the same day.";
        public const string ERROR_REQUEST_FULLTIME_AFTERNOON = "Cannot create request of Full time Leave and Afternoon Leave on the same day.";
        public const string ERROR_REQUEST_FULLTIME_LATECOMING = "Cannot create request of Full time Leave and Afternoon Leave on the same day.";
        public const string ERROR_REQUEST_FULLTIME_EARLYLEAVING = "Cannot create request of Full time Leave and Early-Leaving on the same day.";
        public const string ERROR_REQUEST_MORNING_FULLTIME = "Cannot create request of Full time Leave.";
        public const string ERROR_REQUEST_MORNING = "Cannot create. Request of Morning Leave on this day already exists.";
        public const string ERROR_REQUEST_MORNING_LATECOMING = "Cannot create request of Morning Leave and Late-Coming on the same day.";
        public const string ERROR_REQUEST_AFTERNOONLEAVE = "Cannot create. Request of Afternoon Leave on this day already exists";
        public const string ERROR_REQUEST_AFTERNOONLEAVE_EARLYLEAVING = "Cannot create request of Afternoon Leave and Early-Leaving on the same day.";
        public const string ERROR_REQUEST_LATECOMING_EARLYLEAVING = "Cannot create. Request of Late-Coming on this day already exists.";
        public const string ERROR_REQUEST_EARLYLEAVING_AFTERNOONLEAVE = "Cannot request Afternoon Leave and Early-Leaving on the same day";
        public const string ERROR_REQUEST_EARLYLEAVING = "Cannot create. Request of Early-Leaving on this day already exists.";

        //Request Entitle Day
        public const string ERROR_REQUEST_ENTITLEDAY = "Unable to create a half-day leave request with this excuse/explanation.";

		//Holiday
		public const string ERROR_HOLIDAY_CREATE_INTHE_PAST = "Can not create holiday or offset working day in the past!";
		public const string ERROR_HOLIDAY_CREATE_CONTAIN_HOLIDAY = "Time day list must contain holiday!";
		public const string ERROR_HOLIDAY_CREATE_IN_TIMEDAY = "Offset working day must not in time day list!";
		public const string ERROR_HOLIDAY_CREATE_EXISTED = "Holiday or offset working day was existed!";
		public const string ERROR_DELETE_NOT_FOUND = "Not found Holiday!";
		public const string ERROR_DELETE_INTHE_PAST = "Can not delete holiday in the past.";
	}
}
