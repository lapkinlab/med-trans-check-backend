using System;

namespace Models.Drivers.Exceptions
{
    public class DriverNotFoundException : Exception
    {
        public DriverNotFoundException(string driverId)
            : base($"Driver \"{driverId}\" not found.")
        {
        
        }
    }
}