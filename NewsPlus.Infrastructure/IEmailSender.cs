using NewsPlus.Data.ViewModel;

namespace NewsPlus.Infrastructure
{
    public interface IEmailSender
    {
        void SendEmail(MessageDto message);
        Task SendEmailAsync(MessageDto message);
    }
}
