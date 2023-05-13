using AutoMapper;
using BLL.DTO;
using Domain.Entities;
using DomainTutor = Domain.Entities.Tutor;

namespace App.BLL;

public class AutoMapperConfig: Profile
{
    public AutoMapperConfig()
    {
        CreateMap<BLLTutorSearch, DomainTutor>().ReverseMap();
        CreateMap<Subject, BLLSubjectDetails>().ForMember(
            dest => dest.TaughtBy,
            options =>
                options.MapFrom(src => src.TutorSubjects!.Count)
        ).ForMember(
            dest => dest.LearnedBy,
            options =>
                options.MapFrom(src => src.StudentSubjects!.Count)
        );

    }
}