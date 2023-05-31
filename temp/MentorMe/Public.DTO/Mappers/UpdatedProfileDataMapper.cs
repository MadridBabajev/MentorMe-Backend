using AutoMapper;
using Base.DAL;
using Public.DTO.v1.Profiles;

namespace Public.DTO.Mappers;

public class UpdatedProfileDataMapper: BaseMapper<UpdateProfileDataRequest, UpdatedProfileData>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapper"></param>
    public UpdatedProfileDataMapper(IMapper mapper) : base(mapper)
    {
    }
}