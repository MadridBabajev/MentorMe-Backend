using System.Net;
using System.Net.Mime;
using App.BLL.Contracts;
using Asp.Versioning;
using AutoMapper;
using Domain.Enums;
using Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.Mappers;
using Public.DTO.v1;
using Public.DTO.v1.Identity;
using Public.DTO.v1.Profiles;
using Public.DTO.v1.Profiles.Secondary;

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
    private readonly BankingDetailsMapper _bankingDetailsMapper;
    private readonly EditProfileMapper _editProfileMapper;
    private readonly UpdatedProfileDataMapper _updatedProfileDataMapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProfileController"/> class.
    /// </summary>
    /// <param name="bll">The business logic layer instance.</param>
    /// <param name="autoMapper">The AutoMapper instance.</param>
    public ProfileController(IAppBLL bll, IMapper autoMapper)
    {
        _bll = bll;
        _studentProfileMapper = new StudentProfileMapper(autoMapper);
        _tutorProfileMapper = new TutorProfileMapper(autoMapper);
        _tutorsSearchMapper = new TutorsSearchMapper(autoMapper);
        _bankingDetailsMapper = new BankingDetailsMapper(autoMapper);
        _editProfileMapper = new EditProfileMapper(autoMapper);
        _updatedProfileDataMapper = new UpdatedProfileDataMapper(autoMapper);
    }
    
    /// <summary>
    /// Getting the student profile
    /// </summary>
    /// <param name="profileDataRequest">Field specifying whether
    /// the user want to see the page's information as a guest or the owner</param>
    /// <returns>Student profile details</returns>
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(StudentProfile), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetStudentProfile([FromBody] ProfileDataRequest profileDataRequest)
    {
        try
        {
            Guid userId = User.GetUserId();
            return Ok(_studentProfileMapper.Map(await 
                _bll.StudentsService.GetStudentProfile(userId, profileDataRequest.VisitedUserId)));

        }
        catch (Exception e)
        {
            return FormatErrorResponse($"Error retrieving student data: {e.Message}");
        }
        
    }
    
    /// <summary>
    /// Getting the tutor profile
    /// </summary>
    /// <param name="profileDataRequest">Field specifying whether
    /// the user want to see the page's information as a guest or the owner</param>
    /// <returns>Tutor profile details</returns>
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(TutorProfile), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetTutorProfile([FromBody] ProfileDataRequest profileDataRequest)
    {
        
        Guid userId = User.GetUserId();

        try
        {
            var res = Ok(_tutorProfileMapper.Map(await 
                _bll.TutorsService.GetTutorProfile(userId, profileDataRequest.VisitedUserId)));
            return res;
        }
        catch (Exception e)
        {
            return FormatErrorResponse($"Error retrieving tutor data: {e.Message}");
        }
    }

    /// <summary>
    /// Getting a filtered list of tutors
    /// </summary>
    /// <param name="tutorSearchFilters">Filters</param>
    /// <returns>List of filtered users</returns>
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<TutorSearch>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    public async Task<IActionResult> GetTutorsList([FromBody] TutorSearchFilters tutorSearchFilters)
    {
        try
        {
            var res = await _bll.TutorsService.GetTutorsWithFilters(tutorSearchFilters);
            return Ok(res.Select(ts => _tutorsSearchMapper.Map(ts)));
        }
        catch (Exception e)
        {
            return FormatErrorResponse($"Error retrieving tutor list: {e.Message}");
        }
    }
    
    /// <summary>
    /// Getting user banking details
    /// </summary>
    /// <returns>Tutor banking details data</returns>
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(TutorBankingDetails), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetTutorBankingDetails()
    {
        try
        {
            Guid tutorId = User.GetUserId();
            
            var bankingDetails = await _bll.TutorsService.GetTutorBankingDetails(tutorId);
            return Ok(_bankingDetailsMapper.Map(bankingDetails));
        }
        catch (Exception e)
        {
            return FormatErrorResponse($"Error finding the payment: {e.Message}");
        }
    }
    
    /// <summary>
    /// Edits tutor banking details
    /// </summary>
    /// <param name="updatedBankingDetails">Updated banking details</param>
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType( StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> EditTutorBankingDetails([FromBody] TutorBankingDetailsWithoutType updatedBankingDetails)
    {
        try
        {
            Guid tutorId = User.GetUserId();
            var tutorBankingDetailsWithType = new TutorBankingDetails
            {
                BankAccountName = updatedBankingDetails.BankAccountName,
                BankAccountNumber = updatedBankingDetails.BankAccountNumber,
                BankAccountType = updatedBankingDetails.BankAccountType == 0
                    ? EBankAccountType.Personal
                    : EBankAccountType.Business
            };
            
            await _bll.TutorsService.EditTutorBankingDetails(tutorId, tutorBankingDetailsWithType);
            return Ok();
        }
        catch (Exception e)
        {
            return FormatErrorResponse($"Error editing the banking details: {e.Message}");
        }
    }
    
    /// <summary>
    /// Getting editable user data
    /// </summary>
    /// <returns>Editable user data</returns>
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(UpdatedProfileData),  StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetEditProfileData()
    {
        try
        {
            Guid userId = User.GetUserId();
            
            var editProfileData = await _bll.StudentsService.GetUserEditableData(userId);
            return Ok(_editProfileMapper.Map(editProfileData));
        }
        catch (Exception e)
        {
            return FormatErrorResponse($"Error finding the payment: {e.Message}");
        }
    }
    
    /// <summary>
    /// Editing user profile details
    /// </summary>
    /// <param name="updatedProfileDataRequest">New profile data set by user</param>
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType( StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> EditProfileData([FromBody] UpdateProfileDataRequest updatedProfileDataRequest)
    {
        try
        {
            Guid userId = User.GetUserId();
            var updatedProfileData = _updatedProfileDataMapper.Map(updatedProfileDataRequest);

            if (updatedProfileData!.UserType == "Student")
            {
                await _bll.StudentsService.UpdateStudentProfile(userId, updatedProfileData);
            }
            else
            {
                await _bll.TutorsService.UpdateTutorProfile(userId, updatedProfileData);
            }

            return Ok();
        }
        catch (Exception e)
        {
            return FormatErrorResponse($"Error updating the profile data: {e.Message}");
        }
    }
    
    private ActionResult FormatErrorResponse(string message) {
        return BadRequest(new RestApiErrorResponse {
            Status = HttpStatusCode.BadRequest,
            Error = message
        });
    }
}
