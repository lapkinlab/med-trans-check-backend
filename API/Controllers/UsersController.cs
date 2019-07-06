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
using Models.Users;
using Model = Models.Users;
using Client = ClientModels.Users;
using Converter = ModelConverters.Users;
using Models.Roles;

namespace API.Controllers
{
    /// <summary>
    /// Контролллер пользователей
    /// </summary>
    [Route("api/v1/users")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private const string Target = "User";
        private const string PasswordValidationMessage = "Password and confirmed password don't match.";

        public UsersController(UserManager<User> userManager)
        {
            this.userManager = userManager ?? throw new ArgumentException(nameof(userManager));
        }

        /// <summary>
        /// Создание пользователя
        /// </summary>
        /// <param name="clientUserCreationInfo">Модель создания </param>
        /// <param name="cancellationToken"></param>
        [HttpPost]
        [AllowAnonymous]
        [Route("")]
        public async Task<IActionResult> CreateUserAsync([FromBody] Client.UserCreationInfo clientUserCreationInfo,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (clientUserCreationInfo == null)
            {
                var error = ErrorResponsesService.BodyIsMissing(nameof(clientUserCreationInfo));
                return BadRequest(error);
            }

            if (!clientUserCreationInfo.Password.Equals(clientUserCreationInfo.ConfirmPassword))
            {
                var error = ErrorResponsesService.ValidationError(nameof(clientUserCreationInfo.ConfirmPassword), 
                    PasswordValidationMessage);
                return BadRequest(error);
            }

            UserCreationInfo modelCreationInfo;
            
            try
            {
                modelCreationInfo = Converter.UserCreationInfoConverter.Convert(clientUserCreationInfo);
            }
            catch (ArgumentNullException ex)
            {
                var error = ErrorResponsesService.InvalidCredentialsError(ex.Message);
                return BadRequest(error);
            }
            
            var user = await userManager.FindByNameAsync(modelCreationInfo.UserName);

            if (user != null)
            {
                var error = ErrorResponsesService.DuplicationError(Target, $"User with name '{clientUserCreationInfo.UserName}' already exists.");
                return BadRequest(error);
            }

            var dateTime = DateTime.UtcNow;
            var modelUser = new User
            {
                UserName = modelCreationInfo.UserName,
                Name = modelCreationInfo.Name,
                Email = modelCreationInfo.Email,
                PhoneNumber = modelCreationInfo.PhoneNumber,
                RegisteredAt = dateTime,
                LastUpdateAt = dateTime
            };

            var result = await userManager.CreateAsync(modelUser, modelCreationInfo.Password);

            if (!result.Succeeded)
            {
                var error = ErrorResponsesService.ValidationError(nameof(clientUserCreationInfo), 
                    result.Errors.First().Description);
                return BadRequest(error);
            }

            await userManager.AddToRoleAsync(modelUser, "user");
            var clientUser = ModelConverters.Users.UserConverter.Convert(modelUser);
            return CreatedAtRoute("GetUserRoute", new { username = clientUser.UserName }, clientUser);
        }

        /// <summary>
        /// Получение списка пользователей
        /// </summary>
        /// <param name="clientSearchInfo"></param>
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllUsersAsync([FromQuery]Client.UserSearchInfo clientSearchInfo, 
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var users = await Task.Run(() => userManager.Users, cancellationToken);

            var modelSearchInfo =
                Converter.UserSearchInfoConverter.Convert(clientSearchInfo ?? new Client.UserSearchInfo());

            if (modelSearchInfo.Offset != null)
            {
                users = users.Skip(modelSearchInfo.Offset.Value);
            }

            if (modelSearchInfo.Limit != null)
            {
                users = users.Take(modelSearchInfo.Limit.Value);
            }

            users = users.OrderByDescending(item => item.LastUpdateAt);

            var clientUsers = users.Select(user => Converter.UserConverter.Convert(user));
            return Ok(clientUsers);
        }

        /// <summary>
        /// Получение пользователя по логину
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [Route("{userName}", Name = "GetUserRoute")]
        [Authorize]
        public async Task<IActionResult> GetUserAsync([FromRoute]string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!HasAccessToUser(userName))
            {
                return Forbid();
            }

            var user = await userManager.FindByNameAsync(userName);

            if (user == null)
            {
                var error = ErrorResponsesService.NotFoundError(Target, $"User with name '{userName}' not found.");
                return BadRequest(error);
            }

            var clientUser = ModelConverters.Users.UserConverter.Convert(user);
            return Ok(clientUser);
        }

        /// <summary>
        /// Получение информации о пользователе по cookie
        /// </summary>
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [Route("info")]
        [Authorize]
        public async Task<IActionResult> GetUserInfoAsync(CancellationToken cancellationToken)
        {
            if (HttpContext.User?.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);

                if (user != null)
                {
                    var clientUser = ModelConverters.Users.UserConverter.Convert(user);
                    return Ok(clientUser);
                }
            }

            var error = ErrorResponsesService.UnauthorizedError(Target);
            return NotFound(error);
        }

        /// <summary>
        /// Модификация пользовательских данных
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="clientPatchInfo">Модель модификации пользователя</param>
        /// <param name="cancellationToken"></param>
        [HttpPatch]
        [Route("{userName}")]
        [Authorize]
        public async Task<IActionResult> PatchUserAsync([FromRoute] string userName, [FromBody] Client.UserPatchInfo clientPatchInfo,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (clientPatchInfo == null)
            {
                var error = ErrorResponsesService.BodyIsMissing(nameof(clientPatchInfo));
                return BadRequest(error);
            }

            if (!HasAccessToUser(userName))
            {
                return Forbid();
            }

            var modelPatchInfo = ModelConverters.Users.UserPatchInfoConverter.Convert(userName, clientPatchInfo);
            var user = await userManager.FindByNameAsync(userName);

            if (user == null)
            {
                var error = ErrorResponsesService.NotFoundError(Target, $"User with name '{userName}' not found.");
                return BadRequest(error);
            }

            var updated = false;

            if (modelPatchInfo.OldPassword != null && modelPatchInfo.Password != null)
            {
                var passwordHasher = HttpContext.RequestServices.
                    GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;
                //todo неработает проверка на правильность старого пароля
//                var oldPasswordHash = passwordHasher.HashPassword(user, modelPatchInfo.OldPassword);
//
//                if (!oldPasswordHash.Equals(user.PasswordHash))
//                {
//                    var error = ErrorResponsesService.ValidationError(nameof(clientPatchInfo.OldPassword), 
//                        "Old password doesn't match with actual.");
//                    return BadRequest(error);
//                }
                
                if (!modelPatchInfo.Password.Equals(modelPatchInfo.ConfirmPassword))
                {
                    var error = ErrorResponsesService.ValidationError(nameof(clientPatchInfo.ConfirmPassword), 
                        PasswordValidationMessage);
                    return BadRequest(error);
                }
                
                var passwordValidator = HttpContext.RequestServices.
                    GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                var result = await passwordValidator.ValidateAsync(userManager, user, modelPatchInfo.Password);

                if (result.Succeeded)
                {
                    user.PasswordHash = passwordHasher.HashPassword(user, modelPatchInfo.Password);
                    updated = true;
                }
            }

            if (updated)
            {
                user.LastUpdateAt = DateTime.UtcNow;
                await userManager.UpdateAsync(user);
            }

            var clientUser = ModelConverters.Users.UserConverter.Convert(user);
            return Ok(clientUser);
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        [Route("{userName}")]
        public async Task<ActionResult> DeleteUserAsync([FromRoute]string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!HasAccessToUser(userName))
            {
                return Forbid();
            }

            var user = await userManager.FindByNameAsync(userName);
            
            if (user == null)
            {
                var error = ErrorResponsesService.NotFoundError(Target, $"User with name '{userName}' not found.");
                return NotFound(error);
            }

            await userManager.DeleteAsync(user);
            return NoContent();
        }
        
        private bool HasAccessToUser(string userName)
        {
            return HttpContext.User.IsInRole("admin") ||
                   string.Compare(HttpContext.User.Identity.Name, userName.ToLower(), StringComparison.Ordinal) == 0;
        }
    }
}