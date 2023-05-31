using AutoMapper;
using Base.DAL;
using BLL.DTO.Profiles;
using Public.DTO.v1.Profiles;

namespace Public.DTO.Mappers;

public class EditProfileMapper: BaseMapper<BLLUpdatedProfileData, UpdatedProfileData>
{
    public EditProfileMapper(IMapper mapper) : base(mapper)
    {
    }
}