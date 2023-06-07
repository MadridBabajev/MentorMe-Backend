using System.Net;
using System.Net.Mime;
using App.BLL.Contracts;
using Asp.Versioning;
using AutoMapper;
using Domain.Entities;
using Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.Mappers;
using Public.DTO.v1;
using Public.DTO.v1.Subjects;

namespace WebApp.ApiControllers;

/// <summary>
/// API controller for handling requests related to subjects.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public class SubjectsController : ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly SubjectsMapper _mapper;
    private readonly SubjectDetailsMapper _detailsMapper;
    private readonly SubjectsFilterMapper _subjectsFilterMapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubjectsController"/> class.
    /// </summary>
    /// <param name="bll">The business logic layer instance.</param>
    /// <param name="autoMapper">The AutoMapper instance.</param>
    public SubjectsController(IAppBLL bll, IMapper autoMapper)
    {
        _bll = bll;
        _mapper = new SubjectsMapper(autoMapper);
        _detailsMapper = new SubjectDetailsMapper(autoMapper);
        _subjectsFilterMapper = new SubjectsFilterMapper(autoMapper);
    }

    /// <summary>
    /// Get a list of all subjects.
    /// </summary>
    /// <returns>A list of subjects.</returns>
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<Subject>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SubjectListElement>>> GetAllSubjects()
    {
        // Get subjects from the database
        try
        {
            var subjects = await _bll.SubjectsService.AllSubjects();
            return subjects.Select(e => _mapper.MapListSubject(e)).ToList();
        }
        catch (Exception e)
        {
            return FormatErrorResponse($"Error retrieving the subject list: {e.Message}");
        }
    }

    /// <summary>
    /// Get the subject details.
    /// </summary>
    /// <param name="subjectId">The ID of the subject.</param>
    /// <returns>The image file associated with the subject.</returns>
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(SubjectDetails), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{subjectId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Optional")]
    public async Task<IActionResult> GetSubjectDetails(Guid subjectId)
    {
        Guid? userId;

        try
        {
            userId = User.GetUserId();
        }
        catch (Exception)
        {
            userId = null;
        }

        try
        {
            var subject = await _bll.SubjectsService.FindSubjectAsync(subjectId, userId);
            if (subject != null) return Ok(_detailsMapper.MapDetailsSubject(subject));
            return FormatErrorResponse($"Error finding the subject:");
        }
        catch (Exception e)
        {
            return FormatErrorResponse($"Error modifying the subject details: {e.Message}");
        }
    }
    
    /// <summary>
    /// Get the subject filters.
    /// </summary>
    /// <returns>The image file associated with the subject.</returns>
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<SubjectsFilterElement>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet]
    public async Task<IActionResult> GetSubjectFilters()
    {
        try
        {
            var res = await _bll.SubjectsService.AllSubjectFilters();
            return Ok(res.Select(s => _subjectsFilterMapper.Map(s)));
        }
        catch (Exception e)
        {
            return FormatErrorResponse($"Error retrieving subjects as filters: {e.Message}");
        }
    }

    /// <summary>
    /// Endpoint for adding or removing a subject for users
    /// </summary>
    /// <param name="userSubjectAction">The action of the user</param>
    /// <returns></returns>
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> AddRemoveSubject([FromBody] UserSubjectAction userSubjectAction)
    {
        Guid userId = User.GetUserId();

        try
        {
            await _bll.SubjectsService.AddRemoveUserSubject(userSubjectAction, userId);
            return Ok();
        }
        catch (Exception e)
        {
            return FormatErrorResponse($"Error adding/removing the subject: {e.Message}");
        }
    }
    
    private ActionResult FormatErrorResponse(string message) {
        return BadRequest(new RestApiErrorResponse {
            Status = HttpStatusCode.BadRequest,
            Error = message
        });
    }
}