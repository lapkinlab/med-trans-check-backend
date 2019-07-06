using System;

namespace Models.WayBills.Exceptions
{
    public class WayBillNotFoundException : Exception
    {
        public WayBillNotFoundException(string guid)
            : base($"WayBill with id \"{guid}\" not found.")
        {
            
        }
    }
}