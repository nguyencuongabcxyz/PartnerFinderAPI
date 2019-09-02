
namespace Service.Constants
{
    public class AuthenticationConstant
    {
        public static readonly string UserRole = "User";
        public static readonly string AdminRole = "Admin";
    }
   
    public enum AuthenticateUserResult
    {
        Succeeded,
        Invalid,
        Blocked
    }
}
