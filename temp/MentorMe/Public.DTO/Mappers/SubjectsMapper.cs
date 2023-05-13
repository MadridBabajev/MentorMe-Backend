using AutoMapper;
using Base.DAL;
using BLL.DTO;
using Public.DTO.v1;

namespace Public.DTO.Mappers;
// TODO: Finish off the documentation
/// <summary>
/// 
/// </summary>
public class SubjectsMapper: BaseMapper<BLLSubjectListElement, SubjectListElement>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapper"></param>
    public SubjectsMapper(IMapper mapper) : base(mapper)
    {
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bllSubjectListElement"></param>
    /// <returns></returns>
    public SubjectListElement MapListSubject(BLLSubjectListElement bllSubjectListElement)
    {
        return Mapper.Map<SubjectListElement>(bllSubjectListElement);
    }
}
