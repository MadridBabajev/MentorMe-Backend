using AutoMapper;
using BLL.DTO.Lessons;
using BLL.DTO.Profiles;
using BLL.DTO.Subjects;
using Domain.Entities;
using Domain.Enums;

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
                options => options.MapFrom(src =>
                    src.Reviews!.Any() ? src.Reviews!.Average(review => review.Rating) : 0.0)
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
                options => options.MapFrom(src =>
                    src.Reviews!.Any() ? src.Reviews!.Average(review => review.Rating) : 0.0)
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
                options => options.MapFrom(src =>
                    src.Reviews!.Any() ? src.Reviews!.Average(review => review.Rating) : 0.0)
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

        // === Profile card mappings ===

        CreateMap<Student, BLLProfileCardData>()
            .ForMember(
                dest => dest.FullName,
                opt => opt.MapFrom(src => src.AppUser!.FirstName + " " + src.AppUser.LastName))
            .ForMember(
                dest => dest.ProfilePicture,
                opt => opt.MapFrom(src => src.AppUser!.ProfilePicture))
            .ForMember(
                dest => dest.AverageRating,
                options => options.MapFrom(src =>
                    src.Reviews!.Any() ? src.Reviews!.Average(review => review.Rating) : 0.0)
            );

        CreateMap<Tutor, BLLProfileCardData>()
            .ForMember(
                dest => dest.FullName,
                opt => opt.MapFrom(src => src.AppUser!.FirstName + " " + src.AppUser.LastName))
            .ForMember(
                dest => dest.ProfilePicture,
                opt => opt.MapFrom(src => src.AppUser!.ProfilePicture))
            .ForMember(
                dest => dest.AverageRating,
                options => options.MapFrom(src =>
                    src.Reviews!.Any() ? src.Reviews!.Average(review => review.Rating) : 0.0)
            );

        // === Lesson Data mapping ===
        CreateMap<Lesson, BLLLessonData>()
            .ForMember(
                dest => dest.Price,
                opt => opt.MapFrom(src => src.Tutor != null ? src.Tutor.HourlyRate * src.LessonDuration / 60 : 0)
            )
            .ForMember(
                dest => dest.StudentPaymentMethod,
                opt => opt.MapFrom(src => src.Payments != null && src.Payments.Any()
                    ? src.Payments.FirstOrDefault(p =>
                        p.Payment!.StudentPaymentMethod!.StudentId ==
                        src.LessonParticipations!.FirstOrDefault()!.StudentId)!.Payment!.StudentPaymentMethod
                    : null)
            )
            .ForMember(
                dest => dest.UserCanWriteReview,
                opt => opt.MapFrom(src =>
                    src.EndTime <= DateTime.Now &&
                    src.LessonState != ELessonState.Upcoming &&
                    src.LessonParticipations!.FirstOrDefault()!.Student!.Reviews!.All(r => r.LessonId != src.Id) &&
                    src.Tutor!.Reviews!.All(r => r.LessonId != src.Id)
                )
            )
            .ForMember(
                dest => dest.Subject,
                opt => opt.MapFrom(src => src.Subject)
            )
            .ForMember(
                dest => dest.LessonStudent,
                opt => opt.MapFrom(src => src.LessonParticipations!.FirstOrDefault()!.Student)
            )
            .ForMember(
                dest => dest.LessonTutor,
                opt => opt.MapFrom(src => src.Tutor)
            )
            .ForMember(
                dest => dest.Tags,
                opt => opt.MapFrom(src => src.Tags)
            )
            .ForMember(
            dest => dest.PaymentId,
            opt => opt.MapFrom(src => src.Payments!.First().Payment!.Id)
        );

        // === Lessons list element Data mapping ===

        CreateMap<Lesson, BLLLessonListElement>()
            .ForMember(
                dest => dest.TutorFullName,
                opt => opt.MapFrom(src => src.Tutor!.AppUser!.FirstName + src.Tutor.AppUser.LastName)
            )
            .ForMember(
                dest => dest.StudentFullName,
                opt => opt.MapFrom(src =>
                    src.LessonParticipations!.FirstOrDefault()!.Student!.AppUser!.FirstName +
                    src.LessonParticipations!.FirstOrDefault()!.Student!.AppUser!.LastName)
            )
            .ForMember(
                dest => dest.SubjectName,
                opt => opt.MapFrom(src => src.Subject!.Name)
            )
            .ForMember(
                dest => dest.LessonPrice,
                opt => opt.MapFrom(src => src.Tutor != null ? src.Tutor.HourlyRate * src.LessonDuration / 60 : 0)
            );


        // === Subject mappings ===
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

        CreateMap<Subject, BLLSubjectListElement>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.SubjectPicture, opt => opt.MapFrom(src => src.SubjectPicture));

        CreateMap<Subject, BLLSubjectListElement>();
        CreateMap<Subject, BLLSubjectsFilterElement>();
        CreateMap<TutorAvailability, BLLTutorAvailability>();
        CreateMap<StudentPaymentMethod, BLLStudentPaymentMethod>();
        CreateMap<Tag, BLLTag>()
            .ForMember(
                dest => dest.AddedAt,
                opt => opt.MapFrom(src => src.CreatedAt));
    }
}
