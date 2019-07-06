using System;
using Model = Models.MedicNotes;
using Client = ClientModels.MedicNotes;

namespace ModelConverters.MedicNotes
{
    public static class MedicNoteConverter
    {
        public static Client.MedicNote Convert(Model.MedicNote modelMedicNote)
        {
            if (modelMedicNote == null)
            {
                throw new ArgumentNullException(nameof(modelMedicNote));
            }

            var clientMedicNote = new Client.MedicNote
            {
                Id = modelMedicNote.Id.ToString(),
                Complaints = modelMedicNote.Complaints,
                Temperature = modelMedicNote.Temperature,
                Pressure = modelMedicNote.Pressure,
                Pulse = modelMedicNote.Pulse,
                AlcoholIntoxication = modelMedicNote.AlcoholIntoxication,
                DrugIntoxication = modelMedicNote.DrugIntoxication,
                MedicName = modelMedicNote.MedicName,
                Permission = modelMedicNote.Permission.ToString(),
                LastUpdateAt = modelMedicNote.LastUpdateAt 
            };
            
            return clientMedicNote;
        }
    }
}