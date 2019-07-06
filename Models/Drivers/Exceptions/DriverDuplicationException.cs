using System;

namespace Models.Drivers.Exceptions
{
    public class DriverDuplicationException : Exception
    {
        public DriverDuplicationException(string driverId)
            : base($"Driver \"{driverId}\" already exists.")
        {
        
        }
    }
}