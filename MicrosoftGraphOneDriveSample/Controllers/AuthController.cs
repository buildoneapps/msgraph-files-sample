using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MicrosoftGraphOneDriveSample.Models;
using MicrosoftGraphOneDriveTest.Api;
using MicrosoftGraphOneDriveTest.Models;
using Newtonsoft.Json;

namespace MicrosoftGraphOneDriveTest.Controllers
{
    public class AuthController : Controller
    {
        private readonly IGraphApi _graphApi;

        public AuthController(IGraphApi graphApi)
        {
            _graphApi = graphApi;
        }
        //Get access on behalf of a user
        //https://developer.microsoft.com/en-us/graph/docs/concepts/auth_v2_user

        public async Task<IActionResult> Access([FromServices]IConfiguration settings, string access_token, string refresh_token)
        {

            var accessToken = Request.Query["access_token"];
            var refreshToken = Request.Query["refresh_token"];

            var meProxy = await _graphApi.GetMe(access_token);
            
            var user = new UserInfoViewModel()
            {
                AccessToken = accessToken,
                Id = meProxy.Id,
                Name = meProxy.DisplayName,
                Email = meProxy.Mail
            };

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, JsonConvert.SerializeObject(user)),
            };

            var userIdentity = new ClaimsIdentity(claims, "login");

            var principal = new ClaimsPrincipal(userIdentity);
                
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                principal);
            
            return Redirect("~/");
        }

        public async Task<IActionResult> Index([FromServices]IConfiguration settings)
        {
            string code = Request.Query["code"];

            var data = new List<KeyValuePair<string, string>>();
            data.Add(new KeyValuePair<string, string>("client_id", settings["Services:MS:ClientId"]));
            data.Add(new KeyValuePair<string, string>("scope", settings["Services:MS:Scope"]));
            data.Add(new KeyValuePair<string, string>("code", code));
            data.Add(new KeyValuePair<string, string>("redirect_uri", $"{Request.Scheme}://{Request.Host.Value}/auth"));
            data.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
            data.Add(new KeyValuePair<string, string>("client_secret", settings["Services:MS:ClientSecret"]));

            using (var http = new HttpClient())
            {
                var result = await http.PostAsync(string.Format(settings["Services:MS:TokenEndpoint"], settings["Services:MS:Host"]),
                   new FormUrlEncodedContent(data));

                string content = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<TokenResponse>(content);

                return Redirect(
                    string.Format(settings["Services:WebApp:AuthEndpoint"],
                        Request.Host.Value,
                    response.AccessToken,
                    response.RefreshToken
                    ));
            }
        }
    }
}




