using AutoMapper;
using Base.DAL;
using BLL.DTO.Subjects;
using DomainSubject = Domain.Entities.Subject;

namespace App.BLL.Mappers;

public class SubjectsFilterMapper: BaseMapper<BLLSubjectsFilterElement, DomainSubject>
{
    public SubjectsFilterMapper(IMapper mapper) : base(mapper)
    {
    }
}