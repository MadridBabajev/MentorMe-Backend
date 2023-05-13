using AutoMapper;
using Base.DAL;
using BLL.DTO;
using Public.DTO.v1;

namespace Public.DTO.Mappers;
// TODO: Finish off the documentation
/// <summary>
/// 
/// </summary>
public class SubjectDetailsMapper: BaseMapper<BLLSubjectDetails, SubjectListElement>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapper"></param>
    public SubjectDetailsMapper(IMapper mapper) : base(mapper)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bllSubjectDetails"></param>
    /// <returns></returns>
    public SubjectDetails MapDetailsSubject(BLLSubjectDetails bllSubjectDetails)
    {
        return Mapper.Map<SubjectDetails>(bllSubjectDetails);
    }
}