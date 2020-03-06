using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Common.Constants;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Model.Models;

namespace TMS.Service
{
    public interface IChildcareLeaveService
    {
        ChildcareLeave Add(ChildcareLeave childcareLeave, string userID);
        bool Update(ChildcareLeave childcareLeave, string userID);
        ChildcareLeave GetById(int id);
        ChildcareLeave Delete(int id);
    }
    public class ChildcareLeaveService : IChildcareLeaveService
    {
        private IChildcareLeaveRepository _ChildcareLeaveRepository;
        private IAppUserRepository _appUserRepository;
        private IUnitOfWork _unitOfWork;

        public ChildcareLeaveService(IChildcareLeaveRepository ChildcareLeaveRepository, IAppUserRepository appUserRepository, IUnitOfWork unitOfWork)
        {
            this._appUserRepository = appUserRepository;
            this._ChildcareLeaveRepository = ChildcareLeaveRepository;
            this._unitOfWork = unitOfWork;
        }
        public ChildcareLeave Add(ChildcareLeave childcareLeave, string userID)
        {
            childcareLeave.UserId = userID;
            childcareLeave.StartDate= DateTime.ParseExact(childcareLeave.StartDate.ToString(CommonConstants.FormatDate_DDMMYYY, CultureInfo.InvariantCulture), CommonConstants.FormatDate_DDMMYYY, CultureInfo.InvariantCulture);
            childcareLeave.EndDate = DateTime.ParseExact(childcareLeave.EndDate.ToString(CommonConstants.FormatDate_DDMMYYY, CultureInfo.InvariantCulture), CommonConstants.FormatDate_DDMMYYY, CultureInfo.InvariantCulture);
            var result= _ChildcareLeaveRepository.Add(childcareLeave);
            if(result != null)
            {
                _unitOfWork.Commit();
                var user=_appUserRepository.GetSingleByCondition(x => x.Id == userID);
                var _childcareLeave = _ChildcareLeaveRepository.GetSingleByCondition(x => x.UserId == userID);
                if (user != null&&user.ChildcareLeaveID == null&& _childcareLeave!=null)
                {
                    user.ChildcareLeaveID = _childcareLeave.ID;
                    _appUserRepository.Update(user);
                    _unitOfWork.Commit();
                }
                else
                {
                    return null;
                }
            }
            return result;
        }

        public bool Update(ChildcareLeave childcareLeave, string userID)
        {
            var entity= _ChildcareLeaveRepository.GetSingleByCondition(x => x.UserId == userID);
            if (entity != null)
            {
                entity.IsEarlyLeaving = childcareLeave.IsEarlyLeaving;
                entity.IsLateComing = childcareLeave.IsLateComing;
                entity.StartDate = DateTime.ParseExact(childcareLeave.StartDate.ToString(CommonConstants.FormatDate_DDMMYYY, CultureInfo.InvariantCulture), CommonConstants.FormatDate_DDMMYYY, CultureInfo.InvariantCulture);
                entity.EndDate = DateTime.ParseExact(childcareLeave.EndDate.ToString(CommonConstants.FormatDate_DDMMYYY, CultureInfo.InvariantCulture), CommonConstants.FormatDate_DDMMYYY, CultureInfo.InvariantCulture);
                entity.Time = childcareLeave.Time;
                _ChildcareLeaveRepository.Update(entity);
                _unitOfWork.Commit();
                return true;
            }
            else
            {
                return false;
            }
        }

        public ChildcareLeave GetById(int id)
        {
            return _ChildcareLeaveRepository.GetSingleByCondition(x => x.ID == id);
        }
        public ChildcareLeave Delete(int id)
        {
            var result = _ChildcareLeaveRepository.Delete(id);
            if (result != null)
            {
                var user= _appUserRepository.GetSingleByCondition(x => x.ChildcareLeaveID == id);
                if(user!=null)
                {
                    user.ChildcareLeaveID = null;
                    _appUserRepository.Update(user);
                    _unitOfWork.Commit();
                }
                return result;
            }
            return null;
        }
    }
}
