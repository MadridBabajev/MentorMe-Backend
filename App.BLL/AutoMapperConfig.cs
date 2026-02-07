using AutoMapper;
using BLL.DTO.Lessons;
using BLL.DTO.Profiles;
using BLL.DTO.Subjects;
using Domain.Entities;
using Domain.Enums;
using Public.DTO.v1.Profiles;

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
        
        // === Student edit data mapping ===
        CreateMap<Student, BLLUpdatedProfileData>()
            .ForMember(dest => dest.FirstName,
                opt => opt.MapFrom(src => src.AppUser!.FirstName))
            .ForMember(dest => dest.LastName, 
                opt => opt.MapFrom(src => src.AppUser!.LastName))
            .ForMember(dest => dest.MobilePhone, 
                opt => opt.MapFrom(src => src.AppUser!.MobilePhone))
            .ForMember(dest => dest.Title, 
                opt => opt.MapFrom(src => src.AppUser!.Title))
            .ForMember(dest => dest.Bio,
                opt => opt.MapFrom(src => src.AppUser!.Bio))
            .ForMember(dest => dest.ProfilePicture,
                opt => opt.MapFrom(src => src.AppUser!.ProfilePicture))
            .ForMember(dest => dest.UserType,
                opt => opt.MapFrom(src => src.AppUser!.AppUserType.ToString()))
            .ForMember(dest => dest.HourlyRate, 
                opt => opt.MapFrom(src => (double?)null));

        // === Tutor edit data mapping ===
        CreateMap<Tutor, BLLUpdatedProfileData>()
            .ForMember(dest => dest.FirstName,
                opt => opt.MapFrom(src => src.AppUser!.FirstName))
            .ForMember(dest => dest.LastName, 
                opt => opt.MapFrom(src => src.AppUser!.LastName))
            .ForMember(dest => dest.MobilePhone, 
                opt => opt.MapFrom(src => src.AppUser!.MobilePhone))
            .ForMember(dest => dest.Title, 
                opt => opt.MapFrom(src => src.AppUser!.Title))
            .ForMember(dest => dest.Bio, 
                opt => opt.MapFrom(src => src.AppUser!.Bio))
            .ForMember(dest => dest.ProfilePicture, 
                opt => opt.MapFrom(src => src.AppUser!.ProfilePicture))
            .ForMember(dest => dest.UserType, 
                opt => opt.MapFrom(src => src.AppUser!.AppUserType.ToString()))
            .ForMember(dest => dest.HourlyRate, 
                opt => opt.MapFrom(src => src.HourlyRate));

        // === Lessons list element data mapping ===
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

        // === Payment mapping ===
        CreateMap<Payment, BLLPayment>()
            .AfterMap((src, dest, context) =>
            {
                var lessonPayment = src.LessonPayments?.FirstOrDefault();
                if (lessonPayment != null)
                {
                    // Retrieve Student who is the sender
                    var student = context.Mapper.Map<Student>(lessonPayment.Student);
                    if (student != null && student.AppUser != null)
                    {
                        dest.SenderStudentFullName = $"{student.AppUser.FirstName} {student.AppUser.LastName}";
                    }

                    // Retrieve Tutor who is the recipient
                    var tutor = context.Mapper.Map<Tutor>(lessonPayment.Tutor);
                    if (tutor != null && tutor.AppUser != null)
                    {
                        dest.RecipientTutorFullName = $"{tutor.AppUser.FirstName} {tutor.AppUser.LastName}";
                    }
                }
            })
            .ForMember(dest => dest.PaymentMethodType, 
                opt => opt.MapFrom(src => src.StudentPaymentMethod!.PaymentMethodType));
        
        // === Student Payment Methods mapping ===
        CreateMap<StudentPaymentMethod, BLLPaymentMethodDetailed>()
            .AfterMap((src, dest, context) =>
            {
                // Retrieve Student who is the owner of the payment method
                var student = context.Mapper.Map<Student>(src.Student);
                if (student != null && student.AppUser != null)
                {
                    dest.OwnerFullName = $"{student.AppUser.FirstName} {student.AppUser.LastName}";
                    dest.OwnerCountry = student.AppUser.Country;
                }
            })
            .ForMember(dest => dest.Details, 
                opt => opt.MapFrom(src => src.Details ?? "N/A"))
            .ForMember(dest => dest.CardCvv, 
                opt => opt.MapFrom(src => src.CardCvv ?? "N/A"))
            .ForMember(dest => dest.CardExpirationDate, 
                opt => opt.MapFrom(src => src.CardExpirationDate ?? "N/A"))
            .ForMember(dest => dest.CardNumber, 
                opt => opt.MapFrom(src => src.CardNumber ?? "N/A"));
        
        // === Additional mappings ===
        CreateMap<TutorAvailability, BLLAvailability>();
        CreateMap<StudentPaymentMethod, BLLStudentPaymentMethod>();
        CreateMap<TutorBankingDetails, BLLTutorBankingDetails>();
        CreateMap<Tag, BLLTag>()
            .ForMember(
                dest => dest.AddedAt,
                opt => opt.MapFrom(src => src.CreatedAt));
    }
}
