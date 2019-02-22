using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MicrosoftGraphOneDriveSample.Models.Payload;
using MicrosoftGraphOneDriveSample.Utils;
using MicrosoftGraphOneDriveTest.Api;
using MicrosoftGraphOneDriveTest.Models;
using Newtonsoft.Json.Linq;

namespace MicrosoftGraphOneDriveTest.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IGraphApi _graphApi;

        public HomeController(IGraphApi graphApi)
        {
            _graphApi = graphApi;
        }

        public async Task<IActionResult> Index([FromServices]IConfiguration settings)
        {
            var user = GetUserInfo();

            if (user != null)
            {
                var getRootFilesProxy = await _graphApi.GetRootFiles(user.AccessToken);
                
                return View(getRootFilesProxy.Files.ToList());
            }
            
            string uri = 
                string.Format(
                    settings["Services:MS:AuthorizeEndpoint"],              //endpoint
                    settings["Services:MS:Host"],                           //host
                    settings["Services:MS:ClientId"],                       //client_id
                    settings["Services:MS:ResponseType"],                   //response_type
                    string.Format(
                        settings["Services:MS:RedirectUri"],                //redirect_uri
                        Request.Host.Value
                    ),            
                    settings["Services:MS:ResponseMode"],                   //response_mode
                    settings["Services:MS:Scope"],                          //scope
                    settings["Services:MS:State"]                           //state
                );

            return Redirect(uri);
        }
        
        public async Task<IActionResult> SharedWithMe([FromServices]IConfiguration settings)
        {
            var user = GetUserInfo();

            var getRootFilesProxy = await _graphApi.GetSharedWithMeFiles(user.AccessToken);
                
            return View(getRootFilesProxy.Files.ToList());
        
        }
        
        public async Task<IActionResult> Groups()
        {
            var user = GetUserInfo();

            var groups = await _graphApi.GetGroups(user.AccessToken);
                
            return View(groups.Value);
        
        }
        
        [HttpPost]
        public async Task<IActionResult> Share([FromBody]SharePayload payload)
        {
            var user = GetUserInfo();

            var getRootFilesProxy = await _graphApi.ShareToEmail(
                user.AccessToken, 
                payload.FileId, new ShareToEmailPayload()
                {
                    Recipients = new List<Recipient>()
                    {
                        new Recipient()
                        {
                            Email = payload.ShareEmail
                        }
                    },
                    Message = "Here's the file that we're collaborating on.",
                    RequireSignIn = true,
                    SendInvitation = true,
                    Roles = new List<string>()
                    {
                        "read"
                    }
                });

            return Ok();
        }

        
        public async Task<IActionResult> Upload()
        {
            return View();
        }
        
        [HttpPost("UploadFiles")]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {

            var user = GetUserInfo();
            var itemId = Guid.NewGuid();

         
            
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();
            
            var result = string.Empty;

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                        
                        var uploadSession = await _graphApi.CreateUploadSession(
                            user.AccessToken,
                            formFile.FileName,
                            new CreateUploadSession()
                            {
                                MicrosoftGraphConflictBehavior = "microsoft.graph.driveItemUploadableProperties",
                                Description = "description",
                                FileSystemInfo = new FileSystem()
                                {
                                    DataType = "microsoft.graph.fileSystemInfo"
                                },
                                Name = files.FirstOrDefault()?.FileName
                            });
                        
                        long position = 0;
                        long totalLength = stream.Length;
                        int length = 10 * 1024 * 1024;

                        while (true)
                        {
                            byte[] bytes = await ReadFileFragmentAsync(stream, position, length);
                            if (position >= totalLength)
                            {
                                break;
                            }

                            result = await UploadFileFragmentAsync(bytes, uploadSession.UploadUrl, position, totalLength, user.AccessToken);

                            position += bytes.Length;
                        }
                    }
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Redirect("/");
        }
        
        private async Task<string> UploadFileFragmentAsync(byte[] datas, string uploadUri, long position, long totalLength, string accessToken)
        {
            var request = await InitAuthRequest(uploadUri, HTTPMethod.Put, datas, null, accessToken);
            request.Request.Headers.Add("Content-Range", $"bytes {position}-{position + datas.Length - 1}/{totalLength}");

            return await request.GetResponseStringAsync();
        }
        
        protected async Task<ApiRequest> InitAuthRequest(string uri, HTTPMethod httpMethod, byte[] data, string contentType, string accessToken)
        {

            ApiRequest apiRequest = new ApiRequest(uri, httpMethod, data, contentType);

            if (!string.IsNullOrEmpty(accessToken))
            {
                apiRequest.Request.Headers.Add("Authorization", $"bearer {accessToken}");
            }

            return apiRequest;
        }
        
        private async Task<byte[]> ReadFileFragmentAsync(FileStream stream, long startPos, int count)
        {
            if (startPos >= stream.Length || startPos < 0 || count <= 0)
                return null;

            long trimCount = startPos + count > stream.Length ? stream.Length - startPos : count;

            byte[] retBytes = new byte[trimCount];
            stream.Seek(startPos, SeekOrigin.Begin);
            await stream.ReadAsync(retBytes, 0, (int)trimCount);
            return retBytes;
        }

        
    }
}