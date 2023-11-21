using Relaxinema.Core.MailConfig;

namespace Relaxinema.Core.ServiceContracts;

public interface IMailService
{
    Task SendHtmlAsync(MessageRequest messageRequest);
}