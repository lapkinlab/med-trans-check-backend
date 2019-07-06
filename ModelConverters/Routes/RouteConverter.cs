using System;
using System.Collections.Generic;
using System.Linq;
using Models.Checkpoints;
using Model = Models.Routes;
using Client = ClientModels.Routes;

namespace ModelConverters.Routes
{
    public static class RouteConverter
    {
        public static Client.Route Convert(Model.Route modelRoute, IEnumerable<Checkpoint> modelCheckpoints)
        {
            if (modelRoute == null)
            {
                throw new ArgumentNullException(nameof(modelRoute));
            }

            var clientCheckpoints =
                ConverterUtils.ConvertModelCheckpointsToClientCheckpoints(modelRoute.Checkpoints, modelCheckpoints);
            
            var clientRoute = new Client.Route
            {
                Id = modelRoute.Id.ToString(), 
                FromPlace = modelRoute.FromPlace,
                ToPlace = modelRoute.ToPlace,
                Checkpoints = clientCheckpoints
            };
            
            return clientRoute;
        }
    }
}