using System.Runtime.Serialization;

namespace ClientModels.WayBills
{
    public class WayBillPatchInfo
    {
        [DataMember(IsRequired = false)]
        public string Serial { get; set; }
        
        [DataMember(IsRequired = false)]
        public string Number { get; set; }
        
        [DataMember(IsRequired = false)]
        public string DriverId { get; set; }
        
        [DataMember(IsRequired = false)]
        public string VehicleId { get; set; }
        
        [DataMember(IsRequired = false)]
        public string RouteId { get; set; }
    }
}