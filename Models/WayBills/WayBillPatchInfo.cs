using System;

namespace Models.WayBills
{
    public class WayBillPatchInfo
    {
        public Guid Id { get; }
        public string Serial { get; set; }
        public string Number { get; set; }
        public Guid? DriverId { get; set; }
        public Guid? VehicleId { get; set; }
        public Guid? RouteId { get; set; }

        public WayBillPatchInfo(Guid id, string serial = null, string number = null, Guid? driverId = null,
            Guid? vehicleId = null, Guid? routeId = null)
        {
            Id = id;
            Serial = serial;
            Number = number;
            DriverId = driverId;
            VehicleId = vehicleId;
            RouteId = routeId;
        }
    }
}