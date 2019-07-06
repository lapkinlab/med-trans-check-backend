using System;
using System.Threading;
using System.Threading.Tasks;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.MedicNotes.Exceptions;
using Models.MedicNotes.Repositories;
using Models.Users;
using Model = Models.MedicNotes;
using Client = ClientModels.MedicNotes;
using Converter = ModelConverters.MedicNotes;

namespace API.Controllers
{
    [Route("api/v1/medic-notes")]
    public class MedicNotesController : ControllerBase
    {
        private readonly IMedicNoteRepository repository;
        private readonly UserManager<User> userManager;
        private const string Target = "MedicNote";

        public MedicNotesController(IMedicNoteRepository repository, UserManager<User> userManager)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.userManager = userManager ?? throw new ArgumentException(nameof(userManager));
        }

        /// <summary>
        /// Creates medicNote
        /// </summary>
        /// <param name="cancellationToken"></param>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateMedicNoteAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var modelMedicNote = await repository.CreateAsync(cancellationToken)
                .ConfigureAwait(false);

            var clientMedicNote = Converter.MedicNoteConverter.Convert(modelMedicNote);
            return CreatedAtRoute("GetMedicNoteRoute", new {id = clientMedicNote.Id}, clientMedicNote);
        }

        /// <summary>
        /// Returns a medicNote by id
        /// </summary>
        /// <param name="id">MedicNote id</param>
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [Route("{id}", Name = "GetMedicNoteRoute")]
        public async Task<IActionResult> GetMedicNoteAsync([FromRoute] string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(id, out var guid))
            {
                var error = ErrorResponsesService.NotFoundError(Target, $"MedicNote with id '{id}' not found.");
                return NotFound(error);
            }

            Model.MedicNote modelMedicNote;

            try
            {
                modelMedicNote = await repository.GetAsync(guid, cancellationToken).ConfigureAwait(false);
            }
            catch (MedicNoteNotFoundException ex)
            {
                var error = ErrorResponsesService.NotFoundError(Target, ex.Message);
                return NotFound(error);
            }

            var clientMedicNote = Converter.MedicNoteConverter.Convert(modelMedicNote);
            return Ok(clientMedicNote);
        }

        /// <summary>
        /// Updates medicNote info
        /// </summary>
        /// <param name="id">MedicNote id</param>
        /// <param name="patchInfo">MedicNote patch info</param>
        /// <param name="cancellationToken"></param>
        [HttpPatch]
        [Authorize]
        [Authorize(Roles = "medic, admin")]
        [Route("{id}")]
        public async Task<IActionResult> PatchMedicNoteAsync([FromRoute] string id,
            [FromBody] Client.MedicNotePatchInfo patchInfo, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (patchInfo == null)
            {
                var error = ErrorResponsesService.BodyIsMissing(nameof(patchInfo));
                return BadRequest(error);
            }

            if (!Guid.TryParse(id, out var guid))
            {
                var error = ErrorResponsesService.NotFoundError(Target, $"MedicNote with id '{id}' not found.");
                return NotFound(error);
            }

            var userName = HttpContext.User.Identity.Name;
            var user = await userManager.FindByNameAsync(userName);

            var modelPatchInfo = Converter.MedicNotePatchInfoConverter.Convert(guid, user.Name, patchInfo);
            Model.MedicNote modelMedicNote;

            try
            {
                modelMedicNote = await repository.PatchAsync(modelPatchInfo, cancellationToken).ConfigureAwait(false);
            }
            catch (MedicNoteNotFoundException ex)
            {
                var error = ErrorResponsesService.NotFoundError(Target, ex.Message);
                return NotFound(error);
            }

            var clientMedicNote = Converter.MedicNoteConverter.Convert(modelMedicNote);
            return Ok(clientMedicNote);
        }

        /// <summary>
        /// Deletes medicNote
        /// </summary>
        /// <param name="id">MedicNote id</param>
        /// <param name="cancellationToken"></param>
        [HttpDelete]
        [Authorize(Roles = "medic, admin")]
        [Route("{id}")]
        public async Task<IActionResult> DeleteMedicNoteAsync([FromRoute] string id,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(id, out var guid))
            {
                var error = ErrorResponsesService.NotFoundError(Target, $"MedicNote with id '{id}' not found.");
                return NotFound(error);
            }

            try
            {
                await repository.RemoveAsync(guid, cancellationToken).ConfigureAwait(false);
            }
            catch (MedicNoteNotFoundException ex)
            {
                var error = ErrorResponsesService.NotFoundError(Target, ex.Message);
                return NotFound(error);
            }

            return NoContent();
        }
    }
}