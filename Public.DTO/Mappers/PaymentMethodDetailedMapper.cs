using AutoMapper;
using Base.DAL;
using BLL.DTO.Profiles;
using Public.DTO.v1.Profiles.Secondary;

namespace Public.DTO.Mappers;

/// <summary>
/// This class serves as a mapper for converting between BLLPaymentMethodDetailed and PaymentMethodDetailed.
/// It extends from the BaseMapper and uses the AutoMapper library for the conversion.
/// </summary>
public class PaymentMethodDetailedMapper: BaseMapper<BLLPaymentMethodDetailed, PaymentMethodDetailed>
{
    /// <summary>
    /// Constructor for the PaymentMethodDetailedMapper.
    /// Initializes the base mapper with a provided IMapper instance.
    /// </summary>
    /// <param name="mapper">An instance of AutoMapper's IMapper, used to perform object-object mapping.</param>
    public PaymentMethodDetailedMapper(IMapper mapper) : base(mapper)
    {
    }
}