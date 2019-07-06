using System;

namespace Models.Checkpoints
{
    public class CheckpointCreationInfo
    {
        public string Name { get; }
        public CheckpointCreationInfo(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}