using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TMS.Common.Constants;
using TMS.Model.Models;
using TMS.Web.Models.Request;

namespace TMS.Web.Models.OTRequest
{
    [Serializable]
    public class OTRequestViewModel
    {
        public int ID { set; get; }

        [Required(ErrorMessage = MessageSystem.ValidateTitle)]
        [MinLength(CommonConstants.MinLengthTitle, ErrorMessage = MessageSystem.ValidateTitleMinLength)]
        [MaxLength(CommonConstants.MaxLengthTitle, ErrorMessage = MessageSystem.ValidateTitleMaxLength)]
        public string Title { set; get; }

        public string UserID { get; set; }
        public DateTime? OTDate { set; get; }
        [Required(ErrorMessage = MessageSystem.ValidateOTDateType)]
        public int OTDateTypeID { get; set; }
        [Required(ErrorMessage = MessageSystem.ValidateOTTimeType)]
        public int OTTimeTypeID { get; set; }
        public int StatusRequestID { get; set; }
        public string StartTime { set; get; }
        public string EndTime { set; get; }
        public string UserAssignedID { get; set; }
        [Required(ErrorMessage = MessageSystem.ValidateOTRequestUser)]
        public string[] OTRequestUserID { get; set; }
        public string[] toEmail { get; set; }
        public virtual OTDateTypeViewModel OTDateType { set; get; }
        public virtual OTTimeTypeViewModel OTTimeType{ set; get; }
        public virtual StatusRequestViewModel StatusRequest { set; get; }
        public virtual AppUserViewModel AppUserUpdatedBy { get; set; }
        public virtual AppUserViewModel AppUserCreatedBy { get; set; }
        public virtual AppUserViewModel AppUserAssigned { set; get; }
        public DateTime? CreatedDate { set; get; }
        public string CreatedBy { set; get; }
        public DateTime? UpdatedDate { set; get; }
        public string UpdatedBy { set; get; }
    }
}