using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Models.MechanicNotes.Repositories
{
    public interface IMechanicNoteRepository
    {
        Task<MechanicNote> CreateAsync(CancellationToken cancellationToken);
        Task<MechanicNote> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<IReadOnlyList<MechanicNote>> GetAllAsync(CancellationToken cancellationToken);
        Task<MechanicNote> PatchAsync(MechanicNotePatchInfo patchInfo, CancellationToken cancellationToken);
        Task RemoveAsync(Guid id, CancellationToken cancellationToken);
    }
}