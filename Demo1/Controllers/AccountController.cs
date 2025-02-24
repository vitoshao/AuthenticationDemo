using System.Diagnostics;
using System.Security.Claims;
using Demo1.AppCode;
using Demo1.Models;
using Demo1.Models.DTO;
using Demo1.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Demo1.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly TestDbContext _dbContext;

        public AccountController(IOptions<AppSettings> appSettings, TestDbContext dbContext)
        {
            _appSettings = appSettings.Value;
            _dbContext = dbContext;
        }

        public IActionResult Login(string ReturnUrl = "")
        {
            if (User?.Identity?.IsAuthenticated ?? true)
            {
                if (!string.IsNullOrEmpty(ReturnUrl))
                    return Redirect(ReturnUrl);
                else
                    return RedirectToAction("WhoAmI", "Account");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(VmLogin model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //驗證登入
            Demo1.Models.SignInResult result = this.CheckPassword(model.Account, model.Password);

            if (result.SignInStatus == EmSignInStatus.Success && result.User is not null)
            {
                //1. 建立識別身份(ClaimsIdentity)
                Claim[] claims = [
                    new Claim("Id", result.User.Id.ToString()),
                    new Claim("Account", result.User.Account),
                ];
                ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);//Scheme必填
                ClaimsPrincipal claimsPrincipal = new(claimsIdentity);

                //2. 實現登入，SignInAsync會將使用者資訊存入cookie，以便後續寫入瀏覽器
                HttpContext.SignInAsync(claimsPrincipal);

                if (string.IsNullOrEmpty(model.ReturnUrl))
                    return RedirectToAction("Index", "Home");
                else
                    return Redirect(model.ReturnUrl);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "登入失敗");
                return View(model);
            }
        }

        private Demo1.Models.SignInResult CheckPassword(string account, string password)
        {
            Demo1.Models.SignInResult result = new();

            var user = _dbContext.Users.FirstOrDefault(x => x.Account == account);

            if (user == null)
            {
                result.SignInStatus = EmSignInStatus.AccountNotExist;
                result.Message = $"Invalid User => ACC:{account} PWD:{password}";
            }
            else
            {
                if (user.Password == password)
                {
                    result.SignInStatus = EmSignInStatus.Success;
                    result.User = new DtoLoginUser
                    {
                        Id = user.Id,
                        Account = user.Account,
                    };
                }
                else  //login failed
                {
                    result.SignInStatus = EmSignInStatus.PasswordError;
                    result.Message = $"Login Failed => ACC:{account} PWD:{password}";
                }
            }
            return result;
        }

        public IActionResult WhoAmI()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                ViewBag.Message = $"Id={User.Identity.GetUserId()}, Account={User.Identity.GetAccount()}";
            }
            else
            {
                ViewBag.Message = "Authenticated = false";
            }
            return View();
        }

        [Authorize]
        public IActionResult UserInfo(int id)
        {
            Debug.WriteLine("IsAuthenticated = " + (User?.Identity?.IsAuthenticated == true ? "True" : "False"));

            var user = _dbContext.Users
                    .FirstOrDefault(x => x.Id == id);

            if (user == null) {
                return NotFound();
            }

            return View(user);
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
