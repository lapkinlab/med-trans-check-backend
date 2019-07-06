using System;
using System.Threading;
using System.Threading.Tasks;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.MechanicNotes.Exceptions;
using Models.MechanicNotes.Repositories;
using Models.Users;
using Model = Models.MechanicNotes;
using Client = ClientModels.MechanicNotes;
using Converter = ModelConverters.MechanicNotes;

namespace API.Controllers
{
    [Route("api/v1/mechanic-notes")]
    public class MechanicNotesController : ControllerBase
    {
        private readonly IMechanicNoteRepository repository;
        private readonly UserManager<User> userManager;
        private const string Target = "MechanicNote";

        public MechanicNotesController(IMechanicNoteRepository repository, UserManager<User> userManager)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.userManager = userManager ?? throw new ArgumentException(nameof(userManager));
        }

        /// <summary>
        /// Creates mechanicNote
        /// </summary>
        /// <param name="cancellationToken"></param>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateMechanicNoteAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var modelMechanicNote = await repository.CreateAsync(cancellationToken)
                .ConfigureAwait(false);

            var clientMechanicNote = Converter.MechanicNoteConverter.Convert(modelMechanicNote);
            return CreatedAtRoute("GetMechanicNoteRoute", new {id = clientMechanicNote.Id}, clientMechanicNote);
        }

        /// <summary>
        /// Returns a mechanicNote by id
        /// </summary>
        /// <param name="id">MechanicNote id</param>
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [Route("{id}", Name = "GetMechanicNoteRoute")]
        public async Task<IActionResult> GetMechanicNoteAsync([FromRoute] string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(id, out var guid))
            {
                var error = ErrorResponsesService.NotFoundError(Target, $"MechanicNote with id '{id}' not found.");
                return BadRequest(error);
            }

            Model.MechanicNote modelMechanicNote;

            try
            {
                modelMechanicNote = await repository.GetAsync(guid, cancellationToken).ConfigureAwait(false);
            }
            catch (MechanicNoteNotFoundException ex)
            {
                var error = ErrorResponsesService.NotFoundError(Target, ex.Message);
                return NotFound(error);
            }

            var clientMechanicNote = Converter.MechanicNoteConverter.Convert(modelMechanicNote);
            return Ok(clientMechanicNote);
        }

        /// <summary>
        /// Updates mechanicNote info
        /// </summary>
        /// <param name="id">MechanicNote id</param>
        /// <param name="patchInfo">MechanicNote patch info</param>
        /// <param name="cancellationToken"></param>
        [HttpPatch]
        [Authorize]
        [Authorize(Roles = "mechanic, admin")]
        [Route("{id}")]
        public async Task<IActionResult> PatchMechanicNoteAsync([FromRoute] string id,
            [FromBody] Client.MechanicNotePatchInfo patchInfo, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (patchInfo == null)
            {
                var error = ErrorResponsesService.BodyIsMissing(nameof(patchInfo));
                return BadRequest(error);
            }

            if (!Guid.TryParse(id, out var guid))
            {
                var error = ErrorResponsesService.NotFoundError(Target, $"MechanicNote with id '{id}' not found.");
                return BadRequest(error);
            }

            var userName = HttpContext.User.Identity.Name;
            var user = await userManager.FindByNameAsync(userName);

            var modelPatchInfo = Converter.MechanicNotePatchInfoConverter.Convert(guid, user.Name, patchInfo);
            Model.MechanicNote modelMechanicNote;

            try
            {
                modelMechanicNote = await repository.PatchAsync(modelPatchInfo, cancellationToken).ConfigureAwait(false);
            }
            catch (MechanicNoteNotFoundException ex)
            {
                var error = ErrorResponsesService.NotFoundError(Target, ex.Message);
                return NotFound(error);
            }

            var clientMechanicNote = Converter.MechanicNoteConverter.Convert(modelMechanicNote);
            return Ok(clientMechanicNote);
        }

        /// <summary>
        /// Deletes mechanicNote
        /// </summary>
        /// <param name="id">MechanicNote id</param>
        /// <param name="cancellationToken"></param>
        [HttpDelete]
        [Authorize(Roles = "mechanic, admin")]
        [Route("{id}")]
        public async Task<IActionResult> DeleteMechanicNoteAsync([FromRoute] string id,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(id, out var guid))
            {
                var error = ErrorResponsesService.NotFoundError(Target, $"MechanicNote with id '{id}' not found.");
                return BadRequest(error);
            }

            try
            {
                await repository.RemoveAsync(guid, cancellationToken).ConfigureAwait(false);
            }
            catch (MechanicNoteNotFoundException ex)
            {
                var error = ErrorResponsesService.NotFoundError(Target, ex.Message);
                return NotFound(error);
            }

            return NoContent();
        }
    }
}