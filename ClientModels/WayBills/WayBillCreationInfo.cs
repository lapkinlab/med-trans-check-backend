using System.Runtime.Serialization;

namespace ClientModels.WayBills
{
    public class WayBillCreationInfo
    {
        [DataMember(IsRequired = true)]
        public string Serial { get; set; }
        
        [DataMember(IsRequired = true)]
        public string Number { get; set; }
        
        [DataMember(IsRequired = true)]
        public string DriverId { get; set; }
        
        [DataMember(IsRequired = true)]
        public string VehicleId { get; set; }
        
        [DataMember(IsRequired = true)]
        public string RouteId { get; set; }
    }
}