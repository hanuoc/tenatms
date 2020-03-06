using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TMS.Service;
using TMS.Web.Infrastructure.Core;

namespace TMS.Web.Controllers
{
    [RoutePrefix("api/upload")]
    [Authorize]
    public class UploadController : ApiControllerBase
    {
        public UploadController(IErrorService errorService) : base(errorService)
        {
        }

        [HttpPost]
        [Route("saveImage")]
        public HttpResponseMessage SaveImage(string type = "")
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {

                var httpRequest = HttpContext.Current.Request;
                string directory = string.Empty;
                foreach (var file in httpRequest.Files.AllKeys)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png", ".docx", ".pdf", ".xlsx", ".txt", ".zip" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {

                            var message = string.Format("Please Upload image of type .jpg,.gif,.png,.docx,.pdf,.xlsx,.txt,.zip");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.MethodNotAllowed, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("Please Upload a file upto 1 mb.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.LengthRequired, dict);
                        }
                        else
                        {

                            if (type == "avatar")
                            {
                                directory = "/UploadedFiles/Avatars/";
                            }
                            else if (type == "product")
                            {
                                directory = "/UploadedFiles/Products/";
                            }
                            else if (type == "news")
                            {
                                directory = "/UploadedFiles/News/";
                            }
                            else if (type == "banner")
                            {
                                directory = "/UploadedFiles/Banners/";
                            }
                            else if (type == "sendmail")
                            {
                                directory = "/UploadedFiles/SendMail/";
                            }
                            else
                            {
                                directory = "/UploadedFiles/";
                            }
                            if (!Directory.Exists(HttpContext.Current.Server.MapPath(directory)))
                            {
                                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(directory));
                            }

                            string path = Path.Combine(HttpContext.Current.Server.MapPath(directory), postedFile.FileName);
                            string saveFile = Path.Combine(HttpContext.Current.Server.MapPath(directory), postedFile.FileName);
                            string url = HttpContext.Current.Server.MapPath(directory);
                            DirectoryInfo directoryInfo = new DirectoryInfo(url);
                            if (Directory.Exists(Path.GetDirectoryName(path)))
                            //if (File.Exists(path))
                            {
                                File.Delete(path);
                                //FileInfo fileInfo = new FileInfo(path);
                                //fileInfo.Delete();
                                postedFile.SaveAs(saveFile);
                            }
                            else
                                postedFile.SaveAs(path);
                            //Userimage myfolder name where i want to save my image
                            //return Request.CreateResponse(HttpStatusCode.OK, Path.Combine(directory, postedFile.FileName));
                        }
                    }
                }
                //return Request.CreateResponse(HttpStatusCode.OK, Path.Combine(directory, postedFile.FileName));

                var message1 = string.Format("Image Updated Successfully.");
                return Request.CreateErrorResponse(HttpStatusCode.Created, message1);
                //var res = string.Format("Please Upload a image.");
                //dict.Add("error", res);
                //return Request.CreateResponse(HttpStatusCode.NotFound, dict);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadGateway, ex); ;
            }
        }
    }
}