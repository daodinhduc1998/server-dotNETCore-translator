using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace TranslatorWebAPI.Models
{
    public class Dictionary
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        public string name { get; set; }

        public string key { get; set; }
        public string[] value { get; set; }
        public string[] tag { get; set; }

        public bool deleted { get; set; }

        public Int32 __v { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }

    }
}
