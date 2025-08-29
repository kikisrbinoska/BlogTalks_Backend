using BlogTalks.EmailSenderApi.DTO;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System.Net.Mail;

namespace BlogTalks.EmailSenderApi.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task Send(EmailDTO request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(request.From));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync(_config["EmailHost"], 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config["EmailUsername"], _config["EmailPassword"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

        }
    }
}
