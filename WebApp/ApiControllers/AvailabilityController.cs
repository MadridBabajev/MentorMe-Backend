using System.Net;
using System.Net.Mime;
using App.BLL.Contracts;
using Asp.Versioning;
using AutoMapper;
using Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.Mappers;
using Public.DTO.v1;
using Public.DTO.v1.Profiles.Secondary;

namespace WebApp.ApiControllers;

/// <summary>
/// API controller for handling requests related to availabilities.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public class AvailabilityController: ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly AvailabilityMapper _availabilityMapper;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="AvailabilityController"/> class.
    /// </summary>
    /// <param name="bll">The business logic layer instance.</param>
    /// <param name="autoMapper">The AutoMapper instance.</param>
    public AvailabilityController(IAppBLL bll, IMapper autoMapper)
    {
        _bll = bll;
        _availabilityMapper = new AvailabilityMapper(autoMapper);
    }
    
    /// <summary>
    /// Getting a list of all user's availabilities
    /// </summary>
    /// <returns>A list of availabilities</returns>
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<Availability>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetAvailabilitiesList()
    {
        Guid tutorId = User.GetUserId();
        try
        {
            var res = await _bll.AvailabilityService.GetAllAvailabilities(tutorId);
            return Ok(res.Select(a => _availabilityMapper.Map(a)));
        }
        catch (Exception e)
        {
            return FormatErrorResponse($"Error retrieving the payment method list: {e.Message}");
        }
    }

    /// <summary>
    /// Removing an availability
    /// </summary>
    /// <param name="availabilityId">Specifies the availability to remove</param>
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> RemoveAvailability([FromQuery] string availabilityId)
    {

        try
        {
            // await _bll.AvailabilityService.DeleteAvailability(availability.AvailabilityId);
            await _bll.AvailabilityService.DeleteAvailability(Guid.Parse(availabilityId));
            return Ok();
        }
        catch (Exception e)
        {
            return FormatErrorResponse($"Error occured when removing an availability: {e}");
        }
    }
    
    /// <summary>
    /// Adds a new availability
    /// </summary>
    /// <param name="newAvailability">Data of the new availability</param>
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> AddAvailability([FromBody] NewAvailability newAvailability)
    {
        try
        {
            Guid tutorId = User.GetUserId();
            await _bll.AvailabilityService.AddAvailability(newAvailability, tutorId);
            return Ok();
        }
        catch (Exception e)
        {
            return FormatErrorResponse($"Error occured when adding an availability: {e}");
        }
    }
    
    private ActionResult FormatErrorResponse(string message) {
        return BadRequest(new RestApiErrorResponse {
            Status = HttpStatusCode.BadRequest,
            Error = message
        });
    }
    
}
