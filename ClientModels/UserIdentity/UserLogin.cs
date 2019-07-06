using System.Runtime.Serialization;

namespace ClientModels.UserIdentity
{
    public class UserLogin
    {
        [DataMember(IsRequired = true)]
        public string Username { get; set; }

        [DataMember(IsRequired = true)]
        public string Password { get; set; }

        [DataMember(IsRequired = true)]
        public bool RememberMe { get; set; }
    }
}