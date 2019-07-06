using System;
using AspNetCore.Identity.Mongo.Model;

namespace Models.Users
{
    public class User : MongoUser
    {
        public string Name { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime LastUpdateAt { get; set; }
    }
}