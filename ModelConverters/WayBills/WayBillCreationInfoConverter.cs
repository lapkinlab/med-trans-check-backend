using System;
using System.IO;
using Model = Models.WayBills;
using Client = ClientModels.WayBills;

namespace ModelConverters.WayBills
{
    public static class WayBillCreationInfoConverter
    {
        public static Model.WayBillCreationInfo Convert(Client.WayBillCreationInfo clientCreationInfo)
        {
            if (clientCreationInfo == null)
            {
                throw new ArgumentNullException(nameof(clientCreationInfo));
            }

            if (!Guid.TryParse(clientCreationInfo.DriverId, out var driverId))
            {
                throw new InvalidDataException($"{nameof(clientCreationInfo.DriverId)} must be Guid.");
            }
            
            if (!Guid.TryParse(clientCreationInfo.VehicleId, out var vehicleId))
            {
                throw new InvalidDataException($"{nameof(clientCreationInfo.VehicleId)} must be Guid.");
            }
            
            if (!Guid.TryParse(clientCreationInfo.RouteId, out var routeId))
            {
                throw new InvalidDataException($"{nameof(clientCreationInfo.RouteId)} must be Guid.");
            }
            
            var modelCreationInfo = new Model.WayBillCreationInfo(clientCreationInfo.Serial, clientCreationInfo.Number,
                driverId, vehicleId, routeId);
            return modelCreationInfo;
        }
    }
}