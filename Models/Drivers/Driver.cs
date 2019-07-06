using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Drivers
{
    public class Driver
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        
        [BsonElement("Name")]
        [BsonRepresentation(BsonType.String)]
        public string Name { get; set; }
        
        [BsonElement("SerialNumberPass")]
        [BsonRepresentation(BsonType.String)]
        public string SerialNumberPass { get; set; }
        
        [BsonElement("PhoneNumber")]
        [BsonRepresentation(BsonType.String)]
        public string PhoneNumber { get; set; }
    }
}