using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TMS.Common.Constants;
using TMS.Common.Exceptions.Extensions;
using TMS.Common.ViewModels;
using TMS.Data;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Model.Models;

namespace TMS.Service
{
    public interface IUserService
    {
        /// <summary>
        /// Find user by id
        /// </summary>
        /// <param name="userId">id of user need find</param>
        /// <returns></returns>
        Task<AppUser> FindUserById(string userId);
        List<AppUser> GetAllUserByGroup(int groupId);
        List<AppUser> GetAllUserFilter(FilterUser filter, string column, bool isDesc);
        List<AppUser> GetUserActive(FilterUser filter, string column, bool isDesc);
        List<AppUser> GetUserInActive(FilterUser filter, string column, bool isDesc);
        List<AppUser> GetUserOnSite(FilterUser filter, string column, bool isDesc);
        AppUser GetGroupLeadByGroup(int groupId);
        IEnumerable<AppUser> GetMemberFilter(FilterGroup filter, string search = null);
        void AddListUserEditByAdmin(string userName);
        IEnumerable<AppUser> GetAll();
        //IEnumerable<AppUser> CheckStastWorkingDay();
        TotalUser CountUser();
        string CheckStartDateAndBirthDay(DateTime birthday, DateTime startdate);
        void DeleteTableUser(string id);
    }
    public class UserService : IUserService
    {
        private IAppUserRepository _appUserRepository;
        private IUserOnsiteRepository _userOnsiteRepository;
        private IUnitOfWork _unitOfWork;
        private IOTRequestUserRepository _oTRequestUserRepository;
        private IOTRequestRepository _oTRequestRepository;
        private IRequestRepository _requestRepository;
        private IExplanationRequestRepository _explanationRequestRepository;
        private IEntitleDayAppUserRepository _entitleDayAppUserRepository;
        private IAbnormalCaseRepository _abnormalCaseRepository;
        private IFingerTimeSheetRepository _fingerTimeSheetRepository;
        private IFingerMachineUserRepository _fingerMachineUserRepository;
        private IChildcareLeaveRepository _childcareLeaveRepository;
        private IAnnouncementRepository _announcementRepository;
        private IAnnouncementUserRepository _announcementUserRepository;
        private ITimeSheetRepository _timeSheetRepository;
        public UserService(IAppUserRepository appUserRepository, IUserOnsiteRepository userOnsiteRepository,  IUnitOfWork unitOfWork,
            IOTRequestUserRepository oTRequestUserRepository,
            IRequestRepository requestRepository,
            IOTRequestRepository oTRequestRepository,
            IExplanationRequestRepository explanationRequestRepository,
            IEntitleDayAppUserRepository entitleDayAppUserRepository,
            IAbnormalCaseRepository abnormalCaseRepository,
            IFingerTimeSheetRepository fingerTimeSheetRepository,
            IFingerMachineUserRepository fingerMachineUserRepository,
            IChildcareLeaveRepository childcareLeaveRepository,
            IAnnouncementRepository announcementRepository,
            IAnnouncementUserRepository announcementUserRepository,
            ITimeSheetRepository timeSheetRepository
            )
        {
            this._appUserRepository = appUserRepository;
            this._userOnsiteRepository=userOnsiteRepository;
            this._unitOfWork = unitOfWork;
            this._oTRequestUserRepository = oTRequestUserRepository;
            this._requestRepository = requestRepository;
            this._oTRequestRepository = oTRequestRepository;
            this._explanationRequestRepository = explanationRequestRepository;
            this._entitleDayAppUserRepository = entitleDayAppUserRepository;
            this._abnormalCaseRepository = abnormalCaseRepository;
            this._fingerTimeSheetRepository = fingerTimeSheetRepository;
            this._fingerMachineUserRepository = fingerMachineUserRepository;
            this._childcareLeaveRepository = childcareLeaveRepository;
            this._announcementRepository = announcementRepository;
            this._announcementUserRepository = announcementUserRepository;
            this._timeSheetRepository = timeSheetRepository;
    }
        /// <summary>
        /// Find user by id
        /// </summary>
        /// <param name="userId">id of user need find</param>
        /// <returns>user</returns>
        public async Task<AppUser> FindUserById(string userId)
        {
            return _appUserRepository.GetMulti(x => x.Id == userId, new string[] { "Group" }).SingleOrDefault();
        }
        public List<AppUser> GetAllUserFilter(FilterUser filter, string column, bool isDesc)
        {
            var model = _appUserRepository.GetAll(new string[] { CommonConstants.UserGroup });
            if (filter != null)
            {
                if (filter.GroupID.Count() != 0)
                {
                    model = model.Where(x => filter.GroupID.Contains(x.GroupId.ToString()));
                }
                if (filter.UserID.Count() != 0)
                {
                    model = model.Where(x => filter.UserID.Contains(x.Id));
                }
                //if (filter.Active[0])
                //{

                //}
                if (filter.Active.Count() != 0)
                {
                    switch (filter.Active.Count())
                    {
                        case 1:
                            model = model.Where(x => x.Status == filter.Active[0]);
                            break;
                            //default:
                    }

                    //for (int i = 0; i < filter.Active.Count(); i++)
                    //{
                    //    model = model.Where(x => x.Status == filter.Active[i]);
                    //}
                }
            }
            if (column != null)
            {
                return model.OrderByField(column, isDesc).ToList();
            }
            return model.ToList();
        }

        public List<AppUser> GetUserActive(FilterUser filter, string column, bool isDesc)
        {
            var model = _appUserRepository.GetAll(new string[] { CommonConstants.UserGroup }).Where(x => x.Status == true);
            if (filter != null)
            {
                if (filter.GroupID.Count() != 0)
                {
                    model = model.Where(x => filter.GroupID.Contains(x.GroupId.ToString()));
                }
                if (filter.UserID.Count() != 0)
                {
                    model = model.Where(x => filter.UserID.Contains(x.Id));
                }
            }
            if (column != null)
            {
                return model.OrderByField(column, isDesc).ToList();
            }
            return model.ToList();
        }
        public List<AppUser> GetUserInActive(FilterUser filter, string column, bool isDesc)
        {
            var model = _appUserRepository.GetAll(new string[] { CommonConstants.UserGroup }).Where(x => x.Status == false);
            if (filter != null)
            {
                if (filter.GroupID.Count() != 0)
                {
                    model = model.Where(x => filter.GroupID.Contains(x.GroupId.ToString()));
                }
                if (filter.UserID.Count() != 0)
                {
                    model = model.Where(x => filter.UserID.Contains(x.Id));
                }
            }
            if (column != null)
            {
                return model.OrderByField(column, isDesc).ToList();
            }
            return model.ToList();
        }
        public List<AppUser> GetUserOnSite(FilterUser filter, string column, bool isDesc)
        {
            var lstUserOnsite = _userOnsiteRepository.GetAll().Select(x => x.UserID).Distinct();
            var model = _appUserRepository.GetMulti(x => lstUserOnsite.Contains(x.Id));
            if (filter != null)
            {
                if (filter.GroupID.Count() != 0)
                {
                    model = model.Where(x => filter.GroupID.Contains(x.GroupId.ToString()));
                }
                if (filter.UserID.Count() != 0)
                {
                    model = model.Where(x => filter.UserID.Contains(x.Id));
                }
            }
            if (column != null)
            {
                return model.OrderByField(column, isDesc).ToList();
            }
            return model.ToList();
        }


        public List<AppUser> GetAllUserByGroup(int groupId)
        {
            return _appUserRepository.GetAllUserByGroup(groupId);
        }

        public AppUser GetGroupLeadByGroup(int groupId)
        {
            return _appUserRepository.GetGroupLeadByGroup(groupId);
        }

        /// <summary>
        /// Find user by id
        /// </summary>
        /// <param name="userId">id of user need find</param>
        /// <returns>user</returns>
        private IEnumerable<AppUser> GetMember()
        {
            var getAllMember = _appUserRepository.GetAll(new string[] { CommonConstants.UserGroup });
            return getAllMember.ToList();
        }


        /// <summary>
        /// Get member list
        /// </summary>
        /// <param name="filter">filter by group</param>
        /// <param name="search">search by name / username</param>
        /// <returns></returns>
        public IEnumerable<AppUser> GetMemberFilter(FilterGroup filter, string search = null)
        {
            var model = GetMember();
            if (!string.IsNullOrEmpty(search))
                model = model.Where(x => x.UserName.Contains(search) || x.FullName.Contains(search));
            if(filter!= null) { 
            if (filter.Group != null)
                model = model.Where(x => filter.Group.Equals(x.GroupId.ToString()));
            }
            return model;
        }
        public void AddListUserEditByAdmin(string userName)
        {
            if (HttpRuntime.Cache[CommonConstants.ListUserEditByAdmin] != null)
            {
                //get the list of logged in users from the cache
                var ListUserEditByAdmin = (Dictionary<string, DateTime>)HttpRuntime.Cache[CommonConstants.ListUserEditByAdmin];

                if (!ListUserEditByAdmin.ContainsKey(userName))
                {
                    //add this user to the list
                    ListUserEditByAdmin.Add(userName, DateTime.Now);
                    //add the list back into the cache
                    HttpRuntime.Cache[CommonConstants.ListUserEditByAdmin] = ListUserEditByAdmin;
                }
            }
            //the list does not exist so create it
            else
            {
                //create a new list
                var ListUserEditByAdmin = new Dictionary<string, DateTime>();
                //add this user to the list
                ListUserEditByAdmin.Add(userName, DateTime.Now);
                //add the list into the cache
                HttpRuntime.Cache[CommonConstants.ListUserEditByAdmin] = ListUserEditByAdmin;
            }
        }

        public IEnumerable<AppUser> GetAll()
        {
            return _appUserRepository.GetAll();
        }
        public TotalUser CountUser()
        {
            var model = _appUserRepository.GetAll().Where(x=> x.UserName != "admin");
            TotalUser totalUsers = new TotalUser()
            {
                TotalUsers = model.Count(),
                TotalMale = model.Count(x => x.Gender == true),
                TotalFemale = model.Count(x => x.Gender == false),
                TotalActive = model.Count(x => x.Status == true),
                TotalInactive = model.Count(x => x.Status == false),
                TotalOnsite = _userOnsiteRepository.CountUserOnsite()
            };
            return totalUsers;
        }
        public string CheckStartDateAndBirthDay (DateTime BirthDay, DateTime StartDate)
        {
            var result = DateTime.Compare(BirthDay, StartDate);
            if (result >= 0)
            {
                return MessageSystem.CheckBirthDayAndStartDate;
            }
            return null;
        }
        public void DeleteTableUser(string id)
        {
            // Announcements
            _announcementRepository.DeleteMulti(x => x.UserId.Equals(id));
            // Announcements User
            _announcementUserRepository.DeleteMulti(x => x.UserId.Equals(id));
            // Childcare Leaves
            _childcareLeaveRepository.DeleteMulti(x => x.UserId.Equals(id));
            //AppUserRoles
            //AppUserLogins
            // TimeSheets
            _timeSheetRepository.DeleteMulti(x => x.UserID.Equals(id));
            // Groups
            //_groupRepository.DeleteMulti(x => x.UserID.Equals(id));
            // Entitle Day AppUser
            _entitleDayAppUserRepository.DeleteMulti(x => x.UserId.Equals(id));
            //Request
            _requestRepository.DeleteMulti(x => x.UserId.Equals(id));
            // Request Assign To Id
            _requestRepository.DeleteMulti(x => x.AssignToId.Equals(id));
            //OT Request
            _oTRequestRepository.DeleteMulti(x => x.CreatedBy.Equals(id));
            //OT Request User
            _oTRequestUserRepository.DeleteMulti(x => x.UserID.Equals(id));
            //Explanation Request
            _explanationRequestRepository.DeleteMulti(x => x.CreatedBy.Equals(id));
            //User Onsites
            _userOnsiteRepository.DeleteMulti(x => x.UserID.Equals(id));
            var getUserNo = _fingerMachineUserRepository.GetMulti(x => x.UserId.Equals(id)).FirstOrDefault();

            if (getUserNo != null)
            {
                var getIdFingerTimeSheet = _fingerTimeSheetRepository.GetMulti(x => x.UserNo == getUserNo.ID);
                if(getIdFingerTimeSheet != null)
                {
                    // AbnormalCase
                    _abnormalCaseRepository.DeleteMulti(x => getIdFingerTimeSheet.Count(y => y.ID == x.TimeSheetID) > 0);
                    // Finger Time Sheet
                    _fingerTimeSheetRepository.DeleteMulti(x => x.UserNo.Equals(getUserNo.ID));
                    // FingerMachineUsers
                    _fingerMachineUserRepository.DeleteMulti(x => x.UserId.Equals(id));
                }
            }
            // Childcare Leaves
            _childcareLeaveRepository.DeleteMulti(x => x.UserId.Equals(id));
            // App User
            _appUserRepository.DeleteMulti(x => x.Id.Equals(id));
            //_unitOfWork.Commit();
        }
    }
}
