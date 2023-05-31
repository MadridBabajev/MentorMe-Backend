using AutoMapper;
using Base.DAL;
using BLL.DTO.Profiles;
using Public.DTO.v1.Profiles.Secondary;

namespace Public.DTO.Mappers;

public class PaymentMethodDetailedMapper: BaseMapper<BLLPaymentMethodDetailed, PaymentMethodDetailed>
{
    public PaymentMethodDetailedMapper(IMapper mapper) : base(mapper)
    {
    }
}