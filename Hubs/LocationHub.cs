using Microsoft.AspNetCore.SignalR;

namespace HeavyGo_Project_Identity.Hubs
{
    public class LocationHub : Hub
    {
        public async Task SendLocation(string userId, double lat, double lon)
        {
            await Clients.Others.SendAsync("ReceiveLocation", userId, lat, lon);
        }

    }
}
