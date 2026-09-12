using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace exam_system.Features.Identity.Register
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);

    }

    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(
                "Exam System",
                "ahmed.backend33@gmail.com"));

            email.To.Add(MailboxAddress.Parse(to));

            email.Subject = subject;

            email.Body = new TextPart("plain")
            {
                Text = body
            };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                "smtp.gmail.com",
                587,
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                "ahmed.backend33@gmail.com",
                "zwayuplorbwjhoeb");

            await smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);
        }
    }
}
