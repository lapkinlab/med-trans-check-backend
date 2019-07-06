using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Models.MechanicNotes;
using Models.MedicNotes;
using Models.WayBills.Exceptions;
using MongoDB.Driver;

namespace Models.WayBills.Repositories
{
    public class MongoWayBillRepository : IWayBillRepository
    {
        private readonly IMongoCollection<WayBill> wayBills;

        public MongoWayBillRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("MedTransCheckDb");
            wayBills = database.GetCollection<WayBill>("WayBills");
        }
        
        public Task<WayBill> CreateAsync(WayBillCreationInfo creationInfo, Guid mechanicNoteId, 
            IEnumerable<Guid> medicNotesIds, CancellationToken cancellationToken)
        {
            if (creationInfo == null)
            {
                throw new ArgumentException(nameof(creationInfo));
            }

            if (medicNotesIds == null)
            {
                throw new ArgumentException(nameof(medicNotesIds));
            }

            cancellationToken.ThrowIfCancellationRequested();
            
            var wayBillWithSameSerialNumber = wayBills
                .Find(item => item.Serial == creationInfo.Serial && item.Number == creationInfo.Number)
                .FirstOrDefault();

            if (wayBillWithSameSerialNumber != null)
            {
                throw new WayBillDuplicationException(wayBillWithSameSerialNumber.Serial, wayBillWithSameSerialNumber.Number);
            }
            
            var now = DateTime.Now;
            var id = Guid.NewGuid();
            var wayBill = new WayBill
            {
                Id = id,
                Serial = creationInfo.Serial,
                Number = creationInfo.Number,
                Driver = creationInfo.DriverId,
                Vehicle = creationInfo.VehicleId,
                Route = creationInfo.RouteId,
                MechanicNote = mechanicNoteId,
                MedicNotes = medicNotesIds.ToArray(),
                CreatedAt = now,
                LastUpdateAt = now
            };
            
            wayBills.InsertOneAsync(wayBill, cancellationToken: cancellationToken);
            return Task.FromResult(wayBill);
        }

        public Task<WayBill> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var wayBill = wayBills.Find(item => item.Id == id).FirstOrDefault();
            
            if (wayBill == null)
            {
                throw new WayBillNotFoundException(id.ToString());
            }

            return Task.FromResult(wayBill);
        }

        public Task<IReadOnlyList<WayBill>> SearchAsync(WayBillSearchInfo searchInfo, CancellationToken cancellationToken)
        {
            if (searchInfo == null)
            {
                throw new ArgumentNullException(nameof(searchInfo));
            }
            
            cancellationToken.ThrowIfCancellationRequested();
            
            var search = wayBills.Find(item => true).ToEnumerable();

            if (searchInfo.Offset != null)
            {
                search = search.Skip(searchInfo.Offset.Value);
            }

            if (searchInfo.Limit != null)
            {
                search = search.Take(searchInfo.Limit.Value);
            }

            var result = search.ToList();
            return Task.FromResult<IReadOnlyList<WayBill>>(result);
        }

        public Task<WayBill> PatchAsync(WayBillPatchInfo patchInfo, CancellationToken cancellationToken)
        {
            if (patchInfo == null)
            {
                throw new ArgumentNullException(nameof(patchInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();
            var wayBill = wayBills.Find(item => item.Id == patchInfo.Id).FirstOrDefault();

            if (wayBill == null)
            {
                throw new WayBillNotFoundException(patchInfo.Id.ToString());
            }

            var updated = false;

            if (patchInfo.Serial != null)
            {
                wayBill.Serial = patchInfo.Serial;
                updated = true;
            }
            
            if (patchInfo.Number != null)
            {
                wayBill.Number = patchInfo.Number;
                updated = true;
            }
            
            if (patchInfo.DriverId != null)
            {
                wayBill.Driver = patchInfo.DriverId.Value;
                updated = true;
            }
            
            if (patchInfo.VehicleId != null)
            {
                wayBill.Vehicle = patchInfo.VehicleId.Value;
                updated = true;
            }
            
            if (patchInfo.RouteId != null)
            {
                wayBill.Route = patchInfo.RouteId.Value;
                updated = true;
            }
            
            if (updated)
            {
                var now = DateTime.UtcNow;
                wayBill.LastUpdateAt = now;
                wayBills.ReplaceOne(item => item.Id == patchInfo.Id, wayBill);
            }

            return Task.FromResult(wayBill);
        }

        public Task RemoveAsync(Guid id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            cancellationToken.ThrowIfCancellationRequested();
            var deleteResult = wayBills.DeleteOne(type => type.Id == id);

            if (deleteResult.DeletedCount == 0)
            {
                throw new WayBillNotFoundException(id.ToString());
            }

            return Task.CompletedTask;
        }
    }
}