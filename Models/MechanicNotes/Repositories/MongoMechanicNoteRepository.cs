using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models.MechanicNotes.Exceptions;
using MongoDB.Driver;

namespace Models.MechanicNotes.Repositories
{
    public class MongoMechanicNoteRepository : IMechanicNoteRepository
    {
        private readonly IMongoCollection<MechanicNote> mechanicNotes;

        public MongoMechanicNoteRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("MedTransCheckDb");
            mechanicNotes = database.GetCollection<MechanicNote>("MechanicNotes");
        }
        
        public Task<MechanicNote> CreateAsync(CancellationToken cancellationToken)
        {
            var guid = Guid.NewGuid();
            var now = DateTime.UtcNow;
            
            var mechanicNote = new MechanicNote
            {
                Id = guid,
                Params = new bool[MechanicNote.ParamsCount],
                MechanicName = string.Empty,
                Permission = Permission.NotVisited,
                LastUpdateAt = now 
            };

            mechanicNotes.InsertOne(mechanicNote, cancellationToken: cancellationToken);
            return Task.FromResult(mechanicNote);
        }

        public Task<MechanicNote> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var mechanicNote = mechanicNotes.Find(item => item.Id == id).FirstOrDefault();
            
            if (mechanicNote == null)
            {
                throw new MechanicNoteNotFoundException(id.ToString());
            }

            return Task.FromResult(mechanicNote);
        }

        public Task<IReadOnlyList<MechanicNote>> GetAllAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var allMechanicNotesList = mechanicNotes.Find(item => true).ToList();
            return Task.FromResult<IReadOnlyList<MechanicNote>>(allMechanicNotesList);
        }

        public Task<MechanicNote> PatchAsync(MechanicNotePatchInfo patchInfo, CancellationToken cancellationToken)
        {
            if (patchInfo == null)
            {
                throw new ArgumentNullException(nameof(patchInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();
            var mechanicNote = mechanicNotes.Find(item => item.Id == patchInfo.Id).FirstOrDefault();

            if (mechanicNote == null)
            {
                throw new MechanicNoteNotFoundException(patchInfo.Id.ToString());
            }

            var updated = false;

            if (patchInfo.Params != null)
            {
                mechanicNote.Params = patchInfo.Params;
                updated = true;
            }
            
            if (patchInfo.MechanicName != null)
            {
                mechanicNote.MechanicName = patchInfo.MechanicName;
                updated = true;
            }
            
            if (patchInfo.Permission != null)
            {
                mechanicNote.Permission = patchInfo.Permission.Value;
                updated = true;
            }
            
            if (updated)
            {
                mechanicNotes.ReplaceOne(item => item.Id == patchInfo.Id, mechanicNote);
            }

            return Task.FromResult(mechanicNote);
        }

        public Task RemoveAsync(Guid id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var deleteResult = mechanicNotes.DeleteOne(type => type.Id == id);

            if (deleteResult.DeletedCount == 0)
            {
                throw new MechanicNoteNotFoundException(id.ToString());
            }

            return Task.CompletedTask;
        }
    }
}