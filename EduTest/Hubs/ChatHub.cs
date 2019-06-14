using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduTest.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, object message)
        {
            await Clients.Others.SendAsync("ReceiveMessage", user, message);
        }
    }
}
