using System;
using System.Collections.Generic;

namespace ClientModels.Users
{
    public class User
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public IReadOnlyList<string> Roles { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime LastUpdateAt { get; set; }
    }
}