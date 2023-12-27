using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FPT_Exchange_Data.Entities
{
    [BsonIgnoreExtraElements]
    public class Notify
    {
        [BsonId]
        public string Id { get; set; }

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("createAt")]
        public DateTime CreateAt { get; set; }

        [BsonElement("sendTo")]
        public string? SendTo { get; set; }
    }
}
