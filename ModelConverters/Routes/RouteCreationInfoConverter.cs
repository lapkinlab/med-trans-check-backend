using System;
using System.Linq;
using Model = Models.Routes;
using Client = ClientModels.Routes;

namespace ModelConverters.Routes
{
    public static class RouteCreationInfoConverter
    {
        public static Model.RouteCreationInfo Convert(Client.RouteCreationInfo clientCreationInfo)
        {
            if (clientCreationInfo == null)
            {
                throw new ArgumentNullException(nameof(clientCreationInfo));
            }

            //todo сделать по умному, с игнорированием некорректных идов
            var guids = clientCreationInfo.Checkpoints.Select(Guid.Parse).ToList();
            
            var modelCreationInfo = new Model.RouteCreationInfo(clientCreationInfo.FromPlace, clientCreationInfo.ToPlace,
                guids);
            return modelCreationInfo;
        }
    }
}