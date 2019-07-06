using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using ModelConverters.WayBills;
using Models.Checkpoints.Repositories;
using Models.Drivers.Repositories;
using Models.MechanicNotes.Repositories;
using Models.MedicNotes.Repositories;
using Models.Routes.Repositories;
using Models.Vehicles.Repositories;
using Models.WayBills.Exceptions;
using Models.WayBills.Repositories;
using Client = ClientModels.WayBills;
using Model = Models.WayBills;
using Converters = ModelConverters.WayBills;

namespace API.Controllers
{
    [Route("api/v1/waybills")]
    public sealed class WayBillController : Controller
    {
        private readonly IWayBillRepository wayBillRepository;
        private readonly IDriverRepository driverRepository;
        private readonly IVehicleRepository vehicleRepository;
        private readonly IRouteRepository routeRepository;
        private readonly ICheckpointRepository checkpointRepository;
        private readonly IMedicNoteRepository medicNoteRepository;
        private readonly IMechanicNoteRepository mechanicNoteRepository;
        private const string Target = "WayBill";

        public WayBillController(IWayBillRepository wayBillRepository, IDriverRepository driverRepository, 
            IVehicleRepository vehicleRepository, IRouteRepository routeRepository, ICheckpointRepository checkpointRepository, 
            IMedicNoteRepository medicNoteRepository, IMechanicNoteRepository mechanicNoteRepository)
        {
            this.wayBillRepository = wayBillRepository ?? throw new ArgumentNullException(nameof(wayBillRepository));
            this.driverRepository = driverRepository ?? throw new ArgumentNullException(nameof(driverRepository));
            this.vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
            this.routeRepository = routeRepository ?? throw new ArgumentNullException(nameof(routeRepository));
            this.checkpointRepository = checkpointRepository ?? throw new ArgumentNullException(nameof(checkpointRepository));
            this.medicNoteRepository = medicNoteRepository ?? throw new ArgumentNullException(nameof(medicNoteRepository));
            this.mechanicNoteRepository = mechanicNoteRepository ?? throw new ArgumentNullException(nameof(mechanicNoteRepository));
        }
        
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateWayBillAsync([FromBody]Client.WayBillCreationInfo creationInfo,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (creationInfo == null)
            {
                var error = ErrorResponsesService.BodyIsMissing(nameof(creationInfo));
                return BadRequest(error);
            }

            Model.WayBillCreationInfo modelCreationInfo;

            try
            {
                modelCreationInfo = WayBillCreationInfoConverter.Convert(creationInfo);
            }
            catch (InvalidDataException ex)
            {
                var error = ErrorResponsesService.InvalidBodyData(nameof(creationInfo), ex.Message);
                return BadRequest(error);
            }

            if (!Guid.TryParse(creationInfo.RouteId, out var routeGuid))
            {
                var error = ErrorResponsesService.NotFoundError("Route", $"Route with id '{creationInfo.RouteId}' not found.");
                return BadRequest(error);
            }
            
            Model.WayBill modelWayBill;
            var route = await routeRepository.GetAsync(routeGuid, cancellationToken).ConfigureAwait(false);
            var mechanicNote = await mechanicNoteRepository.CreateAsync(cancellationToken).ConfigureAwait(false);
            var medicNotesCount = route.Checkpoints.Count + 1;
            var medicNotes = new Models.MedicNotes.MedicNote[medicNotesCount];

            for (var i = 0; i < medicNotesCount; i++)
            {
                medicNotes[i] = await medicNoteRepository.CreateAsync(cancellationToken).ConfigureAwait(false);
            }
            
            try
            {
                modelWayBill = await wayBillRepository.CreateAsync(modelCreationInfo, mechanicNote.Id, 
                        medicNotes.Select(item => item.Id), cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (WayBillDuplicationException ex)
            {
                var error = ErrorResponsesService.DuplicationError(Target, ex.Message);
                return BadRequest(error);
            }

            if (!Guid.TryParse(creationInfo.DriverId, out var driverGuid))
            {
                var error = ErrorResponsesService.NotFoundError("Driver", $"Driver with id '{creationInfo.RouteId}' not found.");
                return BadRequest(error);
            }
            
            var driver = await driverRepository.GetAsync(driverGuid, cancellationToken).ConfigureAwait(false);
            var clientDriver = ModelConverters.Drivers.DriverConverter.Convert(driver);
            
            if (!Guid.TryParse(creationInfo.VehicleId, out var vehicleGuid))
            {
                var error = ErrorResponsesService.NotFoundError("Vehicle", $"Vehicle with id '{creationInfo.RouteId}' not found.");
                return BadRequest(error);
            }
            
            var vehicle = await vehicleRepository.GetAsync(vehicleGuid, cancellationToken).ConfigureAwait(false);
            var clientVehicle = ModelConverters.Vehicles.VehicleConverter.Convert(vehicle);

            var checkpoints = await checkpointRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);
            var clientRoute = ModelConverters.Routes.RouteConverter.Convert(route, checkpoints);

            var clientMechanicNote = ModelConverters.MechanicNotes.MechanicNoteConverter.Convert(mechanicNote);
            var clientMedicNotes = medicNotes.Select(ModelConverters.MedicNotes.MedicNoteConverter.Convert);

            var clientWayBill =
                WayBillConverter.Convert(modelWayBill, clientDriver, clientVehicle, clientRoute, clientMechanicNote, clientMedicNotes);
            return CreatedAtRoute("GetWayBillRoute", new {id = clientWayBill.Id}, clientWayBill);
        }
        
        [HttpGet]
        [Route("{id}", Name = "GetWayBillRoute")]
        public async Task<IActionResult> GetWayBillAsync([FromRoute] string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(id, out var guid))
            {
                var error = ErrorResponsesService.NotFoundError(Target, $"WayBill with id '{id}' not found.");
                return NotFound(error);
            }

            Model.WayBill modelWayBill;

            try
            {
                modelWayBill = await wayBillRepository.GetAsync(guid, cancellationToken).ConfigureAwait(false);
            }
            catch (WayBillNotFoundException)
            {
                var error = ErrorResponsesService.NotFoundError(Target, $"WayBill with id '{id}' not found.");
                return NotFound(error);
            }
            
            var driver = await driverRepository.GetAsync(modelWayBill.Driver, cancellationToken).ConfigureAwait(false);
            var clientDriver = ModelConverters.Drivers.DriverConverter.Convert(driver);
            var vehicle = await vehicleRepository.GetAsync(modelWayBill.Vehicle, cancellationToken).ConfigureAwait(false);
            var clientVehicle = ModelConverters.Vehicles.VehicleConverter.Convert(vehicle);
            var route = await routeRepository.GetAsync(modelWayBill.Route, cancellationToken).ConfigureAwait(false);
            var checkpoints = await checkpointRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);
            var clientRoute = ModelConverters.Routes.RouteConverter.Convert(route, checkpoints);
            var mechanicNote = await mechanicNoteRepository.GetAsync(modelWayBill.MechanicNote, cancellationToken)
                .ConfigureAwait(false);
            var medicNotesCount = route.Checkpoints.Count + 1;
            var medicNotes = new Models.MedicNotes.MedicNote[medicNotesCount];

            for (var i = 0; i < medicNotesCount; i++)
            {
                medicNotes[i] = await medicNoteRepository.CreateAsync(cancellationToken).ConfigureAwait(false);
            }
            
            var clientMechanicNote = ModelConverters.MechanicNotes.MechanicNoteConverter.Convert(mechanicNote);
            var clientMedicNotes = medicNotes.Select(ModelConverters.MedicNotes.MedicNoteConverter.Convert);

            var clientWayBill =
                WayBillConverter.Convert(modelWayBill, clientDriver, clientVehicle, clientRoute, clientMechanicNote, clientMedicNotes);

            return Ok(clientWayBill);
        }
        
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> SearchPlacesAsync([FromQuery]Client.WayBillSearchInfo searchInfo,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var modelSearchInfo = WayBillSearchInfoConverter.Convert(searchInfo ?? new Client.WayBillSearchInfo());
            var modelWayBillList = await wayBillRepository.SearchAsync(modelSearchInfo, cancellationToken).ConfigureAwait(false);
            var clientWayBillList = modelWayBillList
                .Select(async item =>
                {
                    var driver = await driverRepository.GetAsync(item.Driver, cancellationToken).ConfigureAwait(false);
                    var clientDriver = ModelConverters.Drivers.DriverConverter.Convert(driver);
                    var vehicle = await vehicleRepository.GetAsync(item.Vehicle, cancellationToken).ConfigureAwait(false);
                    var clientVehicle = ModelConverters.Vehicles.VehicleConverter.Convert(vehicle);
                    var route = await routeRepository.GetAsync(item.Route, cancellationToken).ConfigureAwait(false);
                    var checkpoints = await checkpointRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);
                    var clientRoute = ModelConverters.Routes.RouteConverter.Convert(route, checkpoints);
                    var mechanicNote = await mechanicNoteRepository.GetAsync(item.MechanicNote, cancellationToken)
                        .ConfigureAwait(false);
                    var medicNotesCount = route.Checkpoints.Count + 1;
                    var medicNotes = new Models.MedicNotes.MedicNote[medicNotesCount];

                    for (var i = 0; i < medicNotesCount; i++)
                    {
                        medicNotes[i] = await medicNoteRepository.CreateAsync(cancellationToken).ConfigureAwait(false);
                    }
            
                    var clientMechanicNote = ModelConverters.MechanicNotes.MechanicNoteConverter.Convert(mechanicNote);
                    var clientMedicNotes = medicNotes.Select(ModelConverters.MedicNotes.MedicNoteConverter.Convert);

                    return WayBillConverter.Convert(item, clientDriver, clientVehicle, clientRoute, clientMechanicNote,
                        clientMedicNotes);
                })
                .ToImmutableList();

            return Ok(clientWayBillList);
        }
        
        public async Task<IActionResult> DeleteWayBillAsync([FromRoute]string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(id, out var guid))
            {
                var error = ErrorResponsesService.NotFoundError(Target, $"WayBill with id '{id}' not found.");
                return NotFound(error);
            }
            
            try
            {
                await wayBillRepository.RemoveAsync(guid, cancellationToken).ConfigureAwait(false);
            }
            catch (WayBillNotFoundException ex)
            {
                var error = ErrorResponsesService.NotFoundError(Target, ex.Message);
                return NotFound(error);
            }

            return NoContent();
        }
    }
}