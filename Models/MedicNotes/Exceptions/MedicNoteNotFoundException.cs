using System;

namespace Models.MedicNotes.Exceptions
{
    public class MedicNoteNotFoundException : Exception
    {
        public MedicNoteNotFoundException(string medicNoteId)
            : base($"MedicNote \"{medicNoteId}\" not found.")
        {
        
        }
    }
}