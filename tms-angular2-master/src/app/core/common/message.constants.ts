export class MessageConstants {
    public static SYSTEM_ERROR_MSG = "The connection with the server has been terminated!";
    public static LOGIN_ERROR_MSG = "Your username or password is not correct!";
    public static CONFIRM_DELETE_MSG = "Do you really want to delete this record?";
    public static CONFIRM_DELETE_MULTI_MSG = "Do you really want to delete these groups?";
    public static CONFIRM_CANCEL_MSG = "Do you really want to cancel this record?";
    public static LOGIN_AGAIN_MSG = "Your session is expired.Please login again!."
    public static CREATED_OK_MSG = "Add successfully!";
    public static CREATED_FAIL_MSG = "Timeout to create OT request. Please try it again!";
    public static CREATED_FAIL_NO_GROUPLEAD_MSG = "Your group has no group lead. Please try it again!";
    public static ERROR_CREATE_MSG = "Can not add request. You don't have enough day off!";
    public static APPROVED_DELEGATION_REQUEST_MSG = "Can not apporved request. Member don't have enough day off!";
    public static UPDATED_OK_MSG = "Update successfully!";
    public static CANCEL_OK_MSG = "Cancel successfully!";
    public static DELETED_OK_MSG = "Delete successfully!";
    public static REMOVE_OK_MSG = "Remove successfully!";
    public static RESET_OK_MSG = "Reset successfully!";
    public static CHANGESTATUS_OK_MSG = "Change status successfully!";
    public static FORBIDDEN = "Bạn bị chặn truy cập";
    public static BAD_REQUEST = "Dữ liệu nhập vào không đúng";
    public static APPROVED_SUCCES_MSG = "Approved OT request successfully!";
    public static Rejected_SUCCES_MSG = "Rejected OT request successfully!";
    public static Error_Edit_By_Admin = "Your profile has changed by admin , Please login again!";
    public static CONFIRM_DELETE_TIMEDAY_MSG = "Do you really want to delete this day?";
    public static CONFIRM_DELETE_ENTITLE_MSG = "Do you really want to delete this day?";
    public static Resign_Success_MSG = "Set resignation successfully !";
    // Notification Change Password
    public static CHANGE_PASSWORD_OK = "Change password successfully!";
    public static PASSWORD_NOT_MATCH = "New Password and Confirm Password do not match!";
    public static DUPLICATE_OLD_PASSWORD = "New Password cannot match Old Password!";
    public static CONSTRAINT_NEW_PASSWORD = "Mật khẩu ít nhất 8 đến 16 ký tự, Chữ hoa, Chư Thường và ít nhất 1 ký tự đặc biệt";
    // Notification Request
    public static CREATE_ERROR_REQUEST_TYPE = "You must choose request type";
    public static CREATE_ERROR_REQUEST_REASONTYPE = "You must choose request reason type";
    public static CREATE_ERROR_REQUEST_CREATEDATE = "Create date must be less than start date";
    public static CONFIRM_CANCEL_REQUEST_MSG = "Do you really want to cancel this request?";
    public static REJECT_REQUEST_MSG = " has been rejected";
    public static TEXTAREA_COMMENT_REQUEST_MSG = "Input reason reject this request...";
    public static APPROVED_SUCCES_REQUEST_MSG = "Approved request successfully!";
    public static REJECTED_SUCCES_REQUEST_MSG = "Rejected request successfully!";
    public static REJECTED_SUCCES_EXPLANATION_REQUEST_MSG = "Rejected explanation request successfully!";
    public static DELEGATE_SUCCES_REQUEST_MSG = "Delegate request successfully!";
    public static ERROR_HOLIDAYTYPE_MSG ="You used all Entitled Days";
    public static LIMIT_TIME_CREATE_REQUEST_MONRING_MSG ="Time expired. Please create request of Morning Leave or Late Coming before 12:00PM!";
    public static LIMIT_TIME_CREATE_REQUEST_AFTERNOON_MSG ="Time expired. Cannot create today's request!";
    public static CONFIRM_CLOSE_REQUEST_MSG = "Do you want to close this popup?";
    // OT Request 
    public static REJECT_OTREQUEST_MSG = " has been rejected";
    public static TEXTAREA_COMMENT_OTREQUEST_MSG = "Input reason reject this OT request...";
    public static CONFIRM_CANCEL_OTREQUEST_MSG = "Do you really want to cancel this OT request?";
    public static CREATED_FAIL_OTREQUEST_MSG = "Có lỗi kết nối đến máy chủ.Vui lòng tạo lại sau. Xin cảm ơn!";
    public static REJECT_OTREQUEST_EXPIRE_MSG = "Your OT Request is expire!";
    public static CONFIRM_CLOSE_OTREQUEST_MSG = "Do you want to close this OT popup?";
    // Notification Explanation Request
    public static REJECT_EXPLANATION_MSG = " has been rejected";
    public static CONFIRM_CANCEL_EXPLANATION_MSG = "Do you really want to cancel this explanation request?";
    public static APPROVED_SUCCESS_EXPLANATION_MSG = "Approved explanation successfully!";
    public static REJECTED_SUCCESS_EXPLANATION_MSG = "Rejected explanation successfully!";
    public static DELEGATED_SUCCESS_EXPLANATION_MSG = "Delegated explanation successfully!";
    public static TEXTAREA_COMMENT_REJECT_EXPLANATION_MSG = "Input reason reject this explanation request...";
    public static CONFIRM_CANCEL_DELEGATION_EXPLANATION_MSG = "Do you really want to cancel this explanation request?";
    public static ADD_SUCCESS_EXPLANATION_MSG = "Added explanation successfully!";
    public static CONFIRM_CLOSE_EXPLANATION_MSG = "Do you want to close this popup?";
    // Delegation Request
    public static CONFIRM_CANCEL_DELEGATION_EXPLANATION_REQUEST_MSG = "Do you really want to cancel this delegation explanation request?";
    public static CONFIRM_CANCEL_DELEGATION_REQUEST_MSG = "Do you really want to cancel this delegation request?";
    public static CONFIRM_APPROVE_DELEGATION_REQUEST_MSG = "Do you really want to approve this request?";
    public static CONFIRM_REJECT_DELEGATION_REQUEST_MSG = "Do you really want to reject this request?";
    public static APPROVE_OK_MSG = "Approve successfully!";
    public static REJECTE_OK_MSG = "Reject successfully!";  
    public static REJECTED_OK_MSG = "Request had been rejected";
    public static APPROVED_OK_MSG = "Request had been approved";
    public static SEND_MAIL_OK_MSG = "Send mail successfully!";
    public static SEND_MAIL_ERROR_MSG = "Send mail error";
    public static CHECK_EMAIL_ERROR_MSG = "Check your email";
    public static REJECTED_EXPLANATION_REQUEST_OK_MSG = "Explanation request had been rejected";
    public static APPROVED_EXPLANATION_REQUEST_OK_MSG = "Explanation request had been approved";
    public static VALID_COMMENT_REJECT_MSG = "Reject reason required!";
    public static CONFIRM_Active_Delegation = "Do you really want to activate Delegation ?";
    public static CONFIRM_Inactive_Delegation = "Do you really want to deactivate Delegation ?";
    //Notification group
    public static LoadLeaderFail = "Group Lead Not Found!";
    public static SelectGroupToDelete = "Select group to delete";
    //TimeDay
    public static DateTime_ERROR_MSG = "Check-In time should be sooner than Check-Out time!";
    public static OT_ERROR_MSG = "OT From should be sooner than OT To!";
    public static CONFIRM_TIMESHEET_DELETE_MSG = "Do you really want to delete this time sheet?";
    //Group 
    public static CONFIRM_GROUP_DELETE_MSG = "Do you really want to delete this group?";
    public static CONFIRM_CLOSE_GROUP_MSG = "Do you want to close this popup?";
    public static CONFIRM_DELEGATE_RESET_MSG = "Do you really want to reset this delegate default?";
    //HR 
    public static SEND_MAIL_NO_INPUT_MSG = "Please enter into subject and content!";
    public static SEND_MAIL_NO_SPACE_MSG = "Content and Subject doesn't space";
    public static SEND_MAIL_NO_CHOSEN_MEMBER_MSG = "Please choose a member!";
    //Upload
    public static SELECT_FILE_UPLOAD = "Please select a file you want to upload!";
    public static NO_SUPPORT_FILE = "File format not supported!";
    public static FILE_MAX_LENG = "Max size file 1Mb";
    public static FILE_TYPE_NOT_SUPPORT = "File type is not supported";
    public static FILE_TOO_LARGE = "Max size file 5Mb";
    public static SAVE_ERROR = "Error save file, please try again";
    //List OT
    public static DOWNLOAD_OK_MSG = "Export file successfully!";
    public static DOWNLOAD_OK_ERROR = "Export file error";
    //Time Day
    public static CONFIRM_CLOSE_TIMEDAY_MSG = "Do you want to close this popup?";

    //Entitle day
    public static CONFIRM_CLOSE_ENTITLE_DAY_MSG = "Do you want to close this popup?";

    //Entitle day list(Admin)
    public static CONFIRM_CLOSE_ENTITLE_DAY_ADMIN_MSG = "Do you want to close this popup?";

    //User
    public static CONFIRM_CLOSE_USER_MSG = "Do you want to close this popup?";
    
    //Config delegation
    public static CONFIRM_DELEGATION_DELETE_MSG = "Do you really want to reset this config delegation?";
}