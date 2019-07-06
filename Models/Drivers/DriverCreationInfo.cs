using System;

namespace Models.Drivers
{
    public class DriverCreationInfo
    {
        public string Mark { get; }
        public string GovNumber { get; }

        public DriverCreationInfo(string mark, string govNumber)
        {
            Mark = mark ?? throw new ArgumentNullException(nameof(mark));
            GovNumber = govNumber ?? throw new ArgumentNullException(nameof(govNumber));
        }
    }
}