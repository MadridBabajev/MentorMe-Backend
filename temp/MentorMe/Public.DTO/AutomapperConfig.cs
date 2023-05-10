using AutoMapper;
using BLL.DTO;

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
        // CreateMap<BLLTutorSearch, Public.DTO.v1.TutorSearch>()
        //     .ForMember(
        //         dest => dest.EventCount,
        //         options =>
        //             options.MapFrom(src => src.PlanEvents!.Count)
        //     );
        
        // CreateMap<DAL.DTO.TrainingPlanWithEventCount, Public.DTO.v1.TrainingPlan>();
        CreateMap<BLLTutorSearch, v1.TutorSearch>().ReverseMap();
        CreateMap<BLLSubjectListElement, BLLSubjectListElement>().ReverseMap();
    }
}
