using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Models.Vehicles.Repositories
{
    public interface IVehicleRepository
    {
        Task<Vehicle> CreateAsync(VehicleCreationInfo creationInfo, CancellationToken cancellationToken);
        Task<Vehicle> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<IReadOnlyList<Vehicle>> GetAllAsync(CancellationToken cancellationToken);
        Task RemoveAsync(Guid id, CancellationToken cancellationToken);
    }
}