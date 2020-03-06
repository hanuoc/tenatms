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
    public interface IAbnormalReasonService
    {
        IEnumerable<AbnormalReason> GetAll();
    }
    public class AbnormalReasonService : IAbnormalReasonService
    {
        private IAbnormalReasonRepository _abnormalReasonRepository;
        private IUnitOfWork _unitOfWork;

        public AbnormalReasonService(IAbnormalReasonRepository abnormalReasonRepository,
            IUnitOfWork unitOfWork)
        {
            this._abnormalReasonRepository = abnormalReasonRepository;
            this._unitOfWork = unitOfWork;
        }
        public IEnumerable<AbnormalReason> GetAll()
        {
            return _abnormalReasonRepository.GetAll();
        }
    }
}
