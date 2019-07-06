using System;
using Model = Models.Drivers;
using Client = ClientModels.Drivers;

namespace ModelConverters.Drivers
{
    public static class DriverCreationInfoConverter
    {
        public static Model.DriverCreationInfo Convert(Client.DriverCreationInfo clientCreationInfo)
        {
            if (clientCreationInfo == null)
            {
                throw new ArgumentNullException(nameof(clientCreationInfo));
            }

            var modelCreationInfo = new Model.DriverCreationInfo(clientCreationInfo.Mark, clientCreationInfo.GovNumber);
            return modelCreationInfo;
        }
    }
}