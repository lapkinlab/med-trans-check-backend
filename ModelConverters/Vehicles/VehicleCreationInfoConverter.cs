using System;
using Model = Models.Vehicles;
using Client = ClientModels.Vehicles;

namespace ModelConverters.Vehicles
{
    public static class VehicleCreationInfoConverter
    {
        public static Model.VehicleCreationInfo Convert(Client.VehicleCreationInfo clientCreationInfo)
        {
            if (clientCreationInfo == null)
            {
                throw new ArgumentNullException(nameof(clientCreationInfo));
            }

            var modelCreationInfo = new Model.VehicleCreationInfo(clientCreationInfo.Mark, clientCreationInfo.GovNumber);
            return modelCreationInfo;
        }
    }
}