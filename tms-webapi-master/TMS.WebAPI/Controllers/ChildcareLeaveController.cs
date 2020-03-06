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

namespace TMS.Web.Controllers
{
    [Authorize]
    [RoutePrefix(RoutesConstant.ChildcareLeave)]
    public class ChildcareLeaveController : ApiControllerBase
    {
        private IChildcareLeaveService _childcareLeaveService;
        public ChildcareLeaveController(IErrorService errorService, IChildcareLeaveService childcareLeaveService) : base(errorService)
        {
            _childcareLeaveService = childcareLeaveService;
        }
        [Route(RoutesConstant.Detail)]
        [HttpGet]
        public async Task<HttpResponseMessage> Details(HttpRequestMessage request, int id)
        {
            return await CreateHttpResponse(request, () =>
            {
                var result= _childcareLeaveService.GetById(id);
                if (result != null)
                {
                    return request.CreateResponse(HttpStatusCode.OK, result);
                }
                else
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, "Can not find details !");
                }
            });
        }
        [Route(RoutesConstant.Add)]
        [HttpPost]
        public async Task<HttpResponseMessage> Create(HttpRequestMessage request, string UserID, ChildcareLeave childcareLeave)
        {
            return await CreateHttpResponse(request, () =>
            {
                childcareLeave.Time = CommonConstants.TimeLate;
                var result = _childcareLeaveService.Add(childcareLeave, UserID);
                return request.CreateResponse(HttpStatusCode.OK, result);
            });
        }
        [Route(RoutesConstant.Update)]
        [HttpPut]
        public async Task<HttpResponseMessage> Update(HttpRequestMessage request, string UserID, ChildcareLeave childcareLeave)
        {
            return await CreateHttpResponse(request, () =>
            {
                childcareLeave.Time = CommonConstants.TimeLate;
                if (_childcareLeaveService.Update(childcareLeave, UserID))
                {
                    return request.CreateResponse(HttpStatusCode.OK, childcareLeave);
                }
                else
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest,"Can not update child care leave !");
                }
            });
        }
        [Route(RoutesConstant.Delete)]
        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(HttpRequestMessage request, int id)
        {
            return await CreateHttpResponse(request, () =>
            {
                var result = _childcareLeaveService.Delete(id);
                if (result != null)
                {
                    return request.CreateResponse(HttpStatusCode.OK, result);
                }
                else
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, "Can not delete child care leave !");
                }
            });
        }
    }
}
