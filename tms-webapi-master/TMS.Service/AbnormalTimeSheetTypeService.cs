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
  
    public interface IAbnormalTimeSheetTypeService
    {
        IEnumerable<AbnormalTimeSheetType> GetAllAbnormalTimeSheetType();
    }
    public class AbnormalTimeSheetTypeService : IAbnormalTimeSheetTypeService
    {
        private IAbnormalTimeSheetTypeRepository _abnormalTimeSheetTypeRepository;
        private IUnitOfWork _unitOfWork;
        public AbnormalTimeSheetTypeService(IAbnormalTimeSheetTypeRepository abnormalTimeSheetTypeRepository, IUnitOfWork unitOfWork)
        {
            this._abnormalTimeSheetTypeRepository = abnormalTimeSheetTypeRepository;
            this._unitOfWork = unitOfWork;
        }
        /// <summary>
        /// Get all abnormal reason
        /// </summary>
        /// <returns>abnormal reason</returns>
        public IEnumerable<AbnormalTimeSheetType> GetAllAbnormalTimeSheetType()
        {
            return _abnormalTimeSheetTypeRepository.GetAll();
        }
    }
}
