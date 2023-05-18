using AutoMapper;
using BLL.DTO.Profiles;
using BLL.DTO.Subjects;
using Domain.Entities;

namespace App.BLL;

public class AutoMapperConfig: Profile
{
    public AutoMapperConfig()
    {
        // === Tutor Search mapping ===
        CreateMap<Tutor, BLLTutorSearch>()
            .ForMember(
                dest => dest.FirstName,
                options => options.MapFrom(src => src.AppUser!.FirstName)
            )
            .ForMember(
                dest => dest.LastName,
                options => options.MapFrom(src => src.AppUser!.LastName)
            )
            .ForMember(
                dest => dest.Title,
                options => options.MapFrom(src => src.AppUser!.Title)
            )
            .ForMember(
                dest => dest.AverageRating,
                options => options.MapFrom(src => src.Reviews!.Any() ? src.Reviews!.Average(review => review.Rating) : 0.0)
            )
            .ForMember(
                dest => dest.ClassesTutored,
                options => options.MapFrom(src => src.Lessons!.Count)
            )
            .ForMember(
                dest => dest.ProfilePicture,
                options => options.MapFrom(src => src.AppUser!.ProfilePicture)
            );
        
        // === Tutor Profile mapping ===
        CreateMap<Tutor, BLLTutorProfile>()
            .ForMember(
                dest => dest.NotificationsEnabled,
                options => options.MapFrom(src => src.AppUser!.NotificationsEnabled)
            )
            .ForMember(
                dest => dest.Availabilities, 
                options => options.MapFrom(src => src.Availabilities))
            .ForMember(
                dest => dest.FirstName,
                options => options.MapFrom(src => src.AppUser!.FirstName)
            )
            .ForMember(
                dest => dest.LastName,
                options => options.MapFrom(src => src.AppUser!.LastName)
            )
            .ForMember(
                dest => dest.MobilePhone,
                options => options.MapFrom(src => src.AppUser!.MobilePhone)
            )
            .ForMember(
                dest => dest.Balance,
                options => options.MapFrom(src => src.AppUser!.Balance)
            )
            .ForMember(
                dest => dest.AverageRating,
                options => options.MapFrom(src => src.Reviews!.Any() ? src.Reviews!.Average(review => review.Rating) : 0.0)
            )
            .ForMember(
                dest => dest.Title,
                options => options.MapFrom(src => src.AppUser!.Title)
            )
            .ForMember(
                dest => dest.Bio,
                options => options.MapFrom(src => src.AppUser!.Bio)
            )
            .ForMember(
                dest => dest.ProfilePicture,
                options => options.MapFrom(src => src.AppUser!.ProfilePicture)
            )
            .ForMember(
                dest => dest.Subjects,
                options => options.MapFrom(src => src.TutorSubjects!.Select(ss => ss.Subject))
            )
            .ForMember(
                dest => dest.NumberOfClasses,
                options => options.MapFrom(src => src.Lessons!.Count)
            );

        // === Student Profile mapping ===
        CreateMap<Student, BLLStudentProfile>()
            .ForMember(
                dest => dest.NotificationsEnabled,
                options => options.MapFrom(src => src.AppUser!.NotificationsEnabled)
            )
            .ForMember(
                dest => dest.FirstName,
                options => options.MapFrom(src => src.AppUser!.FirstName)
            )
            .ForMember(
                dest => dest.LastName,
                options => options.MapFrom(src => src.AppUser!.LastName)
            )
            .ForMember(
                dest => dest.MobilePhone,
                options => options.MapFrom(src => src.AppUser!.MobilePhone)
            )
            .ForMember(
                dest => dest.Balance,
                options => options.MapFrom(src => src.AppUser!.Balance)
            )
            .ForMember(
                dest => dest.AverageRating,
                options => options.MapFrom(src => src.Reviews!.Any() ? src.Reviews!.Average(review => review.Rating) : 0.0)
            )
            .ForMember(
                dest => dest.Title,
                options => options.MapFrom(src => src.AppUser!.Title)
            )
            .ForMember(
                dest => dest.Bio,
                options => options.MapFrom(src => src.AppUser!.Bio)
            )
            .ForMember(
                dest => dest.ProfilePicture,
                options => options.MapFrom(src => src.AppUser!.ProfilePicture)
            )
            .ForMember(
                dest => dest.Subjects,
                options => options.MapFrom(src => src.StudentSubjects!.Select(ss => ss.Subject))
            )
            .ForMember(
                dest => dest.NumberOfClasses,
                options => options.MapFrom(src => src.Lessons!.Count)
            );
        
        // === Subject Details mapping ===
        CreateMap<Subject, BLLSubjectDetails>().ForMember(
            dest => dest.TaughtBy,
            options =>
                options.MapFrom(src => src.TutorSubjects!.Count)
        ).ForMember(
            dest => dest.LearnedBy,
            options =>
                options.MapFrom(src => src.StudentSubjects!.Count)
        );
        
        CreateMap<StudentSubject, BLLSubjectListElement>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Subject!.Name))
            .ForMember(dest => dest.SubjectPicture, opt => opt.MapFrom(src => src.Subject!.SubjectPicture));
        
        CreateMap<TutorSubject, BLLSubjectListElement>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Subject!.Name))
            .ForMember(dest => dest.SubjectPicture, opt => opt.MapFrom(src => src.Subject!.SubjectPicture));

        CreateMap<Subject, BLLSubjectListElement>();
        CreateMap<Subject, BLLSubjectsFilterElement>();
        CreateMap<TutorAvailability, BLLTutorAvailability>();
    }
}