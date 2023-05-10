using AutoMapper;
using Base.DAL;
using BLL.DTO;
using DomainTutor = Domain.Entities.Tutor;

namespace App.BLL.Mappers;

public class TutorMapper: BaseMapper<BLLTutorSearch, DomainTutor>
{
    public TutorMapper(IMapper mapper) : base(mapper)
    {
    }
}