using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Common.Constants;
using TMS.Common.ViewModels;
using TMS.Data.Infrastructure;
using TMS.Model.Models;

namespace TMS.Data.Repositories
{
    public interface IListOTRepository : IRepository<ListOTModel>
    {
        List<ListOTModel> GetAllUser(string userID, string groupID, bool isReadAll);
    }
    public class ListOTRepository : RepositoryBase<ListOTModel>, IListOTRepository
    {
        /// <summary>
        /// Constructor of OT Request Repository
        /// </summary>
        /// <param name="dbFactory"></param>
        public ListOTRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        /// <summary>
        /// Function Load All List OT
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="groupID"></param>
        /// <param name="isReadAll"></param>
        /// <returns></returns>
        public List<ListOTModel> GetAllUser(string userID, string groupID, bool isReadAll)
        {
            if (isReadAll)
            {
                return (from otRequest in DbContext.OTRequests
                        join otRequestUser in DbContext.OTRequestUsers on otRequest.ID equals (otRequestUser.OTRequestID)
                        join fingerMachineUser in DbContext.FingerMachineUsers on otRequestUser.UserID equals (fingerMachineUser.UserId)
                        join fingerTimeSheet in DbContext.FingerTimeSheets on fingerMachineUser.ID equals (fingerTimeSheet.UserNo)
                        join explain in DbContext.ExplanationRequests on fingerTimeSheet.ID equals (explain.TimeSheetId)
                        into Ex
                        from e in Ex.DefaultIfEmpty()
                        where otRequest.OTDate == fingerTimeSheet.DayOfCheck
                        select new ListOTModel
                        {
                            UserName = fingerMachineUser.AppUser.UserName,
                            FullName = fingerMachineUser.AppUser.FullName,
                            GroupName = otRequest.AppUserCreatedBy.Group.Name,
                            OTDate = otRequest.OTDate,
                            NameOTDateType = otRequest.OTDateType.Name,
                            NameOTDateTime = otRequest.OTTimeType.Name,
                            OTCheckIn =(string.IsNullOrEmpty(fingerTimeSheet.OTCheckIn) && e.StatusRequest.Name == CommonConstants.StatusApproved) ? otRequest.StartTime:fingerTimeSheet.OTCheckIn,
                            OTCheckOut = (string.IsNullOrEmpty(fingerTimeSheet.OTCheckOut)&& e.StatusRequest.Name == CommonConstants.StatusApproved) ? otRequest.EndTime : fingerTimeSheet.OTCheckOut,
                            UpdatedByName = otRequest.AppUserUpdatedBy.FullName,
                            StatusRequest = otRequest.StatusRequest.Name,
                            StatusID = otRequest.StatusRequest.ID
                        }).ToList();
            }
            else
            {
                return (from otRequest in DbContext.OTRequests
                        join otRequestUser in DbContext.OTRequestUsers on otRequest.ID equals (otRequestUser.OTRequestID)
                        join fingerMachineUser in DbContext.FingerMachineUsers on otRequestUser.UserID equals (fingerMachineUser.UserId)
                        join fingerTimeSheet in DbContext.FingerTimeSheets on fingerMachineUser.ID equals (fingerTimeSheet.UserNo)
                        join explain in DbContext.ExplanationRequests on fingerTimeSheet.ID equals (explain.TimeSheetId)
                        into Ex from e in Ex.DefaultIfEmpty()
                        where otRequestUser.UserID == userID && otRequest.OTDate == fingerTimeSheet.DayOfCheck
                        select new ListOTModel
                        {
                            UserName = fingerMachineUser.AppUser.UserName,
                            FullName = fingerMachineUser.AppUser.FullName,
                            GroupName = otRequest.AppUserCreatedBy.Group.Name,
                            OTDate = otRequest.OTDate,
                            NameOTDateType = otRequest.OTDateType.Name,
                            NameOTDateTime = otRequest.OTTimeType.Name,
                            OTCheckIn = (string.IsNullOrEmpty(fingerTimeSheet.OTCheckIn) && e.StatusRequest.Name == CommonConstants.StatusApproved) ? otRequest.StartTime : fingerTimeSheet.OTCheckIn,
                            OTCheckOut = (string.IsNullOrEmpty(fingerTimeSheet.OTCheckOut) && e.StatusRequest.Name == CommonConstants.StatusApproved) ? otRequest.EndTime : fingerTimeSheet.OTCheckOut,
                            UpdatedByName = otRequest.AppUserUpdatedBy.FullName,
                            StatusRequest = otRequest.StatusRequest.Name,
                            StatusID = otRequest.StatusRequest.ID
                        }).ToList();
            }
        }
    }
}
