using AutoMapper;
using Base.DAL;
using BLL.DTO;
using BLL.DTO.Subjects;
using Public.DTO.v1;
using Public.DTO.v1.Subjects;

namespace Public.DTO.Mappers;

/// <summary>
/// This class is a mapper for converting between BLLSubjectListElement and SubjectListElement types.
/// It extends from the BaseMapper and leverages the AutoMapper library for conversion between types.
/// </summary>
public class SubjectsMapper: BaseMapper<BLLSubjectListElement, SubjectListElement>
{
    /// <summary>
    /// Constructor for the SubjectsMapper.
    /// Initializes the base mapper with a provided IMapper instance.
    /// </summary>
    /// <param name="mapper">An instance of AutoMapper's IMapper, used to perform object-object mapping.</param>
    public SubjectsMapper(IMapper mapper) : base(mapper)
    {
    }
    
    /// <summary>
    /// Maps a BLLSubjectListElement instance to a SubjectListElement instance.
    /// </summary>
    /// <param name="bllSubjectListElement">The BLLSubjectListElement instance to be mapped.</param>
    /// <returns>Returns the mapped SubjectListElement instance.</returns>
    public SubjectListElement MapListSubject(BLLSubjectListElement bllSubjectListElement)
    {
        return Mapper.Map<SubjectListElement>(bllSubjectListElement);
    }
}
