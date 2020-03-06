using System;
using TMS.Web.Models.EntitleDay;
using TMS.Web.Models.EntitleDayManagement;

namespace TMS.Web.Models.Request
{
    [Serializable]
    public class RequestViewModel
    {
        public int ID { set; get; }

        public string Title { set; get; }

        public string UserId { set; get; }
        public string DelegateId { set; get; }
        public int? EntitleDayId { set; get; }

        public int RequestTypeId { set; get; }

        public int RequestStatusId { set; get; }

        public string UserAssignedID { set; get; }

        public string DetailReason { set; get; }
        public DateTime StartDate { set; get; }

        public DateTime EndDate { set; get; }

        public DateTime? CreatedDate { set; get; }

        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        public string UpdatedBy { set; get; }

        public bool Status { set; get; }
        public string ChangeStatusById { set; get; }

        public string AssignToId { set; get; }
        public string FullNameDelegate { set; get; }

        public bool CheckGroupDelegateDefault { set; get; }
        public bool CheckConfigDelegateDefault { set; get; }
        public string AssignGroupDelegate { set; get; }
        public string AssignConfigDelegate { set; get; }
        public string GroupName { set; get; }

        public virtual EntitleDayManagementViewModel EntitleDay { set; get; }

        public virtual RequestTypeViewModel RequestType { set; get; }

        public virtual AppUserViewModel AppUser { set; get; }

        public virtual AppUserViewModel AppUserAssigned { set; get; }

        public virtual StatusRequestViewModel StatusRequest { set; get; }
        public virtual AppUserViewModel AppUserChangeStatus { set; get; }

        public virtual AppUserViewModel AppUserDelegate { set; get; }

        public virtual AppUserViewModel AppUserAssign { set; get; }
    }
}