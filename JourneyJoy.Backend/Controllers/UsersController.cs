using JourneyJoy.Backend.Options;
using JourneyJoy.Backend.Validation;
using JourneyJoy.Contracts;
using JourneyJoy.Model.Database.Tables;
using JourneyJoy.Model.DTOs;
using JourneyJoy.Model.Requests;
using JourneyJoy.Utils.Security.HashAlgorithms;
using JourneyJoy.Utils.Security.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace JourneyJoy.Backend.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        #region Fields

        private AppOptions settings;
        private IRepositoryWrapper repositoryWrapper;
        private IHashAlgorithm hashAlgorithm;
        #endregion

        #region Constructors
        public UsersController(IOptions<AppOptions> settings, IRepositoryWrapper repositoryWrapper, IHashAlgorithm hashAlgorithm)
        {
            this.settings = settings.Value;
            this.repositoryWrapper = repositoryWrapper;
            this.hashAlgorithm = hashAlgorithm;
        }
        #endregion

        #region Post methods
        // POST users/register
        /// <summary>
        /// Register as a user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [TypeFilter(typeof(RegisterUserValidation))]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PostRegisterClient([FromBody] RegisterUserRequest request)
        {
            repositoryWrapper.UserRepository.Create(new User()
            {
                Email = request.Email,
                Nickname = request.Nickname,
                Password = hashAlgorithm.Hash(request.Password),
            });
            repositoryWrapper.Save();

            return Ok();
        }

        // POST users/login
        /// <summary>
        /// Log in as a user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PostClientsLogin([FromBody] LoginUserRequest request)
        {
            var user = repositoryWrapper.UserRepository.FindUserByEmail(request.Email);
            if (user == null)
                return BadRequest("Email doesn't exist in the system");
            if (!hashAlgorithm.Verify(request.Password, user.Password))
                return BadRequest("Password is incorrect");

            var token = JwtTokenHelper.CreateToken(request.Email, user.Id.ToString(), settings.JwtKey, settings.JwtIssuer, Request.Host.Host);
            Response.Cookies.Append("Token", token.token, token.options);

            return Ok(new LoginDTO
            {
                Token = token.token,
                ExpiresAt = token.options.Expires?.DateTime,
                User = UserDTO.FromDatabaseUser(user)
            });
        }
        #endregion
    }
}
