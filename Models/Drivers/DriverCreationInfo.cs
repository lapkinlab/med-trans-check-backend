using System;

namespace Models.Drivers
{
    public class DriverCreationInfo
    {
        public string Name { get; }
        public string SerialNumberPass { get; }
        public string PhoneNumber { get; }

        public DriverCreationInfo(string name, string serialNumberPass, string phoneNumber)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            SerialNumberPass = serialNumberPass ?? throw new ArgumentNullException(nameof(serialNumberPass));
            PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
        }
    }
}