using System;
using Model = Models.Checkpoints;
using Client = ClientModels.Checkpoints;

namespace ModelConverters.Checkpoints
{
    public static class CheckpointConverter
    {
        public static Client.Checkpoint Convert(Model.Checkpoint modelCheckpoint)
        {
            if (modelCheckpoint == null)
            {
                throw new ArgumentNullException(nameof(modelCheckpoint));
            }

            var clientCheckpoint = new Client.Checkpoint
            {
                Id = modelCheckpoint.Id.ToString(), 
                Name = modelCheckpoint.Name
            };
            
            return clientCheckpoint;
        }
    }
}