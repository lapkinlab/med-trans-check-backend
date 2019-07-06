using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models.Routes.Exceptions;
using MongoDB.Driver;

namespace Models.Routes.Repositories
{
    public class MongoRouteRepository : IRouteRepository
    {
        private readonly IMongoCollection<Route> routes;

        public MongoRouteRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("MedTransCheckDb");
            routes = database.GetCollection<Route>("Routes");
        }
        
        public Task<Route> CreateAsync(RouteCreationInfo creationInfo, CancellationToken cancellationToken)
        {
            if (creationInfo == null)
            {
                throw new ArgumentNullException(nameof(creationInfo));
            }

            var guid = Guid.NewGuid();
            var route = new Route
            {
                Id = guid,
                FromPlace = creationInfo.FromPlace,
                ToPlace = creationInfo.ToPlace,
                Checkpoints = creationInfo.Checkpoints
            };

            routes.InsertOne(route, cancellationToken: cancellationToken);
            return Task.FromResult(route);
        }

        public Task<Route> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var route = routes.Find(item => item.Id == id).FirstOrDefault();
            
            if (route == null)
            {
                throw new RouteNotFoundException(id.ToString());
            }

            return Task.FromResult(route);
        }

        public Task<IReadOnlyList<Route>> GetAllAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var allRoutesList = routes.Find(item => true).ToList();
            return Task.FromResult<IReadOnlyList<Route>>(allRoutesList);
        }

        public Task RemoveAsync(Guid id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var deleteResult = routes.DeleteOne(type => type.Id == id);

            if (deleteResult.DeletedCount == 0)
            {
                throw new RouteNotFoundException(id.ToString());
            }

            return Task.CompletedTask;
        }
    }
}