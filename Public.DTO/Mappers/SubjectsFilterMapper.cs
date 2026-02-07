using AutoMapper;
using Base.DAL;
using BLL.DTO.Subjects;
using Public.DTO.v1.Subjects;

namespace Public.DTO.Mappers;

/// <summary>
/// This class is a mapper for converting between BLLSubjectsFilterElement and SubjectsFilterElement types.
/// It extends from the BaseMapper and leverages the AutoMapper library for conversion between types.
/// </summary>
public class SubjectsFilterMapper: BaseMapper<BLLSubjectsFilterElement, SubjectsFilterElement>
{
    /// <summary>
    /// Constructor for the SubjectsFilterMapper.
    /// Initializes the base mapper with a provided IMapper instance.
    /// </summary>
    /// <param name="mapper">An instance of AutoMapper's IMapper, used to perform object-object mapping.</param>
    public SubjectsFilterMapper(IMapper mapper) : base(mapper)
    {
    }
}