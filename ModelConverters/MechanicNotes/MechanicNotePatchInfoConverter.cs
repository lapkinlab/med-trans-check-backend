using System;
using Models;
using Model = Models.MechanicNotes;
using Client = ClientModels.MechanicNotes;

namespace ModelConverters.MechanicNotes
{
    public static class MechanicNotePatchInfoConverter
    {
        public static Model.MechanicNotePatchInfo Convert(Guid id, string mechanicName, Client.MechanicNotePatchInfo clientPatchInfo)
        {
            if (clientPatchInfo == null)
            {
                throw new ArgumentNullException(nameof(clientPatchInfo));
            }

            if (clientPatchInfo.Params != null && clientPatchInfo.Params.Length != Model.MechanicNote.ParamsCount)
            {
                throw new ArgumentException(
                    $"{nameof(clientPatchInfo.Params.Length)} must be equals {Model.MechanicNote.ParamsCount}.");
            }

            Permission? permission = null;

            if (Enum.TryParse(clientPatchInfo.Permission, out Permission perm))
            {
                permission = perm;
            }
            
            var modelPatchInfo = new Model.MechanicNotePatchInfo(id, clientPatchInfo.Params, mechanicName, permission);
            return modelPatchInfo;
        }
    }
}