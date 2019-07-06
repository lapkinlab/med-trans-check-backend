using System;

namespace Models.WayBills
{
    public class WayBillCreationInfo
    {
        public string Serial { get; set; }
        public string Number { get; set; }
        public Guid DriverId { get; set; }
        public Guid VehicleId { get; set; }
        public Guid RouteId { get; set; }

        public WayBillCreationInfo(string serial, string number, Guid driverId, Guid vehicleId, Guid routeId)
        {
            Serial = serial ?? throw new ArgumentNullException(nameof(serial));
            Number = number ?? throw new ArgumentNullException(nameof(number));
            DriverId = driverId;
            VehicleId = vehicleId;
            RouteId = routeId;
        }
    }
}