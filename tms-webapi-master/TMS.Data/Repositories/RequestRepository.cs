using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using TMS.Common.Constants;
using TMS.Data.Infrastructure;
using TMS.Model.Models;

namespace TMS.Data.Repositories
{
    public interface IRequestRepository : IRepository<Request>
    {
        float CalculateDateBreak(DateTime startDate, DateTime endDate, string requestName);
        float CalculateDateRequest(Request request);
    }

    public class RequestRepository : RepositoryBase<Request>, IRequestRepository
    {
        public RequestRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        public float CalculateDateRequest(Request request)
        {
            var datenow = DateTime.Now.ToString(CommonConstants.FormatDate_DDMMYYY);
            DateTime[] daysOfWeek = { request.StartDate, request.EndDate };
            var allDays = Enumerable.Range(CommonConstants.ZERO, (request.EndDate - request.StartDate).Days + CommonConstants.ONE).Select(d => request.StartDate.AddDays(d));
            var Dates = allDays.Where(dt => daysOfWeek.Contains(dt.Date)).ToList();
            float dateBreak = CommonConstants.ZERO;
            foreach (var item in allDays)
            {
                if (datenow.Equals(item.Date.ToString(CommonConstants.FormatDate_DDMMYYY)))
                {
                    dateBreak += CommonConstants.ONE;
                    return dateBreak;
                }
            } 
            return dateBreak;
        }

        public float CalculateDateBreak(DateTime startDate, DateTime endDate, string requestName)
        {
            DayOfWeek[] daysOfWeek = { DayOfWeek.Saturday, DayOfWeek.Sunday };

            var allDays = Enumerable.Range(CommonConstants.ZERO, (endDate - startDate).Days + CommonConstants.ONE).Select(d => startDate.AddDays(d));

            var Dates = allDays.Where(dt => daysOfWeek.Contains(dt.DayOfWeek)).ToList();

            float dateBreak = CommonConstants.ZERO;

            float dateDay = CommonConstants.ZERO;

            float Saturday = CommonConstants.ZERO;

            float Sunday = CommonConstants.ZERO;

            if (requestName.Equals(CommonConstants.BreakMorning) && startDate == endDate || requestName.Equals(CommonConstants.BreakAfternoon) && startDate == endDate)
            {
                dateDay = ((endDate - startDate).Days + CommonConstants.ONE) * (float)CommonConstants.ZERO_PONT_FIVE;

                if (Dates.Count > CommonConstants.ZERO)
                {
                    foreach (var date in Dates)
                    {
                        if (date.DayOfWeek.Equals(DayOfWeek.Saturday))
                        {
                            Saturday += dateDay - (dateDay - (float)CommonConstants.ZERO_PONT_FIVE);

                        }
                        else if (date.DayOfWeek.Equals(DayOfWeek.Sunday))
                        {
                            Sunday += dateDay - (dateDay - (float)CommonConstants.ZERO_PONT_FIVE);
                        }
                        dateBreak = (dateDay - (Saturday + Sunday));
                    }
                }
                else
                {
                    dateDay = ((endDate - startDate).Days + CommonConstants.ONE) - (float)CommonConstants.ZERO_PONT_FIVE;
                    dateBreak = dateDay;
                }
            }
            else if (requestName.Equals(CommonConstants.BreakMorning) || requestName.Equals(CommonConstants.BreakAfternoon))
            {
                dateDay = ((endDate - startDate).Days + CommonConstants.ONE) *(float)CommonConstants.ZERO_PONT_FIVE;

                if (Dates.Count > CommonConstants.ZERO)
                {
                    foreach (var date in Dates)
                    {
                        if (date.DayOfWeek.Equals(DayOfWeek.Saturday))
                        {
                            Saturday += dateDay - (dateDay - (float)CommonConstants.ZERO_PONT_FIVE);

                        }
                        else if (date.DayOfWeek.Equals(DayOfWeek.Sunday))
                        {
                            Sunday += dateDay - (dateDay - (float)CommonConstants.ZERO_PONT_FIVE);
                        }
                        dateBreak = (dateDay - (Saturday + Sunday)) ;
                    }
                }
                else
                {
                    dateBreak = ((endDate - startDate).Days + CommonConstants.ONE) * (float)CommonConstants.ZERO_PONT_FIVE;

                }
            }
            else if (startDate == endDate)
            {
                if (Dates.Count > CommonConstants.ZERO)
                {
                    dateDay = (endDate - startDate).Days;
                    foreach (var date in Dates)
                    {
                        if (date.DayOfWeek.Equals(DayOfWeek.Saturday))
                        {
                            Saturday += dateDay - (dateDay - CommonConstants.ONE);
                        }
                        else if (date.DayOfWeek.Equals(DayOfWeek.Sunday))
                        {
                            Sunday += dateDay - (dateDay - CommonConstants.ONE);
                        }
                        dateBreak = dateDay - (Saturday + Sunday);
                    }
                    dateBreak = dateDay;
                }
                else
                {
                    dateBreak = CommonConstants.ONE;
                }
            }
            else
            {
                if (Dates.Count > CommonConstants.ZERO)
                {
                    dateDay = (endDate - startDate).Days + CommonConstants.ONE;
                    foreach (var date in Dates)
                    {
                        if (date.DayOfWeek.Equals(DayOfWeek.Saturday))
                        {
                            Saturday += dateDay - (dateDay - CommonConstants.ONE);
                        }
                        else if (date.DayOfWeek.Equals(DayOfWeek.Sunday))
                        {
                            Sunday += dateDay - (dateDay - CommonConstants.ONE);
                        }
                        dateBreak = dateDay - (Saturday + Sunday);
                    }
                }
                else
                    dateBreak = (endDate - startDate).Days + CommonConstants.ONE;
            }
            return dateBreak;
        }
    }

}