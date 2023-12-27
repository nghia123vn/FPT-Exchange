using FPT_Exchange_Data.DTO.Request.Post;
using System.Collections.Concurrent;

namespace FPT_Exchange_Service.Notify
{
    public interface INotifyService
    {
        public Task<bool> CreateNotification(CreateNotifyRequest request);
        public ConcurrentQueue<FPT_Exchange_Data.Entities.Notify> GetNotificationsFromSendTo(string id, int skipCount = 0);
    }
}
