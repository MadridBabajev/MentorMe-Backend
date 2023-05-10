using AutoMapper;
using Base.DAL;
using BLL.DTO;
using DomainSubject = Domain.Entities.Subject;

namespace App.BLL.Mappers;

public class SubjectsMapper: BaseMapper<BLLSubjectListElement, DomainSubject>
{
    // If you need to map the BLLSubjectDetails, create another mapper for it
    public SubjectsMapper(IMapper mapper) : base(mapper)
    {
    }
}
