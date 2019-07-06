using System.Runtime.Serialization;

namespace ClientModels.Drivers
{
    public class DriverCreationInfo
    {
        [DataMember(IsRequired = true)]
        public string Mark { get; set; }

        [DataMember(IsRequired = true)]
        public string GovNumber { get; set; }
    }
}