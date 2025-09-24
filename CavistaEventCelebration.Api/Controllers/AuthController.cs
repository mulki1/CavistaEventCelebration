using CavistaEventCelebration.Api.Models;
using CavistaEventCelebration.Api.Models.Authentication;
using CavistaEventCelebration.Api.Models.EmailService;
using CavistaEventCelebration.Api.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CavistaEventCelebration.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMailService _mailService;

        public AuthController(IAuthenticationService authentication, IMailService mailService)
        {
            _authenticationService = authentication;
            _mailService = mailService;
        }

        [HttpPost("Signup")]
        public async Task<ActionResult<SignInResponse>> CreateUser(UserSignInModel userSignInModel)
        {
            if (userSignInModel != null)
            {
                var result = await _authenticationService.CreateAsync(userSignInModel);
                if (result != null && result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }

            return BadRequest();
        }

        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] UserLoginModel userModel)
        {
            if (userModel != null)
            {
                var result = await _authenticationService.LoginAsync(userModel);

                if (result != null && result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }

            return BadRequest();
        }

        [HttpPost("RefreshToken")]
        public async Task<ActionResult<LoginResponse>> RefreshToken([FromBody] RefreshTokenModel refreshTokenModel)
        {
            if (refreshTokenModel != null)
            {
                var result = await _authenticationService.RefreshTokenAsync(refreshTokenModel);

                if (result != null && result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }

            return BadRequest();
        }

        [HttpPost("ChangePassword/{userId}")]
        public async Task<ActionResult<ChangePasswordResponse>> ChangePassword([FromBody] ChangePassword changePasswordModel,  string userId)
        {
            if (changePasswordModel != null)
            {
                var result = await _authenticationService.ChangePasswordAsync(userId, changePasswordModel);

                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }

            return BadRequest();
        }

        [HttpPost("ChangeUserRole/{userId}")]
        public async Task<ActionResult<ChangeUserRoleResponse>> ChangeUserRole([FromBody] ChangeUserRole changeUserRoleModel, string userId)
        {
            if (changeUserRoleModel != null)
            {
                var result = await _authenticationService.ChangeUserRoleAsync(userId, changeUserRoleModel);

                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }

            return BadRequest();
        }

        [HttpGet("GetRoles")]
        public async Task<ActionResult<List<GetRolesResponse>>> GetRoles()
        {
            var result = await _authenticationService.GetRolesAsync();

            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("GetUsers")]
        public async Task<ActionResult<PaginatedList<UserResponse>>> GetUsers(int? index, int? pageSize, string? searchString)
        {
            var result = await _authenticationService.GetUsersAsync(index, pageSize, searchString);

            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }


        [HttpGet("Test-email-on-smtp/{email}")]
        public async Task  Get(string email)
        {
            var rng = new Random();
            var message = new Message(new string[] { email }, "Test email", "This is the content from our email.");
            await _mailService.SendEmailAsync(message);
        }


    }
}
