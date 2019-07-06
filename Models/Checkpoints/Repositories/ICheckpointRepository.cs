using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Models.Checkpoints.Repositories
{
    public interface ICheckpointRepository
    {
        Task<Checkpoint> CreateAsync(CheckpointCreationInfo creationInfo, CancellationToken cancellationToken);
        Task<Checkpoint> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<IReadOnlyList<Checkpoint>> GetAllAsync(CancellationToken cancellationToken);
        Task RemoveAsync(Guid id, CancellationToken cancellationToken);
    }
}