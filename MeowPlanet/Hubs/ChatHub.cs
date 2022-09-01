using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

using Microsoft.EntityFrameworkCore;
using MeowPlanet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeowPlanet.Hubs
{
    public class ChatHub : Hub
    {

        public Task SendMessage(string userName, string userPhoto, string message, string receivedId, int senderId)
        {
            return Clients.User(receivedId).SendAsync("ReceiveMessage", userName, userPhoto, message, senderId);
        }

    }
}
