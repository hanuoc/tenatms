using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Model.Models;

namespace TMS.Service
{
    public interface IUserOnsiteService
    {
        UserOnsite Add(UserOnsite entity);
        void Update(UserOnsite entity);
        UserOnsite Delete(int id);
        IEnumerable<UserOnsite> GetUserOnsite(string userID);
        bool IsValidDuration(UserOnsite userOnsite, bool isUpdate);
    }
    public class UserOnsiteService : IUserOnsiteService
    {
        IUserOnsiteRepository _userOnsiteRepository;
        private IUnitOfWork _unitOfWork;
        public UserOnsiteService(IUserOnsiteRepository userOnsiteRepository, IUnitOfWork unitOfWork)
        {
            this._userOnsiteRepository = userOnsiteRepository;
            this._unitOfWork = unitOfWork;
        }

        public UserOnsite Add(UserOnsite entity)
        {
            entity.StartDate = entity.StartDate.Value.Date;
            entity.EndDate = entity.EndDate.Value.Date;
            var userOnsiteInfo = _userOnsiteRepository.Add(entity);
            _unitOfWork.Commit();
            return userOnsiteInfo;
        }

        public UserOnsite Delete(int id)
        {
            var userOnsite = _userOnsiteRepository.Delete(id);
            _unitOfWork.Commit();
            return userOnsite;
        }

        public void Update(UserOnsite entity)
        {
            var userOnsiteInfo = _userOnsiteRepository.GetSingleById(entity.ID);
            userOnsiteInfo.ID = entity.ID;
            userOnsiteInfo.OnsitePlace = entity.OnsitePlace;
            userOnsiteInfo.StartDate = entity.StartDate.Value.Date;
            userOnsiteInfo.EndDate = entity.EndDate.Value.Date;
            _userOnsiteRepository.Update(userOnsiteInfo);
            _unitOfWork.Commit();
        }

        public List<UserOnsite> GetAllUserOnsite()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserOnsite> GetUserOnsite(string userID)
        {
            return _userOnsiteRepository.GetUserOnsite(userID);
        }

        public bool IsValidDuration(UserOnsite userOnsite, bool isUpdate)
        {
            var lstEntity = _userOnsiteRepository.GetMulti(x => x.UserID == userOnsite.UserID && !(userOnsite.StartDate > x.EndDate || userOnsite.EndDate < x.StartDate));
            foreach (var entity in lstEntity)
            {
                if (!(isUpdate && entity.ID == userOnsite.ID))
                {
                    return false;
                }
            }
            return true;

        }
    }
}
