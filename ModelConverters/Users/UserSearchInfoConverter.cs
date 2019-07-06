using System;
using Model = Models.Users;
using Client = ClientModels.Users;

namespace ModelConverters.Users
{
    public class UserSearchInfoConverter
    {
        public static Model.UserSearchInfo Convert(Client.UserSearchInfo clientSearchInfo)
        {
            if (clientSearchInfo == null)
            {
                throw new ArgumentNullException(nameof(clientSearchInfo));
            }

            var modelSearchInfo = new Model.UserSearchInfo
            {
                Offset = clientSearchInfo.Offset,
                Limit = clientSearchInfo.Limit
            };

            return modelSearchInfo;
        }
    }
}