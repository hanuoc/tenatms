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
    public interface IStatusRequestService
    {
        IEnumerable<StatusRequest> GetAllStatusRequest();
        StatusRequest getIDbyName(string name);
    }

    public class StatusRequestService : IStatusRequestService
    {
        private IStatusRequestRepository _statusrequestRepository;
        private IUnitOfWork _unitOfWork;

        public StatusRequestService(IStatusRequestRepository statusrequestRepository,
            IUnitOfWork unitOfWork)
        {
            this._statusrequestRepository = statusrequestRepository;
            this._unitOfWork = unitOfWork;
        }
        public IEnumerable<StatusRequest> GetAllStatusRequest()
        {
            return _statusrequestRepository.GetAll();
        }

        public StatusRequest getIDbyName(string name)
        {
            return _statusrequestRepository.GetAll().FirstOrDefault(x => x.Name.Equals(name));
        }
    }
}
