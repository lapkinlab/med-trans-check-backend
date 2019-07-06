using System;
using Model = Models.Users;
using Client = ClientModels.Users;

namespace ModelConverters.Users
{
    public static class UserConverter
    {
        public static Client.User Convert(Model.User modelUser)
        {
            if (modelUser == null)
            {
                throw new ArgumentNullException(nameof(modelUser));
            }

            var clientUser = new Client.User
            {
                Id = modelUser.Id,
                UserName = modelUser.NormalizedUserName,
                Name = modelUser.Name,
                Email = modelUser.Email,
                PhoneNumber = modelUser.PhoneNumber,
                Roles = modelUser.Roles,
                RegisteredAt = modelUser.RegisteredAt,
                LastUpdateAt = modelUser.LastUpdateAt
            };

            return clientUser;
        }
    }
}