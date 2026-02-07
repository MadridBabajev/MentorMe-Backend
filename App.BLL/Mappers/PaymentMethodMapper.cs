using AutoMapper;
using Base.DAL;
using BLL.DTO.Profiles;
using Domain.Entities;

namespace App.BLL.Mappers;

public class PaymentMethodMapper: BaseMapper<BLLStudentPaymentMethod, StudentPaymentMethod>
{
    public PaymentMethodMapper(IMapper mapper) : base(mapper)
    {
    }
}