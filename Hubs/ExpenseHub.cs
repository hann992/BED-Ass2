using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BEDAssignment2.Hubs
{
    public class ExpenseHub : Hub
    {
        public async Task SendMessage()
        {
            await Clients.All.SendAsync("ReceiveMessage");
        }
    }
}