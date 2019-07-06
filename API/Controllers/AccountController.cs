using System;
using System.Threading;
using System.Threading.Tasks;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model = Models.Users;
using Client = ClientModels.UserIdentity;

namespace API.Controllers
{
    /// <summary>
    /// Контроллер аутентификации
    /// </summary>
    [Route("api/v1")]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<Model.User> signInManager;

        public AccountController(SignInManager<Model.User> signInManager)
        {
            this.signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        /// <summary>
        /// Аутентификация пользователя
        /// </summary>
        /// <param name="loginInfo">Пользовательские данные для входа</param>
        /// <param name="cancellationToken"></param>
        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Client.UserLogin loginInfo, 
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (loginInfo == null)
            {
                var error = ErrorResponsesService.BodyIsMissing(nameof(loginInfo));
                return BadRequest(error);
            }

            var result = await signInManager.PasswordSignInAsync(loginInfo.Username, loginInfo.Password, 
                loginInfo.RememberMe, false);
            
            if (!result.Succeeded)
            {
                var error = ErrorResponsesService.InvalidCredentialsError(nameof(loginInfo));
                return BadRequest(error);
            }

            return Ok(result);
        }

        /// <summary>
        /// Выход из текущей сессии
        /// </summary>
        [HttpPost]
        [Authorize]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok();
        }
    }
}