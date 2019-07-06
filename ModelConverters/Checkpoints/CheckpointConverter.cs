using System;
using Model = Models.Checkpoints;
using Client = ClientModels.Checkpoints;

namespace ModelConverters.Checkpoints
{
    public static class CheckpointConverter
    {
        public static Client.Checkpoint Convert(Model.Checkpoint modelDriver)
        {
            if (modelDriver == null)
            {
                throw new ArgumentNullException(nameof(modelDriver));
            }

            var clientCheckpoint = new Client.Checkpoint
            {
                Id = modelDriver.Id.ToString(), 
                Name = modelDriver.Name
            };
            
            return clientCheckpoint;
        }
    }
}