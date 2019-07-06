using System;

namespace Models.Users
{
    public class UserCreationInfo
    {
        public string UserName { get; }
        public string Name { get; }
        public string Email { get; }
        public string PhoneNumber { get; }
        public string Password { get; }

        public UserCreationInfo(string userName, string name, string email, string phoneNumber, string password)
        {
            UserName = userName?.ToLower() ?? throw new ArgumentNullException(nameof(userName));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
            Password = password ?? throw new ArgumentNullException(nameof(password));
        }
    }
}