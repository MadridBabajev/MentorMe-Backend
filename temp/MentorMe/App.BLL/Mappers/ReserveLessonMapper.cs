using AutoMapper;
using Base.DAL;
using BLL.DTO.Lessons;
using DomainTutor = Domain.Entities.Tutor;

namespace App.BLL.Mappers;

public class ReserveLessonMapper: BaseMapper<BLLReserveLessonData, DomainTutor>
{
    public ReserveLessonMapper(IMapper mapper) : base(mapper)
    {
    }
}