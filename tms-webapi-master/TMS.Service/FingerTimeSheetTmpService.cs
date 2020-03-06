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
    public interface IFingerTimeSheetTmpService
    {
        FingerTimeSheetTmp Add(FingerTimeSheetTmp fingerTimeSheetTmp);
        void Save();
        void ClearAllData();

    }

    public class FingerTimeSheetTmpService : IFingerTimeSheetTmpService
    {
        private IFingerTimeSheetTmpRepository _fingerTimeSheetTmpRepository;
        private IUnitOfWork _unitOfWork;

        public FingerTimeSheetTmpService(IFingerTimeSheetTmpRepository fingerTimeSheetTmpRepository, IUnitOfWork unitOfWork)
        {
            this._fingerTimeSheetTmpRepository = fingerTimeSheetTmpRepository;
            this._unitOfWork = unitOfWork;
        }

        public FingerTimeSheetTmp Add(FingerTimeSheetTmp fingerTimeSheetTmp)
        {
            return _fingerTimeSheetTmpRepository.Add(fingerTimeSheetTmp);
        }

        public void ClearAllData()
        {
            _fingerTimeSheetTmpRepository.RemoveAllData();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
