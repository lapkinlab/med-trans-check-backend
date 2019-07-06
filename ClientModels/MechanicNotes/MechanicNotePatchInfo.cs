using System.Runtime.Serialization;

namespace ClientModels.MechanicNotes
{
    public class MechanicNotePatchInfo
    {
        [DataMember(IsRequired = false)]
        public bool[] Params { get; set; }
        
        [DataMember(IsRequired = false)]
        public string Permission { get; set; }
    }
}