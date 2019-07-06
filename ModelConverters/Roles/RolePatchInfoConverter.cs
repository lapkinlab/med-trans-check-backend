using System;
using Model = Models.Roles;
using Client = ClientModels.Roles;

namespace ModelConverters.Roles
{
    public static class RolePatchInfoConverter
    {
        public static Model.RolePatchInfo Convert(string userName, Client.RolePatchInfo clientRolePatchInfo)
        {
            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }

            if (clientRolePatchInfo == null)
            {
                throw new ArgumentNullException(nameof(clientRolePatchInfo));
            }

            var modelRolePatchInfo = new Model.RolePatchInfo(userName, clientRolePatchInfo.UserRole);

            return modelRolePatchInfo;
        }
    }
}