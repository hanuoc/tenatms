using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Common.Constants;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Model.Models;

namespace TMS.Service
{
    public interface IDelegationExplanationRequestService
    {
        /// <summary>
        /// get all delegation request
        /// </summary>
        /// <param name="userID">ID of user login</param>
        /// <param name="groupID">GroupID of user login</param>
        /// <returns>list delegation request</returns>
        IEnumerable<ExplanationRequest> GetAllDelegationExplanationRequest(string userID, string groupID);

        /// <summary>
        /// change status request
        /// </summary>
        /// <param name="DelegationRequestID">ID request</param>
        /// <param name="action"></param>
        /// <returns></returns>
        ExplanationRequest ChangeStatus(int DelegationRequestID, string action, string changeStatusBy);

        /// <summary>
        /// get AbnormalCaseReason by abnormal id
        /// </summary>
        /// <param name="id">id of abnormal</param>
        /// <returns>object abnormal</returns>
        //IQueryable<AbnormalCaseReason> GetAbnormalById(int id);
        IEnumerable<AbnormalCase> GetAbnormalById(int id);
    }
    public class DelegationExplanationRequestService : IDelegationExplanationRequestService
    {
        private IExplanationRequestRepository _explanationRequestRepository;
        private IStatusRequestRepository _statusRequestRepository;
        private IUnitOfWork _unitOfWork;
        public DelegationExplanationRequestService(IExplanationRequestRepository ExplanationRequestRepository, IStatusRequestRepository StatusRequestRepository, IUnitOfWork UnitOfWork)
        {
            this._explanationRequestRepository = ExplanationRequestRepository;
            this._statusRequestRepository = StatusRequestRepository;
            this._unitOfWork = UnitOfWork;
        }

        /// <summary>
        /// get all delegation request
        /// </summary>
        /// <param name="userID">ID of username</param>
        /// <param name="GroupID">ID of group</param>
        /// <returns>return list delegation request with user and group<</returns>
        public IEnumerable<ExplanationRequest> GetAllDelegationExplanationRequest(string userID, string groupID)
        {
            return _explanationRequestRepository.GetMulti((x => (x.FingerTimeSheet.FingerMachineUsers.AppUser.GroupId.ToString().Equals(groupID) && x.DelegateId != null)), new string[] {
                    CommonConstants.ReceiverUser,
                    CommonConstants.DelegateUser,
                    CommonConstants.FingerTimeSheet,
                    CommonConstants.FingerMachineUser,
                    CommonConstants.TimeSheetAppUser,
                    CommonConstants.TimeSheetAppUserGroup,
                    CommonConstants.StatusRequest}).OrderByDescending(x => x.UpdatedDate);
        }

        /// <summary>
        /// get list abnormal
        /// </summary>
        /// <param name="id">id of explanation request</param>
        /// <returns>list abnormal</returns>
        //public IQueryable<AbnormalCaseReason> GetAbnormalById(int id)
        //{
        //    return _explanationRequestRepository.GetAbnormalById(id);
        //}

        public IEnumerable<AbnormalCase> GetAbnormalById(int id)
        {
            return _explanationRequestRepository.GetAbnormalById(id);
        }
        /// <summary>
        /// get id of request
        /// </summary>
        /// <param name="Id">Id of request</param>
        /// <returns></returns>
        private ExplanationRequest GetIdRequest(int Id)
        {
            return _explanationRequestRepository.GetSingleByCondition(x => x.ID.Equals(Id), new string[] {
                CommonConstants.ReceiverUser,
                CommonConstants.DelegateUser,
                CommonConstants.FingerTimeSheet,
                CommonConstants.FingerMachineUser,
                CommonConstants.TimeSheetAppUser,
                CommonConstants.TimeSheetAppUserGroup,
                CommonConstants.StatusRequest});
        }

        /// <summary>
        /// commit to database
        /// </summary>
        private void Save()
        {
            _unitOfWork.Commit();
        }

        /// <summary>
        /// change status request
        /// </summary>
        /// <param name="delegationRequestID">ID of request</param>
        /// <param name="action"></param>
        /// <returns>object</returns>
        public ExplanationRequest ChangeStatus(int delegationRequestID, string action, string changeStatusBy)
        {
            return CheckStatus(delegationRequestID, action, changeStatusBy);
        }

        /// <summary>
        /// chech request status
        /// </summary>
        /// <param name="delegationRequestID">ID of Request</param>
        /// <param name="action"></param>
        /// <returns>obejct request</returns>
        private ExplanationRequest CheckStatus(int DelegationRequestID, string action, string changeStatusBy)
        {
            var model = GetIdRequest(DelegationRequestID);
            var statusRequest = _statusRequestRepository.GetMulti(x => x.Name.Equals(action)).FirstOrDefault();
            if (model.StatusRequest.Name == CommonConstants.StatusDelegation)
            {
                model.UpdatedBy = changeStatusBy;
                model.DelegateId = null;
                model.UpdatedDate = DateTime.Now;
                model.StatusRequestId = statusRequest.ID;
                _explanationRequestRepository.Update(model);
                Save();
                return model;
            }
            else
                return model;
        }
    }
}
