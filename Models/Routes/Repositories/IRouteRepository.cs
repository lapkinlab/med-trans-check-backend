using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Models.Routes.Repositories
{
    public interface IRouteRepository
    {
        Task<Route> CreateAsync(RouteCreationInfo creationInfo, CancellationToken cancellationToken);
        Task<Route> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<IReadOnlyList<Route>> GetAllAsync(CancellationToken cancellationToken);
        Task RemoveAsync(Guid id, CancellationToken cancellationToken);
    }
}