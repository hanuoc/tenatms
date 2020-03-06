using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TMS.Common.Constants;
using TMS.Model.Models;
using TMS.Web.Models.TimeSheet;

namespace TMS.Web.Models
{
    public class AppUserViewModel
    {
        public string Id { set; get; }
        [Required(ErrorMessage = MessageSystem.ValidateFullName)]
        [MinLength(CommonConstants.MinLenghtFullName,ErrorMessage = MessageSystem.ValidateFullNameMinLength) ]
        [MaxLength(CommonConstants.MaxLenghtFullName, ErrorMessage = MessageSystem.ValidateFullNameMaxLength)]
        public string FullName { set; get; }

        public string EmployeeID { set; get; }
        public DateTime BirthDay { set; get; }
        [Required(ErrorMessage = MessageSystem.ValidateEmail)]
        [RegularExpression(CommonConstants.RegexEmail, ErrorMessage = MessageSystem.RegexEmail)]
        [MaxLength(CommonConstants.MaxLenghtEmail, ErrorMessage =MessageSystem.ValidateEmailMaxLength)]
        public string Email { get; set; }
        public string Password { set; get; }
        public string UserName { set; get; }
        public string PhoneNumber { set; get; }
        public bool Status { get; set; }
        public string ListUserNo { set; get; }
        public DateTime StartWorkingDay { set; get; }
        public DateTime? ResignationDate { set; get; }
        public string Gender { get; set; }

        public int? GroupId { set; get; }
        public int? ChildcareLeaveID { set; get; }
        public bool? isOnsite { get; set; }
        public virtual GroupViewModel Group { set; get; }
        public ICollection<string> Roles { get; set; }
        public string fingerUserId { get; set; }
    }
}