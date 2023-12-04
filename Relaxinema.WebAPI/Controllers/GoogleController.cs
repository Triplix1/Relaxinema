using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.DTO;
using Relaxinema.Core.DTO.Authorization;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.ServiceContracts;
using Relaxinema.Core.Services;
using Relaxinema.WebAPI.Controllers.Base;

namespace Relaxinema.WebAPI.Controllers;

public class GoogleController : BaseController
{
    private readonly IGoogleOAuthService _googleOAuthService;
    private readonly JwtHelper _jwtHelper;
    private readonly ITokenService _tokenService;
    private readonly UserManager<User> _userManager;
    private const string RedirectUrl = "https://localhost:5001/GoogleOAuth/Code";
    private const string YouTubeScope = "https://www.googleapis.com/auth/youtube";
    private const string PkceSessionKey = "codeVerifier";

    public GoogleController(IGoogleOAuthService googleOAuthService, JwtHelper jwtHelper, ITokenService tokenService,
        UserManager<User> userManager)
    {
        _googleOAuthService = googleOAuthService;
        _jwtHelper = jwtHelper;
        _tokenService = tokenService;
        _userManager = userManager;
    }


    // public IActionResult RedirectOnOAuthServer()
    // {
    //     // PCKE.
    //     var codeVerifier = Guid.NewGuid().ToString();
    //     var codeChellange = Sha256Helper.ComputeHash(codeVerifier);
    //
    //     HttpContext.Session.SetString(PkceSessionKey, codeVerifier);
    //
    //     var url = _googleOAuthService.GenerateOAuthRequestUrl(YouTubeScope, RedirectUrl, codeChellange);
    //     return Redirect(url);
    // }

    
}