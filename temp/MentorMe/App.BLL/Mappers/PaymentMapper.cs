using AutoMapper;
using Base.DAL;
using BLL.DTO.Lessons;
using Domain.Entities;

namespace App.BLL.Mappers;

public class PaymentMapper: BaseMapper<BLLPayment, Payment>
{
    public PaymentMapper(IMapper mapper) : base(mapper)
    {
    }
}