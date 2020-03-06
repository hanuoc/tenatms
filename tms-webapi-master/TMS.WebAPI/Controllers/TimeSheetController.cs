using AutoMapper;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TMS.Common;
using TMS.Common.Constants;
using TMS.Common.ViewModels;
using TMS.Data;
using TMS.Model.Models;
using TMS.Service;
using TMS.Web.Infrastructure.Core;

namespace TMS.Web.Controllers
{
    [Authorize]
    [RoutePrefix(RoutesConstant.TimeSheetAPI)]
    public class TimeSheetController : ApiControllerBase
    {
        private ITimeSheetService _timeSheetService;
        private IFingerTimeSheetTmpService _fingerTimeSheetTmpService;
        private IFingerTimeSheetService _fingerTimeSheetService;
        private IFingerMachineUserService _fingerMachineUserService;

        /// <summary>
        /// Contructor of TimeSheetClontroller class
        /// </summary>
        /// <param name="errorService"></param>
        public TimeSheetController(IErrorService errorService, ITimeSheetService timeSheetService, IFingerTimeSheetTmpService fingerTimeSheetTmpService, IFingerTimeSheetService fingerTimeSheetService, IFingerMachineUserService fingerMachineUserService) : base(errorService)
        {
            _timeSheetService = timeSheetService;
            _fingerTimeSheetTmpService = fingerTimeSheetTmpService;
            _fingerTimeSheetService = fingerTimeSheetService;
            _fingerMachineUserService = fingerMachineUserService;
        }

        /// <summary>
        /// get list timesheet and paging filter
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userID">id of user logged in</param>
        /// <param name="filter">filter timesheet </param>
        /// <param name="page">current page</param>
        /// <param name="pageSize"> Page Size</param>
        /// <returns>list time sheet model contains all information to view</returns>
        [Route(RoutesConstant.TimeSheetGetAll)]
        [HttpPost]
        public async Task<HttpResponseMessage> GetAll(HttpRequestMessage request, string userID, [FromBody] FilterModel filter, int page, int pageSize,string colunm,bool isAsc)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var listTimeSheet = _fingerTimeSheetService.GetListTimeSheetFilter(userID, filter,colunm,isAsc);
                int totalRow = listTimeSheet.Count;
                listTimeSheet = listTimeSheet.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                var dataConverted = _fingerTimeSheetService.MapAbsentField(listTimeSheet);
                PaginationSet<FingerTimeSheetModel> pagedSet = new PaginationSet<FingerTimeSheetModel>()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    TotalRows = totalRow,
                    Items = dataConverted,
                };
                return request.CreateResponse(HttpStatusCode.OK, pagedSet);
            };
            return await CreateHttpResponse(request, func);
        }
        [Route("import")]
        [HttpPost]
        public async Task<HttpResponseMessage> Import()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, MessageSystem.FormatIsNotSupport);
            }

            var root = HttpContext.Current.Server.MapPath(CommonConstants.PathUploadTimeSheet);
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            var provider = new MultipartFormDataStreamProvider(root);
            var result = await Request.Content.ReadAsMultipartAsync(provider);
            //Upload files
            int addedCount = 0;
            foreach (MultipartFileData fileData in result.FileData)
            {
                IList<string> AllowedFileExtensions = new List<string> { CommonConstants.FileTimeSheetSupport };
                var ext = fileData.Headers.ContentDisposition.FileName.Substring(fileData.Headers.ContentDisposition.FileName.LastIndexOf('.'));
                var extension = ext.Remove(ext.Length - 1, 1).ToLower();
                if (!AllowedFileExtensions.Contains(extension)) 
                {
                    return Request.CreateResponse(HttpStatusCode.UnsupportedMediaType, MessageSystem.FormatIsNotSupport);
                }
                if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, MessageSystem.FormatIsNotSupport);
                }
                string fileName = fileData.Headers.ContentDisposition.FileName;
                if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                {
                    fileName = fileName.Trim('"');
                }
                if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                {
                    fileName = Path.GetFileName(fileName);
                }
                var fullPath = Path.Combine(root, fileName);
                File.Copy(fileData.LocalFileName, fullPath, true);
                //insert to DB
                List<FingerTimeSheetTmp> listFingerTimeSheet = new List<FingerTimeSheetTmp>();
                List<FingerTimeSheetTmpErrorModel> listFingerTimeSheetError = new List<FingerTimeSheetTmpErrorModel>();
                this.ReadTimeSheetFromTextFile(fullPath, out listFingerTimeSheet, out listFingerTimeSheetError);
                if (listFingerTimeSheet.Count > 0 && listFingerTimeSheetError.Count == 0)
                {
                    listFingerTimeSheet.Where(x => x.Date < DateTime.Today.Date);
                    _fingerTimeSheetTmpService.ClearAllData();
                    foreach (var timeSheet in listFingerTimeSheet)
                    {
                        _fingerTimeSheetTmpService.Add(timeSheet);
                        
                    }
                    _fingerTimeSheetTmpService.Save();

                    var context = HttpContext.Current.GetOwinContext().Get<TMSDbContext>();
                    var isSuccess = _fingerTimeSheetService.ImportTimeSheet(out addedCount, context, out listFingerTimeSheetError);
                    WriteErrorToFile(listFingerTimeSheetError);
                    if (listFingerTimeSheetError.Count!=0 || !isSuccess)
                        return Request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ErrorImportFile);
                }
                else
                {
                    try
                    {
                        WriteErrorToFile(listFingerTimeSheetError);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ErrorImportFile);
                    }
                    catch (IOException ex)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message.ToString());
                    }
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, "Import " + addedCount + " time sheet successfully.");
        }
        /// <summary>
        /// Write error to file
        /// </summary>
        /// <param name="listFingerTimeSheetError"></param>
        private void WriteErrorToFile(List<FingerTimeSheetTmpErrorModel> listFingerTimeSheetError)
        {
            var pathError = HttpContext.Current.Server.MapPath(CommonConstants.PathUploadTimeSheetError);
            if (!Directory.Exists(pathError))
            {
                Directory.CreateDirectory(pathError);
            }
            FileStream fileStream = new FileStream(pathError+ "error.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            using (StreamWriter sw = new StreamWriter(fileStream, Encoding.UTF8))
            {
                foreach (var item in listFingerTimeSheetError)
                {
                    sw.Write(item.UserNo + CommonConstants.REGEX);
                    sw.Write(item.Date + CommonConstants.REGEX);
                    sw.Write(item.NumberFinger + CommonConstants.REGEX);
                    sw.Write(item.UserName + CommonConstants.REGEX);
                    sw.WriteLine(item.Error);
                }
            }
        }
        /// <summary>
        /// Get list error
        /// </summary>
        /// <param name="request"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [Route(RoutesConstant.GetListError)]
        [HttpGet]
        public async Task<HttpResponseMessage> GetListError(HttpRequestMessage request, int page, int pageSize)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var model = ReadFileError();
                var data = model.Skip((page - 1) * pageSize).Take(pageSize);
                var paginationSet = new PaginationSet<FingerTimeSheetTmpErrorModel>()
                {
                    Items = data,
                    PageIndex = page,
                    TotalRows = model.Count(),
                    PageSize = pageSize
                };
                return request.CreateResponse(HttpStatusCode.OK, paginationSet);
            };
            return await CreateHttpResponse(request, func);
        }
        /// <summary>
        /// Read file error
        /// </summary>
        /// <returns></returns>
        private List<FingerTimeSheetTmpErrorModel> ReadFileError()
        {
            List<FingerTimeSheetTmpErrorModel> listFingerTimeSheetError = new List<FingerTimeSheetTmpErrorModel>();
            var pathError = HttpContext.Current.Server.MapPath(CommonConstants.PathUploadTimeSheetError);
            string[] lines = System.IO.File.ReadAllLines(pathError+"error.txt");
            Regex regex = new Regex(CommonConstants.REGEX);
            foreach (var line in lines)
            {
                var result = regex.Split(line);
                FingerTimeSheetTmpErrorModel fingerTimeSheetTmpError = new FingerTimeSheetTmpErrorModel();
                //fingerTimeSheetTmpError.LineId = result[0];
                fingerTimeSheetTmpError.UserNo = result[0];
                fingerTimeSheetTmpError.Date = result[1];
                fingerTimeSheetTmpError.NumberFinger = result[2];
                fingerTimeSheetTmpError.UserName = result[3];
                fingerTimeSheetTmpError.Error = result[4];
                listFingerTimeSheetError.Add(fingerTimeSheetTmpError);
            }
            return listFingerTimeSheetError;
        }
        /// <summary>
        /// Read timesheet form text file
        /// </summary>
        /// <param name="fullPath">full</param>
        /// <param name="outListTimeSheet"></param>
        /// <param name="outListTimeSheetError"></param>
        private void ReadTimeSheetFromTextFile(string fullPath, out List<FingerTimeSheetTmp> outListTimeSheet, out List<FingerTimeSheetTmpErrorModel> outListTimeSheetError)
        {
            outListTimeSheet = new List<FingerTimeSheetTmp>();
            outListTimeSheetError = new List<FingerTimeSheetTmpErrorModel>();
            string[] lines = System.IO.File.ReadAllLines(fullPath);
            int countLineError = 0;
            Regex regex = new Regex(CommonConstants.REGEX);
            foreach (var line in lines)
            {
                countLineError++;
                var result = regex.Split(line);
                string errorLine = "";
                FingerTimeSheetTmpErrorModel fingerTimeSheetTmpError = new FingerTimeSheetTmpErrorModel();
                if (result.Count() != 7)
                {
                    fingerTimeSheetTmpError.Error = MessageSystem.ErrorInvalidDataFile;
                    outListTimeSheetError.Add(fingerTimeSheetTmpError);
                    continue;
                }
                else
                {
                    int outInt;
                    DateTime outDateTime;
                    if (!int.TryParse(result[0], out outInt))
                        errorLine = MessageSystem.InvalidDataAccNo;
                    if (_fingerMachineUserService.GetFingerMachineUserById(result[0]) == null)
                        errorLine = MessageSystem.InvalidDataAccNoNotExits;
                    if (!DateTime.TryParse(result[1], out outDateTime))
                        errorLine = string.IsNullOrEmpty(errorLine) ? MessageSystem.InvalidDate : errorLine + CommonConstants.POINT + MessageSystem.InvalidDate;
                    if (!DateTime.TryParse(result[1], out outDateTime))
                        errorLine = string.IsNullOrEmpty(errorLine) ? MessageSystem.InvalidDate : errorLine + CommonConstants.POINT + MessageSystem.InvalidDate;
                    if (!int.TryParse(result[3], out outInt))
                        errorLine = string.IsNullOrEmpty(errorLine) ? MessageSystem.InvalidFingerNumber : errorLine + CommonConstants.POINT + MessageSystem.InvalidFingerNumber;
                    if (result[4].Length > 50)
                        errorLine = string.IsNullOrEmpty(errorLine) ? MessageSystem.InvalidAccountName : errorLine + CommonConstants.POINT + MessageSystem.InvalidAccountName;
                    //if (AppUserManager.FindByNameAsync(result[4]).Result == null)
                    //    errorLine = string.IsNullOrEmpty(errorLine) ? MessageSystem.InvalidDataAccNamNotExits : errorLine + CommonConstants.POINT + MessageSystem.InvalidDataAccNamNotExits;
                    if (!string.IsNullOrEmpty(errorLine))
                    {
                        fingerTimeSheetTmpError.UserNo = result[0];
                        fingerTimeSheetTmpError.Date = result[1];
                        fingerTimeSheetTmpError.NumberFinger = result[3];
                        fingerTimeSheetTmpError.UserName = result[4];
                        fingerTimeSheetTmpError.Error = errorLine;
                        outListTimeSheetError.Add(fingerTimeSheetTmpError);
                        continue;
                    }
                    FingerTimeSheetTmp fingerTimeSheetTmp = new FingerTimeSheetTmp();
                    fingerTimeSheetTmp.UserNo = result[0];
                    fingerTimeSheetTmp.Date = DateTime.Parse(result[1]);
                    fingerTimeSheetTmp.NumberFinger = int.Parse(result[3]);
                    fingerTimeSheetTmp.AccName = result[4];
                    outListTimeSheet.Add(fingerTimeSheetTmp);
                }
            }
        }
        [Route(RoutesConstant.ExportExcel)]
        [HttpPost]
        public async Task<HttpResponseMessage> ExportToExcel(HttpRequestMessage request, string userID, [FromBody] FilterModel filter, int page, int pageSize,string colunm , bool isAsc)
        {
            string fileName = string.Concat(CommonConstants.FunctionTimeSheet + DateTime.Now.ToString(CommonConstants.dateExport) + CommonConstants.fileExport);
            var folderReport = ConfigHelper.GetByKey(CommonConstants.reportFolder);
            string fileTemplate = folderReport + CommonConstants.Link + fileName;
            string filePath = HttpContext.Current.Server.MapPath(folderReport);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fullPath = Path.Combine(filePath, fileName);
            try
            {
                var listTimeSheet = _fingerTimeSheetService.GetListTimeSheetFilter(userID, filter, colunm,isAsc);
                var responseData = Mapper.Map<List<FingerTimeSheetModel>, List<FingerTimeSheetExcel>>(listTimeSheet);
                for (int i = 0; i < listTimeSheet.Count; i++)
                {
                    responseData[i].FullName = listTimeSheet[i].UserName;
                    responseData[i].Date = listTimeSheet[i].DayOfCheck.ToString(CommonConstants.FormatDate_DDMMYYY);
                    responseData[i].CheckIn = listTimeSheet[i].CheckIn;
                    responseData[i].CheckOut = listTimeSheet[i].CheckOut;
                    responseData[i].LateTime = listTimeSheet[i].Late;
                    responseData[i].LeaveEarly = listTimeSheet[i].LeaveEarly;
                    if (!string.IsNullOrEmpty(listTimeSheet[i].OTCheckIn) && listTimeSheet[i].OTCheckIn != "null") { 
                        responseData[i].OTCheckIn = listTimeSheet[i].OTCheckIn ;
                    }
                    else
                    {
                        responseData[i].OTCheckIn = null;
                    }
                    if (!string.IsNullOrEmpty(listTimeSheet[i].OTCheckOut) && listTimeSheet[i].OTCheckOut != "null")
                    {
                        responseData[i].OTCheckOut = listTimeSheet[i].OTCheckOut;
                    }else
                    {
                        responseData[i].OTCheckOut = null;
                    }
                    var absent = listTimeSheet[i].Absent;
                    if (absent == StringConstants.TimeSheetAbsentMorning )
                    {
                        absent = StringConstants.AbsentMorning;
                        responseData[i].Absent = absent;
                    }
                    if (absent == StringConstants.TimeSheetAbsentAfternoon)
                    {
                        absent = StringConstants.AbsentAfternoon;
                        responseData[i].Absent = absent;
                    }
                    if (absent == StringConstants.TimeSheetAbsent)
                    {
                        absent = StringConstants.Absent;
                        responseData[i].Absent = absent;
                    }       
                    responseData[i].Allowance = listTimeSheet[i].MinusAllowance;
                    responseData[i].WorkingDay = listTimeSheet[i].NumOfWorkingDay;
                    responseData[i].Status = listTimeSheet[i].StatusExplanation;
                }
                await ReportHelper.GenerateXls(responseData, fullPath);
                return request.CreateResponse(HttpStatusCode.OK, fileTemplate);
            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        //[Route(RoutesConstant.ExportExcel)]
        //[HttpPost]
        //public async Task<HttpResponseMessage> ReportExcel(HttpRequestMessage request, string userID, [FromBody] FilterReport filter)
        //{
        //    if (!string.IsNullOrEmpty( filter.StartDate) && !string.IsNullOrEmpty( filter.EndDate))
        //    {
        //        DateTime startDate = DateTime.ParseExact(filter.StartDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture).Date;
        //        DateTime endDate = DateTime.ParseExact(filter.EndDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture).Date;
        //        //string fileName = string.Concat(CommonConstants.FunctionReport + DateTime.Now.ToString(CommonConstants.dateExport) + CommonConstants.fileExport);
        //        string fileName = string.Concat(CommonConstants.FunctionReport + "- " + startDate.ToString("dd MM yyyy") + "-" + endDate.ToString("dd MM yyyy") + CommonConstants.fileExport);
        //        var folderReport = ConfigHelper.GetByKey(CommonConstants.reportFolder);
        //        string fileTemplate = folderReport + CommonConstants.Link + fileName;
        //        string filePath = HttpContext.Current.Server.MapPath(folderReport);
        //        if (!Directory.Exists(filePath))
        //        {
        //            Directory.CreateDirectory(filePath);
        //        }
        //        if (File.Exists(HttpContext.Current.Server.MapPath(fileTemplate)))
        //        {
        //            File.Delete(HttpContext.Current.Server.MapPath(fileTemplate));
        //        }
        //        string fullPath = Path.Combine(filePath, fileName);
        //        try
        //        {
        //            var listReport = _fingerTimeSheetService.GetAllReport(userID, filter).ToList();
        //            var responseData = Mapper.Map<IEnumerable<ReportModel>, List<ReportExcel>>(listReport);
        //            //for (int i = 0; i < listReport.Count; i++)
        //            //{
        //            //    //responseData[i].AcountNo = listReport[i].ID.ToString();
        //            //    responseData[i].FullName = listReport[i].FullName;
        //            //    responseData[i].TotalEntitleYear = listReport[i].TotalEntitleYear;
        //            //    responseData[i].Remain = listReport[i].Remain;
        //            //    responseData[i].WorkingDaysFromFingerPrint = listReport[i].WorkingDaysFromFingerPrint;
        //            //    responseData[i].TotalAuthorizedLeavesInPeriod = listReport[i].TotalAuthorizedLeavesInPeriod;
        //            //}
        //            await ReportHelper.GenerateXls(responseData, fullPath);
        //            return request.CreateResponse(HttpStatusCode.OK, fileTemplate);
        //        }
        //        catch (Exception ex)
        //        {
        //            return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
        //        }
              
        //    }
        //    else
        //    {
        //        return request.CreateResponse(HttpStatusCode.BadRequest, "Please Choose Start Date and End Date");
        //    }
        //}
        [Route(RoutesConstant.ExportExcelEx)]
        [HttpPost]
        public async Task<HttpResponseMessage> ReportExcelEx(HttpRequestMessage request, string userID, [FromBody] FilterReport filter)
        {
            if (!string.IsNullOrEmpty(filter.StartDate) && !string.IsNullOrEmpty(filter.EndDate))
            {
                DateTime startDate = DateTime.ParseExact(filter.StartDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture).Date;
                DateTime endDate = DateTime.ParseExact(filter.EndDate, CommonConstants.FormatDate_MMDDYYY, CultureInfo.InvariantCulture).Date;
                //string fileName = string.Concat(CommonConstants.FunctionReport + DateTime.Now.ToString(CommonConstants.dateExport) + CommonConstants.fileExport);
                string fileName = @"ReportEx.xlsx";
                var folderReport = ConfigHelper.GetByKey(CommonConstants.reportFolder);
                string fileTemplate = folderReport + CommonConstants.Link + fileName;
                string filePath = HttpContext.Current.Server.MapPath(folderReport);
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                if (File.Exists(HttpContext.Current.Server.MapPath(fileTemplate)))
                {
                    File.Delete(HttpContext.Current.Server.MapPath(fileTemplate));
                }
                string fullPath = Path.Combine(filePath, fileName);
                try
                {
                    await _fingerTimeSheetService.GetAllReportEx(userID, startDate, endDate);
                    return request.CreateResponse(HttpStatusCode.OK, fileTemplate);
                }
                catch (Exception ex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                }

            }
            else
            {
                return request.CreateResponse(HttpStatusCode.BadRequest, "Please Choose Start Date and End Date");
            }
        }
        [Route(RoutesConstant.GetAllReport)]
        [HttpPost]
        public async Task<HttpResponseMessage> GetAllReport(HttpRequestMessage request, string userID, [FromBody] FilterReport filter, int page, int pageSize)
        {
            Func<HttpResponseMessage> func = () =>
            {
                if (string.IsNullOrEmpty(userID ))
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(userID) + MessageSystem.NoValues );
                }
                var totalRow = 0;
                var data = _fingerTimeSheetService.GetAllReportPaging(userID, filter,page,pageSize,ref totalRow);
                //var data = model.Skip((page - 1) * pageSize).Take(pageSize);
                var paginationSet = new PaginationSet<ReportModel>()
                {
                    Items = data,
                    PageIndex = page,
                    TotalRows = totalRow,
                    PageSize = pageSize
                };
                return request.CreateResponse(HttpStatusCode.OK, paginationSet);
            };
            return await CreateHttpResponse(request, func);
        }
        [Route(RoutesConstant.GetProgressValue)]
        [HttpGet]
        public async Task<HttpResponseMessage> GetProgressValue(HttpRequestMessage request, string userID)
        {
            Func<HttpResponseMessage> func = () =>
            {
                if (string.IsNullOrEmpty(userID))
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(userID) + MessageSystem.NoValues);
                }
                int value = HttpRuntime.Cache["ProgressValue:" + userID]!=null?(int)HttpRuntime.Cache["ProgressValue:" + userID]:0;
                return request.CreateResponse(HttpStatusCode.OK, value);
            };
            return await CreateHttpResponse(request, func);
        }
        [Route(RoutesConstant.CountUserReportEx)]
        [HttpGet]
        public async Task<HttpResponseMessage> CountUserReportEx(HttpRequestMessage request, string userID)
        {
            Func<HttpResponseMessage> func = () =>
            {
                if (string.IsNullOrEmpty(userID))
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(userID) + MessageSystem.NoValues);
                }
                int value = _fingerTimeSheetService.CountUserReportEx(userID);
                return request.CreateResponse(HttpStatusCode.OK, value);
            };
            return await CreateHttpResponse(request, func);
        }
        [Route(RoutesConstant.RemoveProgressValue)]
        [HttpGet]
        public async Task<HttpResponseMessage> RemoveProgressValue(HttpRequestMessage request, string userID)
        {
            Func<HttpResponseMessage> func = () =>
            {
                if (string.IsNullOrEmpty(userID))
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(userID) + MessageSystem.NoValues);
                }
                if (HttpRuntime.Cache["ProgressValue:" + userID] != null)
                    HttpRuntime.Cache.Remove("ProgressValue:" + userID);
                return request.CreateResponse(HttpStatusCode.OK, "Success !");
            };
            return await CreateHttpResponse(request, func);
        }
    }
}