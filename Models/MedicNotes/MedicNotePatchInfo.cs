using System;

namespace Models.MedicNotes
{
    public class MedicNotePatchInfo
    {
        public Guid Id { get; set; }
        public string Complaints { get; set; }
        public string Temperature { get; set; }
        public string Pressure { get; set; }
        public string Pulse { get; set; }
        public bool? AlcoholIntoxication { get; set; }
        public bool? DrugIntoxication { get; set; }
        public string MedicName { get; set; }
        public Permission? Permission { get; set; }
        
        public MedicNotePatchInfo(Guid id, string complaints = null, string temperature = null, string pressure = null,
            string pulse = null, bool? alcoholIntoxication = null, bool? drugIntoxication = null, string medicName = null, 
            Permission? permission = null)
        {
            Id = id;
            Complaints = complaints;
            Temperature = temperature;
            Pressure = pressure;
            Pulse = pulse;
            AlcoholIntoxication = alcoholIntoxication;
            DrugIntoxication = drugIntoxication;
            MedicName = medicName;
            Permission = permission;
        }
    }
}