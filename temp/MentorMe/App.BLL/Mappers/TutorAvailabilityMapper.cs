using AutoMapper;
using Base.DAL;
using BLL.DTO.Profiles;
using Domain.Entities;

namespace App.BLL.Mappers;

public class TutorAvailabilityMapper: BaseMapper<BLLTutorAvailability, TutorAvailability>
{
    public TutorAvailabilityMapper(IMapper mapper) : base(mapper)
    {
    }
}