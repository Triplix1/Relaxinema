using Microsoft.AspNetCore.Mvc;
using Relaxinema.Core.MailConfig;
using Relaxinema.Core.ServiceContracts;
using Relaxinema.WebAPI.Controllers.Base;

namespace Relaxinema.WebAPI.Controllers;

public class MailController : BaseController
{
    private readonly IMailService _mailService;

    public MailController(IMailService mailService)
    {
        _mailService = mailService;
    }
    
    [HttpPost]
    public async Task<ActionResult> SendMail(MessageRequest mailRequest)
    {
        await _mailService.SendHtmlAsync(mailRequest);
        return Ok();
    }
}