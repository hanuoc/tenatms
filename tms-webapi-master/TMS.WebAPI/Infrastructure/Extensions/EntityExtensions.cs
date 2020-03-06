using System;
using System.Globalization;
using TMS.Common.Constants;
using TMS.Common.ViewModels;
using TMS.Model.Models;
using TMS.Web.Models;
using TMS.Web.Models.EntitleDay;
using TMS.Web.Models.EntitleDayManagement;
using TMS.Web.Models.Explanation;
using TMS.Web.Models.Group;
using TMS.Web.Models.OTRequest;
using TMS.Web.Models.Request;
using TMS.Web.Models.TimeDay;
using TMS.Web.Models.TimeSheet;

namespace TMS.Web.Infrastructure.Extensions
{
    public static class EntityExtensions
    {
        public static void UpdateOTRequest(this OTRequest otRequest , OTRequestViewModel otRequestVm)
        {
            otRequest.ID = otRequestVm.ID;
            otRequest.Title = otRequestVm.Title;
            otRequest.CreatedBy = otRequestVm.CreatedBy;
            otRequest.OTDate = otRequestVm.OTDate;
            otRequest.OTDateTypeID = otRequestVm.OTDateTypeID;
            otRequest.OTTimeTypeID = otRequestVm.OTTimeTypeID;
            otRequest.StatusRequestID = otRequestVm.StatusRequestID;
            otRequest.UserAssignedID = otRequestVm.UserAssignedID;
            otRequest.CreatedDate = otRequestVm.CreatedDate;
            otRequest.UpdatedDate = otRequestVm.UpdatedDate;
            otRequest.UpdatedBy = otRequestVm.UpdatedBy;
            otRequest.StartTime = otRequestVm.StartTime;
            otRequest.EndTime = otRequestVm.EndTime;
        }
        public static void EditOTRequest(this OTRequest otRequest, OTRequestViewModel otRequestVm)
        {
            otRequest.ID = otRequestVm.ID;
            otRequest.Title = otRequestVm.Title;
            otRequest.CreatedBy = otRequestVm.CreatedBy;
            otRequest.OTDate = otRequestVm.OTDate;
            otRequest.OTDateTypeID = otRequestVm.OTDateTypeID;
            otRequest.OTTimeTypeID = otRequestVm.OTTimeTypeID;
            otRequest.StatusRequestID = otRequestVm.StatusRequestID;
            //otRequest.UserAssignedID = otRequestVm.UserAssignedID;
            otRequest.CreatedDate = otRequestVm.CreatedDate;
            otRequest.CreatedBy = otRequestVm.CreatedBy;
            //otRequest.UpdatedDate = otRequestVm.UpdatedDate;
            //otRequest.UpdatedBy = otRequestVm.UpdatedBy;
            otRequest.StartTime = otRequestVm.StartTime;
            otRequest.EndTime = otRequestVm.EndTime;
        }
        public static void UpdateEntitleDayManagement(this EntitleDay entitleDay, EntitleDayManagementViewModel entitleDayManagementViewModel)
        {
            entitleDay.ID = entitleDayManagementViewModel.ID;
            entitleDay.HolidayType = entitleDayManagementViewModel.HolidayType;
            entitleDay.UnitType = entitleDayManagementViewModel.UnitType;
            entitleDay.MaxEntitleDay = entitleDayManagementViewModel.MaxEntitleDay;
            entitleDay.Description = entitleDayManagementViewModel.Description;
            entitleDay.Status= entitleDayManagementViewModel.Status;
        }
        public static void UpdateEntitleDayAppUser(this Entitleday_AppUser entitleday_AppUser, EntitleDay_AppUserModel entitleDay_AppUserModel)
        {
            entitleday_AppUser.ID = entitleDay_AppUserModel.EntitleDayAppUserId;
            entitleday_AppUser.MaxEntitleDayAppUser = entitleDay_AppUserModel.MaxEntitleDay;
            entitleday_AppUser.NumberDayOff = entitleDay_AppUserModel.NumberDayOff;
            entitleday_AppUser.Note = entitleDay_AppUserModel.Note;
        }

        public static void UpdateFunction(this Function function, FunctionViewModel functionVm)
        {
            function.Name = functionVm.Name;
            function.DisplayOrder = functionVm.DisplayOrder;
            function.IconCss = functionVm.IconCss;
            function.Status = functionVm.Status;
            function.ParentId = functionVm.ParentId;
            function.Status = functionVm.Status;
            function.URL = functionVm.URL;
            function.ID = functionVm.ID;
        }
        public static void UpdatePermission(this Permission permission, PermissionViewModel permissionVm)
        {
            permission.RoleId = permissionVm.RoleId;
            permission.FunctionId = permissionVm.FunctionId;
            permission.CanCreate = permissionVm.CanCreate;
            permission.CanDelete = permissionVm.CanDelete;
            permission.CanRead = permissionVm.CanRead;
            permission.CanUpdate = permissionVm.CanUpdate;
        }

        public static void UpdateApplicationRole(this AppRole appRole, ApplicationRoleViewModel appRoleViewModel, string action = "add")
        {
            if (action == "update")
                appRole.Id = appRoleViewModel.Id;
            else
                appRole.Id = Guid.NewGuid().ToString();
            appRole.Name = appRoleViewModel.Name;
            appRole.Description = appRoleViewModel.Description;
        }

        public static void UpdateUser(this AppUser appUser, AppUserViewModel appUserViewModel, string action = "add")
        {
            appUser.Id = appUserViewModel.Id;
            appUser.EmployeeID = appUserViewModel.EmployeeID;
            appUser.FullName = appUserViewModel.FullName;
            appUser.GroupId = appUserViewModel.GroupId;
            appUser.Email = appUserViewModel.Email;
            appUser.UserName = appUserViewModel.UserName;
            appUser.PhoneNumber = appUserViewModel.PhoneNumber;
            appUser.Gender = appUserViewModel.Gender == "True" ? true : false;
            appUser.Status = appUserViewModel.Status;
            //appUser.isOnsite = appUserViewModel.isOnsite;
            //if (!string.IsNullOrEmpty(appUserViewModel.AccNameInMachineFinger)){
            //    appUser.AccNameInMachineFinger = appUserViewModel.AccNameInMachineFinger;
            //}
            appUser.BirthDay = appUserViewModel.BirthDay;
            appUser.StartWorkingDay = appUserViewModel.StartWorkingDay;
        }
        public static void UpdateRequest(this Request request, RequestViewModel requestViewModel)
        {
            request.AppUser = new AppUser();
            request.ID = requestViewModel.ID;
            request.DelegateId = requestViewModel.DelegateId;
            request.AppUser.GroupId = requestViewModel.AppUser.GroupId;
            request.Title = requestViewModel.Title;
            request.UserId = requestViewModel.UserId;
            request.EntitleDayId = requestViewModel.EntitleDayId;
            request.RequestTypeId = requestViewModel.RequestTypeId;
            request.RequestStatusId = requestViewModel.RequestStatusId;
            request.DetailReason = requestViewModel.DetailReason;
            request.StartDate = requestViewModel.StartDate;
            request.EndDate = requestViewModel.EndDate;
            request.CreatedDate = requestViewModel.CreatedDate;
            request.CreatedBy = requestViewModel.CreatedBy;
            request.UpdatedDate = requestViewModel.UpdatedDate;
            request.UpdatedBy = requestViewModel.UpdatedBy;
            request.Status = requestViewModel.Status;
            request.ChangeStatusById = requestViewModel.ChangeStatusById;
            request.AssignToId = requestViewModel.AssignToId;
        }
        public static void EditRequest(this Request request, RequestViewModel requestViewModel)
        {
            request.ID = requestViewModel.ID;
            request.DelegateId = requestViewModel.DelegateId;
            request.AppUser.GroupId = requestViewModel.AppUser.GroupId;
            request.Title = requestViewModel.Title;
            request.UserId = requestViewModel.UserId;
            request.EntitleDayId = requestViewModel.EntitleDayId;
            request.RequestTypeId = requestViewModel.RequestTypeId;
            request.RequestStatusId = requestViewModel.RequestStatusId;
            request.DetailReason = requestViewModel.DetailReason;
            request.StartDate = requestViewModel.StartDate;
            request.EndDate = requestViewModel.EndDate;
            request.CreatedDate = requestViewModel.CreatedDate;
            request.CreatedBy = requestViewModel.CreatedBy;
            request.UpdatedDate = requestViewModel.UpdatedDate;
            request.UpdatedBy = requestViewModel.UpdatedBy;
            request.Status = requestViewModel.Status;
            request.ChangeStatusById = requestViewModel.ChangeStatusById;
            request.AssignToId = requestViewModel.AssignToId;
        }
        public static void UpdateGroup(this Group group, GroupCreateUpdateModel groupVm)
        {
            group.ID = groupVm.ID;
            group.Name = groupVm.Name;
            group.Description = groupVm.Description;
            group.DelegateId = groupVm.DelegateId;
            group.StartDate = groupVm.StartDate;
            group.EndDate = groupVm.EndDate;

        }
        public static void UpdateTimeday(this TimeDay timeday , TimeDayViewModel timedayViewModel)
        {
            timeday.ID = timedayViewModel.ID;
            timeday.Workingday = timedayViewModel.Workingday;
            timeday.CheckIn = timedayViewModel.CheckIn;
            timeday.CheckOut = timedayViewModel.CheckOut;
        }

        public static void UpdateExplanationRequest(this ExplanationRequest explanation, ExplanationRequestViewModel explanationViewModel)
        {
            explanation.Actual = explanationViewModel.Actual;
            explanation.Title = explanationViewModel.Title;
            explanation.CreatedBy = explanationViewModel.CreatedBy;
            explanation.ReceiverId = explanationViewModel.ReceiverId;
            explanation.ReasonDetail = explanationViewModel.ReasonDetail;
            explanation.CreatedDate = DateTime.Now;
            explanation.StatusRequestId = explanationViewModel.StatusRequestId;
            explanation.TimeSheetId = explanationViewModel.TimeSheetId;
            explanation.Status = true;
        }
        public static void UpdateFingerManchineUser(this FingerMachineUser fingerMachine, FingerMachineUserViewModel fingerMachineViewModel)
        {
            fingerMachine.ID = fingerMachineViewModel.ID;
            fingerMachine.UserId = fingerMachineViewModel.UserId;
        }

        public static void UpdateConfigDelegation(this ConfigDelegation delegation, ConfigDelegationModel delegationVm)
        {
            delegation.AssignTo = delegationVm.AssignTo;
            delegation.StartDate = delegationVm.StartDate;
            delegation.EndDate = delegationVm.EndDate;

        }

        public static void DeleteConfigDelegation(this ConfigDelegation delegation)
        {
            delegation.AssignTo = null;
            delegation.StartDate = null;
            delegation.EndDate = null;
        }

        public static void ResetConfigDelegate(this Group group)
        {
            group.DelegateId = null;
            group.StartDate = null;
            group.EndDate = null;
        }
    }
}