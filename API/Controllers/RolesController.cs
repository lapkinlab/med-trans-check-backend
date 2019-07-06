using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Roles;
using Models.Users;
using Model = Models.Roles;
using Client = ClientModels.Roles;
using Converter = ModelConverters.Roles;

namespace API.Controllers
{
    /// <summary>
    /// Контроллер ролей
    /// </summary>
    [Route("api/v1/roles")]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<Role> roleManager;
        private readonly UserManager<User> userManager;
        private const string TargetRole = "Role";
        private const string TargetUser = "User";

        /// <summary>
        /// Конструктор ролей
        /// </summary>
        /// <param name="roleManager">Менеджер ролей</param>
        /// <param name="userManager">Менеджер пользователей</param>
        public RolesController(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        /// <summary>
        /// Получение списка ролей
        /// </summary>
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllRoles(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var roles = await Task.Run(() => roleManager.Roles, cancellationToken);

            if (roles == null)
            {
                var error = ErrorResponsesService.NotFoundError(TargetRole, "Roles not found.");
                return NotFound(error);
            }

            var clientRoles = roles.Select(item => Converter.RoleConverter.Convert(item)).ToImmutableList();
            return Ok(clientRoles);
        }

        /// <summary>
        /// Изменение роли пользователя
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="clientRolePatchInfo">Модель изменения пользователя</param>
        /// <param name="cancellationToken"></param>
        [HttpPatch]
        [Route("{userName}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PatchRoleAsync([FromRoute]string userName, 
            [FromBody] Client.RolePatchInfo clientRolePatchInfo, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            if (clientRolePatchInfo == null)
            {
                var error = ErrorResponsesService.BodyIsMissing(nameof(clientRolePatchInfo));
                return BadRequest(error);
            }

            var user = await userManager.FindByNameAsync(userName);
            
            if (user == null)
            {
                var error = ErrorResponsesService.NotFoundError(TargetUser, $"User with name '{userName}' not found.");
                return NotFound(error);
            }

            var modelRolePatchInfo = Converter.RolePatchInfoConverter.Convert(userName, clientRolePatchInfo);
            var modelRole = await roleManager.FindByNameAsync(modelRolePatchInfo.UserRole.ToLower());

            if (modelRole == null)
            {
                var error = ErrorResponsesService.NotFoundError(TargetRole, $"Role with name '{modelRolePatchInfo.UserRole}' not found.");
                return NotFound(error);
            }

            if (user.Roles.Contains(modelRolePatchInfo.UserRole.ToUpper()))
            {
                await userManager.RemoveFromRoleAsync(user, modelRolePatchInfo.UserRole);
            }
            else
            {
                await userManager.AddToRoleAsync(user, modelRolePatchInfo.UserRole);
            }

            return Ok(user);
        }
    }
}