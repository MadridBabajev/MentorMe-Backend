using AutoMapper;
using Base.DAL;
using BLL.DTO.Lessons;
using DomainLesson = Domain.Entities.Lesson;

namespace App.BLL.Mappers;

public class LessonDataMapper: BaseMapper<BLLLessonData, DomainLesson>
{
    public LessonDataMapper(IMapper mapper) : base(mapper)
    {
    }
}