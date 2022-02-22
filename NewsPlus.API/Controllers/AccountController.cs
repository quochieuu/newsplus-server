using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NewsPlus.Data.ViewModel;
using NewsPlus.Infrastructure;
using System.Security.Claims;

namespace NewsPlus.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/auth")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;
        private readonly IJwtAuthManager _jwtAuthManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public AccountController(ILogger<AccountController> logger,
            IUserService userService,
            IJwtAuthManager jwtAuthManager,
            IEmailSender emailSender,
            IConfiguration configuration)
        {
            _logger = logger;
            _userService = userService;
            _jwtAuthManager = jwtAuthManager;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!_userService.IsValidUserCredentials(request.UserName, request.Password))
            {
                return Unauthorized();
            }

            var role = _userService.GetUserRole(request.UserName);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,request.UserName),
                new Claim(ClaimTypes.Role, role)
            };

            var jwtResult = _jwtAuthManager.GenerateTokens(request.UserName, claims, DateTime.Now);
            _logger.LogInformation($"User [{request.UserName}] logged in the system.");
            return Ok(new LoginResult
            {
                UserName = request.UserName,
                Role = role,
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var check = _userService.IsAnExistingUser(request.UserName);
            if (check)
            {
                return BadRequest();
            }

            _userService.CreateUser(request);
            return Ok(request);
        }

        [HttpGet("user")]
        [Authorize]
        public ActionResult GetCurrentUser()
        {
            return Ok(new LoginResult
            {
                UserName = User.Identity?.Name,
                Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty
            });
        }

        [HttpPost("logout")]
        [Authorize]
        public ActionResult Logout()
        {
            var userName = User.Identity?.Name;
            _jwtAuthManager.RemoveRefreshTokenByUserName(userName);
            _logger.LogInformation($"User [{userName}] logged out the system.");
            return Ok();
        }

        [HttpPost("change-password")]
        [Authorize]
        public ActionResult RequestChangePassword()
        {
            var userName = User.Identity?.Name;

            string resetCode = new Random().Next(1000, 9999).ToString();

            _userService.SendPasswordResetCodeEmail(userName, resetCode);

            return Ok("Password reset link has been sent to your email...");
        }

        [HttpPost("reset-password")]
        [Authorize]
        public ActionResult ResetPasswordByEmailCode([FromForm] PasswordResetEmailCodeDto request)
        {
            var userName = User.Identity?.Name;

            _userService.ResetUserPasswordEmailCode(userName, request);

            return Ok();
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public ActionResult ForgotPassword(string email)
        {
            var user = _userService.FindUser(email);

            if (user != null)
            {
                string resetCode = new Random().Next(1000, 9999).ToString();

                _userService.SendPasswordResetCodeEmail(user.Username, resetCode);

                return Ok("Password reset code has been sent to your email...");
            }

            return BadRequest();

        }

        [HttpGet("request-verify-email")]
        [Authorize]
        public ActionResult RequestVerifyEmail()
        {
            string resetCode = Guid.NewGuid().ToString();

            var uriBuilder = $"{_configuration.GetSection("PortalUrl").Value}/api/auth/verify-email/{resetCode}";

            var link = uriBuilder.ToString();

            var userName = User.Identity?.Name;
            _userService.SendPasswordResetLinkEmail(userName, link, resetCode);

            return Ok("Password reset code has been sent to your email...");
        }

        [HttpGet("verify-email/{code}")]
        [AllowAnonymous]
        public ActionResult VerifyEmail(string code)
        {
            if (code == null)
                return BadRequest();

            var check = _userService.FindEmailByCode(code);

            if (check == string.Empty)
            {
                return BadRequest("Your code is not valid");
            }
            else
            {
                _userService.VerifyEmail(check);

                return Ok("Verified account successful!");
            }

        }

        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var userName = User.Identity?.Name;
                _logger.LogInformation($"User [{userName}] is trying to refresh JWT token.");

                if (string.IsNullOrWhiteSpace(request.RefreshToken))
                {
                    return Unauthorized();
                }

                var accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");
                var jwtResult = _jwtAuthManager.Refresh(request.RefreshToken, accessToken, DateTime.Now);
                _logger.LogInformation($"User [{userName}] has refreshed JWT token.");
                return Ok(new LoginResult
                {
                    UserName = userName,
                    Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
                    AccessToken = jwtResult.AccessToken,
                    RefreshToken = jwtResult.RefreshToken.TokenString
                });
            }
            catch (SecurityTokenException e)
            {
                return Unauthorized(e.Message); // return 401 so that the client side can redirect the user to login page
            }
        }
    }
}
