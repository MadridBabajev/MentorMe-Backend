using System.Net;
using System.Net.Mime;
using App.BLL.Contracts;
using Asp.Versioning;
using AutoMapper;
using Domain.Entities;
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
        // TODO: Implement adding removing subjects for tutors and students
        // Retrieve subjects from the database
        var subjects = await _bll.SubjectsService.AllSubjects();
        return subjects.Select(e => _mapper.MapListSubject(e)).ToList();
    }

    /// <summary>
    /// Get the subject details.
    /// </summary>
    /// <param name="subjectId">The ID of the subject.</param>
    /// <returns>The image file associated with the subject.</returns>
    [HttpGet("{subjectId}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSubjectDetails(Guid subjectId)
    {
        // Retrieve subject image from the database
        var subject = await _bll.SubjectsService.FindAsync(subjectId);
        if (subject != null) return Ok(_detailsMapper.MapDetailsSubject(subject));
        return NotFound(new RestApiErrorResponse
        {
            Status = HttpStatusCode.NotFound,
            Error = $"Couldn't find the subject with id {subjectId}"
        });
    }
    
    /// <summary>
    /// Get the subject details.
    /// </summary>
    /// <param name="subjectId">The ID of the subject.</param>
    /// <returns>The image file associated with the subject.</returns>
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<SubjectsFilterElement>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet]
    public async Task<IActionResult> GetSubjectFilters()
    {
        
        return Ok(await _bll.SubjectsService.AllSubjectFilters());
    }
}