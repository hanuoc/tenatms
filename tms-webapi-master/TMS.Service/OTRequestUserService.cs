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
    public interface IOTRequestUserService
    {
        OTRequestUser Add(string[] otRequestUser, int requestID);
        void Save();
        IEnumerable<OTRequestUser> GetAll(int requestID);
    }
    public class OTRequestUserService : IOTRequestUserService
    {
        private IOTRequestUserRepository _otRequestUserRepository;
        private IUnitOfWork _unitOfWork;
        /// <summary>
        /// Constructor OT Time Type Service
        /// </summary>
        /// <param name="otrequestRepository"></param>
        /// <param name="unitOfWork"></param>
        public OTRequestUserService(IOTRequestUserRepository otRequestUserRepository, IUnitOfWork unitOfWork)
        {
            this._otRequestUserRepository = otRequestUserRepository;
            this._unitOfWork = unitOfWork;
        }
        public IEnumerable<OTRequestUser> GetAll(int requestID)
        {
            return _otRequestUserRepository.GetAll().Where(x => x.OTRequestID.Equals(requestID));
        }
        public OTRequestUser Add(string[] otRequestUser, int requestID)
        {
            foreach (var item in otRequestUser){
                OTRequestUser otrequestUser = new OTRequestUser();
                otrequestUser.OTRequestID = requestID;
                otrequestUser.UserID = item.ToString();
                _otRequestUserRepository.Add(otrequestUser);
            }
            OTRequestUser otrequestUserReturn = new OTRequestUser();
            _unitOfWork.Commit();
            return otrequestUserReturn;
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }

    }
}
