
using Demo3.Models;

namespace Demo3.AppCode
{
    public interface ITokenService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="issuer"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        string BuildToken(string key, string issuer, string audience, LoginUserDto user);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="issuer"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        bool IsTokenValid(string key, string issuer, string token);
    }
}
