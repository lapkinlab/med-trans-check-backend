using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models.MedicNotes.Exceptions;
using MongoDB.Driver;

namespace Models.MedicNotes.Repositories
{
    public class MongoMedicNoteRepository : IMedicNoteRepository
    {
        private readonly IMongoCollection<MedicNote> medicNotes;

        public MongoMedicNoteRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("MedTransCheckDb");
            medicNotes = database.GetCollection<MedicNote>("MedicNotes");
        }
        
        public Task<MedicNote> CreateAsync(CancellationToken cancellationToken)
        {
            var guid = Guid.NewGuid();
            var now = DateTime.UtcNow;
            
            var medicNote = new MedicNote
            {
                Id = guid,
                Complaints = string.Empty,
                Temperature = string.Empty,
                Pressure = string.Empty,
                Pulse = string.Empty,
                AlcoholIntoxication = false,
                DrugIntoxication = false,
                MedicName = string.Empty,
                Permission = Permission.NotVisited,
                LastUpdateAt = now 
            };

            medicNotes.InsertOne(medicNote, cancellationToken: cancellationToken);
            return Task.FromResult(medicNote);
        }

        public Task<MedicNote> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var medicNote = medicNotes.Find(item => item.Id == id).FirstOrDefault();
            
            if (medicNote == null)
            {
                throw new MedicNoteNotFoundException(id.ToString());
            }

            return Task.FromResult(medicNote);
        }

        public Task<IReadOnlyList<MedicNote>> GetAllAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var allMedicNotesList = medicNotes.Find(item => true).ToList();
            return Task.FromResult<IReadOnlyList<MedicNote>>(allMedicNotesList);
        }

        public Task<MedicNote> PatchAsync(MedicNotePatchInfo patchInfo, CancellationToken cancellationToken)
        {
            if (patchInfo == null)
            {
                throw new ArgumentNullException(nameof(patchInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();
            var medicNote = medicNotes.Find(item => item.Id == patchInfo.Id).FirstOrDefault();

            if (medicNote == null)
            {
                throw new MedicNoteNotFoundException(patchInfo.Id.ToString());
            }

            var updated = false;

            if (patchInfo.Complaints != null)
            {
                medicNote.Complaints = patchInfo.Complaints;
                updated = true;
            }
            
            if (patchInfo.Temperature != null)
            {
                medicNote.Temperature = patchInfo.Temperature;
                updated = true;
            }
            
            if (patchInfo.Pressure != null)
            {
                medicNote.Pressure = patchInfo.Pressure;
                updated = true;
            }
            
            if (patchInfo.Pulse != null)
            {
                medicNote.Pulse = patchInfo.Pulse;
                updated = true;
            }
            
            if (patchInfo.AlcoholIntoxication != null)
            {
                medicNote.AlcoholIntoxication = patchInfo.AlcoholIntoxication.Value;
                updated = true;
            }
            
            if (patchInfo.DrugIntoxication != null)
            {
                medicNote.DrugIntoxication = patchInfo.DrugIntoxication.Value;
                updated = true;
            }
            
            if (patchInfo.MedicName != null)
            {
                medicNote.MedicName = patchInfo.MedicName;
                updated = true;
            }
            
            if (patchInfo.Permission != null)
            {
                medicNote.Permission = patchInfo.Permission.Value;
                updated = true;
            }
            
            if (updated)
            {
                medicNotes.ReplaceOne(item => item.Id == patchInfo.Id, medicNote);
            }

            return Task.FromResult(medicNote);
        }

        public Task RemoveAsync(Guid id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var deleteResult = medicNotes.DeleteOne(type => type.Id == id);

            if (deleteResult.DeletedCount == 0)
            {
                throw new MedicNoteNotFoundException(id.ToString());
            }

            return Task.CompletedTask;
        }
    }
}