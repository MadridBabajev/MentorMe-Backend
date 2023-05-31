using AutoMapper;
using Base.DAL;
using BLL.DTO.Profiles;
using Domain.Entities;

namespace App.BLL.Mappers;

public class PaymentMethodDetailedMapper: BaseMapper<BLLPaymentMethodDetailed, StudentPaymentMethod>
{
    public PaymentMethodDetailedMapper(IMapper mapper) : base(mapper)
    {
    }
}