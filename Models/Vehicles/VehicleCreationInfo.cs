using System;

namespace Models.Vehicles
{
    public class VehicleCreationInfo
    {
        public string Mark { get; }
        public string GovNumber { get; }

        public VehicleCreationInfo(string mark, string govNumber)
        {
            Mark = mark ?? throw new ArgumentNullException(nameof(mark));
            GovNumber = govNumber ?? throw new ArgumentNullException(nameof(govNumber));
        }
    }
}