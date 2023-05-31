using AutoMapper;
using Base.DAL;
using BLL.DTO.Lessons;
using Public.DTO.v1.Lessons;

namespace Public.DTO.Mappers;

public class PaymentMapper: BaseMapper<BLLPayment, Payment>
{
    public PaymentMapper(IMapper mapper) : base(mapper)
    {
    }
}