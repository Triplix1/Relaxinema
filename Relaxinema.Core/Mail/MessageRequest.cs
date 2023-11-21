using MimeKit;

namespace Relaxinema.Core.MailConfig;

public class MessageRequest
{
    public List<string> To { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
}