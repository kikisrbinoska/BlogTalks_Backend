using BlogTalks.EmailSenderApi.DTO;

namespace BlogTalks.EmailSenderApi.Services
{
    public interface IEmailSender
    {
        Task Send(EmailDTO request);
    }
}
