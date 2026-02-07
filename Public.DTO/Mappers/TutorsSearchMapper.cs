using AutoMapper;
using Base.DAL;
using BLL.DTO.Profiles;
using Public.DTO.v1.Profiles;

namespace Public.DTO.Mappers;

/// <summary>
/// This class is a mapper for converting between BLLTutorSearch and TutorSearch types.
/// It extends from the BaseMapper and leverages the AutoMapper library for conversion between types.
/// </summary>
public class TutorsSearchMapper: BaseMapper<BLLTutorSearch, TutorSearch>
{
    /// <summary>
    /// Constructor for the TutorsSearchMapper.
    /// Initializes the base mapper with a provided IMapper instance.
    /// </summary>
    /// <param name="mapper">An instance of AutoMapper's IMapper, used to perform object-object mapping.</param>
    public TutorsSearchMapper(IMapper mapper) : base(mapper)
    {
    }
}