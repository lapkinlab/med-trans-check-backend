using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models.Vehicles.Exceptions;
using MongoDB.Driver;

namespace Models.Vehicles.Repositories
{
    public class MongoVehicleRepository : IVehicleRepository
    {
        private readonly IMongoCollection<Vehicle> vehicles;

        public MongoVehicleRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("MedTransCheckDb");
            vehicles = database.GetCollection<Vehicle>("Vehicles");
        }
        
        public Task<Vehicle> CreateAsync(VehicleCreationInfo creationInfo, CancellationToken cancellationToken)
        {
            if (creationInfo == null)
            {
                throw new ArgumentNullException(nameof(creationInfo));
            }

            var guid = Guid.NewGuid();
            var vehicle = new Vehicle
            {
                Id = guid,
                Mark = creationInfo.Mark,
                GovNumber = creationInfo.GovNumber
            };

            vehicles.InsertOne(vehicle, cancellationToken: cancellationToken);
            return Task.FromResult(vehicle);
        }

        public Task<Vehicle> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var vehicle = vehicles.Find(item => item.Id == id).FirstOrDefault();
            
            if (vehicle == null)
            {
                throw new VehicleNotFoundException(id.ToString());
            }

            return Task.FromResult(vehicle);
        }

        public Task<IReadOnlyList<Vehicle>> GetAllAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var allVehicles = vehicles.Find(item => true).ToList();
            return Task.FromResult<IReadOnlyList<Vehicle>>(allVehicles);
        }

        public Task RemoveAsync(Guid id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var deleteResult = vehicles.DeleteOne(type => type.Id == id);

            if (deleteResult.DeletedCount == 0)
            {
                throw new VehicleNotFoundException(id.ToString());
            }

            return Task.CompletedTask;
        }
    }
}