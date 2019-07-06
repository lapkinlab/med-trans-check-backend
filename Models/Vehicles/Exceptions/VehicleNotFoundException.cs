using System;

namespace Models.Vehicles.Exceptions
{
    public class VehicleNotFoundException : Exception
    {
        public VehicleNotFoundException(string vehicleId)
            : base($"Vehicle \"{vehicleId}\" not found.")
        {
        
        }
    }
}