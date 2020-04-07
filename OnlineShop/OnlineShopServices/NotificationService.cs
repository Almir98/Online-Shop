using Microsoft.AspNetCore.SignalR;
using OnlineShopPodaci;
using OnlineShopPodaci.Hubs;
using System.Threading.Tasks;


namespace OnlineShopServices
{
    public class NotificationService : INotification
    {
        private readonly IHubContext<NotificationHub> hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public async Task SendNotification(string message)
        {
            await hubContext.Clients.All.SendAsync("OnMessageReceived", message);
        }
    }
}
