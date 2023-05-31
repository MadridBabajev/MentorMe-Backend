using AutoMapper;
using BLL.DTO.Lessons;
using BLL.DTO.Profiles;
using BLL.DTO.Subjects;
using Public.DTO.v1.Lessons;
using Public.DTO.v1.Profiles;
using Public.DTO.v1.Profiles.Secondary;
using Public.DTO.v1.Subjects;

namespace Public.DTO;

/// <summary>
/// Provides AutoMapper configuration for mapping between business logic layer (BLL) and public DTOs.
/// </summary>
public class AutomapperConfig : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AutomapperConfig"/> class.
    /// </summary>
    public AutomapperConfig()
    {
        // Subjects
        CreateMap<BLLSubjectListElement, SubjectListElement>().ReverseMap();
        CreateMap<BLLSubjectDetails, SubjectDetails>().ReverseMap();
        CreateMap<BLLSubjectsFilterElement, SubjectsFilterElement>().ReverseMap();
        
        // Tutors
        CreateMap<BLLTutorSearch, TutorSearch>().ReverseMap();
        CreateMap<BLLTutorProfile, TutorProfile>().ReverseMap();
        CreateMap<BLLAvailability, Availability>().ReverseMap();
        CreateMap<BLLTutorBankingDetails, TutorBankingDetails>().ReverseMap();
        CreateMap<BLLUpdatedProfileData, UpdatedProfileData>().ReverseMap();

        // Students
        CreateMap<BLLStudentProfile, StudentProfile>().ReverseMap();
        CreateMap<BLLStudentPaymentMethod, StudentPaymentMethod>().ReverseMap();
        CreateMap<BLLProfileCardData, ProfileCardData>().ReverseMap();
        CreateMap<BLLPaymentMethodDetailed, PaymentMethodDetailed>().ReverseMap();
        CreateMap<UpdateProfileDataRequest, UpdatedProfileData>()
            .ForMember(dest => dest.ProfilePicture,
                opt => opt.MapFrom(src => Convert.FromBase64String(src.ProfilePicture!)));
        
        // Lessons
        CreateMap<BLLLessonData, LessonData>().ReverseMap();
        CreateMap<BLLTag, Tag>().ReverseMap();
        CreateMap<BLLReserveLessonData, ReserveLessonData>().ReverseMap();
        CreateMap<BLLLessonListElement, LessonListElement>().ReverseMap();
        CreateMap<BLLPayment, Payment>().ReverseMap();
    }
}
