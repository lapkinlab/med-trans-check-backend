using System;
using Model = Models.Users;
using Client = ClientModels.Users;

namespace ModelConverters.Users
{
    public static class UserPatchInfoConverter
    {
        public static Model.UserPatchInfo Convert(string username, Client.UserPatchInfo clientPatchInfo)
        {
            if (clientPatchInfo == null)
            {
                throw new ArgumentNullException(nameof(clientPatchInfo));
            }

            var modelPatchInfo = new Model.UserPatchInfo(username)
            {
                OldPassword = clientPatchInfo.OldPassword,
                Password = clientPatchInfo.Password,
                ConfirmPassword = clientPatchInfo.ConfirmPassword
            };

            return modelPatchInfo;
        }
    }
}