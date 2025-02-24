using System.Diagnostics;
using System.Security.Claims;
using Demo2.AppCode;
using Demo2.Models;
using Demo2.Models.DTO;
using Demo2.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Demo2.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly TestDbContext _dbContext;
        private readonly ITokenService _tokenService;

        public AccountController(IOptions<AppSettings> appSettings, TestDbContext dbContext, ITokenService tokenService)
        {
            _appSettings = appSettings.Value;
            _dbContext = dbContext;
            _tokenService = tokenService;
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
            Demo2.Models.SignInResult result = this.CheckPassword(model.Account, model.Password);

            if (result.SignInStatus == EmSignInStatus.Success && result.User is not null)
            {
                //1. 建立識別身份(Token)
                var token = _tokenService.BuildToken(
                    _appSettings.JwtSettings.SignKey,
                    _appSettings.JwtSettings.Issuer,
                    result.User);

                if (token != null)
                {
                    //2. 將 token 寫入 Session
                    HttpContext.Session.SetString("JWToken", token);
                    return RedirectToAction("WhoAmI");
                }
                else
                {
                    throw new Exception("Create Token Error");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "登入失敗");
                return View(model);
            }
        }

        private Demo2.Models.SignInResult CheckPassword(string account, string password)
        {
            Demo2.Models.SignInResult result = new();

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
                ViewBag.Message = $"Id={User.Identity.GetUserId()}, Account={User.Identity.GetAccount()}  ";

                string token = HttpContext.Session.GetString("JWToken") ?? "";

                if (_tokenService.IsTokenValid(
                    _appSettings.JwtSettings.SignKey,
                    _appSettings.JwtSettings.Issuer,
                    token))
                {
                    ViewBag.Message += BuildMessage(token, 50);
                }
                else
                {
                    ViewBag.Message += "Token not valid";
                }
            }
            else
            {
                ViewBag.Message = "Authenticated = false";
            }
            return View();
        }

        private string BuildMessage(string stringToSplit, int chunkSize)
        {
            var data = Enumerable.Range(0, stringToSplit.Length / chunkSize)
                .Select(i => stringToSplit.Substring(i * chunkSize, chunkSize));

            string result = "The generated token is:";

            foreach (string str in data)
            {
                result += Environment.NewLine + str;
            }

            return result;
        }

        [Authorize]
        public IActionResult UserInfo(int id)
        {
            Debug.WriteLine("IsAuthenticated = " + (User?.Identity?.IsAuthenticated == true ? "True" : "False"));

            var user = _dbContext.Users
                    .FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JWToken");
            return RedirectToAction("Login");
        }
    }
}
