using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.MedicNotes
{
    public class MedicNote
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        
        [BsonElement("Complaints")]
        [BsonRepresentation(BsonType.String)]
        public string Complaints { get; set; }
        
        [BsonElement("Temperature")]
        [BsonRepresentation(BsonType.String)]
        public string Temperature { get; set; }
        
        [BsonElement("Pressure")]
        [BsonRepresentation(BsonType.String)]
        public string Pressure { get; set; }
        
        [BsonElement("Pulse")]
        [BsonRepresentation(BsonType.String)]
        public string Pulse { get; set; }
        
        [BsonElement("AlcoholIntoxication")]
        [BsonRepresentation(BsonType.Boolean)]
        public bool AlcoholIntoxication { get; set; }
        
        [BsonElement("DrugIntoxication")]
        [BsonRepresentation(BsonType.Boolean)]
        public bool DrugIntoxication { get; set; }
        
        [BsonElement("MedicName")]
        [BsonRepresentation(BsonType.String)]
        public string MedicName { get; set; }
        
        [BsonElement("Permission")]
        [BsonRepresentation(BsonType.String)]
        public Permission Permission { get; set; }
        
        [BsonElement("LastUpdateAt")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime LastUpdateAt { get; set; }
    }
}