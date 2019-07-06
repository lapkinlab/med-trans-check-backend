using System.Runtime.Serialization;

namespace ClientModels.Routes
{
    public class RouteCreationInfo
    {
        [DataMember(IsRequired = true)]
        public string FromPlace { get; set; }
        
        [DataMember(IsRequired = true)]
        public string ToPlace { get; set; }
        
        [DataMember(IsRequired = true)]
        public string[] Checkpoints { get; set; }
    }
}