using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models.MechanicNotes;
using Models.MedicNotes;

namespace Models.WayBills.Repositories
{
    public interface IWayBillRepository
    {
        Task<WayBill> CreateAsync(WayBillCreationInfo creationInfo, Guid mechanicNoteId, 
            IEnumerable<Guid> medicNotesIds, CancellationToken cancellationToken);
        Task<WayBill> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<IReadOnlyList<WayBill>> SearchAsync(WayBillSearchInfo searchInfo, CancellationToken cancellationToken);
        Task<WayBill> PatchAsync(WayBillPatchInfo patchInfo, CancellationToken cancellationToken);
        Task RemoveAsync(Guid id, CancellationToken cancellationToken);
    }
}