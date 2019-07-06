using System;

namespace Models.Checkpoints.Exceptions
{
    public class CheckpointNotFoundException : Exception
    {
        public CheckpointNotFoundException(string checkpointId)
            : base($"Checkpoint \"{checkpointId}\" not found.")
        {
        
        }
    }
}