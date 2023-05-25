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
using Public.DTO.v1.Identity;
using Public.DTO.v1.Lessons;

namespace WebApp.ApiControllers;

/// <summary>
/// API controller for handling requests related to profiles.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public class LessonsController: ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly ReserveLessonMapper _reserveLessonMapper;
    private readonly LessonDataMapper _lessonDataMapper;
    private readonly LessonListMapper _LessonListMapper;
    // private readonly SubjectDetailsMapper _detailsMapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubjectsController"/> class.
    /// </summary>
    /// <param name="bll">The business logic layer instance.</param>
    /// <param name="autoMapper">The AutoMapper instance.</param>
    public LessonsController(IAppBLL bll, IMapper autoMapper)
    {
        _bll = bll;
        _reserveLessonMapper = new ReserveLessonMapper(autoMapper);
        _lessonDataMapper = new LessonDataMapper(autoMapper);
        _LessonListMapper = new LessonListMapper(autoMapper);
    }
    
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ReserveLessonData), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetReserveLessonData([FromBody] ProfileDataRequest profileDataRequest)
    {
        Guid studentId = User.GetUserId();
        var res = await _bll.LessonsService
            .GetReserveLessonData(studentId, profileDataRequest.VisitedUserId);
        
        return Ok(_reserveLessonMapper.Map(res));
    }
    
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ReserveLessonResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> ReserveLesson(ReserveLessonRequest reserveLessonRequest)
    {
        Guid studentId = User.GetUserId();
        
        // Create lesson and return the lessonId
        Guid lessonId = await _bll.LessonsService.CreateLesson(reserveLessonRequest, studentId);
        return Ok(new ReserveLessonResponse{ LessonId = lessonId });
    }

    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(LessonData), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{lessonId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetLessonData(Guid lessonId)
    {
        Guid userId = User.GetUserId();
        var lesson = await _bll.LessonsService.GetLessonData(userId, lessonId);
        
        if (lesson == null)
        {
            return FormatErrorResponse("Lesson was not found");
        }

        var lessonBelongsToTheUser = _bll.LessonsService.LessonBelongsToUser(lesson, userId);
        if (!lessonBelongsToTheUser)
        {
            return FormatErrorResponse("This lesson does not belong to the user");
        }

        return Ok(_lessonDataMapper.Map(lesson));
    }
    
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<LessonListElement>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetLessonsList()
    {
        Guid userId = User.GetUserId();
        var res = await  _bll.LessonsService.GetLessonsList(userId);
        return Ok(res.Select(l => _LessonListMapper.Map(l)));
    }
    
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType( StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public Task<IActionResult> LeaveReview([FromBody] UserReview userReview)
    {
        try
        {
            Guid userId = User.GetUserId();
            _bll.LessonsService.LeaveReview(userReview, userId);
            
            return Task.FromResult<IActionResult>(Ok());
        }
        catch (Exception e)
        {
            return Task.FromResult<IActionResult>(FormatErrorResponse($"Error occured when adding a review: {e}"));
        }
    }
    
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType( StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public Task<IActionResult> AddTag([FromBody] NewTag tag)
    {
        try
        {
            Guid tutorId = User.GetUserId();
            _bll.LessonsService.AddTag(tag, tutorId);
            return Task.FromResult<IActionResult>(Ok());
        }
        catch (Exception e)
        {
            return Task.FromResult<IActionResult>(FormatErrorResponse($"Error occured when adding a tag: {e}"));
        }
    }
    
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType( StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public Task<IActionResult> RemoveTag([FromBody] RemoveTag tag)
    {
        try
        {
            _bll.LessonsService.DeleteTag(tag.TagId);
            return Task.FromResult<IActionResult>(Ok());
        }
        catch (Exception e)
        {
            return Task.FromResult<IActionResult>(FormatErrorResponse($"Error occured when removing a tag: {e}"));
        }
    }
    
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType( StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost("{lessonId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public Task<IActionResult> CancelLesson(Guid lessonId)
    {
        try
        {
            _bll.LessonsService.CancelLesson(lessonId);
            return Task.FromResult<IActionResult>(Ok());
        }
        catch (Exception e)
        {
            return Task.FromResult<IActionResult>(FormatErrorResponse($"Error occured when cancelling a lesson: {e}"));
        }
    }
    
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType( StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public Task<IActionResult> AcceptDeclineLesson([FromBody] AcceptDeclineRequest acceptDeclineRequest)
    {
        try
        {
            _bll.LessonsService.AcceptDeclineLesson(acceptDeclineRequest.LessonId, acceptDeclineRequest.TutorDecision);
            return Task.FromResult<IActionResult>(Ok());
        }
        catch (Exception e)
        {
            return Task.FromResult<IActionResult>(FormatErrorResponse($"Error occured when accepting/declining a lesson: {e}"));
        }
    }
    
    private ActionResult FormatErrorResponse(string message) {
        return BadRequest(new RestApiErrorResponse {
            Status = HttpStatusCode.BadRequest,
            Error = message
        });
    }
}