using AutoMapper;
using Base.DAL;
using BLL.DTO.Profiles;
using Public.DTO.v1.Profiles;

namespace Public.DTO.Mappers;

public class TutorProfileMapper: BaseMapper<BLLTutorProfile, TutorProfile>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapper"></param>
    public TutorProfileMapper(IMapper mapper) : base(mapper)
    {
    }
}