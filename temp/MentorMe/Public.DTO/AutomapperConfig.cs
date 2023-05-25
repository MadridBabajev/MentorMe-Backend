using AutoMapper;
using BLL.DTO;
using BLL.DTO.Lessons;
using BLL.DTO.Profiles;
using BLL.DTO.Subjects;
using Public.DTO.v1;
using Public.DTO.v1.Lessons;
using Public.DTO.v1.Profiles;
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
        CreateMap<BLLSubjectListElement, SubjectListElement>().ReverseMap();
        CreateMap<BLLSubjectDetails, SubjectDetails>().ReverseMap();
        CreateMap<BLLSubjectsFilterElement, SubjectsFilterElement>().ReverseMap();
        CreateMap<BLLTutorSearch, TutorSearch>().ReverseMap();
        CreateMap<BLLTutorProfile, TutorProfile>().ReverseMap();
        CreateMap<BLLStudentProfile, StudentProfile>().ReverseMap();
        CreateMap<BLLTutorAvailability, TutorAvailability>().ReverseMap();
        CreateMap<BLLStudentPaymentMethod, StudentPaymentMethod>().ReverseMap();
        CreateMap<BLLLessonData, LessonData>().ReverseMap();
        CreateMap<BLLProfileCardData, ProfileCardData>().ReverseMap();
        CreateMap<BLLTag, Tag>().ReverseMap();
        CreateMap<BLLReserveLessonData, ReserveLessonData>().ReverseMap();
        CreateMap<BLLLessonListElement, LessonListElement>().ReverseMap();
    }
}
