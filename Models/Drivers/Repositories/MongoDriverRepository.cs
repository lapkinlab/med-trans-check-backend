using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models.Drivers.Exceptions;
using MongoDB.Driver;

namespace Models.Drivers.Repositories
{
    public class MongoDriverRepository : IDriverRepository
    {
        private readonly IMongoCollection<Driver> drivers;

        public MongoDriverRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("MedTransCheckDb");
            drivers = database.GetCollection<Driver>("Drivers");
        }
        
        public Task<Driver> CreateAsync(DriverCreationInfo creationInfo, CancellationToken cancellationToken)
        {
            if (creationInfo == null)
            {
                throw new ArgumentNullException(nameof(creationInfo));
            }

            var guid = Guid.NewGuid();
            var driver = new Driver
            {
                Id = guid,
                Mark = creationInfo.Mark,
                GovNumber = creationInfo.GovNumber
            };

            drivers.InsertOne(driver, cancellationToken: cancellationToken);
            return Task.FromResult(driver);
        }

        public Task<Driver> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var driver = drivers.Find(item => item.Id == id).FirstOrDefault();
            
            if (driver == null)
            {
                throw new DriverNotFoundException(id.ToString());
            }

            return Task.FromResult(driver);
        }

        public Task<IReadOnlyList<Driver>> GetAllAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var allDrivers = drivers.Find(item => true).ToList();
            return Task.FromResult<IReadOnlyList<Driver>>(allDrivers);
        }

        public Task RemoveAsync(Guid id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var deleteResult = drivers.DeleteOne(type => type.Id == id);

            if (deleteResult.DeletedCount == 0)
            {
                throw new DriverNotFoundException(id.ToString());
            }

            return Task.CompletedTask;
        }
    }
}