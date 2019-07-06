using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Models.Checkpoints.Exceptions;
using Models.Checkpoints.Repositories;
using Model = Models.Checkpoints;
using Client = ClientModels.Checkpoints;
using Converter = ModelConverters.Checkpoints;

namespace API.Controllers
{
    [Route("api/v1/checkpoints")]
    public class CheckpointsController : ControllerBase
    {
        private readonly ICheckpointRepository repository;
        private const string Target = "Checkpoint";

        public CheckpointsController(ICheckpointRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Creates checkpoint
        /// </summary>
        /// <param name="creationInfo">Checkpoint creation info</param>
        /// <param name="cancellationToken"></param>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateCheckpointAsync([FromBody] Client.CheckpointCreationInfo creationInfo,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (creationInfo == null)
            {
                var error = ErrorResponsesService.BodyIsMissing(nameof(creationInfo));
                return BadRequest(error);
            }

            var modelCreationInfo = Converter.CheckpointCreationInfoConverter.Convert(creationInfo);
            var modelCheckpoint = await repository.CreateAsync(modelCreationInfo, cancellationToken).ConfigureAwait(false);

            var clientCheckpoint = Converter.CheckpointConverter.Convert(modelCheckpoint);
            return CreatedAtRoute("GetCheckpointRoute", new {id = clientCheckpoint.Id}, clientCheckpoint);
        }

        /// <summary>
        /// Returns a list of checkpoints
        /// </summary>
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllCheckpointsAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var modelCheckpointsList = await repository.GetAllAsync(cancellationToken).ConfigureAwait(false);
            var clientCheckpointsList = modelCheckpointsList.Select(Converter.CheckpointConverter.Convert).ToImmutableList();

            return Ok(clientCheckpointsList);
        }

        /// <summary>
        /// Returns a checkpoint by id
        /// </summary>
        /// <param name="id">checkpoint id</param>
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [Route("{id}", Name = "GetCheckpointRoute")]
        public async Task<IActionResult> GetCheckpointAsync([FromRoute] string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(id, out var guid))
            {
                var error = ErrorResponsesService.NotFoundError(Target, $"Checkpoint with id '{id}' not found.");
                return BadRequest(error);
            }

            Model.Checkpoint modelCheckpoint;

            try
            {
                modelCheckpoint = await repository.GetAsync(guid, cancellationToken).ConfigureAwait(false);
            }
            catch (CheckpointNotFoundException ex)
            {
                var error = ErrorResponsesService.NotFoundError(Target, ex.Message);
                return NotFound(error);
            }

            var clientCheckpoint = Converter.CheckpointConverter.Convert(modelCheckpoint);
            return Ok(clientCheckpoint);
        }

        /// <summary>
        /// Deletes checkpoint
        /// </summary>
        /// <param name="id">checkpoint id</param>
        /// <param name="cancellationToken"></param>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCheckpointAsync([FromRoute] string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(id, out var guid))
            {
                var error = ErrorResponsesService.NotFoundError(Target, $"Checkpoint with id '{id}' not found.");
                return NotFound(error);
            }

            try
            {
                await repository.RemoveAsync(guid, cancellationToken).ConfigureAwait(false);
            }
            catch (CheckpointNotFoundException ex)
            {
                var error = ErrorResponsesService.NotFoundError(Target, ex.Message);
                return NotFound(error);
            }

            return NoContent();
        }
    }
}