using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using Codebucket.Models.ViewModels;

namespace Codebucket.SocketHub
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            name = Context.User.Identity.Name;
            Clients.All.addNewMessageToPage(name , message);
        }
   
    }
}