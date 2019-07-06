using System;
using Model = Models.Vehicles;
using Client = ClientModels.Vehicles;

namespace ModelConverters.Vehicles
{
    public static class VehicleConverter
    {
        public static Client.Vehicle Convert(Model.Vehicle modelVehicle)
        {
            if (modelVehicle == null)
            {
                throw new ArgumentNullException(nameof(modelVehicle));
            }

            var clientTag = new Client.Vehicle
            {
                Id = modelVehicle.Id.ToString(), 
                Mark = modelVehicle.Mark,
                GovNumber = modelVehicle.GovNumber
            };
            
            return clientTag;
        }
    }
}