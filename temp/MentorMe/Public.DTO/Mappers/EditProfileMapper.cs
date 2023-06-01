using AutoMapper;
using Base.DAL;
using BLL.DTO.Profiles;
using Public.DTO.v1.Profiles;

namespace Public.DTO.Mappers;

/// <summary>
/// This class serves as a mapper for converting between BLLUpdatedProfileData and UpdatedProfileData.
/// It extends from the BaseMapper and uses the AutoMapper library for the conversion.
/// </summary>
public class EditProfileMapper: BaseMapper<BLLUpdatedProfileData, UpdatedProfileData>
{
    /// <summary>
    /// Constructor for the EditProfileMapper.
    /// Initializes the base mapper with a provided IMapper instance.
    /// </summary>
    /// <param name="mapper">An instance of AutoMapper's IMapper, used to perform object-object mapping.</param>
    public EditProfileMapper(IMapper mapper) : base(mapper)
    {
    }
}