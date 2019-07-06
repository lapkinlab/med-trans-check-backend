using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.MechanicNotes
{
    public class MechanicNote
    {
        public const int ParamsCount = 41;
        
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        
        [BsonElement("Params")]
        public IReadOnlyList<bool> Params { get; set; }
        
        [BsonElement("MechanicName")]
        [BsonRepresentation(BsonType.String)]
        public string MechanicName { get; set; }
        
        [BsonElement("Permission")]
        [BsonRepresentation(BsonType.String)]
        public Permission Permission { get; set; }
        
        [BsonElement("LastUpdateAt")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime LastUpdateAt { get; set; }
    }
}