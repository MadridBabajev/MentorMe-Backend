using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mime;
using System.Security.Claims;
using App.DAL.EF;
using Asp.Versioning;
using Domain.Enums;
using Domain.Identity;
using Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Public.DTO.v1;
using Public.DTO.v1.Identity;

namespace WebApp.ApiControllers.Identity;

/// <summary>
/// Controller responsible for account related actions.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/identity/[controller]/[action]")]
public class AccountController : ControllerBase
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AccountController> _logger;
    private readonly Random _rnd = new();
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the AccountController class.
    /// </summary>
    /// <param name="signInManager">Provides the APIs for user sign in.</param>
    /// <param name="userManager">Provides the APIs for managing user in a persistence store.</param>
    /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
    /// <param name="logger">Represents a type used to perform logging.</param>
    /// <param name="context">The database context.</param>
    public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
        IConfiguration configuration, ILogger<AccountController> logger, ApplicationDbContext context)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _configuration = configuration;
        _logger = logger;
        _context = context;
    }

    /// <summary>
    /// Register new user to the system.
    /// </summary>
    /// <param name="registrationData">User registration data.</param>
    /// <param name="expiresInSeconds">Optional, overrides default JWT token expiration value.</param>
    /// <returns>JWTResponse with JWT and refresh token.</returns>
    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(JWTResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<JWTResponse>> Register([FromBody] Register registrationData,
        [FromQuery]
        int expiresInSeconds)
    {
        if (expiresInSeconds <= 0) expiresInSeconds = int.MaxValue;

        // is user already registered
        var existingUsers = await _userManager.Users.Where(u => u.Email == registrationData.Email).ToListAsync();

        var isTutorProfileExists = existingUsers.Any(u => u.AppUserType == EUserType.Tutor);
        var isStudentProfileExists = existingUsers.Any(u => u.AppUserType == EUserType.Student);

        if ((registrationData.IsTutor && isTutorProfileExists) || (!registrationData.IsTutor && isStudentProfileExists))
        {
            _logger.LogWarning("User with email {} is already registered with the given user type", registrationData.Email);
            return BadRequest(new RestApiErrorResponse
            {
                Status = HttpStatusCode.BadRequest,
                Error = $"User with email {registrationData.Email} is already registered with the given user type"
            });
        }

        // register user
        var refreshToken = new AppRefreshToken();
        var appUser = new AppUser
        {
            Email = registrationData.Email,
            MobilePhone = registrationData.MobilePhone,
            UserName = $"{registrationData.Email}_{(registrationData.IsTutor ? "Tutor" : "Student")}",
            FirstName = registrationData.Firstname,
            LastName = registrationData.Lastname,
            AppUserType = registrationData.IsTutor ? EUserType.Tutor : EUserType.Student,
            AppRefreshTokens = new List<AppRefreshToken> {refreshToken},
            Country = registrationData.Country
        };
        refreshToken.AppUser = appUser;

        var result = await _userManager.CreateAsync(appUser, registrationData.Password);
        if (!result.Succeeded)
        {
            var errorDescription = result.Errors.First().Description;
            _logger.LogError("Error registering user: {ErrorDescription}", errorDescription);
            
            return BadRequest(new RestApiErrorResponse
            {
                Status = HttpStatusCode.BadRequest,
                Error = result.Errors.First().Description
            });
        }
        
        // save into claims also the user full name
        result = await _userManager.AddClaimsAsync(appUser, new List<Claim>
        {
            new(ClaimTypes.GivenName, appUser.FirstName),
            new(ClaimTypes.Surname, appUser.LastName),
            new("UserType", appUser.AppUserType.ToString()),
        });

        if (!result.Succeeded)
        {
            return BadRequest(new RestApiErrorResponse
            {
                Status = HttpStatusCode.BadRequest,
                Error = result.Errors.First().Description
            });
        }
        
        // get full user from system with fixed data (maybe there is something generated by identity that we might need
        appUser = await _userManager.FindByEmailAsync(appUser.Email);
        if (appUser == null)
        {
            _logger.LogWarning("User with email {} is not found after registration", registrationData.Email);
            return BadRequest(new RestApiErrorResponse
            {
                Status = HttpStatusCode.BadRequest,
                Error = $"User with email {registrationData.Email} is not found after registration"
            });
        }
        
        int expiresIn = expiresInSeconds < _configuration.GetValue<int>("JWT:ExpiresInSeconds")
            ? expiresInSeconds
            : _configuration.GetValue<int>("JWT:ExpiresInSeconds");

        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        
        var jwt = GenerateJwt(
            claimsPrincipal.Claims,
            appUser.AppUserType.ToString(),
            expiresIn
        );
        
        var res = new JWTResponse
        {
            JWT = jwt,
            RefreshToken = refreshToken.RefreshToken,
            ExpiresIn = expiresIn
        };
        
        var response = result.Succeeded.ToString();
        _logger.LogInformation("Response: {Response}", response);
        return Ok(res);
    }

    /// <summary>
    /// Log in to the system 
    /// </summary>
    /// <param name="loginData">User login info.</param>
    /// <param name="expiresInSeconds">optional, override default value.</param>
    /// <returns>JWTResponse with JWT and refresh token</returns>
    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(JWTResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<JWTResponse>> LogIn([FromBody] Login loginData, [FromQuery] int expiresInSeconds)
    {
        if (expiresInSeconds <= 0) expiresInSeconds = int.MaxValue;

        // TODO right now it doesn't check for the user type
        
        // verify if the user exists
        var appUsers = await _userManager.Users
            .Where(u => u.Email == loginData.Email).ToListAsync();
        
        if (!appUsers.Any())
        {
            _logger.LogWarning("WebApi login failed, email {} not found", loginData.Email);
            await Task.Delay(_rnd.Next(100, 1000));

            return NotFound(new RestApiErrorResponse
            {
                Status = HttpStatusCode.NotFound,
                Error = "User/Password problem"
            });
        }

        var appUser = IdentityHelpers.FilterOutUsers(loginData.IsTutor, appUsers);
        
        if (appUser == null)
        {
            _logger.LogWarning("WebApi login failed, email {} not found", loginData.Email);
            await Task.Delay(_rnd.Next(100, 1000));
        
            return NotFound(new RestApiErrorResponse
            {
                Status = HttpStatusCode.NotFound,
                Error = "User/Password problem"
            });
        }
        
        // Check if the user is logging in with the correct user type
        if ((loginData.IsTutor && appUser.AppUserType != EUserType.Tutor) ||
            (!loginData.IsTutor && appUser.AppUserType != EUserType.Student))
        {
            _logger.LogWarning("WebApi login failed, wrong user type for user {}", loginData.Email);
            await Task.Delay(_rnd.Next(100, 1000));
            return NotFound(new RestApiErrorResponse
            {
                Status = HttpStatusCode.NotFound,
                Error = "User/Password problem"
            });
        }

        // verify username and password
        var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginData.Password, false);
        if (!result.Succeeded)
        {
            _logger.LogWarning("WebApi login failed, password problem for user {}", loginData.Email);
            await Task.Delay(_rnd.Next(100, 1000));
            return NotFound(new RestApiErrorResponse
            {
                Status = HttpStatusCode.NotFound,
                Error = "User/Password problem"
            });
        }

        // get claims based user
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null)
        {
            _logger.LogWarning("Could not get ClaimsPrincipal for user {}", loginData.Email);
            await Task.Delay(_rnd.Next(100, 1000));
            return NotFound(new RestApiErrorResponse
            {
                Status = HttpStatusCode.NotFound,
                Error = "User/Password problem"
            });
        }

        appUser.AppRefreshTokens = await _context
            .Entry(appUser)
            .Collection(a => a.AppRefreshTokens!)
            .Query()
            .Where(t => t.AppUserId == appUser.Id)
            .ToListAsync();

        // remove expired tokens
        foreach (var userRefreshToken in appUser.AppRefreshTokens)
        {
            if (
                userRefreshToken.ExpirationDT < DateTime.UtcNow &&
                (
                    userRefreshToken.PreviousExpirationDT == null ||
                    userRefreshToken.PreviousExpirationDT < DateTime.UtcNow
                )
            )
            {
                _context.AppRefreshTokens.Remove(userRefreshToken);
            }
        }

        var refreshToken = new AppRefreshToken
        {
            AppUserId = appUser.Id
        };
        _context.AppRefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();
        
        // generate jwt
        
        int expiresIn = expiresInSeconds < _configuration.GetValue<int>("JWT:ExpiresInSeconds")
            ? expiresInSeconds
            : _configuration.GetValue<int>("JWT:ExpiresInSeconds");
        
        var jwt = GenerateJwt(
            claimsPrincipal.Claims,
            appUser.AppUserType.ToString(),
            expiresIn
        );

        var res = new JWTResponse
        {
            JWT = jwt,
            RefreshToken = refreshToken.RefreshToken,
            ExpiresIn = expiresIn
        };

        return Ok(res);
    }

    /// <summary>
    /// Refreshes the JWT token for a user.
    /// </summary>
    /// <param name="refreshTokenModel">The refresh token model.</param>
    /// <param name="expiresInSeconds">The new JWT token expiration duration in seconds.</param>
    /// <returns>The new JWT and refresh token, or an error message if the operation fails.</returns>
    [HttpPost]
    public async Task<ActionResult> RefreshToken(
        [FromBody] RefreshTokenModel refreshTokenModel,
        [FromQuery] int expiresInSeconds)
    {
        if (expiresInSeconds <= 0) expiresInSeconds = int.MaxValue;
        
        JwtSecurityToken jwtToken;
        // get user info from jwt
        try
        {
            jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(refreshTokenModel.Jwt);
            if (jwtToken == null)
            {
                return BadRequest(new RestApiErrorResponse
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "No token"
                });
            }
        }
        
        catch (Exception e)
        {
            return BadRequest(new RestApiErrorResponse
            {
                Status = HttpStatusCode.BadRequest,
                Error = $"Cant parse the token, {e.Message}"
            });
        }

        if (!IdentityHelpers.ValidateToken(refreshTokenModel.Jwt, _configuration["JWT:Key"]!,
                _configuration["JWT:Issuer"]!,
                _configuration["JWT:Audience"]!))
        {
            return BadRequest(new RestApiErrorResponse
            {
                Status = HttpStatusCode.BadRequest,
                Error = "JWT validation fail"
            });
        }

        var userEmail = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        if (userEmail == null)
        {
            return BadRequest(new RestApiErrorResponse
            {
                Status = HttpStatusCode.BadRequest,
                Error = "No email in jwt"
            });
        }

        // get user and tokens
        var appUser = await _userManager.Users
            .Include(u => u.AppRefreshTokens)
            .FirstOrDefaultAsync(u => u.Email == userEmail);
        if (appUser == null)
        {
            return NotFound(new RestApiErrorResponse
            {
                Status = HttpStatusCode.NotFound,
                Error = $"User with email {userEmail} not found"
            });
        }

        // load and compare refresh tokens
        await _context.Entry(appUser)
            .Collection(u => u.AppRefreshTokens!)
            .Query()
            .Where(x =>
                (x.RefreshToken == refreshTokenModel.RefreshToken && x.ExpirationDT > DateTime.UtcNow) ||
                (x.PreviousRefreshToken == refreshTokenModel.RefreshToken &&
                 x.PreviousExpirationDT > DateTime.UtcNow)
            )
            .ToListAsync();

        if (appUser.AppRefreshTokens == null || appUser.AppRefreshTokens.Count == 0)
        {
            return NotFound(new RestApiErrorResponse
            {
                Status = HttpStatusCode.NotFound,
                Error = $"RefreshTokens collection is null or empty - {appUser.AppRefreshTokens?.Count}"
            });
        }

        // generate new jwt

        // get claims based user
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null)
        {
            _logger.LogWarning("Could not get ClaimsPrincipal for user {}", userEmail);
            return NotFound(new RestApiErrorResponse
            {
                Status = HttpStatusCode.NotFound,
                Error = "User/Password problem"
            });
        }

        // generate jwt
        int expiresIn = expiresInSeconds < _configuration.GetValue<int>("JWT:ExpiresInSeconds")
            ? expiresInSeconds
            : _configuration.GetValue<int>("JWT:ExpiresInSeconds");
        
        var jwt = GenerateJwt(
            claimsPrincipal.Claims,
            appUser.AppUserType.ToString(),
            expiresIn
        );

        // make new refresh token, keep old one still valid for some time
        var refreshToken = appUser.AppRefreshTokens.First();
        if (refreshToken.RefreshToken == refreshTokenModel.RefreshToken)
        {
            refreshToken.PreviousRefreshToken = refreshToken.RefreshToken;
            refreshToken.PreviousExpirationDT = DateTime.UtcNow.AddMinutes(1);

            refreshToken.RefreshToken = Guid.NewGuid().ToString();
            refreshToken.ExpirationDT = DateTime.UtcNow.AddDays(14);

            await _context.SaveChangesAsync();
        }

        var res = new JWTResponse
        {
            JWT = jwt,
            RefreshToken = refreshToken.RefreshToken,
            ExpiresIn = expiresIn
        };

        return Ok(res);
    }

    /// <summary>
    /// Logs out a user by invalidating their refresh token.
    /// </summary>
    /// <param name="logout">The logout request containing the refresh token to invalidate.</param>
    /// <returns>An OK result if the logout is successful, otherwise an error message.</returns>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost]
    public async Task<ActionResult> Logout(
        [FromBody]
        Logout logout)
    {
        // delete the refresh token - so user is kicked out after jwt expiration
        // We do not invalidate the jwt - that would require pipeline modification and checking against db on every request
        // so client can actually continue to use the jwt until it expires (keep the jwt expiration time short ~1 min)

        var userId = User.GetUserId();

        var appUser = await _context.Users
            .Where(u => u.Id == userId)
            .SingleOrDefaultAsync();
        if (appUser == null)
        {
            return NotFound(new RestApiErrorResponse
            {
                Status = HttpStatusCode.NotFound,
                Error = "User/Password problem"
            });
        }
        
        await _context.Entry(appUser)
            .Collection(u => u.AppRefreshTokens!)
            .Query()
            .Where(x =>
                x.RefreshToken == logout.RefreshToken ||
                x.PreviousRefreshToken == logout.RefreshToken
            )
            .ToListAsync();

        foreach (var appRefreshToken in appUser.AppRefreshTokens!)
        {
            _context.AppRefreshTokens.Remove(appRefreshToken);
        }

        var deleteCount = await _context.SaveChangesAsync();

        return Ok(new {TokenDeleteCount = deleteCount});
    }
    
    private string GenerateJwt(IEnumerable<Claim> claims, string userType, int expiresInSeconds)
    {
        expiresInSeconds = expiresInSeconds < _configuration.GetValue<int>("JWT:ExpiresInSeconds")
            ? expiresInSeconds
            : _configuration.GetValue<int>("JWT:ExpiresInSeconds");

        return IdentityHelpers.GenerateJwt(
            claims,
            _configuration["JWT:Key"]!,
            _configuration["JWT:Issuer"]!,
            _configuration["JWT:Audience"]!,
            userType,
            expiresInSeconds
        );
    }
}