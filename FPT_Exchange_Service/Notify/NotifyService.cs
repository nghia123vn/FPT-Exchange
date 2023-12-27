using FPT_Exchange_Data.DTO.Request.Post;
using FPT_Exchange_Utility.Helpers;
using FPT_Exchange_Utility.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Concurrent;

namespace FPT_Exchange_Service.Notify
{
    public class NotifyService : INotifyService
    {
        private readonly IMongoCollection<FPT_Exchange_Data.Entities.Notify> _notify;

        public NotifyService(IMongoClient mongoClient, IOptions<MongoDbSettings> options)
        {
            var settings = options.Value;
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _notify = database.GetCollection<FPT_Exchange_Data.Entities.Notify>(settings.CollectionName);
        }

        public async Task<bool> CreateNotification(CreateNotifyRequest request)
        {

            var checkValidUser = await ApiHelper.GetUser(request.SendTo);

            if (!checkValidUser.IsSuccessStatusCode) return false;

            var notification = new FPT_Exchange_Data.Entities.Notify
            {
                Id = Guid.NewGuid().ToString(),
                Description = request.Description,
                SendTo = request.SendTo,
            };

            _notify.InsertOne(notification);

            return true;
        }

        public ConcurrentQueue<FPT_Exchange_Data.Entities.Notify> GetNotificationsFromSendTo(string id, int skipCount = 0)
        {
            ConcurrentQueue<FPT_Exchange_Data.Entities.Notify> notifications = new ConcurrentQueue<FPT_Exchange_Data.Entities.Notify>();
            var filter = Builders<FPT_Exchange_Data.Entities.Notify>.Filter.Eq("sendTo", id);
            var notificationsFromDatabase = _notify.Find(filter).Skip(skipCount).ToList();

            foreach (var notification in notificationsFromDatabase)
            {
                notifications.Enqueue(notification);
            }
            return notifications;

        }
    }
}
