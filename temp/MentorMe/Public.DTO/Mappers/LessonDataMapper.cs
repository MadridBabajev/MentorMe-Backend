using AutoMapper;
using Base.DAL;
using BLL.DTO.Lessons;
using Public.DTO.v1.Lessons;

namespace Public.DTO.Mappers;

public class LessonDataMapper: BaseMapper<BLLLessonData, LessonData>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapper"></param>
    public LessonDataMapper(IMapper mapper) : base(mapper)
    {
    }
}