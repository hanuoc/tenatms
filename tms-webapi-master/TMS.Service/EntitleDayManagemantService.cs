using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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
    public interface IEntitleDayManagemantService
    {
        IEnumerable<EntitleDay> GetAllEntitleDayManagement();
        EntitleDay Add(EntitleDay entitleDay);
        void Update(EntitleDay entitleDay);
        EntitleDay Delete(int id);
        EntitleDay GetByIdEntitleDay(int id);
        bool CheckHolidayType(string holidayType, int id);
        void SaveChange();
    }
    public class EntitleDayManagemantService : IEntitleDayManagemantService
    {
        private IEntitleDayManagementRepository _entitleDayManagementRepository;
        private IUnitOfWork _unitOfWork;
        private IRequestService _requestService;
        public EntitleDayManagemantService(IRequestService requestService, IEntitleDayManagementRepository entitleDayManagementRepository, IUnitOfWork unitOfWork)
        {
            this._entitleDayManagementRepository = entitleDayManagementRepository;
            this._unitOfWork = unitOfWork;
            _requestService = requestService;
        }

        public IEnumerable<EntitleDay> GetAllEntitleDayManagement()
        {
            return _entitleDayManagementRepository.GetMulti(x => x.Status == true);
        }
        /// <summary>
        /// Function Get by ID Entitle Day Management
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EntitleDay GetByIdEntitleDay(int id)
        {
            return _entitleDayManagementRepository.GetSingleById(id);
        }
        /// <summary>
        /// Function Create Entitle Day Management
        /// </summary>
        /// <param name="entitleDay"></param>
        /// <returns></returns>
        public EntitleDay Add(EntitleDay entitleDay)
        {
            try
            {
                var EntitleDay = _entitleDayManagementRepository.Add(entitleDay);
                SaveChange();
                return EntitleDay;
            }
            catch (DbUpdateException e)
            {
                return null;
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
        }
        /// <summary>
        /// Function Update Entitle Day Management
        /// </summary>
        /// <param name="entitleDay"></param>
        public void Update(EntitleDay entitleDay)
        {
            _entitleDayManagementRepository.Update(entitleDay);
            SaveChange();
        }
        /// <summary>
        /// Update Entitle Day ID (Request)
        /// Function Delete Entitle Day Management
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EntitleDay Delete(int id)
        {
            try
            {
                _requestService.UpdateEntitleDayID(id);
                var entitleDayId = GetByIdEntitleDay(id);
                var EntitleDay = _entitleDayManagementRepository.Delete(entitleDayId);
                SaveChange();
                return EntitleDay;
            }
            catch (DbUpdateException e)
            {
                return null;
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
        }
        /// <summary>
        /// Function Save
        /// </summary>
        public void SaveChange()
        {
            _unitOfWork.Commit();
        }
        /// <summary>
        /// Function Check Holiday Type of Entitle Day management
        /// </summary>
        /// <param name="holidayType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckHolidayType(string holidayType, int id)
        {
            var _holidayType = _entitleDayManagementRepository.GetSingleByCondition(x => x.HolidayType == holidayType);
            if (_holidayType == null)
                return false;
            else
            {
                if (_holidayType.ID == id)
                    return false;
            }
            return true;
        }
    }
}
