using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TMS.Common;
using TMS.Common.Constants;
using TMS.Model.Models;
using TMS.Service;
using TMS.Web.Infrastructure.Core;
using TMS.Web.Infrastructure.Extensions;
using TMS.Web.Models;

namespace TMS.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/function")]
    public class FunctionController : ApiControllerBase
    {
        /// <summary>
        /// Declare dependency injection
        /// </summary>
        private IFunctionService _functionService;

        public FunctionController(IErrorService errorService, IFunctionService functionService) : base(errorService)
        {
            this._functionService = functionService;
        }

        [Route("getlisthierarchy")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAllHierachy(HttpRequestMessage request)
        {
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                IEnumerable<Function> model;
                model = _functionService.GetAllWithPermission(User.Identity.GetUserId());
                IEnumerable<FunctionViewModel> modelVm = Mapper.Map<IEnumerable<Function>, IEnumerable<FunctionViewModel>>(model);
                var parents = modelVm.Where(x => x.Parent == null);
                foreach (var parent in parents)
                {
                    parent.ChildFunctions = modelVm.Where(x => x.ParentId == parent.ID).ToList();
                }
                response = request.CreateResponse(HttpStatusCode.OK, parents);

                return response;
            });
        }

        [Route("getall")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAll(HttpRequestMessage request, string filter = "")
        {
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                IEnumerable<Function> model = null;
                var cacheFunctions = MemoryCacheHelper.GetValue("function");
                if (cacheFunctions != null)
                {
                    model = (IEnumerable<Function>)cacheFunctions;
                }
                else
                {
                    MemoryCacheHelper.Add("function", _functionService.GetAll(filter), DateTimeOffset.MaxValue);
                    model = _functionService.GetAll(filter);
                }
                IEnumerable<FunctionViewModel> modelVm = Mapper.Map<IEnumerable<Function>, IEnumerable<FunctionViewModel>>(model);

                response = request.CreateResponse(HttpStatusCode.OK, modelVm);

                return response;
            });
        }

        [Route("detail/{id}")]
        [HttpGet]
        public HttpResponseMessage Details(HttpRequestMessage request, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(id) + MessageSystem.NoValues);
            }
            var function = _functionService.Get(id);
            if (function == null)
            {
                return request.CreateErrorResponse(HttpStatusCode.NoContent, MessageSystem.NoData);
            }
            var modelVm = Mapper.Map<Function, FunctionViewModel>(function);

            return request.CreateResponse(HttpStatusCode.OK, modelVm);
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Create(HttpRequestMessage request, FunctionViewModel functionViewModel)
        {
            if (ModelState.IsValid)
            {
                var newFunction = new Function();
                try
                {
                    if (_functionService.CheckExistedId(functionViewModel.ID))
                    {
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, "ID đã tồn tại");
                    }
                    else
                    {
                        if (functionViewModel.ParentId == "") functionViewModel.ParentId = null;
                        newFunction.UpdateFunction(functionViewModel);

                        _functionService.Create(newFunction);
                        _functionService.Save();
                        return request.CreateResponse(HttpStatusCode.OK, functionViewModel);
                    }
                }
                catch (Exception dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [HttpPut]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, FunctionViewModel functionViewModel)
        {
            if (ModelState.IsValid)
            {
                var function = _functionService.Get(functionViewModel.ID);
                try
                {
                    if (functionViewModel.ParentId == "") functionViewModel.ParentId = null;
                    function.UpdateFunction(functionViewModel);
                    _functionService.Update(function);
                    _functionService.Save();

                    return request.CreateResponse(HttpStatusCode.OK, function);
                }
                catch (Exception dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [HttpDelete]
        [Route("delete")]
        public HttpResponseMessage Delete(HttpRequestMessage request, string id)
        {
            _functionService.Delete(id);
            _functionService.Save();

            return request.CreateResponse(HttpStatusCode.OK, id);
        }
    }
}