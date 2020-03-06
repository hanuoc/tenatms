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
    public interface IDelegationRequestService
    {
        /// <summary>
        /// get all delegation request
        /// </summary>
        /// <param name="userID">ID of user login</param>
        /// <param name="groupID">GroupID of user login</param>
        /// <returns>list delegation request</returns>
        IEnumerable<Request> GetAllDelegationRequest(string userID, string groupID);

        /// <summary>
        /// change status request
        /// </summary>
        /// <param name="DelegationRequestID">ID request</param>
        /// <param name="action"></param>
        /// <returns></returns>
        Request ChangeStatus(Request request, string action, string changeStatusBy);
    }
    public class DelegationRequestService : IDelegationRequestService
    {
        private IRequestRepository _requestRepository;
        private IStatusRequestRepository _statusRequestRepository;
        private IUnitOfWork _unitOfWork;

        public DelegationRequestService(IRequestRepository RequestRepository, IStatusRequestRepository StatusRequestRepository, IUnitOfWork UnitOfWork)
        {
            this._requestRepository = RequestRepository;
            this._statusRequestRepository = StatusRequestRepository;
            this._unitOfWork = UnitOfWork;
        }

        /// <summary>
        /// get all delegation request
        /// </summary>
        /// <param name="userID">ID of username</param>
        /// <param name="GroupID">ID of group</param>
        /// <returns>return list delegation request with user and group<</returns>
        public IEnumerable<Request> GetAllDelegationRequest(string userID, string groupID)
        {
            var y= _requestRepository.GetMulti((x => (x.AppUser.GroupId.ToString().Equals(groupID)) &&( x.AssignToId != null) && (x.StatusRequest.Name == CommonConstants.StatusDelegation)), new string[] {
                    CommonConstants.RequestType, CommonConstants.RequestReasonType, CommonConstants.StatusRequest, CommonConstants.AppUserGroup, CommonConstants.AppUserAssignGroup, CommonConstants.AppUserDelegate, CommonConstants.AppUserChangeStatusGroup
                }).OrderByDescending(x => x.UpdatedDate);
            return _requestRepository.GetMulti(x => ((x.AppUser.GroupId.ToString().Equals(groupID)) &&( x.AssignToId != null)), new string[] {
                    CommonConstants.RequestType, CommonConstants.RequestReasonType, CommonConstants.StatusRequest, CommonConstants.AppUserGroup, CommonConstants.AppUserAssignGroup, CommonConstants.AppUserDelegate, CommonConstants.AppUserChangeStatusGroup
                }).OrderByDescending(x => x.UpdatedDate);
        }

        /// <summary>
        /// get id of request
        /// </summary>
        /// <param name="Id">Id of request</param>
        /// <returns></returns>
        private Request GetIdRequest(int Id)
        {
            return _requestRepository.GetSingleByCondition(x => x.ID.Equals(Id), new string[] { CommonConstants.RequestType, CommonConstants.RequestReasonType, CommonConstants.StatusRequest, CommonConstants.AppUserGroup, CommonConstants.AppUserAssignGroup, CommonConstants.AppUserChangeStatusGroup });
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
        //public Request ChangeStatus(int delegationRequestID, string action, string changeStatusBy)
        //{
        //    return CheckStatus(delegationRequestID, action, changeStatusBy);
        //}
        public Request ChangeStatus(Request request, string action, string changeStatusBy)
        {
            return CheckStatus(request, action, changeStatusBy);
        }

        /// <summary>
        /// chech request status
        /// </summary>
        /// <param name="delegationRequestID">ID of Request</param>
        /// <param name="action"></param>
        /// <returns>obejct request</returns>
        private Request CheckStatus(Request request, string action, string changeStatusBy)
        {
            var statusRequest = _statusRequestRepository.GetMulti(x => x.Name.Equals(action)).FirstOrDefault();
            if (request.StatusRequest.Name == CommonConstants.StatusDelegation)
            {
                float dayBreak = _requestRepository.CalculateDateBreak(request.StartDate, request.EndDate, request.RequestType.Name);
                //float dayBreak = _requestRepository.CalculateDateBreak(request.StartDate, request.EndDate, requestName);
                request.RequestStatusId = statusRequest.ID;
                request.ChangeStatusById = changeStatusBy;
                request.AssignToId = null;
                _requestRepository.Update(request);
                Save();
                return request;
            }
            else if (request.StatusRequest.Name == CommonConstants.StatusApproved || request.StatusRequest.Name == CommonConstants.StatusRejected)
                return request;
            return request;
        }
    }
}
