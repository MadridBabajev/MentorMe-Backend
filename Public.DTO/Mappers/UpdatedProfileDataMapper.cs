using AutoMapper;
using Base.DAL;
using Public.DTO.v1.Profiles;

namespace Public.DTO.Mappers;

/// <summary>
/// This class is a mapper for converting between UpdateProfileDataRequest and UpdatedProfileData types.
/// It extends from the BaseMapper and uses the AutoMapper library to perform conversion between types.
/// </summary>
public class UpdatedProfileDataMapper: BaseMapper<UpdateProfileDataRequest, UpdatedProfileData>
{
    /// <summary>
    /// Constructor for the UpdatedProfileDataMapper.
    /// Initializes the base mapper with the provided AutoMapper's IMapper instance.
    /// </summary>
    /// <param name="mapper">An instance of IMapper from AutoMapper, used for object-object mapping.</param>
    public UpdatedProfileDataMapper(IMapper mapper) : base(mapper)
    {
    }
}