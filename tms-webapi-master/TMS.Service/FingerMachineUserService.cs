using System;
using System.Collections.Generic;
using System.Linq;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Model.Models;

namespace TMS.Service
{
    public interface IFingerMachineUserService
    {
        FingerMachineUser GetFingerMachineUserById(string UserNo);
        List<FingerMachineUser> GetFingerMachineUserByUserID(string UserId);
        ///// <summary>
        ///// Check if user No whether exist in Finger Manchine User By User No
        ///// </summary>
        ///// <param name="UserNo"></param>
        ///// <returns></returns>
        //bool IsFingerManchineUserExist(string AccName);
        bool IsUserNoExist(List<string> lstUserNo);
        void Update(List<string>lstUserNoAdd, List<string> lstUserNoRemove, string UserID);
        void Create(FingerMachineUser fingerMachine);
        /// <summary>
        /// Check if user No whether exist in Finger Manchine User By UserID
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        //bool IsExistUserNo(string UserID);
    }

    public class FingerMachineUserService : IFingerMachineUserService
    {
        private IFingerMachineUserRepository _fingerMachineUserRepository;
        private IFingerTimeSheetRepository _fingerTimeSheetRepository;
        private IAbnormalCaseRepository _abnormalCaseRepository;
        private IUnitOfWork _unitOfWork;

        public FingerMachineUserService(IFingerMachineUserRepository fingerMachineUserRepository, IFingerTimeSheetRepository fingerTimeSheetRepository, IAbnormalCaseRepository abnormalCaseRepository, IUnitOfWork unitOfWork)
        {
            this._fingerTimeSheetRepository = fingerTimeSheetRepository;
            this._abnormalCaseRepository = abnormalCaseRepository;
            this._fingerMachineUserRepository = fingerMachineUserRepository;
            this._unitOfWork = unitOfWork;
        }

        public FingerMachineUser GetFingerMachineUserById(string UserNo)
        {
                return _fingerMachineUserRepository.GetMulti(x => x.ID == UserNo).FirstOrDefault();
        }
        public List<FingerMachineUser> GetFingerMachineUserByUserID(string UserId)
        {
            return _fingerMachineUserRepository.GetMulti(x => x.UserId == UserId).ToList();//.FirstOrDefault();
        }
        /// <summary>
        /// Check if user No whether exist in Finger Manchine User By User No
        /// </summary>
        /// <param name="UserNo"></param>
        /// <returns></returns>
        public bool IsFingerManchineUserExist(string userID)
        {
            return _fingerMachineUserRepository.GetMulti(x => x.UserId == userID).Count() > 0;
        }
        public void Update(List<string> lstUserNoAdd, List<string> lstUserNoRemove, string UserID)
        {
            var model = GetFingerMachineUserByUserID(UserID);
            foreach (var item in lstUserNoAdd)
            {
                if (string.IsNullOrEmpty(item))
                    continue;
                Create(new FingerMachineUser() { ID = item, UserId = UserID });
            }
            foreach (var item in lstUserNoRemove)
            {
                _fingerMachineUserRepository.Delete(_fingerMachineUserRepository.GetSingleByCondition(x => x.ID == item));
            }
            _unitOfWork.Commit();
        }
        public void Delete(FingerMachineUser fingermachineUser)
        {
            _fingerMachineUserRepository.Delete(fingermachineUser);
        }
        public void Create(FingerMachineUser fingerMachine)
        {
            _fingerMachineUserRepository.Add(fingerMachine);
        }
        ///// <summary>
        ///// Check if user No whether exist in Finger Manchine User By UserID
        ///// </summary>
        ///// <param name="UserID"></param>
        ///// <returns></returns>
        //public bool IsExistUserNo(string UserID)
        //{
        //    return _fingerMachineUserRepository.GetMulti(x => x.UserId == UserID).Count() > 0;
        //}
        public bool IsUserNoExist(List<string> lstUserNo)
        {
            foreach (var item in lstUserNo)
            {
                if (_fingerMachineUserRepository.GetSingleByCondition(x => x.ID == item) != null)
                    return true;
            }
            return false;
        }
    }
}