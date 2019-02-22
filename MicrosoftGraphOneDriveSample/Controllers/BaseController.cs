using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MicrosoftGraphOneDriveSample.Models;
using MicrosoftGraphOneDriveTest.Models;
using Newtonsoft.Json;

namespace MicrosoftGraphOneDriveTest.Controllers
{
    public class BaseController : Controller
    {

        public UserInfoViewModel GetUserInfo()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var loggedInUser = HttpContext.User;
                return JsonConvert.DeserializeObject<UserInfoViewModel>(loggedInUser.Identity.Name);
            }

            return null;
        }

        public async Task SaveUserInfo(UserInfoViewModel user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, JsonConvert.SerializeObject(user)),
            };

            var userIdentity = new ClaimsIdentity(claims, "login");

            var principal = new ClaimsPrincipal(userIdentity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);
        }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userInfo = GetUserInfo();
            if (userInfo != null)
            {
                ViewData["UserInfo"] = userInfo;
            }
        }
      

        /// <summary>
        ///     Run <paramref name="predicate" /> under default statement.
        /// </summary>
        /// <param name="predicate">Function to be ran.</param>
        /// <returns><paramref name="predicate" /> return or default return in case of an error has been thrown.</returns>
        protected async Task<IActionResult> RunDefaultAsync(
            Func<Task<IActionResult>> predicate)
        {
            try
            {
                return await predicate();
            }
            catch (System.Exception exception)
            {
                return Redirect("~/home/error");
            }
        }

        /// <summary>
        ///     Run <paramref name="predicate" /> under default statement.
        /// </summary>
        /// <param name="predicate">Function to be ran.</param>
        /// <returns><paramref name="predicate" /> return or default return in case of an error has been thrown.</returns>
        protected IActionResult RunDefault(
            Func<IActionResult> predicate)
        {
            try
            {
                return predicate();
            }

            catch (System.Exception exception)
            {
                return Redirect("~/home/error");
            }
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}