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

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public class AvailabilityController: ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly AvailabilityMapper _availabilityMapper;
    
    public AvailabilityController(IAppBLL bll, IMapper autoMapper)
    {
        _bll = bll;
        _availabilityMapper = new AvailabilityMapper(autoMapper);
    }
    
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
    
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> RemoveAvailability([FromBody] RemoveAvailability availability)
    {

        try
        {
            await _bll.AvailabilityService.DeleteAvailability(availability.AvailabilityId);
            return Ok();
        }
        catch (Exception e)
        {
            return FormatErrorResponse($"Error occured when removing an availability: {e}");
        }
    }
    
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
