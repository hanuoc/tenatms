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
    public interface IConfigDelegationService
    {
        List<ConfigDelegationModel> GetListUserConfigDelegationFilter(string groupID, string[] lstFullName);
        ConfigDelegation GetDataDelegationById(string configDelegationID);
        /// <summary>
        /// update group
        /// </summary>
        /// <param name="group">group to uppdate</param>
        void Update(ConfigDelegation delegation);

        /// <summary>
        /// save database
        /// </summary>
        void SaveChange();

        /// <summary>
        /// Change status request by delegate config, when set delegate in menu Config Delegation
        /// </summary>
        void ChangeStatusRequestConfigDelegate(string assignTo, List<Request> lstRequest);

        /// <summary>
        /// Change status explanation request by delegate config, when set delegate in menu Config Delegation
        /// </summary>
        void ChangeStatusExplanationRequestConfigDelegate(string assignTo, List<ExplanationRequest> lstExplanationRequest);

        /// <summary>
        /// Get delegation by id
        /// </summary>
        /// <param name="userId"> id to find</param>
        /// <returns>group</returns>
        ConfigDelegation GetDelegationByUserId(string userId);

        /// <summary>
        /// Change status request by delegate config, when add request in menu request
        /// </summary>
        void ChangeStatusAfterAddRequest(string assignTo, Request request);

        /// <summary>
        /// Change status request by delegate config, when add request in menu request
        /// </summary>
        void ChangeStatusAfterAddExplanationRequest(string assignTo, ExplanationRequest explanationRequest);

        /// <summary>
        /// add delegation
        /// </summary>
        /// <param name="delegation"> delegation to add</param>
        /// <returns>delegation</returns>
        ConfigDelegation Add(ConfigDelegation delegation);
    }

    public class ConfigDelegationService : IConfigDelegationService
    {
        private IConfigDelegationRepository _configDelegationRepositoty;
        private IUnitOfWork _unitOfWork;
        private IStatusRequestRepository _statusRequestRepository;
        private IRequestRepository _requestRepository;
        private IExplanationRequestRepository _explanationRequestRepository;

        public ConfigDelegationService(IUserService userService, IConfigDelegationRepository configDelegationRepositoty, IStatusRequestRepository statusRequestRepository, IRequestRepository requestRepository,IExplanationRequestRepository explanationRequestRepository, IUnitOfWork unitOfWork)
        {
            _configDelegationRepositoty = configDelegationRepositoty;
            _statusRequestRepository = statusRequestRepository;
            _requestRepository = requestRepository;
            _explanationRequestRepository = explanationRequestRepository;
            _unitOfWork = unitOfWork;

        }

        public List<ConfigDelegationModel> GetListUserConfigDelegationFilter(string groupID, string[] lstFullName)
        {
            var model = _configDelegationRepositoty.GetUserConfigDelegateModel();
            model = model.Where(x => x.GroupID.ToString().Equals(groupID));
            if(lstFullName.Count() != 0)
            {
                model = model.Where(x => lstFullName.Contains(x.UserId));
            }
            else
            {
                model = model.OrderByField("FullName", true);
            }
            
            return model.ToList();
        }

        public ConfigDelegation GetDataDelegationById(string configDelegationID)
        {
            return _configDelegationRepositoty.GetSingleById(Int32.Parse(configDelegationID));
        }


        public void Update(ConfigDelegation delegation)
        {
            _configDelegationRepositoty.Update(delegation);
        }

        public void SaveChange()
        {
            _unitOfWork.Commit();
        }

        //Change status request by delegate config, when set delegate in menu Config Delegation
        public void ChangeStatusRequestConfigDelegate(string assignTo, List<Request> lstRequest)
        {
            var status = _statusRequestRepository.GetMulti(x => x.Name.Contains(CommonConstants.StatusDelegation)).FirstOrDefault();
            if (lstRequest.Count() > 0)
            {
                foreach (var itemRequest in lstRequest)
                {
                    if(itemRequest.StatusRequest.Name.Equals(CommonConstants.StatusDelegation))
                    {
                        itemRequest.AssignToId = assignTo;
                        _requestRepository.Update(itemRequest);
                    }else
                    {
                        itemRequest.RequestStatusId = status.ID;
                        itemRequest.AssignToId = assignTo;
                        _requestRepository.Update(itemRequest);
                    }
                }
                SaveChange();
            }
        }

        //Change status explanation request by delegate config, when set delegate config in menu group
        public void ChangeStatusExplanationRequestConfigDelegate(string assignTo, List<ExplanationRequest> lstExplanationRequest)
        {
            var status = _statusRequestRepository.GetMulti(x => x.Name.Contains(CommonConstants.StatusDelegation)).FirstOrDefault();
            if( lstExplanationRequest.Count() > 0)
            {
                foreach (var itemExplanation in lstExplanationRequest)
                {
                    if (itemExplanation.StatusRequest.Name.Equals(CommonConstants.StatusDelegation))
                    {
                        itemExplanation.DelegateId = assignTo;
                        _explanationRequestRepository.Update(itemExplanation);
                    }
                    else
                    {
                        itemExplanation.StatusRequestId = status.ID;
                        itemExplanation.DelegateId = assignTo;
                        _explanationRequestRepository.Update(itemExplanation);
                    }
                }
                SaveChange();
            }
        }

        // Get delegation config by user id
        public ConfigDelegation GetDelegationByUserId(string userId)
        {
            return _configDelegationRepositoty.GetDataDelegationByUserId(userId);
        }

        //Change status request by delegate config, when add request in menu request
        public void ChangeStatusAfterAddRequest(string assignTo, Request request)
        {
            var status = _statusRequestRepository.GetMulti(x => x.Name.Contains(CommonConstants.StatusDelegation)).FirstOrDefault();
            request.RequestStatusId = status.ID;
            request.AssignToId = assignTo;
            _requestRepository.Update(request);
            SaveChange();
        }

        public void ChangeStatusAfterAddExplanationRequest(string assignTo, ExplanationRequest explanationRequest)
        {
            var status = _statusRequestRepository.GetMulti(x => x.Name.Contains(CommonConstants.StatusDelegation)).FirstOrDefault();
            explanationRequest.StatusRequestId = status.ID;
            explanationRequest.DelegateId = assignTo;
            _explanationRequestRepository.Update(explanationRequest);
            SaveChange();
        }

        public ConfigDelegation Add(ConfigDelegation delegation)
        {
            return _configDelegationRepositoty.Add(delegation);
        }
    }
}
