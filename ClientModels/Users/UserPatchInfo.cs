using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ClientModels.Users
{
    public class UserPatchInfo
    {
        [DataMember(IsRequired = false)]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [DataMember(IsRequired = false)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataMember(IsRequired = false)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}