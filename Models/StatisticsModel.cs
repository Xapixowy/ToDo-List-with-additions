using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Runtime.InteropServices;

namespace ToDo_List_with_additions.Models
{
    public class StatisticsModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement]
        public string userId { get; set; }
        [BsonElement]
        public Dictionary<string, int> Done { get; set; }
        [BsonElement]
        public Dictionary<string, int> NotDone { get; set; }
        [BsonElement]
        public Dictionary<string, int> Postponed { get; set; }
    }
}
