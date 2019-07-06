using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace Models.Drivers.Repositories
{
    public interface IDriverRepository
    {
        Task<Driver> CreateAsync(DriverCreationInfo creationInfo, CancellationToken cancellationToken);
        Task<Driver> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<IReadOnlyList<Driver>> GetAllAsync(CancellationToken cancellationToken);
        Task RemoveAsync(Guid id, CancellationToken cancellationToken);
    }
}