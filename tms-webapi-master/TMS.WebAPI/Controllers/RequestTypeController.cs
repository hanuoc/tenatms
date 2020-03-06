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
    [RoutePrefix(RoutesConstant.RequestTypeApi)]
    [Authorize]
    public class RequestTypeController : ApiControllerBase
    {
        #region Initialize

        private IRequestTypeService _requestTypeService;
        /// <summary>
        /// constructor request type controller
        /// </summary>
        /// <param name="errorService"> error service</param>
        /// <param name="requestTypeService"> request type service</param>
        public RequestTypeController(IErrorService errorService, IRequestTypeService requestTypeService)
            : base(errorService)
        {
            this._requestTypeService = requestTypeService;
        }

        #endregion Initialize
        /// <summary>
        /// get all request type
        /// </summary>
        /// <param name="request">Http request</param>
        /// <returns></returns>
        [Route(RoutesConstant.GetAll)]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAll(HttpRequestMessage request)
        {
            Func<HttpResponseMessage> func = () =>
            {
                return request.CreateResponse(HttpStatusCode.OK, Mapper.Map<IEnumerable<RequestType>, IEnumerable<RequestTypeViewModel>>(_requestTypeService.GetAll()));
            };
            return await CreateHttpResponse(request, func);
        }
    }
}
