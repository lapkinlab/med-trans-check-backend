using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Models.Checkpoints.Repositories;
using Models.Routes.Exceptions;
using Models.Routes.Repositories;
using Model = Models.Routes;
using Client = ClientModels.Routes;
using Converter = ModelConverters.Routes;

namespace API.Controllers
{
    [Route("api/v1/routes")]
    public class RoutesController : ControllerBase
    {
        private readonly IRouteRepository routeRepository;
        private readonly ICheckpointRepository checkpointRepository;
        private const string Target = "Route";

        public RoutesController(IRouteRepository routeRepository, ICheckpointRepository checkpointRepository)
        {
            this.routeRepository = routeRepository ?? throw new ArgumentNullException(nameof(routeRepository));
            this.checkpointRepository = checkpointRepository ?? throw new ArgumentNullException(nameof(checkpointRepository));
        }

        /// <summary>
        /// Creates route
        /// </summary>
        /// <param name="creationInfo">Route creation info</param>
        /// <param name="cancellationToken"></param>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateRouteAsync([FromBody] Client.RouteCreationInfo creationInfo,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (creationInfo == null)
            {
                var error = ErrorResponsesService.BodyIsMissing(nameof(creationInfo));
                return BadRequest(error);
            }

            var modelCreationInfo = Converter.RouteCreationInfoConverter.Convert(creationInfo);
            var modelRoute = await routeRepository.CreateAsync(modelCreationInfo, cancellationToken).ConfigureAwait(false);
            var modelCheckpoints = await checkpointRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);

            var clientRoute = Converter.RouteConverter.Convert(modelRoute, modelCheckpoints);
            return CreatedAtRoute("GetRouteRoute", new {id = clientRoute.Id}, clientRoute);
        }

        /// <summary>
        /// Returns a list of routes
        /// </summary>
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllRoutesAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var modelRoutesList = await routeRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);
            var modelCheckpoints = await checkpointRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);
            var clientRoutesList = modelRoutesList.Select(item => Converter.RouteConverter.Convert(item, modelCheckpoints))
                .ToImmutableList();

            return Ok(clientRoutesList);
        }

        /// <summary>
        /// Returns a route by id
        /// </summary>
        /// <param name="id">route id</param>
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [Route("{id}", Name = "GetRouteRoute")]
        public async Task<IActionResult> GetRouteAsync([FromRoute] string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(id, out var guid))
            {
                var error = ErrorResponsesService.NotFoundError(Target, $"Route with id '{id}' not found.");
                return BadRequest(error);
            }

            Model.Route modelRoute;

            try
            {
                modelRoute = await routeRepository.GetAsync(guid, cancellationToken).ConfigureAwait(false);
            }
            catch (RouteNotFoundException ex)
            {
                var error = ErrorResponsesService.NotFoundError(Target, ex.Message);
                return NotFound(error);
            }
            
            var modelCheckpoints = await checkpointRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);
            var clientRoute = Converter.RouteConverter.Convert(modelRoute, modelCheckpoints);
            return Ok(clientRoute);
        }

        /// <summary>
        /// Deletes route
        /// </summary>
        /// <param name="id">route id</param>
        /// <param name="cancellationToken"></param>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteRouteAsync([FromRoute] string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(id, out var guid))
            {
                var error = ErrorResponsesService.NotFoundError(Target, $"Route with id '{id}' not found.");
                return BadRequest(error);
            }

            try
            {
                await routeRepository.RemoveAsync(guid, cancellationToken).ConfigureAwait(false);
            }
            catch (RouteNotFoundException ex)
            {
                var error = ErrorResponsesService.NotFoundError(Target, ex.Message);
                return NotFound(error);
            }

            return NoContent();
        }
    }
}