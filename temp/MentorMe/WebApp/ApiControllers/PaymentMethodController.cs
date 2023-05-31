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

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public class PaymentMethodController: ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly PaymentMethodDetailedMapper _paymentMethodDetailedMapper;

    public PaymentMethodController(IAppBLL bll, IMapper autoMapper)
    {
        _bll = bll;
        _paymentMethodDetailedMapper = new PaymentMethodDetailedMapper(autoMapper);
    }
    
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
    
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> RemovePaymentMethod([FromBody] RemovePaymentMethod paymentMethod)
    {
        try
        {
            await _bll.PaymentMethodService.DeletePaymentMethod(paymentMethod.PaymentMethodId);
            return Ok();
        }
        catch (Exception e)
        {
            return FormatErrorResponse($"Error occured when removing a payment method: {e}");
        }
    }
    
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
