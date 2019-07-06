using System.Runtime.Serialization;

namespace ClientModels.Checkpoints
{
    public class CheckpointCreationInfo
    {
        [DataMember(IsRequired = true)]
        public string Name { get; set; }
    }
}