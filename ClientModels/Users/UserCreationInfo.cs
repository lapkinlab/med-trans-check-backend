using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ClientModels.Users
{
    public class UserCreationInfo
    {
        [DataMember(IsRequired = true)]
        public string UserName { get; set; }
        
        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        [DataMember(IsRequired = true)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataMember(IsRequired = true)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DataMember(IsRequired = true)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataMember(IsRequired = true)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}