using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common
{
    public class CommonConstants
    {
        public const string ProductTag = "product";
        public const string PostTag = "post";
        public const string DefaultFooterId = "default";

        public const string SessionCart = "SessionCart";

        public const string HomeTitle = "HomeTitle";
        public const string HomeMetaKeyword = "HomeMetaKeyword";
        public const string HomeMetaDescription = "HomeMetaDescription";

        public const string Administrator = "Administrator";

        //SignalR
        public const string AdminChannel = "AdminChannel";
        public const string AnnouncementChannel = "AnnouncementChannel";
        // Error Change Password
        public const string ChangePassword = "changepassword";
        public const string ChangePasswordSuccess = "Mật Khẩu Không Được Giống Nhau";
        public const string ErrorNewPassword = "Mật Khẩu Cũ Và Mật Khẩu Mới Không Được Giống Nhau";
        public const string ErrorPassword = "Mật khẩu cũ không đúng";
        public const string SreachUserChangePassword = "Không tìm thấy người dùng";
        public const string CheckNewPassword = "Mật Khẩu phải gồm chữ số, chữ cái, chữ hoa và có ít nhất 1 ký tự đặc biệt";
        //Constants Message
        public const string MessageErrorUserEmail = "Email đã tồn tại!";
        public const string MessageErrorUserFullName = "Họ tên không hợp lệ";
        //Constants Number
        public const int MinLenghtFullName = 3;
    }
}
