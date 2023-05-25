using AutoMapper;
using Base.DAL;
using BLL.DTO.Subjects;
using Public.DTO.v1.Subjects;

namespace Public.DTO.Mappers;

public class SubjectsFilterMapper: BaseMapper<BLLSubjectsFilterElement, SubjectsFilterElement>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapper"></param>
    public SubjectsFilterMapper(IMapper mapper) : base(mapper)
    {
    }
}