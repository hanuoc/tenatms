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
    public interface IOTTimeTypeService
    {
        IEnumerable<OTTimeType> GetAllOTTimeType();
    }
    public class OTTimeTypeService : IOTTimeTypeService
    {
        private IOTTimeTypeRepository _ottimetypeRepository;
        private IUnitOfWork _unitOfWork;
        /// <summary>
        /// Constructor OT Time Type Service
        /// </summary>
        /// <param name="otrequestRepository"></param>
        /// <param name="unitOfWork"></param>
        public OTTimeTypeService(IOTTimeTypeRepository ottimetypeRepository, IUnitOfWork unitOfWork)
        {
            this._ottimetypeRepository = ottimetypeRepository;
            this._unitOfWork = unitOfWork;
        }
        public IEnumerable<OTTimeType> GetAllOTTimeType()
        {
            return _ottimetypeRepository.GetAll();
        }
    }
}
