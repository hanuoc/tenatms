using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Common.Constants;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Model.Models;

namespace TMS.Service
{
    public interface ITimeDayService
    {
        IEnumerable<TimeDay> GetAllTimeDay();
        TimeDay Add(TimeDay timeday);
        TimeDay Delete(int id);
        void SaveChange();
        void Update(TimeDay timeday);
        TimeDay GetbyId(int Id);
        bool CheckEqual(string workingday, int id);
        string CheckConditionTime(string StartTime, string EndTime);
    }
    public class TimeDayService : ITimeDayService
    {
        private ITimeDayRepository _timeDayRepository;
        private IUnitOfWork _unitOfWork;
        public TimeDayService (ITimeDayRepository timeDayRepository , IUnitOfWork unitOfWork)
        {
            this._timeDayRepository = timeDayRepository;
            this._unitOfWork = unitOfWork;
        }
        /// <summary>
        /// function Get all time day
        /// </summary>
        /// <returns>return list time day</returns>
        public IEnumerable<TimeDay> GetAllTimeDay()
        {
            return _timeDayRepository.GetAll();
        }
        /// <summary>
        /// Get by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public TimeDay GetbyId(int Id)
        {
            return _timeDayRepository.GetSingleById(Id);
        }
        /// <summary>
        /// function add
        /// </summary>
        /// <param name="timeday"></param>
        /// <returns></returns>
        public TimeDay Add(TimeDay timeday)
        {
           return _timeDayRepository.Add(timeday);
        }
        /// <summary>
        /// fuction delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TimeDay Delete(int id)
        {
            var timeday = _timeDayRepository.GetSingleById(id);
            return _timeDayRepository.Delete(timeday);
        }
        /// <summary>
        /// function save data
        /// </summary>
        public void SaveChange()
        {
            _unitOfWork.Commit();
        }
        /// <summary>
        /// fuction update
        /// </summary>
        /// <param name="timeday"></param>
        public void Update(TimeDay timeday)
        {
            _timeDayRepository.Update(timeday);

        }
        /// <summary>
        /// fuction check equal
        /// </summary>
        /// <param name="workingday"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckEqual(string workingday, int id)
        {
            var checkequal = _timeDayRepository.GetSingleByCondition(x => x.Workingday == workingday);
            if (checkequal == null)
            {
                return false;
            }
            else
            {
                if (checkequal.ID == id)
                {
                    return false;
                }
            }
            return true;
        }
        public string CheckConditionTime(string StartTime , string EndTime)
        {
            if (TimeSpan.Parse(StartTime) >= TimeSpan.Parse(EndTime))
            {
                return MessageSystem.CheckTime;
            }
            if (TimeSpan.Parse(StartTime) > TimeSpan.FromHours(CommonConstants.StartTimeday))
            {
                return MessageSystem.CheckTimeday;
            }
            if (TimeSpan.Parse(EndTime) < TimeSpan.FromHours(CommonConstants.StartTimeday))
            {
                return MessageSystem.CheckTimeday;
            }

            return null;
        }
    }
   
}
