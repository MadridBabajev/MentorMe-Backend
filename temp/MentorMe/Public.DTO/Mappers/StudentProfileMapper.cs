using AutoMapper;
using Base.DAL;
using BLL.DTO.Profiles;
using Public.DTO.v1.Profiles;

namespace Public.DTO.Mappers;

public class StudentProfileMapper: BaseMapper<BLLStudentProfile, StudentProfile>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapper"></param>
    public StudentProfileMapper(IMapper mapper) : base(mapper)
    {
    }
}