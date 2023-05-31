using AutoMapper;
using Base.DAL;
using BLL.DTO.Profiles;
using Public.DTO.v1.Profiles.Secondary;

namespace Public.DTO.Mappers;

public class AvailabilityMapper: BaseMapper<BLLAvailability, Availability>
{
    public AvailabilityMapper(IMapper mapper) : base(mapper)
    {
    }
}