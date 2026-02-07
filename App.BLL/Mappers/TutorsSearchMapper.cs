using AutoMapper;
using Base.DAL;
using BLL.DTO.Profiles;
using DomainTutor = Domain.Entities.Tutor;

namespace App.BLL.Mappers;

public class TutorsSearchMapper: BaseMapper<BLLTutorSearch, DomainTutor>
{
    public TutorsSearchMapper(IMapper mapper) : base(mapper)
    {
    }
}