using System;
using System.Linq;
using Model = Models.MechanicNotes;
using Client = ClientModels.MechanicNotes;

namespace ModelConverters.MechanicNotes
{
    public static class MechanicNoteConverter
    {
        public static Client.MechanicNote Convert(Model.MechanicNote modelMechanicNote)
        {
            if (modelMechanicNote == null)
            {
                throw new ArgumentNullException(nameof(modelMechanicNote));
            }

            var clientMechanicNote = new Client.MechanicNote
            {
                Id = modelMechanicNote.Id.ToString(),
                Params = modelMechanicNote.Params.ToArray(),
                MechanicName = modelMechanicNote.MechanicName,
                Permission = modelMechanicNote.Permission.ToString(),
                LastUpdateAt = modelMechanicNote.LastUpdateAt 
            };
            
            return clientMechanicNote;
        }
    }
}