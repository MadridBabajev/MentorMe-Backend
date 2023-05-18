using AutoMapper;
using Base.DAL;
using BLL.DTO.Subjects;
using DomainSubject = Domain.Entities.Subject;

namespace App.BLL.Mappers;

public class SubjectDetailsMapper: BaseMapper<BLLSubjectDetails, DomainSubject>
{
    public SubjectDetailsMapper(IMapper mapper) : base(mapper)
    {
    }
}