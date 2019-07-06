using System;
using Model = Models.WayBills;
using Client = ClientModels.WayBills;

namespace ModelConverters.WayBills
{
    public static class WayBillPatchInfoConverter
    {
        public static Model.WayBillPatchInfo Convert(Guid id, Client.WayBillPatchInfo clientPatchInfo)
        {
            if (clientPatchInfo == null)
            {
                throw new ArgumentNullException(nameof(clientPatchInfo));
            }

            Guid? driverId = null, vehicleId = null, routeId = null;

            if (clientPatchInfo.DriverId != null && Guid.TryParse(clientPatchInfo.DriverId, out var tmpDriver))
            {
                driverId = tmpDriver;
            }
            
            if (clientPatchInfo.VehicleId != null && Guid.TryParse(clientPatchInfo.VehicleId, out var tmpVehicle))
            {
                vehicleId = tmpVehicle;
            }
            
            if (clientPatchInfo.RouteId != null && Guid.TryParse(clientPatchInfo.RouteId, out var tmpRoute))
            {
                routeId = tmpRoute;
            }

            var modelPatchInfo = new Model.WayBillPatchInfo(id, clientPatchInfo.Serial, clientPatchInfo.Number,
                driverId, vehicleId, routeId);
            return modelPatchInfo;
        }
    }
}