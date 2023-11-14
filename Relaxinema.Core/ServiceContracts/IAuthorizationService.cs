﻿using Relaxinema.Core.DTO.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relaxinema.Core.ServiceContracts
{
    public interface IAuthorizationService
    {
        Task<AuthorizationResponse> RegisterUserAsync(RegisterDto registerDto);
        Task<AuthorizationResponse> LoginAsync(LoginDto loginDto);
    }
}