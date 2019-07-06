using System;
using Model = Models.Checkpoints;
using Client = ClientModels.Checkpoints;

namespace ModelConverters.Checkpoints
{
    public static class CheckpointCreationInfoConverter
    {
        public static Model.CheckpointCreationInfo Convert(Client.CheckpointCreationInfo clientCreationInfo)
        {
            if (clientCreationInfo == null)
            {
                throw new ArgumentNullException(nameof(clientCreationInfo));
            }

            var modelCreationInfo = new Model.CheckpointCreationInfo(clientCreationInfo.Name);
            return modelCreationInfo;
        }
    }
}