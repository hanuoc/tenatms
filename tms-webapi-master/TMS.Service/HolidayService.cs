using System.Collections.Generic;
using System.Linq;
using TMS.Common.Exceptions.Extensions;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Model.Models;

namespace TMS.Service
{
    public interface IHolidayService
    {
        IEnumerable<Holiday> Get(string column, bool isDesc);
        Holiday GetById(int id);
        void Create(Holiday holiday);
        void Update(Holiday holiday);
        void Save();
        void Delete(int id);
    }

    public class HolidayService : IHolidayService
    {
        private readonly IHolidayRepository _holidayRepository;
        private IUnitOfWork _unitOfWork;

        public HolidayService(IHolidayRepository holidayRepository, IUnitOfWork unitOfWork)
        {
            _holidayRepository = holidayRepository;
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<Holiday> Get(string column, bool isDesc)
        {
            var list = _holidayRepository.GetAll();
            return column != null ? list.OrderByField(column, isDesc).ToList() : list.ToList();
        }

        public Holiday GetById(int id)
        {
            return _holidayRepository.GetSingleById(id);
        }

        public void Create(Holiday holiday)
        {
            _holidayRepository.Add(holiday);
        }

        public void Update(Holiday holiday)
        {
           _holidayRepository.Update(holiday);
        }
        public void Delete(int id)
        {
            _holidayRepository.Delete(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
