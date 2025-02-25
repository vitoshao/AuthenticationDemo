using System.Diagnostics;
using Demo.DAL;
using Demo3.AppCode;
using Demo3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Demo3.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
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

        [HttpPost]
        public IResult SignIn(LoginDto model)
        {
            //驗證登入
            Demo3.Models.SignInResult result = this.CheckPassword(model.Account, model.Password);

            if (result.SignInStatus == EmSignInStatus.Success && result.User is not null)
            {
                //1. 建立識別身份(Token)
                var token = _tokenService.BuildToken(
                    _appSettings.JwtSettings.SignKey,
                    _appSettings.JwtSettings.Issuer,
                    _appSettings.JwtSettings.Audience,
                    result.User);

                if (token != null)
                {
                    return Results.Ok(new { token });
                }
                else
                {
                    Debug.WriteLine("Create Token Error");
                    return Results.Empty;
                }
            }
            else
            {
                Debug.WriteLine("SignIn Failed");
                return Results.Empty;
            }
        }

        private Demo3.Models.SignInResult CheckPassword(string account, string password)
        {
            Demo3.Models.SignInResult result = new();

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
                    result.User = new LoginUserDto
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

        [Authorize]
        [HttpPost]
        public IActionResult UserInfo(int id)
        {
            Debug.WriteLine("IsAuthenticated = " + (User?.Identity?.IsAuthenticated == true ? "True" : "False"));

            var user = _dbContext.Users
                    .FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return Json(user);
        }

    }

}
