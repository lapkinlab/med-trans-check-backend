using System.Runtime.Serialization;

namespace ClientModels.Vehicles
{
    public class VehicleCreationInfo
    {
        [DataMember(IsRequired = true)]
        public string Mark { get; set; }

        [DataMember(IsRequired = true)]
        public string GovNumber { get; set; }
    }
}