using AutoMapper;
using Base.DAL;
using BLL.DTO.Lessons;
using Public.DTO.v1.Lessons;

namespace Public.DTO.Mappers;

public class ReserveLessonMapper: BaseMapper<BLLReserveLessonData, ReserveLessonData>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapper"></param>
    public ReserveLessonMapper(IMapper mapper) : base(mapper)
    {
    }
}