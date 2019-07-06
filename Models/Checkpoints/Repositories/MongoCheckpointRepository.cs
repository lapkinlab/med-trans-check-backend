using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models.Checkpoints.Exceptions;
using MongoDB.Driver;

namespace Models.Checkpoints.Repositories
{
    public class MongoCheckpointRepository : ICheckpointRepository
    {
        private readonly IMongoCollection<Checkpoint> chekpoints;

        public MongoCheckpointRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("MedTransCheckDb");
            chekpoints = database.GetCollection<Checkpoint>("Checkpoints");
        }
        
        public Task<Checkpoint> CreateAsync(CheckpointCreationInfo creationInfo, CancellationToken cancellationToken)
        {
            if (creationInfo == null)
            {
                throw new ArgumentNullException(nameof(creationInfo));
            }

            var guid = Guid.NewGuid();
            var checkpoint = new Checkpoint
            {
                Id = guid,
                Name = creationInfo.Name
            };

            chekpoints.InsertOne(checkpoint, cancellationToken: cancellationToken);
            return Task.FromResult(checkpoint);
        }

        public Task<Checkpoint> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var checkpoint = chekpoints.Find(item => item.Id == id).FirstOrDefault();
            
            if (checkpoint == null)
            {
                throw new CheckpointNotFoundException(id.ToString());
            }

            return Task.FromResult(checkpoint);
        }

        public Task<IReadOnlyList<Checkpoint>> GetAllAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var allCheckpointsList = chekpoints.Find(item => true).ToList();
            return Task.FromResult<IReadOnlyList<Checkpoint>>(allCheckpointsList);
        }

        public Task RemoveAsync(Guid id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var deleteResult = chekpoints.DeleteOne(type => type.Id == id);

            if (deleteResult.DeletedCount == 0)
            {
                throw new CheckpointNotFoundException(id.ToString());
            }

            return Task.CompletedTask;
        }
    }
}