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
    [RoutePrefix(Common.Constants.RoutesConstant.UserOnsite)]
    public class UserOnsiteController : ApiControllerBase
    {
        IUserOnsiteService _userOnsiteService;
        public UserOnsiteController(IUserOnsiteService userOnsiteService,IErrorService errorService) : base(errorService)
        {
            this._userOnsiteService = userOnsiteService;
        }
        [Route(RoutesConstant.GetUserOnsiteInfo)]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAll(HttpRequestMessage request,string userID,int page,int pageSize)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var model = _userOnsiteService.GetUserOnsite(userID);
                var data = model.Skip((page - 1) * pageSize).Take(pageSize);
                PaginationSet<UserOnsite> pagedSet = new PaginationSet<UserOnsite>()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    TotalRows = model.Count(),
                    Items = data,
                };
                return request.CreateResponse(HttpStatusCode.OK, pagedSet);
            };
            return await CreateHttpResponse(request, func);
        }
        [Route(RoutesConstant.Add)]
        [HttpPost]
        public async Task<HttpResponseMessage> Add(HttpRequestMessage request, UserOnsite userOnsite)
        {
            Func<HttpResponseMessage> func = () =>
            {
                if (!_userOnsiteService.IsValidDuration(userOnsite,false))
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, "Duration invalid!");
                }
                if (_userOnsiteService.Add(userOnsite) != null)
                {
                    return request.CreateResponse(HttpStatusCode.OK, "Add user onsite infomation success!");
                }
                else
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error!");
                
            };
            return await CreateHttpResponse(request, func);
        }
        [Route(RoutesConstant.Update)]
        [HttpPut]
        public async Task<HttpResponseMessage> Update(HttpRequestMessage request, UserOnsite userOnsite)
        {
            Func<HttpResponseMessage> func = () =>
            {
                if (!_userOnsiteService.IsValidDuration(userOnsite,true))
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, "Duration invalid!");
                }
                _userOnsiteService.Update(userOnsite);
                return request.CreateResponse(HttpStatusCode.OK, "Update success!");
            };
            return await CreateHttpResponse(request, func);
        }
        [Route(RoutesConstant.Delete)]
        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(HttpRequestMessage request, int id)
        {
            Func<HttpResponseMessage> func = () =>
            {
                _userOnsiteService.Delete(id);
                return request.CreateResponse(HttpStatusCode.OK, "Delete success!");
            };
            return await CreateHttpResponse(request, func);
        }

        [Route(RoutesConstant.GetUserOnsiteInfoInTime)]
        [HttpGet]
        public async Task<HttpResponseMessage> GetDetailUserOnsiteInTime(HttpRequestMessage request, string userID)
        {
            Func<HttpResponseMessage> func = () =>
            {
                var model = _userOnsiteService.GetUserOnsite(userID).Where(x=> x.StartDate.Value.Date <= DateTime.Now.Date && DateTime.Now.Date <= x.EndDate.Value.Date);
                return request.CreateResponse(HttpStatusCode.OK, model);
            };
            return await CreateHttpResponse(request, func);
        }
    }
}
