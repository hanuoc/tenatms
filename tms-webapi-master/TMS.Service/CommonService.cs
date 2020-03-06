using System;
using System.Text;
using TMS.Common.Constants;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Model.Models;

namespace TMS.Service
{
    public interface ICommonService
    {
        SystemConfig GetSystemConfig(string code);
        bool IsHolidayOrDayOff(DateTime date);
        TimeDay GetTimeDay(DateTime date);
        bool isHoliday(DateTime date);
        bool isWeekend(DateTime date);
        bool isWorkingDay(DateTime date);
		DateTime GetDateExRequestInPast(DateTime dayOfCheck);
        string CreateMD5(string input);
    }

    public class CommonService : ICommonService
    {
        private ISystemConfigRepository _systemConfigRepository;
        private IUnitOfWork _unitOfWork;
        private ITimeDayRepository _timeDayRepository;
        private IHolidayRepository _holidayRepository;
        public CommonService(ISystemConfigRepository systemConfigRepository,ITimeDayRepository timeDayRepository, IHolidayRepository holidayRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _systemConfigRepository = systemConfigRepository;
            _timeDayRepository = timeDayRepository;
            _holidayRepository = holidayRepository;
        }

        public SystemConfig GetSystemConfig(string code)
        {
            return _systemConfigRepository.GetSingleByCondition(x => x.Code == code);
        }

        public TimeDay GetTimeDay(DateTime date)
        {
            if (_holidayRepository.IsHoliday(date))
                return null;
            TimeDay timeDay = _timeDayRepository.GetSingleByCondition(x => x.Workingday == date.DayOfWeek.ToString());
            if(timeDay == null)
            {
                Holiday holiday = _holidayRepository.GetHolidayForDateOffset(date);
                return holiday != null ? GetTimeDayForDateOffset(holiday) : null;
            }
            return timeDay;
        }
        private TimeDay GetTimeDayForDateOffset(Holiday holiday)
        {
            return _timeDayRepository.GetSingleByCondition(x => x.Workingday == holiday.Date.DayOfWeek.ToString());
        }
        /// <summary>
        /// true if is not timeday and not date offset
        /// true if is holiday
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool IsHolidayOrDayOff(DateTime date)
        {
            return (!_timeDayRepository.IsTimeDay(date) && _holidayRepository.GetSingleByCondition(x=>x.Workingday == date) == null) || _holidayRepository.IsHoliday(date);
        }

        public bool isHoliday(DateTime date)
        {
            return _timeDayRepository.IsTimeDay(date) && _holidayRepository.IsHoliday(date);
        }

        public bool isWeekend(DateTime date)
        {
            return !_timeDayRepository.IsTimeDay(date) && !_holidayRepository.IsWorkingday(date);
        }

        public bool isWorkingDay(DateTime date)
        {
            return _timeDayRepository.IsTimeDay(date) && !_holidayRepository.IsHoliday(date) ||
                   _holidayRepository.IsWorkingday(date);
        }

		public DateTime GetDateExRequestInPast(DateTime dayofCheck)
		{
			var countTimeDay = 0;
			var count = 1;
			var addDay = 0;
			while (countTimeDay <= CommonConstants.DateReject)
			{
				if (!IsHolidayOrDayOff(dayofCheck.AddDays(count)))
				{
					countTimeDay++;
				}
				count++;
				addDay++;
				if (countTimeDay == CommonConstants.DateReject)
				{
					countTimeDay++;
				}
			}
			return dayofCheck.Date.AddDays(addDay);
		}
        public string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}