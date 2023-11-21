
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Relaxinema.Core.MailConfig;
using Relaxinema.Core.ServiceContracts;

namespace Relaxinema.Core.Services;

public class MailService : IMailService
{
    private readonly MailConfig.MailConfig _mailConfig;
    
    public MailService(IOptions<MailConfig.MailConfig> mailConfig)
    {
        _mailConfig = mailConfig.Value;
    }
    
    public async Task SendHtmlAsync(MessageRequest messageRequest)
    {
        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse(_mailConfig.Mail);
        email.To.AddRange(messageRequest.To.Select(email => MailboxAddress.Parse(email)));
        email.Subject = messageRequest.Subject;

        var builder = new BodyBuilder
        {
            HtmlBody = GetBody(messageRequest.Content)
        };
        
        email.Body = builder.ToMessageBody();
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_mailConfig.Host, _mailConfig.Port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_mailConfig.Mail, _mailConfig.Password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
    
    private string GetBody(string content)
    {
        return
            $@"<body style=""font-family: Arial, sans-serif; background-color: #ffcccc; text-align: center; padding: 20px;"">
            <h1 style=""color: #ff0000;"">Film realized!</h1>
            
            {content}
        </body>";
    }
    
    private async Task SendAsync(MimeMessage mailMessage)
    {
        using (var client = new SmtpClient())
        {
            try
            {
                await client.ConnectAsync(_mailConfig.Host, _mailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_mailConfig.DisplayName, _mailConfig.Password);
                await client.SendAsync(mailMessage);
            }
            catch
            {
                //log an error message or throw an exception or both.
                throw;
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
}