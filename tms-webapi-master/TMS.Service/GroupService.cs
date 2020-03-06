using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Data;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Model.Models;

namespace TMS.Service
{
    public interface IGroupService
    {
        /// <summary>
        /// Get group by id
        /// </summary>
        /// <param name="groupId"> id to find</param>
        /// <returns>group</returns>
        Group GetGroupById(string groupId);
        /// <summary>
        /// get group lead ID by group
        /// </summary>
        /// <param name="groupId"> id of group</param>
        /// <returns>ID of Group Leader</returns>
        string GetGroupLeadIdByGroup(int groupId);
        /// <summary>
        /// get all group
        /// </summary>
        /// <returns>list group</returns>
        List<Group> GetAllGroup();
        /// <summary>
        /// get group by name
        /// </summary>
        /// <param name="groupName">group name to find</param>
        /// <returns>group</returns>
        Group GetGroupByName(string groupName);
        /// <summary>
        /// update group
        /// </summary>
        /// <param name="group">group to uppdate</param>
        void Update(Group group);
        /// <summary>
        /// add group
        /// </summary>
        /// <param name="group"> group to add</param>
        /// <returns>group</returns>
        Group Add(Group group);
        /// <summary>
        /// delete group by id
        /// </summary>
        /// <param name="id">id of group to delete</param>
        /// <returns>group</returns>
        Group Delete(int id);
        /// <summary>
        /// save database
        /// </summary>
        void SaveChange();
        /// <summary>
        /// check duplicate group name
        /// </summary>
        /// <param name="groupName">group name to check</param>
        /// <param name="id">id of old group</param>
        /// <returns>true if group name already exist</returns>
        bool IsDuplicateGroup(string groupName, int id);
        /// <summary>
        /// check group empty
        /// </summary>
        /// <param name="groupId">id of group to check</param>
        /// <param name="groupLeadId">id of group lead this group</param>
        /// <returns>true if group has no member(only have group lead)</returns>
        bool IsGroupEmpty(int groupId, string groupLeadId);
    }
    public class GroupService : IGroupService
    {
        private IGroupRepository _groupRepository;
        private IUnitOfWork _unitOfWork;
        private IUserService _userService;
        /// <summary>
        /// Constructor OT Request Service
        /// </summary>
        /// <param name="otrequestRepository"></param>
        /// <param name="unitOfWork"></param>
        public GroupService(IUserService userService, IGroupRepository groupRepository, IUnitOfWork unitOfWork)
        {
            this._userService = userService;
            this._groupRepository = groupRepository;
            this._unitOfWork = unitOfWork;
        }

        public Group GetGroupById(string groupID)
        {
            return _groupRepository.GetSingleById(Int32.Parse(groupID));
        }
        /// <summary>
        /// get all group
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns>list group</returns>
        public List<Group> GetAllGroup()
        {
            return _groupRepository.GetAll().Where(x => x.Name != "SuperAdmin").ToList();
        }

        public Group Add(Group group)
        {
            return _groupRepository.Add(group);
        }
        public void SaveChange()
        {
            _unitOfWork.Commit();
        }

        public Group Delete(int id)
        {
            return _groupRepository.Delete(id);
        }

        public Group GetGroupByName(string groupName)
        {
            return _groupRepository.GetSingleByCondition(x => x.Name == groupName);
        }

        public bool IsDuplicateGroup(string groupName, int id)
        {
            var group = _groupRepository.GetSingleByCondition(x => x.Name.ToUpper() == groupName.ToUpper());
            if (group == null)
                return false;
            else
            {
                if (group.ID == id)
                    return false;
            }
            return true;
        }

        public bool IsGroupEmpty(int groupId,string groupLeadId)
        {
            var group = _groupRepository.GetSingleByCondition(x => x.ID == groupId);
            if (group != null)
            {
                var users = _userService.GetAllUserByGroup(groupId);
                switch (users.Count)
                {
                    case 1:
                        if (users.FirstOrDefault().Id == groupLeadId)
                            return true;
                        else
                            return false;
                    case 0:
                        return true;
                    default:
                        return false;
                }
            }
            else
                return false;
        }

        public string GetGroupLeadIdByGroup(int groupId)
        {
            if (_userService.GetGroupLeadByGroup(groupId) != null)
            {
                return _userService.GetGroupLeadByGroup(groupId).Id;
            }
            else
                return null;
        }

        public void Update(Group group)
        {
            _groupRepository.Update(group);
        }
    }
}
