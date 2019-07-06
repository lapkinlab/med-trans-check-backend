using System;
using ClientModels.Drivers;
using ClientModels.Routes;
using ClientModels.Vehicles;
using ClientModels.MechanicNotes;
using ClientModels.MedicNotes;

namespace ClientModels.WayBills
{
    public class WayBill
    {
        public string Id { get; set; }
        public string Serial { get; set; }
        public string Number { get; set; }
        public Driver Driver { get; set; }
        public Vehicle Vehicle { get; set; }
        public Route Route { get; set; }
        public MechanicNote MechanicNote { get; set; }
        public MedicNote[] MedicNotes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdateAt { get; set; }
    }
}