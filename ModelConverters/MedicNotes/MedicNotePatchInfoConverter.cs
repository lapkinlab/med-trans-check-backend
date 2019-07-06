using System;
using Models;
using Model = Models.MedicNotes;
using Client = ClientModels.MedicNotes;

namespace ModelConverters.MedicNotes
{
    public static class MedicNotePatchInfoConverter
    {
        public static Model.MedicNotePatchInfo Convert(Guid id, string medicName, Client.MedicNotePatchInfo clientPatchInfo)
        {
            if (clientPatchInfo == null)
            {
                throw new ArgumentNullException(nameof(clientPatchInfo));
            }

            Permission? permission = null;

            if (Enum.TryParse(clientPatchInfo.Permission, out Permission perm))
            {
                permission = perm;
            }
            
            var modelPatchInfo = new Model.MedicNotePatchInfo(id, clientPatchInfo.Complaints, clientPatchInfo.Temperature,
                clientPatchInfo.Pressure, clientPatchInfo.Pulse, clientPatchInfo.AlcoholIntoxication, clientPatchInfo.DrugIntoxication,
                medicName, permission);
            return modelPatchInfo;
        }
    }
}