using System;
using System.Collections.Generic;
using Models.Drivers;
using Models.MechanicNotes;
using Models.MedicNotes;
using Models.Routes;
using Models.Vehicles;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.WayBills
{
    public class WayBill
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        
        [BsonElement("Serial")]
        [BsonRepresentation(BsonType.String)]
        public string Serial { get; set; }

        [BsonElement("Number")]
        [BsonRepresentation(BsonType.String)]
        public string Number { get; set; }
        
        [BsonElement("Driver")]
        [BsonRepresentation(BsonType.String)]
        public Guid Driver { get; set; }
        
        [BsonElement("Vehicle")]
        [BsonRepresentation(BsonType.String)]
        public Guid Vehicle { get; set; }
        
        [BsonElement("Route")]
        [BsonRepresentation(BsonType.String)]
        public Guid Route { get; set; }
        
        [BsonElement("MechanicNote")]
        [BsonRepresentation(BsonType.String)]
        public Guid MechanicNote { get; set; }
        
        [BsonElement("MedicNotes")]
        public IReadOnlyList<Guid> MedicNotes { get; set; }
        
        [BsonElement("CreatedAt")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; }
        
        [BsonElement("LastUpdateAt")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime LastUpdateAt { get; set; }
    }
}