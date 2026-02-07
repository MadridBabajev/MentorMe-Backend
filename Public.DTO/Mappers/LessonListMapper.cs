using AutoMapper;
using Base.DAL;
using BLL.DTO.Lessons;
using Public.DTO.v1.Lessons;

namespace Public.DTO.Mappers;

/// <summary>
/// This class serves as a mapper for converting between BLLLessonListElement and LessonListElement.
/// It extends from the BaseMapper and uses the AutoMapper library for the conversion.
/// </summary>
public class LessonListMapper: BaseMapper<BLLLessonListElement, LessonListElement>
{
    /// <summary>
    /// Constructor for the LessonListMapper.
    /// Initializes the base mapper with a provided IMapper instance.
    /// </summary>
    /// <param name="mapper">An instance of AutoMapper's IMapper, used to perform object-object mapping.</param>
    public LessonListMapper(IMapper mapper) : base(mapper)
    {
    }
}