using AutoMapper;
using Base.DAL;
using BLL.DTO.Lessons;
using Public.DTO.v1.Lessons;

namespace Public.DTO.Mappers;

/// <summary>
/// This class serves as a mapper for converting between BLLPayment and Payment.
/// It extends from the BaseMapper and uses the AutoMapper library for the conversion.
/// </summary>
public class PaymentMapper: BaseMapper<BLLPayment, Payment>
{
    /// <summary>
    /// Constructor for the PaymentMapper.
    /// Initializes the base mapper with a provided IMapper instance.
    /// </summary>
    /// <param name="mapper">An instance of AutoMapper's IMapper, used to perform object-object mapping.</param>
    public PaymentMapper(IMapper mapper) : base(mapper)
    {
    }
}