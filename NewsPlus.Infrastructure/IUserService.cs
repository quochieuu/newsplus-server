using NewsPlus.Data.Entities;
using NewsPlus.Data.ViewModel;

namespace NewsPlus.Infrastructure
{
    public interface IUserService
    {
        bool IsAnExistingUser(string userName);
        bool IsValidUserCredentials(string userName, string password);
        string GetUserRole(string userName);
        void CreateUser(RegisterRequest request);
        bool SendPasswordResetCodeEmail(string username, string code);
        bool ResetUserPasswordEmailCode(string username, PasswordResetEmailCodeDto request);
        SysAppUser FindUser(string email);
        void SendPasswordResetLinkEmail(string username, string link, string code);
        string FindEmailByCode(string code);
        void VerifyEmail(string email);
    }
}
