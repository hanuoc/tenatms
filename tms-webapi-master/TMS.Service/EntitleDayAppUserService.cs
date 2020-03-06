using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Common.Constants;
using TMS.Common.ViewModels;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Model.Models;

namespace TMS.Service
{
    public interface IEntitleDayAppUserService
    {
        IEnumerable<Entitleday_AppUser> GetAll(string[] includes = null);
        void UpdateEntitleDay(DateTime dateJobExcute);
        void CreateEntitleDayAppUser(AppUser appUser);
        //void UpdateEntitleDay(Entitleday_AppUser entitleday_AppUser);
        //IEnumerable<Entitleday_AppUser> GetByID(int id);
        IEnumerable<EntitledayModel> GetModelById(int id);
        Entitleday_AppUser GetById(int id);
        void UpdateEntitleDayAppUser(Entitleday_AppUser entitleday_AppUser);
        void Update(Entitleday_AppUser entitleday_AppUser);
        void Save();
        void Delete(int id);
    }
    public class EntitleDayAppUserService : IEntitleDayAppUserService
    {
        private IEntitleDayAppUserRepository _entitleDayAppUserRepository;
        private IUnitOfWork _unitOfWork;
        private IEntitleDayService _entitleDayService;
        private IEntitleDayRepository _entitleDayRepository;

        public EntitleDayAppUserService(IEntitleDayRepository entitleDayRepository, IEntitleDayService entitleDayService, IEntitleDayAppUserRepository entitleDayAppUserRepository, IUnitOfWork unitOfWork)
        {
            _entitleDayAppUserRepository = entitleDayAppUserRepository;
            _unitOfWork = unitOfWork;
            _entitleDayService = entitleDayService;
            _entitleDayRepository = entitleDayRepository;
        }
        // Get All table EntitleDay AppUser
        public IEnumerable<Entitleday_AppUser> GetAll(string [] includes = null)
        {
            return _entitleDayAppUserRepository.GetAll(includes);
        }
        // Check tháng 4 reset Number Day Off
        public void UpdateEntitleDay(DateTime dateJobExcute)
        {
            var model = GetAll(new string[] { CommonConstants.EntitleDay }).Where(x=>x.EntitleDay.UnitType == CommonConstants.Day);
            var datenow = dateJobExcute.ToString(CommonConstants.dateNowStartEntitleDay);
            var entitleday = _entitleDayService.GetAllType();
            if (datenow == CommonConstants.dateStartEntitleDay)
            {
                foreach (var item in model)
                {
                    foreach (var entitle in entitleday)
                    {
                        if (entitle.ID == item.EntitleDayId)
                        {
                            item.MaxEntitleDayAppUser = entitle.MaxEntitleDay;
                            item.NumberDayOff = CommonConstants.ZERO;
                            item.TemporaryMaxEntitleDay = CommonConstants.ZERO;
                            _entitleDayAppUserRepository.Update(item);
                        }
                    }            
                }
            }
            _unitOfWork.Commit();
        }
        //Create entitle day of user new
        public void CreateEntitleDayAppUser(AppUser appUser)
        {
            var model = _entitleDayService.GetAllType();
            foreach (var item in model)
            {
                Entitleday_AppUser entitleday_AppUser = new Entitleday_AppUser();
                entitleday_AppUser.UserId = appUser.Id;
                entitleday_AppUser.EntitleDayId = item.ID;
                if (item.HolidayType == "Authorized Leave")
                {
                    entitleday_AppUser.MaxEntitleDayAppUser = 0;
                }
                else
                {
                    entitleday_AppUser.MaxEntitleDayAppUser = item.MaxEntitleDay;
                }
                _entitleDayAppUserRepository.Add(entitleday_AppUser);
            }
            _unitOfWork.Commit();
        }
        //public IEnumerable<Entitleday_AppUser> GetByID(int id)
        //{
        //    var model = _entitleDayAppUserRepository.GetMulti(x => x.ID.Equals(id));
        //    return model;
        //}
        public IEnumerable<EntitledayModel> GetModelById(int id)
        {
            var model = _entitleDayRepository.GetById(id);
            return model;
        }
        public Entitleday_AppUser GetById(int id)
        {
            var model = _entitleDayAppUserRepository.GetSingleById(id);
            return model;
        }

        public void UpdateEntitleDayAppUser(Entitleday_AppUser entitleday_AppUser)
        {
            _entitleDayAppUserRepository.Update(entitleday_AppUser);
            _unitOfWork.Commit();
        }
        public void Update(Entitleday_AppUser entitleday_AppUser)
        {
            _entitleDayAppUserRepository.Update(entitleday_AppUser);
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public void Delete(int id)
        {
            _entitleDayAppUserRepository.Delete(id);
        }
    }
}
