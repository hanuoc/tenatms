using System;
using System.Collections.Generic;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Model.Models;

namespace TMS.Service
{
    public interface IRequestTypeService
    {
        IEnumerable<RequestType> GetAll();
        RequestType GetById(int id);

    }

    public class RequestTypeService : IRequestTypeService
    {
        private IRequestTypeRepository _requestTypeRepository;
        private IUnitOfWork _unitOfWork;

        public RequestTypeService(IRequestTypeRepository requestTypeRepository,
            IUnitOfWork unitOfWork)
        {
            this._requestTypeRepository = requestTypeRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<RequestType> GetAll()
        {
            return _requestTypeRepository.GetAll();
        }

        public RequestType GetById(int id)
        {
            return _requestTypeRepository.GetSingleById(id);
        }
    }
}