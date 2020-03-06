using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Common.ViewModels;
using TMS.Data;
using TMS.Data.Infrastructure;
using TMS.Data.Repositories;
using TMS.Model.Models;

namespace TMS.Service
{
    public interface IScheduleService
    {
        bool ImportTimeSheet(out string message, DateTime date);
        bool JobChangeStatus(out string message);
        bool JobEntitleDay(out string message,DateTime dateJobExcute);
        bool JobUpdateEntitleDayByRequest(out string message,DateTime dateTime);
        
    }
    public class ScheduleService : IScheduleService
    {
        IEntitleDayAppUserService _entitleDayAppUserService;
        IFingerTimeSheetService _fingerTSService;
        IFingerTimeSheetTmpRepository _tmpTimeSheetRepository;
        IRequestService _requestService;
		private IExplanationRequestRepository _explanationRequestRepository;
		ICommonService _commonService;
		IExplanationRequestService _explanationRequestService;
		private IUnitOfWork _unitOfWork;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ScheduleService(IFingerTimeSheetService fingerTSService, IFingerTimeSheetTmpRepository tmpTimeSheetRepository, IEntitleDayAppUserService entitleDayAppUserService, IRequestService requestService, IUnitOfWork unitOfWork, IExplanationRequestRepository explanationRequestRepository, ICommonService commonService, IExplanationRequestService explanationRequestService)
        {
            this._requestService = requestService;
            this._entitleDayAppUserService = entitleDayAppUserService;
            this._tmpTimeSheetRepository = tmpTimeSheetRepository;
            this._fingerTSService = fingerTSService;
			this._explanationRequestRepository = explanationRequestRepository;
			this._commonService = commonService;
			this._explanationRequestService = explanationRequestService;
            this._unitOfWork = unitOfWork;
        }
        public bool ImportTimeSheet(out string message, DateTime dateJobExcute)
        {
            message = "error";
            DateTime yesterday = dateJobExcute.AddDays(-1).Date;
            TMSDbContext DbContext = new TMSDbContext();
            try
            {
                var listTimeSheet = DbContext.CHECKINOUT.Where(x => x.CHECKTIME >= yesterday && x.CHECKTIME < dateJobExcute).ToList();
                if (listTimeSheet.Count > 0)
                {
                    _tmpTimeSheetRepository.RemoveAllData();
                    List<FingerTimeSheetTmp> listTmp = new List<FingerTimeSheetTmp>();
                    foreach (var item in listTimeSheet)
                    {
                        FingerTimeSheetTmp tmp = new FingerTimeSheetTmp();
                        var user = DbContext.USERINFO.FirstOrDefault(x => x.USERID == item.USERID);
                        if (user == null)
                        {
                            continue;
                        }
                        tmp.UserNo = user.Badgenumber;
                        tmp.Date = item.CHECKTIME;
                        tmp.AccName = DbContext.USERINFO.Where(x => x.USERID == item.USERID).Select(x => x.Name).FirstOrDefault();
                        _tmpTimeSheetRepository.Add(tmp);
                        listTmp.Add(tmp);
                    }
                    _unitOfWork.Commit();
                    //import to table finger timesheet
                    int countSuccess = 0;
                    List<FingerTimeSheetTmpErrorModel> listModel = new List<FingerTimeSheetTmpErrorModel>();
                    if (_fingerTSService.ImportTimeSheet(out countSuccess, DbContext, out listModel))
                    {
                        message = "Success";
                        log.Info("Job Time Sheet Run Success :" + DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"));
                        return true;
                    }
                    else
                    {
                        message = "";
                        StringBuilder sb = new StringBuilder();
                        foreach (var item in listModel)
                        {
                            sb.AppendLine(item.Error);
                        }
                        message =  sb.ToString();
                        return false;
                    }
                }
                else
                {
                    message = "FingerTimeSheetTmp table has no data !";
                    return true;
                }
            }
            catch (Exception ex)
            {
                message =ex.Message;
                log.Error(ex.Message);
                if(ex.StackTrace!=null)
                    log.Info(ex.StackTrace);
                return false;
            }
           
        }

        public bool JobChangeStatus(out string message)
        {
            message = "error";
            try
            {
                TMSDbContext DbContext = new TMSDbContext();
                DbContext.Database.CommandTimeout = Common.Constants.CommonConstants.TimeExcuteSql;
                DbContext.Database.ExecuteSqlCommand(Common.Constants.AbnormalQuery.ExcuteCheckTimeOut);
				var explanationRequest = _explanationRequestRepository.GetMulti(x => x.StatusRequestId == 1 || x.StatusRequestId == 5).ToList();
				foreach (var item in explanationRequest)
				{
					if (_commonService.GetDateExRequestInPast(item.CreatedDate.Value) < DateTime.Now.Date)
					{
						item.StatusRequestId = 2;
						_explanationRequestRepository.Update(item);
						_explanationRequestService.Save();
					}
				}
                log.Info("Job Change Status Run Success :" + DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"));
                message = "Success";
                return true;
            }
            catch (Exception ex)
            {
                message = "Error:" + ex.Message;
                return false;
            }
            
        }

        public bool JobEntitleDay(out string message,DateTime dateJobExcute)
        {
            message = "error";
            try
            {
                _entitleDayAppUserService.UpdateEntitleDay(dateJobExcute);
                log.Info("Job Reset EntitleDay Run Success :" + DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"));
                message = "Success";
                return true;
            }
            catch (Exception ex)
            {
                message = "Error:" + ex.Message;
                return false;
            }
            
        }

        public bool JobUpdateEntitleDayByRequest(out string message,DateTime dateTime)
        {
            message = "error";
            try
            {
                _requestService.UpdateDateRequest(dateTime);
                log.Info("Job Update EntitleDay Run Success :" + DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"));
                message = "Success";
                return true;
            }
            catch (Exception ex)
            {
                message = "Error:" + ex.Message;
                return false;
            }
        }
    }
}
