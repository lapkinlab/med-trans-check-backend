using System.Runtime.Serialization;

namespace ClientModels.Drivers
{
    public class DriverCreationInfo
    {
        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        [DataMember(IsRequired = true)]
        public string SerialNumberPass { get; set; }
        
        [DataMember(IsRequired = true)]
        public string PhoneNumber { get; set; }
    }
}