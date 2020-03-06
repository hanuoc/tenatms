using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TMS.Common.Constants;
using TMS.Model.Models;
using TMS.Service;
using TMS.Web.Infrastructure.Core;

namespace TMS.Web.Controllers
{
	[Authorize]
	[RoutePrefix("api/holiday")]
	public class HolidayController : ApiControllerBase
	{
		private readonly IHolidayService _holidayService;
		private readonly ITimeDayService _timedayService;
		private readonly IOTRequestService _otRequestService;
		private readonly IStatusRequestService _statusRequestService;
		private readonly IRequestService _requestService;

		public HolidayController(IErrorService errorService, IHolidayService holidayService,
			ITimeDayService timedayService, IOTRequestService otRequestService, IStatusRequestService statusRequestService, IRequestService requestService) : base(errorService)
		{
			_holidayService = holidayService;
			_timedayService = timedayService;
			_otRequestService = otRequestService;
			_statusRequestService = statusRequestService;
			_requestService = requestService;
		}

		[Route("list")]
		[HttpGet]
		public async Task<HttpResponseMessage> Get(HttpRequestMessage request, string column, bool isDesc, int page, int pageSize)
		{
			Func<HttpResponseMessage> func = () =>
			{

				var model = _holidayService.Get(column, isDesc).ToList();
				var paginationSet = new PaginationSet<Holiday>()
				{
					Items = model.Skip((page - 1) * pageSize).Take(pageSize),
					PageIndex = page,
					TotalRows = model.Count,
					PageSize = pageSize
				};
				return request.CreateResponse(HttpStatusCode.OK, paginationSet);
			};
			return await CreateHttpResponse(request, func);
		}

		[Route("create")]
		[HttpPost]
		public async Task<HttpResponseMessage> Create(HttpRequestMessage request, Holiday holiday)
		{
			return await CreateHttpResponse(request, () =>
			{
				if (!ModelState.IsValid)
				{
					return request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
				}

				if (holiday.Date < DateTime.Now || (holiday.Workingday != null && holiday.Workingday.Value < DateTime.Now))
				{
					return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_HOLIDAY_CREATE_INTHE_PAST);
				}

				var listTimeDay = _timedayService.GetAllTimeDay().ToList();
				if (listTimeDay.FirstOrDefault(x => x.Workingday.Equals(holiday.Date.DayOfWeek.ToString())) == null)
				{
					return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_HOLIDAY_CREATE_CONTAIN_HOLIDAY);
				}

				if (listTimeDay.FirstOrDefault(x => holiday.Workingday != null && x.Workingday.Equals(holiday.Workingday.Value.DayOfWeek.ToString())) != null)
				{
					return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_HOLIDAY_CREATE_IN_TIMEDAY);
				}

				var listHoliday = _holidayService.Get(null, true);
				holiday.Workingday = holiday.Workingday.HasValue ? holiday.Workingday.Value.Date : (DateTime?)null;
				if (listHoliday.FirstOrDefault(x => x.Date == holiday.Date.Date || x.Date == holiday.Workingday ||
													x.Workingday.HasValue && x.Workingday == holiday.Date.Date ||
													x.Workingday.HasValue && x.Workingday == holiday.Workingday) != null)
				{
					return request.CreateResponse(HttpStatusCode.BadRequest,
						MessageSystem.ERROR_HOLIDAY_CREATE_EXISTED);
				}
                holiday.Date = holiday.Date.Date;
                holiday.Note = !string.IsNullOrEmpty(holiday.Note) ? holiday.Note.Trim() : null;
				holiday.Status = true;
				holiday.CreatedDate = DateTime.Now;
				holiday.CreatedBy = User.Identity.Name;
				_holidayService.Create(holiday);
				// Entitle Day
				_requestService.HandleAddHoliday(holiday);
				_holidayService.Save();
				return request.CreateResponse(HttpStatusCode.OK, holiday);
			});
		}


		[Route("update")]
		[HttpPut]
		public async Task<HttpResponseMessage> Update(HttpRequestMessage request, Holiday holiday)
		{
			return await CreateHttpResponse(request, () =>
			{
				if (!ModelState.IsValid)
				{
					return request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
				}
				var existHoliday = _holidayService.GetById(holiday.ID);
				if (existHoliday.Date.Date < DateTime.Now.Date && existHoliday.Date.Date != holiday.Date.Date)
				{
					return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_HOLIDAY_CREATE_INTHE_PAST);
				}

				if (holiday.Workingday != null && holiday.Workingday.Value.Date < DateTime.Now.Date || existHoliday.Workingday != null && existHoliday.Workingday.Value.Date < DateTime.Now.Date  && existHoliday.Workingday.Value.Date != holiday.Workingday.Value.Date)
				{
					return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_HOLIDAY_CREATE_INTHE_PAST);
				}
				if(holiday.Workingday != null && holiday.Workingday.Value < DateTime.Now)
				{
					return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_HOLIDAY_CREATE_INTHE_PAST);
				}

				var listTimeDay = _timedayService.GetAllTimeDay().ToList();

				if (listTimeDay.FirstOrDefault(x => x.Workingday.Equals(holiday.Date.DayOfWeek.ToString())) == null)
				{
					return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_HOLIDAY_CREATE_CONTAIN_HOLIDAY);
				}

				if (listTimeDay.FirstOrDefault(x => holiday.Workingday != null && x.Workingday.Equals(holiday.Workingday.Value.DayOfWeek.ToString())) != null)
				{
					return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_HOLIDAY_CREATE_IN_TIMEDAY);
				}

				var listHoliday = _holidayService.Get(null, true).Where(x => x.ID != holiday.ID);
				holiday.Workingday = holiday.Workingday.HasValue ? holiday.Workingday.Value.Date : (DateTime?)null;
				if (listHoliday.FirstOrDefault(x => x.Date == holiday.Date.Date || x.Date == holiday.Workingday ||
													x.Workingday.HasValue && x.Workingday == holiday.Date.Date ||
													x.Workingday.HasValue && x.Workingday == holiday.Workingday) != null)
				{
					return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_HOLIDAY_CREATE_EXISTED);
				}

				
				_requestService.HandleUpdateHoliday_EntitleDay(holiday, existHoliday);
                holiday.Date = holiday.Date.Date;
                existHoliday.UpdatedDate = DateTime.Now;
				existHoliday.UpdatedBy = User.Identity.Name;
				existHoliday.Date = holiday.Date;
				existHoliday.Workingday = holiday.Workingday;
				existHoliday.Note = !string.IsNullOrEmpty(holiday.Note) ? holiday.Note.Trim() : null;
				_holidayService.Update(existHoliday);
				_holidayService.Save();
				return request.CreateResponse(HttpStatusCode.OK, existHoliday);
			});
		}

		[HttpDelete]
		[Route("delete")]
		public HttpResponseMessage Delete(HttpRequestMessage request, int id)
		{
			var holiday = _holidayService.GetById(id);
			if (holiday == null)
			{
				return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_DELETE_NOT_FOUND);
			}
			if(holiday.Date.Date < DateTime.Now.Date || (holiday.Workingday.HasValue && holiday.Workingday.Value.Date < DateTime.Now.Date))
			{
				return request.CreateResponse(HttpStatusCode.BadRequest, MessageSystem.ERROR_DELETE_INTHE_PAST);
			}
			_requestService.HandleDeleteHoliday(holiday);
			_holidayService.Delete(id);
			_holidayService.Save();
			return request.CreateResponse(HttpStatusCode.OK, id);
		}
	}
}
