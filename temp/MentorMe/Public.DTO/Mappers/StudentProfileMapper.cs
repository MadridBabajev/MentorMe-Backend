using AutoMapper;
using Base.DAL;
using BLL.DTO.Profiles;
using Public.DTO.v1.Profiles;

namespace Public.DTO.Mappers;

/// <summary>
/// This class is a mapper for converting between BLLStudentProfile and StudentProfile.
/// It extends from the BaseMapper and leverages the AutoMapper library for the conversion.
/// </summary>
public class StudentProfileMapper: BaseMapper<BLLStudentProfile, StudentProfile>
{
    /// <summary>
    /// Constructor for the StudentProfileMapper.
    /// Initializes the base mapper with a provided IMapper instance.
    /// </summary>
    /// <param name="mapper">An instance of AutoMapper's IMapper, used to perform object-object mapping.</param>
    public StudentProfileMapper(IMapper mapper) : base(mapper)
    {
    }
}