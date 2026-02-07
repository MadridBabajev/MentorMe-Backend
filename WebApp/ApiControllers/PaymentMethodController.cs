using System.Net;
using System.Net.Mime;
using App.BLL.Contracts;
using Asp.Versioning;
using AutoMapper;
using Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.v1;
using Public.DTO.v1.Profiles.Secondary;
using PaymentMethodDetailedMapper = Public.DTO.Mappers.PaymentMethodDetailedMapper;

namespace WebApp.ApiControllers;

/// <summary>
/// Controller for handling payment method operations
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public class PaymentMethodController: ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly PaymentMethodDetailedMapper _paymentMethodDetailedMapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProfileController"/> class.
    /// </summary>
    /// <param name="bll">The business logic layer instance.</param>
    /// <param name="autoMapper">The AutoMapper instance.</param>
    public PaymentMethodController(IAppBLL bll, IMapper autoMapper)
    {
        _bll = bll;
        _paymentMethodDetailedMapper = new PaymentMethodDetailedMapper(autoMapper);
    }
    
    /// <summary>
    /// Getting list of all the student's payment methods
    /// </summary>
    /// <returns>List of payment methods</returns>
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<PaymentMethodDetailed>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetPaymentMethodsList()
    {
        Guid userId = User.GetUserId();
        try
        {
            var res = await _bll.PaymentMethodService.GetAllPaymentMethods(userId);
            return Ok(res.Select(pm => _paymentMethodDetailedMapper.Map(pm)));
        }
        catch (Exception e)
        {
            return FormatErrorResponse($"Error retrieving the payment method list: {e.Message}");
        }
    }

    /// <summary>
    /// Removing a payment method
    /// </summary>
    /// <param name="paymentMethodId">Specifies what payment method to delete</param>
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> RemovePaymentMethod([FromQuery] string paymentMethodId)
    {
        try
        {
            await _bll.PaymentMethodService.DeletePaymentMethod(Guid.Parse(paymentMethodId));
            return Ok();
        }
        catch (Exception e)
        {
            return FormatErrorResponse($"Error occured when removing a payment method: {e}");
        }
    }
    
    /// <summary>
    /// Adding a new payment method
    /// </summary>
    /// <param name="newPaymentMethod">Details of a new payment method</param>
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> AddPaymentMethod([FromBody] NewPaymentMethod newPaymentMethod)
    {
        try
        {
            Guid studentId = User.GetUserId();
            await _bll.PaymentMethodService.AddPaymentMethod(newPaymentMethod, studentId);
            return Ok();
        }
        catch (Exception e)
        {
            return FormatErrorResponse($"Error occured when adding a payment method: {e}");
        }
    }
    
    private ActionResult FormatErrorResponse(string message) {
        return BadRequest(new RestApiErrorResponse {
            Status = HttpStatusCode.BadRequest,
            Error = message
        });
    }
    
}
