using System;
using System.Collections.Generic;
using System.Linq;
using TMS.Common.Constants;
using TMS.Common.Exceptions.Extensions;
using TMS.Common.ViewModels;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Model.Models;


namespace TMS.Service
{
    public interface IEntitleDayService
    {
        List<EntitledayModel> GetAllOTFilter(string userID, string groupID, string column, bool isDesc, FilterEntitleDay filter);
        IEnumerable<EntitleDay> GetAllType();
        IEnumerable<EntitleDay> GetAllTypeUser(string UserID);
        void Add(EntitleDay entitleDay);
        IEnumerable<EntitleDay> GetAllTypeUserFilter(string UserID);
        //IEnumerable<Entitleday_AppUser> GetAll();
        //void UpdateEntitleDay();
        //IEnumerable<EntitledayModel> GetById(int id);
    }

    public class EntitleDayService : IEntitleDayService
    {
        private IEntitleDayRepository _entitleDayRepositoty;
        private IUserService _userService;
        private IEntitleDayAppUserRepository _entitleDayAppUserRepository;
        private IAppUserRepository _appUserRepository;
        private IUnitOfWork _unitOfWork;

        public EntitleDayService(IAppUserRepository appUserRepository, IUserService userService, IEntitleDayAppUserRepository entitleDayAppUserRepository, IEntitleDayRepository entitleDayService, IUnitOfWork unitOfWork)
        {
            _entitleDayRepositoty = entitleDayService;
            _entitleDayAppUserRepository = entitleDayAppUserRepository;
            _unitOfWork = unitOfWork;
            _userService = userService;
            _appUserRepository = appUserRepository;
        }
        /// <summary>
        /// Function Get All List Entitle Day
        /// Paging & Sort
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        /// 

        public IEnumerable<EntitledayModel> GetAllWithUser(string userID, string groupID, FilterEntitleDay filter)
        {
            if (_entitleDayRepositoty.IsReadAll(userID, CommonConstants.FunctionEntitleDay))
            {
                return _entitleDayRepositoty.GetAllEntitleDay(userID, groupID, true).ToList();
            }
            return _entitleDayRepositoty.GetAllEntitleDay(userID, groupID, false);
        }
        //public IEnumerable<EntitledayModel> GetById(int id)
        //{
        //    var model = _entitleDayRepositoty.GetById(id);
        //    return model;
        //}
        //public IEnumerable<Entitleday_AppUser> GetAll()
        //{
        //    return _entitleDayAppUserRepository.GetAll();
        //}

        public List<EntitledayModel> GetAllOTFilter(string userID, string groupID, string column, bool isDesc, FilterEntitleDay filter)
        {
            var model = GetAllWithUser(userID, groupID, filter);
            if (filter != null)
            {
                if (filter.FullName.Count() != 0)
                {
                    model = model.Where(x => filter.FullName.Contains(x.IDUser));
                }
                if (filter.HolidayType.Count() != 0)
                {
                    model = model.Where(x => filter.HolidayType.Contains(x.IDEntitleday.ToString()));
                }
            }
            return model.OrderByField(column, isDesc).ToList();
        }
        // Get All Entitle Day of User
        public IEnumerable<EntitleDay> GetAllType()
        {
            return _entitleDayRepositoty.GetAll();
        }
        public IEnumerable<EntitleDay> GetAllTypeUser(string UserID)
        {
            var modelUser = _appUserRepository.GetMulti(x => x.Id == UserID).FirstOrDefault();
            if (modelUser.Gender == true)
            {
                return _entitleDayRepositoty.GetMulti(x => x.ID != 3);
            }
            else
            {
                return _entitleDayRepositoty.GetMulti(x => x.ID != 4);
            }
        }
        public IEnumerable<EntitleDay> GetAllTypeUserFilter(string UserID)
        {

            var modelUser = _appUserRepository.GetMulti(x => x.Id == UserID).FirstOrDefault();
            if (modelUser.Gender == true)
            {
                return _entitleDayRepositoty.GetMulti(x => x.ID != 3);
            }
            else
            {
                return _entitleDayRepositoty.GetMulti(x => x.ID != 4);
            }

            //var modelUser = _appUserRepository.GetMulti(x => x.Id == UserID).FirstOrDefault();
            //if (true)
            //{

            //}
            //else
            //{
            //    return _entitleDayRepositoty.GetAll();

            //}
        }
        /// <summary>
        /// Add Entitle Day
        /// </summary>
        /// <param name="entitleDay"></param>
        public void Add(EntitleDay entitleDay)
        {
            var model = _userService.GetAll();
            foreach (var item in model)
            {
                Entitleday_AppUser entitleday_AppUser = new Entitleday_AppUser();
                entitleday_AppUser.UserId = item.Id;
                entitleday_AppUser.EntitleDayId = entitleDay.ID;
                entitleday_AppUser.MaxEntitleDayAppUser = entitleDay.MaxEntitleDay;
                _entitleDayAppUserRepository.Add(entitleday_AppUser);
            }
            _unitOfWork.Commit();
        }
        public void AddEntitleDayUser(EntitleDay entitleDay)
        {
            var model = _entitleDayAppUserRepository.GetAll();
            foreach (var item in model)
            {
                AppUser appUser = new AppUser();
                Entitleday_AppUser entitleday_AppUser = new Entitleday_AppUser();
                entitleday_AppUser.UserId = appUser.Id;
                entitleday_AppUser.EntitleDayId = entitleDay.ID;
                _entitleDayAppUserRepository.Add(entitleday_AppUser);
            }
        }
    }
}
