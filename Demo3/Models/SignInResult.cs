using Demo3.Models;

namespace Demo3.Models
{
    public class SignInResult
    {
        public EmSignInStatus SignInStatus { get; set; }
        public LoginUserDto? User { get; set; }
        public string? Message { get; set; }
    }

    public enum EmSignInStatus
    {
        Success = 0,
        CmpcodeNotExist = 1,
        AccountNotExist = 2,
        InputInvalid = 3,
        LockedOut = 4,
        AccountDuplicate = 5,
        PasswordError = 6,
        AccountCaseProblem = 7,
    }
}
