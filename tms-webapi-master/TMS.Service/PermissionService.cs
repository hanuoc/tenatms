using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Common;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Model.Models;

namespace TMS.Service
{
    public interface IPermissionService
    {
        ICollection<Permission> GetByFunctionId(string functionId);
        ICollection<Permission> GetByUserId(string userId);
        void Add(Permission permission);
        void DeleteAll(string functionId);
        void SaveChange();
        void Update(Permission permission);
        void ChangeDelegatePermission(List<AppRole> lstRole, string action);
    }

    public class PermissionService : IPermissionService
    {
        private IPermissionRepository _permissionRepository;
        private IUnitOfWork _unitOfWork;

        public PermissionService(IPermissionRepository permissionRepository, IUnitOfWork unitOfWork)
        {
            this._permissionRepository = permissionRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Add(Permission permission)
        {
            _permissionRepository.Add(permission);
        }

        public void ChangeDelegatePermission(List<AppRole> lstRole, string action)
        {
            lstRole.RemoveAll(x => x.Name == "Admin");
            var lstRoleId = new List<string>();
            foreach (var item in lstRole)
            {
                lstRoleId.Add( item.Id);
            }
            List<Permission> lstPermission = _permissionRepository.GetMulti(x=>lstRoleId.Contains(x.RoleId)&&
            (x.FunctionId== "DELEGATION_LIST"), new string[] { "AppRole"}).ToList();
            if (action == "Active")
            {
                ConfigHelper.SetValue("DelegationStatus", "Actived");
                foreach (var item in lstPermission)
                {
                    if (item.AppRole.Name == "GroupLead")
                    {
                        var PermissionDelegateRequestManagement = _permissionRepository.GetSingleByCondition(x => x.AppRole.Name == "GroupLead" &&
           x.FunctionId == "DELEGATION_REQUEST_MANAGEMENT", new string[] { "AppRole" });
                        PermissionDelegateRequestManagement.CanRead = true;
                        PermissionDelegateRequestManagement.CanReadAll = true;
                        PermissionDelegateRequestManagement.CanUpdate = true;
                        PermissionDelegateRequestManagement.CanDelete = true;
                        PermissionDelegateRequestManagement.CanCreate = true;
                        Update(PermissionDelegateRequestManagement);
                        var PermissionDelegateExplainManagement = _permissionRepository.GetSingleByCondition(x => x.AppRole.Name == "GroupLead" &&
             x.FunctionId == "DELEGATION_EXPLANATION_REQUEST_MANAGEMENT", new string[] { "AppRole" });
                        PermissionDelegateExplainManagement.CanRead = true;
                        PermissionDelegateExplainManagement.CanReadAll = true;
                        PermissionDelegateExplainManagement.CanUpdate = true;
                        PermissionDelegateExplainManagement.CanDelete = true;
                        PermissionDelegateExplainManagement.CanCreate = true;
                        Update(PermissionDelegateExplainManagement);
                        var PermissionDelegateList = _permissionRepository.GetSingleByCondition(x => x.AppRole.Name == "GroupLead" &&
             x.FunctionId == "DELEGATION_LIST", new string[] { "AppRole" });
                        PermissionDelegateList.CanRead = true;
                        PermissionDelegateList.CanReadAll = false;
                        PermissionDelegateList.CanUpdate = true;
                        PermissionDelegateList.CanDelete = false;
                        PermissionDelegateList.CanCreate = true;
                        Update(PermissionDelegateList);
                    }
                    else
                    {
                        item.CanRead = true;
                        Update(item);
                    }
                }
            }
            else if (action == "Inactive")
            {
                ConfigHelper.SetValue("DelegationStatus", "Inactived");
                foreach (var item in lstPermission)
                {
                    if (item.AppRole.Name == "GroupLead")
                    {
                        var PermissionDelegateRequestManagement = _permissionRepository.GetSingleByCondition(x => x.AppRole.Name == "GroupLead" &&
            (x.FunctionId == "DELEGATION_REQUEST_MANAGEMENT"), new string[] { "AppRole" });
                        PermissionDelegateRequestManagement.CanRead = false;
                        PermissionDelegateRequestManagement.CanReadAll = false;
                        PermissionDelegateRequestManagement.CanUpdate = false;
                        PermissionDelegateRequestManagement.CanDelete = false;
                        PermissionDelegateRequestManagement.CanCreate = false;
                        Update(PermissionDelegateRequestManagement);
                        var PermissionDelegateExplainManagement = _permissionRepository.GetSingleByCondition(x => x.AppRole.Name == "GroupLead" &&
             (x.FunctionId == "DELEGATION_EXPLANATION_REQUEST_MANAGEMENT"), new string[] { "AppRole" });
                        PermissionDelegateExplainManagement.CanRead = false;
                        PermissionDelegateExplainManagement.CanReadAll = false;
                        PermissionDelegateExplainManagement.CanUpdate = false;
                        PermissionDelegateExplainManagement.CanDelete = false;
                        PermissionDelegateExplainManagement.CanCreate = false;
                        Update(PermissionDelegateExplainManagement);
                        var PermissionDelegateList = _permissionRepository.GetSingleByCondition(x => x.AppRole.Name == "GroupLead" &&
             x.FunctionId == "DELEGATION_LIST", new string[] { "AppRole" });
                        PermissionDelegateList.CanRead = false;
                        PermissionDelegateList.CanReadAll = false;
                        PermissionDelegateList.CanUpdate = true;
                        PermissionDelegateList.CanDelete = false;
                        PermissionDelegateList.CanCreate = true;
                        Update(PermissionDelegateList);
                    }
                    else
                    {
                        item.CanRead = false;
                        Update(item);
                    }
                }
            }
            SaveChange();
            
        }

        public void DeleteAll(string functionId)
        {
            _permissionRepository.DeleteMulti(x => x.FunctionId == functionId);
        }

        public ICollection<Permission> GetByFunctionId(string functionId)
        {
            return _permissionRepository
                .GetMulti(x => x.FunctionId == functionId, new string[] { "AppRole", "AppRole" }).ToList();
        }

        public ICollection<Permission> GetByUserId(string userId)
        {
            return _permissionRepository.GetByUserId(userId);
        }

        public void SaveChange()
        {
            _unitOfWork.Commit();
        }

        public void Update(Permission permission)
        {
            _permissionRepository.Update(permission);
        }
    }
}
