using System;
using AspNetCore.Identity.Mongo.Model;

namespace Models.Users
{
    public class User : MongoUser
    {
        public DateTime RegisteredAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}