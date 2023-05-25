using AutoMapper;
using Base.DAL;
using BLL.DTO.Profiles;
using Public.DTO.v1.Profiles;

namespace Public.DTO.Mappers;

public class TutorsSearchMapper: BaseMapper<BLLTutorSearch, TutorSearch>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapper"></param>
    public TutorsSearchMapper(IMapper mapper) : base(mapper)
    {
    }
}