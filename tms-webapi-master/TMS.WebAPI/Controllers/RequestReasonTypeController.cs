using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TMS.Common.Constants;
using TMS.Model.Models;
using TMS.Service;
using TMS.Web.Infrastructure.Core;
using TMS.Web.Models.Request;

namespace TMS.Web.Controllers
{
    [RoutePrefix(RoutesConstant.RequestReasonTypeApi)]
    [Authorize]
    public class RequestReasonTypeController : ApiControllerBase
    {
        #region Initialize

        private IRequestReasonTypeService _requestReasonTypeService;
        /// <summary>
        /// constructor request reason type controller
        /// </summary>
        /// <param name="errorService"> error service</param>
        /// <param name="requestReasonTypeService"> request reason type service</param>
        public RequestReasonTypeController(IErrorService errorService, IRequestReasonTypeService requestReasonTypeService)
            : base(errorService)
        {
            this._requestReasonTypeService = requestReasonTypeService;
        }

        #endregion Initialize
        /// <summary>
        /// get all request reason type
        /// </summary>
        /// <param name="request">Http request</param>
        /// <returns></returns>
        [Route(RoutesConstant.GetAll)]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAll(HttpRequestMessage request)
        {
            Func<HttpResponseMessage> func = () =>
            {
                return request.CreateResponse(HttpStatusCode.OK, Mapper.Map<IEnumerable<RequestReasonType>, IEnumerable<RequestReasonTypeViewModel>>(_requestReasonTypeService.GetAll()));
            };
            return await CreateHttpResponse(request, func);
        }
    }
}
