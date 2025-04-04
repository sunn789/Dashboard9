using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Modicom.Services.Configuration;


public class EmailService
{
    private readonly EmailSettings _emailConfig;

    public EmailService(IOptions<EmailSettings> emailConfig)
    {
        _emailConfig = emailConfig.Value;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        using var client = new SmtpClient(_emailConfig.SmtpServer, _emailConfig.SmtpPort)
        {
            Credentials = new NetworkCredential(
                _emailConfig.SenderEmail, 
                _emailConfig.SenderPassword),
            EnableSsl = true
        };
        
        await client.SendMailAsync(
            new MailMessage(_emailConfig.SenderEmail!, to, subject, body)
        );
    }
}
