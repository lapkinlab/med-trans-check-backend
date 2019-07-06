using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Models.MedicNotes.Repositories
{
    public interface IMedicNoteRepository
    {
        Task<MedicNote> CreateAsync(CancellationToken cancellationToken);
        Task<MedicNote> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<IReadOnlyList<MedicNote>> GetAllAsync(CancellationToken cancellationToken);
        Task<MedicNote> PatchAsync(MedicNotePatchInfo patchInfo, CancellationToken cancellationToken);
        Task RemoveAsync(Guid id, CancellationToken cancellationToken);
    }
}