using System.Net;
using System.Net.Mime;
using App.BLL.Contracts;
using Asp.Versioning;
using AutoMapper;
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
    // private readonly SubjectsMapper _mapper;
    // private readonly SubjectDetailsMapper _detailsMapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubjectsController"/> class.
    /// </summary>
    /// <param name="bll">The business logic layer instance.</param>
    /// <param name="autoMapper">The AutoMapper instance.</param>
    public ProfileController(IAppBLL bll /*, IMapper autoMapper*/)
    {
        _bll = bll;
        // _mapper = new SubjectsMapper(autoMapper);
        // _detailsMapper = new SubjectDetailsMapper(autoMapper);
    }
    
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(StudentProfile), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetStudentProfile([FromBody] UserProfileRequest userProfileRequest)
    {
        
        Guid userId = User.GetUserId();
        return Ok(await _bll.StudentsService.GetStudentProfile(userId, userProfileRequest.VisitedUserId));
    }
    
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(TutorProfile), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetTutorProfile([FromBody] UserProfileRequest userProfileRequest)
    {
        
        Guid userId = User.GetUserId();
        return Ok(await _bll.TutorsService.GetTutorProfile(userId, userProfileRequest.VisitedUserId));
    }

    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<TutorSearch>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    public async Task<IActionResult> GetTutorsList([FromBody] TutorSearchFilters tutorSearchFilters)
    {
        
        return Ok(await 
            _bll.TutorsService.GetTutorsWithFilters(tutorSearchFilters));
    }
}