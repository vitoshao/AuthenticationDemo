using Demo1.Models.DTO;

namespace Demo1.Models
{
    public class SignInResult
    {
        public EmSignInStatus SignInStatus { get; set; }
        public DtoLoginUser? User { get; set; }
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
