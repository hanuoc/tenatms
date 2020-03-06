using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TMS.Common;
using TMS.Common.Constants;
using TMS.Common.Exceptions;
using TMS.Common.ViewModels;
using TMS.Data.Infrastructure;
using TMS.Model.Models;
using TMS.Service;
using TMS.Web.App_Start;
using TMS.Web.Infrastructure.Core;
using TMS.Web.Infrastructure.Extensions;
using TMS.Web.Models;

using TMS.Web.Providers;

namespace TMS.Web.Controllers
{
    [Authorize]
    [RoutePrefix(Common.Constants.RoutesConstant.User)]
    public class AppUserController : ApiControllerBase
    {
        /// <summary>
        /// Contructor of AppUserController class
        /// </summary>
        /// <param name="errorService"></param>
        private IUserService _userService;
        private IEntitleDayAppUserService _entitleDayAppUserService;
        private IEntitleDayManagemantService _entitleDayManagemantService;
        private IEntitleDayService _entitleDayService;
        private IFingerMachineUserService _fingermachineuserService;
        private IFingerTimeSheetService _fingerTimeSheetService;
        private IConfigDelegationService _configDelegateionService;
        private IUnitOfWork _unitOfWork;
        public AppUserController(IUnitOfWork unitOfWork, IErrorService errorService,
            IUserService userService,
            IEntitleDayAppUserService entitleDayAppUserService,
            IEntitleDayService entitleDayService,
            IEntitleDayManagemantService entitleDayManagemantService,
            IFingerMachineUserService fingermachineuserService,
            IFingerTimeSheetService fingerTimeSheetService,
            IConfigDelegationService configDelegateionService)
            : base(errorService)
        {
            this._userService = userService;
            _entitleDayAppUserService = entitleDayAppUserService;
            _entitleDayService = entitleDayService;
            _entitleDayManagemantService = entitleDayManagemantService;
            _fingermachineuserService = fingermachineuserService;
            _fingerTimeSheetService = fingerTimeSheetService;
            _configDelegateionService = configDelegateionService;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Function change password of user
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id">Id user change password</param>
        /// <param name="password">User enters old passwor</param>
        /// <param name="newPassword">User enters new password</param>
        /// <returns></returns>
        [HttpPut]
        [Route(Common.Constants.RoutesConstant.ChangePassword)]
        public async Task<HttpResponseMessage> ChangePassword(HttpRequestMessage request, string id, string password, string newPassword)
        {
            if (User != null && MemoryCacheHelper.RemoveUserEditByAdmin(User.Identity.Name))
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, CommonConstants.Error_Edit_By_Admin);
            }
            if (!string.IsNullOrEmpty(id))
            {
                var appUser = await AppUserManager.FindByIdAsync(id);
                if (appUser != null)
                {
                    bool passwordCheck = await AppUserManager.CheckPasswordAsync(appUser, password);
                    if (passwordCheck)
                    {
                        var result = await AppUserManager.ChangePasswordAsync(appUser.Id, password, newPassword);
                        return request.CreateResponse(HttpStatusCode.OK, Common.Constants.MessageSystem.ChangePasswordSuccess);
                    }
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, Common.Constants.MessageSystem.ErrorOldPassword);
                }
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, Common.Constants.MessageSystem.UserNotFound);
            }
            return request.CreateErrorResponse(HttpStatusCode.BadRequest, Common.Constants.MessageSystem.ErrorIdNull);
        }

        /// <summary>
        /// Get list of user with pagination
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isDesc">Descending or not</param>
        /// <param name="page">Number of page</param>
        /// <param name="pageSize">Number of pagesize</param>
        /// <param name="filter">List of filtter or not</param>
        /// <param name="column">Colunm want sort</param>
        /// <returns>Retrun a user list after pagination</returns>
        [Route("getlistpaging")]
        [HttpPost]
        [Permission(Action = "Read", Function = "USER")]
        public async Task<HttpResponseMessage> GetListPaging(HttpRequestMessage request, string column, bool isDesc, int page, int pageSize, [FromBody]FilterUser filter)
        {
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int totalRow = 0;
                var model = _userService.GetAllUserFilter(filter, column, isDesc);
                foreach (var user in model)
                {
                    //Loại user admin 
                    if (user.UserName.Equals(CommonConstants.AdminUsername))
                    {
                        model.Remove(user);
                        break;
                    }
                }
                totalRow = model.Count();
                IEnumerable<AppUserViewModel> modelVm = Mapper.Map<IEnumerable<AppUser>, IEnumerable<AppUserViewModel>>(model.Skip((page - 1) * pageSize).Take(pageSize));
                PaginationSet<AppUserViewModel> pagedSet = new PaginationSet<AppUserViewModel>()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    TotalRows = totalRow,
                    Items = modelVm,
                };
                response = request.CreateResponse(HttpStatusCode.OK, pagedSet);
                return response;
            });
        }

        [Route(Common.Constants.RoutesConstant.GetUserActive)]
        [HttpPost]
        public async Task<HttpResponseMessage> GetListActivePaging(HttpRequestMessage request, string column, bool isDesc, int page, int pageSize, [FromBody]FilterUser filter)
        {
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int totalRow = 0;
                var model = _userService.GetUserActive(filter, column, isDesc);
                foreach (var user in model)
                {
                    //Loại user admin 
                    if (user.UserName.Equals(CommonConstants.AdminUsername))
                    {
                        model.Remove(user);
                        break;
                    }
                }
                totalRow = model.Count();
                IEnumerable<AppUserViewModel> modelVm = Mapper.Map<IEnumerable<AppUser>, IEnumerable<AppUserViewModel>>(model.Skip((page - 1) * pageSize).Take(pageSize));
                PaginationSet<AppUserViewModel> pagedSet = new PaginationSet<AppUserViewModel>()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    TotalRows = totalRow,
                    Items = modelVm,
                };
                response = request.CreateResponse(HttpStatusCode.OK, pagedSet);
                return response;
            });
        }
        [Route(Common.Constants.RoutesConstant.GetUserInActive)]
        [HttpPost]
        public async Task<HttpResponseMessage> GetListInActivePaging(HttpRequestMessage request, string column, bool isDesc, int page, int pageSize, [FromBody]FilterUser filter)
        {
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int totalRow = 0;
                var model = _userService.GetUserInActive(filter, column, isDesc);
                foreach (var user in model)
                {
                    //Loại user admin 
                    if (user.UserName.Equals(CommonConstants.AdminUsername))
                    {
                        model.Remove(user);
                        break;
                    }
                }
                totalRow = model.Count();
                IEnumerable<AppUserViewModel> modelVm = Mapper.Map<IEnumerable<AppUser>, IEnumerable<AppUserViewModel>>(model.Skip((page - 1) * pageSize).Take(pageSize));
                PaginationSet<AppUserViewModel> pagedSet = new PaginationSet<AppUserViewModel>()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    TotalRows = totalRow,
                    Items = modelVm,
                };
                response = request.CreateResponse(HttpStatusCode.OK, pagedSet);
                return response;
            });
        }

        [Route(Common.Constants.RoutesConstant.GetUserOnSite)]
        [HttpPost]
        public async Task<HttpResponseMessage> GetListOnSitePaging(HttpRequestMessage request, string column, bool isDesc, int page, int pageSize, [FromBody]FilterUser filter)
        {
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int totalRow = 0;
                var model = _userService.GetUserOnSite(filter, column, isDesc);
                foreach (var user in model)
                {
                    //Loại user admin 
                    if (user.UserName.Equals(CommonConstants.AdminUsername))
                    {
                        model.Remove(user);
                        break;
                    }
                }
                totalRow = model.Count();
                IEnumerable<AppUserViewModel> modelVm = Mapper.Map<IEnumerable<AppUser>, IEnumerable<AppUserViewModel>>(model.Skip((page - 1) * pageSize).Take(pageSize));
                PaginationSet<AppUserViewModel> pagedSet = new PaginationSet<AppUserViewModel>()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    TotalRows = totalRow,
                    Items = modelVm,
                };
                response = request.CreateResponse(HttpStatusCode.OK, pagedSet);
                return response;
            });
        }

        /// <summary>
        /// View user detail by id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id">UserId of a user</param>
        /// <returns>Return user information</returns>
        [Route(Common.Constants.RoutesConstant.DetailsUser)]
        [HttpGet]
        public async Task<HttpResponseMessage> Details(HttpRequestMessage request, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(id) + Common.Constants.MessageSystem.NoValues);
            }
            if (User != null && MemoryCacheHelper.RemoveUserEditByAdmin(User.Identity.Name))
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, CommonConstants.Error_Edit_By_Admin);
            }
            var user = await _userService.FindUserById(id);
            if (user == null)
            {
                return request.CreateErrorResponse(HttpStatusCode.NoContent, Common.Constants.MessageSystem.NoData);
            }
            else
            {
                var roles = await AppUserManager.GetRolesAsync(user.Id);
                var applicationUserViewModel = Mapper.Map<AppUser, AppUserViewModel>(user);
                var listUserNo = _fingermachineuserService.GetFingerMachineUserByUserID(user.Id).Select(x => x.ID);
                applicationUserViewModel.ListUserNo = string.Join("-", listUserNo);
                applicationUserViewModel.Roles = roles;
                return request.CreateResponse(HttpStatusCode.OK, applicationUserViewModel);
            }
        }

        /// <summary>
        /// Create a new resource (user).
        /// </summary>
        /// <param name="request"></param>
        /// <param name="applicationUserViewModel">Send a resource of user to create new a user</param>
        /// <returns>Return a message success or not</returns>
        [HttpPost]
        [Route(RoutesConstant.AppUsserAdd)]
        public async Task<HttpResponseMessage> Create(HttpRequestMessage request, AppUserViewModel applicationUserViewModel)
        {
            if (ModelState.IsValid)
            {
                if (User != null && MemoryCacheHelper.RemoveUserEditByAdmin(User.Identity.Name))
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, CommonConstants.Error_Edit_By_Admin);
                }
                if (applicationUserViewModel.Roles.Contains(CommonConstants.GroupLead) && applicationUserViewModel.GroupId != null)
                {
                    var groupleaderID = AppRoleManager.FindByNameAsync(CommonConstants.GroupLead).Result.Id;
                    var groupLeader = AppUserManager.Users.Where(x => x.Roles.Any(r => r.RoleId.Equals(groupleaderID) && x.GroupId == applicationUserViewModel.GroupId)).FirstOrDefault();
                    if (groupLeader != null && !groupLeader.Equals(applicationUserViewModel))
                    {
                        return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.Error_Create_Exist_Group_Lead);
                    }
                }

                var username = AppUserManager.FindByNameAsync(applicationUserViewModel.UserName).Result;
                var email = AppUserManager.FindByEmailAsync(applicationUserViewModel.Email).Result;
                //if (username != null && email != null)
                //{
                //    if (username.Status == false)
                //    {
                //        return request.CreateResponse(HttpStatusCode.OK, "Inaction");
                //    }
                //}
                if (username != null)
                {
                    if (username.Status == true)
                    {
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, MessageSystem.MessageDuplicateUserName);
                    }
                }

                if (email != null)
                {
                    if (email.Status == true)
                    {
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, MessageSystem.MessageDuplicateEmail);
                    } else {
                        return request.CreateResponse(HttpStatusCode.OK, "Inaction");
                    }
                }
                var newAppUser = new AppUser();
                newAppUser.UpdateUser(applicationUserViewModel);
                newAppUser.Status = true;
                try
                {
                    newAppUser.Id = Guid.NewGuid().ToString();
                    if (applicationUserViewModel.ListUserNo != null && applicationUserViewModel.ListUserNo.Count() > 0)
                    {
                        if (!ValidateUserNo(applicationUserViewModel.ListUserNo.Split('-')))
                        {
                            return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.MessageUserNoNotValid);
                        }
                        if (_fingermachineuserService.IsUserNoExist(applicationUserViewModel.ListUserNo.Split('-').ToList()))
                        {
                            return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.MessageUserNoExist);
                        }
                    }
                    var checkStartDateAndBirthDay = _userService.CheckStartDateAndBirthDay(applicationUserViewModel.BirthDay, applicationUserViewModel.StartWorkingDay);
                    if (!string.IsNullOrEmpty(checkStartDateAndBirthDay))
                    {
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, checkStartDateAndBirthDay);
                    }
                    var result = await AppUserManager.CreateAsync(newAppUser, applicationUserViewModel.Password);
                    if (result.Succeeded)
                    {
                        var roles = applicationUserViewModel.Roles.ToArray();
                        await AppUserManager.AddToRolesAsync(newAppUser.Id, roles);
                        //Create entitle day of user new
                        _entitleDayAppUserService.CreateEntitleDayAppUser(newAppUser);
                        //Add user into table config delegation
                        var configDelegation = new ConfigDelegation();
                        configDelegation.UserId = newAppUser.Id;
                        _configDelegateionService.Add(configDelegation);
                        _configDelegateionService.SaveChange();

                        if (applicationUserViewModel.ListUserNo != null)
                        {
                            List<string> lstUserno = applicationUserViewModel.ListUserNo.Split('-').ToList();
                            if (lstUserno.Count() > 0)
                            {
                                if (_fingermachineuserService.IsUserNoExist(lstUserno))
                                {
                                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.MessageUserNoExist);
                                }
                                else
                                {
                                    foreach (var item in lstUserno)
                                    {
                                        _fingermachineuserService.Create(new FingerMachineUser() { ID = item, UserId = newAppUser.Id });
                                    }
                                    _unitOfWork.Commit();
                                }
                            }
                        }
                        return request.CreateResponse(HttpStatusCode.OK, applicationUserViewModel);
                    }

                    else
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Join(",", result.Errors));
                }
                catch (NameDuplicatedException dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
                catch (Exception ex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

        }

        /// <summary>
        /// Updating user by a resource of user
        /// </summary>
        /// <param name="request"></param>
        /// <param name="applicationUserViewModel">User infomation has updated</param>
        /// <returns>Return a message success or not</returns>
        [HttpPut]
        [Route(Common.Constants.RoutesConstant.UpdateUser)]
        //[Permission(Action = "Update", Function = "USER")]
        public async Task<HttpResponseMessage> Update(HttpRequestMessage request, AppUserViewModel applicationUserViewModel)
        {
            if (ModelState.IsValid)
            {
                if (User != null && MemoryCacheHelper.RemoveUserEditByAdmin(User.Identity.Name) && User.Identity.Name != applicationUserViewModel.UserName)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, CommonConstants.Error_Edit_By_Admin);
                }

                var appUser = await AppUserManager.FindByIdAsync(applicationUserViewModel.Id);

                if (applicationUserViewModel.Roles.Contains(CommonConstants.GroupLead) && applicationUserViewModel.GroupId != null)
                {
                    var groupleaderID = AppRoleManager.FindByNameAsync(CommonConstants.GroupLead).Result.Id;
                    var groupLeader = AppUserManager.Users.Where(x => x.Roles.Any(r => r.RoleId.Equals(groupleaderID) && x.GroupId == applicationUserViewModel.GroupId)).FirstOrDefault();
                    if (groupLeader != null && !groupLeader.Equals(appUser))
                    {
                        return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.Error_Exist_Group_Lead);
                    }
                }
                var roleIDNew = AppRoleManager.FindByNameAsync(applicationUserViewModel.Roles.FirstOrDefault()).Result.Id;
                if (applicationUserViewModel.UserName == User.Identity.Name && appUser.Roles.FirstOrDefault().RoleId != roleIDNew)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.Unable_Update_Role_YourSelf);
                }
                var checkStartDateAndBirthDay = _userService.CheckStartDateAndBirthDay(applicationUserViewModel.BirthDay, applicationUserViewModel.StartWorkingDay);
                if (!string.IsNullOrEmpty(checkStartDateAndBirthDay))
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, checkStartDateAndBirthDay);
                }
                try
                {
                    var FingerManchineUser = _fingermachineuserService.GetFingerMachineUserByUserID(applicationUserViewModel.Id);
                    var lstUserNoAdd = applicationUserViewModel.ListUserNo.Split('-').ToList().Except(FingerManchineUser.Select(x => x.ID));
                    if (!ValidateUserNo(lstUserNoAdd))
                    {
                        return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.MessageUserNoNotValid);
                    }
                    if (FingerManchineUser.Count > 0)
                    {
                        var lstUserNoRemove = FingerManchineUser.Select(x => x.ID).Except(applicationUserViewModel.ListUserNo.Split('-').ToList());
                        if (FingerManchineUser.Select(x => x.ID).ToList().Count() > 0 && (lstUserNoAdd.Count() > 0 || lstUserNoRemove.Count() > 0))
                        {
                            if (_fingermachineuserService.IsUserNoExist(lstUserNoAdd.ToList()))//_fingermachineuserService.IsFingerManchineUserExist(applicationUserViewModel.AccNameInMachineFinger))
                            {
                                return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.MessageUserNoExist);
                            }
                            if (_fingerTimeSheetService.IsUserNoExistTimeSheet(lstUserNoAdd.ToList()) || _fingerTimeSheetService.IsUserNoExistTimeSheet(lstUserNoRemove.ToList()))
                            {
                                return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.MessageExistTimeSheetEmpNo);
                            }
                            _fingermachineuserService.Update(lstUserNoAdd.ToList(), lstUserNoRemove.ToList(), applicationUserViewModel.Id);
                        }
                    }
                    else
                    {
                        if (_fingermachineuserService.IsUserNoExist(lstUserNoAdd.ToList()))
                            return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.MessageUserNoExist);
                    }
                    if (applicationUserViewModel.ListUserNo.Split('-').Count() > 0&& !string.IsNullOrEmpty(applicationUserViewModel.ListUserNo.Split('-')[0]) && FingerManchineUser.Count == 0)
                    {
                        foreach (var item in applicationUserViewModel.ListUserNo.Split('-').Distinct())
                        {
                            _fingermachineuserService.Create(new FingerMachineUser() { ID = item, UserId = applicationUserViewModel.Id });
                        }
                        _unitOfWork.Commit();
                    }
                    //Update Not Done


                    appUser.UpdateUser(applicationUserViewModel);
                    var result = await AppUserManager.UpdateAsync(appUser);
                    if (result.Succeeded)
                    {
                        if (User.IsInRole(CommonConstants.Admin) && User.Identity.Name != applicationUserViewModel.UserName)
                        {
                            _userService.AddListUserEditByAdmin(applicationUserViewModel.UserName);
                        }
                        var userRoles = await AppUserManager.GetRolesAsync(appUser.Id);
                        var selectedRole = applicationUserViewModel.Roles.ToArray();
                        selectedRole = selectedRole ?? new string[] { };
                        await AppUserManager.RemoveFromRolesAsync(appUser.Id, userRoles.ToArray());
                        await AppUserManager.AddToRolesAsync(appUser.Id, selectedRole.ToArray());
                        return request.CreateResponse(HttpStatusCode.OK, applicationUserViewModel);
                    }
                    return request.CreateResponse(HttpStatusCode.BadRequest, Common.Constants.MessageSystem.MessageDuplicateEmail);
                }
                catch (NameDuplicatedException dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
            }
            return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }
        private bool ValidateUserNo(IEnumerable<string> lstUserNo)
        {
            if (lstUserNo.Count() != lstUserNo.Distinct().Count())
                return false;
            foreach (var userNo in lstUserNo)
            {
                if (string.IsNullOrEmpty(userNo) || string.IsNullOrWhiteSpace(userNo))
                    continue;
                try
                {
                    Int32.Parse(userNo);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// User edit profile
        /// </summary>
        /// <param name="request"></param>
        /// <param name="applicationUserViewModel">User infomation has updated</param>
        /// <returns>Return a message success or not</returns>
        [HttpPut]
        [Route(Common.Constants.RoutesConstant.UpdateUserProfile)]
        //[Permission(Action = "Update", Function = "USER")]
        public async Task<HttpResponseMessage> UpdateProfile(HttpRequestMessage request, AppUserViewModel applicationUserViewModel)
        {
            if (ModelState.IsValid)
            {
                var appUser = await AppUserManager.FindByIdAsync(applicationUserViewModel.Id);
                try
                {
                    appUser.UpdateUser(applicationUserViewModel);
                    var result = await AppUserManager.UpdateAsync(appUser);
                    if (result.Succeeded)
                    {
                        var userRoles = await AppUserManager.GetRolesAsync(appUser.Id);
                        var selectedRole = applicationUserViewModel.Roles.ToArray();
                        selectedRole = selectedRole ?? new string[] { };
                        await AppUserManager.RemoveFromRolesAsync(appUser.Id, userRoles.ToArray());
                        await AppUserManager.AddToRolesAsync(appUser.Id, selectedRole.ToArray());
                        return request.CreateResponse(HttpStatusCode.OK, applicationUserViewModel);
                    }
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, Common.Constants.MessageSystem.MessageDuplicateEmail);
                }
                catch (NameDuplicatedException dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
            }
            return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }
        /// <summary>
        /// Deleting a user by id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id">UserId of a user</param>
        /// <returns>Return a message success or not</returns>
        [HttpDelete]
        [Route("delete")]
        [Permission(Action = "Delete", Function = "USER")]
        //[Authorize(Roles ="DeleteUser")]
        public async Task<HttpResponseMessage> Delete(HttpRequestMessage request, string id)
        {
            if (User != null && MemoryCacheHelper.RemoveUserEditByAdmin(User.Identity.Name))
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, CommonConstants.Error_Edit_By_Admin);
            }
            var appUser = await AppUserManager.FindByIdAsync(id);
            var result = await AppUserManager.DeleteAsync(appUser);
            if (result.Succeeded)
                return request.CreateResponse(HttpStatusCode.OK, id);
            else
                return request.CreateErrorResponse(HttpStatusCode.OK, string.Join(",", result.Errors));
        }

        [HttpPost]
        [Route("resetPassword")]
        [Permission(Action = "Read", Function = "USER")]
        public async Task<HttpResponseMessage> ResetPassword(HttpRequestMessage request, AppUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userEmail = await AppUserManager.FindByEmailAsync(model.Email);
                if (userEmail == null)
                    return request.CreateErrorResponse(HttpStatusCode.NoContent, "Email không tồn tại");
                //Send an email with link
                string sendLink = await AppUserManager.GeneratePasswordResetTokenAsync(userEmail.Id);
                await AppUserManager.SendEmailAsync(userEmail.Id, "Reset password", "Click here ");
                return request.CreateResponse(HttpStatusCode.OK, sendLink);
            }
            else
                return request.CreateErrorResponse(HttpStatusCode.NoContent, "Models không thỏa mãn yêu cầu");
        }
        [HttpGet]
        [Route(RoutesConstant.GetUserByGroup)]
        public async Task<HttpResponseMessage> GetUserByGroup(HttpRequestMessage request, int groupID)
        {
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var model = AppUserManager.Users.Select(x => new { x.Id, x.FullName, x.UserName, x.Email , x.GroupId, x.Status}).Where(x => x.GroupId == groupID && x.Status == true);
                response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            });
        }
        [HttpGet]
        [Route(RoutesConstant.GetAllUserNameByUser)]
        public async Task<HttpResponseMessage> GetAllUserName(HttpRequestMessage request)
        {
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var model = AppUserManager.Users;
                response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            });
        }
        [Route(RoutesConstant.GetUserByGroup)]
        public async Task<HttpResponseMessage> GetUserAllRequest(HttpRequestMessage request)
        {
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var user = AppUserManager.Users.Select(x => new { x.Id, x.FullName, x.Group.Name, x.UserName, x.Status }).Where(x => x.Name != "SuperAdmin" && x.Status == true).Distinct().ToList();
                response = request.CreateResponse(HttpStatusCode.OK, user);
                return response;
            });
        }
        [HttpGet]
        [Route(RoutesConstant.GetUserByDelegate)]
        public async Task<HttpResponseMessage> GetAllUserByDelegate(HttpRequestMessage request)
        {
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var model = AppUserManager.Users;
                response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            });
        }
        [HttpGet]
        [Route(RoutesConstant.GetGroupLeadByGroupId)]
        public async Task<HttpResponseMessage> GetGroupLeadByGroup(HttpRequestMessage request, int groupId)
        {
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var groupleaderID = AppRoleManager.FindByNameAsync(CommonConstants.GroupLead).Result.Id;
                var user = AppUserManager.Users.Where(x => x.Roles.Any(r => r.RoleId.Equals(groupleaderID)) && x.GroupId == groupId).ToList();
                response = request.CreateResponse(HttpStatusCode.OK, Mapper.Map<List<AppUser>, List<AppUserViewModel>>(user));
                return response;
            });
        }

        [HttpGet]
        [Route(RoutesConstant.GetGroupLeadToAssign)]
        public async Task<HttpResponseMessage> GetGroupLeadToAssign(HttpRequestMessage request)
        {
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var groupleaderID = AppRoleManager.FindByNameAsync(CommonConstants.GroupLead).Result.Id;
                var user = AppUserManager.Users.Where(x => x.Roles.Any(r => r.RoleId.Equals(groupleaderID) && x.GroupId == null)).ToList();
                response = request.CreateResponse(HttpStatusCode.OK, Mapper.Map<List<AppUser>, List<AppUserViewModel>>(user));
                return response;
            });

        }
        [HttpGet]
        [Route(RoutesConstant.GetAllUser)]
        public async Task<HttpResponseMessage> GetUserFullName(HttpRequestMessage request)
        {
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var user = AppUserManager.Users.Select(x => x.FullName).Distinct().ToList();
                response = request.CreateResponse(HttpStatusCode.OK, user);
                return response;
            });

        }

        [HttpGet]
        [Route(RoutesConstant.GetAllAppUser)]
        public async Task<HttpResponseMessage> GetAllAppUser(HttpRequestMessage request, string userlogin, int groupId)
        {
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                var groupleaderID = AppRoleManager.FindByNameAsync(CommonConstants.GroupLead).Result.Id;
                var user = AppUserManager.Users.Where(x => x.Id != userlogin && !(x.Roles.Any(r => r.RoleId.Equals(groupleaderID)) && x.GroupId == groupId)).Distinct().ToList();
                //var user = AppUserManager.Users.Select(x => new { x.Id, x.FullName, x.Email, x.Roles, x.GroupId }).Where(x => x.Id != userlogin && x.Roles.Any(r=> r.RoleId.Equals(groupleaderID)) && x.GroupId == groupId).Distinct().ToList();
                response = request.CreateResponse(HttpStatusCode.OK, user);
                return response;
            });
        }
        /// <summary>
        /// Change Status Multi Select
        /// </summary>
        /// <param name="request"></param>
        /// <param name="listUser"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(RoutesConstant.ChangeStatusUserMulti)]
        public async Task<HttpResponseMessage> ChangeStatusUserMulti(HttpRequestMessage request, [FromBody]string[] listUser)
        {
            if (User != null && MemoryCacheHelper.RemoveUserEditByAdmin(User.Identity.Name))
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, CommonConstants.Error_Edit_By_Admin);
            }
            foreach (var item in listUser)
            {
                var appUser = await AppUserManager.FindByIdAsync(item);
                appUser.Status = !appUser.Status;
                var result = await AppUserManager.UpdateAsync(appUser);
                _userService.AddListUserEditByAdmin(appUser.UserName);
            }
            return request.CreateResponse(HttpStatusCode.OK, true);
        }
        /// <summary>
        /// Change Status Single
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(RoutesConstant.ChangeStatusUser)]
        public async Task<HttpResponseMessage> ChangeStatusUser(HttpRequestMessage request, string userId)
        {
            if (User != null && MemoryCacheHelper.RemoveUserEditByAdmin(User.Identity.Name))
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, CommonConstants.Error_Edit_By_Admin);
            }
            var appUser = await AppUserManager.FindByIdAsync(userId);
            if (appUser.Status==false)
            {
                appUser.ResignationDate = null;
            }
            appUser.Status = !appUser.Status;
            var result = await AppUserManager.UpdateAsync(appUser);
            _userService.AddListUserEditByAdmin(appUser.UserName);
            return request.CreateResponse(HttpStatusCode.OK, result);
        }
        [HttpGet]
        [Route(RoutesConstant.Resign)]
        public async Task<HttpResponseMessage> Resign(HttpRequestMessage request, string userId,DateTime resignationDate)
        {
            if (User != null && MemoryCacheHelper.RemoveUserEditByAdmin(User.Identity.Name))
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, CommonConstants.Error_Edit_By_Admin);
            }
            var appUser = await AppUserManager.FindByIdAsync(userId);
            appUser.Status = false;
            appUser.ResignationDate = resignationDate.Date;
            var result = await AppUserManager.UpdateAsync(appUser);
            _userService.AddListUserEditByAdmin(appUser.UserName);
            if (result.Succeeded)
                return request.CreateResponse(HttpStatusCode.OK, result);
            else
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, "Cannot update user resign !");
        }
        /// <summary>
        /// Get Finger User Id By User Id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RoutesConstant.GetFingerUserIdByUserID)]
        public HttpResponseMessage GetFingerUserIdByUserId(HttpRequestMessage request, string userId)
        {
            var model = _fingermachineuserService.GetFingerMachineUserByUserID(userId);
            return request.CreateResponse(HttpStatusCode.OK, model);
        }

        /// <summary>
        /// Count total member
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(RoutesConstant.GetTotalUser)]
        public HttpResponseMessage GetTotalUser(HttpRequestMessage request)
        {
            var model = _userService.CountUser();
            return request.CreateResponse(HttpStatusCode.OK, model);
        }
        [HttpPost]
        [Route(RoutesConstant.GetUserByAll)]
        public HttpResponseMessage GetAllUser(HttpRequestMessage request)
        {
            var model = _userService.GetAll();
            return request.CreateResponse(HttpStatusCode.OK, model);
        }

        [HttpGet]
        [Route(RoutesConstant.GetAllUserSuperAdmin)]
        public async Task<HttpResponseMessage> GetUserBySuperAdmin(HttpRequestMessage request)
        {
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var model = AppUserManager.Users;
                response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            });
        }

        //[Route(RoutesConstant.GetUserAccount)]
        //public HttpResponseMessage GetUserAllOTRequest(HttpRequestMessage request, string groupID)
        //{
        //    return await CreateHttpResponse(request, () =>
        //    {
        //        HttpResponseMessage response = null;
        //        //var model = _otrequestRepository.GetMulti(x => x.AppUserCreatedBy.GroupId.ToString().Equals(groupID), new string[] { CommonConstants.AppUserCreateByGroup }).Select(x => x.AppUserCreatedBy.FullName).Distinct();
        //        //var user = AppUserManager.Users.Select(x => new { x.Id, x.FullName, x.Group.Name, x.UserName }).Distinct().ToList();
        //        //var user = AppUserManager.Users.Select(x => new { x.Id, x.FullName, x.Group.Name, x.UserName }).Where(x => x.).Distinct().ToList();
        //        response = request.CreateResponse(HttpStatusCode.OK, user);
        //        return response;
        //    });
        //}

        //[Route(RoutesConstant.GetUserAccount)]
        //public HttpResponseMessage GetUserAccount(HttpRequestMessage request)
        //{
        //    return await CreateHttpResponse(request, () =>
        //    {
        //        HttpResponseMessage response = null;
        //        var user = AppUserManager.Users.Select(x => new { x.Id, x.FullName, x.UserName }).Distinct().ToList();
        //        foreach (var item in user)
        //        {
        //            var FullNameAccount = item.FullName + item.UserName;
        //        }
        //        response = request.CreateResponse(HttpStatusCode.OK, user);
        //        return response;
        //    });
        //}
        [HttpGet]
        [Route(RoutesConstant.GetUserAllRequestByRoleAdmin)]
        public async Task<HttpResponseMessage> GetUserAllRequestByRoleAdmin(HttpRequestMessage request)
        {
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var user = AppUserManager.Users.Select(x => new { x.Id, x.FullName, x.Group.Name, x.UserName }).Where(x => x.Name != "SuperAdmin").Distinct().ToList();
                response = request.CreateResponse(HttpStatusCode.OK, user);
                return response;
            });
        }
        [HttpGet]
        [Route(RoutesConstant.GetAllStatus)]
        public async Task<HttpResponseMessage> GetAllStatus (HttpRequestMessage request)
        {
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var user = AppUserManager.Users.Select(x => new {x.Status }).Distinct().ToList();
                response = request.CreateResponse(HttpStatusCode.OK, user);
                return response;
            });
        }
        [HttpDelete]
        [Route(RoutesConstant.UpdateUserOld)]
        public async Task<HttpResponseMessage> CheckUser(HttpRequestMessage request, string id)
        {
            //var username = AppUserManager.FindByNameAsync(id).Result;
            var username = AppUserManager.FindByEmailAsync(id).Result;
            if (username != null)
            {
                var rolesForUser = await AppUserManager.GetRolesAsync(username.Id);
                if (rolesForUser.Count() > 0)
                {
                    foreach (var item in rolesForUser.ToList())
                    {
                        // item should be the name of the role
                        var result = await AppUserManager.RemoveFromRoleAsync(username.Id, item);
                    }
                }
                _userService.DeleteTableUser(username.Id);
                _unitOfWork.Commit();
                return request.CreateResponse(HttpStatusCode.OK, Common.Constants.MessageSystem.ChangePasswordSuccess);
            }
            return request.CreateResponse(HttpStatusCode.OK);
        }
        
    }
}