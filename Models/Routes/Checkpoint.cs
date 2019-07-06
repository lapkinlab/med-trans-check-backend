using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Routes
{
    public class Route
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        
        [BsonElement("FromPlace")]
        [BsonRepresentation(BsonType.String)]
        public string FromPlace { get; set; }
        
        [BsonElement("ToPlace")]
        [BsonRepresentation(BsonType.String)]
        public string ToPlace { get; set; }
        
        [BsonElement("Checkpoints")]
        public IReadOnlyList<Guid> Checkpoints { get; set; }
    }
}