using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ModelConverters.Checkpoints;

namespace ModelConverters
{
    public static class ConverterUtils
    {
        public static ClientModels.Checkpoints.Checkpoint[] ConvertModelCheckpointsToClientCheckpoints(
            IEnumerable<Guid> ids, IEnumerable<Models.Checkpoints.Checkpoint> modelCheckpoints)
        {
            if (ids == null)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            if (modelCheckpoints == null)
            {
                throw new ArgumentNullException(nameof(modelCheckpoints));
            }

            var clientCheckpoints = modelCheckpoints.Where(item => ids.Contains(item.Id))
                .Select(CheckpointConverter.Convert)
                .ToArray();
            return clientCheckpoints;
        }
    }
}