using System;
using Model = Models.Users;
using Client = ClientModels.Users;

namespace ModelConverters.Users
{
    public static class UserCreationInfoConverter
    {
        public static Model.UserCreationInfo Convert(Client.UserCreationInfo clientCreationInfo)
        {
            if (clientCreationInfo == null)
            {
                throw new ArgumentNullException(nameof(clientCreationInfo));
            }

            if (string.IsNullOrEmpty(clientCreationInfo.UserName))
            {
                throw new ArgumentNullException(nameof(clientCreationInfo.UserName));
            }
            
            if (string.IsNullOrEmpty(clientCreationInfo.Name))
            {
                throw new ArgumentNullException(nameof(clientCreationInfo.Name));
            }

            if (string.IsNullOrEmpty(clientCreationInfo.Email))
            {
                throw new ArgumentNullException(nameof(clientCreationInfo.Email));
            }

            if (string.IsNullOrEmpty(clientCreationInfo.PhoneNumber))
            {
                throw new ArgumentNullException(nameof(clientCreationInfo.PhoneNumber));
            }

            if (string.IsNullOrEmpty(clientCreationInfo.Password))
            {
                throw new ArgumentNullException(nameof(clientCreationInfo.Password));
            }

            var modelCreationInfo = new Model.UserCreationInfo(
                clientCreationInfo.UserName, 
                clientCreationInfo.Name,
                clientCreationInfo.Email,
                clientCreationInfo.PhoneNumber,
                clientCreationInfo.Password
            );

            return modelCreationInfo;
        }
    }
}