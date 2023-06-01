using AutoMapper;
using Base.DAL;
using BLL.DTO;
using BLL.DTO.Subjects;
using Public.DTO.v1;
using Public.DTO.v1.Subjects;

namespace Public.DTO.Mappers;


/// <summary>
/// This class is a mapper for converting between BLLSubjectDetails and SubjectListElement.
/// It extends from the BaseMapper and leverages the AutoMapper library for the conversion.
/// It also provides a method for mapping to a SubjectDetails object.
/// </summary>
public class SubjectDetailsMapper: BaseMapper<BLLSubjectDetails, SubjectListElement>
{
    /// <summary>
    /// Constructor for the SubjectDetailsMapper.
    /// Initializes the base mapper with a provided IMapper instance.
    /// </summary>
    /// <param name="mapper">An instance of AutoMapper's IMapper, used to perform object-object mapping.</param>
    public SubjectDetailsMapper(IMapper mapper) : base(mapper)
    {
    }

    /// <summary>
    /// Maps a BLLSubjectDetails object to a SubjectDetails object.
    /// </summary>
    /// <param name="bllSubjectDetails">The BLLSubjectDetails object to map from.</param>
    /// <returns>The mapped SubjectDetails object.</returns>
    public SubjectDetails MapDetailsSubject(BLLSubjectDetails bllSubjectDetails)
    {
        return Mapper.Map<SubjectDetails>(bllSubjectDetails);
    }
}