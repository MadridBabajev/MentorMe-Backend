using AutoMapper;
using Base.DAL;
using BLL.DTO.Profiles;
using Domain.Entities;

namespace App.BLL.Mappers;

public class AvailabilityMapper: BaseMapper<BLLAvailability, TutorAvailability>
{
    public AvailabilityMapper(IMapper mapper) : base(mapper)
    {
    }
}