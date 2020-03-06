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
    public interface IOTDateTypeService
    {
        IEnumerable<OTDateType> GetAllOTDateType();
    }
    public class OTDateTypeService : IOTDateTypeService
    {
        private IOTDateTypeRespository _otdatetypeRepository;
        private IUnitOfWork _unitOfWork;
        /// <summary>
        /// Constructor OT Time Type Service
        /// </summary>
        /// <param name="otrequestRepository"></param>
        /// <param name="unitOfWork"></param>
        public OTDateTypeService(IOTDateTypeRespository otdatetypeRepository, IUnitOfWork unitOfWork)
        {
            this._otdatetypeRepository = otdatetypeRepository;
            this._unitOfWork = unitOfWork;
        }
        public IEnumerable<OTDateType> GetAllOTDateType()
        {
            return _otdatetypeRepository.GetAll();
        }
    }
}
