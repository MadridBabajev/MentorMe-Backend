using AutoMapper;
using Base.DAL;
using BLL.DTO.Lessons;
using DomainLesson = Domain.Entities.Lesson;

namespace App.BLL.Mappers;

public class LessonListMapper: BaseMapper<BLLLessonListElement, DomainLesson>
{
    public LessonListMapper(IMapper mapper) : base(mapper)
    {
    }
}