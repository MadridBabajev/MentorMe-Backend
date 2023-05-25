using AutoMapper;
using Base.DAL;
using BLL.DTO.Lessons;
using Public.DTO.v1.Lessons;

namespace Public.DTO.Mappers;

public class LessonListMapper: BaseMapper<BLLLessonListElement, LessonListElement>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapper"></param>
    public LessonListMapper(IMapper mapper) : base(mapper)
    {
    }
}