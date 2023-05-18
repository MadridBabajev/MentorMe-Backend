using AutoMapper;
using Base.DAL;
using BLL.DTO.Subjects;
using DomainSubject = Domain.Entities.Subject;

namespace App.BLL.Mappers;

public class SubjectsListMapper: BaseMapper<BLLSubjectListElement, DomainSubject>
{
    // If you need to map the BLLSubjectDetails, create another mapper for it
    public SubjectsListMapper(IMapper mapper) : base(mapper)
    {
    }
}
