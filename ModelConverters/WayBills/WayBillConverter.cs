using System;
using System.Collections.Generic;
using System.Linq;
using ClientModels.MedicNotes;
using Model = Models.WayBills;
using Client = ClientModels.WayBills;

namespace ModelConverters.WayBills
{
    public static class WayBillConverter
    {
        public static Client.WayBill Convert(Model.WayBill modelWayBill, ClientModels.Drivers.Driver driver, 
            ClientModels.Vehicles.Vehicle vehicle, ClientModels.Routes.Route route,
            ClientModels.MechanicNotes.MechanicNote mechanicNote, IEnumerable<MedicNote> medicNotes)
        {
            if (modelWayBill == null)
            {
                throw new ArgumentNullException(nameof(modelWayBill));
            }

            var clientWayBill = new Client.WayBill
            {
                Id = modelWayBill.Id.ToString(),
                Serial = modelWayBill.Serial,
                Number = modelWayBill.Number,
                Driver = driver,
                Vehicle = vehicle,
                Route = route,
                MechanicNote = mechanicNote,
                MedicNotes = medicNotes.ToArray(),
                CreatedAt = modelWayBill.CreatedAt,
                LastUpdateAt = modelWayBill.LastUpdateAt
            };
            
            return clientWayBill;
        }
    }
}