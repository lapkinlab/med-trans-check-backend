using System;

namespace Models.WayBills.Exceptions
{
    public class WayBillDuplicationException : Exception
    {
        public WayBillDuplicationException(string serial, string number)
            : base($"Way bill with serial \"{serial}\" and number \"{number}\" already exists;")
        {
            
        }
    }
}