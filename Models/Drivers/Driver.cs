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
        
        [BsonElement("Mark")]
        [BsonRepresentation(BsonType.String)]
        public string Mark { get; set; }
        
        [BsonElement("GovNumber")]
        [BsonRepresentation(BsonType.String)]
        public string GovNumber { get; set; }
    }
}