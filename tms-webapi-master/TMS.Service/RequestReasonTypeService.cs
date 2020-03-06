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
    public interface IRequestReasonTypeService
    {
        IEnumerable<RequestReasonType> GetAll();
    }

    public class RequestReasonTypeService : IRequestReasonTypeService
    {
        private IRequestReasonTypeRepository _requestReasonTypeRepository;
        private IUnitOfWork _unitOfWork;

        public RequestReasonTypeService(IRequestReasonTypeRepository requestReasonTypeRepository,
            IUnitOfWork unitOfWork)
        {
            this._requestReasonTypeRepository = requestReasonTypeRepository;
            this._unitOfWork = unitOfWork;
        }
        public IEnumerable<RequestReasonType> GetAll()
        {
            return _requestReasonTypeRepository.GetAll();
        }
    }
}
