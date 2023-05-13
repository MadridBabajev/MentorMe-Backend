using System.Net;
using System.Net.Mime;
using App.BLL.Contracts;
using Asp.Versioning;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.Mappers;
using Public.DTO.v1;

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
    
    /// <summary>
    /// Get the student profile data.
    /// </summary>
    /// <param name="subjectId">The ID of the student.</param>
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{subjectId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetStudentProfile(Guid userId)
    {
        // Retrieve subject image from the database
        var user = new StudentProfile
        {
            Id = userId,
            FirstName = "First",
            LastName = "Last",
            MobilePhone = "5555555555",
            Balance = 0.0,
            AverageRating = 4.5,
            Title = "title",
            Bio = "biotext",
            ProfilePicture = null,
            Subjects = await _bll.SubjectsService.AllSubjects(),
            IsPublic = false
        };

        if (user != null) return Ok(user);
        return NotFound(new RestApiErrorResponse
        {
            Status = HttpStatusCode.NotFound,
            Error = $"Couldn't find the user with id {userId}"
        });

        // var subject = await _bll.SubjectsService.FindAsync(subjectId);
        // if (subject != null) return Ok(_detailsMapper.MapDetailsSubject(subject));
        // return NotFound(new RestApiErrorResponse
        // {
        //     Status = HttpStatusCode.NotFound,
        //     Error = $"Couldn't find the subject with id {subjectId}"
        // });
    }

    /// <summary>
    /// Get the tutor profile data.
    /// </summary>
    /// <param name="subjectId">The ID of the tutor.</param>
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{subjectId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetTutorProfile(Guid userId)
    {
        // Retrieve subject image from the database
        var user = new TutorProfile
        {
            Id = userId,
            FirstName = "First",
            LastName = "Last",
            MobilePhone = "5555555555",
            Balance = 0.0,
            AverageRating = 4.5,
            Title = "title",
            Bio = "biotext",
            ProfilePicture = null,
            Subjects = await _bll.SubjectsService.AllSubjects(),
            IsPublic = false,
            HourlyRate = 15.0,
            Availabilities = new List<TutorAvailability>
            {
                new()
                {
                    TutorId = userId,
                    FromHours = TimeSpan.FromHours(9),
                    ToHours = TimeSpan.FromHours(12),
                    DayOfTheWeek = EAvailabilityDayOfTheWeek.Monday
                },
                new()
                {
                    TutorId = userId,
                    FromHours = TimeSpan.FromHours(14),
                    ToHours = TimeSpan.FromHours(18),
                    DayOfTheWeek = EAvailabilityDayOfTheWeek.Wednesday
                },
            }
        };

        if (user != null) return Ok(user);
        return NotFound(new RestApiErrorResponse
        {
            Status = HttpStatusCode.NotFound,
            Error = $"Couldn't find the user with id {userId}"
        });
    }
}