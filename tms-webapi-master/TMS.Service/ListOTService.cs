using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TMS.Data;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Model.Models;
using TMS.Common.Constants;
using TMS.Common.ViewModels;
using TMS.Web.Models.Common;
using System.Globalization;
using System.IO;
using System.Web;
using TMS.Common.Exceptions.Extensions;

namespace TMS.Service
{
    public interface IListOTService
    {
        List<ListOTModel> GetAllOTFilter(string userID, string groupId, string column, bool isDesc, FilterOTRequestModel filter);
    }
    public class ListOTService : IListOTService
    {
        private IListOTRepository _otrequestRepository;
        private IUnitOfWork _unitOfWork;


        public ListOTService(IListOTRepository otrequestRepository, IUnitOfWork unitOfWork)
        {
            this._otrequestRepository = otrequestRepository;
            this._unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Function Get All OT List
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        /// 

        public IEnumerable<ListOTModel> GetAllWithUser(string userID, string groupID, FilterOTRequestModel filter)
        {
            if (_otrequestRepository.IsReadAll(userID, CommonConstants.FunctionOTList))
            {
                return _otrequestRepository.GetAllUser(userID, groupID, true).OrderBy(x => x.FullName).Where(x => x.OTCheckIn != null && x.OTCheckOut != null && x.StatusRequest == CommonConstants.StatusApproved);
            }
            return _otrequestRepository.GetAllUser(userID, groupID, false).OrderBy(x => x.OTDate).Where(x => x.OTCheckIn != null && x.OTCheckOut != null && x.StatusRequest == CommonConstants.StatusApproved);
        }

        /// <summary>
        /// Function Get OT Filter
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="groupId"></param>
        /// <param name="column"></param>
        /// <param name="isDesc"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<ListOTModel> GetAllOTFilter(string userID, string groupID, string column, bool isDesc, FilterOTRequestModel filter)
        {
            var model = GetAllWithUser(userID, groupID, filter);
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int count = 0;
            foreach (var item in model)
            {
                if (string.IsNullOrEmpty(item.OTCheckIn)|| string.IsNullOrEmpty(item.OTCheckOut) || item.OTCheckIn == "null" || item.OTCheckOut == "null")
                {
                    item.WorkingTime = 0;
                }
                else
                {
                    item.WorkingTime = Math.Round((Convert.ToDouble((TimeSpan.Parse(item.OTCheckOut)).TotalHours - TimeSpan.Parse(item.OTCheckIn).TotalHours)), 2);
                }
            }
            if (filter != null)
            {
                if (filter.OTDateType.Count() != 0)
                {
                    model = model.Where(x => filter.OTDateType.Contains(x.NameOTDateType.ToString()));
                    ++count;
                }
                if (filter.OTTimeType.Count() != 0)
                {
                    model = model.Where(x => filter.OTTimeType.Contains(x.NameOTDateTime.ToString()));
                    ++count;
                }

                if (!string.IsNullOrEmpty(filter.startDate) && !string.IsNullOrEmpty(filter.endDate))
                {
                    model = model.Where(x => (x.OTDate >= DateTime.ParseExact(filter.startDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture)) &&
                                             (x.OTDate <= DateTime.ParseExact(filter.endDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture)));
                    ++count;
                }
                if (filter.FullName.Count() != 0)
                {
                    model = model.Where(x => filter.FullName.Contains(x.UserName));
                    ++count;
                }
            }
            if (filter == null || count == 0)
            {
                model = model.Where(x => x.OTDate.Value.Month == month && x.OTDate.Value.Year == year);
            }
            return model.OrderByField(column,isDesc).ToList();
            //return model.ToList();
        }
    }
}
