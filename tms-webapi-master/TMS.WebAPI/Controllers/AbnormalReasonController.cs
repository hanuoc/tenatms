using AutoMapper;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TMS.Common.Constants;
using TMS.Model.Models;
using TMS.Service;
using TMS.Web.Infrastructure.Core;
using TMS.Web.Models.AbnormalCase;

namespace TMS.Web.Controllers
{
    [Authorize]
    [RoutePrefix(RoutesConstant.AbnormalReasonApi)]
    public class AbnormalReasonController : ApiControllerBase
    {
        private IAbnormalReasonService _abnormalReasonService;

        public AbnormalReasonController(IErrorService errorService , IAbnormalReasonService abnormalReasonService) : base(errorService)
        {
            this._abnormalReasonService = abnormalReasonService;
        }
        [Route(RoutesConstant.GetAllAbnormalReason)]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAll(HttpRequestMessage request)
        {
            Func<HttpResponseMessage> func = () =>
            {
                return request.CreateResponse(HttpStatusCode.OK, Mapper.Map<IEnumerable<AbnormalReason>, IEnumerable<AbnormalReasonViewModel>>(_abnormalReasonService.GetAll()));
            };
            return await CreateHttpResponse(request, func);
        }
    }
}
