

using Microsoft.AspNetCore.SignalR;

using System.Threading.Tasks;

namespace keef2.Hub_Config
{
    public class ReviewHub : Hub
    {
        //public void sendmessage(string name, string message)
        //{

        //    Clients.All.newmessage(name, message);
        //}
        public async Task sendMessage(string user, string message)
        => await Clients.All.SendAsync("ReceiveMessage", user, message);
        //public async Task askServer(string name,string textFromClient)
        //{
        //    string tempString;

        //    if (textFromClient == "hey")
        //    {
        //        tempString = "message was Hey";
        //    }
        //    else
        //    {
        //        tempString = "message was A7A";
        //    }
        //    await Clients.Clients(this.Context.ConnectionId).SendAsync("ask server response", tempString);
        //}
    }
}
