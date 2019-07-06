using System.Runtime.Serialization;

namespace ClientModels.Roles
{
    public class RolePatchInfo
    {
        [DataMember(IsRequired = true)]
        public string UserRole { get; set; }
    }
}