using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Models.Vehicles.Exceptions;
using Models.Vehicles.Repositories;
using Model = Models.Vehicles;
using Client = ClientModels.Vehicles;
using Converter = ModelConverters.Vehicles;

namespace API.Controllers
{
    [Route("api/v1/vehicles")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleRepository repository;
        private const string Target = "Vehicle";

        public VehiclesController(IVehicleRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Creates vehicle
        /// </summary>
        /// <param name="creationInfo">Vehicle creation info</param>
        /// <param name="cancellationToken"></param>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateVehicleAsync([FromBody] Client.VehicleCreationInfo creationInfo,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (creationInfo == null)
            {
                var error = ErrorResponsesService.BodyIsMissing(nameof(creationInfo));
                return BadRequest(error);
            }

            var modelCreationInfo = Converter.VehicleCreationInfoConverter.Convert(creationInfo);
            var modelVehicle = await repository.CreateAsync(modelCreationInfo, cancellationToken).ConfigureAwait(false);

            var clientVehicle = Converter.VehicleConverter.Convert(modelVehicle);
            return CreatedAtRoute("GetVehicleRoute", new {id = clientVehicle.Id}, clientVehicle);
        }

        /// <summary>
        /// Returns a list of vehicles
        /// </summary>
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllVehiclesAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var modelVehiclesList = await repository.GetAllAsync(cancellationToken).ConfigureAwait(false);
            var clientVehiclesList = modelVehiclesList.Select(Converter.VehicleConverter.Convert).ToImmutableList();

            return Ok(clientVehiclesList);
        }

        /// <summary>
        /// Returns a vehicle by id
        /// </summary>
        /// <param name="id">Vehicle id</param>
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [Route("{id}", Name = "GetVehicleRoute")]
        public async Task<IActionResult> GetVehicleAsync([FromRoute] string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(id, out var guid))
            {
                var error = ErrorResponsesService.NotFoundError(Target, $"Vehicle with id '{id}' not found.");
                return NotFound(error);
            }

            Model.Vehicle modelVehicle;

            try
            {
                modelVehicle = await repository.GetAsync(guid, cancellationToken).ConfigureAwait(false);
            }
            catch (VehicleNotFoundException ex)
            {
                var error = ErrorResponsesService.NotFoundError(Target, ex.Message);
                return NotFound(error);
            }

            var clientVehicle = Converter.VehicleConverter.Convert(modelVehicle);
            return Ok(clientVehicle);
        }

        /// <summary>
        /// Deletes vehicle
        /// </summary>
        /// <param name="id">Vehicle id</param>
        /// <param name="cancellationToken"></param>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteVehicleAsync([FromRoute] string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(id, out var guid))
            {
                var error = ErrorResponsesService.NotFoundError(Target, $"Vehicle with id '{id}' not found.");
                return NotFound(error);
            }

            try
            {
                await repository.RemoveAsync(guid, cancellationToken).ConfigureAwait(false);
            }
            catch (VehicleNotFoundException ex)
            {
                var error = ErrorResponsesService.NotFoundError(Target, ex.Message);
                return NotFound(error);
            }

            return NoContent();
        }
    }
}