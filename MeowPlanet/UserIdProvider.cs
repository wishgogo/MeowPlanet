﻿using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace MeowPlanet
{
    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {


            return connection.User?.FindFirst(ClaimTypes.Sid)?.Value!;
        }
    }
}
