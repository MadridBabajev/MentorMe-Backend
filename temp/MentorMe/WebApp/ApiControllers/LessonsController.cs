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
/// API controller for handling requests related to lessons.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public class LessonsController: ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly ReserveLessonMapper _reserveLessonMapper;
    private readonly LessonDataMapper _lessonDataMapper;
    private readonly LessonListMapper _lessonListMapper;
    private readonly PaymentMapper _paymentMapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="LessonsController"/> class.
    /// </summary>
    /// <param name="bll">The business logic layer instance.</param>
    /// <param name="autoMapper">The AutoMapper instance.</param>
    public LessonsController(IAppBLL bll, IMapper autoMapper)
    {
        _bll = bll;
        _reserveLessonMapper = new ReserveLessonMapper(autoMapper);
        _lessonDataMapper = new LessonDataMapper(autoMapper);
        _lessonListMapper = new LessonListMapper(autoMapper);
        _paymentMapper = new PaymentMapper(autoMapper);
    }
    
    /// <summary>
    /// Getting the necessary data for lesson reservation
    /// </summary>
    /// <param name="profileDataRequest">Specifies, which tutor we want to reserve a lesson from</param>
    /// <returns>Lesson Reservation data</returns>
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ReserveLessonData), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetReserveLessonData([FromBody] ProfileDataRequest profileDataRequest)
    {

        try
        {
            Guid studentId = User.GetUserId();
            var res = await _bll.LessonsService
                .GetReserveLessonData(studentId, profileDataRequest.VisitedUserId);
        
            return Ok(_reserveLessonMapper.Map(res));
        }
        catch (Exception e)
        {
            return FormatErrorResponse($"Error getting the lesson reserve data: {e.Message}");
        }
    }
    
    /// <summary>
    /// Reserving and creating a lesson
    /// </summary>
    /// <param name="reserveLessonRequest">Data the user specified during the reservation process</param>
    /// <returns>New lesson id wrapped inside of an object</returns>
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ReserveLessonResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> ReserveLesson(ReserveLessonRequest reserveLessonRequest)
    {
        Guid studentId = User.GetUserId();

        try
        {
            Guid lessonId = await _bll.LessonsService.CreateLesson(reserveLessonRequest, studentId);
            return Ok(new ReserveLessonResponse{ LessonId = lessonId });
        }
        catch (Exception e)
        {
            return FormatErrorResponse($"Error reserving the lesson {e.Message}");
        }
    }

    /// <summary>
    /// Getting the lesson data
    /// </summary>
    /// <param name="lessonId">The lesson id</param>
    /// <returns>Lesson data</returns>
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
    
    /// <summary>
    /// Getting a list of user lessons
    /// </summary>
    /// <returns>User lessons list</returns>
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<LessonListElement>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetLessonsList()
    {
        try
        {
            Guid userId = User.GetUserId();
            var res = await  _bll.LessonsService.GetLessonsList(userId);
            return Ok(res.Select(l => _lessonListMapper.Map(l)));
        }
        catch (Exception e)
        {
            return FormatErrorResponse($"Error retrieving the lesson list: {e.Message}");
        }
        
    }
    
    /// <summary>
    /// Adds a review from one user to another once the lesson is finished
    /// </summary>
    /// <param name="userReview">User review</param>
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType( StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> LeaveReview([FromBody] UserReview userReview)
    {
        try
        {
            Guid userId = User.GetUserId();
            await _bll.LessonsService.LeaveReview(userReview, userId);
            
            return Ok();
        }
        catch (Exception e)
        {
            return FormatErrorResponse($"Error occured when adding a review: {e}");
        }
    }
    
    /// <summary>
    /// Adds a new tag to the lesson
    /// </summary>
    /// <param name="tag">New tag</param>
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType( StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> AddTag([FromBody] NewTag tag)
    {
        try
        {
            Guid tutorId = User.GetUserId();
            await _bll.LessonsService.AddTag(tag, tutorId);
            return Ok();
        }
        catch (Exception e)
        {
            return FormatErrorResponse($"Error occured when adding a tag: {e}");
        }
    }
    
    /// <summary>
    /// Removes a tag
    /// </summary>
    /// <param name="tag">Tag to remove</param>
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType( StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> RemoveTag([FromBody] RemoveTag tag)
    {
        try
        {
            await _bll.LessonsService.DeleteTag(tag.TagId);
            return Ok();
        }
        catch (Exception e)
        {
            return FormatErrorResponse($"Error occured when removing a tag: {e}");
        }
    }
    
    /// <summary>
    /// Cancels a lesson
    /// </summary>
    /// <param name="lessonId">Specifies the lesson to cancel</param>
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType( StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost("{lessonId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> CancelLesson(Guid lessonId)
    {
        try
        {
            await _bll.LessonsService.CancelLesson(lessonId);
            return Ok();
        }
        catch (Exception e)
        {
            return FormatErrorResponse($"Error occured when cancelling a lesson: {e}");
        }
    }
    
    /// <summary>
    /// Accepts or declines an incoming lesson request
    /// </summary>
    /// <param name="acceptDeclineRequest">Specifies the action of the tutor</param>
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType( StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> AcceptDeclineLesson([FromBody] AcceptDeclineRequest acceptDeclineRequest)
    {
        try
        {
            await _bll.LessonsService.AcceptDeclineLesson(acceptDeclineRequest.LessonId, acceptDeclineRequest.TutorDecision);
            return Ok();
        }
        catch (Exception e)
        {
            return FormatErrorResponse($"Error occured when accepting/declining a lesson: {e}");
        }
    }
    
    /// <summary>
    /// Getting the lesson payment data
    /// </summary>
    /// <param name="paymentId">Specifies the payment id</param>
    /// <returns></returns>
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(Payment),  StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{paymentId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetPayment(Guid paymentId)
    {
        try
        {
            var payment = await _bll.LessonsService.GetPaymentData(paymentId);
            return Ok(_paymentMapper.Map(payment));
        }
        catch (Exception e)
        {
            return FormatErrorResponse($"Error finding the payment: {e.Message}");
        }
    }
    
    private ActionResult FormatErrorResponse(string message) {
        return BadRequest(new RestApiErrorResponse {
            Status = HttpStatusCode.BadRequest,
            Error = message
        });
    }
}