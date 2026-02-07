using AutoMapper;
using Base.DAL;
using BLL.DTO.Profiles;
using DomainTutor = Domain.Entities.Tutor;

namespace App.BLL.Mappers;

public class TutorProfileMapper: BaseMapper<BLLTutorProfile, DomainTutor>
{
    public TutorProfileMapper(IMapper mapper) : base(mapper)
    {
    }
}