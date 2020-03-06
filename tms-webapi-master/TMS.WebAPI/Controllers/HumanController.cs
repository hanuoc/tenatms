using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TMS.Common.Constants;
using TMS.Common.Exceptions.Extensions;
using TMS.Common.ViewModels;
using TMS.Model.Models;
using TMS.Service;
using TMS.Web.Infrastructure.Core;
using TMS.Web.Models;

namespace TMS.Web.Controllers
{
    [Authorize]
    [RoutePrefix(RoutesConstant.humanApi)]
    public class HumanController : ApiControllerBase
    {
        private IHumanService _humanService;
        private IUserService _userService;

        public HumanController(IErrorService errorService, IHumanService humanService, IUserService userService) : base(errorService)
        {
            this._humanService = humanService;
            this._userService = userService;
        }

        /// <summary>
        /// Send mail to member
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model">object send mail</param>
        /// <returns>object</returns>
        [Route(RoutesConstant.SendEmail)]
        [HttpPost]
        public async Task<HttpResponseMessage> SendMail(HttpRequestMessage request, SendMailViewModel model)
        {
            return await CreateHttpResponse(request, () =>
            {
                string Body = "";
                if (model.toEmail == null || string.IsNullOrEmpty(model.Content.Trim()) || string.IsNullOrEmpty(model.Subject.Trim()))
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(model.toEmail) + MessageSystem.NoValues + nameof(model.ccToEmail)+ MessageSystem.NoValues + nameof(model.Content) + MessageSystem.NoValues + nameof(model.Subject) + MessageSystem.NoValues);
                }
                else
                {
                    Body = _humanService.getBodyMail(MailConsstants.TemplateSendMailToMember, model.Content);
                    var rs = _humanService.SendMail(model.toEmail,model.ccToEmail,model.Subject, Body, model.attackFile);
                    if (rs == true)
                    {
                        return request.CreateResponse(HttpStatusCode.OK, model);
                    }
                    else
                        return request.CreateErrorResponse(HttpStatusCode.NotFound, CommonConstants.SEND_MAIL_NO_SPACE);
                }
            });
        }

        /// <summary>
        /// Get list of user with pagination
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isDesc">Descending or not</param>
        /// <param name="page">Number of page</param>
        /// <param name="pageSize">Number of pagesize</param>
        /// <param name="search">Search by name</param>
        /// <param name="filter">List of filtter or not</param>
        /// <param name="column">Colunm want sort</param>
        /// <returns>Retrun a user list after pagination</returns>
        [Route(RoutesConstant.GetAllMember)]
        [HttpPost]
        public async Task<HttpResponseMessage> GetAllMember(HttpRequestMessage request, string column, bool isDesc, int page, int pageSize, FilterGroup filter, string search = null)
        {
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int totalRow = 0;
                var lstMember = _userService.GetMemberFilter(filter, search);
                totalRow = lstMember.Count();
                var data = lstMember.OrderByField(column, isDesc).Skip((page - 1) * pageSize).Take(pageSize);
                IEnumerable<AppUserViewModel> modelVm = Mapper.Map<IEnumerable<AppUser>, IEnumerable<AppUserViewModel>>(data);
                PaginationSet<AppUserViewModel> pagedSet = new PaginationSet<AppUserViewModel>()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    TotalRows = totalRow,
                    Items = modelVm,
                };
                response = request.CreateResponse(HttpStatusCode.OK, pagedSet);
                return response;
            });
        }
    }
}
