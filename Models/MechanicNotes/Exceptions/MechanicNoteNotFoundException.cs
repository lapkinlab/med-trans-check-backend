using System;

namespace Models.MechanicNotes.Exceptions
{
    public class MechanicNoteNotFoundException : Exception
    {
        public MechanicNoteNotFoundException(string mechanicNoteId)
            : base($"MechanicNote \"{mechanicNoteId}\" not found.")
        {
        
        }
    }
}