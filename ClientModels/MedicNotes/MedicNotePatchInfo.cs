using System.Runtime.Serialization;

namespace ClientModels.MedicNotes
{
    public class MedicNotePatchInfo
    {
        [DataMember(IsRequired = false)]
        public string Complaints { get; set; }
        
        [DataMember(IsRequired = false)]
        public string Temperature { get; set; }
        
        [DataMember(IsRequired = false)]
        public string Pressure { get; set; }
        
        [DataMember(IsRequired = false)]
        public string Pulse { get; set; }
        
        [DataMember(IsRequired = false)]
        public bool AlcoholIntoxication { get; set; }
        
        [DataMember(IsRequired = false)]
        public bool DrugIntoxication { get; set; }
        
        [DataMember(IsRequired = false)]
        public string Permission { get; set; }
    }
}