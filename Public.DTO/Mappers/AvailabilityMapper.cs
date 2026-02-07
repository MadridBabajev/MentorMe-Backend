using AutoMapper;
using Base.DAL;
using BLL.DTO.Profiles;
using Public.DTO.v1.Profiles.Secondary;

namespace Public.DTO.Mappers;


/// <summary>
/// This class serves as a mapper for converting between BLLAvailability and Availability.
/// It extends from BaseMapper and uses the AutoMapper library.
/// </summary>
public class AvailabilityMapper: BaseMapper<BLLAvailability, Availability>
{
    /// <summary>
    /// Constructor for the AvailabilityMapper.
    /// Initializes the base mapper with a provided IMapper instance.
    /// </summary>
    /// <param name="mapper">An instance of AutoMapper's IMapper, used to perform object-object mapping.</param>
    public AvailabilityMapper(IMapper mapper) : base(mapper)
    {
    }
}