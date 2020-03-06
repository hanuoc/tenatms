using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using TMS.Common.Constants;
using TMS.Model.Models;
using TMS.Service;
using TMS.Web.Infrastructure.Core;
using TMS.Web.Models.Request;
using System.Web.Http;
using System.Threading.Tasks;

namespace TMS.Web.Controllers
{
    [Authorize]
    [RoutePrefix(RoutesConstant.StatusRequest)]
    public class StatusRequestController : ApiControllerBase
    {
        private IStatusRequestService _statusrequestService;
        /// <summary>
        /// Contructor of OTRequestController class
        /// </summary>
        /// <param name="errorService"></param>
        public StatusRequestController(IErrorService errorService, IStatusRequestService statusrequestService) : base(errorService)
        {
            _statusrequestService = statusrequestService;
        }
        /// <summary>
        /// Get list ot request by userId and groupID
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userID">Descending or not</param>
        /// <param name="groupID">Number of page</param>
        /// <returns>Retern a list request</returns>
        [Route(RoutesConstant.StatusRequestGetAll)]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAll(HttpRequestMessage request)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var responseData = Mapper.Map<IEnumerable<StatusRequest>, IEnumerable<StatusRequestViewModel>>(_statusrequestService.GetAllStatusRequest());
                return request.CreateResponse(HttpStatusCode.OK, responseData);
            };
            return await CreateHttpResponse(request, func);
        }
    }
}
