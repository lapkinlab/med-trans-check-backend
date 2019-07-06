using System;

namespace ClientModels.MedicNotes
{
    public class MedicNote
    {
        public string Id { get; set; }
        public string Complaints { get; set; }
        public string Temperature { get; set; }
        public string Pressure { get; set; }
        public string Pulse { get; set; }
        public bool AlcoholIntoxication { get; set; }
        public bool DrugIntoxication { get; set; }
        public string MedicName { get; set; }
        public string Permission { get; set; }
        public DateTime LastUpdateAt { get; set; }
    }
}