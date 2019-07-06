using System;

namespace ClientModels.MechanicNotes
{
    public class MechanicNote
    {
        public string Id { get; set; }
        public bool[] Params { get; set; }
        public string MechanicName { get; set; }
        public string Permission { get; set; }
        public DateTime LastUpdateAt { get; set; }
    }
}