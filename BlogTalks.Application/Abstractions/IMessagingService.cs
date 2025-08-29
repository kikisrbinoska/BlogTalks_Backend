using BlogTalks.EmailSenderApi.DTO;

namespace BlogTalks.Application.Abstractions;

public interface IMessagingService
{
    Task Send(EmailDTO emailDto); 
}