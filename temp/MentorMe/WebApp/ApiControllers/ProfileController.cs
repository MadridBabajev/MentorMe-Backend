using System.Net;
using System.Net.Mime;
using App.BLL.Contracts;
using Asp.Versioning;
using AutoMapper;
using BLL.DTO.Profiles;
using Domain.Entities;
using Domain.Enums;
using Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.Mappers;
using Public.DTO.v1;
using Public.DTO.v1.Identity;
using Public.DTO.v1.Profiles;

namespace WebApp.ApiControllers;

/// <summary>
/// API controller for handling requests related to profiles.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public class ProfileController: ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly StudentProfileMapper _studentProfileMapper;
    private readonly TutorProfileMapper _tutorProfileMapper;
    private readonly TutorsSearchMapper _tutorsSearchMapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubjectsController"/> class.
    /// </summary>
    /// <param name="bll">The business logic layer instance.</param>
    /// <param name="autoMapper">The AutoMapper instance.</param>
    public ProfileController(IAppBLL bll, IMapper autoMapper)
    {
        _bll = bll;
        _studentProfileMapper = new StudentProfileMapper(autoMapper);
        _tutorProfileMapper = new TutorProfileMapper(autoMapper);
        _tutorsSearchMapper = new TutorsSearchMapper(autoMapper);
    }
    
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(StudentProfile), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetStudentProfile([FromBody] ProfileDataRequest profileDataRequest)
    {
        // TODO: map from bll to public
        Guid userId = User.GetUserId();
        return Ok(_studentProfileMapper.Map(await _bll.StudentsService.GetStudentProfile(userId, profileDataRequest.VisitedUserId)));
    }
    
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(TutorProfile), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetTutorProfile([FromBody] ProfileDataRequest profileDataRequest)
    {
        
        Guid userId = User.GetUserId();
        // return
        //     Ok(await _bll.TutorsService.GetTutorProfile(userId,
        //         profileDataRequest
        //             .VisitedUserId)); 
        // TODO: Fix the mapping
        return Ok(_tutorProfileMapper.Map(await _bll.TutorsService
             .GetTutorProfile(userId, profileDataRequest.VisitedUserId)));
    }

    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<TutorSearch>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    public async Task<IActionResult> GetTutorsList([FromBody] TutorSearchFilters tutorSearchFilters)
    {
        
        var res = await _bll.TutorsService.GetTutorsWithFilters(tutorSearchFilters);
        return Ok(res.Select(ts => _tutorsSearchMapper.Map(ts)));
    }
}