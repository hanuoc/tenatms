using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TMS.Common.Constants;
using TMS.Service;
using TMS.Web.Infrastructure.Core;

namespace TMS.Web.Controllers
{
    [Authorize]
    [RoutePrefix(RoutesConstant.AbnormalTimeSheetType)]
    public class AbnormalTimeSheetTypeController : ApiControllerBase
    {
        private IAbnormalTimeSheetTypeService _abnormalTimeSheetTypeService;
        /// <summary>
        /// contructor AbnormalTimeSheetTypeController
        /// </summary>
        /// <param name="errorService"></param>
        /// <param name="abnormalTimeSheetTypeService"></param>
        public AbnormalTimeSheetTypeController(IErrorService errorService, IAbnormalTimeSheetTypeService abnormalTimeSheetTypeService) : base(errorService)
        {
            _abnormalTimeSheetTypeService = abnormalTimeSheetTypeService;
        }
        /// <summary>
        /// Get all reason abnormal time sheet
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Get all reason abnormal time sheet</returns>
        [Route(RoutesConstant.StatusRequestGetAll)]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAll(HttpRequestMessage request)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var model = _abnormalTimeSheetTypeService.GetAllAbnormalTimeSheetType();
                return request.CreateResponse(HttpStatusCode.OK, model);
            };
            return await CreateHttpResponse(request, func);
        }
    }
}
