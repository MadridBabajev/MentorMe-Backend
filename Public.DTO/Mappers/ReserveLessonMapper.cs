using AutoMapper;
using Base.DAL;
using BLL.DTO.Lessons;
using Public.DTO.v1.Lessons;

namespace Public.DTO.Mappers;

/// <summary>
/// This class is a mapper for converting between BLLReserveLessonData and ReserveLessonData.
/// It extends from the BaseMapper and leverages the AutoMapper library for the conversion.
/// </summary>
public class ReserveLessonMapper: BaseMapper<BLLReserveLessonData, ReserveLessonData>
{
    /// <summary>
    /// Constructor for the ReserveLessonMapper.
    /// Initializes the base mapper with a provided IMapper instance.
    /// </summary>
    /// <param name="mapper">An instance of AutoMapper's IMapper, used to perform object-object mapping.</param>
    public ReserveLessonMapper(IMapper mapper) : base(mapper)
    {
    }
}