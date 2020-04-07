using System.Threading.Tasks;

namespace OnlineShopPodaci
{
    public interface INotification
    {
        Task SendNotification(string messsage);

    }
}
