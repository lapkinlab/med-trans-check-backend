using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Models.Drivers.Exceptions;
using Models.Drivers.Repositories;
using Model = Models.Drivers;
using Client = ClientModels.Drivers;
using Converter = ModelConverters.Drivers;

namespace API.Controllers
{
    [Route("api/v1/drivers")]
    public class DriversController : ControllerBase
    {
        private readonly IDriverRepository repository;
        private const string Target = "Driver";

        public DriversController(IDriverRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Creates driver
        /// </summary>
        /// <param name="creationInfo">Driver creation info</param>
        /// <param name="cancellationToken"></param>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateDriverAsync([FromBody] Client.DriverCreationInfo creationInfo,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (creationInfo == null)
            {
                var error = ErrorResponsesService.BodyIsMissing(nameof(creationInfo));
                return BadRequest(error);
            }

            var modelCreationInfo = Converter.DriverCreationInfoConverter.Convert(creationInfo);
            var modelDriver = await repository.CreateAsync(modelCreationInfo, cancellationToken).ConfigureAwait(false);

            var clientDriver = Converter.DriverConverter.Convert(modelDriver);
            return CreatedAtRoute("GetDriverRoute", new {id = clientDriver.Id}, clientDriver);
        }

        /// <summary>
        /// Returns a list of drivers
        /// </summary>
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllDriversAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var modelDriversList = await repository.GetAllAsync(cancellationToken).ConfigureAwait(false);
            var clientDriversList = modelDriversList.Select(Converter.DriverConverter.Convert).ToImmutableList();

            return Ok(clientDriversList);
        }

        /// <summary>
        /// Returns a driver by id
        /// </summary>
        /// <param name="id">driver id</param>
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [Route("{id}", Name = "GetDriverRoute")]
        public async Task<IActionResult> GetDriverAsync([FromRoute] string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(id, out var guid))
            {
                var error = ErrorResponsesService.NotFoundError(Target, $"Driver with id '{id}' not found.");
                return NotFound(error);
            }

            Model.Driver modelDriver;

            try
            {
                modelDriver = await repository.GetAsync(guid, cancellationToken).ConfigureAwait(false);
            }
            catch (DriverNotFoundException ex)
            {
                var error = ErrorResponsesService.NotFoundError(Target, ex.Message);
                return NotFound(error);
            }

            var clientDriver = Converter.DriverConverter.Convert(modelDriver);
            return Ok(clientDriver);
        }

        /// <summary>
        /// Deletes driver
        /// </summary>
        /// <param name="id">driver id</param>
        /// <param name="cancellationToken"></param>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteDriverAsync([FromRoute] string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(id, out var guid))
            {
                var error = ErrorResponsesService.NotFoundError(Target, $"Driver with id '{id}' not found.");
                return NotFound(error);
            }

            try
            {
                await repository.RemoveAsync(guid, cancellationToken).ConfigureAwait(false);
            }
            catch (DriverNotFoundException ex)
            {
                var error = ErrorResponsesService.NotFoundError(Target, ex.Message);
                return NotFound(error);
            }

            return NoContent();
        }
    }
}